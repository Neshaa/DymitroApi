using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.DTOs;

namespace Dymitro.Services
{
    public class SportActivityService : ISportActivityService
    {
        private readonly DapperContext _context;

        public SportActivityService(DapperContext context)
        {
            _context = context;
        }

        public async Task<bool> InsertSportActivityAsync(SportActivityDto activity)
        {
            const string sql = @"
                INSERT INTO public.sportactivities (name, ddate, duration, distance, elevation_gain, elevation_loss, avg_speed, max_speed, moving_time)
                VALUES (@Name, @Ddate, @Duration, @Distance, @ElevationGain, @ElevationLoss, @AvgSpeed, @MaxSpeed, @MovingTime)";

            using var connection = _context.CreateConnection();
            int rows = await connection.ExecuteAsync(sql, new
            {
                activity.Name,
                activity.Ddate,
                activity.Duration,
                activity.Distance,
                activity.ElevationGain,
                activity.ElevationLoss,
                activity.AvgSpeed,
                activity.MaxSpeed,
                activity.MovingTime
            });

            return rows > 0;
        }

        public async Task<IEnumerable<SportActivityDto>> GetSportActivitiesAsync()
        {
            const string sql = @"
                SELECT id, name, ddate, duration, distance, elevation_gain AS ElevationGain, elevation_loss AS ElevationLoss,
                       avg_speed AS AvgSpeed, max_speed AS MaxSpeed, moving_time AS MovingTime
                FROM public.sportactivities
                ORDER BY ddate DESC";

            using var connection = _context.CreateConnection();
            var result = await connection.QueryAsync<SportActivityDto>(sql);

            return result;
        }
    }
}
