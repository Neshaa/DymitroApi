using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface INbaPlayerService
    {
        Task<IEnumerable<NbaPlayerDto>> GetNbaPlayersAsync();
        Task<bool> InsertNbaPlayerAsync(NbaPlayerDto player);
        Task<bool> UpdateNbaPlayerAsync(NbaPlayerDto player);
        Task<bool> InsertNbaResultAsync(NbaResultDto result);

        Task<IEnumerable<AllStatsDto>> GetPointsAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetReboundsAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetAsistsAsync(string season, bool balkan);

        Task<IEnumerable<AllStatsDto>> GetPointsByCountryAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetReboundsByCountryAsync(string season, bool balkan);
        Task<IEnumerable<AllStatsDto>> GetAsistsByCountryAsync(string season, bool balkan);
    }
}
