using Microsoft.AspNetCore.SignalR;
using TeamCollaboration.Application.DTOs;

namespace TeamCollaboration.Application.RealTime;

/// <summary>
/// SignalR hub responsible for broadcasting task updates to connected clients.
/// </summary>
public class KanbanHub : Hub
{
    public const string HubPath = "/hubs/kanban";

    public async Task BroadcastTaskCreated(TaskDto task)
    {
        await Clients.Others.SendAsync("TaskCreated", task);
    }

    public async Task BroadcastTaskUpdated(TaskDto task)
    {
        await Clients.Others.SendAsync("TaskUpdated", task);
    }

    public async Task BroadcastTaskDeleted(Guid taskId)
    {
        await Clients.Others.SendAsync("TaskDeleted", taskId);
    }
}

