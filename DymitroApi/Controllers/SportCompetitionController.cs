using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DymitroApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SportCompetitionController : Controller
    {
        private readonly ISportCompetitionService _sportCompetitionService;

        public SportCompetitionController(ISportCompetitionService sportCompetitionService)
        {
            _sportCompetitionService = sportCompetitionService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSportCompetitionsAsync([FromQuery] string sport, [FromQuery] string competition)
        {
            var result = await _sportCompetitionService.GetSportCompetitionsAsync(sport, competition);
            return Ok(result);
        }

        [HttpGet("stats")]
        public async Task<IActionResult> GetSportCompetitionStatsAsync([FromQuery] string sport, [FromQuery] string competition)
        {
            var result = await _sportCompetitionService.GetSportCompetitionStatsAsync(sport, competition);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> InsertSportCompetitionAsync([FromBody] SportCompetitionDto competition)
        {
            bool isSuccess = await _sportCompetitionService.InsertSportCompetitionAsync(competition);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}
