using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models.DTO;


namespace AttractionApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AdminController : ControllerBase
    {
        IUserAttractionService _userAttractionService = null;
        ILogger<AdminController> _logger;

        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(adminInfoDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed()
        {
            try
            {
                adminInfoDbDto _info = await _userAttractionService.SeedAsync();
                return Ok(_info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("RemoveSeed")]
        [ProducesResponseType(200, Type = typeof(adminInfoDbDto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> RemoveSeed(string seeded = "true")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);

                adminInfoDbDto _info = await _userAttractionService.RemoveSeedAsync(_seeded);
                return Ok(_info);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public AdminController(IUserAttractionService service, ILogger<AdminController> logger)
        {
            _userAttractionService = service;
            _logger = logger;

        }
    }
}