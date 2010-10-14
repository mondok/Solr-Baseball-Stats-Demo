using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Practices.ServiceLocation;
using SolrCodeCamp.Shared;
using SolrNet;
using SolrNet.Commands.Parameters;

namespace SolrCodeCamp.BaseballWeb.Models
{
    public class BaseballQueryBuilder
    {
        private readonly ISolrOperations<BaseballGame> _solrOperations;

        public List<Tuple<string, string>> AppliedFacets { get; set; }

        public string CurrentSortTerm { get; set; }

        public string SortDirection { get; set; }

        public string SearchTerm { get; set; }

        public BaseballQueryBuilder()
        {
            _solrOperations = ServiceLocator.Current.GetInstance<ISolrOperations<BaseballGame>>();
            this.AppliedFacets = new List<Tuple<string, string>>();
        }

        public BaseballQueryBuilder(ISolrOperations<BaseballGame> solrOperations)
        {
            _solrOperations = solrOperations;
            this.AppliedFacets = new List<Tuple<string, string>>();
        }

        private FacetParameters BuildBaseFacetQuery()
        {
            FacetParameters facetParameters = new FacetParameters();
            foreach (var facet in Global.BaseballGameFacetNames)
            {
                var facetQuery = new SolrFacetFieldQuery(facet.Item2);
                facetQuery.Limit = -1;
                facetParameters.Queries.Add(facetQuery);
            }
            return facetParameters;
        }

        public ISolrQueryResults<BaseballGame> ExecuteQuery()
        {
            List<ISolrQuery> fieldQueries = new List<ISolrQuery>();
            QueryOptions options = new QueryOptions();
            options.Facet = BuildBaseFacetQuery();
            options.Rows = 200;

            if (this.AppliedFacets.Count > 0)
            {
                var facetGroups = this.AppliedFacets.Select(t => t.Item1).Distinct().ToList();

                foreach(var group in facetGroups)
                {
                    List<ISolrQuery> queries = this.AppliedFacets.Where(fg => fg.Item1 == group).Select(q => new SolrQueryByField(q.Item1, q.Item2) as ISolrQuery).ToList();
                    SolrMultipleCriteriaQuery smcq =
                        new SolrMultipleCriteriaQuery(queries,"OR");
                    fieldQueries.Add(smcq);
                }

                ISolrQuery multipleCriteriaQuery = new SolrMultipleCriteriaQuery(fieldQueries, "AND");
                options.AddFilterQueries(multipleCriteriaQuery);
            }
            options.OrderBy.Add(new SortOrder(this.CurrentSortTerm, this.SortDirection == "D" ? Order.DESC : Order.ASC));
            options.AddFilterQueries(new SolrQueryByField("docType", DocType.BaseballGame.ToString()));
            ISolrQuery finalQuery = SolrQuery.All;

            if (!string.IsNullOrEmpty(this.SearchTerm))
            {
                finalQuery = new SolrQuery(this.SearchTerm);
            }



            return _solrOperations.Query(finalQuery, options);
        }
    }
}