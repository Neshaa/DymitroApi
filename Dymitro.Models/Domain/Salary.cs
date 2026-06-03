using System;
using System.Collections.Generic;
using System.Text;

namespace Dymitro.Models.Domain
{
    public class Salary
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string Company { get; set; }
        public decimal? Taxes { get; set; }
        public decimal? Net { get; set; }
        public decimal? Gross { get; set; }
        public decimal? Course { get; set; }
        public decimal? NetInEuro { get; set; }
    }
}
