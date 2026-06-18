using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface IFootballService
    {
        Task<IEnumerable<FootballTeamDto>> GetFootballTeamsAsync(string? name, string? country, string? continent);
        Task<IEnumerable<FootballTeamDto>> GetFootballTeamsSuggestionsAsync(string search);
        Task<bool> CreateFootballTeamAsync(FootballTeamDto team);
        Task<bool> UpdateFootballTeamAsync(int id, FootballTeamDto team);
        Task<IEnumerable<WorldCupStatsDto>> GetWorldCupStatistics(int year);

        Task<IEnumerable<WorldCupStatsDto>> GetWorldCupStatisticsByCountry(int year);

        Task<int> InsertData(WCTeamDto requset);
    }
}
