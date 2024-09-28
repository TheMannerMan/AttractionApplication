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
    public class ReviewsController : ControllerBase
    {
        //private ILogger<csAttractionController> _logger = null;
        private IUserAttractionService _service = null;


        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IReview>))]
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

                var _users = await _service.ReadReviewsAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);

                return Ok(_users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IReview))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "true")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);

                var item = await _service.ReadReviewAsync(_id, _flat);
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
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                Guid _id = Guid.Parse(id);

                var _resp = await _service.DeleteReviewAsync(_id);

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
        [ProducesResponseType(200, Type = typeof(csReviewCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                var _id = Guid.Parse(id);

                var item = await _service.ReadReviewAsync(_id, false);
                if (item == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                var dto = new csReviewCUdto(item);
                return Ok(dto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ActionName("UpdateItem")] //
        [ProducesResponseType(200, Type = typeof(csReviewCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csReviewCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.ReviewId != _id)
                    throw new Exception("Id mismatch");

                var _item = await _service.UpdateReviewAsync(item);

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(IReview))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csReviewCUdto item)
        {
            try
            {
                var _item = await _service.CreateReviewAsync(item);
               

                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public ReviewsController(IUserAttractionService service)
        {
            _service = service;
        }
    }
}