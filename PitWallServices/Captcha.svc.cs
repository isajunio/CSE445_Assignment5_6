using System;
using System.IO;
using System.Net;
using System.ServiceModel.Web;
using System.Xml.Linq;

namespace PitWallServices
{
    public class CaptchaService : ICaptchaService
    {
        private const string AsuBase =
            "https://venus.sod.asu.edu/WSRepository/Services/ImageVerifier/Service.svc/";

        public string GetVerifierString(string length)
        {
            int n;
            if (!int.TryParse(length, out n) || n < 1 || n > 12)
                throw new WebFaultException<string>(
                    "length must be an integer between 1 and 12", HttpStatusCode.BadRequest);

            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");

            string raw;
            try
            {
                using (var wc = new WebClient())
                    raw = wc.DownloadString(AsuBase + "GetVerifierString/" + n);
            }
            catch (WebException)
            {
                throw new WebFaultException<string>(
                    "ASU image verifier service is unavailable", HttpStatusCode.BadGateway);
            }

            raw = raw.Trim();
            if (raw.StartsWith("<"))                 // XML reply: <string>abc</string>
                raw = XDocument.Parse(raw).Root.Value;
            return raw.Trim('"');                    // JSON-style quotes
        }

        public Stream GetImage(string str)
        {
            if (string.IsNullOrEmpty(str) || str.Length > 20)
                throw new WebFaultException<string>(
                    "str must be 1 to 20 characters", HttpStatusCode.BadRequest);

            byte[] data;
            string contentType;
            try
            {
                using (var wc = new WebClient())
                {
                    data = wc.DownloadData(AsuBase + "GetImage/" + Uri.EscapeDataString(str));
                    contentType = wc.ResponseHeaders["Content-Type"] ?? "image/png";
                }
            }
            catch (WebException)
            {
                throw new WebFaultException<string>(
                    "ASU image verifier service is unavailable", HttpStatusCode.BadGateway);
            }

            WebOperationContext.Current.OutgoingResponse.ContentType = contentType;
            return new MemoryStream(data);
        }
    }
}