using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Management.Common;

public sealed class Schedule
{
	private FrequencyType _frequencyType;

	private int _frequencyRecurrenceFactor;

	private int _frequencyInterval;

	private FrequencyRelativeInterval _frequencyRelativeInterval;

	private FrequencySubDayType _frequencySubDayType;

	private int _frequencySubDayInterval;

	private TimeSpan _activeStartTimeOfDay;

	private TimeSpan _activeEndTimeOfDay;

	public FrequencyType FrequencyType
	{
		get
		{
			return _frequencyType;
		}
		set
		{
			_frequencyType = value;
			RaiseScheduleChanged();
		}
	}

	public int FrequencyRecurrenceFactor
	{
		get
		{
			return _frequencyRecurrenceFactor;
		}
		set
		{
			_frequencyRecurrenceFactor = value;
			RaiseScheduleChanged();
		}
	}

	public int FrequencyInterval
	{
		get
		{
			return _frequencyInterval;
		}
		set
		{
			_frequencyInterval = value;
			RaiseScheduleChanged();
		}
	}

	public FrequencyRelativeInterval FrequencyRelativeInterval
	{
		get
		{
			return _frequencyRelativeInterval;
		}
		set
		{
			_frequencyRelativeInterval = value;
			RaiseScheduleChanged();
		}
	}

	public FrequencySubDayType FrequencySubDayType
	{
		get
		{
			return _frequencySubDayType;
		}
		set
		{
			_frequencySubDayType = value;
			RaiseScheduleChanged();
		}
	}

	public int FrequencySubDayInterval
	{
		get
		{
			return _frequencySubDayInterval;
		}
		set
		{
			_frequencySubDayInterval = value;
			RaiseScheduleChanged();
		}
	}

	public TimeSpan ActiveStartTimeOfDay
	{
		get
		{
			return _activeStartTimeOfDay;
		}
		set
		{
			_activeStartTimeOfDay = value;
			RaiseScheduleChanged();
		}
	}

	public TimeSpan ActiveEndTimeOfDay
	{
		get
		{
			return _activeEndTimeOfDay;
		}
		set
		{
			_activeEndTimeOfDay = value;
			RaiseScheduleChanged();
		}
	}

	public event EventHandler ScheduleChanged;

	private void RaiseScheduleChanged()
	{
		if (this.ScheduleChanged != null)
		{
			this.ScheduleChanged(this, null);
		}
	}

	public Schedule()
	{
		SetDefaults();
	}

	public void ApplyValues(Schedule schedule)
	{
		if (schedule == null)
		{
			throw new ArgumentNullException("schedule");
		}
		ActiveEndTimeOfDay = schedule.ActiveEndTimeOfDay;
		ActiveStartTimeOfDay = schedule.ActiveStartTimeOfDay;
		FrequencyInterval = schedule.FrequencyInterval;
		FrequencyRecurrenceFactor = schedule.FrequencyRecurrenceFactor;
		FrequencyRelativeInterval = schedule.FrequencyRelativeInterval;
		FrequencySubDayInterval = schedule.FrequencySubDayInterval;
		FrequencySubDayType = schedule.FrequencySubDayType;
		FrequencyType = schedule.FrequencyType;
	}

