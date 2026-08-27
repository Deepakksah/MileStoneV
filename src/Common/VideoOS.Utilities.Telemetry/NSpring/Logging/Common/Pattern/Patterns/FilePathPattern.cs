using System.Text;

namespace NSpring.Logging.Common.Pattern.Patterns;

public sealed class FilePathPattern : Pattern
{
	private const string INVALID_FILE_PATH_CHARS = ":*?\"<>|";

	private const char UNDERSCORE_CHAR = '_';

	private const char COLON_CHAR = ':';

	private const char ASTERISK_CHAR = '*';

	private const char QUESTION_MARK_CHAR = '?';

	private const char QUOTE_CHAR = '"';

	private const char LESS_THAN_CHAR = '<';

	private const char GREATER_THAN_CHAR = '>';

	private const char PIPE_CHAR = '|';

	private const char BACKSLASH_CHAR = '\\';

	private const char FORWARD_SLASH_CHAR = '/';

	private const string BLANK_STRING = "";

	private bool isDirectoryPathStatic;

	private bool isDirectoryPathVolatile;

	private bool isFilePathStatic;

	private bool isFilePathVolatile;

	public bool IsDirectoryPathStatic => isDirectoryPathStatic;

	public bool IsDirectoryPathVolatile => isDirectoryPathVolatile;

	public bool IsFilePathStatic => isFilePathStatic;

	public bool IsFilePathVolatile => isFilePathVolatile;

	public FilePathPattern(string source)
		: base(source, _isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: false, _isMessageEnabled: false, _isDataEnabled: false)
	{
		bool flag = false;
		isDirectoryPathStatic = true;
		isDirectoryPathVolatile = false;
		isFilePathStatic = true;
		isFilePathVolatile = false;
		for (int num = tokens.Length - 1; num >= 0; num--)
		{
			Token token = tokens[num];
			switch (token.Type)
			{
			case TokenType.Text:
				if (token.Text.IndexOf('\\') >= 0 || token.Text.IndexOf('/') > 0)
				{
					flag = true;
				}
				if (num > 0)
				{
					token.Text = Escape(token.Text);
				}
				break;
			case TokenType.Separator:
				if (num > 0)
				{
					token.Text = Escape(token.Text);
				}
				flag = true;
				break;
			case TokenType.ContextProperty:
				if (flag)
				{
					isDirectoryPathStatic = false;
				}
				isFilePathStatic = false;
				break;
			case TokenType.EventProperty:
			{
				if (flag)
				{
					isDirectoryPathStatic = false;
				}
				isFilePathStatic = false;
				EventPropertyType propertyType = ((EventPropertyToken)token).PropertyType;
				if (propertyType == EventPropertyType.ID || propertyType == EventPropertyType.Millisecond || (uint)(propertyType - 14) <= 1u)
				{
					if (flag)
					{
						isDirectoryPathVolatile = true;
					}
					isFilePathVolatile = true;
				}
				break;
			}
			default:
				if (flag)
				{
					isDirectoryPathStatic = false;
				}
				break;
			}
		}
	}

	public override string Format(Event e)
	{
		StringBuilder stringBuilder = new StringBuilder(80);
		bool flag = false;
		string value = null;
		for (int i = 0; i < tokens.Length; i++)
		{
			Token token = tokens[i];
			switch (token.Type)
			{
			case TokenType.Text:
				stringBuilder.Append(token.Text);
				flag = false;
				break;
			case TokenType.Separator:
				if (!flag)
				{
					stringBuilder.Append(token.Text);
					value = token.Text;
					flag = true;
				}
				else if (!token.Text.Equals(value))
				{
					stringBuilder.Append(token.Text);
					value = token.Text;
				}
				break;
			case TokenType.ContextProperty:
			case TokenType.EventProperty:
				stringBuilder.Append(EscapeToken(token.Format(e)));
				flag = false;
				break;
			}
		}
		return stringBuilder.ToString();
	}

	public override void Format(Event e, StringBuilder sb)
	{
		bool flag = false;
		string value = null;
		for (int i = 0; i < tokens.Length; i++)
		{
			Token token = tokens[i];
			switch (token.Type)
			{
			case TokenType.Text:
				sb.Append(token.Text);
				break;
			case TokenType.Separator:
				if (!flag)
				{
					sb.Append(token.Text);
					value = token.Text;
					flag = true;
				}
				else if (!token.Text.Equals(value))
				{
					sb.Append(token.Text);
					value = token.Text;
				}
				break;
			case TokenType.ContextProperty:
			case TokenType.EventProperty:
				sb.Append(EscapeToken(token.Format(e)));
				break;
			}
		}
	}

	private static string Escape(string s)
	{
		for (int i = 0; i < ":*?\"<>|".Length; i++)
		{
			if (s.IndexOf(":*?\"<>|"[i]) >= 0)
			{
				s = s.Replace(":*?\"<>|"[i], '_');
			}
		}
		return s;
	}

	private static string EscapeToken(string s)
	{
		if (s.IndexOf(':') >= 0)
		{
			s = s.Replace(':', '_');
		}
		if (s.IndexOf('*') >= 0)
		{
			s = s.Replace('*', '_');
		}
		if (s.IndexOf('?') >= 0)
		{
			s = s.Replace('?', '_');
		}
		if (s.IndexOf('"') >= 0)
		{
			s = s.Replace('"', '_');
		}
		if (s.IndexOf('<') >= 0)
		{
			s = s.Replace('<', '_');
		}
		if (s.IndexOf('|') >= 0)
		{
			s = s.Replace('|', '_');
		}
		if (s.IndexOf('\\') >= 0)
		{
			s = s.Replace('\\', '_');
		}
		if (s.IndexOf('/') >= 0)
		{
			s = s.Replace('/', '_');
		}
		return s;
	}
}
