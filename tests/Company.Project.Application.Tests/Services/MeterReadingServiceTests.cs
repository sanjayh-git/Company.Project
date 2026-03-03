using System.Text;
using FluentAssertions;
using Company.Project.Application.Services;
using Company.Project.Data.Entities;
using Xunit;
using Company.Project.Domain.DTOs;
using Company.Project.Data.Repositories.Interfaces;
using Moq;
using Company.Project.Domain.Csv.Interfaces;

namespace Company.Project.Application.Tests.Services;

public class MeterReadingServiceTests
{
    private static MemoryStream CreateStream(string content)
    {
        return new MemoryStream(Encoding.UTF8.GetBytes(content));
    }

    [Fact]
    public async Task ProcessCsvAsync_ShouldStoreValidReading_AndReturnSuccess1Failure0()
    {
        var accountRepo = new Mock<IAccountRepository>();
        accountRepo.Setup(r => r.GetByAccountIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int accountId, CancellationToken ct) =>
            {
                if (accountId == 2344)
                {
                    return new Account { AccountId = 2344, FirstName = "Tommy", LastName = "Test" };
                }
                return null;
            });

        var rows = new[]
        {
            new[] { "2344", "22/04/2019 09:24", "01234" }
        };

        var meterRepo = new Mock<IMeterReadingRepository>();
        var csvParser = new Mock<ICsvParser>(rows);
        var sut = new MeterReadingService(accountRepo.Object, meterRepo.Object, csvParser.Object);

        using var stream = CreateStream("dummy");
        var result = await sut.ProcessCsvAsync(stream);

        result.Should().BeEquivalentTo(new MeterReadingUploadResultDto(1, 0));
    }

    [Fact]
    public async Task ProcessCsvAsync_ShouldFail_WhenAccountDoesNotExist()
    {
        var accountRepo = new Mock<IAccountRepository>();

        var rows = new[]
        {
            new[] { "9999", "22/04/2019 09:24", "01234" }
        };

        //var meterRepo = new FakeMeterReadingRepository();
        var meterRepo = new Mock<IMeterReadingRepository>();
        var csvParser = new Mock<ICsvParser>(rows);
        var sut = new MeterReadingService(accountRepo.Object, meterRepo.Object, csvParser.Object);

        using var stream = CreateStream("dummy");
        var result = await sut.ProcessCsvAsync(stream);

        result.SuccessCount.Should().Be(0);
        result.FailureCount.Should().Be(1);
    }

    [Fact]
    public async Task ProcessCsvAsync_ShouldFail_WhenReadingValueIsNotFiveDigits()
    {
        var accountRepo = new Mock<IAccountRepository>();
        accountRepo.Setup(r => r.GetByAccountIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int accountId, CancellationToken ct) =>
            {
                if (accountId == 2344)
                {
                    return new Account { AccountId = 2344, FirstName = "Tommy", LastName = "Test" };
                }
                return null;
            });

        var rows = new[]
        {
            new[] { "2344", "22/04/2019 09:24", "1234" },
            new[] { "2344", "22/04/2019 09:25", "123456" },
            new[] { "2344", "22/04/2019 09:26", "12A34" }
        };

        var meterRepo = new Mock<IMeterReadingRepository>();
        var csvParser = new Mock<ICsvParser>(rows);
        var sut = new MeterReadingService(accountRepo.Object, meterRepo.Object, csvParser.Object);

        using var stream = CreateStream("dummy");
        var result = await sut.ProcessCsvAsync(stream);

        result.SuccessCount.Should().Be(0);
        result.FailureCount.Should().Be(3);
    }

    [Fact]
    public async Task ProcessCsvAsync_ShouldNotAllowDuplicateMeterReading_ForSameAccountAndTimestamp()
    {
        var accountRepo = new Mock<IAccountRepository>();
        accountRepo.Setup(r => r.GetByAccountIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int accountId, CancellationToken ct) =>
            {
                if (accountId == 2344)
                {
                    return new Account { AccountId = 2344, FirstName = "Tommy", LastName = "Test" };
                }
                return null;
            });

        var rows = new[]
        {
            new[] { "2344", "22/04/2019 09:24", "01234" },
            new[] { "2344", "22/04/2019 09:24", "01235" }
        };

        var meterRepo = new Mock<IMeterReadingRepository>();
        var csvParser = new Mock<ICsvParser>(rows);
        var sut = new MeterReadingService(accountRepo.Object, meterRepo.Object, csvParser.Object);

        using var stream = CreateStream("dummy");
        var result = await sut.ProcessCsvAsync(stream);

        result.SuccessCount.Should().Be(1);
        result.FailureCount.Should().Be(1);
    }

    [Fact]
    public async Task ProcessCsvAsync_ShouldRejectNewReading_WhenOlderThanExistingReadingForAccount()
    {
        var accountRepo = new Mock<IAccountRepository>();
        accountRepo.Setup(r => r.GetByAccountIdAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync((int accountId, CancellationToken ct) =>
            {
                if (accountId == 2344)
                {
                    return new Account { AccountId = 2344, FirstName = "Tommy", LastName = "Test" };
                }
                return null;
            });

        var rows = new[]
        {
            new[] { "2344", "22/04/2019 10:00", "01234" },
            new[] { "2344", "22/04/2019 09:00", "01235" }
        };

        var meterRepo = new Mock<IMeterReadingRepository>();
        var csvParser = new Mock<ICsvParser>(rows);
        var sut = new MeterReadingService(accountRepo.Object, meterRepo.Object, csvParser.Object);

        using var stream = CreateStream("dummy");
        var result = await sut.ProcessCsvAsync(stream);

        result.SuccessCount.Should().Be(1);
        result.FailureCount.Should().Be(1);
    }
}
