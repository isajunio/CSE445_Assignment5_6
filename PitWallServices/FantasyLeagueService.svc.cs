using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Hosting;
using System.Web.Script.Serialization;

namespace PitWallServices
{
    //fantasy league service, saves everything in App_Data/league.json (no database)
    public class FantasyLeagueService : IFantasyLeagueService
    {
        //points for an exact podium spot, and for a podium driver in the wrong spot
        private const int ExactPoints = 10;
        private const int PodiumPoints = 3;

        //only one request can read or write the file at a time
        private static readonly object FileLock = new object();

        //adds a new player to the league
        //username (string) = 3 to 20 letters, numbers or _
        //returns string = success or error message
        public string RegisterPlayer(string username)
        {
            string name = (username ?? "").Trim();
            if (!Regex.IsMatch(name, @"^[A-Za-z0-9_]{3,20}$"))
            {
                return "Error: username must be 3 to 20 letters, numbers or _";
            }

            lock (FileLock)
            {
                LeagueData league = Load();
                if (FindPlayer(league, name) != null)
                {
                    return "Error: " + name + " is already registered";
                }

                league.Players.Add(new Player { Username = name, Points = 0, ExactPicks = 0 });
                Save(league);
                return name + " joined the league!";
            }
        }

        //saves a player's podium picks for a race (submitting again replaces the old picks)
        //username (string), sessionKey (string) = race session key, p1/p2/p3 (int) = car numbers
        //returns string = success or error message
        public string SubmitPicks(string username, string sessionKey, int p1, int p2, int p3)
        {
            int key;
            if (!int.TryParse(sessionKey, out key) || key <= 0)
            {
                return "Error: sessionKey must be a positive number, e.g. 9693";
            }
            if (p1 <= 0 || p2 <= 0 || p3 <= 0)
            {
                return "Error: car numbers must be positive";
            }
            if (p1 == p2 || p1 == p3 || p2 == p3)
            {
                return "Error: you can't pick the same driver twice";
            }

            try
            {
                //check the drivers really raced in this session (uses our RaceDataService)
                var drivers = new RaceDataService().GetDrivers(sessionKey).Select(d => d.Number).ToList();
                if (drivers.Count == 0)
                {
                    return "Error: no drivers found for session " + key;
                }
                foreach (int car in new[] { p1, p2, p3 })
                {
                    if (!drivers.Contains(car))
                    {
                        return "Error: car #" + car + " did not race in session " + key;
                    }
                }
            }
            catch (Exception ex)
            {
                return "Error: could not check drivers right now (" + ex.Message + ")";
            }

            lock (FileLock)
            {
                LeagueData league = Load();
                Player player = FindPlayer(league, username);
                if (player == null)
                {
                    return "Error: player not found, register first";
                }
                if (league.ScoredRaces.Contains(key))
                {
                    return "Error: race " + key + " is already scored, picks are closed";
                }

                //remove old picks for this race so there's only one per player
                league.Picks.RemoveAll(p => p.SessionKey == key && SameName(p.Username, player.Username));
                league.Picks.Add(new Pick { Username = player.Username, SessionKey = key, P1 = p1, P2 = p2, P3 = p3 });
                Save(league);
                return "Picks saved for " + player.Username + ": P1 #" + p1 + ", P2 #" + p2 + ", P3 #" + p3;
            }
        }

        //scores every pick for a race using the real podium from OpenF1
        //sessionKey (string) = race session key
        //returns string = summary of points given out, or an error message
        public string ScoreRace(string sessionKey)
        {
            int key;
            if (!int.TryParse(sessionKey, out key) || key <= 0)
            {
                return "Error: sessionKey must be a positive number, e.g. 9693";
            }

            int[] podium;
            try
            {
                //get the real top 3 (uses our RaceDataService)
                podium = new RaceDataService().GetRaceResults(sessionKey, "3")
                    .Where(r => r.Position >= 1 && r.Position <= 3)
                    .OrderBy(r => r.Position)
                    .Select(r => r.DriverNumber)
                    .ToArray();
            }
            catch (Exception ex)
            {
                return "Error: could not get results right now (" + ex.Message + ")";
            }
            if (podium.Length < 3)
            {
                return "Error: results for session " + key + " are not available yet";
            }

            lock (FileLock)
            {
                LeagueData league = Load();
                if (league.ScoredRaces.Contains(key))
                {
                    return "Error: race " + key + " was already scored";
                }

                var picks = league.Picks.Where(p => p.SessionKey == key).ToList();
                var lines = new List<string>();
                foreach (Pick pick in picks)
                {
                    int exact;
                    pick.Points = ScorePick(pick, podium, out exact);
                    pick.Scored = true;

                    Player player = FindPlayer(league, pick.Username);
                    if (player != null)
                    {
                        player.Points += pick.Points;
                        player.ExactPicks += exact;
                    }
                    lines.Add(pick.Username + " +" + pick.Points);
                }

                league.ScoredRaces.Add(key);
                Save(league);

                string result = "Podium: #" + podium[0] + ", #" + podium[1] + ", #" + podium[2] + ". ";
                return picks.Count == 0
                    ? result + "No picks for this race."
                    : result + "Scored " + picks.Count + " player(s): " + string.Join(", ", lines);
            }
        }

