using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace VideoOS.Mobile.Helpers;

public class SanitizerWrapper : ISanitizerConfiguration
{
	private uint _maxAllowedLength = 255u;

	private char _replacingCharacter = '_';

	private string _invalidCharacters = "\\/:*?!.\"<>|+";

	[ExcludeFromCodeCoverage]
	public string InvalidCharacters
	{
		get
		{
			return _invalidCharacters;
		}
		set
		{
			_invalidCharacters = value;
		}
	}

	[ExcludeFromCodeCoverage]
	public char ReplacingCharacter
	{
		get
		{
			return _replacingCharacter;
		}
		set
		{
			_replacingCharacter = value;
		}
	}

	[ExcludeFromCodeCoverage]
	public uint MaxAllowedLength
	{
		get
		{
			return _maxAllowedLength;
		}
		set
		{
			_maxAllowedLength = value;
		}
	}

	[ExcludeFromCodeCoverage]
	public SanitizerWrapper()
	{
	}

	[ExcludeFromCodeCoverage]
	public SanitizerWrapper(string invalidCharacters)
	{
		_invalidCharacters = invalidCharacters;
		_maxAllowedLength = uint.MaxValue;
	}

	[ExcludeFromCodeCoverage]
	public SanitizerWrapper(string invalidCharacters, char replacingCharacter, uint maxAllowedLength)
	{
		_invalidCharacters = invalidCharacters;
		_replacingCharacter = replacingCharacter;
		_maxAllowedLength = maxAllowedLength;
	}

	public SanitizerError ValidateString(string input, string exceptionCharacters = null, uint maxAllowedLength = 0u)
	{
		return CharactersSanitizer.ValidateCustomInvalidCharsAndLength(input, _invalidCharacters, exceptionCharacters, (maxAllowedLength == 0) ? _maxAllowedLength : maxAllowedLength);
	}

	public IEnumerable<SanitizerError> ValidateStrings(IEnumerable<string> input, string exceptionCharacters = null, uint maxAllowedLength = 0u)
	{
		return input.Select((string str) => ValidateString(str, exceptionCharacters, maxAllowedLength));
	}

	public string SanitizeString(string input, string invalidCharacters = null, char replacingCharacter = '\0')
	{
		return input.SanitizedCustom(invalidCharacters ?? _invalidCharacters, (replacingCharacter != 0) ? replacingCharacter : _replacingCharacter);
	}

	public IEnumerable<string> SanitizeStrings(IEnumerable<string> input, string invalidCharacters = null, char replacingCharacter = '\0')
	{
		return input.Select((string str) => SanitizeString(str, invalidCharacters, replacingCharacter));
	}
}
