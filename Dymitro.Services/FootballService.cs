using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;
using System.Diagnostics.Metrics;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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

        public async Task<IEnumerable<FootballTeamDto>> GetFootballTeamsSuggestionsAsync(string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return Enumerable.Empty<FootballTeamDto>();

            const string sql = @"SELECT id, name, country, continent, active FROM public.footballteams
                                 WHERE name ILIKE @Search OR country ILIKE @Search
                                 ORDER BY name
                                 LIMIT 10";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<FootballTeam>(sql, new { Search = $"%{search}%" });

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

        public async Task<IEnumerable<WorldCupStatsDto>> GetWorldCupStatistics(int year)
        {
            List<WorldCupStatsDto> FinallResult = new List<WorldCupStatsDto>();

            int prevSeason = 0;

            if (year == 1952)
            {
                prevSeason = 1938;
            }
            else
            {
                prevSeason = year - 4;
            }

            var currentSeason = await GetStatsByYear(year);
            var previousSeason = await GetStatsByYear(prevSeason);

            var query = (from curr in currentSeason
                         join pre in previousSeason
                               on curr.Team.Id equals pre.Team.Id
                          into temp
                         from j in temp.DefaultIfEmpty(new WorldCupStatsDto())
                         select new WorldCupStatsDto
                         {
                             Team = new FootballTeamDto
                             {
                                 Id = curr.Team.Id,
                                 Name = curr.Team.Name,
                                 Country = curr.Team.Country,
                                 Active = curr.Team.Active
                             },

                             NoOfPlayers = curr.NoOfPlayers,
                             Position = curr.Position,
                             PositionMove = (j.Position == 0 ? 0 : j.Position - curr.Position)
                         }
            ).OrderByDescending(x => x.NoOfPlayers).ToList();

            foreach (var item in query)
            {
                FinallResult.Add(new WorldCupStatsDto
                {
                    Team = item.Team,
                    NoOfPlayers = item.NoOfPlayers,
                    Position = item.Position,
                    PositionMove = item.PositionMove
                });
            }

            return FinallResult;
        }

        public async Task<IEnumerable<WorldCupStatsDto>> GetWorldCupStatisticsByCountry(int year)
        {
            List<WorldCupStatsDto> FinallResult = new List<WorldCupStatsDto>();

            int prevSeason = 0;

            if (year == 1952)
            {
                prevSeason = 1938;
            }
            else
            {
                prevSeason = year - 4;
            }

            var currentSeason = await GetStatsByCountryByYear(year);
            var previousSeason = await GetStatsByCountryByYear(prevSeason);

            var query = (from curr in currentSeason
                         join pre in previousSeason
                               on curr.Team.Country equals pre.Team.Country
                          into temp
                         from j in temp.DefaultIfEmpty(new WorldCupStatsDto())
                         select new WorldCupStatsDto
                         {
                             Team = new FootballTeamDto
                             {
                                 Id = curr.Team.Id,
                                 Name = curr.Team.Name,
                                 Country = curr.Team.Country,
                             },

                             NoOfPlayers = curr.NoOfPlayers,
                             Position = curr.Position,
                             PositionMove = (j.Position == 0 ? 0 : j.Position - curr.Position)
                         }
                        ).OrderByDescending(x => x.NoOfPlayers).ToList();


            foreach (var item in query)
            {
                FinallResult.Add(new WorldCupStatsDto
                {
                    Team = item.Team,
                    NoOfPlayers = item.NoOfPlayers,
                    Position = item.Position,
                    PositionMove = item.PositionMove,
                });
            }

            return FinallResult;
        }

        public async Task<int> InsertData(WCTeamDto requset)
        {
            const string sqlSelect = @"SELECT id, teamid, year, noofplayers FROM public.worldcupplayers
                                       WHERE teamId = @teamId AND year = @year";

            using var connection = _context.CreateConnection();
            var dataFromBase = await connection.QueryAsync<WorldCupPlayers>(sqlSelect, new
            {
                teamId = requset.Team.Id,
                year = requset.Year
            });

            if (!dataFromBase.Any())
            {
                const string sqlIns = @"
                    INSERT INTO public.worldcupplayers (teamid, year, noofplayers)
                    VALUES (@teamId, @year, @noOfPlayers)";

                int rows = await connection.ExecuteAsync(sqlIns, new
                {
                    teamId = requset.Team.Id,
                    year = requset.Year,
                    noOfPlayers = requset.No
                });

                return rows;
            }
            else
            {
                const string sqlUpd = @"
                    UPDATE public.worldcupplayers
                    SET noofplayers = @noOfPlayers
                    WHERE teamId = @teamId AND year = @year";

                int rows = await connection.ExecuteAsync(sqlUpd, new
                {
                    teamId = requset.Team.Id,
                    year = requset.Year,
                    noOfPlayers = requset.No
                });

                return rows;
            }
        }

        #region Private
        private async Task<List<WorldCupStatsDto>> GetStatsByYear(int year)
        {
            List<WorldCupStatsDto> AllDto = new List<WorldCupStatsDto>();
            int i = 1;

            var sql = @"SELECT SUM(wc.noofplayers) as noofplayers, ft.Name, ft.Country, ft.Id as TeamId
                            FROM worldcupplayers wc	left join footballteams ft on wc.Teamid = ft.Id";

            var parameters = new DynamicParameters();

            sql += " WHERE wc.Year <= @Year";
            parameters.Add("Year", year);

            sql += " GROUP BY ft.Name, ft.Country, ft.Id ORDER BY 1 DESC, LENGTH(ft.Name) DESC, ft.Name ASC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<WorldCupStats>(sql, parameters);

            foreach (var item in result)
            {
                var pre = new FootballTeamDto
                {
                    Name = item.Name,
                    Country = item.Country,
                    Id = item.TeamId
                };

                AllDto.Add(new WorldCupStatsDto
                {
                    Position = i,
                    Team = pre,
                    NoOfPlayers = item.NoOfPlayers,
                    Year = year,
                });

                i++;
            }

            return AllDto;
        }

        private async Task<List<WorldCupStatsDto>> GetStatsByCountryByYear(int year)
        {
            List<WorldCupStatsDto> AllDto = new List<WorldCupStatsDto>();
            int i = 1;

            var sql = @"SELECT SUM(wc.noofplayers) as noofplayers, ft.Country
                            FROM worldcupplayers wc	left join footballteams ft on wc.Teamid = ft.Id";

            var parameters = new DynamicParameters();

            sql += " WHERE wc.Year <= @Year";
            parameters.Add("Year", year);

            sql += " GROUP BY ft.Country ORDER BY 1 DESC, LENGTH(ft.Country) DESC, ft.Country ASC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<WorldCupStats>(sql, parameters);

            foreach (var item in result)
            {
                AllDto.Add(new WorldCupStatsDto
                {
                    Position = i,
                    Team = new FootballTeamDto { Country = item.Country },
                    NoOfPlayers = item.NoOfPlayers
                    //Year = year,
                });

                i++;
            }

            return AllDto;
        }

        #endregion    
    }
}
