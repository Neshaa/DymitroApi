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

        public async Task<bool> CreateFootballTeamAsync(FootballTeamDto team)
        {
            const string sql = @"
                INSERT INTO public.footballteams (name, country, continent, active)
                VALUES (@Name, @Country, @Continent, @Active)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                team.Name,
                team.Country,
                team.Continent,
                team.Active
            });

            return rows > 0;
        }

        public async Task<bool> UpdateFootballTeamAsync(int id, FootballTeamDto team)
        {
            const string sql = @"
                UPDATE public.footballteams
                SET name = @Name,
                    country = @Country,
                    continent = @Continent,
                    active = @Active
                WHERE id = @Id";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                Id = id,
                team.Name,
                team.Country,
                team.Continent,
                team.Active
            });

            return rows > 0;
        }
    }
}
