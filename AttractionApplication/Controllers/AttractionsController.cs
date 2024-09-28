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
                                                string showOnlyWithNoComments = "false",
                                                string pageNr = "0",
                                                string pageSize = "10")
        {
            try
            {
                bool _seeded = bool.Parse(seeded);
                bool _flat = bool.Parse(flat);
                bool _noComments = bool.Parse(showOnlyWithNoComments);
                int _pageNr = int.Parse(pageNr);
                int _pageSize = int.Parse(pageSize);

                var _resp = await _service.ReadAttractionsAsync(_seeded, _flat, category?.Trim()?.ToLower(),
                    attractionName?.Trim()?.ToLower(), description?.Trim()?.ToLower(), city?.Trim()?.ToLower(),
                    country?.Trim()?.ToLower(), _pageNr, _pageSize, _noComments);
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "true")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);

                var item = await _service.ReadAttractionAsync(_id, _flat);
                if (item == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                return Ok(item);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete("{id}")]
        [ActionName("DeleteItem")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> DeleteITem(string id)
        {
            try
            {
                Guid _id = Guid.Parse(id);

                var _resp = await _service.DeleteAttractionAsync(_id);

                if (_resp == null)
                {
                    return BadRequest($"Item with {id} does not exist");
                }

                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                var _id = Guid.Parse(id);

                var item = await _service.ReadAttractionAsync(_id, false);
                if (item == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                var dto = new csAttractionCUdto(item);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ActionName("UpdateItem")] //
        [ProducesResponseType(200, Type = typeof(csAttractionCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csAttractionCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.AttractionId != _id)
                    throw new Exception("Id mismatch");

                var _item = await _service.UpdateAttractionAsync(item);

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(IAttraction))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csAttractionCUdto item)
        {
            try
            {
                var _item = await _service.CreateAttractionAsync(item);
               

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public AttractionsController(IUserAttractionService service)
        {
            _service = service;
        }

    }
}