using System.Text;
using Microsoft.Extensions.Options;
using VideoOS.IdentityServer.Configuration;

namespace VideoOS.IdentityServer.Services;

public class PasswordRequirements : IPasswordRequirements
{
	private string _pattern;

	private const string RegexLowerCase = "(?=.*\\p{Ll})";

	private const string RegexUpperCase = "(?=.*\\p{Lu})";

	private const string RegexSpecialChar = "(?=.*[~!@#$%^&*_+=`|\\(){}\\[\\]:;\"'<>,.?\\/-])";

	private const string RegexMinLength = "(^.{{{0},{1}}})";

	private const string RegexDigit = "(?=.*\\d)";

	private const string RegexNewUniqueCharacterBegin = ".*?(";

	private const string RegexNewUniqueCharacterNotEqualsGroup = "(?!\\{0})";

	private const string RegexNewUniqueCharacterEnd = ".)";

	private const string RegexUniqueCharactersStart = "(?=(.)";

	private const string RegexUniqueCharactersEnd = ")";

	private const string RegexStart = "^";

	private const string RegexMoreThanOneCharecters = ".+";

	private const string RegexEnd = "$";

	public PasswordSettings Settings { get; }

	public string Pattern => _pattern;

	public PasswordRequirements(IOptions<IdentityServerConfiguration> configuration)
	{
		Settings = configuration.Value.PasswordSettings;
		InitPattern();
	}

	public void InitPattern()
	{
		StringBuilder stringBuilder = new StringBuilder().Append("^");
		if (Settings.RequireLowercase)
		{
			stringBuilder.Append("(?=.*\\p{Ll})");
		}
		if (Settings.RequireUppercase)
		{
			stringBuilder.Append("(?=.*\\p{Lu})");
		}
		if (Settings.RequireNonAlphanumeric)
		{
			stringBuilder.Append("(?=.*[~!@#$%^&*_+=`|\\(){}\\[\\]:;\"'<>,.?\\/-])");
		}
		if (Settings.RequireDigit)
		{
			stringBuilder.Append("(?=.*\\d)");
		}
		if (Settings.RequiredUniqueChars > 1)
		{
			stringBuilder.Append("(?=(.)");
			for (int i = 1; i < Settings.RequiredUniqueChars; i++)
			{
				stringBuilder.Append(".*?(");
				for (int j = 1; j <= i; j++)
				{
					stringBuilder.AppendFormat("(?!\\{0})", j);
				}
				stringBuilder.Append(".)");
			}
			stringBuilder.Append(")");
		}
		if (Settings.RequiredLength > 0 || Settings.MaxLength > 0)
		{
			stringBuilder.AppendFormat("(^.{{{0},{1}}})", Settings.RequiredLength, Settings.MaxLength);
		}
		else
		{
			stringBuilder.Append(".+");
		}
		stringBuilder.Append("$");
		_pattern = stringBuilder.ToString();
	}
}
