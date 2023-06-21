using api.CustomExceptions;
using api.Dtos.ItemControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
            if (item == null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(CreateItem), item);
        }

        [HttpGet]
        public async Task<ActionResult<GetItemsResponse>> GetItems([FromQuery] GetItemsParams request)
        {
            try
            {
                var items = await _itemService.GetItems(request);
                if (!items.Items?.Any() ?? false)
                {
                    return NotFound();
                }
                return items;
            }
            catch 
            {
                return BadRequest();
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetItemResponse>> GetItemById(int id, GetItemParams getParams)
        {
            getParams.Id = id;
            var item = await _itemService.GetItemByIdAsync(getParams);
            if (item == null)
            {
                return NotFound();
            }

            return Ok(item);
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

        [HttpDelete("{id:int}")]
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
