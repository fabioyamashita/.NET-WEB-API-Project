using AutoFixture;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using SPX_WEBAPI.Controllers;
using SPX_WEBAPI.Domain.Models;
using SPX_WEBAPI.Infra.Interfaces;
using System.Linq.Expressions;
using Xunit;

namespace SPX_WEBAPI.Tests.Controllers
{
    public class SpxControllerTests
    {
        private readonly SpxController _sut;
        private readonly Mock<IBaseRepository<Spx>> _mockBaseRepository;

        public SpxControllerTests()
        {
            _mockBaseRepository = new Mock<IBaseRepository<Spx>>();
            _sut = new SpxController(_mockBaseRepository.Object);
        }

        [Fact(DisplayName = "GetById Valid Id Returns Status Code 200")]
        [Trait("SPX", "SPX Controller Tests")]
        public async Task GetById_ValidId_ReturnsStatusCode200()
        {
            // Arrange
            var spx = new Fixture().Create<Spx>();

            _mockBaseRepository.Setup(br => br.GetByIdAsync(It.IsAny<Expression<Func<Spx, bool>>>()).Result)
                .Returns(spx);

            // Act
            var result = await _sut.GetById(It.IsAny<int>());
            var okObjectResult = result as OkObjectResult;

            // Assert
            okObjectResult.Should().NotBeNull();
            okObjectResult?.Value.Should().BeEquivalentTo(spx);
            okObjectResult?.StatusCode.Should().Be(200);
        }

        [Fact(DisplayName = "GetById Invalid Id Returns Status Code 404")]
        [Trait("SPX", "SPX Controller Tests")]
        public async Task GetById_ValidId_ReturnsStatusCode404()
        {
            // Arrange
            _mockBaseRepository.Setup(br => br.GetByIdAsync(It.IsAny<Expression<Func<Spx, bool>>>()).Result)
                .Returns<Spx>(null);

            // Act
            var result = await _sut.GetById(It.IsAny<int>());
            var notFoundObjectResult = result as NotFoundObjectResult;

            // Assert
            notFoundObjectResult.Should().NotBeNull();
            notFoundObjectResult?.Value.Should().Be("Id not found");
            notFoundObjectResult?.StatusCode.Should().Be(404);
        }
    }
}
