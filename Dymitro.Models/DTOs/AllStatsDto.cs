using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dymitro.Models.DTOs
{ 
    public class AllStatsDto
    {
        public NbaPlayerDto Player { get; set; }
        public string Season { get; set; }
        public int? Points { get; set; }
        public int PtsPosition { get; set; }
        public int? Rebounds { get; set; }
        public int RbnPosition { get; set; }
        public int? Asists { get; set; }
        public int AstPosition { get; set; }
        public int PositionMove { get; set; }
    }
}
