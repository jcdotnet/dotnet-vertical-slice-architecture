using Api.Common.Models;
using MediatR;

namespace Api.Features.ProjectTasks.CreateProjectTask;

public static class CreateProjectTaskEndpoint
{
    extension(IEndpointRouteBuilder app)
    {
        public void MapCreateProjectTask()
        {
            app.MapPost("api/project-tasks", async (CreateProjectTaskCommand command, ISender mediator) =>
            {
                var result = await mediator.Send(command); // Result<Guid>

                
                return result.IsSuccess ? Results.Ok(result.Value) : result.ToHttpResult();

            })
            // These methods create the OpenApi ID, summary and description for the Swagger UI (if added)
            .WithName("CreateProjectTask")
            .WithSummary("Creates a new project task");
            // .WithDescription();
        }
    }
}
