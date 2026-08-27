using System;
using System.Collections;
using System.Text;
using NSpring.Logging.Common;

namespace NSpring.Logging.DataFormatters;

public sealed class FlatDataFormatter : DataFormatter
{
	private static string[] yearStrings = Constants.YearStrings;

	private static string[] dateStrings = Constants.DateStrings;

	private static string[] millisecondStrings = Constants.MillisecondStrings;

	private const string TIMESTAMP_FORMAT = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz";

	private const string QUOTE = "\"";

	private const string APOSTROPHE = "'";

	private const string BLANK_STRING = "";

	private const string DT = "DT";

	private const string ELLIPSIS = "...";

	private const string ELEMENT_SEPARATOR = ", ";

	private const string PAD = "  ";

	private const string NULL_VALUE = "NULL";

	private const string BACKSLASH = "\\";

	private const string BACKSLASH_ESCAPE = "\\\\";

	private const string APOSTROPHE_ESCAPE = "\\'";

	private const string QUOTE_ESCAPE = "\\\"";

	private static string NEW_LINE_VALUE = '\n'.ToString();

	private const string NEW_LINE_ESCAPE = "\\n";

	private const char SPACE_CHAR = ' ';

	private const char QUOTE_CHAR = '"';

	private const char APOSTROPHE_CHAR = '\'';

	private const char NEW_LINE_CHAR = '\n';

	private const char COLON_CHAR = ':';

	private const char HYPHEN_CHAR = '-';

	private const char PERIOD_CHAR = '.';

	private const char T_CHAR = 'T';

	private const char P_CHAR = 'P';

	private const char H_CHAR = 'H';

	private const char M_CHAR = 'M';

	private const char S_CHAR = 'S';

	private const char BACKSLASH_CHAR = '\\';

	private const char OPEN_BRACKET_CHAR = '[';

	private const char CLOSE_BRACKET_CHAR = ']';

	private const char OPEN_PARENTHESIS_CHAR = '(';

	private const char CLOSE_PARENTHESIS_CHAR = ')';

	private string prefix;

	private string suffix;

