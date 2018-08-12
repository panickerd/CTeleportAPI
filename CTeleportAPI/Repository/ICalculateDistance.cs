using System.Threading.Tasks;

namespace CTeleportAPI.Repository
{
    public interface ICalculateDistance
    {
        Task<string> CalculateAirportDistance(string source, string destination);
    }
}
