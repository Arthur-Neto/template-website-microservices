using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;

namespace Template.Api.Controllers.Api.UsersModule
{
    [Route("api/users")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        public async Task<IActionResult> RetrieveByIDAsync(int id)
        {
            return Ok(await _userService.RetrieveByIDAsync(id));
        }

        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateAsync(UserCreateCommand command)
        {
            return Ok(await _userService.CreateAsync(command));
        }

        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        public IActionResult Update(UserUpdateCommand command)
        {
            _userService.Update(command);

            return Ok(true);
        }

        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteAsync(UserDeleteCommand command)
        {
            await _userService.DeleteAsync(command);

            return Ok(true);
        }
    }
}
