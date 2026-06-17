using Api.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Api.Infrastructure.Persistence;

public class AuditableEntityInterceptor : SaveChangesInterceptor
{
    // Synchronous version to ensure auditing works even if SaveChanges() is called
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        SetCreationMetadata(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    // Asynchronous version for standard web requests (SaveChangesAsync)
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        SetCreationMetadata(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private static void SetCreationMetadata(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<IAuditable>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }
        }
    }
}
