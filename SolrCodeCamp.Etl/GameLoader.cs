using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using FileHelpers;
using SolrCodeCamp.Shared;

namespace SolrCodeCamp.Etl
{
    public class GameLoader
    {
        public Dictionary<string, string> GetPeople()
        {
            string[] peopleFile = File.ReadAllLines(Global.PEOPLE_FILE);
            Dictionary<string, string> people = new Dictionary<string, string>();
            foreach (string line in peopleFile)
            {
                string[] person = line.Split(',');
                people.Add(person[2], (person[1] + " " + person[0]).Trim());
            }
            return people;
        }

        public Dictionary<string, string> GetTeams()
        {
            Dictionary<string, string> teams = new Dictionary<string, string>();
            var csvTable = FileHelpers.CsvEngine.CsvToDataTable(Global.TEAM_FILE, new CsvOptions("Test", ',', 5));

            CsvEngine engine = new CsvEngine(new CsvOptions("Team", ',', 161));

            foreach (DataRow row in csvTable.Rows)
            {
                teams.Add(row[0].ToString().Replace("\"", "") + row[1].ToString().Replace("\"", ""), row[2].ToString() + " " + row[3].ToString());
            }
            return teams;
        }

        public List<BaseballGame> GetGames()
        {
            FileInfo[] files = new DirectoryInfo(Global.FILES_DIR).GetFiles();
            var people = GetPeople();
            var teams = GetTeams();
            List<BaseballGame> games = new List<BaseballGame>();
            int idCounter = 0;
            foreach (var file in files)
            {
                var csvTable = FileHelpers.CsvEngine.CsvToDataTable(file.FullName, new CsvOptions("Test", ',', 161));

                CsvEngine engine = new CsvEngine(new CsvOptions("BaseballGame", ',', 161));

                foreach (DataRow row in csvTable.Rows)
                {
                    BaseballGame game = new BaseballGame();
                    game.Id = idCounter.ToString();
                    string home = null;
                    teams.TryGetValue(row[6].ToString().Replace("\"", "") + row[7].ToString().Replace("\"", ""),
                                      out home);

                    game.HomeTeam = home;

                    string visitor = null;
                    teams.TryGetValue(row[3].ToString().Replace("\"", "") + row[4].ToString(), out visitor);
                    game.DocType = DocType.BaseballGame;
                    game.VisitingTeam = visitor;
                    game.DayOfTheWeek = row[2].ToString().Replace("\"", "");
                    game.DateRaw = row[0].ToString().Replace("\"", "");

                    game.Date = DateTime.ParseExact(game.DateRaw, "yyyyMMdd", null);
                    game.Year = game.Date.Year;

                    game.HomeTeamScore = Int32.Parse(row[10].ToString());
                    game.VisitingTeamScore = Int32.Parse(row[9].ToString());

                    game.WinningPitcher = row[94].ToString();

                    if (game.WinningPitcher == String.Empty)
                        game.WinningPitcher = "Unknown";
                    else
                        game.PitchersInvolved.Add(game.WinningPitcher);

                    // losing pitcher row - row 96
                    game.LosingPitcher = row[96].ToString();
                    if (game.LosingPitcher == String.Empty)
                        game.LosingPitcher = "Unknown";
                    else 
                        game.PitchersInvolved.Add(game.LosingPitcher);

                    string dayOrNight = row[12].ToString();
                    if (dayOrNight == "D")
                        game.DayOrNight = "Day";
                    else if (dayOrNight == "N")
                        game.DayOrNight = "Night";
                    else game.DayOrNight = "Unknown";

                    string gameLength = row[18].ToString();
                    if (gameLength != String.Empty)
                        game.LengthOfGame = Int32.Parse(gameLength);
                    string homeUmp = null;

                    people.TryGetValue(row[77].ToString(), out homeUmp);
                    game.HomePlateUmpireName = homeUmp;
                    game.Winner = (game.HomeTeamScore > game.VisitingTeamScore)
                                      ? WinningLocation.Home
                                      : WinningLocation.Visitor;

                    if (!string.IsNullOrEmpty(game.HomeTeam))
                        game.TeamsInvolved.Add(game.HomeTeam);

                    if (!string.IsNullOrEmpty(game.VisitingTeam))
                        game.TeamsInvolved.Add(game.VisitingTeam);

                    games.Add(game);
                    idCounter++;
                }
            }
            return games;
        }
    }
}
