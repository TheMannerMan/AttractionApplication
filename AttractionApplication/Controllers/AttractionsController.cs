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
    public class AttractionsController : ControllerBase
    {
        //loginUserSessionDto _usr = null;
        IUserAttractionService _service = null;
        /*
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true", string flat = "true",
            string filter = null, string pageNr = "0", string pageSize = "10")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);

                var _resp = await _service.ReadAttractionsAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        } */

        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IAttraction>))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> Read(string seeded = "true",
                                                string flat = "true",
                                                string category = null, 
                                                string attractionName = null, 
                                                string description = null,
                                                string city = null,
                                                string country = null, 
                                                string pageNr = "0", 
                                                string pageSize = "10")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);

                var _resp = await _service.ReadAttractionsAsync(_seeded, _flat, category?.Trim()?.ToLower(), 
                    attractionName?.Trim()?.ToLower(), description?.Trim()?.ToLower(), city?.Trim()?.ToLower(), 
                    country?.Trim()?.ToLower(), _pageNr, _pageSize);
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        public AttractionsController(IUserAttractionService service)
        {
            _service = service;
        }

    }
}