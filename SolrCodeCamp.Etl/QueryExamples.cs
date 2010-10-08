using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SolrCodeCamp.Shared;
using SolrNet;
using SolrNet.Commands.Parameters;
using SolrNet.DSL;
using SolrNet.DSL.Impl;

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

            var umpreFacetResults = results.FacetFields["bg_homePlateUmpire"];
        }

        public void RunTextSearchExample(string searchTerm)
        {
            ISolrQueryResults<BaseballGame> results = _solrOperations.Query(new SolrQuery(searchTerm));

            List<BaseballGame> gameResults = results.ToList();
        }

        public void RunRangeSearchExample(int startYear, int endYear)
        {
            ISolrQueryResults<BaseballGame> results =
                _solrOperations.Query(new SolrQueryByRange<int>("bg_year", startYear, endYear));

            List<BaseballGame> gameResults = results.ToList();

            var winningPitchers = gameResults.GroupBy(t => t.WinningPitcher).OrderByDescending(p => p.Count()).Take(10);

            foreach(var pitcher in winningPitchers)
            {
                Console.WriteLine(string.Format("Pitcher: {0}, Wins: {1}", pitcher.Key, pitcher.Count()));
            }

        }


    }
}
