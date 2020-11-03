using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule.Enums;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.Api.UsersModule
{
    [ApiController]
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedUserModel), 200)]
        public async Task<IActionResult> AuthenticateAsync(UserAuthenticateCommand command)
        {
            var user = await _userService.AuthenticateAsync(command);

            return Ok(user);
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        public async Task<IActionResult> RetrieveByIDAsync(int id)
        {
            return Ok(await _userService.RetrieveByIDAsync(id));
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateAsync(UserCreateCommand command)
        {
            return Ok(await _userService.CreateAsync(command));
        }

        [AuthorizeRoles(Role.Manager, Role.Client)]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateAsync(UserUpdateCommand command)
        {
            return Ok(await _userService.UpdateAsync(command));
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteAsync(UserDeleteCommand command)
        {
            await _userService.DeleteAsync(command);

            return Ok(true);
        }
    }
}
