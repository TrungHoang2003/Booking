using Dapper;
using Property.Domain.Aggregates.LanguageAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface ILanguageRepository: IGenericRepository<Language>
{
   Task<Language?> GetById(int id); 
}

public class LanguageRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<Language>(dbContext), ILanguageRepository
{
   public async  Task<Language?> GetById(int id)
   {
      var cnn = server.OpenConnection();
      
      const string sql = "Select * from \"Languages\" where \"Id\" = @id";
      var result = await cnn.QueryFirstOrDefaultAsync<Language>(sql, new { id });
      return result;
   }
}