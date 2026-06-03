using Dymitro.Models.Domain;
using Dymitro.Models.DTOs;
using Mapster;

namespace Dymitro.Common
{
    public class MapsterConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<Salary, SalaryDto>();
            config.NewConfig<SalaryDto, Salary>();
        }
    }
}
