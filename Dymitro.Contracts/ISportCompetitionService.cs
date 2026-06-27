using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface ISportCompetitionService
    {
        Task<bool> InsertSportCompetitionAsync(SportCompetitionDto competition);
        Task<IEnumerable<SportCompetitionDto>> GetSportCompetitionsAsync(string sport, string competition);
        Task<IEnumerable<SportCompetitionStatsDto>> GetSportCompetitionStatsAsync(string sport, string competition);
    }
}
