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
            LoadGames();
            RunExamples();
        }

        static void RunExamples()
        {
            QueryExamples examples = new QueryExamples(_solr);

            examples.RunFacetExample();

            examples.RunRangeSearchExample(2008, 2010);

            examples.RunTextSearchExample("Halladay");
        }

        static void LoadGames()
        {
            GameLoader loader = new GameLoader();
            var games = loader.GetGames();
            _solr.Delete(SolrQuery.All);
            _solr.Add(games);
           _solr.Commit();
            _solr.Optimize();
        }
    }
}
