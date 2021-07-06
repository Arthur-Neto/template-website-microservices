using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Template.Application.TenantsModule.Commands;
using Template.Application.TenantsModule.Models;
using Template.Domain.EnterprisesModule;
using Template.Domain.TenantsModule;
using Template.Domain.TenantsModule.Enums;
using Template.Infra.Crosscutting.Exceptions;
using Template.Security;

namespace Template.Tests.Application.TenantsModule.Commands
{
    [TestFixture]
    public class TenantAuthenticateCommandTests
    {
        private IConfiguration _moqConfiguration;
        private Mock<IMapper> _moqMapper;
        private Mock<ITenantRepository> _moqTenantRepository;
        private Mock<ILogger<TenantAuthenticateCommandHandler>> _moqLogger;
        private Mock<IJwtTokenFactory> _moqJwtTokenFactory;
        private Mock<IHashing> _moqHashing;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqTenantRepository = new Mock<ITenantRepository>(MockBehavior.Strict);
            _moqLogger = new Mock<ILogger<TenantAuthenticateCommandHandler>>(MockBehavior.Loose);
            _moqJwtTokenFactory = new Mock<IJwtTokenFactory>(MockBehavior.Strict);
            _moqHashing = new Mock<IHashing>(MockBehavior.Strict);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMapper.VerifyAll();
            _moqTenantRepository.VerifyAll();
            _moqJwtTokenFactory.VerifyAll();
            _moqHashing.VerifyAll();
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
            var secret = "super secret key";
            var tokenExpiration = 10;
            BuildConfiguration(secret, tokenExpiration.ToString());

            var command = new TenantAuthenticateCommand()
            {
                Logon = "username",
                Password = "password"
            };

            var tenant = new Tenant()
            {
                ID = Guid.NewGuid(),
                Salt = "123456789",
                Logon = command.Logon,
                Password = command.Password,
                Role = Role.Client,
                Enterprise = new Enterprise()
                {
                    ConnectionString = "mock-schema"
                }
            };

            var expectedResult = new AuthenticatedTenantModel()
            {
                ID = tenant.ID.ToString(),
                Logon = tenant.Logon,
                Role = tenant.Role.ToString()
            };

            _moqTenantRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Logon.Equals(command.Logon), true, default, x => x.Enterprise))
                .ReturnsAsync(tenant);

            _moqHashing
                .Setup(p => p.IsValidHash(tenant.Password, tenant.Salt, command.Password))
                .Returns(true);

            _moqJwtTokenFactory
                .Setup(p => p.CreateToken(secret, tokenExpiration, tenant.ID.ToString(), tenant.Role.ToString(), tenant.Enterprise.NormalizedEnterpriseName))
                .Returns(It.IsAny<string>());

            _moqMapper
                .Setup(p => p.Map<AuthenticatedTenantModel>(tenant))
                .Returns(expectedResult);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Value.ID.Should().Be(expectedResult.ID);
            result.Value.Logon.Should().Be(expectedResult.Logon);
            result.Value.Role.Should().Be(expectedResult.Role);
            result.Value.Token.Should().Be(expectedResult.Token);
        }

        [Test]
        public async Task Deve_Verificar_Metodo_E_Retornar_Falha_De_SecretKey_Com_Tamanho_Insuficiente()
        {
            // Arrange
            BuildConfiguration("secretkey");

            var command = new TenantAuthenticateCommand()
            {
                Logon = "username",
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

            var command = new TenantAuthenticateCommand()
            {
                Logon = "username",
                Password = "password"
            };

            Tenant tenant = null;

            _moqTenantRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Logon.Equals(command.Logon), true, default, x => x.Enterprise))
                .ReturnsAsync(tenant);

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

            var command = new TenantAuthenticateCommand()
            {
                Logon = "username",
                Password = "password"
            };

            var tenant = new Tenant()
            {
                ID = Guid.NewGuid(),
                Salt = "123456789",
                Logon = command.Logon,
                Password = "anotherPassword",
                Role = Role.Client
            };

            _moqTenantRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Logon.Equals(command.Logon), true, default, x => x.Enterprise))
                .ReturnsAsync(tenant);

            _moqHashing
                .Setup(p => p.IsValidHash(tenant.Password, tenant.Salt, command.Password))
                .Returns(false);

            // Act
            var result = await GetHandler().Handle(command, default);

            // Assert
            result.Error.Should().Be(ErrorType.IncorrectUserPassword.ToString());
        }

        private TenantAuthenticateCommandHandler GetHandler()
        {
            return new TenantAuthenticateCommandHandler(
                _moqMapper.Object,
                _moqLogger.Object,
                _moqConfiguration,
                _moqTenantRepository.Object,
                _moqHashing.Object,
                _moqJwtTokenFactory.Object
            );
        }
    }
}
