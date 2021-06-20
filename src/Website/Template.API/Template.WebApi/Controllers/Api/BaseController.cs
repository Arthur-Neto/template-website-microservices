using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Template.WebApi.Controllers.Api
{
    public abstract class BaseController : ControllerBase
    {
        protected readonly IMediator _mediator;

        public BaseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        public IActionResult HandleResult<T>(Result<T> result)
        {
            return result.IsSuccess ?
                Ok(result.Value) :
                BadRequest(result.Error);
        }
    }
}
