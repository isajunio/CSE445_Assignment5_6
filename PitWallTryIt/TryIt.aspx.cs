using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.UI;

namespace PitWallTryIt
{
    //tryit page for the 3 pit wall services
    public partial class TryIt : Page
    {
        //address of the REST race data service
        private const string RaceUrl = "http://localhost:5001/RaceDataService.svc/";

        //runs when the page loads
        protected void Page_Load(object sender, EventArgs e)
        {
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        //---------------- weather ----------------

        //check weather button, calls Weather5day
        protected void btnWeather_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new WeatherRef.WeatherServiceClient())
                {
                    lblWeather.Text = Lines(client.Weather5day(txtZip.Text));
                }
            }
            catch (Exception ex)
            {
                lblWeather.Text = ShowError(ex);
            }
        }

        //---------------- race center (REST) ----------------

        //show calendar button, calls GetRaceCalendar
        protected void btnCalendar_Click(object sender, EventArgs e)
        {
            try
            {
                var races = GetJsonList(RaceUrl + "races/" + Uri.EscapeDataString(txtYear.Text.Trim()));
                lblRaces.Text = races.Count == 0 ? "No races found." : Lines(races.Select(r =>
                    r["Name"] + " · " + r["Circuit"] + " · " + ShortDate(r["RaceStartUtc"].ToString())
                    + " · key " + r["RaceSessionKey"] + ((bool)r["IsCompleted"] ? "" : " (upcoming)")));
            }
            catch (Exception ex)
            {
                lblRaces.Text = ShowError(ex);
            }
        }

        //drivers button, calls GetDrivers
        protected void btnDrivers_Click(object sender, EventArgs e)
        {
            try
            {
                var drivers = GetJsonList(RaceUrl + "drivers/" + Uri.EscapeDataString(txtSession.Text.Trim()));
                lblRaces.Text = drivers.Count == 0 ? "No drivers found." : Lines(drivers.Select(d =>
                    "#" + d["Number"] + " " + d["FullName"] + " · " + d["Team"]));
            }
            catch (Exception ex)
            {
                lblRaces.Text = ShowError(ex);
            }
        }

        //results button, calls GetRaceResults
        protected void btnResults_Click(object sender, EventArgs e)
        {
            try
            {
                string url = RaceUrl + "results/" + Uri.EscapeDataString(txtSession.Text.Trim())
                             + "?top=" + Uri.EscapeDataString(txtTop.Text.Trim());
                var results = GetJsonList(url);
                lblRaces.Text = results.Count == 0 ? "No results found." : Lines(results.Select(r =>
                    ((int)r["Position"] == 0 ? "--" : "P" + r["Position"]) + " " + r["DriverName"] + " · " + r["Team"]
                    + " · " + r["GapToLeader"] + (r["Status"].ToString() == "Finished" ? "" : " · " + r["Status"])));
            }
            catch (Exception ex)
            {
                lblRaces.Text = ShowError(ex);
            }
        }

        //---------------- fantasy league ----------------

        //join button, calls RegisterPlayer
        protected void btnJoin_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new FantasyRef.FantasyLeagueServiceClient())
                {
                    lblFantasy.Text = HttpUtility.HtmlEncode(client.RegisterPlayer(txtUser.Text));
                }
            }
            catch (Exception ex)
            {
                lblFantasy.Text = ShowError(ex);
            }
        }

        //lock picks button, calls SubmitPicks
        protected void btnLockPicks_Click(object sender, EventArgs e)
        {
            int p1, p2, p3;
            if (!int.TryParse(txtP1.Text, out p1) || !int.TryParse(txtP2.Text, out p2) || !int.TryParse(txtP3.Text, out p3))
            {
                lblFantasy.Text = "Error: P1, P2 and P3 must be car numbers, like 4, 1, 63";
                return;
            }

            try
            {
                using (var client = new FantasyRef.FantasyLeagueServiceClient())
                {
                    lblFantasy.Text = HttpUtility.HtmlEncode(client.SubmitPicks(txtUser.Text, txtPickSession.Text, p1, p2, p3));
                }
            }
            catch (Exception ex)
            {
                lblFantasy.Text = ShowError(ex);
            }
        }

        //score race button, calls ScoreRace
        protected void btnScore_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new FantasyRef.FantasyLeagueServiceClient())
                {
                    lblFantasy.Text = HttpUtility.HtmlEncode(client.ScoreRace(txtPickSession.Text));
                }
            }
            catch (Exception ex)
            {
                lblFantasy.Text = ShowError(ex);
            }
        }

        //leaderboard button, calls GetLeaderboard
        protected void btnLeaderboard_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new FantasyRef.FantasyLeagueServiceClient())
                {
                    lblFantasy.Text = Lines(client.GetLeaderboard());
                }
            }
            catch (Exception ex)
            {
                lblFantasy.Text = ShowError(ex);
            }
        }

        //my picks button, calls GetPlayerPicks
        protected void btnMyPicks_Click(object sender, EventArgs e)
        {
            try
            {
                using (var client = new FantasyRef.FantasyLeagueServiceClient())
                {
                    lblFantasy.Text = Lines(client.GetPlayerPicks(txtUser.Text));
                }
            }
            catch (Exception ex)
            {
                lblFantasy.Text = ShowError(ex);
            }
        }

        //---------------- helpers ----------------

        //calls a REST url and turns the json list into dictionaries
        //if the service sends back an error (like 400) it shows that message
        private List<Dictionary<string, object>> GetJsonList(string url)
        {
            try
            {
                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;
                    var array = new JavaScriptSerializer().DeserializeObject(web.DownloadString(url)) as object[];
                    return array == null ? new List<Dictionary<string, object>>()
                                         : array.OfType<Dictionary<string, object>>().ToList();
                }
            }
            catch (WebException ex)
            {
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        throw new Exception(Regex.Replace(reader.ReadToEnd(), "<[^>]+>", "").Trim());
                    }
                }
                throw;
            }
        }

        //turns "2025-03-16T04:00:00+00:00" into "Mar 16"
        private string ShortDate(string iso)
        {
            DateTime d;
            return DateTime.TryParse(iso, out d) ? d.ToString("MMM d") : iso;
        }

        //puts each item on its own line
        private string Lines(IEnumerable<string> items)
        {
            return string.Join("<br/>", items.Select(i => HttpUtility.HtmlEncode(i)));
        }

        //shows an error in the output box
        private string ShowError(Exception ex)
        {
            return "Error: " + HttpUtility.HtmlEncode(ex.Message);
        }
    }
}