using Logitar.Identity.Core;

namespace SkillCraft.Web.Email
{
  public interface IEmailService
  {
    Task SendPasswordRecoveryAsync(RecoverPasswordResult result, CancellationToken cancellationToken = default);
    Task SendSignUpAsync(SignUpResult result, CancellationToken cancellationToken = default);
  }
}
