using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DymitroApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class NBAController : Controller
    {
        private readonly INbaPlayerService _nbaPlayerService;

        public NBAController(INbaPlayerService nbaPlayerService)
        {
            _nbaPlayerService = nbaPlayerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetNbaPlayersAsync()
        {
            var result = await _nbaPlayerService.GetNbaPlayersAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> InsertNbaPlayerAsync([FromBody] NbaPlayerDto player)
        {
            bool isSuccess = await _nbaPlayerService.InsertNbaPlayerAsync(player);
            return isSuccess ? Ok() : BadRequest();
        }

        [HttpPut]
        public async Task<IActionResult> UpdateNbaPlayerAsync([FromBody] NbaPlayerDto player)
        {
            bool isSuccess = await _nbaPlayerService.UpdateNbaPlayerAsync(player);
            return isSuccess ? Ok() : BadRequest();
        }


        [HttpGet("GetPoints/{season}/{balkan}")]
        public async Task<IActionResult> GetPoints(string season, string balkan)
        {
            var result = await _nbaPlayerService.GetPointsAsync(season, balkan);
            return Ok(result);
        }

        [HttpGet("GetRebounds/{season}/{balkan}")]
        public async Task<IActionResult> GetRebounds(string season, string balkan)
        {
            var result = await _nbaPlayerService.GetReboundsAsync(season, balkan);
            return Ok(result);
        }

        [HttpGet("GetAsists/{season}/{balkan}")]
        public async Task<IActionResult> GetAsists(string season, string balkan)
        {
            var result = await _nbaPlayerService.GetAsistsAsync(season, balkan);
            return Ok(result);
        }
    }
}
