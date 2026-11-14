using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;

namespace PitWallServices
{
    [ServiceContract]
    public interface ICaptchaService
    {
        // Returns random alphanumeric string of requested length
        // Example: .../Service.svc/GetVerifierString/5
        [OperationContract]
        [WebGet(UriTemplate = "GetVerifierString/{length}", ResponseFormat = WebMessageFormat.Json)]
        string GetVerifierString(string length);

        // Returns CAPTCHA that with given string
        // Example: .../Service.svc/GetImage/myString
        [OperationContract]
        [WebGet(UriTemplate = "GetImage/{str}")]
        Stream GetImage(string str);
    }
}
