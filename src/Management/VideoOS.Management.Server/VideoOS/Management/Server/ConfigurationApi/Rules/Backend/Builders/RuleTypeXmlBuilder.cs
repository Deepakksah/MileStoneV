using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Adapters.XmlHelper;
using VideoOS.Rules.Interfaces;
using VideoOS.Utilities.Validation;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class RuleTypeXmlBuilder : IRuleTypeXmlBuilder
{
	private static readonly IDictionary<FrequencyType, Action<XmlTextWriter, FrequencyModel>> _frequencyTypeWriterMap = new Dictionary<FrequencyType, Action<XmlTextWriter, FrequencyModel>>
	{
		{
			FrequencyType.Daily,
			WriteDailyElement
		},
		{
			FrequencyType.Weekly,
			WriteWeeklyElement
		},
		{
			FrequencyType.Monthly,
			WriteMonthlyElement
		},
		{
			FrequencyType.MonthlyRelative,
			WriteMonthlyRelativeElement
		}
	};

	public string GetXml(RulePartModel rulePart)
	{
		rulePart.NullCheck("", "GetXml", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Management.Server\\ConfigurationApi\\BusinessObjects\\Rules\\Backend\\Builders\\RuleTypeXmlBuilder.cs", 30);
		switch (rulePart.TriggerType)
		{
		case TriggerType.TimeInterval:
			return RuleXmlHelper.MakeTypeXml("TimeInterval", SourceGuid.PeriodicTimerEvent.ToString(), "", "", "");
		case TriggerType.Event:
			return RuleXmlHelper.MakeTypeXml("Event", rulePart.EventModel.EventType.ToString(), FilterSources(rulePart.EventModel.EventSources), "", "");
		case TriggerType.RecurringTime:
		{
			string schedule = WriteRecurringTime(rulePart);
			return RuleXmlHelper.MakeTypeXml("RecurringTime", SourceGuid.PeriodicTimerEvent.ToString(), "", "", schedule);
		}
		case TriggerType.Timer:
			return RuleXmlHelper.MakeTypeXml("Timer", "", "", XmlConvert.ToString(rulePart.Delay), "");
		default:
			throw new NotImplementedException();
		}
	}

	private static string WriteRecurringTime(RulePartModel rulePart)
	{
		SubDayModel subDay = rulePart.RecurringTime.SubDay;
		FrequencyModel frequency = rulePart.RecurringTime.Frequency;
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("Schedule");
			xmlTextWriter.WriteAttributeString(ScheduleXmlConstants.CreationTimeAttributeName, PropertyTypesHelper.GetValue(DateTime.UtcNow));
			WriteFrequency(xmlTextWriter, frequency);
			WriteSubDay(xmlTextWriter, subDay);
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
			stringWriter = null;
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	private static void WriteSubDay(XmlTextWriter writer, SubDayModel subDay)
	{
		ScheduleTypes.SubdayType subDayType = subDay.SubDayType;
		writer.WriteStartElement(ScheduleXmlConstants.SubDayElementName);
		switch (subDayType)
		{
		case ScheduleTypes.SubdayType.Once:
			writer.WriteAttributeString(ScheduleXmlConstants.TypeAttributeName, ScheduleXmlConstants.OnceElementName);
			writer.WriteStartElement(ScheduleXmlConstants.OnceElementName);
			writer.WriteAttributeString(ScheduleXmlConstants.TimeOfDayAttributeName, XmlConvert.ToString(subDay.StartTimeOfDay));
			break;
		case ScheduleTypes.SubdayType.Recurring:
			writer.WriteAttributeString(ScheduleXmlConstants.TypeAttributeName, ScheduleXmlConstants.RecurringElementName);
			writer.WriteStartElement(ScheduleXmlConstants.RecurringElementName);
			writer.WriteAttributeString(ScheduleXmlConstants.FrequencyAttributeName, XmlConvert.ToString(subDay.Frequency));
			writer.WriteAttributeString(ScheduleXmlConstants.StartTimeOfDayAttributeName, XmlConvert.ToString(subDay.StartTimeOfDay));
			writer.WriteAttributeString(ScheduleXmlConstants.EndTimeOfDayAttributeName, XmlConvert.ToString(subDay.EndTimeOfDay));
			break;
		}
		writer.WriteEndElement();
	}

	private static void WriteFrequency(XmlTextWriter writer, FrequencyModel frequencyModel)
	{
		FrequencyType frequencyType = frequencyModel.FrequencyType;
		writer.WriteStartElement(ScheduleXmlConstants.FrequencyAttributeName);
		writer.WriteAttributeString(ScheduleXmlConstants.TypeAttributeName, GetFrequencyTypeText(frequencyType));
		_frequencyTypeWriterMap[frequencyType](writer, frequencyModel);
		writer.WriteEndElement();
	}

	private static void WriteMonthlyRelativeElement(XmlTextWriter writer, FrequencyModel frequencyModel)
	{
		int frequency = frequencyModel.Frequency;
		writer.WriteStartElement(ScheduleXmlConstants.MonthlyElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.TypeAttributeName, ScheduleXmlConstants.RelativeDayElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.EveryAttributeName, XmlConvert.ToString(frequency));
		writer.WriteStartElement(ScheduleXmlConstants.RelativeDayElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.OffsetAttributeName, frequencyModel.Parameters["Offset"].Value.ToLowerInvariant());
		writer.WriteAttributeString(ScheduleXmlConstants.DayAttributeName, frequencyModel.Parameters["RelativeDay"].Value.ToLowerInvariant());
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static void WriteMonthlyElement(XmlTextWriter writer, FrequencyModel frequencyModel)
	{
		int frequency = frequencyModel.Frequency;
		writer.WriteStartElement(ScheduleXmlConstants.MonthlyElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.TypeAttributeName, ScheduleXmlConstants.AbsoluteDayElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.EveryAttributeName, XmlConvert.ToString(frequency));
		writer.WriteStartElement(ScheduleXmlConstants.AbsoluteDayElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.DayAttributeName, frequencyModel.Parameters["AbsoluteDay"].Value);
		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static void WriteWeeklyElement(XmlTextWriter writer, FrequencyModel frequencyModel)
	{
		IEnumerable<string> source = frequencyModel.Parameters["DayOfWeekIsOneOf"].Values.Select((string x) => x.ToLowerInvariant());
		writer.WriteStartElement(ScheduleXmlConstants.WeeklyElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.EveryAttributeName, XmlConvert.ToString(frequencyModel.Frequency));
		foreach (object value in Enum.GetValues(typeof(DayOfWeek)))
		{
			writer.WriteAttributeString(value.ToString().ToLowerInvariant(), source.Contains(value.ToString().ToLowerInvariant()).ToString().ToLowerInvariant());
		}
		writer.WriteEndElement();
	}

	private static void WriteDailyElement(XmlTextWriter writer, FrequencyModel frequencyModel)
	{
		writer.WriteStartElement(ScheduleXmlConstants.DailyElementName);
		writer.WriteAttributeString(ScheduleXmlConstants.EveryAttributeName, XmlConvert.ToString(frequencyModel.Frequency));
		writer.WriteEndElement();
	}

	private static string GetFrequencyTypeText(FrequencyType frequencyType)
	{
		switch (frequencyType)
		{
		case FrequencyType.Daily:
			return ScheduleXmlConstants.DailyElementName;
		case FrequencyType.Weekly:
			return ScheduleXmlConstants.WeeklyElementName;
		case FrequencyType.Monthly:
		case FrequencyType.MonthlyRelative:
			return ScheduleXmlConstants.MonthlyElementName;
		default:
			return ScheduleXmlConstants.UnknownElementName;
		}
	}

	private static string FilterSources(IEnumerable<EventSource> eventSources)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("Sources");
			xmlTextWriter.WriteStartElement("SourceList");
			foreach (EventSource item in eventSources.Where((EventSource x) => !x.IsGroup))
			{
				xmlTextWriter.WriteStartElement("SourceId");
				xmlTextWriter.WriteRaw(item.Id.ToString());
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("GroupList");
			foreach (EventSource item2 in eventSources.Where((EventSource x) => x.IsGroup))
			{
				xmlTextWriter.WriteStartElement("GroupId");
				xmlTextWriter.WriteRaw(item2.Id.ToString());
				xmlTextWriter.WriteEndElement();
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("SiteSourceList");
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.Close();
			stringWriter = null;
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}
}
