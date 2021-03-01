using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Commands;
using Template.WebApi.Controllers.Api.UsersModule;

namespace Template.Tests.Controllers.Api.UsersModule.UserControllerTests
{
    [TestFixture]
    public class CreateAsyncTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_ID_Do_Usuario_Criado()
        {
            // Arrange
            var command = new UserCreateCommand
            {
                Username = "mock-username",
                Password = "123"
            };

            var expectedResult = 1;

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Success(expectedResult));

            // Act
            var result = await GetController().CreateAsync(command);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_Com_Retorno_BadRequest_Quando_Resultado_For_Falha()
        {
            // Arrange
            var command = new UserCreateCommand
            {
                Username = "mock-username",
                Password = "123"
            };

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Failure<int>(expectedResult));

            // Act
            var result = await GetController().CreateAsync(command);

            // Assert
            var badRequest = result as BadRequestObjectResult;

            badRequest.StatusCode.Should().Be(400);
            badRequest.Value.Should().Be(expectedResult);
        }

        [Test]
        public void Deve_Verificar_Atributos()
        {
            // Assert
            typeof(UserController)
                .GetMethod("CreateAsync")
                .Should()
                .BeDecoratedWith<AllowAnonymousAttribute>().And
                .BeDecoratedWith<HttpPostAttribute>().And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(int) && a.StatusCode == 200);
        }

        private UserController GetController()
        {
            return new UserController(_moqMediator.Object);
        }
    }
}
