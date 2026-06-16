using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;

namespace Dymitro.Services
{
    public class FootballService : IFootballService
    {
        private readonly DapperContext _context;

        public FootballService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<FootballTeamDto>> GetFootballTeamsAsync(string? name, string? country, string? continent)
        {
            var sql = "SELECT id, name, country, continent, active FROM public.footballteams where 1=1";

            var parameters = new DynamicParameters();

            if (!string.IsNullOrWhiteSpace(name))
            {
                sql += " AND name ILIKE @Name";
                parameters.Add("Name", $"%{name}%");
            }

            if (!string.IsNullOrWhiteSpace(country))
            {
                sql += " AND country ILIKE @Country";
                parameters.Add("Country", $"%{country}%");
            }

            if (!string.IsNullOrWhiteSpace(continent))
            {
                sql += " AND continent ILIKE @Continent";
                parameters.Add("Continent", $"%{continent}%");
            }

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<FootballTeam>(sql, parameters);

            return result.Select(t => new FootballTeamDto
            {
                Id = t.Id,
                Name = t.Name,
                Country = t.Country,
                Continent = t.Continent,
                Active = t.Active
            });
        }
    }
}
