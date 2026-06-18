using Api.Common.Models;
using Api.Infrastructure;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Riok.Mapperly.Abstractions;

namespace Api.Features.ProjectTasks.GetProjectTasks;

public record ProjectTaskDto(
    Guid Id, string Title, string Description, PriorityLevel Priority, bool IsCompleted, DateTime? DueDate
);

public record GetProjectTasksQuery : IRequest<Result<IEnumerable<ProjectTaskDto>>>;

[Mapper]
public partial class GetProjectTasksMapper
{
    [MapperIgnoreSource(nameof(ProjectTask.CreatedAt))]
    public partial ProjectTaskDto ProjectTaskToDto(ProjectTask entity);
}

public class GetProjectTasksQueryHandler(ApplicationDbContext context)
    : IRequestHandler<GetProjectTasksQuery, Result<IEnumerable<ProjectTaskDto>>>
{
    public async Task<Result<IEnumerable<ProjectTaskDto>>> Handle(
         GetProjectTasksQuery request,
         CancellationToken cancellationToken)
    {
        var mapper = new GetProjectTasksMapper();

        var tasks = await context.ProjectTasks
            .AsNoTracking()
            .ToListAsync(cancellationToken);


        return tasks.Select(mapper.ProjectTaskToDto).ToList();
    }
}
