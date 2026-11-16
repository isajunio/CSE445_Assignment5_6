using System;
using System.Web;

namespace PitWallTryIt
{
    // Saves the user's F1 preferences (username, favorite driver, favorite team) in ONE HttpCookie that lasts 7 days.
    // No passwords or login data are stored here.
    public static class PreferencesCookie
    {
        public const string CookieName = "RWH_F1Prefs";

        public const int ExpiryDays = 7; 


        public static void Save(HttpResponse response, string username, string driver, string team)
        {
            HttpCookie cookie = new HttpCookie(CookieName);

            cookie.Values["user"] = HttpUtility.UrlEncode(username ?? "");
            cookie.Values["driver"] = HttpUtility.UrlEncode(driver ?? "");
            cookie.Values["team"] = HttpUtility.UrlEncode(team ?? "");

            cookie.Expires = DateTime.Now.AddDays(ExpiryDays); 
            cookie.HttpOnly = true;
            cookie.Path = "/"; 

            response.Cookies.Set(cookie);
        }
        public static F1Preferences Load(HttpRequest request)
        {
            HttpCookie cookie = request.Cookies[CookieName];
            if (cookie == null)
            {
                return null;
            }

            return new F1Preferences
            {
                Username = HttpUtility.UrlDecode(cookie.Values["user"] ?? ""),
                Driver = HttpUtility.UrlDecode(cookie.Values["driver"] ?? ""),
                Team = HttpUtility.UrlDecode(cookie.Values["team"] ?? "")
            };
        }

        public static void Clear(HttpResponse response)
        {
            HttpCookie cookie = new HttpCookie(CookieName);
            cookie.Expires = DateTime.Now.AddDays(-1);
            response.Cookies.Set(cookie);
            cookie.Path = "/";
            cookie.HttpOnly = true;
        }
    }
    public class F1Preferences
    {
        public string Username { get; set; }
        public string Driver { get; set; }
        public string Team { get; set; }
    }
}