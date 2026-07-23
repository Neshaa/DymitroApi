using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface INbaPlayerService
    {
        Task<IEnumerable<NbaPlayerDto>> GetNbaPlayersAsync();
        Task<bool> InsertNbaPlayerAsync(NbaPlayerDto player);
        Task<bool> UpdateNbaPlayerAsync(NbaPlayerDto player);

        Task<IEnumerable<AllStatsDto>> GetPointsAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetReboundsAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetAsistsAsync(string season, bool balkan);
    }
}
