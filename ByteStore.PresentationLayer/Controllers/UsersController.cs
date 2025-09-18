using ByteStore.Domain.Abstractions.Constants;
using ByteStore.PresentationLayer.Controllers;
using BytStore.Application.DTOs.Identity;
using BytStore.Application.IServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ByteStore.Presentation.Controllers
{
    [Authorize(Roles = Roles.AdminRole)]
    public class UsersController : BaseController
    {
        public UsersController(IServiceManager serviceManager) : base(serviceManager)
        {
        }

        // GET: api/users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var result = await serviceManager.UserService.GetAllUsersAsync();
            return result.IsSuccess ? Ok(result.Value) : BadRequest(result.Error);
        }

        // GET: api/users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var result = await serviceManager.UserService.GetUserByIdAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/users/email/{email}
        [HttpGet("email/{email}")]
        public async Task<IActionResult> GetUserByEmail(string email)
        {
            var result = await serviceManager.UserService.GetUserByEmailAsync(email);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/users/{id}/roles
        [HttpGet("{id}/roles")]
        public async Task<IActionResult> GetRolesOfUser(string id)
        {
            var result = await serviceManager.UserService.GetRolesOfUserAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // GET: api/users/{id}/roles/names
        [HttpGet("{id}/roles/names")]
        public async Task<IActionResult> GetRolesNameOfUser(string id)
        {
            var result = await serviceManager.UserService.GetRolesNameOfUserAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/users
        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] CreatedUserDto dto)
        {
            var result = await serviceManager.UserService.AddUserAsync(dto);
            return result.IsSuccess ? Ok("User created successfully") : BadRequest(result.Error);
        }

        // PUT: api/users
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] UpdateUserDto dto)
        {
            var result = await serviceManager.UserService.UpdateUserAsync(dto);
            return result.IsSuccess ? Ok("User updated successfully") : BadRequest(result.Error);
        }

        // DELETE: api/users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await serviceManager.UserService.DeleteUserAsync(id);
            return result.IsSuccess ? Ok("User deleted successfully") : BadRequest(result.Error);
        }

        // DELETE: api/users/email/{email}
        [HttpDelete("email/{email}")]
        public async Task<IActionResult> DeleteUserByEmail(string email)
        {
            var result = await serviceManager.UserService.DeleteUserByEmailAsync(email);
            return result.IsSuccess ? Ok("User deleted successfully") : BadRequest(result.Error);
        }

        // PUT: api/users/{id}/lock
        [HttpPut("{id}/lock")]
        public async Task<IActionResult> LockUnlock(string id)
        {
            var result = await serviceManager.UserService.LockUnLock(id);
            return result.IsSuccess ? Ok("User lock/unlock updated") : BadRequest(result.Error);
        }

        // GET: api/users/{id}/manage-roles
        [HttpGet("{id}/manage-roles")]
        public async Task<IActionResult> GetRolesForManaging(string id)
        {
            var result = await serviceManager.UserService.GetRolesForManagingAsync(id);
            return result.IsSuccess ? Ok(result.Value) : NotFound(result.Error);
        }

        // POST: api/users/manage-roles
        [HttpPost("manage-roles")]
        public async Task<IActionResult> ManageUserRoles([FromBody] ManageRolesDto dto)
        {
            var result = await serviceManager.UserService.ManageUserRolesAsync(dto);
            return result.IsSuccess ? Ok("User roles updated successfully") : BadRequest(result.Error);
        }
    }
}
