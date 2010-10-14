using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrCodeCamp.Shared;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace SolrCodeCamp.Etl
{
    public class QueryExamples
    {
        private ISolrOperations<BaseballGame> _solrOperations;

        public QueryExamples(ISolrOperations<BaseballGame> solrOperationsOperations)
        {
            _solrOperations = solrOperationsOperations;
        }

        public void RunFacetExample()
        {
            Console.WriteLine("-- RunFacetExample --");

            FacetParameters facetParameters = new FacetParameters();
            QueryOptions queryOptions = new QueryOptions();

            SolrFacetFieldQuery umpireFacets = new SolrFacetFieldQuery("bg_homePlateUmpire");
            umpireFacets.Limit = 50;
            umpireFacets.Sort = true;

            // this uses the DateMathParser syntax - i.e. +1DAY, +1MONTH, DAY+6MONTHS+3DAYS
            SolrFacetDateQuery dateFacets = new SolrFacetDateQuery("bg_date", new DateTime(2005, 1, 1), DateTime.Now, "+1MONTH");

            queryOptions.AddFacets(umpireFacets, dateFacets);

            // we just want the facets, not data
            queryOptions.Rows = 0;

            ISolrQueryResults<BaseballGame> results = _solrOperations.Query(SolrQuery.All, queryOptions);

            DateFacetingResult dateFacetResults = results.FacetDates["bg_date"];

            var umpireFacetResults = results.FacetFields["bg_homePlateUmpire"];

            Console.WriteLine("Date Facets");
            foreach (KeyValuePair<DateTime,int> dateFacet in dateFacetResults.DateResults)
            {
                Console.WriteLine(string.Format("Date: {0}, Games: {1}", dateFacet.Key, dateFacet.Value));
            }

            Console.WriteLine("Umpire Facets");
            foreach(KeyValuePair<string,int> umpireFacet in umpireFacetResults)
            {
                Console.WriteLine(string.Format("Umpire Name: {0}, Games Called: {1}", umpireFacet.Key, umpireFacet.Value));
            }

        }

        public void RunTextSearchExample(string searchTerm)
        {
            Console.WriteLine("-- RunTextSearchExample --");

            ISolrQueryResults<BaseballGame> results = _solrOperations.Query(new SolrQuery(searchTerm));

            List<BaseballGame> gameResults = results.ToList();

            foreach(BaseballGame baseballGame in gameResults)
            {
                Console.WriteLine(string.Format("Home: {0}, Visitor: {1}, Winning Pitcher: {2}",
                    baseballGame.HomeTeam, baseballGame.VisitingTeam, baseballGame.WinningPitcher));
            }
        }

        public void RunRangeSearchExample(int startYear, int endYear)
        {
            Console.WriteLine("-- RunRangeSearchExample --");

            ISolrQueryResults<BaseballGame> results =
                _solrOperations.Query(new SolrQueryByRange<int>("bg_year", startYear, endYear));

            List<BaseballGame> gameResults = results.ToList();

            IEnumerable<IGrouping<string, BaseballGame>> winningPitchers = gameResults.GroupBy(t => t.WinningPitcher)
                .OrderByDescending(p => p.Count()).Take(10);

            foreach (IGrouping<string, BaseballGame> pitcher in winningPitchers)
            {
                Console.WriteLine(string.Format("Pitcher: {0}, Wins: {1}", pitcher.Key, pitcher.Count()));
            }

        }


    }
}
