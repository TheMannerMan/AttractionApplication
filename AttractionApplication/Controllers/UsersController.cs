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
        private IUserAttractionService _service = null;
        ILogger<UsersController> _logger = null;


        //GET: api/csAdmin/Attractions
        [HttpGet()]
        [ActionName("Read")]
        [ProducesResponseType(200, Type = typeof(csRespPageDTO<IUser>))]
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

                var _users = await _service.ReadUsersAsync(_seeded, _flat, filter?.Trim()?.ToLower(), _pageNr, _pageSize);

                return Ok(_users);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }

        [HttpGet()]
        [ActionName("ReadItem")]
        [ProducesResponseType(200, Type = typeof(IUser))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> ReadItem(string id = null, string flat = "true")
        {
            try
            {
                var _id = Guid.Parse(id);
                bool _flat = bool.Parse(flat);

                var item = await _service.ReadUserAsync(_id, _flat);
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

        [HttpGet()]
        [ActionName("ReadItemDto")]
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        [ProducesResponseType(404, Type = typeof(string))]
        public async Task<IActionResult> ReadItemDto(string id = null)
        {
            try
            {
                var _id = Guid.Parse(id);

                var item = await _service.ReadUserAsync(_id, false);
                if (item == null)
                {
                    return BadRequest($"Item with id {id} does not exist");
                }

                var dto = new csUserCUdto(item);
                return Ok(dto);
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

                var _resp = await _service.DeleteUserAsync(_id);

                if (_resp == null)
                {
                    return BadRequest($"Item with {id} does not exist");
                }
                _logger.LogInformation($"item {_id} deleted");
                return Ok(_resp);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id}")]
        [ActionName("UpdateItem")] //
        [ProducesResponseType(200, Type = typeof(csUserCUdto))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> UpdateItem(string id, [FromBody] csUserCUdto item)
        {
            try
            {
                var _id = Guid.Parse(id);

                if (item.UserId != _id)
                    throw new Exception("Id mismatch");

                var _item = await _service.UpdateUserAsync(item);
                _logger.LogInformation($"item {_id} updated");
                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not update. Error {ex.Message}");
            }
        }

        [HttpPost()]
        [ActionName("CreateItem")]
        [ProducesResponseType(200, Type = typeof(IUser))]
        [ProducesResponseType(400, Type = typeof(string))]
        public async Task<IActionResult> CreateItem([FromBody] csUserCUdto item)
        {
            try
            {
                var _item = await _service.CreateUserAsync(item);

                _logger.LogInformation($"item {_item.UserId} created");
                return Ok(_item);
            }
            catch (Exception ex)
            {
                return BadRequest($"Could not create. Error {ex.Message}");
            }
        }

        public UsersController(IUserAttractionService service, ILogger<UsersController> logger)
        {
            _service = service;
            _logger = logger;
        }
    }
}