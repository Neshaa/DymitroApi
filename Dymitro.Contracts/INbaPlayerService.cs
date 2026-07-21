using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface INbaPlayerService
    {
        Task<IEnumerable<NbaPlayerDto>> GetNbaPlayersAsync();
        Task<bool> InsertNbaPlayerAsync(NbaPlayerDto player);
        Task<bool> UpdateNbaPlayerAsync(NbaPlayerDto player);
    }
}
