using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dymitro.Models.DTOs
{
    public class WorldCupStatsDto
    {
        public FootballTeamDto Team { get; set; }
        public int? Year { get; set; }
        public int? NoOfPlayers { get; set; }
        public int Position { get; set; }
        public int PositionMove { get; set; }
    }
}
