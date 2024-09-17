using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

using Seido.Utilities.SeedGenerator;

using Models;
using Services;
using Configuration;

namespace AttractionApplication.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AnimalController : ControllerBase
    {

        private IAnimalService _service = null;

        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(List<IAnimal>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string count)
        {
            try
            {
                //_logger.LogInformation("Endpoint AfricanAnimals executed");
                int _count = int.Parse(count);


                List<IAnimal> animals = _service.Read(_count);
                //List<IAnimal> animals = new csAnimalsService1().AfricanAnimals(_count);


                return Ok(animals);
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
           
        }

        //GET: api/csAdmin/AfricanAnimals
        [HttpGet()]
        [ActionName("Seed")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Seed(string count)
        {
            try
            {
                //_logger.LogInformation("Endpoint Seed executed");
                int _count = int.Parse(count);


                _service.Seed(_count);


                return Ok("Seeded");
            }
            catch (Exception ex)
            {
                //_logger.LogError(ex.Message);
                return BadRequest(ex.Message);
            }
           
        }

        public AnimalController(IAnimalService service)
        {
            _service = service;
        }
    }
}