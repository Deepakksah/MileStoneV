using System;
using System.Collections;

namespace NSpring.Logging.Common.Pattern;

public sealed class PatternParser
{
	private class FormattingHints
	{
		public readonly int MinimumWidth;

		public readonly int MaximumWidth;

		public readonly Alignment Alignment;

		public readonly Case Case;

		private FormattingHints()
		{
		}

		public FormattingHints(int minimumWidth, int maximumWidth, Alignment alignment, Case _case)
		{
			MinimumWidth = Math.Max(minimumWidth, 0);
			MaximumWidth = Math.Max(MinimumWidth, maximumWidth);
			Alignment = alignment;
			Case = _case;
		}
	}

	private abstract class FormattingHintInfo
	{
		public const int ALIGNMENT = 0;

		public const int CASE = 1;

		public const int MAXIMUM_WIDTH = 2;

		public const int MINIMUM_WIDTH = 3;

		public const int WIDTH = 4;

		public int FormattingHintType;

		public string Name;

		public string Suffix;
	}

	private sealed class AlignmentHintInfo : FormattingHintInfo
	{
		private Hashtable alignmentMap = new Hashtable(50);

		private AlignmentHintInfo()
		{
		}

		public AlignmentHintInfo(string name, string suffix)
		{
			FormattingHintType = 0;
			Name = name;
			Suffix = suffix;
		}

		public void AddAlignment(string key, Alignment a)
		{
			alignmentMap.Add(key, a);
		}

		public bool HasMapping(string s)
		{
			return alignmentMap.ContainsKey(s);
		}

		public Alignment GetAlignment(string s)
		{
			return (Alignment)alignmentMap[s];
		}
	}

	private sealed class CaseHintInfo : FormattingHintInfo
	{
		private Hashtable caseMap = new Hashtable(50);

		private CaseHintInfo()
		{
		}

		public CaseHintInfo(string name, string suffix)
		{
			FormattingHintType = 1;
			Name = name;
			Suffix = suffix;
		}

		public void AddCase(string key, Case c)
		{
			caseMap.Add(key, c);
		}

		public bool HasMapping(string s)
		{
			return caseMap.ContainsKey(s);
		}

		public Case GetCase(string s)
		{
			return (Case)caseMap[s];
		}
	}

	private sealed class MaximumWidthHintInfo : FormattingHintInfo
	{
		private MaximumWidthHintInfo()
		{
		}

		public MaximumWidthHintInfo(string name, string suffix)
		{
			FormattingHintType = 2;
			Name = name;
			Suffix = suffix;
		}
	}

	private sealed class MinimumWidthHintInfo : FormattingHintInfo
	{
		private MinimumWidthHintInfo()
		{
		}

		public MinimumWidthHintInfo(string name, string suffix)
		{
			FormattingHintType = 3;
			Name = name;
			Suffix = suffix;
		}
	}

	private sealed class WidthHintInfo : FormattingHintInfo
	{
		private WidthHintInfo()
		{
		}

		public WidthHintInfo(string name, string suffix)
		{
			FormattingHintType = 4;
			Name = name;
			Suffix = suffix;
		}
	}

	private abstract class TokenInfo
	{
		public string Name;

		public string AlternateName;

		public TokenType TokenType;
	}

	private sealed class ContextPropertyTokenInfo : TokenInfo
	{
		public static readonly FormattingHints DefaultFormattingHints = new FormattingHints(0, int.MaxValue, Alignment.Left, Case.Default);

		public ContextPropertyType PropertyType;

		private ContextPropertyTokenInfo()
		{
		}

		public ContextPropertyTokenInfo(string name, string alternateName, ContextPropertyType propertyType)
		{
			TokenType = TokenType.ContextProperty;
			Name = name;
			AlternateName = alternateName;
			PropertyType = propertyType;
		}
	}

	private sealed class EventPropertyTokenInfo : TokenInfo
	{
		public static readonly FormattingHints DefaultFormattingHints = new FormattingHints(0, int.MaxValue, Alignment.Left, Case.Default);

		public EventPropertyType PropertyType;

		private EventPropertyTokenInfo()
		{
		}

		public EventPropertyTokenInfo(string name, string alternateName, EventPropertyType propertyType)
		{
			TokenType = TokenType.EventProperty;
			Name = name;
			AlternateName = alternateName;
			PropertyType = propertyType;
		}
	}

	private sealed class EventTokenInfo : TokenInfo
	{
		private EventTokenInfo()
		{
		}

