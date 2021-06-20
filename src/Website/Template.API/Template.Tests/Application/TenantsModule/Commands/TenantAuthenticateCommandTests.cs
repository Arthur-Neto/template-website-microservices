using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CSharpFunctionalExtensions;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using Template.Application.TenantsModule.Commands;
using Template.Application.TenantsModule.Models;
using Template.Domain.TenantsModule;
using Template.Domain.TenantsModule.Enums;
using Template.Infra.Crosscutting.Exceptions;

namespace Template.Tests.Application.TenantsModule.Commands
{
    [TestFixture]
    public class TenantAuthenticateCommandTests
    {
        private IConfiguration _moqConfiguration;
        private Mock<IMapper> _moqMapper;
        private Mock<ITenantRepository> _moqTenantRepository;
        private Mock<ILogger<TenantAuthenticateCommandHandler>> _moqLogger;

        [SetUp]
        public void SetUp()
        {
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);
            _moqTenantRepository = new Mock<ITenantRepository>(MockBehavior.Strict);
            _moqLogger = new Mock<ILogger<TenantAuthenticateCommandHandler>>(MockBehavior.Loose);
        }

        [TearDown]
        public void TearDown()
        {
            _moqMapper.VerifyAll();
            _moqTenantRepository.VerifyAll();
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

            var command = new TenantAuthenticateCommand()
            {
                Logon = "username",
                Password = "password"
            };

            Maybe<Tenant> tenant = new Tenant()
            {
                ID = Guid.NewGuid(),
                Salt = "123456789",
                Logon = command.Logon,
                Password = command.Password,
                Role = Role.Client
            };

            var expectedResult = new AuthenticatedTenantModel()
            {
                ID = tenant.Value.ID.ToString(),
                Logon = tenant.Value.Logon,
                Role = tenant.Value.Role.ToString()
            };

            _moqTenantRepository
                .Setup(p => p.SingleOrDefaultAsync(x => x.Logon.Equals(command.Logon), true, default, x => x.Enterprise))
                .ReturnsAsync(tenant.Value);

            _moqMapper
                .Setup(p => p.Map<AuthenticatedTenantModel>(tenant.Value))
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
                _moqTenantRepository.Object
            );
        }
    }
}
