using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Application.UsersModule.Queries;
using Template.Domain.UsersModule.Enums;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.Api.UsersModule
{
    [ApiController]
    [Route("api/users")]
    public class UserController : BaseController
    {
        public UserController(IMediator mediator)
            : base(mediator)
        { }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedUserModel), 200)]
        public async Task<IActionResult> AuthenticateAsync(UserAuthenticateCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Route("{id:int}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        public async Task<IActionResult> RetrieveByIDAsync(int id)
        {
            var query = new UserRetrieveByIDQuery { ID = id };
            var result = await _mediator.Send(query);

            return HandleResult(result);
        }

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateAsync(UserCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [AuthorizeRoles(Role.Manager, Role.Client)]
        [HttpPut]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> UpdateAsync(UserUpdateCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }

        [AuthorizeRoles(Role.Manager)]
        [HttpDelete]
        [ProducesResponseType(typeof(bool), 200)]
        public async Task<IActionResult> DeleteAsync(UserDeleteCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }
    }
}
