using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule;
using Template.Domain.UsersModule;

namespace Template.Api.Controllers
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

        [HttpGet]
        [Route("")]
        public async Task<IActionResult> RetrieveAllAsync()
        {
            return Ok(await _userService.RetrieveAllAsync());
        }

        [HttpGet]
        [Route("{id:int}")]
        public async Task<IActionResult> RetrieveByIDAsync(int id)
        {
            return Ok(await _userService.RetrieveByIDAsync(id));
        }

        [HttpPost]
        [Route("")]
        public async Task<IActionResult> CreateAsync(User user)
        {
            return Ok(await _userService.CreateAsync(user));
        }

        [HttpPut]
        [Route("")]
        public IActionResult Update(User user)
        {
            _userService.Update(user);

            return Ok(true);
        }

        [HttpDelete]
        [Route("{id:int}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            await _userService.DeleteAsync(id);

            return Ok(true);
        }
    }
}
