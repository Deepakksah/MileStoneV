using System;
using System.Collections;
using System.Text;
using NSpring.Logging.Common;

namespace NSpring.Logging;

public abstract class DataFormatter
{
	protected static readonly Type STRING = Type.GetType("System.String");

	protected static readonly Type BOOLEAN = Type.GetType("System.Boolean");

	protected static readonly Type BYTE = Type.GetType("System.Byte");

	protected static readonly Type SBYTE = Type.GetType("System.SByte");

	protected static readonly Type INT16 = Type.GetType("System.Int16");

	protected static readonly Type UINT16 = Type.GetType("System.UInt16");

	protected static readonly Type INT32 = Type.GetType("System.Int32");

	protected static readonly Type UINT32 = Type.GetType("System.UInt32");

	protected static readonly Type INT64 = Type.GetType("System.Int64");

	protected static readonly Type UINT64 = Type.GetType("System.UInt64");

	protected static readonly Type CHAR = Type.GetType("System.Char");

	protected static readonly Type DOUBLE = Type.GetType("System.Double");

	protected static readonly Type SINGLE = Type.GetType("System.Single");

	protected static readonly Type DECIMAL = Type.GetType("System.Decimal");

	protected static readonly Type DATE_TIME = Type.GetType("System.DateTime");

	protected static readonly Type TIMESPAN = Type.GetType("System.TimeSpan");

	protected static readonly Type XML_DOCUMENT = Type.GetType("System.Xml.XmlDocument");

	private const string DEFAULT_TRUE_VALUE = "true";

	private const string DEFAULT_FALSE_VALUE = "false";

	private int maximumDepth = 20;

	private Encoding encoding = Encoding.UTF8;

	private int typeFormatterCount;

	private TypeFormatter[] typeFormatters = new TypeFormatter[100];

	private Hashtable typeFormattersMap = new Hashtable(100);

	protected string trueValue = "true";

	protected string falseValue = "false";

	protected string dateTimeFormat = "";

	protected string decimalFormat;

	protected string doubleFormat;

	protected string floatFormat;

	protected string integerFormat;

	protected string longFormat;

	protected bool isKeySortingEnabled;

	protected bool isListSortingEnabled;

	protected object monitor = new object();

	private static string timeZone = Constants.ISO_8601_TIME_ZONE;

	private static string daylightSavingsTimeZone = Constants.ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE;

	public int MaximumDepth
	{
		get
		{
			return maximumDepth;
		}
		set
		{
			maximumDepth = Math.Max(0, value);
		}
	}

	public string TrueValue
	{
		get
		{
			return trueValue;
		}
		set
		{
			if (value == null)
			{
				trueValue = "true";
			}
			else
			{
				trueValue = value;
			}
		}
	}

	public string FalseValue
	{
		get
		{
			return falseValue;
		}
		set
		{
			if (value == null)
			{
				falseValue = "false";
			}
			else
			{
				falseValue = value;
			}
		}
	}

	public string DateTimeFormat
	{
		get
		{
			return dateTimeFormat;
		}
		set
		{
			if (value != null)
			{
				DateTime.UtcNow.ToString(value);
			}
			dateTimeFormat = value;
		}
	}

	public string DecimalFormat
	{
		get
		{
			lock (monitor)
			{
				return decimalFormat;
			}
		}
		set
		{
			if (value != null)
			{
				1000.0001m.ToString(value);
			}
			lock (monitor)
			{
				decimalFormat = value;
				if (decimalFormat != null)
				{
					if (floatFormat == null)
					{
						floatFormat = decimalFormat;
					}
					if (doubleFormat == null)
					{
						doubleFormat = decimalFormat;
					}
				}
			}
		}
	}

	public string DoubleFormat
	{
		get
		{
			lock (monitor)
			{
				return doubleFormat;
			}
		}
		set
		{
			if (value != null)
			{
				decimal.MaxValue.ToString(value);
				decimal.MinValue.ToString(value);
			}
			lock (monitor)
			{
				doubleFormat = value;
				if (doubleFormat != null)
				{
					if (floatFormat == null)
					{
						floatFormat = doubleFormat;
					}
					if (decimalFormat == null)
					{
						decimalFormat = doubleFormat;
					}
				}
			}
		}
	}

	public string FloatFormat
	{
		get
		{
			lock (monitor)
			{
				return floatFormat;
			}
		}
		set
		{
			if (value != null)
			{
				float.MaxValue.ToString(value);
				float.MinValue.ToString(value);
			}
			lock (monitor)
			{
				floatFormat = value;
				if (floatFormat != null)
				{
					if (doubleFormat == null)
					{
						doubleFormat = floatFormat;
					}
					if (decimalFormat == null)
					{
						decimalFormat = floatFormat;
					}
				}
			}
		}
	}

	public string IntegerFormat
	{
		get
		{
			lock (monitor)
			{
				return integerFormat;
			}
		}
		set
		{
			if (value != null)
			{
				int.MaxValue.ToString(value);
				int.MinValue.ToString(value);
				uint.MaxValue.ToString(value);
				0u.ToString(value);
			}
			lock (monitor)
			{
				integerFormat = value;
				if (integerFormat != null && longFormat == null)
				{
					longFormat = integerFormat;
				}
			}
		}
	}

