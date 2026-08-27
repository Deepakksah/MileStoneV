using System;
using System.Collections.Specialized;
using VideoOS.Mobile.Helpers;

namespace VideoOS.Mobile.Configuration.Section;

public class TimeoutConfig : IConfigSection
{
	public const string NameDays = "Days";

	public const string NameHours = "Hours";

	public const string NameMinutes = "Minutes";

	public const string NameSeconds = "Seconds";

	public const string NameMiliSeconds = "MiliSeconds";

	private readonly string _sectionName;

	public TimeSpan Value { get; private set; }

	public string SectionName => _sectionName;

	public TimeoutConfig(TimeSpan value, string sectionName = "Timeout")
	{
		Value = value;
		_sectionName = sectionName;
	}

	public bool InitFromCollection(NameValueCollection section)
	{
		bool result = true;
		try
		{
			TimeSpan value = new TimeSpan(GetValue(section, "Days"), GetValue(section, "Hours"), GetValue(section, "Minutes"), GetValue(section, "Seconds"), GetValue(section, "MiliSeconds"));
			Value = value;
		}
		catch
		{
			result = false;
		}
		return result;
	}

	private int GetValue(NameValueCollection section, string name)
	{
		int result = 0;
		string text = section.Get(name).Sanitized();
		if (text != null)
		{
			result = Convert.ToInt32(text);
		}
		return result;
	}

	public bool ConvToCollection(NameValueCollection serverSection)
	{
		bool result = true;
		try
		{
			serverSection["Days"] = Value.Days.ToString();
			serverSection["Hours"] = Value.Hours.ToString();
			serverSection["Minutes"] = Value.Minutes.ToString();
			serverSection["Seconds"] = Value.Seconds.ToString();
			serverSection["MiliSeconds"] = Value.Milliseconds.ToString();
		}
		catch
		{
			result = false;
		}
		return result;
	}
}