		public EventTokenInfo(string name, string alternateName)
		{
			TokenType = TokenType.Event;
			Name = name;
			AlternateName = alternateName;
		}
	}

	private sealed class MessageTokenInfo : TokenInfo
	{
		public static readonly FormattingHints DefaultFormattingHints = new FormattingHints(0, int.MaxValue, Alignment.Left, Case.Default);

		private MessageTokenInfo()
		{
		}

		public MessageTokenInfo(string name, string alternateName)
		{
			TokenType = TokenType.Message;
			Name = name;
			AlternateName = alternateName;
		}
	}

	private sealed class DataTokenInfo : TokenInfo
	{
		private DataTokenInfo()
		{
		}

		public DataTokenInfo(string name, string alternateName)
		{
			TokenType = TokenType.Data;
			Name = name;
			AlternateName = alternateName;
		}
	}

	private static TextToken EMPTY_TEXT_TOKEN;

	private static char[] EMPTY_CHAR_ARRAY;

	private const string BLANK_STRING = "";

	private const char COLON_CHAR = ':';

	private const char OPEN_PARENTHESIS_CHAR = '(';

	private const char CLOSE_PARENTHESIS_CHAR = ')';

	private const char OPEN_CURLY_BRACE_CHAR = '{';

	private const char CLOSE_CURLY_BRACE_CHAR = '}';

	private const char EQUALS_SIGN_CHAR = '=';

	private static char[] FORMATTING_HINT_SEPARATORS;

	private static string NEW_LINE;

	private static string NEW_LINE_CHAR;

	private static TokenInfo[] tokenInfoArray;

	private static Hashtable tokenInfoMap;

	private static Hashtable formattingHintInfoMap;

	private bool isContextPropertyEnabled;

	private bool isEventPropertyEnabled;

	private bool isEventEnabled;

	private bool isDataEnabled;

	private bool isMessageEnabled;

	private char[] separators;

	public bool IsContextPropertyEnabled
	{
		get
		{
			return isContextPropertyEnabled;
		}
		set
		{
			isContextPropertyEnabled = value;
		}
	}

	public bool IsEventEnabled
	{
		get
		{
			return isEventEnabled;
		}
		set
		{
			isEventEnabled = value;
		}
	}

	public bool IsEventPropertyEnabled
	{
		get
		{
			return isEventPropertyEnabled;
		}
		set
		{
			isEventPropertyEnabled = value;
		}
	}

	public bool IsDataEnabled
	{
		get
		{
			return isDataEnabled;
		}
		set
		{
			isDataEnabled = value;
		}
	}

