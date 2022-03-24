using System.ComponentModel.DataAnnotations;

namespace SkillCraft.Web.Settings
{
  public class ApplicationSettings
  {
    [Required]
    [Url]
    public string BaseUrl { get; set; } = null!;
  }
}
