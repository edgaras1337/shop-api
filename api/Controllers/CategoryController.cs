using api.CustomExceptions;
using api.Dtos;
using api.Dtos.CategoryControllerDtos;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace api.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpPost("add")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<CreateCategoryResponse>> Create(CreateCategoryRequest request)
        {
            var category = await _categoryService.CreateAsync(request);

            if(category is null)
            {
                return Conflict();
            }

            return CreatedAtAction(nameof(Create), category);
        }

        [HttpGet("search")]
        public async Task<ActionResult<SearchCategoryWithItemsResponse>> FindItem(SearchCategoryWithItemsRequest dto)
        {
            var items = await _categoryService.FindCategoryAsync(dto);

            return Ok(items);
        }

        [HttpGet("{id}/items")]
        public async Task<ActionResult<GetCategoryWithItemsResponse>> GetCategoryWithItemsById(int id)
        {
            // get category dto, which contains category items
            var category = await _categoryService.GetCategoryWithItemsByIdAsync(id);

            if(category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<GetCategoryResponse>> GetCategoryById(int id)
        {
            // get category dto, which contains category items
            var category = await _categoryService.GetCategoryByIdAsync(id);

            if (category is null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetAllCategoriesResponse>>> GetAll()
        {
            // get category list dto, which contains category items
            var categories = await _categoryService.GetAllCategoriesAsync();

            return Ok(categories);
        }

        [HttpGet("items")]
        public async Task<ActionResult<List<GetAllCategoriesWithItemsResponse>>> GetAllWithItems()
        {
            // get category list dto, which contains category items
            var categories = await _categoryService.GetAllCategoriesWithItemsAsync();

            return Ok(categories);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory([FromForm] UpdateCategoryRequest request)
        {
            UpdateCategoryResponse? response;
            try
            {
                response = await _categoryService.UpdateAsync(request);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }

            return Ok(response);
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                await _categoryService.DeleteByIdAsync(id);
            }
            catch(ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (RelatedDataException)
            {
                return BadRequest();
            }

            return NoContent();
        }
    }
}
