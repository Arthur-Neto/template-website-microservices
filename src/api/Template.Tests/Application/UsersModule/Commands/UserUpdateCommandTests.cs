using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Template.Application;
using Template.Application.UsersModule.Commands;
using Template.Domain.UsersModule;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Tests.Application.UsersModule.Commands
{
    [TestFixture]
    public class UserUpdateCommandTests
    {
        private Mock<IMapper> _moqMapper;
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private Mock<IUserRepository> _moqUserRepository;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
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
        public async Task Deve_Verificar_Metodo_E_Retornar_Verdadeiro_Quando_Atualizar_Usuario()
        {
            // Arrange
            var command = new UserUpdateCommand()
            {
                ID = 1,
                Username = "username",
                Password = "password"
            };

            var user = new User()
            {
                ID = 1,
                Username = command.Username,
                Password = command.Password,
            };

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(command.ID, default))
                .ReturnsAsync(user);

            _moqUserRepository
                .Setup(p => p.AnyAsync(x => x.Username.Equals(command.Username) && x.ID != command.ID, default))
                .ReturnsAsync(false);

            _moqUnitOfWork
                .Setup(p => p.CommitAsync())
                .ReturnsAsync(1);

            _moqUserRepository
                .Setup(p => p.Update(user));

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Value.Should().BeTrue();
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Nao_Encontrado()
        {
            // Arrange
            var command = new UserUpdateCommand()
            {
                ID = 1,
                Username = "username",
                Password = "password"
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

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Duplicado_Quando_Atualizar_Username_Para_Um_Ja_Existente()
        {
            // Arrange
            var command = new UserUpdateCommand()
            {
                ID = 1,
                Username = "username",
                Password = "password"
            };

            var user = new User()
            {
                ID = 1,
                Username = command.Username,
                Password = command.Password,
            };

            _moqUserRepository
                .Setup(p => p.RetrieveByIDAsync(command.ID, default))
                .ReturnsAsync(user);

            _moqUserRepository
                .Setup(p => p.AnyAsync(x => x.Username.Equals(command.Username) && x.ID != command.ID, default))
                .ReturnsAsync(true);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.Duplicating.ToString());
        }

        private UserUpdateCommandHandler GetHandler()
        {
            return new UserUpdateCommandHandler(
                _moqMapper.Object,
                _moqUnitOfWork.Object,
                _moqUserRepository.Object
            );
        }
    }
}