	static PatternParser()
	{
		EMPTY_TEXT_TOKEN = new TextToken("");
		EMPTY_CHAR_ARRAY = new char[0];
		FORMATTING_HINT_SEPARATORS = new char[2] { ';', ',' };
		NEW_LINE = new string(new char[2] { '\r', '\n' });
		NEW_LINE_CHAR = "\n";
		tokenInfoArray = new TokenInfo[35]
		{
			new ContextPropertyTokenInfo("applicationname", "an", ContextPropertyType.ApplicationName),
			new ContextPropertyTokenInfo("systemname", "sn", ContextPropertyType.SystemName),
			new ContextPropertyTokenInfo("username", "un", ContextPropertyType.UserName),
			new ContextPropertyTokenInfo("threadname", "tn", ContextPropertyType.ThreadName),
			new ContextPropertyTokenInfo("threadid", "ti", ContextPropertyType.ThreadId),
			new ContextPropertyTokenInfo("threadpriority", "tp", ContextPropertyType.ThreadPriority),
			new ContextPropertyTokenInfo("property", "p", ContextPropertyType.Property),
			new EventPropertyTokenInfo("levelid", "li", EventPropertyType.LevelID),
			new EventPropertyTokenInfo("levelname", "ln", EventPropertyType.LevelName),
			new EventPropertyTokenInfo("levelpriority", "lp", EventPropertyType.LevelPriority),
			new EventPropertyTokenInfo("id", null, EventPropertyType.ID),
			new EventPropertyTokenInfo("category", "cat", EventPropertyType.Category),
			new EventPropertyTokenInfo("code", "cod", EventPropertyType.Code),
			new EventPropertyTokenInfo("year", "yyyy", EventPropertyType.Year),
			new EventPropertyTokenInfo("month", "mm", EventPropertyType.Month),
			new EventPropertyTokenInfo("day", "dd", EventPropertyType.Day),
			new EventPropertyTokenInfo("hour", "h", EventPropertyType.Hour),
			new EventPropertyTokenInfo("minute", "m", EventPropertyType.Minute),
			new EventPropertyTokenInfo("second", "s", EventPropertyType.Second),
			new EventPropertyTokenInfo("millisecond", "ms", EventPropertyType.Millisecond),
			new EventPropertyTokenInfo("date", "dt", EventPropertyType.Date),
			new EventPropertyTokenInfo("time", "tm", EventPropertyType.Time),
			new EventPropertyTokenInfo("timestamp", "ts", EventPropertyType.Timestamp),
			new EventPropertyTokenInfo("zone", "z", EventPropertyType.TimeZone),
			new EventPropertyTokenInfo("monthname", "mn", EventPropertyType.MonthName),
			new EventPropertyTokenInfo("monthshortname", "msn", EventPropertyType.MonthShortName),
			new EventPropertyTokenInfo("weekday", "wd", EventPropertyType.Weekday),
			new EventPropertyTokenInfo("weekdayname", "wdn", EventPropertyType.WeekdayName),
			new EventPropertyTokenInfo("weekdayshortname", "wdsn", EventPropertyType.WeekdayShortName),
			new EventPropertyTokenInfo("dayofyear", "doy", EventPropertyType.DayOfYear),
			new EventPropertyTokenInfo("weekofmonth", "wom", EventPropertyType.WeekOfMonth),
			new EventPropertyTokenInfo("weekofyear", "woy", EventPropertyType.WeekOfYear),
			new EventTokenInfo("event", null),
			new MessageTokenInfo("message", "msg"),
			new DataTokenInfo("data", null)
		};
		tokenInfoMap = new Hashtable(111);
		formattingHintInfoMap = new Hashtable(111);
		for (int i = 0; i < tokenInfoArray.Length; i++)
		{
			TokenInfo tokenInfo = tokenInfoArray[i];
			tokenInfoMap.Add(tokenInfo.Name, tokenInfo);
			if (tokenInfo.AlternateName != null)
			{
				tokenInfoMap.Add(tokenInfo.AlternateName, tokenInfo);
			}
		}
		FormattingHintInfo formattingHintInfo = new MaximumWidthHintInfo("maxwidth", "l");
		formattingHintInfoMap.Add(formattingHintInfo.Name, formattingHintInfo);
		formattingHintInfoMap.Add(formattingHintInfo.Suffix, formattingHintInfo);
		formattingHintInfo = new MinimumWidthHintInfo("minwidth", "s");
		formattingHintInfoMap.Add(formattingHintInfo.Name, formattingHintInfo);
		formattingHintInfoMap.Add(formattingHintInfo.Suffix, formattingHintInfo);
		formattingHintInfo = new WidthHintInfo("width", "w");
		formattingHintInfoMap.Add(formattingHintInfo.Name, formattingHintInfo);
		formattingHintInfoMap.Add(formattingHintInfo.Suffix, formattingHintInfo);
		AlignmentHintInfo alignmentHintInfo = new AlignmentHintInfo("align", "a");
		alignmentHintInfo.AddAlignment("c", Alignment.Center);
		alignmentHintInfo.AddAlignment("center", Alignment.Center);
		alignmentHintInfo.AddAlignment("l", Alignment.Left);
		alignmentHintInfo.AddAlignment("left", Alignment.Left);
		alignmentHintInfo.AddAlignment("r", Alignment.Right);
		alignmentHintInfo.AddAlignment("right", Alignment.Right);
		formattingHintInfoMap.Add(alignmentHintInfo.Name, alignmentHintInfo);
		formattingHintInfoMap.Add(alignmentHintInfo.Suffix, alignmentHintInfo);
		CaseHintInfo caseHintInfo = new CaseHintInfo("case", "c");
		caseHintInfo.AddCase("l", Case.Lower);
		caseHintInfo.AddCase("lower", Case.Lower);
		caseHintInfo.AddCase("u", Case.Upper);
		caseHintInfo.AddCase("upper", Case.Upper);
		formattingHintInfoMap.Add(caseHintInfo.Name, caseHintInfo);
		formattingHintInfoMap.Add(caseHintInfo.Suffix, caseHintInfo);
	}

	public PatternParser()
		: this(_isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: true, _isMessageEnabled: true, _isDataEnabled: true, null)
	{
	}

