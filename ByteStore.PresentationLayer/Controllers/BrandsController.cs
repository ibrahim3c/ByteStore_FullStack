using ByteStore.Domain.Abstractions.Constants;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Brand;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    [Authorize]
    public class BrandsController:BaseController
    {
        public BrandsController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        // GET: api/brands
        [HttpGet]
        public async Task<IActionResult> GetAllBrands()
        {
            var result = await serviceManager.BrandService.GetAllBrandsAsync();
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/brands/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            var result = await serviceManager.BrandService.GetBrandByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/brands
        [HttpPost]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDto brandDto)
        {
            var result = await serviceManager.BrandService.CreateBrandAsync(brandDto);
            return result.IsSuccess ? CreatedAtAction(nameof(GetBrandById), new { id = brandDto.Id }, brandDto) : BadRequest(result.Error);
        }

        // PUT: api/brands/5
        [HttpPut("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDto brandDto)
        {
            var result = await serviceManager.BrandService.UpdateBrandAsync(id, brandDto);
            return result.IsSuccess ? NoContent() : BadRequest(result.Error);
        }

        // DELETE: api/brands/5
        [HttpDelete("{id}")]
        [Authorize(Roles = Roles.AdminRole)]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            var result = await serviceManager.BrandService.DeleteBrandAsync(id);
            return result.IsSuccess ? NoContent() : NotFound(result.Error);
        }

    }
}
