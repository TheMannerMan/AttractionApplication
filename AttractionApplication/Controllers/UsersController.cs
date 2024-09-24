using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;
using Models.DTO;

namespace AttractionApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        //private ILogger<csAttractionController> _logger = null;
        private IUserAttractionService _uService = null;


        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IUser>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "false",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            //_logger.LogInformation("Endpoint Attractions executed");
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);

                var _users = await _uService.ReadUsersAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);

                return Ok(_users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public UsersController(IUserAttractionService service)
        {
            _uService = service;
        }
    }
}