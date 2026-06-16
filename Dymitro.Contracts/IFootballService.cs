using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface IFootballService
    {
        Task<IEnumerable<FootballTeamDto>> GetFootballTeamsAsync(string? name, string? country, string? continent);
        Task<bool> CreateFootballTeamAsync(FootballTeamDto team);
        Task<bool> UpdateFootballTeamAsync(int id, FootballTeamDto team);
    }
}
