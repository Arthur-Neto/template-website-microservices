using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Template.Application.TenantsModule.Commands;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Attributes;

namespace Template.WebApi.Controllers.Api.TenantsModule
{
    [ApiController]
    [Route("api/tenants")]
    public class TenantsController : BaseController
    {
        public TenantsController(IMediator mediator)
            : base(mediator)
        { }

        [AuthorizeRoles(Role.Manager)]
        [HttpPost]
        [ProducesResponseType(typeof(int), 200)]
        public async Task<IActionResult> CreateAsync(TenantCreateCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }
    }
}
