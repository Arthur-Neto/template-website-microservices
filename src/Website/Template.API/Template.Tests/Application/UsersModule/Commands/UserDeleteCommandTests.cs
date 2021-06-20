using System;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Template.Application.UsersModule.Commands;
using Template.Domain;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;
using Template.Infra.Data.EF.Contexts;

namespace Template.Tests.Application.UsersModule.Commands
{
    [TestFixture]
    public class UserDeleteCommandTests
    {
        private Mock<IMapper> _moqMapper;
        private Mock<IUnitOfWork<ITenantDbContext>> _moqUnitOfWork;
        private Mock<IUserRepository> _moqUserRepository;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqUnitOfWork = new Mock<IUnitOfWork<ITenantDbContext>>(MockBehavior.Strict);
            _moqUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMapper.VerifyAll();
            _moqUnitOfWork.VerifyAll();
            _moqUserRepository.VerifyAll();
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Verdadeiro_Quando_Remover_Usuario()
        {
            // Arrange
            var command = new UserDeleteCommand()
            {
                ID = Guid.NewGuid()
            };

            var user = new User()
            {
                ID = command.ID,
            };

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(command.ID, default))
                .ReturnsAsync(user);

            _moqUserRepository
                .Setup(p => p.Delete(user));

            _moqUnitOfWork
                .Setup(p => p.CommitAsync())
                .ReturnsAsync(1);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Value.Should().BeTrue();
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Nao_Encontrado()
        {
            // Arrange
            var command = new UserDeleteCommand()
            {
                ID = Guid.NewGuid()
            };

            User user = null;

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(command.ID, default))
                .ReturnsAsync(user);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.NotFound.ToString());
        }

        private UserDeleteCommandHandler GetHandler()
        {
            return new UserDeleteCommandHandler(
                _moqMapper.Object,
                _moqUnitOfWork.Object,
                _moqUserRepository.Object
            );
        }
    }
}
