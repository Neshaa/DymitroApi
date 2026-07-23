using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;
using System.Diagnostics.Metrics;

namespace Dymitro.Services
{
    public class NbaPlayerService : INbaPlayerService
    {
        private readonly DapperContext _context;

        public NbaPlayerService(DapperContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<NbaPlayerDto>> GetNbaPlayersAsync()
        {
            const string sql = @"
                SELECT id, firstname AS FirstName, lastname AS LastName, country, active, balkan
                FROM public.nbaplayers
                ORDER BY lastname, firstname";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<NbaPlayerDto>(sql);

            return result;
        }

        public async Task<bool> InsertNbaPlayerAsync(NbaPlayerDto player)
        {
            const string sql = @"
                INSERT INTO public.nbaplayers (firstname, lastname, country, active, balkan)
                VALUES (@FirstName, @LastName, @Country, @Active, @Balkan)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                player.FirstName,
                player.LastName,
                player.Country,
                player.Active,
                player.Balkan
            });

            return rows > 0;
        }

        public async Task<bool> UpdateNbaPlayerAsync(NbaPlayerDto player)
        {
            const string sql = @"
                UPDATE public.nbaplayers
                SET firstname = @FirstName, lastname = @LastName, country = @Country, active = @Active, balkan = @Balkan
                WHERE id = @Id";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                player.Id,
                player.FirstName,
                player.LastName,
                player.Country,
                player.Active,
                player.Balkan
            });

            return rows > 0;
        }

        public async Task<bool> InsertNbaResultAsync(NbaResultDto result)
        {
            const string sql = @"
                INSERT INTO public.nbaresults (playerid, season, points, rebounds, asists)
                VALUES (@PlayerId, @Season, @Points, @Rebounds, @Asists)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                result.PlayerId,
                result.Season,
                result.Points,
                result.Rebounds,
                result.Asists
            });

            return rows > 0;
        }

        public async Task<IEnumerable<AllStatsDto>> GetPointsAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetPointsBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetPointsBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetPointsBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Id equals prev.Player.Id into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Points = curr.Points,
                        PtsPosition = curr.PtsPosition,
                        PositionMove = prevMatch == null || prevMatch.PtsPosition == 0 ? 0 : prevMatch.PtsPosition - curr.PtsPosition
                    }).OrderByDescending(x => x.Points).ToList();
        }

        public async Task<IEnumerable<AllStatsDto>> GetReboundsAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetReboundsBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetReboundsBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetReboundsBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Id equals prev.Player.Id into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Rebounds = curr.Rebounds,
                        RbnPosition = curr.RbnPosition,
                        PositionMove = prevMatch == null || prevMatch.RbnPosition == 0 ? 0 : prevMatch.RbnPosition - curr.RbnPosition
                    }).OrderByDescending(x => x.Rebounds).ToList();
        }

        public async Task<IEnumerable<AllStatsDto>> GetAsistsAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetAsistsBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetAsistsBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetAsistsBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Id equals prev.Player.Id into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Asists = curr.Asists,
                        AstPosition = curr.AstPosition,
                        PositionMove = prevMatch == null || prevMatch.AstPosition == 0 ? 0 : prevMatch.AstPosition - curr.AstPosition
                    }).OrderByDescending(x => x.Asists).ToList();
        }

        public async Task<IEnumerable<AllStatsDto>> GetPointsByCountryAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetPointsByCountryBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetPointsByCountryBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetPointsByCountryBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Country equals prev.Player.Country into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Points = curr.Points,
                        PtsPosition = curr.PtsPosition,
                        PositionMove = prevMatch == null || prevMatch.PtsPosition == 0 ? 0 : prevMatch.PtsPosition - curr.PtsPosition
                    }).OrderByDescending(x => x.Points).ToList();
        }

        public async Task<IEnumerable<AllStatsDto>> GetReboundsByCountryAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetReboundsByCountryBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetReboundsByCountryBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetReboundsByCountryBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Country equals prev.Player.Country into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Rebounds = curr.Rebounds,
                        RbnPosition = curr.RbnPosition,
                        PositionMove = prevMatch == null || prevMatch.RbnPosition == 0 ? 0 : prevMatch.RbnPosition - curr.RbnPosition
                    }).OrderByDescending(x => x.Rebounds).ToList();
        }

        public async Task<IEnumerable<AllStatsDto>> GetAsistsByCountryAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                return await GetAsistsByCountryBySeasonAsync(season, balkan);
            }

            string previousSeason = GetPreviousSeason(season);

            var currentSeasonStats = await GetAsistsByCountryBySeasonAsync(season, balkan);
            var previousSeasonStats = await GetAsistsByCountryBySeasonAsync(previousSeason, balkan);

            return (from curr in currentSeasonStats
                    join prev in previousSeasonStats on curr.Player.Country equals prev.Player.Country into temp
                    from prevMatch in temp.DefaultIfEmpty()
                    select new AllStatsDto
                    {
                        Player = curr.Player,
                        Asists = curr.Asists,
                        AstPosition = curr.AstPosition,
                        PositionMove = prevMatch == null || prevMatch.AstPosition == 0 ? 0 : prevMatch.AstPosition - curr.AstPosition
                    }).OrderByDescending(x => x.Asists).ToList();
        }

        #region Private

        private static List<short> GetBalkanFilter(bool balkan) =>
            balkan == true ? new List<short> { 1 } : new List<short> { 0, 1, 77 };

        private static string GetPreviousSeason(string currentSeason) =>
            currentSeason == "2010" ? "Previous" : (Convert.ToInt32(currentSeason) - 1).ToString();

        private async Task<List<AllStatsDto>> GetPointsBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetSeasonRowsAsync(season, balkan, "r.points");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.Points,
                    Rebounds = r.Rebounds,
                    Asists = r.Asists,
                    Season = season,
                    PtsPosition = position++
                }).ToList();
            }

            var sums = await GetMetricSumsAsync(season, balkan, "r.points");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.Player,
                Points = s.MetricSum,
                PtsPosition = pos++
            }).ToList();
        }

        private async Task<List<AllStatsDto>> GetReboundsBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetSeasonRowsAsync(season, balkan, "r.rebounds");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.Points,
                    Rebounds = r.Rebounds,
                    Asists = r.Asists,
                    Season = season,
                    RbnPosition = position++
                }).ToList();
            }

            var sums = await GetMetricSumsAsync(season, balkan, "r.rebounds");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.Player,
                Rebounds = s.MetricSum,
                RbnPosition = pos++
            }).ToList();
        }

        private async Task<List<AllStatsDto>> GetAsistsBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetSeasonRowsAsync(season, balkan, "r.asists");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.Points,
                    Rebounds = r.Rebounds,
                    Asists = r.Asists,
                    Season = season,
                    AstPosition = position++
                }).ToList();
            }

            var sums = await GetMetricSumsAsync(season, balkan, "r.asists");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.Player,
                Asists = s.MetricSum,
                AstPosition = pos++
            }).ToList();
        }

        private async Task<IEnumerable<PlayerSeasonRow>> GetSeasonRowsAsync(string season, bool balkan, string orderColumn)
        {
            var counteri = GetBalkanFilter(balkan);

            string sql = $@"
                SELECT r.points AS Points, r.rebounds AS Rebounds, r.asists AS Asists,
                       p.id AS Id, p.firstname AS FirstName, p.lastname AS LastName, p.country AS Country, p.active AS Active, p.balkan AS Balkan
                FROM public.nbaresults r
                JOIN public.nbaplayers p ON r.playerid = p.id
                WHERE r.season = @Season AND p.balkan = ANY(@Counteri)
                ORDER BY {orderColumn} DESC, p.id ASC";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<PlayerSeasonRow>(sql, new { Season = season, Counteri = counteri });
        }

        private async Task<List<PlayerMetricSum>> GetMetricSumsAsync(string season, bool balkan, string metricColumn)
        {
            var counteri = GetBalkanFilter(balkan);

            string sql = $@"
                SELECT r.playerid AS PlayerId, SUM({metricColumn}) AS MetricSum
                FROM public.nbaresults r
                JOIN public.nbaplayers p ON r.playerid = p.id
                WHERE (r.season = 'Previous' OR r.season <= @Season) AND p.balkan = ANY(@Counteri)
                GROUP BY r.playerid
                ORDER BY MetricSum DESC";

            using var connection = _context.CreateConnection();
            var sums = await connection.QueryAsync<PlayerMetricSumRow>(sql, new { Season = season, Counteri = counteri });

            var players = (await GetNbaPlayersAsync()).ToDictionary(p => p.Id);

            return sums
                .Where(s => players.ContainsKey(s.PlayerId))
                .Select(s => new PlayerMetricSum { Player = players[s.PlayerId], MetricSum = s.MetricSum })
                .ToList();
        }

        private async Task<List<AllStatsDto>> GetPointsByCountryBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetCountrySeasonRowsAsync(season, balkan, "TotalPoints DESC, TotalRebounds DESC, TotalAsists DESC");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.TotalPoints,
                    Rebounds = r.TotalRebounds,
                    Asists = r.TotalAsists,
                    Season = season,
                    PtsPosition = position++
                }).ToList();
            }

            var sums = await GetCountryMetricSumsAsync(season, balkan, "r.points");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.ToPlayerDto(),
                Points = s.MetricSum,
                PtsPosition = pos++
            }).ToList();
        }

        private async Task<List<AllStatsDto>> GetReboundsByCountryBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetCountrySeasonRowsAsync(season, balkan, "TotalRebounds DESC, TotalPoints DESC, TotalAsists DESC");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.TotalPoints,
                    Rebounds = r.TotalRebounds,
                    Asists = r.TotalAsists,
                    Season = season,
                    RbnPosition = position++
                }).ToList();
            }

            var sums = await GetCountryMetricSumsAsync(season, balkan, "r.rebounds");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.ToPlayerDto(),
                Rebounds = s.MetricSum,
                RbnPosition = pos++
            }).ToList();
        }

        private async Task<List<AllStatsDto>> GetAsistsByCountryBySeasonAsync(string season, bool balkan)
        {
            if (season == "Previous")
            {
                var rows = await GetCountrySeasonRowsAsync(season, balkan, "TotalAsists DESC, TotalPoints DESC, TotalRebounds DESC");
                int position = 1;
                return rows.Select(r => new AllStatsDto
                {
                    Player = r.ToPlayerDto(),
                    Points = r.TotalPoints,
                    Rebounds = r.TotalRebounds,
                    Asists = r.TotalAsists,
                    Season = season,
                    AstPosition = position++
                }).ToList();
            }

            var sums = await GetCountryMetricSumsAsync(season, balkan, "r.asists");
            int pos = 1;
            return sums.Select(s => new AllStatsDto
            {
                Player = s.ToPlayerDto(),
                Asists = s.MetricSum,
                AstPosition = pos++
            }).ToList();
        }

        private async Task<IEnumerable<CountrySeasonRow>> GetCountrySeasonRowsAsync(string season, bool balkan, string orderByClause)
        {
            var counteri = GetBalkanFilter(balkan);

            string sql = $@"
                SELECT p.country AS Country, p.balkan AS Balkan,
                       SUM(r.points) AS TotalPoints, SUM(r.rebounds) AS TotalRebounds, SUM(r.asists) AS TotalAsists
                FROM public.nbaresults r
                JOIN public.nbaplayers p ON r.playerid = p.id
                WHERE r.season = @Season AND p.balkan = ANY(@Counteri)
                GROUP BY p.country, p.balkan
                ORDER BY {orderByClause}";

            using var connection = _context.CreateConnection();
            return await connection.QueryAsync<CountrySeasonRow>(sql, new { Season = season, Counteri = counteri });
        }

        private async Task<List<CountryMetricSumRow>> GetCountryMetricSumsAsync(string season, bool balkan, string metricColumn)
        {
            var counteri = GetBalkanFilter(balkan);

            string sql = $@"
                SELECT p.country AS Country, p.balkan AS Balkan, SUM({metricColumn}) AS MetricSum
                FROM public.nbaresults r
                JOIN public.nbaplayers p ON r.playerid = p.id
                WHERE (r.season = 'Previous' OR r.season <= @Season) AND p.balkan = ANY(@Counteri)
                GROUP BY p.country, p.balkan
                ORDER BY MetricSum DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<CountryMetricSumRow>(sql, new { Season = season, Counteri = counteri });

            return result.ToList();
        }

        private class CountrySeasonRow
        {
            public string? Country { get; set; }
            public short? Balkan { get; set; }
            public int? TotalPoints { get; set; }
            public int? TotalRebounds { get; set; }
            public int? TotalAsists { get; set; }

            public NbaPlayerDto ToPlayerDto() => new NbaPlayerDto
            {
                Country = Country,
                Balkan = Balkan,
                Active = 1
            };
        }

        private class CountryMetricSumRow
        {
            public string? Country { get; set; }
            public short? Balkan { get; set; }
            public int? MetricSum { get; set; }

            public NbaPlayerDto ToPlayerDto() => new NbaPlayerDto
            {
                Country = Country,
                Balkan = Balkan,
                Active = 1
            };
        }

        private class PlayerSeasonRow
        {
            public int? Points { get; set; }
            public int? Rebounds { get; set; }
            public int? Asists { get; set; }
            public int Id { get; set; }
            public string? FirstName { get; set; }
            public string? LastName { get; set; }
            public string? Country { get; set; }
            public short? Active { get; set; }
            public short? Balkan { get; set; }

            public NbaPlayerDto ToPlayerDto() => new NbaPlayerDto
            {
                Id = Id,
                FirstName = FirstName,
                LastName = LastName,
                Country = Country,
                Active = Active,
                Balkan = Balkan
            };
        }

        private class PlayerMetricSumRow
        {
            public int PlayerId { get; set; }
            public int? MetricSum { get; set; }
        }

        private class PlayerMetricSum
        {
            public NbaPlayerDto Player { get; set; } = null!;
            public int? MetricSum { get; set; }
        }

        #endregion
    }
}
