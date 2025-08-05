using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.Drafts;
using Orchestrator.Api.Sagas;
using MassTransit;

namespace Orchestrator.Api.Database;

public class SagaDbContext(DbContextOptions<SagaDbContext> options): DbContext(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<BecomeHostSagaData>(entity =>
      {
         entity.HasKey(x => x.CorrelationId);
         entity.Property(x => x.HostId);
         entity.Property(x => x.PropertyId);

         entity.Property(x => x.CurrentState)
            .HasConversion(
               v => v.Name,
               v => (State)Activator.CreateInstance(typeof(State), v)!
            )
            .HasMaxLength(64);

         entity.Property(x => x.Draft)
            .HasConversion(
               v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
               v => JsonSerializer.Deserialize<BecomeHostDraft>(v, new JsonSerializerOptions())!
            );
      });
   }
}