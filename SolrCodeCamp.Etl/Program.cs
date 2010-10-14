using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using SolrCodeCamp.Shared;
using SolrNet;

namespace SolrCodeCamp.Etl
{
    // data source 
    // http://www.retrosheet.org/gamelogs/index.html

    // Solr admin 
    // http://localhost:9091/solr/admin/

    class Program
    {
        private static ISolrOperations<BaseballGame> _solrOperations;

        // set to true in order to load the index
        public const bool SHOULD_LOAD_GAMES = false;

        static void Main(string[] args)
        {
            Startup.Init<BaseballGame>(Global.SOLR_LOCATION);
            _solrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<BaseballGame>>();

            if (SHOULD_LOAD_GAMES)
                LoadGamesIntoIndex();
            else Console.WriteLine("You have indexing set to OFF - just a friendly reminder");
           
            RunExamples();
            Console.WriteLine("Press any key to quit");
            Console.ReadKey();
        }

        static void RunExamples()
        {
            QueryExamples examples = new QueryExamples(_solrOperations);

            examples.RunFacetExample();

            examples.RunRangeSearchExample(2007, 2010);

            examples.RunTextSearchExample("Halladay");
        }

        static void LoadGamesIntoIndex()
        {
            ////  This code adds a boost value to any years beyond whatever is set in the boostYearsAfter field
            ////  The boostValue field is the amount of boost to give the documents.  This value should be tweaked to 
            ////  find a sweet spot.
            
            int boostYearsAfter = 1999;
            double boostValue = 0.5;

            GameLoader loader = new GameLoader();
            List<BaseballGame> games = loader.GetGames();
            _solrOperations.Delete(SolrQuery.All);

            IEnumerable<int> yearsAvailable = games.Select(y => y.Year).Distinct().OrderBy(y => y);

            foreach (int year in yearsAvailable)
            {
                IEnumerable<BaseballGame> gameList = games.Where(y => y.Year == year);
                double boost = 0;
                if (year > boostYearsAfter)
                {
                    boost = boostValue;
                }
                _solrOperations.AddWithBoost(gameList.Select(g => new KeyValuePair<BaseballGame, double?>(g, boost)));
                _solrOperations.Commit();
            }

            //// The code below would perform chunk inserts without adding
            //// a boost value to any fields or documents

            //int offset = 0;
            //int numToTake = 1000;
            //while (true)
            //{
            //    IEnumerable<BaseballGame> gameList = games.Skip(offset).Take(numToTake);
            //    offset += numToTake;
            //    if (gameList.Any())
            //    {
            //        _solrOperations.Add(gameList);
            //        _solrOperations.Commit();
            //    }
            //    else
            //    {
            //        break;
            //    }
            //}

            _solrOperations.Optimize();
        }
    }
}
