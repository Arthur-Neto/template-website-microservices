using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule;

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
    }
}