	public string Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			prefix = value;
		}
	}

	public string Suffix
	{
		get
		{
			return suffix;
		}
		set
		{
			suffix = value;
		}
	}

	public FlatDataFormatter()
	{
		dateTimeFormat = "yyyy'-'MM'-'dd'T'HH':'mm':'ss'.'fffzzz";
	}

	public override DataFormatter Copy()
	{
		FlatDataFormatter flatDataFormatter = new FlatDataFormatter();
		TypeFormatter[] array = base.TypeFormatters;
		for (int i = 0; i < array.Length; i++)
		{
			flatDataFormatter.AddTypeFormatter(array[i].Copy());
		}
		flatDataFormatter.dateTimeFormat = dateTimeFormat;
		flatDataFormatter.decimalFormat = decimalFormat;
		flatDataFormatter.doubleFormat = doubleFormat;
		flatDataFormatter.falseValue = falseValue;
		flatDataFormatter.floatFormat = floatFormat;
		flatDataFormatter.integerFormat = integerFormat;
		flatDataFormatter.isKeySortingEnabled = isKeySortingEnabled;
		flatDataFormatter.isListSortingEnabled = isListSortingEnabled;
		flatDataFormatter.longFormat = longFormat;
		flatDataFormatter.trueValue = trueValue;
		flatDataFormatter.prefix = prefix;
		flatDataFormatter.suffix = suffix;
		return flatDataFormatter;
	}

	public override string Format(object data)
	{
		if (data == null)
		{
			return "";
		}
		StringBuilder stringBuilder = new StringBuilder(128);
		Format(data, stringBuilder);
		return stringBuilder.ToString();
	}

	public override void Format(object data, StringBuilder sb)
	{
		if (data == null)
		{
			sb.Append("NULL");
			return;
		}
		Type type = data.GetType();
		if (type == DataFormatter.STRING)
		{
			sb.Append('"');
			sb.Append(EscapeString((string)data));
			sb.Append('"');
			return;
		}
		if (type.IsPrimitive)
		{
			if (type == DataFormatter.BOOLEAN)
			{
				sb.Append(((bool)data) ? trueValue : falseValue);
			}
			else if (type == DataFormatter.INT32)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((int)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.INT64)
			{
				string text = longFormat;
				if (text != null)
				{
					sb.Append(((long)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.SINGLE)
			{
				string text = floatFormat;
				if (text != null)
				{
					sb.Append(((float)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.DOUBLE)
			{
				string text = doubleFormat;
				if (text != null)
				{
					sb.Append(((double)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.INT16)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((short)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT16)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((ushort)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT32)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((uint)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT64)
			{
				string text = longFormat;
				if (text != null)
				{
					sb.Append(((ulong)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.BYTE)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((byte)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.SBYTE)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((sbyte)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.CHAR)
			{
				sb.Append('"');
				sb.Append(EscapeString(data.ToString()));
				sb.Append('"');
			}
			return;
		}
		if (type == DataFormatter.DATE_TIME)
		{
			sb.Append('\'');
			string text2 = dateTimeFormat;
			if (text2 != null)
			{
				sb.Append(EscapeDateTime(((DateTime)data).ToString(text2)));
			}
			else
			{
				DateTime dateTime = (DateTime)data;
				int year = dateTime.Year;
				if (year < yearStrings.Length)
				{
					sb.Append(yearStrings[year]).Append('-');
				}
				else
				{
					sb.Append(year).Append('-');
				}
				sb.Append(yearStrings[dateTime.Year]).Append('-');
				sb.Append(dateStrings[dateTime.Month]).Append('-');
				sb.Append(dateStrings[dateTime.Day]).Append(' ');
				sb.Append(dateStrings[dateTime.Hour]).Append(':');
				sb.Append(dateStrings[dateTime.Minute]).Append(':');
				sb.Append(dateStrings[dateTime.Second]).Append('.');
				sb.Append(millisecondStrings[dateTime.Millisecond]);
				sb.Append(DataFormatter.GetLocalTimeZone(dateTime.Month, dateTime.Day));
			}
			sb.Append('\'');
			return;
		}
		if (type == DataFormatter.DECIMAL)
		{
			string text3 = decimalFormat;
			if (text3 != null)
			{
				sb.Append(((decimal)data).ToString(text3));
			}
			else
			{
				sb.Append(data.ToString());
			}
			return;
		}
		if (type == DataFormatter.TIMESPAN)
		{
			sb.Append('\'');
			TimeSpan timeSpan = (TimeSpan)data;
			sb.Append('P');
			sb.Append(timeSpan.Days).Append("DT");
			sb.Append(timeSpan.Hours).Append('H');
			sb.Append(timeSpan.Minutes).Append('M');
			sb.Append(timeSpan.Seconds);
			if (timeSpan.Milliseconds > 0)
			{
				sb.Append('.').Append(millisecondStrings[timeSpan.Milliseconds]);
			}
			sb.Append('S');
			sb.Append('\'');
			return;
		}
		if (type.IsArray)
		{
			sb.Append('[');
			Array array = (Array)data;
			if (isListSortingEnabled)
			{
				object[] array2 = DataFormatter.Sort(array);
				if (array2.Length != 0)
				{
					object obj = array2[0];
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, 2);
					}
				}
				for (int i = 1; i < array2.Length; i++)
				{
					sb.Append(", ");
					object obj = array2[i];
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, 2);
					}
				}
			}
			else
			{
				if (array.Length > 0)
				{
					object obj = array.GetValue(0);
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, 2);
					}
				}
				for (int j = 1; j < array.Length; j++)
				{
					sb.Append(", ");
					object obj = array.GetValue(j);
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, 2);
					}
				}
			}
			sb.Append(']');
			return;
		}
		if (data is IDictionary)
		{
			sb.Append('[');
			IDictionary dictionary = (IDictionary)data;
			if (isKeySortingEnabled)
			{
				object[] array3 = SortKeys(dictionary);
				if (array3.Length != 0)
				{
					sb.Append('(');
					object obj2 = array3[0];
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, 2);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, 2);
					}
					sb.Append(')');
				}
				for (int k = 1; k < array3.Length; k++)
				{
					sb.Append(", ");
					sb.Append('(');
					object obj2 = array3[k];
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, 2);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, 2);
					}
					sb.Append(')');
				}
				return;
			}
			IEnumerator enumerator = dictionary.Keys.GetEnumerator();
			if (enumerator.MoveNext())
			{
				sb.Append('(');
				object obj2 = enumerator.Current;
				object obj3 = dictionary[obj2];
				if (obj2 == null)
				{
					sb.Append("NULL");
				}
				else
				{
					FormatElement(obj2, sb, 2);
				}
				sb.Append(", ");
				if (obj3 == null)
				{
					sb.Append("NULL");
				}
				else
				{
					FormatElement(obj3, sb, 2);
				}
				sb.Append(')');
			}
			while (enumerator.MoveNext())
			{
				sb.Append(", ");
				sb.Append('(');
				object obj2 = enumerator.Current;
				object obj3 = dictionary[obj2];
				if (obj2 == null)
				{
					sb.Append("NULL");
				}
				else
				{
					FormatElement(obj2, sb, 2);
				}
				sb.Append(", ");
				if (obj3 == null)
				{
					sb.Append("NULL");
				}
				else
				{
					FormatElement(obj3, sb, 2);
				}
				sb.Append(')');
			}
			sb.Append(']');
			return;
		}
		if (data is IEnumerable)
		{
			sb.Append('[');
			if (isListSortingEnabled)
			{
				object[] array4 = DataFormatter.Sort((IEnumerable)data);
				if (array4.Length != 0)
				{
					object obj4 = array4[0];
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, 2);
					}
				}
				for (int l = 1; l < array4.Length; l++)
				{
					object obj4 = array4[l];
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, 2);
					}
				}
			}
			else
			{
				IEnumerator enumerator2 = ((IEnumerable)data).GetEnumerator();
				if (enumerator2.MoveNext())
				{
					object obj4 = enumerator2.Current;
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, 2);
					}
				}
				while (enumerator2.MoveNext())
				{
					sb.Append(", ");
					object obj4 = enumerator2.Current;
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, 2);
					}
				}
			}
			sb.Append(']');
			return;
		}
		TypeFormatter typeFormatter = GetTypeFormatter(type);
		if (typeFormatter != null)
		{
			if (typeFormatter.IsPassThroughRequested)
			{
				sb.Append(typeFormatter.Format(data));
				return;
			}
			sb.Append('"');
			sb.Append(EscapeString(typeFormatter.Format(data)));
			sb.Append('"');
		}
		else
		{
			sb.Append('"');
			sb.Append(EscapeString(data.ToString()));
			sb.Append('"');
		}
	}

	private void FormatElement(object data, StringBuilder sb, int depth)
	{
		if (depth > base.MaximumDepth)
		{
			sb.Append("...");
			return;
		}
		if (data == null)
		{
			sb.Append("NULL");
			return;
		}
		Type type = data.GetType();
		if (type == DataFormatter.STRING)
		{
			sb.Append('"');
			sb.Append(EscapeString((string)data));
			sb.Append('"');
			return;
		}
		if (type.IsPrimitive)
		{
			if (type == DataFormatter.BOOLEAN)
			{
				sb.Append(((bool)data) ? trueValue : falseValue);
			}
			else if (type == DataFormatter.INT32)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((int)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.INT64)
			{
				string text = longFormat;
				if (text != null)
				{
					sb.Append(((long)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.SINGLE)
			{
				string text = floatFormat;
				if (text != null)
				{
					sb.Append(((float)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.DOUBLE)
			{
				string text = doubleFormat;
				if (text != null)
				{
					sb.Append(((double)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.INT16)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((short)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT16)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((ushort)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT32)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((uint)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.UINT64)
			{
				string text = longFormat;
				if (text != null)
				{
					sb.Append(((ulong)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.BYTE)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((byte)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.SBYTE)
			{
				string text = integerFormat;
				if (text != null)
				{
					sb.Append(((sbyte)data).ToString(text));
				}
				else
				{
					sb.Append(data.ToString());
				}
			}
			else if (type == DataFormatter.CHAR)
			{
				sb.Append('"');
				sb.Append(EscapeString(data.ToString()));
				sb.Append('"');
			}
			return;
		}
		if (type == DataFormatter.DATE_TIME)
		{
			sb.Append('\'');
			string text2 = dateTimeFormat;
			if (text2 != null)
			{
				sb.Append(EscapeDateTime(((DateTime)data).ToString(text2)));
			}
			else
			{
				DateTime dateTime = (DateTime)data;
				int year = dateTime.Year;
				if (year < yearStrings.Length)
				{
					sb.Append(yearStrings[year]).Append('-');
				}
				else
				{
					sb.Append(year).Append('-');
				}
				sb.Append(dateStrings[dateTime.Month]).Append('-');
				sb.Append(dateStrings[dateTime.Day]).Append(' ');
				sb.Append(dateStrings[dateTime.Hour]).Append(':');
				sb.Append(dateStrings[dateTime.Minute]).Append(':');
				sb.Append(dateStrings[dateTime.Second]).Append('.');
				sb.Append(millisecondStrings[dateTime.Millisecond]);
				sb.Append(DataFormatter.GetLocalTimeZone(dateTime.Month, dateTime.Day));
			}
			sb.Append('\'');
			return;
		}
		if (type == DataFormatter.DECIMAL)
		{
			string text3 = decimalFormat;
			if (text3 != null)
			{
				sb.Append(((decimal)data).ToString(text3));
			}
			else
			{
				sb.Append(data.ToString());
			}
			return;
		}
		if (type == DataFormatter.TIMESPAN)
		{
			sb.Append('\'');
			TimeSpan timeSpan = (TimeSpan)data;
			sb.Append('P');
			sb.Append(timeSpan.Days).Append("DT");
			sb.Append(timeSpan.Hours).Append('H');
			sb.Append(timeSpan.Minutes).Append('M');
			sb.Append(timeSpan.Seconds);
			if (timeSpan.Milliseconds > 0)
			{
				sb.Append('.').Append(millisecondStrings[timeSpan.Milliseconds]);
			}
			sb.Append('S');
			sb.Append('\'');
			return;
		}
		if (type.IsArray)
		{
			sb.Append('[');
			Array array = (Array)data;
			int depth2 = depth + 1;
			if (isListSortingEnabled)
			{
				object[] array2 = DataFormatter.Sort(array);
				if (array2.Length != 0)
				{
					object obj = array2[0];
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, depth2);
					}
				}
				for (int i = 1; i < array2.Length; i++)
				{
					sb.Append(", ");
					object obj = array2[i];
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, depth2);
					}
				}
			}
			else
			{
				if (array.Length > 0)
				{
					object obj = array.GetValue(0);
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, depth2);
					}
				}
				for (int j = 1; j < array.Length; j++)
				{
					sb.Append(", ");
					object obj = array.GetValue(j);
					if (obj == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj, sb, depth2);
					}
				}
			}
			sb.Append(']');
			return;
		}
		if (data is IDictionary)
		{
			sb.Append('[');
			IDictionary dictionary = (IDictionary)data;
			int depth3 = depth + 1;
			if (isKeySortingEnabled)
			{
				object[] array3 = SortKeys(dictionary);
				if (array3.Length != 0)
				{
					sb.Append('(');
					object obj2 = array3[0];
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, depth3);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, depth3);
					}
					sb.Append(')');
				}
				for (int k = 1; k < array3.Length; k++)
				{
					sb.Append(", ");
					sb.Append('(');
					object obj2 = array3[k];
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, depth3);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, depth3);
					}
					sb.Append(')');
				}
			}
			else
			{
				IEnumerator enumerator = dictionary.Keys.GetEnumerator();
				if (enumerator.MoveNext())
				{
					sb.Append('(');
					object obj2 = enumerator.Current;
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, depth3);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, depth3);
					}
					sb.Append(')');
				}
				while (enumerator.MoveNext())
				{
					sb.Append(", ");
					sb.Append('(');
					object obj2 = enumerator.Current;
					object obj3 = dictionary[obj2];
					if (obj2 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj2, sb, depth3);
					}
					sb.Append(", ");
					if (obj3 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj3, sb, depth3);
					}
					sb.Append(')');
				}
			}
			sb.Append(']');
			return;
		}
		if (data is IEnumerable)
		{
			sb.Append('[');
			int depth4 = depth + 1;
			if (isListSortingEnabled)
			{
				object[] array4 = DataFormatter.Sort((IEnumerable)data);
				if (array4.Length != 0)
				{
					object obj4 = array4[0];
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, depth4);
					}
				}
				for (int l = 1; l < array4.Length; l++)
				{
					sb.Append(", ");
					object obj4 = array4[l];
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, depth4);
					}
				}
			}
			else
			{
				IEnumerator enumerator2 = ((IEnumerable)data).GetEnumerator();
				if (enumerator2.MoveNext())
				{
					object obj4 = enumerator2.Current;
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, depth4);
					}
				}
				while (enumerator2.MoveNext())
				{
					sb.Append(", ");
					object obj4 = enumerator2.Current;
					if (obj4 == null)
					{
						sb.Append("NULL");
					}
					else
					{
						FormatElement(obj4, sb, depth4);
					}
				}
			}
			sb.Append(']');
			return;
		}
		TypeFormatter typeFormatter = GetTypeFormatter(type);
		if (typeFormatter != null)
		{
			if (typeFormatter.IsPassThroughRequested)
			{
				sb.Append(typeFormatter.Format(data));
				return;
			}
			sb.Append('"');
			sb.Append(EscapeString(typeFormatter.Format(data)));
			sb.Append('"');
		}
		else
		{
			sb.Append('"');
			sb.Append(EscapeString(data.ToString()));
			sb.Append('"');
		}
	}

	private static string EscapeString(string s)
	{
		if (s.IndexOf('\\') >= 0)
		{
			s = s.Replace("\\", "\\\\");
		}
		if (s.IndexOf('"') >= 0)
		{
			s = s.Replace("\"", "\\\"");
		}
		if (s.IndexOf('\n') >= 0)
		{
			s = s.Replace(NEW_LINE_VALUE, "\\n");
		}
		return s;
	}

	private static string EscapeDateTime(string s)
	{
		if (s.IndexOf('\\') >= 0)
		{
			s = s.Replace("\\", "\\\\");
		}
		if (s.IndexOf('\'') >= 0)
		{
			s = s.Replace("'", "\\'");
		}
		if (s.IndexOf('\n') >= 0)
		{
			s = s.Replace(NEW_LINE_VALUE, "\\n");
		}
		return s;
	}
}
