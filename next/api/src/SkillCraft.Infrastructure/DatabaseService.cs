using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace SkillCraft.Infrastructure
{
  internal class DatabaseService : IDatabaseService
  {
    private readonly IConfiguration _configuration;
    private readonly SkillCraftDbContext _dbContext;

    public DatabaseService(IConfiguration configuration, SkillCraftDbContext dbContext)
    {
      _configuration = configuration;
      _dbContext = dbContext;
    }

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
      if (_configuration.GetValue<bool>("MigrateDatabase"))
      {
        await _dbContext.Database.MigrateAsync(cancellationToken);
      }
    }
  }
}
