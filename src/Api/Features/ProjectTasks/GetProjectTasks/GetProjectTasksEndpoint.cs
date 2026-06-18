using Api.Common.Models;
using MediatR;

namespace Api.Features.ProjectTasks.GetProjectTasks;
public static class GetProjectTasksEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public void MapGetProjectTasks()
        {
            app.MapGet("api/project-tasks", async (ISender mediator) =>
            {
                var query = new GetProjectTasksQuery();
                var result = await mediator.Send(query); // Result<IEnumerable<ProjectTaskDto>>

                return result.IsSuccess ? Results.Ok(result) : result.ToHttpResult();
            })
            .WithName("GetProjectTasks")
            .WithSummary("Retrieves all project tasks");
        }
    }
}
