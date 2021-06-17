using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Template.Application;
using Template.Application.UsersModule.Commands;
using Template.Application.UsersModule.Models;
using Template.Domain.UsersModule;
using Template.Domain.UsersModule.Enums;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Tests.Application.UsersModule.Commands
{
    [TestFixture]
    public class UserAuthenticateCommandTests
    {
        private IConfiguration _moqConfiguration;
        private Mock<IMapper> _moqMapper;
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private Mock<IUserRepository> _moqUserRepository;
        private Mock<ILogger<UserAuthenticateCommandHandler>> _moqLogger;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _moqUserRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _moqLogger = new Mock<ILogger<UserAuthenticateCommandHandler>>(MockBehavior.Loose);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMapper.VerifyAll();
            _moqUnitOfWork.VerifyAll();
            _moqUserRepository.VerifyAll();
        }

        private void BuildConfiguration(string secret = "super secret key", string tokenExpiration = "10")
        {
            var inMemorySettings = new Dictionary<string, string> {
                {"TokenExpiration", tokenExpiration},
                {"Secret", secret},
            };

            _moqConfiguration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Usuario_Autenticado()
        {
            // Arrange
            BuildConfiguration();

            var command = new UserAuthenticateCommand()
            {
                Username = "username",
                Password = "password"
            };

            var user = new User()
            {
                ID = 1,
                Username = command.Username,
                Password = command.Password,
                Role = Role.Client
            };

            var expectedResult = new AuthenticatedUserModel()
            {
                ID = user.ID,
                Username = user.Username,
                Role = user.Role.ToString()
            };

            _moqUserRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Username.Equals(command.Username), true, default))
                .ReturnsAsync(user);

            _moqUnitOfWork
                .Setup(p => p.CommitAsync())
                .ReturnsAsync(1);

            _moqMapper
                .Setup(p => p.Map<AuthenticatedUserModel>(user))
                .Returns(expectedResult);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Value.ID.Should().Be(expectedResult.ID);
            result.Value.Username.Should().Be(expectedResult.Username);
            result.Value.Role.Should().Be(expectedResult.Role);
            result.Value.Token.Should().Be(expectedResult.Token);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_SecretKey_Com_Tamanho_Insuficiente()
        {
            // Arrange
            BuildConfiguration("secretkey");

            var command = new UserAuthenticateCommand()
            {
                Username = "username",
                Password = "password"
            };

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.SecretKeyTooShort.ToString());
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Usuario_Nao_Existente()
        {
            // Arrange
            BuildConfiguration();

            var command = new UserAuthenticateCommand()
            {
                Username = "username",
                Password = "password"
            };

            User user = null;

            _moqUserRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Username.Equals(command.Username), true, default))
                .ReturnsAsync(user);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.NotFound.ToString());
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_Senha_Incorreta()
        {
            // Arrange
            BuildConfiguration();

            var command = new UserAuthenticateCommand()
            {
                Username = "username",
                Password = "password"
            };

            var user = new User()
            {
                ID = 1,
                Username = command.Username,
                Password = "anotherPassword",
                Role = Role.Client
            };

            _moqUserRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Username.Equals(command.Username), true, default))
                .ReturnsAsync(user);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.IncorrectUserPassword.ToString());
        }

        private UserAuthenticateCommandHandler GetHandler()
        {
            return new UserAuthenticateCommandHandler(
                _moqMapper.Object,
                _moqUnitOfWork.Object,
                _moqLogger.Object,
                _moqConfiguration,
                _moqUserRepository.Object
            );
        }
    }
}
