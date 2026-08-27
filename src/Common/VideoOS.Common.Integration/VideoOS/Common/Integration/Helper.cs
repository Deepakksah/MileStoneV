using System;
using System.Xml;

namespace VideoOS.Common.Integration;

public static class Helper
{
	public static string StringFromXml(XmlReader reader)
	{
		string result = string.Empty;
		bool isEmptyElement = reader.IsEmptyElement;
		reader.Read();
		if (!isEmptyElement)
		{
			if (reader.NodeType == XmlNodeType.Text)
			{
				result = reader.Value;
				reader.Read();
			}
			reader.ReadEndElement();
		}
		return result;
	}

	public static string StringFromXmlAttribute(XmlReader reader)
	{
		return reader.Value;
	}

	public static object EnumFromXmlElement(XmlReader reader, Type type)
	{
		string value = StringFromXml(reader);
		return Enum.Parse(type, value, ignoreCase: true);
	}

	public static object EnumFromXmlAttribute(XmlReader reader, Type type)
	{
		string value = StringFromXmlAttribute(reader);
		return Enum.Parse(type, value, ignoreCase: true);
	}

	public static void WeekDayFromAttribute(ScheduleData schedule, XmlReader reader)
	{
		string name = reader.Name;
		if (name.Equals("monday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 1;
			}
		}
		else if (name.Equals("tuesday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 2;
			}
		}
		else if (name.Equals("wednesday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 4;
			}
		}
		else if (name.Equals("thursday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 8;
			}
		}
		else if (name.Equals("friday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 16;
			}
		}
		else if (name.Equals("saturday", StringComparison.OrdinalIgnoreCase))
		{
			if (reader.ReadContentAsBoolean())
			{
				schedule.FrequencyInterval |= 32;
			}
		}
		else if (name.Equals("sunday", StringComparison.OrdinalIgnoreCase) && reader.ReadContentAsBoolean())
		{
			schedule.FrequencyInterval |= 64;
		}
	}

	public static void MonthRelativeFromAttribute(ScheduleData schedule, XmlReader reader)
	{
		if (!reader.Name.Equals("day", StringComparison.OrdinalIgnoreCase))
		{
			return;
		}
		string text = StringFromXmlAttribute(reader);
		if (text == null)
		{
			return;
		}
		switch (text.Length)
		{
		case 6:
			switch (text[0])
			{
			case 'm':
				if (text == "monday")
				{
					schedule.FrequencyInterval = 0;
				}
				break;
			case 'f':
				if (text == "friday")
				{
					schedule.FrequencyInterval = 4;
				}
				break;
			case 's':
				if (text == "sunday")
				{
					schedule.FrequencyInterval = 6;
				}
				break;
			}
			break;
		case 7:
			switch (text[0])
			{
			case 't':
				if (text == "tuesday")
				{
					schedule.FrequencyInterval = 1;
				}
				break;
			case 'w':
				if (text == "weekday")
				{
					schedule.FrequencyInterval = 8;
				}
				break;
			}
			break;
		case 8:
			switch (text[0])
			{
			case 't':
				if (text == "thursday")
				{
					schedule.FrequencyInterval = 3;
				}
				break;
			case 's':
				if (text == "saturday")
				{
					schedule.FrequencyInterval = 5;
				}
				break;
			}
			break;
		case 9:
			if (text == "wednesday")
			{
				schedule.FrequencyInterval = 2;
			}
			break;
		case 3:
			if (text == "day")
			{
				schedule.FrequencyInterval = 7;
			}
			break;
		case 10:
			if (text == "weekendday")
			{
				schedule.FrequencyInterval = 9;
			}
			break;
		case 4:
		case 5:
			break;
		}
	}
}
