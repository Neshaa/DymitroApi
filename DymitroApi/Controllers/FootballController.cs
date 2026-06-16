using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DymitroApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FootballController : Controller
    {
        private readonly IFootballService _footballService;

        public FootballController(IFootballService footballService)
        {
            _footballService = footballService;
        }

        [HttpGet]
        public async Task<IActionResult> GetFootballTeamsAsync(
            [FromQuery] string? name,
            [FromQuery] string? country,
            [FromQuery] string? continent)
        {
            var result = await _footballService.GetFootballTeamsAsync(name, country, continent);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> CreateFootballTeamAsync([FromBody] FootballTeamDto team)
        {
            bool isSuccess = await _footballService.CreateFootballTeamAsync(team);
            return isSuccess ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateFootballTeamAsync(int id, [FromBody] FootballTeamDto team)
        {
            bool isSuccess = await _footballService.UpdateFootballTeamAsync(id, team);
            return isSuccess ? Ok() : NotFound();
        }
    }
}
