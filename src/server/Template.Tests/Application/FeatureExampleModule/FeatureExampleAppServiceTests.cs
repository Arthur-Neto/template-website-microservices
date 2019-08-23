using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using FluentAssertions;
using Moq;
using NUnit.Framework;
using Template.Application.FeatureExampleModule;
using Template.Application.FeatureExampleModule.Models.Commands;
using Template.Domain.CommonModule;
using Template.Domain.FeatureExampleModule;

namespace Template.Tests.Application.FeatureExampleModule
{
    [TestFixture]
    public class FeatureExampleAppServiceTests
    {
        private Mock<IUnitOfWork> _moqUnitOfWork;
        private Mock<IMapper> _moqMapper;

        private Mock<IFeatureExampleRepository> _moqFeatureExampleRepository;

        public FeatureExampleAppServiceTests()
        {
            _moqUnitOfWork = new Mock<IUnitOfWork>(MockBehavior.Strict);
            _moqMapper = new Mock<IMapper>(MockBehavior.Strict);

            _moqFeatureExampleRepository = new Mock<IFeatureExampleRepository>(MockBehavior.Strict);
        }

        [Test]
        public async Task Should_VerifyMethod_When_Add()
        {
            // Arrange
            var rowsAffected = 1;

            var feature = new FeatureExample(FeatureExampleEnum.EnumExample);

            var command = new FeatureExampleAddCommand()
            {
                FeatureExampleType = FeatureExampleEnum.EnumExample
            };

            _moqMapper
                .Setup(x => x.Map<FeatureExample>(command))
                .Returns(feature)
                .Verifiable();

            _moqFeatureExampleRepository
                .Setup(x => x.AddAsync(feature))
                .Returns(Task.CompletedTask)
                .Verifiable();

            _moqUnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(rowsAffected)
                .Verifiable();

            // Action
            var service = GetService();

            var result = await service.AddAsync(command);

            // Assert
            result.Should().BeTrue();

            _moqMapper.Verify();
            _moqFeatureExampleRepository.Verify();
            _moqUnitOfWork.Verify();
        }

        [Test]
        public async Task Should_VerifyMethod_When_Remove()
        {
            // Arrange
            var rowsAffected = 1;

            var feature = new FeatureExample();

            var featureIDToRemove = 1;

            _moqFeatureExampleRepository
                .Setup(x => x.GetByIDAsync(featureIDToRemove))
                .ReturnsAsync(feature)
                .Verifiable();

            _moqFeatureExampleRepository
                .Setup(x => x.Remove(feature))
                .Verifiable();

            _moqUnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(rowsAffected)
                .Verifiable();

            // Action
            var service = GetService();

            var result = await service.RemoveAsync(featureIDToRemove);

            // Assert
            result.Should().BeTrue();

            _moqMapper.Verify();
            _moqFeatureExampleRepository.Verify();
            _moqUnitOfWork.Verify();
        }

        [Test]
        public void Should_ThrowNullReferenceException_When_Remove_With_FeatureExampleNotFound()
        {
            // Arrange
            var feature = new FeatureExample();

            var featureIDToRemove = 1;

            _moqFeatureExampleRepository
                .Setup(x => x.GetByIDAsync(featureIDToRemove))
                .ReturnsAsync(default(FeatureExample))
                .Verifiable();

            // Action
            var service = GetService();

            Func<Task> action = async () => await service.RemoveAsync(featureIDToRemove);

            // Assert
            action.Should().Throw<NullReferenceException>().WithMessage("NotFound");

            _moqFeatureExampleRepository.Verify();
        }

        [Test]
        public async Task Should_VerifyMethod_When_RetrieveAll()
        {
            // Arrange
            var listFeatures = new List<FeatureExample>()
            {
                new FeatureExample()
            };

            var numberOfFeatures = listFeatures.Count();

            _moqFeatureExampleRepository
                .Setup(x => x.GetAllAsync())
                .ReturnsAsync(listFeatures)
                .Verifiable();

            // Action
            var service = GetService();

            var result = await service.RetrieveAllAsync();

            // Assert
            result.Count().Should().Be(numberOfFeatures);

            _moqFeatureExampleRepository.Verify();
        }

        [Test]
        public async Task Should_VerifyMethod_When_RetrieveByID()
        {
            // Arrange
            var featureIDToSearch = 1;

            var moqFeature = new Mock<FeatureExample>(MockBehavior.Strict);

            moqFeature
                .SetupGet(x => x.ID)
                .Returns(featureIDToSearch)
                .Verifiable();

            _moqFeatureExampleRepository
                .Setup(x => x.GetByIDAsync(featureIDToSearch))
                .ReturnsAsync(moqFeature.Object)
                .Verifiable();

            // Action
            var service = GetService();

            var result = await service.RetrieveByIDAsync(featureIDToSearch);

            // Assert
            result.ID.Should().Be(featureIDToSearch);

            moqFeature.Verify();
            _moqFeatureExampleRepository.Verify();
        }

        [Test]
        public async Task Should_VerifyMethod_When_Update()
        {
            // Arrange
            var rowsAffected = 1;

            var command = new FeatureExampleUpdateCommand()
            {
                ID = 1,
                FeatureExampleType = FeatureExampleEnum.EnumExample
            };

            var feature = new FeatureExample();

            _moqFeatureExampleRepository
                .Setup(x => x.GetByIDAsync(command.ID))
                .ReturnsAsync(feature)
                .Verifiable();

            _moqFeatureExampleRepository
                .Setup(x => x.Update(feature))
                .Verifiable();

            _moqUnitOfWork
                .Setup(x => x.CommitAsync())
                .ReturnsAsync(rowsAffected)
                .Verifiable();

            // Action
            var service = GetService();

            var result = await service.UpdateAsync(command);

            // Assert
            result.Should().BeTrue();

            _moqMapper.Verify();
            _moqFeatureExampleRepository.Verify();
            _moqUnitOfWork.Verify();
        }

        [Test]
        public void Should_ThrowNullReferenceException_When_Update_With_FeatureExampleNotFound()
        {
            // Arrange
            var command = new FeatureExampleUpdateCommand()
            {
                ID = 1,
                FeatureExampleType = FeatureExampleEnum.EnumExample
            };

            _moqFeatureExampleRepository
                .Setup(x => x.GetByIDAsync(command.ID))
                .ReturnsAsync(default(FeatureExample))
                .Verifiable();

            // Action
            var service = GetService();

            Func<Task> action = async () => await service.UpdateAsync(command);

            // Assert
            action.Should().Throw<NullReferenceException>().WithMessage("NotFound");

            _moqFeatureExampleRepository.Verify();
        }

        private FeatureExampleAppService GetService()
        {
            var lazyUnitOfWork = new Lazy<IUnitOfWork>(() => _moqUnitOfWork.Object);
            var lazyMapper = new Lazy<IMapper>(() => _moqMapper.Object);

            return new FeatureExampleAppService(
                lazyUnitOfWork,
                lazyMapper,
                _moqFeatureExampleRepository.Object);
        }
    }
}
