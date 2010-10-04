using System;
using System.Configuration;
using System.Linq;

namespace SolrCodeCamp.Shared
{
    public static class Global
    {

        /// <summary>
        /// Gets the baseball game facet names in order Tuple<displayname, solrname>
        /// </summary>
        /// <value>The baseball game facet names.</value>
        public static Tuple<string,string>[] BaseballGameFacetNames
        {
            get
            {
                return new[]
                           {
                               new Tuple<string, string>("Home Team", "bg_homeTeam"),
                               new Tuple<string, string>("Visiting Team", "bg_visitingTeam"),
                               new Tuple<string, string>("Day of the Week", "bg_dayOfTheWeek"),
                               new Tuple<string, string>("Year", "bg_year"),
                               new Tuple<string, string>("Day/Night", "bg_dayOrNight"),
                               new Tuple<string, string>("Home Plate Ump", "bg_homePlateUmpire")
                           };
            }
        }

        public static string LookupFacetDisplayName(string facetKey)
        {
            return BaseballGameFacetNames.Where(t => t.Item2 == facetKey).Single().Item1;
        }

        public static string PEOPLE_FILE
        {
            get
            {
                return ConfigurationManager.AppSettings["PEOPLE_FILE"];
            }
        }

        public static string TEAM_FILE
        {
            get
            {
                return ConfigurationManager.AppSettings["TEAM_FILE"];
            }
        }

        public static string SOLR_LOCATION
        {
            get
            {
                return ConfigurationManager.AppSettings["SOLR_LOCATION"];
            }
        }

        public static string FILES_DIR
        {
            get
            {
                return ConfigurationManager.AppSettings["FILES_DIR"];
            }
        }
    }
}