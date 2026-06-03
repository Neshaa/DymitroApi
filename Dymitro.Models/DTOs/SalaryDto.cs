using System;
using System.Collections.Generic;
using System.Text;

namespace Dymitro.Models.DTOs
{
    public class SalaryDto
    {
        public string Company { get; set; }
        public DateTime Date { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Net { get; set; }
        public decimal? Gross { get; set; }
        public decimal? Course { get; set; }
        public decimal? NetInEuro { get; set; }
        public string MonthYear { get; set; }
    }
}
