using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration.Exceptions;
using VideoOS.Common.Integration.Properties;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

public sealed class ScheduleData
{
	private TimeSpan _endTime;

	private static TimeSpan MaxTimeSpan => new TimeSpan(23, 59, 59);

	public DateTime CreationTime { get; set; }

	public FrequencyType FrequencyType { get; set; }

	public int FrequencyInterval { get; set; }

	public FrequencyRelativeInterval FrequencyRelativeInterval { get; set; }

	public int FrequencyRecurrenceFactor { get; set; }

	public FrequencySubDayType FrequencySubDayType { get; set; }

	public int FrequencySubDayInterval { get; set; }

	public TimeSpan ActiveStartTime { get; set; }

	public TimeSpan ActiveEndTime
	{
		get
		{
			return _endTime;
		}
		set
		{
			_endTime = ((value > MaxTimeSpan) ? MaxTimeSpan : value);
		}
	}

	public ScheduleData()
	{
		SetDefaults();
	}

	public static ScheduleData FromManagementCommonSchedule(Schedule schedule)
	{
		if (schedule == null)
		{
			throw new ArgumentNullException("schedule");
		}
		ScheduleData scheduleData = new ScheduleData();
		scheduleData.CreationTime = DateTime.UtcNow;
		switch (schedule.FrequencyType)
		{
		case VideoOS.Management.Common.FrequencyType.Unknown:
			scheduleData.FrequencyType = FrequencyType.Unknown;
			break;
		case VideoOS.Management.Common.FrequencyType.Daily:
			scheduleData.FrequencyType = FrequencyType.Daily;
			break;
		case VideoOS.Management.Common.FrequencyType.Weekly:
			scheduleData.FrequencyType = FrequencyType.Weekly;
			break;
		case VideoOS.Management.Common.FrequencyType.Monthly:
			scheduleData.FrequencyType = FrequencyType.Monthly;
			break;
		case VideoOS.Management.Common.FrequencyType.MonthlyRelative:
			scheduleData.FrequencyType = FrequencyType.MonthlyRelative;
			break;
		}
		scheduleData.FrequencyInterval = schedule.FrequencyInterval;
		switch (schedule.FrequencyRelativeInterval)
		{
		case VideoOS.Management.Common.FrequencyRelativeInterval.First:
			scheduleData.FrequencyRelativeInterval = FrequencyRelativeInterval.First;
			break;
		case VideoOS.Management.Common.FrequencyRelativeInterval.Second:
			scheduleData.FrequencyRelativeInterval = FrequencyRelativeInterval.Second;
			break;
		case VideoOS.Management.Common.FrequencyRelativeInterval.Third:
			scheduleData.FrequencyRelativeInterval = FrequencyRelativeInterval.Third;
			break;
		case VideoOS.Management.Common.FrequencyRelativeInterval.Fourth:
			scheduleData.FrequencyRelativeInterval = FrequencyRelativeInterval.Fourth;
			break;
		case VideoOS.Management.Common.FrequencyRelativeInterval.Last:
			scheduleData.FrequencyRelativeInterval = FrequencyRelativeInterval.Last;
			break;
		}
		scheduleData.FrequencyRecurrenceFactor = schedule.FrequencyRecurrenceFactor;
		switch (schedule.FrequencySubDayType)
		{
		case VideoOS.Management.Common.FrequencySubDayType.Unknown:
			scheduleData.FrequencySubDayType = FrequencySubDayType.Unknown;
			break;
		case VideoOS.Management.Common.FrequencySubDayType.Once:
			scheduleData.FrequencySubDayType = FrequencySubDayType.Once;
			break;
		case VideoOS.Management.Common.FrequencySubDayType.Hour:
			scheduleData.FrequencySubDayType = FrequencySubDayType.Hour;
			break;
		}
		scheduleData.FrequencySubDayInterval = schedule.FrequencySubDayInterval;
		scheduleData.ActiveStartTime = schedule.ActiveStartTimeOfDay;
		scheduleData.ActiveEndTime = schedule.ActiveEndTimeOfDay;
		return scheduleData;
	}

