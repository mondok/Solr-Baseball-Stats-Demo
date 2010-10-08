using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SolrCodeCamp.Shared;
using SolrNet;

namespace SolrCodeCamp.BaseballWeb.Models
{
    public class BaseballView
    {
        // display name, solr name, count
        public Dictionary<string, List<Tuple<string, int>>> Facets { get; set; }

        public string SortTerm
        {
            get
            {
                return _queryBuilder.CurrentSortTerm;
            }
        }

        public string SortDirection
        {
            get
            {
                return _queryBuilder.SortDirection;
            }
        }

        public List<BaseballGame> GameResults { get; set; }

        public int TotalRecordsFound { get; set; }

        private ISolrQueryResults<BaseballGame> _queryResult;

        private readonly BaseballQueryBuilder _queryBuilder;

        public BaseballView(BaseballQueryBuilder queryBuilder)
        {
            this.Facets = new Dictionary<string, List<Tuple<string, int>>>();
            _queryBuilder = queryBuilder;
        }

        public void ConstructView()
        {
            _queryResult = _queryBuilder.ExecuteQuery();

            this.TotalRecordsFound = _queryResult.NumFound;

            // add facets
            // here we loop through all of our known facets and pull out what had results
            // this allows us to keep the facets in the order we want
            foreach (var facet in Global.BaseballGameFacetNames.OrderBy(d => d.Item3))
            {
                string key = facet.Item2;
                if (_queryResult.FacetFields.ContainsKey(key))
                {
                    List<Tuple<string, int>> facetValues = new List<Tuple<string, int>>();

                    foreach (var v in _queryResult.FacetFields[key])
                    {
                        facetValues.Add(new Tuple<string, int>(v.Key, v.Value));
                    }

                    this.Facets.Add(key, facetValues);
                }
            }

            // set results
            this.GameResults = _queryResult.ToList();
        }
    }
}