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
    }
}
