using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace VideoOS.Common.Integration;

public class PasswordRequirementsParser : IPasswordRequirementsParser
{
	private const string _MandatoryCharactersRegex = "m\\[([^\\]]*)\\]m";

	private const string _ForbiddenCharactersRegex = "f\\[([\\S\\s]*?)\\]f";

	private const string _AllUppercaseLetters = "_A";

	private const string _AllLowercaseLetters = "_a";

	private const string _AllDigits = "_0";

	private static CharacterSet _UppercaseSymbolTable = new CharacterSet(from num in Enumerable.Range(65, 26)
		select (char)num);

	private static CharacterSet _LowercaseSymbolTable = new CharacterSet(from num in Enumerable.Range(97, 26)
		select (char)num);

	private static CharacterSet _DigitsSymbolTable = new CharacterSet(from num in Enumerable.Range(48, 10)
		select (char)num);

	public PasswordRequirements GetRequirements(string passwordRequirements)
	{
		if (string.IsNullOrEmpty(passwordRequirements))
		{
			return new PasswordRequirements(new List<CharacterSet>(), new CharacterSet(), needsUppercaseLetters: false, needsLowercaseLetters: false, needsDigits: false);
		}
		List<string> list = Regex.Match(passwordRequirements, "m\\[([^\\]]*)\\]m").Groups[1].Value.Split(',').ToList();
		CharacterSet characterSet = new CharacterSet();
		characterSet = new CharacterSet(Regex.Match(passwordRequirements, "f\\[([\\S\\s]*?)\\]f").Groups[1].Value.ToCharArray().ToList());
		bool needsUppercaseLetters = list.Remove("_A");
		bool needsLowercaseLetters = list.Remove("_a");
		bool needsDigits = list.Remove("_0");
		PasswordRequirements requirements = GetRequirements(needsUppercaseLetters, needsLowercaseLetters, needsDigits);
		foreach (string item in list)
		{
			List<char> list2 = item.ToCharArray().ToList();
			if (list2.Count != 0)
			{
				requirements.SetsOfRequiredCharacters.Add(new CharacterSet(list2));
			}
		}
		return new PasswordRequirements(requirements.SetsOfRequiredCharacters, characterSet, requirements.NeedsUppercaseLetters, requirements.NeedsLowercaseLetters, requirements.NeedsDigits);
	}

	public PasswordRequirements GetRequirements(bool needsUppercaseLetters, bool needsLowercaseLetters, bool needsDigits)
	{
		IList<CharacterSet> list = new List<CharacterSet>();
		if (needsUppercaseLetters)
		{
			list.Add(_UppercaseSymbolTable);
		}
		if (needsLowercaseLetters)
		{
			list.Add(_LowercaseSymbolTable);
		}
		if (needsDigits)
		{
			list.Add(_DigitsSymbolTable);
		}
		return new PasswordRequirements(list, new CharacterSet(), needsUppercaseLetters, needsLowercaseLetters, needsDigits);
	}
}
