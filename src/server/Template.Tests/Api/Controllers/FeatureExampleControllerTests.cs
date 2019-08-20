using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using NUnit.Framework;
using Template.Api.Controllers;
using Template.Application.FeatureExampleModule;
using Template.Application.FeatureExampleModule.Models.Commands;
using Template.Domain.FeatureExampleModule;

namespace Template.Tests.Api.Controllers
{
    [TestFixture]
    public class FeatureExampleControllerTests
    {
        private readonly Mock<IFeatureExampleAppService> _moqFeatureExampleAppService;

        public FeatureExampleControllerTests()
        {
            _moqFeatureExampleAppService = new Mock<IFeatureExampleAppService>(MockBehavior.Strict);
        }

        [Test]
        public async Task Should_ReturnOk_When_RetrieveAll()
        {
            // Arrange
            _moqFeatureExampleAppService
                .Setup(x => x.RetrieveAllAsync())
                .ReturnsAsync(It.IsNotNull<IEnumerable<FeatureExample>>())
                .Verifiable();

            // Action
            var controller = GetController();

            var result = await controller.RetrieveAllAsync();

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be(200);

            _moqFeatureExampleAppService.Verify();
        }

        [Test]
        public async Task Should_ReturnOk_When_RetrieveByID()
        {
            // Arrange
            var id = 1;

            _moqFeatureExampleAppService
                .Setup(x => x.RetrieveByIDAsync(id))
                .ReturnsAsync(It.IsNotNull<FeatureExample>())
                .Verifiable();

            // Action
            var controller = GetController();

            var result = await controller.RetrieveByIDAsync(id);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be(200);

            _moqFeatureExampleAppService.Verify();
        }

        [Test]
        public async Task Should_ReturnOk_When_Add()
        {
            // Arrange
            var command = new FeatureExampleAddCommand()
            {
                FeatureExampleType = FeatureExampleEnum.EnumExample,
            };

            _moqFeatureExampleAppService
                .Setup(x => x.AddAsync(command))
                .ReturnsAsync(true)
                .Verifiable();

            // Action
            var controller = GetController();

            var result = await controller.AddAsync(command);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be(200);

            _moqFeatureExampleAppService.Verify();
        }

        [Test]
        public async Task Should_ReturnOk_When_Update()
        {
            // Arrange
            var command = new FeatureExampleUpdateCommand()
            {
                ID = 1,
                FeatureExampleType = FeatureExampleEnum.EnumExample,
            };

            _moqFeatureExampleAppService
                .Setup(x => x.UpdateAsync(command))
                .ReturnsAsync(true)
                .Verifiable();

            // Action
            var controller = GetController();

            var result = await controller.UpdateAsync(command);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be(200);

            _moqFeatureExampleAppService.Verify();
        }

        [Test]
        public async Task Should_ReturnOk_When_Remove()
        {
            // Arrange
            var id = 1;

            _moqFeatureExampleAppService
                .Setup(x => x.RemoveAsync(id))
                .ReturnsAsync(true)
                .Verifiable();

            // Action
            var controller = GetController();

            var result = await controller.RemoveAsync(id);

            // Assert
            var okObjectResult = result.Should().BeOfType<OkObjectResult>().Subject;
            okObjectResult.StatusCode.Should().Be(200);

            _moqFeatureExampleAppService.Verify();
        }

        private FeatureExampleController GetController()
        {
            return new FeatureExampleController(_moqFeatureExampleAppService.Object);
        }
    }
}
