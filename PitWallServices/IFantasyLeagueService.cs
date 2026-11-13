using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace PitWallServices
{
    //fantasy league service, players pick the podium for a race and get points
    [ServiceContract]
    public interface IFantasyLeagueService
    {
        //adds a new player to the league, returns a message
        [OperationContract]
        string RegisterPlayer(string username);

        //saves a player's top 3 picks (car numbers) for a race, returns a message
        [OperationContract]
        string SubmitPicks(string username, string sessionKey, int p1, int p2, int p3);

        //scores everyone's picks for a race using the real results, returns a summary
        [OperationContract]
        string ScoreRace(string sessionKey);

        //gets all players sorted by points, highest first
        [OperationContract]
        string[] GetLeaderboard();

        //gets all the picks one player made and the points for each
        [OperationContract]
        string[] GetPlayerPicks(string username);
    }
}