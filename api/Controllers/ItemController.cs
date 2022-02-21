using api.CustomExceptions;
using api.Dtos.ItemControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace api.Controllers
{
    [Route("api/[controller]")]
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

            if (item is null)
            {
                return NotFound();
            }

            return CreatedAtAction(nameof(CreateItem), item);
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchItemResponse>> FindItem(SearchItemRequest dto)
        {
            var items = await _itemService.FindItemAsync(dto);

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

        [HttpGet]
        public async Task<ActionResult<List<GetAllItemsResponse>>> GetAll()
        {
            var items = await _itemService.GetAllItemsAsync();

            return Ok(items);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UpdateItemResponse>> UpdateItem([FromForm] UpdateItemRequest request)
        {
            UpdateItemResponse? response;
            try
            {
                response = await _itemService.UpdateItemAsync(request);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException)
            {
                return BadRequest();
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
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
