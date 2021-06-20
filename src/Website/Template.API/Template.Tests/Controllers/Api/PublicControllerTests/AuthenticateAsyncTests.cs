using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Application.TenantsModule.Commands;
using Template.Application.TenantsModule.Models;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Controllers.Api;

namespace Template.Tests.Controllers.Api.PublicControllerTests
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
            var command = new TenantAuthenticateCommand
            {
                Logon = "mock-username",
                Password = "123"
            };

            var expectedResult = new AuthenticatedTenantModel()
            {
                ID = Guid.NewGuid().ToString(),
                Role = Role.Manager.ToString(),
                Token = string.Empty,
                Logon = "mock-username"
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
            var command = new TenantAuthenticateCommand
            {
                Logon = "mock-username",
                Password = "123"
            };

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Failure<AuthenticatedTenantModel>(expectedResult));

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
            typeof(PublicController)
                .GetMethod("AuthenticateAsync")
                .Should()
                .BeDecoratedWith<AllowAnonymousAttribute>().And
                .BeDecoratedWith<HttpPostAttribute>().And
                .BeDecoratedWith<RouteAttribute>(a => a.Template == "login").And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(AuthenticatedTenantModel) && a.StatusCode == 200);
        }

        private PublicController GetController()
        {
            return new PublicController(_moqMediator.Object);
        }
    }
}
