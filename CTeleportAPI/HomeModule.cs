using CTeleportAPI.Repository;
using Nancy;

namespace CTeleportAPI
{
    public class HomeModule : NancyModule
    {
        public HomeModule(ICalculateDistance calculateDistance)
        {
            Get("/GetDistance", _ =>
            {
                this.Context.EnableOutputCache(30);
                return calculateDistance.CalculateAirportDistance(this.Request.Query["source"],
                   this.Request.Query["destination"]);
            });
        }
    }
}
