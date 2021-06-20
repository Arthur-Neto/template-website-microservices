using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Template.Application.TenantsModule.Commands;
using Template.Application.TenantsModule.Models;

namespace Template.WebApi.Controllers.Api
{
    [ApiController]
    [Route("api/public")]
    public class PublicController : BaseController
    {
        public PublicController(IMediator mediator)
            : base(mediator)
        { }

        [AllowAnonymous]
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(typeof(AuthenticatedTenantModel), 200)]
        public async Task<IActionResult> AuthenticateAsync(TenantAuthenticateCommand command)
        {
            var result = await _mediator.Send(command);

            return HandleResult(result);
        }
    }
}
