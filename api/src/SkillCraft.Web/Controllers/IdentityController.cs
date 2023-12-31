﻿using Logitar.AspNetCore.Identity;
using Logitar.Identity.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SkillCraft.Web.Models.Identity;

namespace SkillCraft.Web.Controllers
{
  [ApiController]
  [Route("identity")]
  public class IdentityController : ControllerBase
  {
    private readonly IIdentityService _identityService;

    public IdentityController(IIdentityService identityService)
    {
      _identityService = identityService;
    }

    [HttpPost("confirm")]
    public async Task<ActionResult> ConfirmAsync(
      [FromBody] ConfirmPayload payload,
      CancellationToken cancellationToken
    )
    {
      await _identityService.ConfirmAsync(payload, cancellationToken);

      return NoContent();
    }

    [Authorize]
    [HttpGet("profile")]
    public async Task<ActionResult<ProfileModel>> GetProfileAsync(CancellationToken cancellationToken)
    {
      User user = await _identityService.GetUserAsync(cancellationToken);

      return Ok(new ProfileModel(user));
    }

    [HttpPost("renew")]
    public async Task<ActionResult<TokenModel>> RenewAsync(
      [FromBody] RenewPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _identityService.RenewAsync(payload, cancellationToken));
    }

    [HttpPost("sign/in")]
    public async Task<ActionResult<TokenModel>> SignInAsync(
      [FromBody] SignInPayload payload,
      CancellationToken cancellationToken
    )
    {
      return Ok(await _identityService.SignInAsync(payload, cancellationToken));
    }

    [Authorize]
    [HttpPost("sign/out")]
    public async Task<ActionResult> SignOutAsync(
      [FromBody] SignOutPayload payload,
      CancellationToken cancellationToken
    )
    {
      await _identityService.SignOutAsync(payload, cancellationToken);

      return NoContent();
    }

    [HttpPost("sign/up")]
    public async Task<ActionResult> SignUpAsync(
      [FromBody] SignUpPayload payload,
      CancellationToken cancellationToken
    )
    {
      SignUpResult result = await _identityService.SignUpAsync(payload, cancellationToken: cancellationToken);

      if (result.Token != null)
      {
        return Ok(new ConfirmPayload
        {
          Id = result.User.Id,
          Token = result.Token
        }); // TODO(fpion): implement
      }

      return NoContent();
    }
  }
}
