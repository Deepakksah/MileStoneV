using System;
using System.Collections.Generic;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class ScheduleParser
{
	public static bool TryParse(XmlNode ruleTypeNode, RulePartModel theRule)
	{
		XmlNode nodeByTagName = ruleTypeNode.GetNodeByTagName("Schedule");
		if (nodeByTagName == null)
		{
			return false;
		}
		XmlNode nodeByTagName2 = nodeByTagName.GetNodeByTagName(ScheduleXmlConstants.FrequencyElementName);
		if (nodeByTagName2 == null)
		{
			return false;
		}
		XmlNode nodeByTagName3 = nodeByTagName.GetNodeByTagName(ScheduleXmlConstants.SubDayElementName);
		if (nodeByTagName3 == null)
		{
			return false;
		}
		if (TryParseFrequency(nodeByTagName2, theRule))
		{
			return TryParseSubDay(nodeByTagName3, theRule);
		}
		return false;
	}

	private static bool TryParseFrequency(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.FrequencyElementName, expectedChild: true) || !node.HasAttribute<FrequencyType>("type", out var attributeValue, capitalize: true) || attributeValue == FrequencyType.None)
		{
			return false;
		}
		XmlNode firstChild = node.FirstChild;
		return attributeValue switch
		{
			FrequencyType.Daily => TryParseFrequencyDaily(firstChild, theRule), 
			FrequencyType.Weekly => TryParseFrequencyWeekly(firstChild, theRule), 
			FrequencyType.Monthly => TryParseFrequencyMonthly(firstChild, theRule), 
			_ => false, 
		};
	}

	private static bool TryParseFrequencyDaily(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.DailyElementName) || !node.HasAttribute<int>(ScheduleXmlConstants.EveryAttributeName, out var attributeValue))
		{
			return false;
		}
		theRule.RecurringTime = new RecurringTimeModel();
		theRule.RecurringTime.Frequency.FrequencyType = FrequencyType.Daily;
		theRule.RecurringTime.Frequency.Frequency = attributeValue;
		return true;
	}

	private static bool TryParseFrequencyWeekly(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.WeeklyElementName) || !node.HasAttribute<int>(ScheduleXmlConstants.EveryAttributeName, out var attributeValue))
		{
			return false;
		}
		List<string> list = new List<string>();
		foreach (object value in Enum.GetValues(typeof(DayOfWeek)))
		{
			if (node.HasAttribute<bool>(value.ToString().ToLowerInvariant(), out var attributeValue2, capitalize: true) && attributeValue2)
			{
				list.Add(value.ToString());
			}
		}
		if (list.Count == 0)
		{
			return false;
		}
		theRule.RecurringTime = new RecurringTimeModel();
		theRule.RecurringTime.Frequency.FrequencyType = FrequencyType.Weekly;
		theRule.RecurringTime.Frequency.Frequency = attributeValue;
		theRule.RecurringTime.Frequency.Parameters.Add("DayOfWeekIsOneOf", new RuleParameter
		{
			Name = "DayOfWeekIsOneOf",
			Values = list.ToArray()
		});
		return true;
	}

	private static bool TryParseFrequencyMonthly(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.MonthlyElementName, expectedChild: true) || !node.HasAttribute<string>(ScheduleXmlConstants.TypeAttributeName, out var attributeValue) || !node.HasAttribute<int>(ScheduleXmlConstants.EveryAttributeName, out var attributeValue2))
		{
			return false;
		}
		XmlNode firstChild = node.FirstChild;
		if (attributeValue.Equals(ScheduleXmlConstants.AbsoluteDayElementName))
		{
			return TryParseFrequencyMonthlyAbsolute(firstChild, theRule, attributeValue2);
		}
		if (attributeValue.Equals(ScheduleXmlConstants.RelativeDayElementName))
		{
			return TryParseFrequencyMonthlyRelative(firstChild, theRule, attributeValue2);
		}
		return false;
	}

	private static bool TryParseFrequencyMonthlyAbsolute(XmlNode node, RulePartModel theRule, int frequency)
	{
		if (!node.IsValid(ScheduleXmlConstants.AbsoluteDayElementName) || !node.HasAttribute<string>(ScheduleXmlConstants.DayAttributeName, out var attributeValue))
		{
			return false;
		}
		theRule.RecurringTime = new RecurringTimeModel();
		theRule.RecurringTime.Frequency.FrequencyType = FrequencyType.Monthly;
		theRule.RecurringTime.Frequency.Frequency = frequency;
		theRule.RecurringTime.Frequency.Parameters.Add("AbsoluteDay", new RuleParameter
		{
			Name = "AbsoluteDay",
			Value = attributeValue
		});
		return true;
	}

	private static bool TryParseFrequencyMonthlyRelative(XmlNode node, RulePartModel theRule, int frequency)
	{
		if (!node.IsValid(ScheduleXmlConstants.RelativeDayElementName) || !node.HasAttribute<string>(ScheduleXmlConstants.OffsetAttributeName, out var attributeValue) || !node.HasAttribute<string>(ScheduleXmlConstants.DayAttributeName, out var attributeValue2))
		{
			return false;
		}
		theRule.RecurringTime = new RecurringTimeModel();
		theRule.RecurringTime.Frequency.FrequencyType = FrequencyType.MonthlyRelative;
		theRule.RecurringTime.Frequency.Frequency = frequency;
		theRule.RecurringTime.Frequency.Parameters.Add("Offset", new RuleParameter
		{
			Name = "Offset",
			Value = attributeValue
		});
		theRule.RecurringTime.Frequency.Parameters.Add("RelativeDay", new RuleParameter
		{
			Name = "RelativeDay",
			Value = attributeValue2
		});
		return true;
	}

	private static bool TryParseSubDay(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.SubDayElementName, expectedChild: true) || !node.HasAttribute<ScheduleTypes.SubdayType>("type", out var attributeValue))
		{
			return false;
		}
		XmlNode firstChild = node.FirstChild;
		return attributeValue switch
		{
			ScheduleTypes.SubdayType.Undefined => false, 
			ScheduleTypes.SubdayType.Once => TryParseSubDayTypeOnce(firstChild, theRule), 
			ScheduleTypes.SubdayType.Recurring => TryParseSubDayTypeRecurring(firstChild, theRule), 
			_ => false, 
		};
	}

	private static bool TryParseSubDayTypeOnce(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.OnceElementName) || !node.HasAttribute<int>(ScheduleXmlConstants.TimeOfDayAttributeName, out var attributeValue))
		{
			return false;
		}
		theRule.RecurringTime.SubDay.SubDayType = ScheduleTypes.SubdayType.Once;
		theRule.RecurringTime.SubDay.StartTimeOfDay = attributeValue;
		return true;
	}

	private static bool TryParseSubDayTypeRecurring(XmlNode node, RulePartModel theRule)
	{
		if (!node.IsValid(ScheduleXmlConstants.RecurringElementName) || !node.HasAttribute<int>(ScheduleXmlConstants.FrequencyAttributeName, out var attributeValue) || !node.HasAttribute<int>(ScheduleXmlConstants.StartTimeOfDayAttributeName, out var attributeValue2) || !node.HasAttribute<int>(ScheduleXmlConstants.EndTimeOfDayAttributeName, out var attributeValue3))
		{
			return false;
		}
		theRule.RecurringTime.SubDay.SubDayType = ScheduleTypes.SubdayType.Recurring;
		theRule.RecurringTime.SubDay.Frequency = attributeValue;
		theRule.RecurringTime.SubDay.StartTimeOfDay = attributeValue2;
		theRule.RecurringTime.SubDay.EndTimeOfDay = attributeValue3;
		return true;
	}
}
