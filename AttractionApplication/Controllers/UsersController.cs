using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Services;
using Models;

namespace AttractionApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UsersController : ControllerBase
    {
        //private ILogger<csAttractionController> _logger = null;
        private IUserService _uService = null;

        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Users")]
        [ProducesResponseType(200, Type = typeof(List<IUser>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Attractions(string count = "5")
        {
            //_logger.LogInformation("Endpoint Attractions executed");
           try
            {
                int _count = int.Parse(count);

                var users = _uService.ReadUsersAsync(_count);

                return Ok(users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }

         [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(string count)
        {
            //_logger.LogInformation("Endpoint Attractions executed");
           try
            {
                // _logger.LogInformation("Endpoint Seed executed");
                int _count = int.Parse(count);

                var users = _uService.ReadUsersAsync(_count);

                return Ok("Seeded");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
           
        }


        public UsersController(IUserService service)
        {
            _uService = service;
        }
    }
}