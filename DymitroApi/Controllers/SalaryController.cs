using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dymitro.Contracts;
using Dymitro.Models.DTOs;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace DymitroApi.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class SalaryController : Controller
    {
        private ISalaryService _salaryService;

        public SalaryController(ISalaryService salaryService)
        {
            _salaryService = salaryService;
        }


        [HttpGet]
        public async Task<IActionResult> GetSalariesAsync()
        {
            var result = await _salaryService.GetSalariesAsync();

            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> AddNewSalaryAsync(SalaryDto salary)
        {
            bool isSuccess = await _salaryService.InsertSalaryAsync(salary);

            if (isSuccess)
            {
                ViewBag.HttpCode = 200;
                ModelState.Clear();
                return Ok();
            }
            else
            {
                ViewBag.HttpCode = 400;
                return Ok();
            }
        }
    }
}
