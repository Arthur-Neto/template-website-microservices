using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Models;
using Template.Domain.TenantsModule.Enums;
using Template.Domain.UsersModule;
using Template.WebApi.Attributes;
using Template.WebApi.Controllers.OData.UsersModule;

namespace Template.Tests.Controllers.OData.UsersModule.UsersControllerTests
{
    [TestFixture]
    public class GetTests
    {
        private Mock<IMapper> _moqMapper;
        private Mock<IUserRepository> _moqUserRepository;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMapper.VerifyAll();
            _moqUserRepository.VerifyAll();
        }

        [Test]
        public void Deve_Verificar_Metodo_E_Retornar_Usuarios()
        {
            // Arrange
            var id = Guid.NewGuid();

            var expectedResult = new List<UserModel>()
            {
                new UserModel()
                {
                    ID = id.ToString(),
                    Name = "mock-Name"
                }
            };

            _moqUserRepository
                .Setup(p => p.RetrieveOData<UserModel>(_moqMapper.Object))
                .Returns(expectedResult.AsQueryable());

            // Act
            var result = GetController().Get();

            // Assert
            var okResult = result as OkObjectResult;

            okResult.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedResult);
        }

        [Test]
        public void Deve_Verificar_Atributos()
        {
            // Assert
            typeof(UsersController)
                .GetMethod("Get")
                .Should()
                .BeDecoratedWith<AuthorizeRoles>(p => p.Roles.Equals(Role.Manager.ToString())).And
                .BeDecoratedWith<HttpGetAttribute>().And
                .BeDecoratedWith<EnableQueryAttribute>().And
                .BeDecoratedWith<ProducesResponseTypeAttribute>(a => a.Type == typeof(IQueryable<UserModel>) && a.StatusCode == 200);
        }

        private UsersController GetController()
        {
            return new UsersController(_moqMapper.Object, _moqUserRepository.Object);
        }
    }
}
