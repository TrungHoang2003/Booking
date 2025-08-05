using System.Text.Json;
using Contracts.Drafts;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Api.Sagas;
using MassTransit;
using MassTransit.EntityFrameworkCoreIntegration;

namespace Orchestrator.Api.Database;

public class SagaDbContext(DbContextOptions<SagaDbContext> options): DbContext(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<BecomeHostSagaData>(entity =>
      {
         entity.HasKey(x => x.CorrelationId);
         
         // MassTransit sẽ tự động map CurrentState
         entity.Property(x => x.CurrentState)
            .HasMaxLength(64);

         // Chỉ custom map cho Draft field
         entity.Property(x => x.Draft)
            .HasConversion(
               v => JsonSerializer.Serialize(v, new JsonSerializerOptions()),
               v => JsonSerializer.Deserialize<BecomeHostDraft>(v, new JsonSerializerOptions())!
            );
      });
   }
}