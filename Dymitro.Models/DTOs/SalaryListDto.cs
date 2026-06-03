using System;
using System.Collections.Generic;
using System.Text;

namespace Dymitro.Models.DTOs
{
    public class SalaryListDto
    {
        public IEnumerable<SalaryDto> salariesviewmodels { get; set; }
        public IEnumerable<StatsByYear> statsByYear { get; set; }
        public decimal TotalRSDNet { get; set; }
        public decimal TotalEurNet { get; set; }

        public class StatsByYear
        {
            public string Year { get; set; }
            public decimal? TotalSalary { get; set; }
        }
    }
}
