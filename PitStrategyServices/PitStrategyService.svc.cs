using System;
using System.Net;
using System.ServiceModel.Web;

namespace PitStrategyServices
{
    // Gabriela Martinez - PitStrategyService
    // Total race time = (average lap time x number of laps) + (pit stops x time lost per stop)
    public class PitStrategyService : IPitStrategyService
    {
        public double EstimateRaceTime(double averageLapSeconds, int totalLaps, int plannedPitStops, double pitStopLossSeconds)
        {
            // check the inputs are realistic for an F1 race; bad input returns HTTP 400 with a message
            if (averageLapSeconds <= 0 || averageLapSeconds > 600)
                throw Bad("averageLapSeconds must be between 0 and 600.");
            if (totalLaps < 1 || totalLaps > 100)
                throw Bad("totalLaps must be between 1 and 100.");
            if (plannedPitStops < 0 || plannedPitStops > 10)
                throw Bad("plannedPitStops must be between 0 and 10.");
            if (pitStopLossSeconds < 0 || pitStopLossSeconds > 120)
                throw Bad("pitStopLossSeconds must be between 0 and 120.");

            double driving = averageLapSeconds * totalLaps;          // time spent racing
            double pitting = plannedPitStops * pitStopLossSeconds;   // time lost in the pit lane
            return Math.Round(driving + pitting, 3);
        }

        // builds a 400 Bad Request error with a readable message
        private static WebFaultException<string> Bad(string message)
        {
            return new WebFaultException<string>(message, HttpStatusCode.BadRequest);
        }
    }
}