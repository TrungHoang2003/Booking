using Microsoft.EntityFrameworkCore;

namespace Property.Infrastructure.DbHelper;

public class PropertyDbContext(DbContextOptions<PropertyDbContext> options): DbContext(options)
{
    
}