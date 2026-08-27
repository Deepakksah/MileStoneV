using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Common.Integration;

public class PasswordRequirements
{
	public const char SmallestPrintableCharacter = ' ';

	public const char LargestPrintableCharacter = '~';

	public bool NeedsUppercaseLetters { get; private set; }

	public bool NeedsLowercaseLetters { get; private set; }

	public bool NeedsDigits { get; private set; }

	public IList<CharacterSet> SetsOfRequiredCharacters { get; private set; }

	public CharacterSet ForbiddenCharacters { get; private set; }

	public static HashSet<char> ExcludedPrintableCharacters { get; } = new HashSet<char>
	{
		':', ';', '&', '%', '?', '\\', '/', '<', '>', '"',
		'\'', ',', '+', ' ', '{', '}', '^', '$', '[', ']',
		'`', '@', '=', '|', '~', '#'
	};

	public PasswordRequirements(IList<CharacterSet> setsOfRequiredCharacters, CharacterSet forbiddenCharacters, bool needsUppercaseLetters, bool needsLowercaseLetters, bool needsDigits)
	{
		SetsOfRequiredCharacters = setsOfRequiredCharacters;
		if (forbiddenCharacters != null)
		{
			ForbiddenCharacters = new CharacterSet(forbiddenCharacters.Union(ExcludedPrintableCharacters));
		}
		NeedsUppercaseLetters = needsUppercaseLetters;
		NeedsLowercaseLetters = needsLowercaseLetters;
		NeedsDigits = needsDigits;
	}

	public PasswordValidationResult ValidatePassword(string password)
	{
		if (NeedsDigits && !password.Any(char.IsDigit))
		{
			return PasswordValidationResult.NeedsDigits;
		}
		if (NeedsLowercaseLetters && !password.Any(char.IsLower))
		{
			return PasswordValidationResult.NeedsLowercase;
		}
		if (NeedsUppercaseLetters && !password.Any(char.IsUpper))
		{
			return PasswordValidationResult.NeedsUppercase;
		}
		bool flag = password.All((char c) => c > ' ' && c < '~');
		if (ForbiddenCharacters.CharactersList.Any(((IEnumerable<char>)password).Contains<char>) || !flag)
		{
			return PasswordValidationResult.UsesForbiddenCharacter;
		}
		if (!SetsOfRequiredCharacters.All((CharacterSet characterSet) => characterSet.CharactersList.Any(((IEnumerable<char>)password).Contains<char>)))
		{
			return PasswordValidationResult.MissingRequiredCharacter;
		}
		return PasswordValidationResult.Success;
	}
}
