using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Commands;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Attributes;
using Template.WebApi.Controllers.Api.UsersModule;

namespace Template.Tests.Controllers.Api.UsersModule.UsersControllerTests
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
                Name = "mock-Name",
            };

            var expectedResult = Guid.NewGuid();

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
                Name = "mock-Name",
            };

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Failure<Guid>(expectedResult));

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
            typeof(UsersController)
                .GetMethod("CreateAsync")
                .Should()
                .BeDecoratedWith<AuthorizeRoles>(p => p.Roles.Equals(Role.Manager.ToString())).And
                .BeDecoratedWith<HttpPostAttribute>().And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(int) && a.StatusCode == 200);
        }

        private UsersController GetController()
        {
            return new UsersController(_moqMediator.Object);
        }
    }
}
