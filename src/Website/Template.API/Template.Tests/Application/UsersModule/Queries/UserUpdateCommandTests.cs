using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Models;
using Template.Application.UsersModule.Queries;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Tests.Application.UsersModule.Commands
{
    [TestFixture]
    public class UserRetrieveByIDQueryTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_Usuario_Quando_Buscar_Por_ID()
        {
            // Arrange
            var id = Guid.NewGuid();
            var query = new UserRetrieveByIDQuery()
            {
                ID = id,
            };

            var user = new User()
            {
                ID = id,
                Name = "Name",
            };

            var userModel = new UserModel()
            {
                ID = user.ID.ToString(),
                Name = user.Name,
            };

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(query.ID, default))
                .ReturnsAsync(user);

            _moqMapper
                .Setup(p => p.Map<UserModel>(user))
                .Returns(userModel);

            // Act
            var result = await GetHandler().Handle(query, default);

            // Assert
            result.Value.Should().Be(userModel);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Nao_Encontrado()
        {
            // Arrange
            var query = new UserRetrieveByIDQuery()
            {
                ID = Guid.NewGuid(),
            };

            User user = null;

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(query.ID, default))
                .ReturnsAsync(user);

            // Act
            var result = await GetHandler().Handle(query, default);

            // Assert
            result.Error.Should().Be(ErrorType.NotFound.ToString());
        }

        private UserRetrieveByIDQueryHandler GetHandler()
        {
            return new UserRetrieveByIDQueryHandler(
                _moqMapper.Object,
                _moqUserRepository.Object
            );
        }
    }
}
