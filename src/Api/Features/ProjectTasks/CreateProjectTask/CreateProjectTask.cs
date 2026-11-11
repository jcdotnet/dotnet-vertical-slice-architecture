using Api.Common.Models;
using Api.Infrastructure;
using FluentValidation;
using MediatR;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;

namespace Api.Features.ProjectTasks.CreateProjectTask;

public record CreateProjectTaskCommand( // Request DTO
    string Title,
    string Description,
    PriorityLevel Priority,
    DateTime? DueDate) : IRequest<Result<Guid>>;

public class CreateProjectTaskCommandValidator : AbstractValidator<CreateProjectTaskCommand>
{
    public CreateProjectTaskCommandValidator()
    {
        RuleFor(v => v.Title)
            .NotEmpty().WithMessage("Title is required.")
            .MaximumLength(100).WithMessage("Title must not exceed 100 characters.");

        RuleFor(v => v.Description)
            .MaximumLength(500).WithMessage("Description must not exceed 500 characters.");

        RuleFor(v => v.DueDate)
            .Must(date => !date.HasValue || date.Value > DateTime.UtcNow)
            .WithMessage("The due date must be in the future.");
    }
}

public class CreateProjectTaskCommandHandler(ApplicationDbContext context) :
    IRequestHandler<CreateProjectTaskCommand, Result<Guid>>
{
    public async Task<Result<Guid>> Handle(CreateProjectTaskCommand request, CancellationToken cancellationToken)
    {
        var result = ProjectTask.Create(
            request.Title,
            request.Description,
            request.Priority,
            request.DueDate);

        return await (result switch
        {
            ProjectTask projectTask => SaveAsync(projectTask),
            Error error => Task.FromResult<Result<Guid>>(error),
            null => Task.FromResult<Result<Guid>>(Error.NullValue)
        });

        async Task<Result<Guid>> SaveAsync(ProjectTask projectTask)
        {
            context.ProjectTasks.Add(projectTask);
            await context.SaveChangesAsync(cancellationToken);

            return projectTask.Id;
        }
    }

}