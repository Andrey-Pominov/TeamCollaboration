using System.Linq;
using Microsoft.AspNetCore.SignalR;
using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Application.Services;

namespace TeamCollaboration.Application.RealTime;

/// <summary>
/// SignalR hub responsible for coordinating task updates with connected clients.
/// </summary>
public class KanbanHub(ITaskService taskService) : Hub
{
    public const string HubPath = "/hubs/kanban";

    /// <summary>
    /// Handles client requests to move a task between columns.
    /// </summary>
    /// <exception cref="HubException">Thrown when the move fails validation or persistence.</exception>
    public async Task MoveTask(MoveTaskRequest request)
    {
        var (success, errors) = await taskService.MoveTaskAsync(request, Context.ConnectionAborted);
        if (!success)
        {
            var message = errors.FirstOrDefault()?.ErrorMessage ?? "Unable to move task.";
            throw new HubException(message);
        }
    }
}

