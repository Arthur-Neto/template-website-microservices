using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule.Enums;
using Template.WebApi.Controllers.Api.UsersModule;

namespace Template.Tests.Controllers.Api.UsersModule.UserControllerTests
{
    [TestFixture]
    public class AuthenticateAsyncTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_UsuarioAutenticado()
        {
            // Arrange
            var command = new UserAuthenticateCommand
            {
                Username = "mock-username",
                Password = "123"
            };

            var expectedResult = new AuthenticatedUserModel()
            {
                ID = 1,
                Role = Role.Manager.ToString(),
                Token = string.Empty,
                Username = "mock-username"
            };

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Success(expectedResult));

            // Act
            var result = await GetController().AuthenticateAsync(command);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_Com_Retorno_BadRequest_Quando_Resultado_For_Falha()
        {
            // Arrange
            var command = new UserAuthenticateCommand
            {
                Username = "mock-username",
                Password = "123"
            };

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Failure<AuthenticatedUserModel>(expectedResult));

            // Act
            var result = await GetController().AuthenticateAsync(command);

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
                .GetMethod("AuthenticateAsync")
                .Should()
                .BeDecoratedWith<AllowAnonymousAttribute>().And
                .BeDecoratedWith<HttpPostAttribute>().And
                .BeDecoratedWith<RouteAttribute>(a => a.Template == "login").And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(AuthenticatedUserModel) && a.StatusCode == 200);
        }

        private UserController GetController()
        {
            return new UserController(_moqMediator.Object);
        }
    }
}
