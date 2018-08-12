using CTeleportAPI.Repository;
using Nancy;

namespace CTeleportAPI
{
    public class HomeModule : NancyModule
    {
        public HomeModule(ICalculateDistance calculateDistance)
        {
            Get("/GetDistance", async args =>
            {
                this.Context.EnableOutputCache(300);
                return await calculateDistance.CalculateAirportDistance(this.Request.Query["source"],
                   this.Request.Query["destination"]);
            });
        }
    }
}
