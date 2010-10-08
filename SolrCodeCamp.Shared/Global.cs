using System;
using System.Configuration;
using System.Linq;

namespace SolrCodeCamp.Shared
{
    public static class Global
    {

        /// <summary>
        /// Gets the baseball game facet names in order Tuple<displayname, solrname, order>
        /// </summary>
        /// <value>The baseball game facet names.</value>
        public static Tuple<string, string, int>[] BaseballGameFacetNames
        {
            get
            {
                return new[]
                           {
                               new Tuple<string, string, int>("Home Team", "bg_homeTeam",1),
                               new Tuple<string, string, int>("Visiting Team", "bg_visitingTeam",2),
                               new Tuple<string, string, int>("Day of the Week", "bg_dayOfTheWeek",3),
                               new Tuple<string, string, int>("Year", "bg_year",5),
                               new Tuple<string, string, int>("Day/Night", "bg_dayOrNight",6),
                               new Tuple<string, string, int>("Home Plate Ump", "bg_homePlateUmpire",4),
                               new Tuple<string, string, int>("Winner Location", "bg_winningLocation",7)
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