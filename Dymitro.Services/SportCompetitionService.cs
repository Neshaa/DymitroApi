using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;

namespace Dymitro.Services
{
    public class SportCompetitionService : ISportCompetitionService
    {
        private readonly DapperContext _context;

        public SportCompetitionService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertSportCompetitionAsync(SportCompetitionDto competition)
        {
            const string sql = @"
                INSERT INTO public.sportcompetitions (sport, host, year, first, second, third, competition)
                VALUES (@Sport, @Host,@Year, @First, @Second, @Third, @Competition)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                competition.Sport,
                competition.Host,
                competition.Year,
                competition.First,
                competition.Second,
                competition.Third,
                competition.Competition
            });

            return rows > 0;
        }

        public async Task<IEnumerable<SportCompetitionDto>> GetSportCompetitionsAsync(string sport, string competition)
        {
            const string sql = @"
                SELECT sport, host, year, first, second, third, competition
                FROM public.sportcompetitions
                WHERE sport ILIKE @Sport AND competition ILIKE @Competition
                ORDER BY year DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SportCompetition>(sql, new
            {
                Sport = $"%{sport}%",
                Competition = $"%{competition}%"
            });

            return result.Select(r => new SportCompetitionDto
            {
                Sport = r.Sport,
                Host = r.Host,
                Year = r.Year,
                First = r.First,
                Second = r.Second,
                Third = r.Third,
                Competition = r.Competition
            });
        }

        public async Task<IEnumerable<SportCompetitionStatsDto>> GetSportCompetitionStatsAsync(string sport, string competition)
        {
            const string sql = @"
                SELECT name,
                       SUM(CASE WHEN first = name THEN 1 ELSE 0 END) AS FirstCount,
                       SUM(CASE WHEN second = name THEN 1 ELSE 0 END) AS SecondCount,
                       SUM(CASE WHEN third = name THEN 1 ELSE 0 END) AS ThirdCount,
                       SUM(CASE WHEN first = name OR second = name OR third = name THEN 1 ELSE 0 END) AS Total
                FROM public.sportcompetitions
                CROSS JOIN LATERAL (VALUES (first), (second), (third)) AS t(name)
                WHERE sport ILIKE @Sport AND competition ILIKE @Competition
                GROUP BY name
                ORDER BY FirstCount DESC, SecondCount DESC, ThirdCount DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SportCompetitionStatsDto>(sql, new
            {
                Sport = $"%{sport}%",
                Competition = $"%{competition}%"
            });

            return result;
        }
    }
}
