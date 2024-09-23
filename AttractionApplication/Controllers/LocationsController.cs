using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;
using Models.DTO;

namespace AttractionApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class LocationsController : ControllerBase
    {
        //private ILogger<csAttractionController> _logger = null;
        private IUserAttractionService _uService = null;


        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<ILocation>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            //_logger.LogInformation("Endpoint Attractions executed");
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);

                var _users = await _uService.ReadLocationsAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);

                return Ok(_users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        public LocationsController(IUserAttractionService service)
        {
            _uService = service;
        }
    }
}