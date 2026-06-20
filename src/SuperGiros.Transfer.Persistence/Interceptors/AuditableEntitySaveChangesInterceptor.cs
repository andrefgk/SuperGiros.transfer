using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SuperGiros.Transfer.Domain.Common;
using SuperGiros.Transfer.Domain.Entities;
using SuperGiros.Transfer.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SuperGiros.Transfer.Persistence.Interceptors
{
    public class AuditableEntitySaveChangesInterceptor: SaveChangesInterceptor
    {
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChanges(eventData, result);
        }

        public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
        {
            UpdateEntities(eventData.Context);
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        public void UpdateEntities(DbContext? context)
        {
            if (context == null) return;

            foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
            {
                if (entry.State == EntityState.Added)
                {
                    entry.Entity.CreatedBy = "system";
                    entry.Entity.Created = DateTime.Now;
                    entry.Entity.State = State.Activo;
                }

                if (entry.State == EntityState.Modified)
                {
                    entry.Entity.LastModififyBy = "system";
                    entry.Entity.LastModifify = DateTime.Now;
                }

                // Lógica de hasheo para AMBOS casos (Nuevo o Modificado)
                if (entry.Entity is User user)
                {
                    // Solo hashear si la contraseña no es ya un hash de BCrypt
                    if (!string.IsNullOrEmpty(user.PasswordHash) && !user.PasswordHash.StartsWith("$2a$"))
                    {
                        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(user.PasswordHash);
                    }
                }
            }
        }
    }
}