	public PatternParser(bool _isContextPropertyEnabled, bool _isEventPropertyEnabled, bool _isEventEnabled, bool _isMessageEnabled, bool _isDataEnabled)
		: this(_isContextPropertyEnabled, _isEventPropertyEnabled, _isEventEnabled, _isMessageEnabled, _isDataEnabled, null)
	{
	}

	public PatternParser(bool _isContextPropertyEnabled, bool _isEventPropertyEnabled, bool _isEventEnabled, bool _isMessageEnabled, bool _isDataEnabled, char[] _separators)
	{
		isContextPropertyEnabled = _isContextPropertyEnabled;
		isEventPropertyEnabled = _isEventPropertyEnabled;
		isEventEnabled = _isEventEnabled;
		isMessageEnabled = _isMessageEnabled;
		isDataEnabled = _isDataEnabled;
		if (_separators == null || _separators.Length == 0)
		{
			separators = EMPTY_CHAR_ARRAY;
			return;
		}
		separators = new char[_separators.Length];
		Array.Copy(_separators, 0, separators, 0, separators.Length);
	}

	public Token[] Parse(string text)
	{
		if (text == null)
		{
			return new Token[1] { EMPTY_TEXT_TOKEN };
		}
		text = text.Replace(NEW_LINE_CHAR, NEW_LINE);
		int length = text.Length;
		if (length == 0)
		{
			return new Token[1] { EMPTY_TEXT_TOKEN };
		}
		ArrayList arrayList = new ArrayList(50);
		int num = 0;
		int num2 = 0;
		int num3 = text.IndexOf('{', num);
		while (num3 >= 0)
		{
			Token token = null;
			num2 = text.IndexOf('}', num3);
			if (num2 < 0)
			{
				break;
			}
			token = ParseToken(text.Substring(num3 + 1, num2 - num3 - 1));
			if (token != null)
			{
				if (num3 > num)
				{
					arrayList.Add(new TextToken(text.Substring(num, num3 - num)));
				}
				arrayList.Add(token);
				num = num2 + 1;
			}
			if (token != null)
			{
				num3 = text.IndexOf('{', num2);
			}
			else if (num3 < text.Length - 1)
			{
				num3 = text.IndexOf('{', num3 + 1);
			}
		}
		if (num < length)
		{
			arrayList.Add(new TextToken(text.Substring(num, text.Length - num)));
		}
		if (arrayList.Count == 0)
		{
			return new Token[1] { EMPTY_TEXT_TOKEN };
		}
		Token[] array = new Token[arrayList.Count];
		arrayList.CopyTo(array);
		return array;
	}

