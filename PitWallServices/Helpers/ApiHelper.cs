using System.Net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Text;
using System.Web.Script.Serialization;

namespace PitWallServices.Helpers
{
    public static class ApiHelper
    {

        static ApiHelper()
        {
            // Type this exactly. It turns on TLS 1.2 so HTTPS APIs work.
            ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls12;
        }

        //downloads the text from the url and returns it as a string
        public static string Get(string url)
        {
            
            using (WebClient client = new WebClient())
            {
                client.Encoding = Encoding.UTF8;
                client.Headers.Add(HttpRequestHeader.UserAgent, "PitWall-CSE445");
                return client.DownloadString(url);
            }
        }

        //gets the json from the url and returns it as an object
        public static object GetJson(string url)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            serializer.MaxJsonLength = int.MaxValue;
            return serializer.DeserializeObject(Get(url));
        }

    }
}