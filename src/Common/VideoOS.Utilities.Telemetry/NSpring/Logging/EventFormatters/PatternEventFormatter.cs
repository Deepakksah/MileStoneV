using System;
using System.Text;
using NSpring.Logging.Common;
using NSpring.Logging.Common.Pattern;
using NSpring.Logging.DataFormatters;

namespace NSpring.Logging.EventFormatters;

public sealed class PatternEventFormatter : EventFormatter
{
	private static string timeZone = Constants.ISO_8601_TIME_ZONE;

	private static string daylightSavingsTimeZone = Constants.ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE;

	private static string[] monthNames = Constants.MonthNames;

	private static string[] monthShortNames = Constants.MonthShortNames;

	private static string[] weekdayNames = Constants.WeekdayNames;

	private static string[] weekdayShortNames = Constants.WeekdayShortNames;

	private static string[] yearStrings = Constants.YearStrings;

	private static string[] dateStrings = Constants.DateStrings;

	private static string[] millisecondStrings = Constants.MillisecondStrings;

	private const char COLON_CHAR = ':';

	private const char HYPHEN_CHAR = '-';

	private const char PERIOD_CHAR = '.';

	private const char SPACE_CHAR = ' ';

	private const string BLANK_STRING = "";

	private const char SPACE = ' ';

	private const int DEFAULT_BUFFER_SIZE = 64;

	private const int SAMPLE_FREQUENCY = 5;

	private const int BUFFER_SIZE_RESET_FREQUENCY = 1500;

	private int bufferSize = 64;

	private int sampleCount;

	private int sampleTickCount;

	private Token[] tokens;

	private string patternSource;

	public string Pattern
	{
		get
		{
			return patternSource;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			lock (monitor)
			{
				Token[] array = new PatternParser(_isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: false, _isMessageEnabled: true, _isDataEnabled: true).Parse(value + suffix);
				tokens = array;
				patternSource = value;
			}
		}
	}

	private PatternEventFormatter()
	{
	}

	public PatternEventFormatter(string _pattern)
	{
		if (_pattern == null)
		{
			throw new ArgumentNullException();
		}
		dataFormatter = new FlatDataFormatter();
		patternSource = _pattern;
		tokens = new PatternParser(_isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: false, _isMessageEnabled: true, _isDataEnabled: true).Parse(patternSource + suffix);
	}

	protected override bool UpdateSuffix(string _newSuffix)
	{
		tokens = new PatternParser(_isContextPropertyEnabled: true, _isEventPropertyEnabled: true, _isEventEnabled: false, _isMessageEnabled: true, _isDataEnabled: true).Parse(patternSource + suffix);
		return true;
	}

	public override string Format(Event _event)
	{
		StringBuilder stringBuilder = new StringBuilder(bufferSize);
		for (int i = 0; i < tokens.Length; i++)
		{
			Token token = tokens[i];
			switch (token.Type)
			{
			case TokenType.Text:
			case TokenType.Separator:
				stringBuilder.Append(token.Text);
				break;
			case TokenType.ContextProperty:
			case TokenType.EventProperty:
				token.Format(_event, stringBuilder);
				break;
			case TokenType.Data:
				dataFormatter.Format(_event.Data, stringBuilder);
				break;
			default:
				stringBuilder.Append(token.Format(_event));
				break;
			}
		}
		stringBuilder.Append(suffix);
		sampleCount++;
		if (sampleCount == 5)
		{
			int length = stringBuilder.Length;
			if (length > bufferSize)
			{
				bufferSize = length;
			}
			sampleCount = 0;
			sampleTickCount++;
			if (sampleTickCount >= 1500)
			{
				bufferSize = 64;
				sampleTickCount = 0;
			}
		}
		return stringBuilder.ToString();
	}

	public override string GetEventListHeader()
	{
		return null;
	}

	public override string GetEventListFooter()
	{
		return null;
	}
}
