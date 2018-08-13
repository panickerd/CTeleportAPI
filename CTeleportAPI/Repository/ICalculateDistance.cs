using System.Threading.Tasks;

namespace CTeleportAPI.Repository
{
    public interface ICalculateDistance
    {
        string CalculateAirportDistance(string source, string destination);
    }
}
