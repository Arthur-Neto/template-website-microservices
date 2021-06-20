using System;
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using Template.Domain.TenantsModule.Enums;
using Template.WebApi.Attributes;

namespace Template.Tests.Controllers.Attributes
{
    [TestFixture]
    public class AuthorizeRolesAttributeTests
    {
        [TestCase(Role.Manager)]
        [TestCase(Role.Manager, Role.Client)]
        [TestCase(Role.Client)]
        public void Deve_Setar_Corretamente_As_Roles_De_Autorizacao(params Role[] expectedRoles)
        {
            // Arrange
            var rolesAsListOfString = expectedRoles.Select(x => Enum.GetName(typeof(Role), x));

            // Act
            var attribute = new AuthorizeRoles(expectedRoles);

            // Assert
            var roles = attribute.Roles.Split(",");

            roles.Should().Contain(rolesAsListOfString);
        }
    }
}
