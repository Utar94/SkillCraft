using Logitar.WebApiToolKit.Core.Exceptions;
using SkillCraft.Core.Customizations;
using System.Text;

namespace SkillCraft.Core.Natures
{
  internal class InvalidCustomizationTypeException : BadRequestException
  {
    public InvalidCustomizationTypeException(Customization customization, CustomizationType expectedType)
      : base("InvalidCustomizationType", GetMessage(customization, expectedType))
    {
      Customization = customization ?? throw new ArgumentNullException(nameof(customization));
      ExpectedType = expectedType;
    }

    public Customization Customization { get; }
    public CustomizationType ExpectedType { get; }

    private static string GetMessage(Customization customization, CustomizationType expectedType)
    {
      var message = new StringBuilder();

      message.AppendLine("The customization type does not match the expected type.");
      message.AppendLine($"Customization: {customization}");
      message.AppendLine($"Expected type: {expectedType}");
      message.AppendLine($"Actual type: {customization?.Type}");

      return message.ToString();
    }
  }
}
