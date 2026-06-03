using Dymitro.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dymitro.Contracts
{
    public interface ISalaryService
    {
        Task<bool> InsertSalaryAsync(SalaryDto salary);

        Task<IEnumerable<SalaryListDto>> GetSalariesAsync();
    }
}
