using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.WebApi.Controllers.Api;

namespace Template.Tests.Controllers.Api
{
    [TestFixture]
    public class BaseControllerTests
    {
        private Mock<IMediator> _moqMediator;

        [SetUp]
        public void SetUp()
        {
            _moqMediator = new Mock<IMediator>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMediator.VerifyAll();
        }

        [Test]
        public void Deve_Retornar_OkObjectResult()
        {
            // Arrange
            var successResult = Result.Success();

            // Act
            var result = GetController().HandleResult<object>(successResult);

            // Assert
            result.Should().BeOfType<OkObjectResult>();
        }

        [Test]
        public void Deve_Retornar_BadRequestObjectResult()
        {
            // Arrange
            var failureResult = Result.Failure<object>("Error");

            // Act
            var result = GetController().HandleResult(failureResult);

            // Assert
            result.Should().BeOfType<BadRequestObjectResult>();
        }

        private BaseController GetController()
        {
            return new BaseControllerDummy(_moqMediator.Object);
        }
    }

    internal class BaseControllerDummy : BaseController
    {
        public BaseControllerDummy(IMediator mediator)
            : base(mediator)
        { }
    }
}
