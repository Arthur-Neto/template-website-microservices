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
    public class DeleteAsyncTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_Verdadeiro_Quando_Remover_Corretamente()
        {
            // Arrange
            var command = new UserDeleteCommand
            {
                ID = Guid.NewGuid()
            };

            var expectedResult = true;

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Success(expectedResult));

            // Act
            var result = await GetController().DeleteAsync(command);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_Com_Retorno_BadRequest_Quando_Resultado_For_Falha()
        {
            // Arrange
            var command = new UserDeleteCommand
            {
                ID = Guid.NewGuid()
            };

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(command, default))
                .ReturnsAsync(Result.Failure<bool>(expectedResult));

            // Act
            var result = await GetController().DeleteAsync(command);

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
                .GetMethod("DeleteAsync")
                .Should()
                .BeDecoratedWith<AuthorizeRoles>(p => p.Roles.Equals(Role.Manager.ToString())).And
                .BeDecoratedWith<HttpDeleteAttribute>().And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(bool) && a.StatusCode == 200);
        }

        private UsersController GetController()
        {
            return new UsersController(_moqMediator.Object);
        }
    }
}