        //gets the leaderboard, highest points first (ties go to more exact picks)
        //returns string[] = one line per player
        public string[] GetLeaderboard()
        {
            lock (FileLock)
            {
                LeagueData league = Load();
                if (league.Players.Count == 0)
                {
                    return new[] { "No players yet." };
                }

                var sorted = league.Players
                    .OrderByDescending(p => p.Points)
                    .ThenByDescending(p => p.ExactPicks)
                    .ThenBy(p => p.Username, StringComparer.OrdinalIgnoreCase)
                    .ToList();

                var lines = new List<string>();
                for (int i = 0; i < sorted.Count; i++)
                {
                    lines.Add((i + 1) + ". " + sorted[i].Username + " - " + sorted[i].Points + " pts (" + sorted[i].ExactPicks + " exact)");
                }
                return lines.ToArray();
            }
        }

        //gets one player's picks and points
        //username (string)
        //returns string[] = one line per race picked
        public string[] GetPlayerPicks(string username)
        {
            lock (FileLock)
            {
                LeagueData league = Load();
                Player player = FindPlayer(league, username);
                if (player == null)
                {
                    return new[] { "Error: player not found" };
                }

                var picks = league.Picks.Where(p => SameName(p.Username, player.Username))
                                        .OrderBy(p => p.SessionKey).ToList();
                if (picks.Count == 0)
                {
                    return new[] { player.Username + " has no picks yet." };
                }

                return picks.Select(p => "Race " + p.SessionKey + ": P1 #" + p.P1 + ", P2 #" + p.P2 + ", P3 #" + p.P3
                    + (p.Scored ? " - " + p.Points + " pts" : " - not scored yet")).ToArray();
            }
        }

        //--------------------- Helper methods ---------------------//

        //works out points for one pick
        //pick (Pick), podium (int[]) = real P1/P2/P3, out exact (int) = how many were exact
        //returns int = points earned
        private int ScorePick(Pick pick, int[] podium, out int exact)
        {
            int[] guess = { pick.P1, pick.P2, pick.P3 };
            int points = 0;
            exact = 0;
            for (int i = 0; i < 3; i++)
            {
                if (guess[i] == podium[i])
                {
                    points += ExactPoints;
                    exact++;
                }
                else if (podium.Contains(guess[i]))
                {
                    points += PodiumPoints;
                }
            }
            return points;
        }

        //finds a player by name (not case sensitive)
        //league (LeagueData), username (string)
        //returns Player, or null if not found
        private Player FindPlayer(LeagueData league, string username)
        {
            string name = (username ?? "").Trim();
            return league.Players.FirstOrDefault(p => SameName(p.Username, name));
        }

        //checks if two usernames match (not case sensitive)
        //a (string), b (string)
        //returns bool
        private bool SameName(string a, string b)
        {
            return string.Equals(a, b, StringComparison.OrdinalIgnoreCase);
        }

        //gets the full path of league.json inside App_Data
        //returns string = file path
        private string FilePath()
        {
            string folder = HostingEnvironment.MapPath("~/App_Data")
                            ?? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "App_Data");
            Directory.CreateDirectory(folder);
            return Path.Combine(folder, "league.json");
        }

        //reads the league from the json file (makes an empty one if the file is missing)
        //returns LeagueData
        private LeagueData Load()
        {
            string path = FilePath();
            if (!File.Exists(path))
            {
                return new LeagueData();
            }

            string json = File.ReadAllText(path);
            LeagueData league = string.IsNullOrWhiteSpace(json)
                ? new LeagueData()
                : new JavaScriptSerializer().Deserialize<LeagueData>(json);

            league = league ?? new LeagueData();
            league.Players = league.Players ?? new List<Player>();
            league.Picks = league.Picks ?? new List<Pick>();
            league.ScoredRaces = league.ScoredRaces ?? new List<int>();
            return league;
        }

        //writes the league back to the json file
        //league (LeagueData)
        private void Save(LeagueData league)
        {
            File.WriteAllText(FilePath(), new JavaScriptSerializer().Serialize(league));
        }
    }

    //everything stored in league.json
    public class LeagueData
    {
        public List<Player> Players { get; set; } = new List<Player>();
        public List<Pick> Picks { get; set; } = new List<Pick>();
        public List<int> ScoredRaces { get; set; } = new List<int>();
    }

    //one player in the league
    public class Player
    {
        public string Username { get; set; }
        public int Points { get; set; }
        public int ExactPicks { get; set; }
    }

    //one player's podium picks for one race
    public class Pick
    {
        public string Username { get; set; }
        public int SessionKey { get; set; }
        public int P1 { get; set; }
        public int P2 { get; set; }
        public int P3 { get; set; }
        public int Points { get; set; }
        public bool Scored { get; set; }
    }
}