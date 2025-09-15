using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    public class RolesController : BaseController
    {
        public RolesController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        // GET: api/roles
        [HttpGet]
        public async Task<IActionResult> GetAllRoles()
        {
            var result = await serviceManager.RoleService.GetAllRolesAsync();
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // GET: api/roles/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetRoleById(string id)
        {
            var result = await serviceManager.RoleService.GetRoleByIdAsync(id);
            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value);
        }

        // POST: api/roles
        [HttpPost]
        public async Task<IActionResult> AddRole([FromBody] GetRoleDto roleDto)
        {
            var result = await serviceManager.RoleService.AddRoleAsync(roleDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // PUT: api/roles
        [HttpPut]
        public async Task<IActionResult> UpdateRole([FromBody] GetRoleDto roleDto)
        {
            var result = await serviceManager.RoleService.UpdateRoleAsync(roleDto);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }

        // DELETE: api/roles/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(string id)
        {
            var result = await serviceManager.RoleService.DeleteRoleAsync(id);
            if (!result.IsSuccess)
                return BadRequest(result.Error);

            return Ok(result);
        }
    }
}
