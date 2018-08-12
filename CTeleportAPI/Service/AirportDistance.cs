using CTeleportAPI.Repository;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;

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
            return Math.Acos(
                             Math.Cos(sourceCoordinate[1] * (Math.PI / 180)) *
                             Math.Cos(sourceCoordinate[0] * (Math.PI / 180)) *
                             Math.Cos(destinationCoordinate[1] * (Math.PI / 180)) *
                             Math.Cos(destinationCoordinate[0] * (Math.PI / 180))
                             +
                             Math.Cos(sourceCoordinate[1] * (Math.PI / 180)) *
                             Math.Sin(sourceCoordinate[0] * (Math.PI / 180)) *
                             Math.Cos(destinationCoordinate[1] * (Math.PI / 180)) *
                             Math.Sin(destinationCoordinate[0] * (Math.PI / 180))
                             +
                             Math.Sin(sourceCoordinate[1] * (Math.PI / 180)) *
                             Math.Sin(destinationCoordinate[1] * (Math.PI / 180))
                            ) * 3959;
        }
    }
}
