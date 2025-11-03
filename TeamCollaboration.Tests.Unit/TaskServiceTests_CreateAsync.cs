using System.ComponentModel.DataAnnotations;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Moq;
using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Application.RealTime;
using TeamCollaboration.Application.Services;
using TeamCollaboration.Domain.Entities;
using TeamCollaboration.Domain.Repositories;

namespace TeamCollaboration.Tests.Unit;

public class TaskServiceTests_CreateAsync
{
    [Fact]
    public async Task CreateAsync_WhenTitleMissing_ReturnsValidationError_AndDoesNotCallRepository()
    {
        var repo = new Mock<ITaskRepository>(MockBehavior.Strict);
        var hub = new Mock<IHubContext<KanbanHub>>();
        var logger = new Mock<ILogger<TaskService>>();
        var service = new TaskService(repo.Object, hub.Object, logger.Object);

        var request = new TaskItemDto
        {
            Id = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Title = string.Empty,
            SortOrder = 0
        };

        var (task, errors) = await service.CreateAsync(request);

        task.Should().BeNull();
        errors.Should().NotBeEmpty();
        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public async Task CreateAsync_WithValidRequest_PersistsAndReturnsDto()
    {
        var repo = new Mock<ITaskRepository>();
        repo.Setup(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var clients = new Mock<IHubClients>();
        var clientProxy = new Mock<IClientProxy>();
        clients.Setup(c => c.All).Returns(clientProxy.Object);
        clientProxy.Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var hub = new Mock<IHubContext<KanbanHub>>();
        hub.SetupGet(h => h.Clients).Returns(clients.Object);

        var logger = new Mock<ILogger<TaskService>>();
        var service = new TaskService(repo.Object, hub.Object, logger.Object);

        var request = new TaskItemDto
        {
            Id = Guid.NewGuid(),
            BoardId = Guid.NewGuid(),
            BoardColumnId = Guid.NewGuid(),
            Title = "Test",
            SortOrder = 1
        };

        var (task, errors) = await service.CreateAsync(request);

        errors.Should().BeEmpty();
        task.Should().NotBeNull();
        task!.Title.Should().Be("Test");
        repo.Verify(r => r.AddAsync(It.IsAny<TaskItem>(), It.IsAny<CancellationToken>()), Times.Once);
    }
}