	private void SetDefaults()
	{
		CreationTime = DateTime.UtcNow;
		FrequencyType = FrequencyType.Weekly;
		FrequencyRecurrenceFactor = 1;
		FrequencyInterval = 64;
		FrequencySubDayType = FrequencySubDayType.Unknown;
		FrequencySubDayInterval = 0;
		ActiveStartTime = TimeSpan.Zero;
		_endTime = MaxTimeSpan;
	}

	public void Validate()
	{
		StringBuilder stringBuilder = new StringBuilder();
		if (FrequencyType == FrequencyType.Weekly && FrequencyInterval == 0)
		{
			stringBuilder.AppendLine(Resources.InvalidWeeklySchedule);
		}
		if (ActiveStartTime == ActiveEndTime)
		{
			stringBuilder.AppendLine(Resources.EndTimeEqualToStartTime);
		}
		if (stringBuilder.Length > 0)
		{
			throw new ValidateScheduleDataException(stringBuilder.ToString());
		}
	}

	private string GetRelativeDayText(int frequencyInterval)
	{
		return FrequencyInterval switch
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
			_ => throw new ArgumentException("Invalid frequency interval"), 
		};
	}

	public void ToXml(XmlWriter writer, string element)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		writer.WriteStartElement(element);
		writer.WriteAttributeString("creationtime", CreationTime.ToString("o", CultureInfo.InvariantCulture));
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
			writer.WriteAttributeString("monday", ((FrequencyInterval & 1) > 0) ? "true" : "false");
			writer.WriteAttributeString("tuesday", ((FrequencyInterval & 2) > 0) ? "true" : "false");
			writer.WriteAttributeString("wednesday", ((FrequencyInterval & 4) > 0) ? "true" : "false");
			writer.WriteAttributeString("thursday", ((FrequencyInterval & 8) > 0) ? "true" : "false");
			writer.WriteAttributeString("friday", ((FrequencyInterval & 0x10) > 0) ? "true" : "false");
			writer.WriteAttributeString("saturday", ((FrequencyInterval & 0x20) > 0) ? "true" : "false");
			writer.WriteAttributeString("sunday", ((FrequencyInterval & 0x40) > 0) ? "true" : "false");
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
			writer.WriteAttributeString("timeofday", XmlConvert.ToString(Math.Round(ActiveStartTime.TotalSeconds)));
			writer.WriteEndElement();
			break;
		case FrequencySubDayType.Hour:
		{
			writer.WriteAttributeString("type", "recurring");
			writer.WriteStartElement("recurring");
			int num = FrequencySubDayInterval * 60 * 60;
			writer.WriteAttributeString("frequency", XmlConvert.ToString(num));
			writer.WriteAttributeString("starttimeofday", XmlConvert.ToString(Math.Round(ActiveStartTime.TotalSeconds)));
			writer.WriteAttributeString("endtimeofday", XmlConvert.ToString(Math.Round(ActiveEndTime.TotalSeconds)));
			writer.WriteEndElement();
			break;
		}
		}
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	public void ToVmoSchedule(Schedule schedule)
	{
		if (schedule == null)
		{
			throw new ArgumentNullException("schedule");
		}
		switch (FrequencyType)
		{
		case FrequencyType.Unknown:
			schedule.FrequencyType = VideoOS.Management.Common.FrequencyType.Unknown;
			break;
		case FrequencyType.Daily:
			schedule.FrequencyType = VideoOS.Management.Common.FrequencyType.Daily;
			break;
		case FrequencyType.Weekly:
			schedule.FrequencyType = VideoOS.Management.Common.FrequencyType.Weekly;
			break;
		case FrequencyType.Monthly:
			schedule.FrequencyType = VideoOS.Management.Common.FrequencyType.Monthly;
			break;
		case FrequencyType.MonthlyRelative:
			schedule.FrequencyType = VideoOS.Management.Common.FrequencyType.MonthlyRelative;
			break;
		}
		schedule.FrequencyInterval = FrequencyInterval;
		switch (FrequencyRelativeInterval)
		{
		case FrequencyRelativeInterval.First:
			schedule.FrequencyRelativeInterval = VideoOS.Management.Common.FrequencyRelativeInterval.First;
			break;
		case FrequencyRelativeInterval.Second:
			schedule.FrequencyRelativeInterval = VideoOS.Management.Common.FrequencyRelativeInterval.Second;
			break;
		case FrequencyRelativeInterval.Third:
			schedule.FrequencyRelativeInterval = VideoOS.Management.Common.FrequencyRelativeInterval.Third;
			break;
		case FrequencyRelativeInterval.Fourth:
			schedule.FrequencyRelativeInterval = VideoOS.Management.Common.FrequencyRelativeInterval.Fourth;
			break;
		case FrequencyRelativeInterval.Last:
			schedule.FrequencyRelativeInterval = VideoOS.Management.Common.FrequencyRelativeInterval.Last;
			break;
		}
		schedule.FrequencyRecurrenceFactor = FrequencyRecurrenceFactor;
		switch (FrequencySubDayType)
		{
		case FrequencySubDayType.Unknown:
			schedule.FrequencySubDayType = VideoOS.Management.Common.FrequencySubDayType.Unknown;
			break;
		case FrequencySubDayType.Once:
			schedule.FrequencySubDayType = VideoOS.Management.Common.FrequencySubDayType.Once;
			break;
		case FrequencySubDayType.Hour:
			schedule.FrequencySubDayType = VideoOS.Management.Common.FrequencySubDayType.Hour;
			break;
		}
		schedule.FrequencySubDayInterval = FrequencySubDayInterval;
		schedule.ActiveStartTimeOfDay = ActiveStartTime;
		schedule.ActiveEndTimeOfDay = ActiveEndTime;
	}

	public static ScheduleData FromXml(string scheduleXml)
	{
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.ConformanceLevel = ConformanceLevel.Fragment;
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.IgnoreComments = true;
		using StringReader input = new StringReader(scheduleXml);
		using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
		if (!xmlReader.IsStartElement())
		{
			throw new ArgumentException("Data is not valid schedule xml", "scheduleXml");
		}
		return FromXml(xmlReader);
	}

	public static ScheduleData FromXml(XmlReader reader)
	{
		ScheduleData scheduleData = new ScheduleData();
		if (!reader.IsEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Element)
			{
				string name = reader.Name;
				if (name != null && name.Equals("schedule", StringComparison.OrdinalIgnoreCase))
				{
					ReadScheduleAttributes(scheduleData, reader);
				}
			}
			while (reader.NodeType == XmlNodeType.Element)
			{
				string name2 = reader.Name;
				if (name2 != null)
				{
					if (name2.Equals("frequency", StringComparison.OrdinalIgnoreCase))
					{
						ReadFrequencyElement(scheduleData, reader);
						continue;
					}
					if (name2.Equals("subday", StringComparison.OrdinalIgnoreCase))
					{
						ReadSubdayElement(scheduleData, reader);
						continue;
					}
				}
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return scheduleData;
	}

	private static void ReadScheduleAttributes(ScheduleData schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("creationtime", StringComparison.OrdinalIgnoreCase))
				{
					schedule.CreationTime = DateTime.Parse(Helper.StringFromXmlAttribute(reader), CultureInfo.InvariantCulture);
				}
			}
		}
		reader.Read();
	}

	private static void ReadFrequencyElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadDailyElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadWeeklyElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadMonthlyElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadMonthlyAbsoluteElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadMonthlyRelativeElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadSubdayElement(ScheduleData schedule, XmlReader reader)
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

	private static void ReadSubTypeOnceElement(ScheduleData schedule, XmlReader reader)
	{
		if (reader.HasAttributes)
		{
			while (reader.MoveToNextAttribute())
			{
				string name = reader.Name;
				if (name != null && name.Equals("timeofday", StringComparison.OrdinalIgnoreCase))
				{
					schedule.ActiveStartTime = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
				}
			}
		}
		reader.Read();
	}

	private static void ReadSubTypeRecurringElement(ScheduleData schedule, XmlReader reader)
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
						schedule.ActiveStartTime = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
					}
					if (name.Equals("endtimeofday", StringComparison.OrdinalIgnoreCase))
					{
						schedule.ActiveEndTime = TimeSpan.FromSeconds(XmlConvert.ToInt32(Helper.StringFromXmlAttribute(reader)));
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
			_ => FrequencyRelativeInterval.Unknown, 
		};
	}
}
