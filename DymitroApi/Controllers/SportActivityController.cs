using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace DymitroApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SportActivityController : Controller
    {
        private readonly ISportActivityService _sportActivityService;

        public SportActivityController(ISportActivityService sportActivityService)
        {
            _sportActivityService = sportActivityService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSportActivitiesAsync()
        {
            var result = await _sportActivityService.GetSportActivitiesAsync();
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> InsertSportActivityAsync([FromBody] SportActivityDto activity)
        {
            bool isSuccess = await _sportActivityService.InsertSportActivityAsync(activity);
            return isSuccess ? Ok() : BadRequest();
        }
    }
}
