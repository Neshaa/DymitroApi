using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Dymitro.Services;
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

        [HttpGet("suggestions")]
        public async Task<IActionResult> GetFootballTeamsSuggestionsAsync([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
                return BadRequest("Search term is required.");

            var result = await _footballService.GetFootballTeamsSuggestionsAsync(search);
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

        [Route("Football/GetWorldCupStatistics/{year}")]
        [HttpGet]
        public async Task<IActionResult> GetWorldCupStatistics(int year)
        {
            return Ok(await _footballService.GetWorldCupStatistics(year));
        }

        [Route("Football/GetWorldCupStatisticsByCountry/{year}")]
        [HttpGet]
        public async Task<IActionResult> GetWorldCupStatisticsByCountry(int year)
        {
            return Ok(await _footballService.GetWorldCupStatisticsByCountry(year));
        }

        [Route("Football/InsertWorldCupPlayer")]
        [HttpPost]
        public async Task<IActionResult> InsertData([FromBody] WCTeamDto request)
        {
            return Ok(_footballService.InsertData(request));
        }
    }
}
