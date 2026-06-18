using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dymitro.Models.DTOs
{
    public class WCTeamDto
    {
        public FootballTeamDto Team { get; set; }
        public int Id { get; set; }
        public int No { get; set; }
        public int Year { get; set; }

    }
}
