using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Races;
using SkillCraft.Core.Races.Models;
using SkillCraft.Core.Races.Mutations;
using SkillCraft.Core.Races.Payloads;
using SkillCraft.Core.Races.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("races")]
  public class RaceController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public RaceController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost]
    public async Task<ActionResult<RaceModel>> CreateAsync(
      [FromBody] CreateRacePayload payload,
      CancellationToken cancellationToken
    )
    {
      RaceModel model = await _pipeline.ExecuteAsync(new CreateRaceMutation(payload), cancellationToken);
      var uri = new Uri($"/races/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<RaceModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteRaceMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<RaceModel>> GetAsync(
      bool? deleted,
      string? search,
      RaceSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetRacesQuery
      {
        Deleted = deleted,
        Search = search,
        Sort = sort,
        Desc = desc,
        Index = index,
        Count = count
      }, cancellationToken));
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<RaceModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      RaceModel? model = await _pipeline.ExecuteAsync(new GetRaceQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<RaceModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateRacePayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateRaceMutation(id, payload), cancellationToken));
    }
  }
}
