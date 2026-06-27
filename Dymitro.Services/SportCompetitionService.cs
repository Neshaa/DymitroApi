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
                VALUES (@Sport, @Host, @Year, @First, @Second, @Third, @Competition)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                competition.Sport,
                competition.Host,
                competition.Year,
                First = competition.First?.Country,
                Second = competition.Second?.Country,
                Third = competition.Third?.Country,
                competition.Competition
            });

            return rows > 0;
        }

        public async Task<IEnumerable<SportCompetitionDto>> GetSportCompetitionsAsync(string sport, string competition)
        {
            const string sql = @"
                SELECT sc.sport, sc.host, sc.year, sc.competition,
                       c1.country AS FirstCountry, c1.active AS FirstActive, c1.balkan AS FirstBalkan,
                       c2.country AS SecondCountry, c2.active AS SecondActive, c2.balkan AS SecondBalkan,
                       c3.country AS ThirdCountry, c3.active AS ThirdActive, c3.balkan AS ThirdBalkan
                FROM public.sportcompetitions sc
                LEFT JOIN public.sportcountries c1 ON sc.first = c1.country
                LEFT JOIN public.sportcountries c2 ON sc.second = c2.country
                LEFT JOIN public.sportcountries c3 ON sc.third = c3.country
                WHERE sc.sport ILIKE @Sport AND sc.competition ILIKE @Competition
                ORDER BY sc.year DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SportCompetitionDbResult>(sql, new
            {
                Sport = $"%{sport}%",
                Competition = $"%{competition}%"
            });

            return result.Select(MapToDto);
        }

        public async Task<IEnumerable<SportCompetitionStatsDto>> GetSportCompetitionStatsAsync(string sport, string competition)
        {
            const string sql = @"
                SELECT c.country, c.active, c.balkan,
                       SUM(CASE WHEN sc.first = c.country THEN 1 ELSE 0 END) AS FirstCount,
                       SUM(CASE WHEN sc.second = c.country THEN 1 ELSE 0 END) AS SecondCount,
                       SUM(CASE WHEN sc.third = c.country THEN 1 ELSE 0 END) AS ThirdCount,
                       SUM(CASE WHEN sc.first = c.country OR sc.second = c.country OR sc.third = c.country THEN 1 ELSE 0 END) AS Total
                FROM public.sportcountries c
                INNER JOIN public.sportcompetitions sc
                       ON sc.first = c.country OR sc.second = c.country OR sc.third = c.country
                WHERE sc.sport ILIKE @Sport AND sc.competition ILIKE @Competition
                GROUP BY c.country, c.active, c.balkan
                ORDER BY FirstCount DESC, SecondCount DESC, ThirdCount DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SportCompetitionStatsDbResult>(sql, new
            {
                Sport = $"%{sport}%",
                Competition = $"%{competition}%"
            });

            return result.Select(r => new SportCompetitionStatsDto
            {
                Name = new SportCountryDto
                {
                    Country = r.Country,
                    Active = r.Active,
                    Balkan = r.Balkan
                },
                FirstCount = r.FirstCount,
                SecondCount = r.SecondCount,
                ThirdCount = r.ThirdCount,
                Total = r.Total
            });
        }

        #region Private

        private static SportCompetitionDto MapToDto(SportCompetitionDbResult r) => new SportCompetitionDto
        {
            Sport = r.Sport,
            Host = r.Host,
            Year = r.Year,
            Competition = r.Competition,
            First = r.FirstCountry != null ? new SportCountryDto { Country = r.FirstCountry, Active = r.FirstActive, Balkan = r.FirstBalkan } : null,
            Second = r.SecondCountry != null ? new SportCountryDto { Country = r.SecondCountry, Active = r.SecondActive, Balkan = r.SecondBalkan } : null,
            Third = r.ThirdCountry != null ? new SportCountryDto { Country = r.ThirdCountry, Active = r.ThirdActive, Balkan = r.ThirdBalkan } : null
        };

        private class SportCompetitionDbResult
        {
            public string? Sport { get; set; }
            public int? Year { get; set; }
            public string? Host { get; set; }
            public string? Competition { get; set; }
            public string? FirstCountry { get; set; }
            public short FirstActive { get; set; }
            public short FirstBalkan { get; set; }
            public string? SecondCountry { get; set; }
            public short SecondActive { get; set; }
            public short SecondBalkan { get; set; }
            public string? ThirdCountry { get; set; }
            public short ThirdActive { get; set; }
            public short ThirdBalkan { get; set; }
        }

        private class SportCompetitionStatsDbResult
        {
            public string? Country { get; set; }
            public short Active { get; set; }
            public short Balkan { get; set; }
            public int FirstCount { get; set; }
            public int SecondCount { get; set; }
            public int ThirdCount { get; set; }
            public int Total { get; set; }
        }

        #endregion
    }
}
