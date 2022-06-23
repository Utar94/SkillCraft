using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Core.Characters.Models;
using SkillCraft.Core.Characters.Mutations;
using SkillCraft.Core.Characters.Payloads;
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
  }
}