	public Token ParseToken(string text)
	{
		if (text == null || text.Length == 0)
		{
			return null;
		}
		string text2 = null;
		string text3 = null;
		string text4 = null;
		int num = text.LastIndexOf(':');
		int num2 = text.IndexOf('(');
		if (num2 < 0)
		{
			if (num < 0)
			{
				text2 = text;
			}
			else
			{
				if (num == 0)
				{
					return null;
				}
				text2 = text.Substring(0, num);
				if (num < text.Length - 1)
				{
					text4 = text.Substring(num + 1, text.Length - (num + 1));
				}
			}
		}
		else
		{
			if (num2 == 0)
			{
				return null;
			}
			if (num < 0)
			{
				text2 = text.Substring(0, num2);
				if (num2 < text.Length - 1)
				{
					int num3 = text.LastIndexOf(')');
					if (num3 > num2 + 1)
					{
						text3 = text.Substring(num2 + 1, num3 - num2 - 1);
					}
				}
			}
			else if (num2 < num)
			{
				text2 = text.Split(':')[0].Substring(0, num2);
				int num4 = text.LastIndexOf(')');
				if (num4 > num2 + 1)
				{
					text3 = text.Substring(num2 + 1, num4 - num2 - 1);
				}
				if (num < text.Length - 1)
				{
					text4 = text.Substring(num + 1, text.Length - (num + 1));
				}
			}
			else
			{
				if (num == 0)
				{
					return null;
				}
				text2 = text.Substring(0, num);
				if (num < text.Length - 1)
				{
					text4 = text.Substring(num + 1, text.Length - (num + 1));
				}
			}
		}
		text2 = text2.Trim().ToLower();
		if (tokenInfoMap.ContainsKey(text2))
		{
			TokenInfo tokenInfo = (TokenInfo)tokenInfoMap[text2];
			switch (tokenInfo.TokenType)
			{
			case TokenType.ContextProperty:
				if (isContextPropertyEnabled)
				{
					FormattingHints formattingHints = ContextPropertyTokenInfo.DefaultFormattingHints;
					if (text4 != null)
					{
						formattingHints = ParseFormattingHints(text4, formattingHints);
					}
					ContextPropertyTokenInfo contextPropertyTokenInfo = (ContextPropertyTokenInfo)tokenInfo;
					if (contextPropertyTokenInfo.PropertyType == ContextPropertyType.Property)
					{
						if (text3 == null)
						{
							return null;
						}
						return new ContextPropertyToken(contextPropertyTokenInfo.PropertyType, text3, "{" + text + "}", formattingHints.MinimumWidth, formattingHints.MaximumWidth, formattingHints.Alignment, formattingHints.Case);
					}
					return new ContextPropertyToken(contextPropertyTokenInfo.PropertyType, null, "{" + text + "}", formattingHints.MinimumWidth, formattingHints.MaximumWidth, formattingHints.Alignment, formattingHints.Case);
				}
				return null;
			case TokenType.EventProperty:
				if (isEventPropertyEnabled)
				{
					FormattingHints formattingHints2 = EventPropertyTokenInfo.DefaultFormattingHints;
					if (text4 != null)
					{
						formattingHints2 = ParseFormattingHints(text4, formattingHints2);
					}
					return new EventPropertyToken(((EventPropertyTokenInfo)tokenInfo).PropertyType, "{" + text + "}", formattingHints2.MinimumWidth, formattingHints2.MaximumWidth, formattingHints2.Alignment, formattingHints2.Case);
				}
				return null;
			case TokenType.Event:
				if (isEventEnabled)
				{
					return new EventToken("{" + text + "}");
				}
				return null;
			case TokenType.Message:
				if (isMessageEnabled)
				{
					FormattingHints formattingHints3 = MessageTokenInfo.DefaultFormattingHints;
					if (text4 != null)
					{
						formattingHints3 = ParseFormattingHints(text4, formattingHints3);
					}
					return new MessageToken("{" + text + "}", formattingHints3.MinimumWidth, formattingHints3.MaximumWidth, formattingHints3.Alignment, formattingHints3.Case);
				}
				return null;
			case TokenType.Data:
				if (isDataEnabled)
				{
					return new DataToken("{" + text + "}");
				}
				return null;
			default:
				return null;
			}
		}
		return null;
	}

	private static FormattingHints ParseFormattingHints(string text, FormattingHints defaults)
	{
		if (text == null)
		{
			return defaults;
		}
		Alignment alignment = defaults.Alignment;
		Case obj = defaults.Case;
		int maximumWidth = defaults.MaximumWidth;
		int minimumWidth = defaults.MinimumWidth;
		string[] array = text.Split(FORMATTING_HINT_SEPARATORS);
		for (int i = 0; i < array.Length; i++)
		{
			string text2 = array[i].Trim();
			text2 = text2.ToLower();
			string key;
			string s;
			if (text2.IndexOf('=') > 0)
			{
				string[] array2 = text2.Split('=');
				key = array2[0].Trim();
				s = array2[1].Trim();
			}
			else
			{
				if (text2.Length < 2)
				{
					continue;
				}
				key = text2.Substring(text2.Length - 1, 1).Trim();
				s = text2.Substring(0, text2.Length - 1).Trim();
			}
			FormattingHintInfo formattingHintInfo = (FormattingHintInfo)formattingHintInfoMap[key];
			if (formattingHintInfo == null)
			{
				continue;
			}
			switch (formattingHintInfo.FormattingHintType)
			{
			case 0:
			{
				AlignmentHintInfo alignmentHintInfo = (AlignmentHintInfo)formattingHintInfo;
				if (alignmentHintInfo.HasMapping(s))
				{
					alignment = alignmentHintInfo.GetAlignment(s);
				}
				break;
			}
			case 1:
			{
				CaseHintInfo caseHintInfo = (CaseHintInfo)formattingHintInfo;
				if (caseHintInfo.HasMapping(s))
				{
					obj = caseHintInfo.GetCase(s);
				}
				break;
			}
			case 2:
				try
				{
					maximumWidth = int.Parse(s);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				break;
			case 3:
				try
				{
					minimumWidth = int.Parse(s);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				break;
			case 4:
				try
				{
					minimumWidth = (maximumWidth = int.Parse(s));
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch
				{
				}
				break;
			}
		}
		return new FormattingHints(minimumWidth, maximumWidth, alignment, obj);
	}
}
