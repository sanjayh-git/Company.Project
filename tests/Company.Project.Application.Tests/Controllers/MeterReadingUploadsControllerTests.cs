using System.Text;
using Company.Project.Api.Controllers;
using Company.Project.Application.MeterReadings.Commands;
using Company.Project.Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Company.Project.Api.Tests.Controllers
{
    public class MeterReadingUploadsControllerTests
    {
        private readonly Mock<IMediator> _mediatorMock;
        private readonly MeterReadingUploadsController _controller;

        public MeterReadingUploadsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _controller = new MeterReadingUploadsController(_mediatorMock.Object);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsNull()
        {
            // Arrange
            IFormFile? file = null;
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Upload(file!, cancellationToken);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("File is required.", badRequest.Value);
        }

        [Fact]
        public async Task Upload_ReturnsBadRequest_WhenFileIsEmpty()
        {
            // Arrange
            var emptyContent = new MemoryStream(Array.Empty<byte>());
            var file = new FormFile(emptyContent, 0, 0, "file", "test.csv");
            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Upload(file, cancellationToken);

            // Assert
            var badRequest = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Uploaded file is empty.", badRequest.Value);
        }

        [Fact]
        public async Task Upload_SendsCommandToMediator_AndReturnsOkWithResult()
        {
            // Arrange
            var fileBytes = Encoding.UTF8.GetBytes("some,csv,data");
            var stream = new MemoryStream(fileBytes);
            var file = new FormFile(stream, 0, fileBytes.Length, "file", "meter-readings.csv");

            var expectedResult = new MeterReadingUploadResultDto(10,2);

            _mediatorMock
                .Setup(m => m.Send(
                    It.IsAny<UploadMeterReadingsCommand>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResult);

            var cancellationToken = CancellationToken.None;

            // Act
            var result = await _controller.Upload(file, cancellationToken);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var actualResult = Assert.IsType<MeterReadingUploadResultDto>(okResult.Value);            

            _mediatorMock.Verify(m => m.Send(
                    It.Is<UploadMeterReadingsCommand>(cmd =>
                        cmd.FileBytes != null &&
                        cmd.FileBytes.Length == fileBytes.Length &&
                        cmd.FileBytes.SequenceEqual(fileBytes)),
                    cancellationToken),
                Times.Once);
        }
    }
}
