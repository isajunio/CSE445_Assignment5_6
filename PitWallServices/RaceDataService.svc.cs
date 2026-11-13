using PitWallServices.Helpers;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using static PitWallServices.IRaceDataService;


namespace PitWallServices
{

    public class RaceDataService : IRaceDataService
    {

        // Base address of the free OpenF1 API (no key needed, data from 2023 on).
        private const string OpenF1 = "https://api.openf1.org/v1/";

        //code for the GetRaceCalendar method
        public List<RaceInfo> GetRaceCalendar(string year)
        {
            //check the input year is valid
            int y;
            if (!int.TryParse(year, out y) || y < 2023 || y > DateTime.UtcNow.Year + 1)
            {
                throw BadRequest("year must be a number from 2023 to " + (DateTime.UtcNow.Year + 1));
            }

            //list all the races for the year, so you can look up the weekend name
            var meetingNames = new Dictionary<int, string>();
            foreach (var m in GetArray(OpenF1 + "meetings?year=" + y))
            {
                meetingNames[ToInt(m["meeting_key"])] = Text(m, "meeting_name");
            }

            //calls race sessions for the year, and returns a list of RaceInfo objects
            var races = new List<RaceInfo>();
            foreach (var s in GetArray(OpenF1 + "sessions?year=" + y + "&session_name=Race"))
            {
                int meetingKey = ToInt(s["meeting_key"]);
                string name;
                if (!meetingNames.TryGetValue(meetingKey, out name))
                {
                    name = Text(s, "country_name") + " Grand Prix";
                }

                races.Add(new RaceInfo
                {
                    MeetingKey = meetingKey,
                    RaceSessionKey = ToInt(s["session_key"]),
                    Name = name,
                    Circuit = Text(s, "circuit_short_name"),
                    Country = Text(s, "country_name"),
                    RaceStartUtc = Text(s, "date_start"),
                    IsCompleted = ToUtc(s, "date_end") < DateTime.UtcNow
                });
            }

            // Sort by race date (ISO dates sort correctly as text).
            return races.OrderBy(r => r.RaceStartUtc).ToList();
        }

        //code for the GetDrivers method
        //get all the drivers for a given sessionKey
        public List<DriverInfo> GetDrivers(string sessionKey)
        {
            int key = CheckSessionKey(sessionKey);

            // Use a Dictionary so each car number appears only once.
            var drivers = new Dictionary<int, DriverInfo>();
            foreach (var d in GetArray(OpenF1 + "drivers?session_key=" + key))
            {
                int number = ToInt(d["driver_number"]);
                drivers[number] = new DriverInfo
                {
                    Number = number,
                    FullName = Text(d, "full_name"),
                    Acronym = Text(d, "name_acronym"),
                    Team = Text(d, "team_name")
                };
            }

            return drivers.Values.OrderBy(d => d.Number).ToList();
        }


        //code for the GetRaceResults method
        //get the race results for a given sessionKey and raceId

        public List<RaceResult> GetRaceResults(string sessionKey, string top)
        {
            int key = CheckSessionKey(sessionKey);

            // "top" is optional. Empty means 10.
            int count = 10;
            if (!string.IsNullOrWhiteSpace(top) && (!int.TryParse(top, out count) || count < 1 || count > 30))
            {
                throw BadRequest("top must be a number from 1 to 30");
            }

            // Look up names and teams so results show "Lando NORRIS" instead of just "4".
            var names = GetDrivers(sessionKey).ToDictionary(d => d.Number);

            var results = new List<RaceResult>();
            foreach (var r in GetArray(OpenF1 + "session_result?session_key=" + key))
            {
                int number = ToInt(r["driver_number"]);
                DriverInfo driver;
                names.TryGetValue(number, out driver);

                results.Add(new RaceResult
                {
                    Position = ToInt(r.ContainsKey("position") ? r["position"] : null),
                    DriverNumber = number,
                    DriverName = driver != null ? driver.FullName : "Driver #" + number,
                    Team = driver != null ? driver.Team : "",
                    Laps = ToInt(r.ContainsKey("number_of_laps") ? r["number_of_laps"] : null),
                    GapToLeader = FormatGap(r.ContainsKey("gap_to_leader") ? r["gap_to_leader"] : null),
                    Status = GetStatus(r)
                });
            }

            // Classified drivers first (1, 2, 3...), unclassified (0) at the end.
            return results.OrderBy(r => r.Position == 0 ? int.MaxValue : r.Position)
                          .Take(count)
                          .ToList();
        }