	public string LongFormat
	{
		get
		{
			lock (monitor)
			{
				return longFormat;
			}
		}
		set
		{
			if (value != null)
			{
				long.MaxValue.ToString(value);
				long.MinValue.ToString(value);
				ulong.MaxValue.ToString(value);
				0uL.ToString(value);
			}
			lock (monitor)
			{
				longFormat = value;
				if (longFormat != null && integerFormat == null)
				{
					integerFormat = longFormat;
				}
			}
		}
	}

	public TypeFormatter[] TypeFormatters
	{
		get
		{
			lock (monitor)
			{
				TypeFormatter[] array = new TypeFormatter[typeFormatterCount];
				Array.Copy(typeFormatters, 0, array, 0, typeFormatterCount);
				return array;
			}
		}
	}

	public Encoding Encoding
	{
		get
		{
			return encoding;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			lock (monitor)
			{
				if (UpdateEncoding(value))
				{
					encoding = value;
					return;
				}
				throw new ArgumentException("Invalid encoding");
			}
		}
	}

	public bool IsKeySortingEnabled
	{
		get
		{
			return isKeySortingEnabled;
		}
		set
		{
			isKeySortingEnabled = value;
		}
	}

	public bool IsListSortingEnabled
	{
		get
		{
			return isListSortingEnabled;
		}
		set
		{
			isListSortingEnabled = value;
		}
	}

	public bool IsSortingEnabled
	{
		set
		{
			isKeySortingEnabled = value;
			isListSortingEnabled = value;
		}
	}

	public void AddTypeFormatter(TypeFormatter typeFormatter)
	{
		if (typeFormatter == null)
		{
			throw new ArgumentNullException();
		}
		Type supportedType = typeFormatter.SupportedType;
		if (supportedType == null)
		{
			throw new ArgumentException("No supported data type");
		}
		lock (monitor)
		{
			if (typeFormattersMap.ContainsKey(supportedType))
			{
				throw new InvalidOperationException("Type " + supportedType.ToString() + " already has a supporting TypeFormatter");
			}
			typeFormatter.DataFormatter = this;
			typeFormattersMap.Add(supportedType, typeFormatter);
			if (typeFormatterCount == typeFormatters.Length)
			{
				TypeFormatter[] destinationArray = new TypeFormatter[typeFormatterCount * 2];
				Array.Copy(typeFormatters, 0, destinationArray, 0, typeFormatterCount);
				typeFormatters = destinationArray;
			}
			typeFormatters[typeFormatterCount++] = typeFormatter;
		}
	}

	public TypeFormatter GetTypeFormatter(Type dataType)
	{
		if (typeFormatterCount <= 100)
		{
			for (int i = 0; i < typeFormatterCount; i++)
			{
				if (typeFormatters[i].SupportedType == dataType)
				{
					return typeFormatters[i];
				}
			}
			return null;
		}
		lock (monitor)
		{
			return (TypeFormatter)typeFormattersMap[dataType];
		}
	}

	protected virtual bool UpdateEncoding(Encoding _newEncoding)
	{
		return true;
	}

	protected static object[] Sort(Array array)
	{
		object[] array2 = new object[array.Length];
		array.CopyTo(array2, 0);
		try
		{
			Array.Sort(array2);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return array2;
	}

	protected static object[] Sort(IEnumerable enumerable)
	{
		ArrayList arrayList = new ArrayList(50);
		IEnumerator enumerator = enumerable.GetEnumerator();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Current);
		}
		object[] array = new object[arrayList.Count];
		arrayList.CopyTo(array);
		try
		{
			Array.Sort(array);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return array;
	}

	protected static object[] GetElements(IEnumerable enumerable)
	{
		ArrayList arrayList = new ArrayList(50);
		IEnumerator enumerator = enumerable.GetEnumerator();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Current);
		}
		object[] array = new object[arrayList.Count];
		arrayList.CopyTo(array);
		return array;
	}

	protected object[] SortKeys(IDictionary dictionary)
	{
		ArrayList arrayList = new ArrayList(dictionary.Count);
		IEnumerator enumerator = dictionary.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Current);
		}
		object[] array = new object[arrayList.Count];
		arrayList.CopyTo(array);
		try
		{
			Array.Sort(array);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch
		{
		}
		return array;
	}

	protected object[] GetKeys(IDictionary dictionary)
	{
		ArrayList arrayList = new ArrayList(dictionary.Count);
		IEnumerator enumerator = dictionary.Keys.GetEnumerator();
		while (enumerator.MoveNext())
		{
			arrayList.Add(enumerator.Current);
		}
		object[] array = new object[arrayList.Count];
		arrayList.CopyTo(array);
		return array;
	}

	protected static string GetLocalTimeZone(int month, int day)
	{
		switch (month)
		{
		case 1:
		case 2:
		case 3:
			return daylightSavingsTimeZone;
		case 4:
			if (day > 2)
			{
				return timeZone;
			}
			return daylightSavingsTimeZone;
		case 5:
		case 6:
		case 7:
		case 8:
		case 9:
			return timeZone;
		case 10:
			if (day < 30)
			{
				return timeZone;
			}
			return daylightSavingsTimeZone;
		case 11:
		case 12:
			return daylightSavingsTimeZone;
		default:
			return timeZone;
		}
	}

	public abstract string Format(object o);

	public abstract void Format(object o, StringBuilder sb);

	public abstract DataFormatter Copy();
}
