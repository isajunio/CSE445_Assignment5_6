using System.ServiceModel;
using System.ServiceModel.Web;

namespace PitStrategyServices
{
    // Gabriela Martinez - PitStrategyService (REST, JSON)
    [ServiceContract]
    public interface IPitStrategyService
    {
        
        [OperationContract]
        [WebGet(UriTemplate = "estimate?lap={averageLapSeconds}&laps={totalLaps}&stops={plannedPitStops}&loss={pitStopLossSeconds}",
                ResponseFormat = WebMessageFormat.Json)]
        double EstimateRaceTime(double averageLapSeconds, int totalLaps, int plannedPitStops, double pitStopLossSeconds);
    }
}