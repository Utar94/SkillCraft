using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Characters;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Mutations;
using SkillCraft.Core.Characters.Payloads;
using SkillCraft.Core.Characters.Queries;
using SkillCraft.Web.Filters;
using SkillCraft.Web.Pipeline;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Authorize]
  [RequireWorld]
  [Route("characters")]
  public class CharacterController : ControllerBase
  {
    private readonly IRequestPipeline _pipeline;

    public CharacterController(IRequestPipeline pipeline)
    {
      _pipeline = pipeline;
    }

    [HttpPost("steps/2")]
    public async Task<ActionResult<CharacterModel>> CreateStep2Async(
      [FromBody] SaveCharacterStep2Payload payload,
      CancellationToken cancellationToken
    )
    {
      CharacterModel model = await _pipeline.ExecuteAsync(new SaveCharacterStep2Mutation(payload), cancellationToken);
      var uri = new Uri($"/characters/{model.Id}", UriKind.Relative);

      return Created(uri, model);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<CharacterModel>> DeleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new DeleteCharacterMutation(id), cancellationToken));
    }

    [HttpGet]
    public async Task<ActionResult<CharacterModel>> GetAsync(
      bool? deleted,
      string? search,
      CharacterSort? sort,
      bool desc,
      int? index,
      int? count,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new GetCharactersQuery
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
    public async Task<ActionResult<CharacterModel>> GetAsync(Guid id, CancellationToken cancellationToken)
    {
      CharacterModel? model = await _pipeline.ExecuteAsync(new GetCharacterQuery(id), cancellationToken);
      if (model == null)
      {
        return NotFound();
      }

      return Ok(model);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<CharacterModel>> UpdateAsync(
      Guid id,
      [FromBody] UpdateCharacterPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new UpdateCharacterMutation(id, payload), cancellationToken));
    }

    [HttpPut("{id}/steps/2")]
    public async Task<ActionResult<CharacterModel>> UpdateStep2Async(
      Guid id,
      [FromBody] SaveCharacterStep2Payload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new SaveCharacterStep2Mutation(payload, id), cancellationToken));
    }

    [HttpPut("{id}/steps/3")]
    public async Task<ActionResult<CharacterModel>> UpdateStep3Async(
      Guid id,
      [FromBody] SaveCharacterStep3Payload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new SaveCharacterStep3Mutation(id, payload), cancellationToken));
    }

    [HttpPut("{id}/steps/4")]
    public async Task<ActionResult<CharacterModel>> UpdateStep4Async(
      Guid id,
      [FromBody] SaveCharacterStep4Payload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new SaveCharacterStep4Mutation(id, payload), cancellationToken));
    }

    [HttpPatch("{id}/complete")]
    public async Task<ActionResult<CharacterModel>> CompleteAsync(Guid id, CancellationToken cancellationToken)
    {
      return Ok(await _pipeline.ExecuteAsync(new CompleteCharacterMutation(id), cancellationToken));
    }

    [HttpPatch("{id}/level-up")]
    public async Task<ActionResult<CharacterModel>> LevelUpAsync(
      Guid id,
      [FromBody] LevelUpCharacterPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _pipeline.ExecuteAsync(new LevelUpCharacterMutation(id, payload), cancellationToken));
    }
  }
}
