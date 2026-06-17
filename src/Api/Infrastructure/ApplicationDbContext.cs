using Api.Features.ProjectTasks;
using Api.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Api.Infrastructure;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
    AuditableEntityInterceptor auditableInterceptor) : DbContext(options)
{
    public DbSet<ProjectTask> ProjectTasks => Set<ProjectTask>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(auditableInterceptor);
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }
}