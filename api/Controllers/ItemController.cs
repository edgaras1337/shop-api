using api.CustomExceptions;
using api.Dtos.ItemControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/items")]
    [ApiController]
    public class ItemController : ControllerBase
    {
        private readonly IItemService _itemService;
        public ItemController(IItemService itemService)
        {
            _itemService = itemService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<CreateItemResponse>> CreateItem([FromForm] CreateItemRequest request)
        {
            var item = await _itemService.CreateItemAsync(request);

            // returns not found if category id is invalid
            if (item is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(CreateItem), item);
        }

        [HttpGet("search/{searchKey}")]
        public async Task<ActionResult<SearchItemResponse>> FindItem(string searchKey)
        {
            var items = await _itemService.FindItemAsync(searchKey);

            if (items is null || items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetItemResponse>> GetItemById(int id)
        {
            var item = await _itemService.GetItemByIdAsync(id);

            if (item is null)
            {
                return NotFound();
            }

            return Ok(item);
        }

        [HttpGet("all")]
        public async Task<ActionResult<List<GetAllItemsResponse>>> GetAll()
        {
            var items = await _itemService.GetAllItemsAsync();

            if (items is null || items.Count == 0)
            {
                return NotFound();
            }

            return Ok(items);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UpdateItemResponse>> UpdateItem([FromForm] UpdateItemRequest request)
        {
            try
            {
                var response = await _itemService.UpdateItemAsync(request);

                return Ok(response);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> DeleteItem(int id)
        {
            try
            {
                await _itemService.DeleteItemAsync(id);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            
            return NoContent();
        }

    }
}
