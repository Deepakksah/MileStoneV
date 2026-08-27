using System;
using System.Globalization;
using System.IO;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules;

public class RecurringSchedule : IRecurringSchedule
{
	private DateTime _creationTime;

	private ScheduleTypes.FrequencyType _frequency;

	private DailyInfo _daily;

	private WeeklyInfo _weekly;

	private MonthlyInfo _monthly;

	private SubdayInfo _subday;

	public bool IsDefined => _frequency != ScheduleTypes.FrequencyType.Undefined;

	public DateTime CreationTime => _creationTime;

	private void Initialize()
	{
		_creationTime = DateTime.UtcNow;
		_frequency = ScheduleTypes.FrequencyType.Undefined;
		_daily = new DailyInfo();
		_weekly = new WeeklyInfo();
		_monthly = new MonthlyInfo();
		_subday = new SubdayInfo();
	}

	private DateTime GetDayForScheduledTime(DateTime time)
	{
		DateTime dateTime = new DateTime(time.Year, time.Month, time.Day);
		if (_subday.Type == ScheduleTypes.SubdayType.Recurring && _subday.Recurring.IsMidnightCrossed())
		{
			DateTime dateTime2 = ScheduleTypeHelpers.SetTimeOfDayAndAccountForDST(dateTime, _subday.Recurring.EndTimeOfDay);
			if (time <= dateTime2)
			{
				return ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, -1);
			}
		}
		return dateTime;
	}

	private bool IsDayScheduled(DateTime time)
	{
		switch (_frequency)
		{
		case ScheduleTypes.FrequencyType.Daily:
			return true;
		case ScheduleTypes.FrequencyType.Weekly:
		{
			int weekDay = ScheduleTypeHelpers.GetWeekDay(time);
			return _weekly.Days[weekDay];
		}
		case ScheduleTypes.FrequencyType.Monthly:
			switch (_monthly.Type)
			{
			case ScheduleTypes.MonthlyType.AbsoluteDay:
				return time.Day == _monthly.AbsoluteDay.Day;
			case ScheduleTypes.MonthlyType.RelativeDay:
				return time.Day == FindMonthlyScheduledDay(time);
			}
			break;
		}
		return false;
	}

	private int FindMonthlyScheduledDay(DateTime time)
	{
		int year = time.Year;
		int month = time.Month;
		DateTime time2 = new DateTime(year, month, 1);
		DateTime time3 = ScheduleTypeHelpers.AddMonths(time2, 1);
		time3 = ScheduleTypeHelpers.AddDaysAndAccountForDST(time3, -1);
		int result = -1;
		int num = 0;
		int num2 = ScheduleTypeHelpers.GetWeekDay(time2);
		for (int i = time2.Day; i <= time3.Day; i++)
		{
			if (ScheduleTypeHelpers.IsDayOfType(num2, _monthly.RelativeDay.Day))
			{
				result = i;
				num++;
				switch (_monthly.RelativeDay.Offset)
				{
				case ScheduleTypes.MonthlyRelativeOffset.First:
					if (num == 1)
					{
						return i;
					}
					break;
				case ScheduleTypes.MonthlyRelativeOffset.Second:
					if (num == 2)
					{
						return i;
					}
					break;
				case ScheduleTypes.MonthlyRelativeOffset.Third:
					if (num == 3)
					{
						return i;
					}
					break;
				case ScheduleTypes.MonthlyRelativeOffset.Fourth:
					if (num == 4)
					{
						return i;
					}
					break;
				}
			}
			num2 = (num2 + 1) % 7;
		}
		if (_monthly.RelativeDay.Offset == ScheduleTypes.MonthlyRelativeOffset.Last && num > 0)
		{
			return result;
		}
		return -1;
	}

	private bool GetNextSubDayTime(DateTime scheduledDay, DateTime prevTime, ref DateTime nextTime)
	{
		switch (_subday.Type)
		{
		case ScheduleTypes.SubdayType.Once:
			nextTime = ScheduleTypeHelpers.SetTimeOfDayAndAccountForDST(scheduledDay, _subday.Once.TimeOfDay);
			return nextTime > prevTime;
		case ScheduleTypes.SubdayType.Recurring:
		{
			nextTime = ScheduleTypeHelpers.SetTimeOfDayAndAccountForDST(scheduledDay, _subday.Recurring.StartTimeOfDay);
			DateTime dateTime = ((!_subday.Recurring.IsMidnightCrossed()) ? ScheduleTypeHelpers.SetTimeOfDayAndAccountForDST(scheduledDay, _subday.Recurring.EndTimeOfDay) : ScheduleTypeHelpers.SetTimeOfDayAndAccountForDST(ScheduleTypeHelpers.AddDaysAndAccountForDST(scheduledDay, 1), _subday.Recurring.EndTimeOfDay));
			while (nextTime <= prevTime && nextTime < dateTime)
			{
				nextTime += _subday.Recurring.Frequency;
			}
			if (nextTime > dateTime)
			{
				return false;
			}
			return nextTime > prevTime;
		}
		default:
			return false;
		}
	}

	public RecurringSchedule()
	{
		_frequency = ScheduleTypes.FrequencyType.Undefined;
	}

	private void ReadXml(XmlElement elem)
	{
		Initialize();
		try
		{
			if (elem.NodeType != XmlNodeType.Element || !string.Equals(elem.LocalName, ScheduleXmlConstants.ScheduleElementName, StringComparison.OrdinalIgnoreCase))
			{
				throw new ArgumentException(FormattableString.Invariant($"Input node is not a '{ScheduleXmlConstants.ScheduleElementName}' element."), "elem");
			}
			XmlAttribute attributeNode = elem.GetAttributeNode(ScheduleXmlConstants.CreationTimeAttributeName);
			if (attributeNode == null)
			{
				throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.ScheduleElementName}' '{ScheduleXmlConstants.CreationTimeAttributeName}' attribute not found."));
			}
			_creationTime = XmlConvert.ToDateTime(attributeNode.Value, XmlDateTimeSerializationMode.Utc);
			bool flag = false;
			bool flag2 = false;
			XmlNode xmlNode = elem;
			for (xmlNode = xmlNode.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
			{
				if (xmlNode.NodeType == XmlNodeType.Element)
				{
					if (string.Equals(xmlNode.LocalName, ScheduleXmlConstants.FrequencyElementName, StringComparison.OrdinalIgnoreCase))
					{
						flag = ReadFrequencyElement(xmlNode);
					}
					else if (string.Equals(xmlNode.LocalName, ScheduleXmlConstants.SubDayElementName, StringComparison.OrdinalIgnoreCase))
					{
						flag2 = ReadSubDayElement(xmlNode);
					}
				}
			}
			if (!flag)
			{
				throw new InvalidScheduleXmlException(FormattableString.Invariant($"Node '{ScheduleXmlConstants.FrequencyElementName}' not found."));
			}
			if (!flag2)
			{
				throw new InvalidScheduleXmlException(FormattableString.Invariant($"Node '{ScheduleXmlConstants.SubDayElementName}' not found."));
			}
		}
		catch
		{
			throw;
		}
	}

	private bool ReadSubDayElement(XmlNode node)
	{
		XmlElement xmlElement = (XmlElement)node;
		XmlAttribute attributeNode = xmlElement.GetAttributeNode(ScheduleXmlConstants.TypeAttributeName);
		if (attributeNode == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute not found."));
		}
		string value = attributeNode.Value;
		_subday.Type = ScheduleTypeHelpers.GetSubDayTypeEnum(value);
		if (_subday.Type == ScheduleTypes.SubdayType.Undefined)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute must be '{ScheduleTypeHelpers.GetSubDayTypeString(ScheduleTypes.SubdayType.Once)}'/'{ScheduleTypeHelpers.GetSubDayTypeString(ScheduleTypes.SubdayType.Recurring)}'."));
		}
		bool flag = false;
		bool flag2 = false;
		for (XmlNode xmlNode = xmlElement.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement2 = (XmlElement)xmlNode;
				if (xmlElement2.LocalName == ScheduleTypeHelpers.GetSubDayTypeString(ScheduleTypes.SubdayType.Once))
				{
					XmlAttribute attributeNode2 = xmlElement2.GetAttributeNode(ScheduleXmlConstants.TimeOfDayAttributeName);
					if (attributeNode2 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.OnceElementName}' '{ScheduleXmlConstants.TimeOfDayAttributeName}' attribute not found."));
					}
					_subday.Once.TimeOfDay = new TimeSpan(0, 0, 0, XMLReadNumber(attributeNode2));
					flag = true;
				}
				else if (xmlElement2.LocalName == ScheduleTypeHelpers.GetSubDayTypeString(ScheduleTypes.SubdayType.Recurring))
				{
					XmlAttribute attributeNode3 = xmlElement2.GetAttributeNode(ScheduleXmlConstants.FrequencyAttributeName);
					if (attributeNode3 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.RecurringElementName}' '{ScheduleXmlConstants.FrequencyAttributeName}' attribute not found."));
					}
					_subday.Recurring.Frequency = new TimeSpan(0, 0, 0, XMLReadNumber(attributeNode3));
					XmlAttribute attributeNode4 = xmlElement2.GetAttributeNode(ScheduleXmlConstants.StartTimeOfDayAttributeName);
					if (attributeNode4 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.RecurringElementName}' '{ScheduleXmlConstants.StartTimeOfDayAttributeName}' attribute not found."));
					}
					_subday.Recurring.StartTimeOfDay = new TimeSpan(0, 0, 0, XMLReadNumber(attributeNode4));
					XmlAttribute attributeNode5 = xmlElement2.GetAttributeNode(ScheduleXmlConstants.EndTimeOfDayAttributeName);
					if (attributeNode5 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' '{ScheduleXmlConstants.RecurringElementName}' '{ScheduleXmlConstants.EndTimeOfDayAttributeName}' attribute not found."));
					}
					_subday.Recurring.EndTimeOfDay = new TimeSpan(0, 0, 0, XMLReadNumber(attributeNode5));
					flag2 = true;
				}
			}
		}
		if (_subday.Type == ScheduleTypes.SubdayType.Once && !flag)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' 'type' was once but '{ScheduleXmlConstants.OnceElementName}' section was not found."));
		}
		if (_subday.Type == ScheduleTypes.SubdayType.Recurring && !flag2)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.SubDayElementName}' 'type' was recurring but '{ScheduleXmlConstants.RecurringElementName}' section was not found."));
		}
		return true;
	}

	private bool ReadFrequencyElement(XmlNode node)
	{
		XmlAttribute attributeNode = ((XmlElement)node).GetAttributeNode(ScheduleXmlConstants.TypeAttributeName);
		if (attributeNode == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.FrequencyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute not found."));
		}
		string value = attributeNode.Value;
		_frequency = ScheduleTypeHelpers.GetFrequencyTypeEnum(value);
		if (_frequency == ScheduleTypes.FrequencyType.Undefined)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.FrequencyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute must be ") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Daily)}'/'{ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Weekly)}'/'{ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Monthly)}'."));
		}
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		for (XmlNode xmlNode = node.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (string.Equals(xmlElement.LocalName, ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Daily), StringComparison.OrdinalIgnoreCase))
				{
					flag = ReadDailyFrequencyElement(xmlElement);
				}
				else if (string.Equals(xmlElement.LocalName, ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Weekly), StringComparison.OrdinalIgnoreCase))
				{
					flag2 = ReadWeeklyFrequencyElement(xmlElement);
				}
				else if (string.Equals(xmlElement.LocalName, ScheduleTypeHelpers.GetFrequencyTypeString(ScheduleTypes.FrequencyType.Monthly), StringComparison.OrdinalIgnoreCase))
				{
					flag3 = ReadMonthlyFrequencyElement(xmlElement);
				}
			}
		}
		if (_frequency == ScheduleTypes.FrequencyType.Daily && !flag)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.FrequencyElementName}' type was daily but '{ScheduleXmlConstants.DailyElementName}' section was not found."));
		}
		if (_frequency == ScheduleTypes.FrequencyType.Weekly && !flag2)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.FrequencyElementName}' type was weekly but '{ScheduleXmlConstants.WeeklyElementName}' section was not found."));
		}
		if (_frequency == ScheduleTypes.FrequencyType.Monthly && !flag3)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.FrequencyElementName}' type was monthly but '{ScheduleXmlConstants.MonthlyElementName}' section was not found."));
		}
		return true;
	}

	private bool ReadMonthlyFrequencyElement(XmlElement freqElem)
	{
		XmlAttribute attributeNode = freqElem.GetAttributeNode(ScheduleXmlConstants.TypeAttributeName);
		if (attributeNode == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute not found."));
		}
		string value = attributeNode.Value;
		_monthly.Type = ScheduleTypeHelpers.GetMonthlyTypeEnum(value);
		if (_monthly.Type == ScheduleTypes.MonthlyType.Undefined)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' attribute must be '{ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.AbsoluteDay)}'/'{ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.RelativeDay)}'."));
		}
		XmlAttribute attributeNode2 = freqElem.GetAttributeNode(ScheduleXmlConstants.EveryAttributeName);
		if (attributeNode2 == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.EveryAttributeName}' attribute not found."));
		}
		_monthly.Day = XMLReadNumber(attributeNode2);
		bool flag = false;
		bool flag2 = false;
		for (XmlNode xmlNode = freqElem.FirstChild; xmlNode != null; xmlNode = xmlNode.NextSibling)
		{
			if (xmlNode.NodeType == XmlNodeType.Element)
			{
				XmlElement xmlElement = (XmlElement)xmlNode;
				if (string.Equals(xmlElement.LocalName, ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.AbsoluteDay), StringComparison.OrdinalIgnoreCase))
				{
					XmlAttribute attributeNode3 = xmlElement.GetAttributeNode(ScheduleXmlConstants.DayAttributeName);
					if (attributeNode3 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.AbsoluteDayElementName}' '{ScheduleXmlConstants.DayAttributeName}' attribute not found."));
					}
					_monthly.AbsoluteDay.Day = XMLReadNumber(attributeNode3);
					flag = true;
				}
				else if (string.Equals(xmlElement.LocalName, ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.RelativeDay), StringComparison.OrdinalIgnoreCase))
				{
					XmlAttribute attributeNode4 = xmlElement.GetAttributeNode(ScheduleXmlConstants.OffsetAttributeName);
					if (attributeNode4 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.RelativeDayElementName}' '{ScheduleXmlConstants.OffsetAttributeName}' attribute not found."));
					}
					string value2 = attributeNode4.Value;
					_monthly.RelativeDay.Offset = ScheduleTypeHelpers.GetMonthlyRelativeOffsetEnum(value2);
					if (_monthly.RelativeDay.Offset == ScheduleTypes.MonthlyRelativeOffset.Undefined)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.RelativeDayElementName}' '{ScheduleXmlConstants.OffsetAttributeName}' attribute must be ") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset.First)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset.Second)}'/") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset.Third)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset.Fourth)}'/") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeOffsetString(ScheduleTypes.MonthlyRelativeOffset.Last)}'."));
					}
					XmlAttribute attributeNode5 = xmlElement.GetAttributeNode(ScheduleXmlConstants.DayAttributeName);
					if (attributeNode5 == null)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.RelativeDayElementName}' '{ScheduleXmlConstants.DayAttributeName}' attribute not found."));
					}
					string value3 = attributeNode5.Value;
					_monthly.RelativeDay.Day = ScheduleTypeHelpers.GetMonthlyRelativeDayEnum(value3);
					if (_monthly.RelativeDay.Day == ScheduleTypes.MonthlyRelativeDay.Undefined)
					{
						throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.RelativeDayElementName}' '{ScheduleXmlConstants.DayAttributeName}' attribute must be ") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Monday)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Tuesday)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Wednesday)}'/") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Thursday)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Friday)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Saturday)}'/") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Sunday)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.Day)}'/'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.WeekDay)}'/") + FormattableString.Invariant($"'{ScheduleTypeHelpers.GetMonthlyRelativeDayString(ScheduleTypes.MonthlyRelativeDay.WeekendDay)}'."));
					}
					flag2 = true;
				}
			}
		}
		if (_monthly.Type == ScheduleTypes.MonthlyType.AbsoluteDay && !flag)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' was {ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.AbsoluteDay)} but '{ScheduleXmlConstants.AbsoluteDayElementName}' section was not found."));
		}
		if (_monthly.Type == ScheduleTypes.MonthlyType.RelativeDay && !flag2)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.MonthlyElementName}' '{ScheduleXmlConstants.TypeAttributeName}' was {ScheduleTypeHelpers.GetMonthlyTypeString(ScheduleTypes.MonthlyType.RelativeDay)} but '{ScheduleXmlConstants.RelativeDayElementName}' section was not found."));
		}
		return true;
	}

	private bool ReadWeeklyFrequencyElement(XmlElement freqElem)
	{
		XmlAttribute attributeNode = freqElem.GetAttributeNode(ScheduleXmlConstants.EveryAttributeName);
		if (attributeNode == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.WeeklyElementName}' '{ScheduleXmlConstants.EveryAttributeName}' attribute not found."));
		}
		_weekly.Every = XMLReadNumber(attributeNode);
		for (int i = 0; i < 7; i++)
		{
			XmlAttribute attributeNode2 = freqElem.GetAttributeNode(ScheduleTypeHelpers.GetDayString(i));
			if (attributeNode2 != null && attributeNode2.Value == "true")
			{
				_weekly.Days[i] = true;
			}
		}
		return true;
	}

	private bool ReadDailyFrequencyElement(XmlElement freqElem)
	{
		XmlAttribute attributeNode = freqElem.GetAttributeNode(ScheduleXmlConstants.EveryAttributeName);
		if (attributeNode == null)
		{
			throw new InvalidScheduleXmlException(FormattableString.Invariant($"'{ScheduleXmlConstants.DailyElementName}' '{ScheduleXmlConstants.EveryAttributeName}' attribute not found."));
		}
		_daily.Every = XMLReadNumber(attributeNode);
		return true;
	}

	public void ReadXmlString(string configuration)
	{
		XmlDocument xmlDocument = new XmlDocument
		{
			XmlResolver = null
		};
		using XmlReader reader = XmlReader.Create(new StringReader(configuration), new XmlReaderSettings
		{
			XmlResolver = null
		});
		xmlDocument.Load(reader);
		ReadXml(xmlDocument.DocumentElement);
	}

	private static int XMLReadNumber(XmlAttribute attr)
	{
		if (!int.TryParse(attr.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out var result))
		{
			throw new InvalidScheduleXmlException("Attribute does not contain a valid integer value.");
		}
		return result;
	}

	public DateTime GetNextScheduleTime(DateTime previousTime)
	{
		DateTime nextTime = DateTime.MinValue;
		DateTime dateTime = GetDayForScheduledTime(previousTime);
		switch (_frequency)
		{
		case ScheduleTypes.FrequencyType.Daily:
			if (GetNextSubDayTime(dateTime, previousTime, ref nextTime))
			{
				return nextTime;
			}
			dateTime = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, _daily.Every);
			break;
		case ScheduleTypes.FrequencyType.Weekly:
		{
			if (IsDayScheduled(dateTime) && GetNextSubDayTime(dateTime, previousTime, ref nextTime))
			{
				return nextTime;
			}
			bool flag = false;
			int weekDay = ScheduleTypeHelpers.GetWeekDay(dateTime);
			for (int i = weekDay + 1; i < 7; i++)
			{
				if (_weekly.Days[i])
				{
					dateTime = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, i - weekDay);
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				dateTime = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, -weekDay);
				dateTime = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, 7 * _weekly.Every);
				for (int i = 0; i < 7; i++)
				{
					if (_weekly.Days[i])
					{
						dateTime = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, i);
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				throw new InvalidScheduleXmlException("Error generating next schedule time, no weekdays are enabled for weekly schedule.");
			}
			break;
		}
		case ScheduleTypes.FrequencyType.Monthly:
			if (IsDayScheduled(dateTime) && GetNextSubDayTime(dateTime, previousTime, ref nextTime))
			{
				return nextTime;
			}
			switch (_monthly.Type)
			{
			case ScheduleTypes.MonthlyType.AbsoluteDay:
			{
				DateTime time = ScheduleTypeHelpers.AddMonths(dateTime, 1);
				time = ScheduleTypeHelpers.AddDaysAndAccountForDST(time, -1);
				dateTime = ((dateTime.Day < _monthly.AbsoluteDay.Day && dateTime.Day < time.Day) ? ScheduleTypeHelpers.AddMonths(dateTime, 0) : ScheduleTypeHelpers.AddMonths(dateTime, _monthly.Day));
				DateTime dateTime2 = ScheduleTypeHelpers.AddDaysAndAccountForDST(dateTime, _monthly.AbsoluteDay.Day - 1);
				dateTime = ((dateTime2.Month != dateTime.Month) ? time : dateTime2);
				break;
			}
			case ScheduleTypes.MonthlyType.RelativeDay:
			{
				dateTime = ScheduleTypeHelpers.AddMonths(dateTime, _monthly.Day);
				int num = FindMonthlyScheduledDay(dateTime);
				if (num == -1)
				{
					throw new InvalidScheduleXmlException("Error generating next schedule time, no days scheduled in the month.");
				}
				dateTime = new DateTime(dateTime.Year, dateTime.Month, num);
				break;
			}
			default:
				throw new InvalidScheduleXmlException("Error generating next schedule time, monthly type unsupported.");
			}
			break;
		default:
			throw new InvalidScheduleXmlException("Error generating next schedule time, frequency type unsupported.");
		}
		if (GetNextSubDayTime(dateTime, previousTime, ref nextTime))
		{
			return nextTime;
		}
		throw new InvalidScheduleXmlException("Error generating next schedule time.");
	}

	public DateTime FindFirstScheduleTime()
	{
		return GetNextScheduleTime(_creationTime);
	}
}
