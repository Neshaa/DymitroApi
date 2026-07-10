using Dymitro.Models.DTOs;

namespace Dymitro.Contracts
{
    public interface ISportActivityService
    {
        Task<bool> InsertSportActivityAsync(SportActivityDto activity);
        Task<IEnumerable<SportActivityDto>> GetSportActivitiesAsync();
    }
}
