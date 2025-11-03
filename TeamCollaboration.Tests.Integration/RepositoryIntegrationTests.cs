using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using TeamCollaboration.Application.RealTime;
using TeamCollaboration.Application.Services;
using TeamCollaboration.Domain.Entities;
using TeamCollaboration.Domain.Repositories;
using TeamCollaboration.Infrastructure.Persistence;
using TeamCollaboration.Infrastructure.Repositories;

namespace TeamCollaboration.Tests.Integration;

public class RepositoryIntegrationTests
{
    [Fact]
    public async Task TaskRepository_Add_And_GetByBoard_Works_With_InMemoryDb()
    {
        var dbName = $"tc_tests_{Guid.NewGuid()}";
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(dbName)
            .Options;

        await using var db = new ApplicationDbContext(options);
        ITaskRepository repo = new TaskRepository(db);

        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();

        var a = new TaskItem(boardId, columnId, "A");
        a.MoveTo(columnId, Domain.Enums.TaskStatus.ToDo, 2);
        var b = new TaskItem(boardId, columnId, "B");
        b.MoveTo(columnId, Domain.Enums.TaskStatus.ToDo, 1);

        await repo.AddAsync(a);
        await repo.AddAsync(b);

        var items = await repo.GetByBoardAsync(boardId);
        items.Should().HaveCount(2);
        items.Select(i => i.Title).Should().ContainInOrder("B", "A");
    }

    [Fact]
    public async Task TaskService_Create_Persists_And_Publishes()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase($"svc_tests_{Guid.NewGuid()}")
            .Options;

        await using var db = new ApplicationDbContext(options);
        ITaskRepository repo = new TaskRepository(db);

        var clients = new Mock<IHubClients>();
        var clientProxy = new Mock<IClientProxy>();
        clients.Setup(c => c.All).Returns(clientProxy.Object);
        clientProxy.Setup(c => c.SendCoreAsync(It.IsAny<string>(), It.IsAny<object?[]>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask)
            .Verifiable();
        var hub = new Mock<IHubContext<KanbanHub>>();
        hub.SetupGet(h => h.Clients).Returns(clients.Object);

        var logger = new Mock<ILogger<TaskService>>();
        var service = new TaskService(repo, hub.Object, logger.Object);

        var boardId = Guid.NewGuid();
        var columnId = Guid.NewGuid();
        var dto = new Application.DTOs.TaskItemDto
        {
            Id = Guid.NewGuid(),
            BoardId = boardId,
            BoardColumnId = columnId,
            Title = "Hello",
            SortOrder = 0
        };

        var (created, errors) = await service.CreateAsync(dto);

        errors.Should().BeEmpty();
        created.Should().NotBeNull();
        (await repo.GetByBoardAsync(boardId)).Should().ContainSingle(x => x.Title == "Hello");
        clientProxy.Verify();
    }
}