        //--------------------- Helper methods ---------------------//

        //GetArray: calls the API and returns an array of dictionaries (one per item)
        private IEnumerable<Dictionary<string, object>> GetArray(string url)
        {
            try
            {
                var array = ApiHelper.GetJson(url) as object[];
                if (array == null)
                {
                    return new List<Dictionary<string, object>>();
                }
                return array.OfType<Dictionary<string, object>>().ToList();
            }
            catch (WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                if (response != null && response.StatusCode == HttpStatusCode.NotFound)
                {
                    return new List<Dictionary<string, object>>();
                }
                throw new WebFaultException<string>("OpenF1 is unavailable right now: " + ex.Message, HttpStatusCode.BadGateway);
            }
        }


        // Converts a value to an int, treating null as 0.
        private int ToInt(object value)
        {
            if (value == null) return 0;
            return Convert.ToInt32(value, CultureInfo.InvariantCulture);
        }

        //sessionKey is a string, but the API needs it as an int. This method checks that the string is a valid int and throws a BadRequest if not.
        private int CheckSessionKey(string sessionKey)
        {
            int key;
            if (!int.TryParse(sessionKey, out key) || key <= 0)
            {
                throw BadRequest("sessionKey must be a positive number, e.g. 9693");
            }
            return key;
        }

        //GetStatus returns the status of a driver in a race result
        private string GetStatus(Dictionary<string, object> r)
        {
            if (IsTrue(r, "dsq")) return "DSQ";
            if (IsTrue(r, "dns")) return "DNS";
            if (IsTrue(r, "dnf")) return "DNF";
            return "Finished";
        }

        //formats the gap to leader as a string, or returns "0" if null
        private string FormatGap(object gap)
        {
            // In qualifying the gap is an array (Q1, Q2, Q3), so use the last value that is not null.
            var list = gap as object[];
            if (list != null)
            {
                gap = list.LastOrDefault(g => g != null);
            }
            if (gap == null) return "";

            decimal seconds;
            if (decimal.TryParse(Convert.ToString(gap, CultureInfo.InvariantCulture),
                NumberStyles.Float, CultureInfo.InvariantCulture, out seconds))
            {
                return seconds == 0 ? "Leader" : "+" + seconds.ToString("0.###", CultureInfo.InvariantCulture) + "s";
            }
            return gap.ToString(); // already text, e.g. "+1 LAP"
        }


        //reads a field text
        private string Text(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != null)
            {
                return dict[key].ToString();
            }
            return "";
        }

        //converts a JSON value to a whole number 
        private int ToInt(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != null)
            {
                return Convert.ToInt32(dict[key]);
            }
            return 0;
        }

        //converts a JSON value to a UTC DateTime
        private DateTime ToUtc(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != null)
            {
                DateTime result;
                if (DateTime.TryParse(dict[key].ToString(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal | DateTimeStyles.AdjustToUniversal, out result))
                {
                    return result;
                }
            }
            return DateTime.MaxValue;
        }

        //checks if a JSON value is true
        private bool IsTrue(Dictionary<string, object> dict, string key)
        {
            if (dict.ContainsKey(key) && dict[key] != null)
            {
                return Convert.ToBoolean(dict[key]);
            }
            return false;
        }

        //throws a BadRequest exception with the given message
        private WebFaultException<string> BadRequest(string message)
        {
            return new WebFaultException<string>(message, System.Net.HttpStatusCode.BadRequest);


        }

    }
}