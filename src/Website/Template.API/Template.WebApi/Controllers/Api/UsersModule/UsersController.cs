using System;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Application.UsersModule.Queries;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.Api.UsersModule
{
    [ApiController]
    [Route("api/users")]
    public class UsersController : BaseController
    {
        public UsersController(IMediator mediator)
            : base(mediator)
        { }

        [AuthorizeRoles(Role.Manager)]
        [HttpGet]
        [Route("{id:Guid}")]
        [ProducesResponseType(typeof(UserModel), 200)]
        public async Task<IActionResult> RetrieveByIDAsync(Guid id)
        {
            var query = new UserRetrieveByIDQuery { ID = id };
            var result = await _mediator.Send(query);

            return HandleResult(result);
        }

        [AuthorizeRoles(Role.Manager)]
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
