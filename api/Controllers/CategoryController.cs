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
            try
            {
                var category = await _categoryService.CreateAsync(request);

                return CreatedAtAction(nameof(Create), category);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (DuplicateDataException)
            {
                return Conflict();
            }
        }

        [HttpGet("search/{searchKey}")]
        public async Task<ActionResult<SearchCategoryWithItemsResponse>> FindItem(string searchKey)
        {
            var categories = await _categoryService.FindCategoryAsync(searchKey);

            if (categories is null || categories.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
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

        [HttpGet("all")]
        public async Task<ActionResult<List<GetAllCategoriesResponse>>> GetAll()
        {
            // get category list dto, which contains category items
            var categories = await _categoryService.GetAllCategoriesAsync();

            if (categories is null || categories.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpGet("all/items")]
        public async Task<ActionResult<List<GetAllCategoriesWithItemsResponse>>> GetAllWithItems()
        {
            // get category list dto, which contains category items
            var categories = await _categoryService.GetAllCategoriesWithItemsAsync();

            if (categories is null || categories.Count == 0)
            {
                return NotFound();
            }

            return Ok(categories);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory(
            [FromForm] UpdateCategoryRequest request)
        {
            try
            {
                var response = await _categoryService.UpdateAsync(request);

                return Ok(response);
            }
            catch (ObjectNotFoundException)
            {
                return NotFound();
            }
            catch (DuplicateNameException)
            {
                return Conflict();
            }
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