	public static Schedule FromXml(XmlReader reader)
	{
		Schedule schedule = new Schedule();
		if (!reader.Read())
		{
			return schedule;
		}
		if (!reader.IsEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null && name.Equals("schedule", StringComparison.OrdinalIgnoreCase))
				{
					ReadScheduleAttributes(reader);
				}
			}
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name2 = reader.Name;
				if (name2 != null)
				{
					if (name2.Equals("frequency", StringComparison.OrdinalIgnoreCase))
					{
						ReadFrequencyElement(schedule, reader);
						continue;
					}
					if (name2.Equals("subday", StringComparison.OrdinalIgnoreCase))
					{
						ReadSubdayElement(schedule, reader);
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return schedule;
	}

	public static Schedule FromXml(string scheduleXml)
	{
		if (string.IsNullOrEmpty(scheduleXml))
		{
			return new Schedule();
		}
		using StringReader input = new StringReader(scheduleXml);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		return FromXml(XmlReader.Create(input, xmlReaderSettings));
	}

	private void SetDefaults()
	{
		FrequencyType = FrequencyType.Daily;
		FrequencyRecurrenceFactor = 1;
		FrequencyInterval = 1;
		FrequencySubDayType = FrequencySubDayType.Once;
		FrequencySubDayInterval = 0;
		ActiveStartTimeOfDay = TimeSpan.Zero;
		ActiveEndTimeOfDay = new TimeSpan(0, 23, 59, 59);
	}

	public void ToXml(XmlWriter writer)
	{
		writer.WriteStartElement("schedule");
		writer.WriteAttributeString("creationtime", DateTime.Now.ToString("o", CultureInfo.InvariantCulture));
		writer.WriteStartElement("frequency");
		writer.WriteAttributeString("type", GetFrequencyTypeText(FrequencyType));
		switch (FrequencyType)
		{
		case FrequencyType.Daily:
			writer.WriteStartElement("daily");
			writer.WriteAttributeString("every", XmlConvert.ToString(FrequencyInterval));
			writer.WriteEndElement();
			break;
		case FrequencyType.Weekly:
			writer.WriteStartElement("weekly");
			writer.WriteAttributeString("every", XmlConvert.ToString(FrequencyRecurrenceFactor));
			writer.WriteAttributeString("monday", Convert.ToString((FrequencyInterval & 1) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("tuesday", Convert.ToString((FrequencyInterval & 2) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("wednesday", Convert.ToString((FrequencyInterval & 4) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("thursday", Convert.ToString((FrequencyInterval & 8) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("friday", Convert.ToString((FrequencyInterval & 0x10) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("saturday", Convert.ToString((FrequencyInterval & 0x20) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteAttributeString("sunday", Convert.ToString((FrequencyInterval & 0x40) > 0, CultureInfo.InvariantCulture).ToLowerInvariant());
			writer.WriteEndElement();
			break;
		case FrequencyType.Monthly:
			writer.WriteStartElement("monthly");
			writer.WriteAttributeString("type", "absoluteday");
			writer.WriteAttributeString("every", XmlConvert.ToString(FrequencyRecurrenceFactor));
			writer.WriteStartElement("absoluteday");
			writer.WriteAttributeString("day", XmlConvert.ToString(FrequencyInterval));
			writer.WriteEndElement();
			writer.WriteEndElement();
			break;
		case FrequencyType.MonthlyRelative:
			writer.WriteStartElement("monthly");
			writer.WriteAttributeString("type", "relativeday");
			writer.WriteAttributeString("every", XmlConvert.ToString(FrequencyRecurrenceFactor));
			writer.WriteStartElement("relativeday");
			writer.WriteAttributeString("offset", GetFrequencyRelativeIntervalText(FrequencyRelativeInterval));
			writer.WriteAttributeString("day", GetRelativeDayText(FrequencyInterval));
			writer.WriteEndElement();
			writer.WriteEndElement();
			break;
		}
		writer.WriteEndElement();
		writer.WriteStartElement("subday");
		switch (FrequencySubDayType)
		{
		case FrequencySubDayType.Once:
			writer.WriteAttributeString("type", "once");
			writer.WriteStartElement("once");
			writer.WriteAttributeString("timeofday", XmlConvert.ToString(Math.Round(ActiveStartTimeOfDay.TotalSeconds)));
			writer.WriteEndElement();
			break;
		case FrequencySubDayType.Hour:
		{
			writer.WriteAttributeString("type", "recurring");
			writer.WriteStartElement("recurring");
			int num = FrequencySubDayInterval * 60 * 60;
			writer.WriteAttributeString("frequency", XmlConvert.ToString(num));
			writer.WriteAttributeString("starttimeofday", XmlConvert.ToString(Math.Round(ActiveStartTimeOfDay.TotalSeconds)));
			writer.WriteAttributeString("endtimeofday", XmlConvert.ToString(Math.Round(ActiveEndTimeOfDay.TotalSeconds)));
			writer.WriteEndElement();
			break;
		}
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	public string ToXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.OmitXmlDeclaration = true;
		using (XmlWriter writer = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			ToXml(writer);
		}
		return stringBuilder.ToString();
	}

	private static void ReadScheduleAttributes(XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				reader.Name?.Equals("creationtime", StringComparison.OrdinalIgnoreCase);
			}
		}
		reader.Read();
	}

	private static void ReadFrequencyElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					schedule.FrequencyType = GetFrequencyTypeEnum(Helper.StringFromXmlAttribute(reader));
				}
			}
		}
		reader.Read();
		if (schedule.FrequencyType == FrequencyType.Daily)
		{
			ReadDailyElement(schedule, reader);
		}
		else if (schedule.FrequencyType == FrequencyType.Weekly)
		{
			ReadWeeklyElement(schedule, reader);
		}
		else if (schedule.FrequencyType == FrequencyType.Monthly || schedule.FrequencyType == FrequencyType.MonthlyRelative)
		{
			ReadMonthlyElement(schedule, reader);
		}
		reader.ReadEndElement();
	}

	private static void ReadDailyElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("every", StringComparison.OrdinalIgnoreCase))
				{
					schedule.FrequencyInterval = XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader));
				}
			}
		}
		reader.Read();
	}

	private static void ReadWeeklyElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			schedule.FrequencyInterval = 0;
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("every", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencyRecurrenceFactor = XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader));
					}
					Helper.WeekDayFromAttribute(schedule, reader);
				}
			}
		}
		reader.Read();
	}

	private static void ReadMonthlyElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name == null)
				{
					continue;
				}
				if (name.Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					string text = Helper.StringFromXmlAttribute(reader);
					if (text.Equals("absoluteday", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencyType = FrequencyType.Monthly;
					}
					if (text.Equals("relativeday", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencyType = FrequencyType.MonthlyRelative;
					}
				}
				if (name.Equals("every", StringComparison.OrdinalIgnoreCase))
				{
					schedule.FrequencyRecurrenceFactor = XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader));
				}
			}
		}
		reader.Read();
		if (schedule.FrequencyType == FrequencyType.Monthly)
		{
			ReadMonthlyAbsoluteElement(schedule, reader);
		}
		else if (schedule.FrequencyType == FrequencyType.MonthlyRelative)
		{
			ReadMonthlyRelativeElement(schedule, reader);
		}
		reader.ReadEndElement();
	}

	private static void ReadMonthlyAbsoluteElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("day", StringComparison.OrdinalIgnoreCase))
				{
					schedule.FrequencyInterval = XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader));
				}
			}
		}
		reader.Read();
	}

	private static void ReadMonthlyRelativeElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("offset", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencyRelativeInterval = GetFrequencyRelativeIntervalEnum(Helper.StringFromXmlAttribute(reader));
					}
					Helper.MonthRelativeFromAttribute(schedule, reader);
				}
			}
		}
		reader.Read();
	}

	private static void ReadSubdayElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("type", StringComparison.OrdinalIgnoreCase))
				{
					string text = Helper.StringFromXmlAttribute(reader);
					if (text.Equals("once", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencySubDayType = FrequencySubDayType.Once;
					}
					if (text.Equals("recurring", StringComparison.OrdinalIgnoreCase))
					{
						schedule.FrequencySubDayType = FrequencySubDayType.Unknown;
					}
				}
			}
		}
		reader.Read();
		if (schedule.FrequencySubDayType == FrequencySubDayType.Once)
		{
			ReadSubTypeOnceElement(schedule, reader);
		}
		else if (schedule.FrequencySubDayType == FrequencySubDayType.Unknown)
		{
			ReadSubTypeRecurringElement(schedule, reader);
		}
		reader.ReadEndElement();
	}

	private static void ReadSubTypeOnceElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("timeofday", StringComparison.OrdinalIgnoreCase))
				{
					schedule.ActiveStartTimeOfDay = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
				}
			}
		}
		reader.Read();
	}

	private static void ReadSubTypeRecurringElement(Schedule schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null)
				{
					if (name.Equals("frequency", StringComparison.OrdinalIgnoreCase))
					{
						int num = XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader));
						schedule.FrequencySubDayType = FrequencySubDayType.Hour;
						schedule.FrequencySubDayInterval = num / 60 / 60;
					}
					if (name.Equals("starttimeofday", StringComparison.OrdinalIgnoreCase))
					{
						schedule.ActiveStartTimeOfDay = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
					}
					if (name.Equals("endtimeofday", StringComparison.OrdinalIgnoreCase))
					{
						schedule.ActiveEndTimeOfDay = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
					}
				}
			}
		}
		reader.Read();
	}

	private static string GetFrequencyTypeText(FrequencyType frequencyType)
	{
		return frequencyType switch
		{
			FrequencyType.Daily => "daily", 
			FrequencyType.Weekly => "weekly", 
			FrequencyType.Monthly => "monthly", 
			FrequencyType.MonthlyRelative => "monthly", 
			_ => "unknown", 
		};
	}

	private static FrequencyType GetFrequencyTypeEnum(string text)
	{
		return text switch
		{
			"daily" => FrequencyType.Daily, 
			"weekly" => FrequencyType.Weekly, 
			"monthly" => FrequencyType.Monthly, 
			_ => FrequencyType.Unknown, 
		};
	}

	private static string GetFrequencyRelativeIntervalText(FrequencyRelativeInterval frequencyRelativeInterval)
	{
		return frequencyRelativeInterval switch
		{
			FrequencyRelativeInterval.First => "first", 
			FrequencyRelativeInterval.Second => "second", 
			FrequencyRelativeInterval.Third => "third", 
			FrequencyRelativeInterval.Fourth => "fourth", 
			FrequencyRelativeInterval.Last => "last", 
			_ => "unknown", 
		};
	}

	private static FrequencyRelativeInterval GetFrequencyRelativeIntervalEnum(string text)
	{
		return text switch
		{
			"first" => FrequencyRelativeInterval.First, 
			"second" => FrequencyRelativeInterval.Second, 
			"third" => FrequencyRelativeInterval.Third, 
			"fourth" => FrequencyRelativeInterval.Fourth, 
			"last" => FrequencyRelativeInterval.Last, 
			_ => throw new ArgumentOutOfRangeException("text", "Invalid FrequencyRelativeInterval"), 
		};
	}

	private static string GetRelativeDayText(int frequencyInterval)
	{
		return frequencyInterval switch
		{
			0 => "monday", 
			1 => "tuesday", 
			2 => "wednesday", 
			3 => "thursday", 
			4 => "friday", 
			5 => "saturday", 
			6 => "sunday", 
			7 => "day", 
			8 => "weekday", 
			9 => "weekendday", 
			_ => throw new ArgumentException("Invalid frequency interval", "frequencyInterval"), 
		};
	}

	public static bool IsFrequencyIntervalValid(FrequencyType frequencyType, int frequencyInterval)
	{
		switch (frequencyType)
		{
		case FrequencyType.Daily:
			if (1 <= frequencyInterval)
			{
				return frequencyInterval <= 100;
			}
			return false;
		case FrequencyType.Monthly:
			if (1 <= frequencyInterval)
			{
				return frequencyInterval <= 31;
			}
			return false;
		case FrequencyType.MonthlyRelative:
			if (0 <= frequencyInterval)
			{
				return frequencyInterval <= 9;
			}
			return false;
		case FrequencyType.Weekly:
			if (1 <= frequencyInterval)
			{
				return frequencyInterval <= 127;
			}
			return false;
		default:
			return true;
		}
	}
}
