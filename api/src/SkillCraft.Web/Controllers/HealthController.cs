using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SkillCraft.Infrastructure;

namespace SkillCraft.Web.Controllers
{
  [ApiExplorerSettings(IgnoreApi = true)]
  [Route("health")]
  public class HealthController : ControllerBase
  {
    private readonly SkillCraftDbContext dbContext;

    public HealthController(SkillCraftDbContext dbContext)
    {
      this.dbContext = dbContext;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync(CancellationToken cancellationToken)
    {
      await dbContext.Users
        .AsNoTracking()
        .ToArrayAsync(cancellationToken);

      return NoContent();
    }
  }
}
