using Microsoft.AspNetCore.SignalR;
using TeamCollaboration.Application.DTOs;
using TeamCollaboration.Application.Services;

namespace TeamCollaboration.Application.RealTime;


public class KanbanHub(ITaskService taskService) : Hub
{
    public const string HubPath = "/hubs/kanban";
    
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

