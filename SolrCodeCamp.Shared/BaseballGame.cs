﻿using System;
using System.Collections.Generic;
using SolrNet.Attributes;

namespace SolrCodeCamp.Shared
{
    public class BaseballGame
    {
        [SolrUniqueKey("id")]
        public string Id { get; set; }

        [SolrField("docType")]
        public DocType DocType { get; set; }

        [SolrField("bg_teams")]
        public List<string> TeamsInvolved { get; set; }

        [SolrField("bg_pitchers")]
        public List<string> PitchersInvolved { get; set; }

        // 7
        [SolrField("bg_homeTeam")]
        public string HomeTeam { get; set; }

        // 4
        [SolrField("bg_visitingTeam")]
        public string VisitingTeam { get; set; }

        [SolrField("bg_winningPitcher")]
        public string WinningPitcher { get; set; }

        [SolrField("bg_losingPitcher")]
        public string LosingPitcher { get; set; }

        // 3
        [SolrField("bg_dayOfTheWeek")]
        public string DayOfTheWeek { get; set; }

        // 1
        [SolrField("bg_dateRaw")]
        public string DateRaw { get; set; }

        [SolrField("bg_year")]
        public int Year { get; set; }

        [SolrField("bg_date")]
        public DateTime Date { get; set; }

        // 11
        [SolrField("bg_homeTeamScore")]
        public int HomeTeamScore { get; set; }

        // 10
        [SolrField("bg_visitingTeamScore")]
        public int VisitingTeamScore { get; set; }

        // 13
        [SolrField("bg_dayOrNight")]
        public string DayOrNight { get; set; }

        // 19
        [SolrField("bg_lengthOfGame")]
        public int LengthOfGame { get; set; }

        // 78
        [SolrField("bg_homePlateUmpire")]
        public string HomePlateUmpireName { get; set; }

        [SolrField("bg_winningLocation")]
        public WinningLocation Winner { get; set; }

        public BaseballGame()
        {
            this.TeamsInvolved = new List<string>();
            this.PitchersInvolved = new List<string>();
        }
    }
}
