using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Seminario.Datos.Entidades.Interfaces;
using Seminario.Datos.Services.CurrentUserService;

namespace Seminario.Datos.Contextos.SaveChangesInterceptors;

public class AuditSaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUser;

    public AuditSaveChangesInterceptor(ICurrentUserService currentUser)
    {
        _currentUser = currentUser;
    }
    
    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData, 
        InterceptionResult<int> result)
    {
        var context = eventData.Context!;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                    auditable.CreatedAt(DateTime.Now, _currentUser?.Name);
                else if (entry.State == EntityState.Modified)
                    auditable.ModifiedAt(DateTime.Now, _currentUser?.Name);
            }

            if (entry.Entity is ICreatedTrigger created && entry.State == EntityState.Added)
                created.Created();

            if (entry.Entity is IModifiedTrigger modified && entry.State == EntityState.Modified)
                modified.Modified();
        }

        return result;
    }

    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context!;

        var entries = context.ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.Entity is IAuditable auditable)
            {
                if (entry.State == EntityState.Added)
                    auditable.CreatedAt(DateTime.Now, _currentUser?.Name);
                else if (entry.State == EntityState.Modified)
                    auditable.ModifiedAt(DateTime.Now, _currentUser?.Name);
            }

            if (entry.Entity is ICreatedTrigger created && entry.State == EntityState.Added)
                created.Created();

            if (entry.Entity is IModifiedTrigger modified && entry.State == EntityState.Modified)
                modified.Modified();
        }

        return result;
    }
}