using System;
using System.Globalization;
using System.IO;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Configuration;

namespace PitWallTryIt
{
    // Gabriela Martinez - TryIt for PitStrategyService (REST).
    
    public partial class RaceWeekend
    {
        
        protected void btnEstimate_Click(object sender, EventArgs e)
        {
            double lap, loss;
            int laps, stops;
            CultureInfo inv = CultureInfo.InvariantCulture;

            if (!double.TryParse(txtLapTime.Text.Trim(), NumberStyles.Float, inv, out lap) ||
                !int.TryParse(txtLaps.Text.Trim(), out laps) ||
                !int.TryParse(txtStops.Text.Trim(), out stops) ||
                !double.TryParse(txtPitLoss.Text.Trim(), NumberStyles.Float, inv, out loss))
            {
                lblEstimate.Text = "Error: enter numbers only (laps and stops must be whole numbers).";
                return;
            }

            // service address comes from Web.config, so only that line changes after deployment
            string baseUrl = (WebConfigurationManager.AppSettings["PitStrategyBaseUrl"] ?? "").Trim();
            if (baseUrl.Length == 0)
            {
                baseUrl = "http://localhost:59014/PitStrategyService.svc"; // fallback for local testing
            }

            string url = baseUrl.TrimEnd('/') + "/estimate"
                + "?lap=" + lap.ToString(inv) + "&laps=" + laps
                + "&stops=" + stops + "&loss=" + loss.ToString(inv);

            try
            {
                using (var web = new WebClient())
                {
                    web.Encoding = Encoding.UTF8;
                    double seconds = double.Parse(web.DownloadString(url), inv);
                    TimeSpan t = TimeSpan.FromSeconds(seconds);

                    lblEstimate.Text = "Estimated race time: " + seconds.ToString("0.###", inv) + " seconds ("
                        + (int)t.TotalHours + "h " + t.Minutes + "m " + t.Seconds + "s)";
                }
            }
            catch (WebException ex)
            {
                // the service sends a readable message for bad inputs (HTTP 400)
                if (ex.Response != null)
                {
                    using (var reader = new StreamReader(ex.Response.GetResponseStream()))
                    {
                        string msg = Regex.Replace(reader.ReadToEnd(), "<[^>]+>", "").Trim().Trim('"');
                        lblEstimate.Text = "Error: " + HttpUtility.HtmlEncode(msg);
                    }
                }
                else
                {
                    lblEstimate.Text = "Error: could not reach PitStrategyService. Is it running?<br/>URL: " + HttpUtility.HtmlEncode(url);
                }
            }
            catch (Exception ex)
            {
                // any other problem: show the message and the address that was tried
                lblEstimate.Text = "Error: " + HttpUtility.HtmlEncode(ex.Message) + "<br/>URL: " + HttpUtility.HtmlEncode(url);
            }
        }
    }
}