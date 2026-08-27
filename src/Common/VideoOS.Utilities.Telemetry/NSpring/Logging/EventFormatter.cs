using System;
using System.Text;
using NSpring.Logging.Common;

namespace NSpring.Logging;

public abstract class EventFormatter
{
	private Logger logger;

	protected Encoding encoding = Encoding.UTF8;

	protected DataFormatter dataFormatter;

	protected string prefix;

	protected string suffix;

	protected object monitor = new object();

	private static string timeZone = Constants.ISO_8601_TIME_ZONE;

	private static string daylightSavingsTimeZone = Constants.ISO_8601_DAYLIGHT_SAVINGS_TIME_ZONE;

	public DataFormatter DataFormatter
	{
		get
		{
			return dataFormatter;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException();
			}
			DataFormatter newDataFormatter = value.Copy();
			lock (monitor)
			{
				if (UpdateDataFormatter(newDataFormatter))
				{
					dataFormatter = newDataFormatter;
					return;
				}
				throw new ArgumentException("Invalid data formatter");
			}
		}
	}

	public string Prefix
	{
		get
		{
			return prefix;
		}
		set
		{
			lock (monitor)
			{
				if (UpdatePrefix(value))
				{
					prefix = value;
					return;
				}
				throw new ApplicationException("Invalid operation");
			}
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
			lock (monitor)
			{
				if (UpdateSuffix(value))
				{
					suffix = value;
					return;
				}
				throw new ApplicationException("Invalid operation");
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
				if (!UpdateEncoding(value))
				{
					throw new ArgumentException("Invalid encoding");
				}
				dataFormatter.Encoding = encoding;
				encoding = value;
				dataFormatter.Encoding = encoding;
			}
		}
	}

	public Logger Logger
	{
		set
		{
			lock (monitor)
			{
				if (value != null)
				{
					if (logger != null)
					{
						throw new InvalidOperationException("An EventFormatter instance can only be used with one logger.  (Try using Copy() to create a deep copy)");
					}
					logger = value;
				}
			}
		}
	}

	protected virtual bool UpdatePrefix(string _newPrefix)
	{
		return true;
	}

	protected virtual bool UpdateSuffix(string _newSuffix)
	{
		return true;
	}

	protected virtual bool UpdateDataFormatter(DataFormatter _newDataFormatter)
	{
		return true;
	}

	public abstract string Format(Event _event);

	public virtual string GetEventListHeader()
	{
		return null;
	}

	public virtual string GetEventListFooter()
	{
		return null;
	}

	public virtual void Initialize()
	{
	}

	protected virtual bool UpdateEncoding(Encoding _newEncoding)
	{
		return true;
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
}
