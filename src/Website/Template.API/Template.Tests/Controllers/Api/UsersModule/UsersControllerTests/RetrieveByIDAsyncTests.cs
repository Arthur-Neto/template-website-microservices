using System;
using System.Threading.Tasks;
using CSharpFunctionalExtensions;
using FluentAssertions;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Models;
using Template.Application.UsersModule.Queries;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Attributes;
using Template.WebApi.Controllers.Api.UsersModule;

namespace Template.Tests.Controllers.Api.UsersModule.UsersControllerTests
{
    [TestFixture]
    public class RetrieveByIDAsyncTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_Usuario_Por_ID()
        {
            // Arrange
            var id = Guid.NewGuid();
            var expectedResult = new UserModel()
            {
                ID = id.ToString(),
                Name = "mock-Name"
            };

            _moqMediator
                .Setup(p => p.Send(It.IsAny<UserRetrieveByIDQuery>(), default))
                .ReturnsAsync(Result.Success(expectedResult));

            // Act
            var result = await GetController().RetrieveByIDAsync(id);

            // Assert
            var okResult = result as OkObjectResult;

            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedResult);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_Com_Retorno_BadRequest_Quando_Resultado_For_Falha()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expectedResult = "BadRequest";

            _moqMediator
                .Setup(p => p.Send(It.IsAny<UserRetrieveByIDQuery>(), default))
                .ReturnsAsync(Result.Failure<UserModel>(expectedResult));

            // Act
            var result = await GetController().RetrieveByIDAsync(id);

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
                .GetMethod("RetrieveByIDAsync")
                .Should()
                .BeDecoratedWith<AuthorizeRoles>(p => p.Roles.Equals(Role.Manager.ToString())).And
                .BeDecoratedWith<HttpGetAttribute>().And
                .BeDecoratedWith<RouteAttribute>(a => a.Template == "{id:Guid}").And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(UserModel) && a.StatusCode == 200);
        }

        private UsersController GetController()
        {
            return new UsersController(_moqMediator.Object);
        }
    }
}
