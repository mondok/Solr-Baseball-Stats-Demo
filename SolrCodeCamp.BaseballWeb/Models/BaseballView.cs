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

        private ISolrQueryResults<BaseballGame> _queryResult;

        private BaseballQueryBuilder _queryBuilder;

        public BaseballView(BaseballQueryBuilder queryBuilder)
        {
            this.Facets = new Dictionary<string, List<Tuple<string, int>>>();
            _queryBuilder = queryBuilder;
        }

        public void ConstructView()
        {
            _queryResult = _queryBuilder.ExecuteQuery();

            // add facets
            foreach (var f in _queryResult.FacetFields)
            {
                string key = f.Key;
                List<Tuple<string, int>> facetValues = new List<Tuple<string, int>>();

                foreach (var v in f.Value)
                {
                    facetValues.Add(new Tuple<string, int>(v.Key, v.Value));
                }

                this.Facets.Add(key, facetValues);
            }

            // set results
            this.GameResults = _queryResult.ToList();
        }
    }
}