using System;
using NSpring.Logging.EventFormatters;

namespace NSpring.Logging;

public sealed class Event
{
	private static EventFormatter eventFormatter = new PatternEventFormatter("{msg}");

	private Context context;

	private string id;

	private Level level;

	private DateTime timestamp;

	private string message;

	private object data;

	private string category;

	private string code;

	public Context Context
	{
		get
		{
			return context;
		}
		set
		{
			if (context == null)
			{
				context = value;
			}
		}
	}

	public string ID
	{
		get
		{
			return id;
		}
		set
		{
			if (id == null)
			{
				id = value;
			}
		}
	}

	public Level Level
	{
		get
		{
			return level;
		}
		set
		{
			if (level == null)
			{
				level = value;
			}
		}
	}

	public DateTime Timestamp => timestamp;

	public string Message
	{
		get
		{
			return message;
		}
		set
		{
			if (message == null)
			{
				message = value;
			}
		}
	}

	public object Data
	{
		get
		{
			return data;
		}
		set
		{
			if (data == null)
			{
				data = value;
			}
		}
	}

	public string Category
	{
		get
		{
			return category;
		}
		set
		{
			if (category == null)
			{
				category = value;
			}
		}
	}

	public string Code
	{
		get
		{
			return code;
		}
		set
		{
			if (code == null)
			{
				code = value;
			}
		}
	}

	public Event()
	{
		timestamp = DateTime.Now;
	}

	public Event(DateTime _timestamp)
	{
		timestamp = _timestamp.ToLocalTime();
	}

	public Event(Level _level, string _message, object _data, string _category, string _code)
	{
		timestamp = DateTime.Now;
		level = _level;
		message = _message;
		data = _data;
		category = _category;
		code = _code;
		context = null;
		id = null;
	}

	public Event(DateTime _timestamp, Level _level, string _message, object _data, string _category, string _code)
	{
		timestamp = _timestamp.ToLocalTime();
		level = _level;
		message = _message;
		data = _data;
		category = _category;
		code = _code;
		context = null;
		id = null;
	}

	public Event(Level _level, string _message, object _data, string _category, string _code, string _id)
	{
		timestamp = DateTime.Now;
		level = _level;
		message = _message;
		data = _data;
		category = _category;
		code = _code;
		id = _id;
		context = null;
	}

	public Event(DateTime _timestamp, Level _level, string _message, object _data, string _category, string _code, string _id)
	{
		timestamp = _timestamp.ToLocalTime();
		level = _level;
		message = _message;
		data = _data;
		category = _category;
		code = _code;
		id = _id;
		context = null;
	}

	public override string ToString()
	{
		return eventFormatter.Format(this);
	}
}
