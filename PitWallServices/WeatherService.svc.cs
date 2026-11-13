using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text.RegularExpressions;
using PitWallServices.Helpers;

namespace PitWallServices
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "WeatherService" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select WeatherService.svc or WeatherService.svc.cs at the Solution Explorer and start debugging.
    public class WeatherService : IWeatherService
    {

        // Web addresses of the two outside APIs this service uses.
        private const string GeocodeUrl = "https://geocoding-api.open-meteo.com/v1/search";
        private const string ForecastUrl = "https://api.open-meteo.com/v1/forecast";

        // This method takes a zipcode and returns the 5-day weather forecast for that zipcode.
        // Parameter: zipcode (string) = 5-digit U.S. zipcode, e.g. "78617".
        // Returns: string[] = 5 forecast lines, or 1 line starting with "Error:".
        public string[] Weather5day(string zipcode)
        {
            //check the input 
            if (string.IsNullOrWhiteSpace(zipcode) || !Regex.IsMatch(zipcode.Trim(), @"^\d{5}$"))
            {
                return new[] { "Error: Invalid zipcode. Please provide a 5-digit zipcode." };
            }
            zipcode = zipcode.Trim();

            try
            {
                // Get the latitude and longitude for the zipcode using the geocoding API
                double latitude, longitude;
                string place;
                if (!TryGetCoordinates(zipcode, out latitude, out longitude, out place))
                {
                    return new[] { "Error: Could not find coordinates for the provided zipcode." };
                }

                // Get the 5-day forecast for those coordinates
                return GetWeatherForecast(latitude, longitude, place);
            }
            catch (Exception ex)
            {
                // Network or data problem: return a message instead of crashing the service
                return new[] { "Error: Weather data is unavailable right now (" + ex.Message + ")" };
            }
        }

        //TryGetCoordinates turns zipcode into latitude and longitude using the geocoding API
        // Parameters: zipcode (string), out latitude (double), out longitude (double), out place (string).
        // Returns: bool = true if the zipcode was found.
        private bool TryGetCoordinates(string zipcode, out double latitude, out double longitude, out string place)
        {
            latitude = 0;
            longitude = 0;
            place = string.Empty;

            string url = $"{GeocodeUrl}?name={zipcode}&countryCode=US&count=5&language=en&format=json";
            var json = ApiHelper.GetJson(url) as Dictionary<string, object>;
            if (json != null && json.ContainsKey("results"))
            {
                // JavaScriptSerializer turns JSON arrays into object[] (not List<object>)
                var results = json["results"] as object[];
                if (results != null)
                {
                    foreach (object item in results)
                    {
                        var result = item as Dictionary<string, object>;
                        if (result == null || !result.ContainsKey("postcodes"))
                        {
                            continue;
                        }

                        // The API does fuzzy matching, so make sure this result really has our zipcode
                        var postcodes = result["postcodes"] as object[];
                        if (postcodes == null || Array.IndexOf(postcodes, zipcode) < 0)
                        {
                            continue;
                        }

                        latitude = Convert.ToDouble(result["latitude"], CultureInfo.InvariantCulture);
                        longitude = Convert.ToDouble(result["longitude"], CultureInfo.InvariantCulture);
                        place = result["name"].ToString();
                        if (result.ContainsKey("admin1"))
                        {
                            place += ", " + result["admin1"];
                        }
                        return true;
                    }
                }
            }
            return false;
        }

        //GetWeatherForecast gets the 5-day forecast for the given latitude and longitude
        // Parameters: latitude (double), longitude (double), place (string) = name shown in each line.
        // Returns: string[] = 5 formatted forecast lines.
        private string[] GetWeatherForecast(double latitude, double longitude, string place)
        {
            // InvariantCulture keeps the decimal point as "." in the URL
            string url = $"{ForecastUrl}?latitude={latitude.ToString(CultureInfo.InvariantCulture)}"
                       + $"&longitude={longitude.ToString(CultureInfo.InvariantCulture)}"
                       + "&daily=temperature_2m_max,temperature_2m_min,weather_code,precipitation_probability_max"
                       + "&temperature_unit=fahrenheit&timezone=auto&forecast_days=5";

            var json = ApiHelper.GetJson(url) as Dictionary<string, object>;
            var daily = json["daily"] as Dictionary<string, object>;

            // Each field is an array with one value per day
            var dates = (object[])daily["time"];
            var highs = (object[])daily["temperature_2m_max"];
            var lows = (object[])daily["temperature_2m_min"];
            var codes = (object[])daily["weather_code"];
            var rain = (object[])daily["precipitation_probability_max"];

            var forecast = new List<string>();
            for (int i = 0; i < 5 && i < dates.Length; i++)
            {
                string day = DateTime.ParseExact((string)dates[i], "yyyy-MM-dd", CultureInfo.InvariantCulture)
                                     .ToString("ddd, MMM d yyyy", CultureInfo.InvariantCulture);
                decimal high = Math.Round(Convert.ToDecimal(highs[i], CultureInfo.InvariantCulture));
                decimal low = Math.Round(Convert.ToDecimal(lows[i], CultureInfo.InvariantCulture));
                string sky = DescribeWeatherCode(Convert.ToInt32(codes[i], CultureInfo.InvariantCulture));
                string rainText = rain[i] != null ? $", {rain[i]}% chance of rain" : "";

                forecast.Add($"{day} ({place}): High {high}\u00B0F / Low {low}\u00B0F, {sky}{rainText}");
            }

            return forecast.ToArray();
        }

        // DescribeWeatherCode converts the numeric weather code from the API into a human-readable description
        // Parameter: code (int) = WMO weather code from Open-Meteo.
        // Returns: string = description such as "Partly cloudy".
        private string DescribeWeatherCode(int code)
        {
            if (code == 0) return "Clear sky";
            if (code == 1) return "Mainly clear";
            if (code == 2) return "Partly cloudy";
            if (code == 3) return "Overcast";
            if (code >= 45 && code <= 48) return "Fog and depositing rime fog";
            if (code >= 51 && code <= 57) return "Drizzle: Light, moderate, and dense intensity";
            if (code >= 61 && code <= 67) return "Rain: Slight, moderate and heavy intensity";
            if (code >= 71 && code <= 77) return "Snow: Slight, moderate, and heavy intensity";
            if (code >= 80 && code <= 82) return "Rain showers: Slight, moderate, and violent";
            if (code >= 85 && code <= 86) return "Snow showers slight and heavy";
            if (code >= 95 && code <= 99) return "Thunderstorm: Slight or moderate, with hail";
            return "Unknown weather condition";
        }


    }
}
