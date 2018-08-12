using CTeleportAPI.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using static System.Math;

namespace CTeleportAPI.Service
{
    public class AirportDistance : ICalculateDistance
    {
        public async Task<string> CalculateAirportDistance(string source, string destination)
        {
            const string AirportUrl = "https://places-dev.cteleport.com/airports/";
            double[] sourceCoordinate = await GetResponse(string.Concat(AirportUrl, source));
            double[] destinationCoordinate = await GetResponse(string.Concat(AirportUrl, destination));
            double distance = GetSphericalDistance(sourceCoordinate, destinationCoordinate);
            return distance.ToString();
        }

        private async Task<double[]> GetResponse(string url)
        {
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetStringAsync(new Uri(url));
                dynamic releases = JObject.Parse(response);
                return new double[] { releases.location.lon, releases.location.lat };
            }
        }

        private static double GetSphericalDistance(double[] sourceCoordinate, double[] destinationCoordinate)
        {
             return Acos(
                             Cos(sourceCoordinate[1] * (PI / 180)) *
                             Cos(sourceCoordinate[0] * (PI / 180)) *
                             Cos(destinationCoordinate[1] * (PI / 180)) *
                             Cos(destinationCoordinate[0] * (PI / 180))
                             +
                             Cos(sourceCoordinate[1] * (PI / 180)) *
                             Sin(sourceCoordinate[0] * (PI / 180)) *
                             Cos(destinationCoordinate[1] * (PI / 180)) *
                             Sin(destinationCoordinate[0] * (PI / 180))
                             +
                             Sin(sourceCoordinate[1] * (PI / 180)) *
                             Sin(destinationCoordinate[1] * (PI / 180))
                            ) * 3959;
        }
    }
}
