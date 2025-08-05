using System.Text.Json;
using Contracts.Drafts;
using Microsoft.EntityFrameworkCore;
using Orchestrator.Application.Sagas;
using Orchestrator.Domain.Models;

namespace Orchestrator.Infrastructure.Database;

public class SagaDbContext(DbContextOptions<SagaDbContext> options): DbContext(options)
{
   protected override void OnModelCreating(ModelBuilder modelBuilder)
   {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<BecomeHostSagaData>(entity =>
      {
         entity.HasKey(x => x.CorrelationId);
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