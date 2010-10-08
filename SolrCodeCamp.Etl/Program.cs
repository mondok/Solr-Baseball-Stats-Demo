using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Text;
using FileHelpers;
using Microsoft.Practices.ServiceLocation;
using SolrCodeCamp.Shared;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace SolrCodeCamp.Etl
{
    // data source 
    // http://www.retrosheet.org/gamelogs/index.html

    class Program
    {
        private static ISolrOperations<BaseballGame> _solr;

        static void Main(string[] args)
        {
            Startup.Init<BaseballGame>(Global.SOLR_LOCATION);
            _solr = ServiceLocator.Current.GetInstance<ISolrOperations<BaseballGame>>();
            LoadGamesIntoIndex();
            RunExamples();
        }

        static void RunExamples()
        {
            QueryExamples examples = new QueryExamples(_solr);

            examples.RunFacetExample();

            examples.RunRangeSearchExample(2008, 2010);

            examples.RunTextSearchExample("Halladay");
        }

        static void LoadGamesIntoIndex()
        {
            GameLoader loader = new GameLoader();
            var games = loader.GetGames();
            _solr.Delete(SolrQuery.All);

            int offset = 0;
            int numToTake = 1000;
            while(true)
            {
                var gameList = games.Skip(offset).Take(numToTake);
                offset += numToTake;
                if (gameList.Any())
                {
                    _solr.Add(gameList);
                    _solr.Commit();
                }
                else
                {
                    break;
                }
            }
            _solr.Commit();
            _solr.Optimize();
        }
    }
}
