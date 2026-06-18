using Api.Common.Models;
using Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Riok.Mapperly.Abstractions;

namespace Api.Features.ProjectTasks.GetProjectTaskById;


public record ProjectTaskDto(
    Guid Id, string Title, string Description, PriorityLevel Priority, bool IsCompleted, DateTime? DueDate
);

public record GetProjectTaskByIdQuery(Guid Id) : IRequest<Result<ProjectTaskDto>>;


[Mapper]
public partial class GetProjectTaskByIdMapper
{
    [MapperIgnoreSource(nameof(ProjectTask.CreatedAt))]
    public partial ProjectTaskDto ProjectTaskToDto(ProjectTask entity);
}

public class GetProjectTaskByIdQueryHandler(ApplicationDbContext context) :
    IRequestHandler<GetProjectTaskByIdQuery, Result<ProjectTaskDto>>
{
    public async Task<Result<ProjectTaskDto>> Handle(GetProjectTaskByIdQuery request,
        CancellationToken cancellationToken)
    {
        var mapper = new GetProjectTaskByIdMapper();
        var entity = await context.ProjectTasks
        .AsNoTracking()
        .FirstOrDefaultAsync(t => t.Id == request.Id, cancellationToken);

        if (entity is null)
        {
            return Result.FailureResult<ProjectTaskDto>(
                Error.NotFound("ProjectTask.NotFound", $"Task {request.Id} not found."));
        }
        return mapper.ProjectTaskToDto(entity);
    }
}

