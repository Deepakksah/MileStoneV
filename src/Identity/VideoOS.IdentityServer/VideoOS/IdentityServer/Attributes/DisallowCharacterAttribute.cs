using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VideoOS.IdentityServer.Attributes;

public class DisallowCharacterAttribute : ValidationAttribute
{
	private readonly char[] _notAllowedCharacters;

	public DisallowCharacterAttribute(params char[] notAllowedCharacters)
	{
		_notAllowedCharacters = notAllowedCharacters ?? throw new ArgumentNullException("notAllowedCharacters");
	}

	protected override ValidationResult IsValid(object value, ValidationContext validationContext)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Expected O, but got Unknown
		if (value == null || !_notAllowedCharacters.Any())
		{
			return ValidationResult.Success;
		}
		char[] notAllowedCharacters = _notAllowedCharacters;
		foreach (char value2 in notAllowedCharacters)
		{
			if (((string)value).Contains(value2))
			{
				return new ValidationResult(GetErrorMessage());
			}
		}
		return ValidationResult.Success;
	}

	private string GetErrorMessage()
	{
		return "Cannot have invalid characters (" + string.Join(", ", _notAllowedCharacters) + ")";
	}
}
