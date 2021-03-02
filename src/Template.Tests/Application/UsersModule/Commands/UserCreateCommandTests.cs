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
    public class UserCreateCommandTests
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
        public async Task Deve_Verificar_Metodo_E_Retornar_ID_Do_Usuario_Criado()
        {
            // Arrange
            var command = new UserCreateCommand()
            {
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
                .Setup(p => p.AnyAsync(x => x.Username.Equals(command.Username), default))
                .ReturnsAsync(false);

            _moqUnitOfWork
                .Setup(p => p.CommitAsync())
                .ReturnsAsync(1);

            _moqMapper
                .Setup(p => p.Map<User>(command))
                .Returns(user);

            _moqUserRepository
                .Setup(p => p.CreateAsync(user, default))
                .ReturnsAsync(user);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Value.Should().Be(user.ID);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Duplicado()
        {
            // Arrange
            var command = new UserCreateCommand()
            {
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
                .Setup(p => p.AnyAsync(x => x.Username.Equals(command.Username), default))
                .ReturnsAsync(true);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.Duplicating.ToString());
        }

        private UserCreateCommandHandler GetHandler()
        {
            return new UserCreateCommandHandler(
                _moqMapper.Object,
                _moqUnitOfWork.Object,
                _moqUserRepository.Object
            );
        }
    }
}
