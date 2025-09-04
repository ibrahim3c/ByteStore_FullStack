using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Category;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class CategoriesController:BaseController
    {
        public CategoriesController(IServiceManager serviceManager) : base(serviceManager)
        {
            
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await serviceManager.CategoryService.GetAllCategoriesAsync();
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
        }

        // GET: api/categories/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            var result = await serviceManager.CategoryService.GetCategoryByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Errors);
        }

        // POST: api/categories
        [HttpPost]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto)
        {
            var result = await serviceManager.CategoryService.CreateCategoryAsync(categoryDto);

            // Since the service doesn't return the created entity with its new ID,
            // we can't use CreatedAtAction. A simple Ok or Created is used instead.
            return result.IsSuccess
                ? StatusCode(201) // Returns 201 Created
                : BadRequest(result.Errors);
        }

        // PUT: api/categories/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            var result = await serviceManager.CategoryService.UpdateCategoryAsync(id, categoryDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Errors);
        }

        // DELETE: api/categories/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await serviceManager.CategoryService.DeleteCategoryAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Errors);
        }

    }
}
