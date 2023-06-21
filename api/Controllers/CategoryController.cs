using api.CustomExceptions;
using api.Dtos;
using api.Dtos.CategoryControllerDtos;
using api.Models;
using api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace api.Controllers
{
    [Route("api/categories")]
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
        public async Task<ActionResult<CreateCategoryResponse>> Create([FromForm] CreateCategoryRequest request)
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

        [HttpGet("{id:int}")]
        public async Task<ActionResult<GetCategoryResponse>> GetCategoryByIdAsync([FromRoute] int id, [FromQuery] GetCategoryParams getParams)
        {
            getParams.Id = id;
            var response = await _categoryService.GetCategoryByIdAsync(getParams);
            if (response == null)
            {
                return NotFound();
            }

            return Ok(response);
        }

        [HttpGet]
        public async Task<ActionResult<List<GetCategoriesResponse>>> GetCategories([FromQuery] GetCategoriesParams getParams)
        {
            var response = await _categoryService.GetCategoriesAsync(getParams);
            return Ok(response);
        }

        [HttpPut("update")]
        [Authorize(Roles = "Administrator")]
        public async Task<ActionResult<UpdateCategoryResponse>> UpdateCategory([FromForm] UpdateCategoryRequest request)
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

        [HttpDelete("{id:int}")]
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
