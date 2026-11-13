using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace PitWallServices
{
  
    [ServiceContract]
    public interface IRaceDataService
    {

        //get race calendar for a given year
        [OperationContract]
        [WebGet(UriTemplate = "races/{year}", ResponseFormat = WebMessageFormat.Json)]
        List<RaceInfo> GetRaceCalendar(string year);

        //gets the drivers standinmg for a given year
        [OperationContract]
        [WebGet(UriTemplate = "drivers/{sessionKey}", ResponseFormat = WebMessageFormat.Json)]
        List<DriverInfo> GetDrivers(string sessionKey);

        //get the race results for a given race
        [OperationContract]
        [WebGet(UriTemplate = "results/{sessionKey}?top={top}", ResponseFormat = WebMessageFormat.Json)]
        List<RaceResult> GetRaceResults(string sessionKey, string top);

    }


    //race information for each race in the calendar
    [DataContract]
    public class RaceInfo
    {

        [DataMember] public int MeetingKey { get; set; }       // id of the race weekend
        [DataMember] public int RaceSessionKey { get; set; }   // id of the race itself 
        [DataMember] public string Name { get; set; }          // name of the race
        [DataMember] public string Circuit { get; set; }       // circuit name
        [DataMember] public string Country { get; set; }       // race country
        [DataMember] public string RaceStartUtc { get; set; }  // race start time in UTC
        [DataMember] public bool IsCompleted { get; set; }     // true if the race is over

    }

    //driver information for each driver in the standings
    [DataContract]
    public class DriverInfo
    {
        [DataMember] public int Number { get; set; }   // car number 
        [DataMember] public string FullName { get; set; }  // driver full name
        [DataMember] public string Team { get; set; }   // team name
        [DataMember] public string Acronym { get; set; }  // drivers name acronym
    }
    //race results for each race
    [DataContract]
    public class RaceResult
    {
        [DataMember] public int Position { get; set; }   // finishing position
        [DataMember] public int DriverNumber { get; set; }   // drivers car number
        [DataMember] public string DriverName { get; set; }  // driver full name
        [DataMember] public string Team { get; set; }   // team name
        [DataMember] public int Laps { get; set; }  // how many laps each driver completed
        [DataMember] public string GapToLeader { get; set; } // time gap to the leader
        [DataMember] public string Status { get; set; }  // status of the driver (finished, DNF, DNS, etc.)
    }
}