using Dapper;
using Dymitro.Contracts;
using Dymitro.DAL.Dapper.Context;
using Dymitro.Models.DTOs;

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
    }
}
