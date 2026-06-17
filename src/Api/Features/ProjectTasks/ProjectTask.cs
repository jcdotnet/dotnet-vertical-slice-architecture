namespace Api.Features.ProjectTasks;

public class ProjectTask
{
    public Guid Id { get; private set; }

    public string Title { get; private set; }
    public string Description
    {
        get;
        private set => field = value?.Trim() ?? string.Empty;
    }

    public PriorityLevel Priority { get; private set; }
    public bool IsCompleted { get; private set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? DueDate { get; private set; }

    // TODO later: change to private
    public ProjectTask(string title, string description, PriorityLevel priority, DateTime? dueDate)
    {
        Id = Guid.NewGuid();
        Title = title;
        Description = description;
        Priority = priority;
        DueDate = dueDate;
        IsCompleted = false;
    }

    // TODO: factory method that ensures the task has a valid state
    //public static Result<ProjectTask> Create(string title, string description, PriorityLevel priority,
    //    DateTime? dueDate)
    //{
    //    if (string.IsNullOrWhiteSpace(title))
    //    {
    //        return Result.FailureResult<ProjectTask>(Error.TitleRequired);
    //    }

    //    return new ProjectTask(title, description, priority, dueDate);
    //}

    public void MarkAsCompleted() => IsCompleted = true;
}
