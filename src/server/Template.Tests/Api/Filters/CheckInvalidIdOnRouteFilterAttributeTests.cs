using System.Collections.Generic;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Routing;
using NUnit.Framework;
using Template.Api.Filters;

namespace Template.Tests.Api.Filters
{
    [TestFixture]
    public class CheckInvalidIdOnRouteFilterAttributeTests
    {
        [Test]
        public void Should_ReturnBadRequest_When_IdIsZero()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new object());

            context.ActionArguments.Add("id", 0);

            var filter = new CheckInvalidIdOnRouteFilterAttribute();

            //Act
            filter.OnActionExecuting(context);

            //Assert
            context.Result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
        }

        [Test]
        public void Should_ReturnBadRequest_When_IdIsNegative()
        {
            //Arrange
            var httpContext = new DefaultHttpContext();
            var context = new ActionExecutingContext(
                new ActionContext
                {
                    HttpContext = httpContext,
                    RouteData = new RouteData(),
                    ActionDescriptor = new ActionDescriptor(),
                },
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                new object());

            context.ActionArguments.Add("id", -1);

            var filter = new CheckInvalidIdOnRouteFilterAttribute();

            //Act
            filter.OnActionExecuting(context);

            //Assert
            context.Result.Should().NotBeNull().And.BeOfType<BadRequestResult>();
        }
    }
}
