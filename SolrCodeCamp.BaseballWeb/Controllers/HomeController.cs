using System;
using System.Linq;
using System.Web.Mvc;
using Microsoft.Practices.ServiceLocation;
using SolrCodeCamp.BaseballWeb.Models;
using SolrCodeCamp.Shared;
using SolrNet;

namespace SolrCodeCamp.BaseballWeb.Controllers
{
    public class HomeController : Controller
    {
        private ISolrOperations<BaseballGame> _solrOperator;
        private readonly BaseballQueryBuilder _queryBuilder;

        public HomeController()
        {
            _solrOperator = ServiceLocator.Current.GetInstance<ISolrOperations<BaseballGame>>();
            _queryBuilder = new BaseballQueryBuilder {SortDirection = "A", CurrentSortTerm = "bg_homeTeam"};
        }

        public ActionResult Index()
        {
            BaseballView baseballView = new BaseballView(_queryBuilder);

            baseballView.ConstructView();

            return View(baseballView);
        }

        public ActionResult Query(string sortTerm, string sortDir, string facets, string searchTerm)
        {
            _queryBuilder.SearchTerm = searchTerm;
            if (!string.IsNullOrEmpty(facets))
            {
                facets.Split(',').ToList().ForEach(f =>
                                                       {
                                                           string[] keyVal = f.Split('^');
                                                           _queryBuilder.AppliedFacets.Add(new Tuple<string, string>(keyVal[0], keyVal[1]));
                                                       });

            }
            _queryBuilder.CurrentSortTerm = sortTerm;
            _queryBuilder.SortDirection = sortDir;
            
            BaseballView baseballView = new BaseballView(_queryBuilder);
            baseballView.ConstructView();

            return View(baseballView);
        }

    }
}
