using Api.Common.Models;
using MediatR;

namespace Api.Features.ProjectTasks.GetProjectTaskById;

public static class GetProjectTaskByIdEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public void MapGetProjectTaskById()
        {
            app.MapGet("api/project-tasks/{id:guid}", async (Guid id, ISender mediator) =>
            {
            
                var result = await mediator.Send(new GetProjectTaskByIdQuery(id)); // Result<ProjectTaskDto>


                return result.ToHttpResult();
            })
            .WithName("GetProjectTask")
            .WithSummary("Retrieves project task by Id");
        }
    }
}
