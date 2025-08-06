using Dapper;
using Property.Domain.Aggregates.LanguageAggregate;
using Property.Infrastructure.DbHelper;

namespace Property.Infrastructure.Repositories;

public interface ILanguageRepository: IGenericRepository<Language>
{
}

public class LanguageRepository(PropertyDbContext dbContext, PostgresServer server) : GenericRepository<Language>(dbContext), ILanguageRepository
{
}