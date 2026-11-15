using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using System.Net;
using System.Security.Principal;
using System.Web.Configuration;
using PitWallTryIt.Security;
using System.Configuration;

namespace PitWallTryIt
{
    public class Global : System.Web.HttpApplication
    {
        // ================= Section: Ayomide Oshilaja =====================
        //runs the visitor counter, XML files, race calender cache

        protected void Application_Start(object sender, EventArgs e)
        {

            Application["Visitors"] = 0;   // Izzy's Session_Start increases this
            AccountStore.EnsureFiles();     // Member.xml + Staff.xml

            try
            {
                // Cache this season's calendar so pages don't call the service every load
                string baseUrl = WebConfigurationManager.AppSettings["RaceDataBaseUrl"];
                using (var client = new WebClient())
                {
                    Application["RaceCalendarJson"] =
                        client.DownloadString(baseUrl + "/races/" + DateTime.Now.Year);
                }
            }
            catch (Exception)
            {
                Application["RaceCalendarJson"] = null; // pages fall back to calling the service
            }
        }

            //login cookie so the staff rules work
            protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

             

             HttpCookie authCookie = Request.Cookies[FormsAuthentication.FormsCookieName];
             if (authCookie == null || string.IsNullOrEmpty(authCookie.Value)) return;

            FormsAuthenticationTicket ticket; 
             try
             {
                ticket = FormsAuthentication.Decrypt(authCookie.Value);
             }
             catch
            {
                return;
            }

             if (ticket == null) return;

            string[] roles = ticket.UserData.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            Context.User = new GenericPrincipal(new FormsIdentity(ticket), roles);



        }


        //if anything happens - catches unhandled errors, saves the messages, shows errors page 
        protected void Application_Error(object sender, EventArgs e)
        {

            Exception ex = Server.GetLastError();
            if (ex is HttpUnhandledException && ex.InnerException != null) ex = ex.InnerException;

            Application.Lock();
            Application["LastError"] = DateTime.Now + ": " + (ex != null ? ex.Message : "Unknown error");
            Application.UnLock();

            // Don't redirect if Error.aspx itself failed (prevents a loop)
            if (Request.Path.EndsWith("Error.aspx", StringComparison.OrdinalIgnoreCase)) return;

            Server.ClearError();
            Response.Redirect("~/Error.aspx", false);
            Context.ApplicationInstance.CompleteRequest();

        }


        // ================= Section: Izzy Junio =====================
        //Session_Start
        // Runs once for each new visitor session: add 1 to the counter.
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["Started"] = DateTime.UtcNow;

            // Lock so two visitors arriving together don't overwrite each other's update.
            Application.Lock();
            try
            {
                Application["VisitorCount"] = (int)(Application["VisitorCount"] ?? 0) + 1;
            }
            finally
            {
                Application.UnLock();
            }
        }
        //Session_End



    }
}