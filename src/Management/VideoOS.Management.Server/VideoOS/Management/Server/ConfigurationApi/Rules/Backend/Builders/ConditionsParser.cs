using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class ConditionsParser : IConditionsParser
{
	private static readonly Dictionary<string, List<ConditionParameter>> _conditionParameters = new Dictionary<string, List<ConditionParameter>>
	{
		{
			"VideoOS.WithinTimeProfile",
			new List<ConditionParameter>
			{
				new ConditionParameter
				{
					ParameterName = "WithinTimeProfile",
					ConditionType = ConditionType.WithinTimeProfile
				}
			}
		},
		{
			"VideoOS.NotWithinTimeProfile",
			new List<ConditionParameter>
			{
				new ConditionParameter
				{
					ParameterName = "OutsideTimeProfile",
					ConditionType = ConditionType.OutsideTimeProfile
				}
			}
		},
		{
			"VideoOS.TimeOfDayBetween",
			new List<ConditionParameter>
			{
				new ConditionParameter
				{
					ParameterName = "StartTime",
					ConditionType = ConditionType.TimeBetween
				},
				new ConditionParameter
				{
					ParameterName = "EndTime",
					ConditionType = ConditionType.TimeBetween
				}
			}
		},
		{
			"VideoOS.WeekdayIsOneOf",
			new List<ConditionParameter>
			{
				new ConditionParameter
				{
					ParameterName = "DayOfWeekIsOneOf",
					ConditionType = ConditionType.DaysOfWeek
				}
			}
		},
		{
			"VideoOS.GetMetaData",
			new List<ConditionParameter>
			{
				new ConditionParameter
				{
					ParameterName = "EventIsWithinWindow",
					ConditionType = ConditionType.WithinWindow
				}
			}
		},
		{
			"VideoOS.IsFailover",
			new List<ConditionParameter>()
		},
		{
			"VideoOS.IsNotFailover",
			new List<ConditionParameter>()
		}
	};

	public bool TryParseConditions(XmlNode conditionsNode, out List<ConditionModel> conditions)
	{
		conditions = new List<ConditionModel>();
		foreach (XmlNode childNode in conditionsNode.GetNodeByTagName("Condition").ChildNodes)
		{
			if (TryParseCondition(childNode, out var condition))
			{
				if (condition != null)
				{
					conditions.Add(condition);
				}
				continue;
			}
			return false;
		}
		foreach (XmlNode childNode2 in conditionsNode.GetNodeByTagName("TimeCondition").ChildNodes)
		{
			if (TryParseCondition(childNode2, out var condition2))
			{
				if (condition2 != null)
				{
					conditions.Add(condition2);
				}
				continue;
			}
			return false;
		}
		return true;
	}

	private static bool TryParseCondition(XmlNode node, out ConditionModel condition)
	{
		condition = null;
		if (string.IsNullOrEmpty(node.Name))
		{
			return false;
		}
		if (node.Name.Equals("Token", StringComparison.OrdinalIgnoreCase))
		{
			return TryParseTokenElement(node, out condition);
		}
		if (node.Name.Equals("Region", StringComparison.OrdinalIgnoreCase))
		{
			return TryParseRegionElement(node, out condition);
		}
		return false;
	}

	private static bool TryParseTokenElement(XmlNode node, out ConditionModel condition)
	{
		condition = null;
		if (node.HasAttributeWithValue("type", "Function"))
		{
			if (TryParseFunctionName(node, out var value))
			{
				return TryParseParameters(node, value, out condition);
			}
			return false;
		}
		if (node.HasAttributeWithValue("type", "Operator") && node.InnerXml.Equals("And", StringComparison.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static bool TryParseRegionElement(XmlNode node, out ConditionModel condition)
	{
		condition = null;
		if (node.HasAttributeWithValue("name", "VideoOS.Always"))
		{
			condition = new ConditionModel
			{
				ConditionType = ConditionType.Always
			};
			return true;
		}
		if (node.HasAttributeWithValue("name", "VideoOS.MotionEventIsFromWindow") && node.HasChildNodes)
		{
			return TryParseParenthesis(node.ChildNodes[0], out condition);
		}
		return false;
	}

	private static bool TryParseParenthesis(XmlNode node, out ConditionModel condition)
	{
		condition = null;
		if (!node.HasAttributeWithValue("type", "Parenthesis") || node.InnerText != "(")
		{
			return false;
		}
		XmlNode nextSibling = node.NextSibling;
		if (!TryParseFunctionName(nextSibling, out var value) || !TryParseParameters(nextSibling, value, out condition) || !condition.Parameters.ContainsKey(_conditionParameters[value].First().ParameterName))
		{
			return false;
		}
		nextSibling = nextSibling.NextSibling;
		if (!nextSibling.HasAttributeWithValue("type", "Operator") || !nextSibling.InnerXml.Equals("Equals", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		nextSibling = nextSibling.NextSibling;
		if (!nextSibling.Name.Equals("Token", StringComparison.OrdinalIgnoreCase))
		{
			return false;
		}
		if (!nextSibling.HasAttribute<string>("datatype", out var attributeValue) || !TryParseParameterValues(nextSibling, attributeValue, out var values))
		{
			return false;
		}
		condition.Parameters[_conditionParameters[value][0].ParameterName] = new RuleParameter
		{
			Name = _conditionParameters[value][0].ParameterName,
			Values = values
		};
		nextSibling = nextSibling.NextSibling;
		if (nextSibling.HasAttributeWithValue("type", "Parenthesis"))
		{
			return nextSibling.InnerText == ")";
		}
		return false;
	}

	private static bool TryParseParameters(XmlNode xmlNode, string functionName, out ConditionModel condition)
	{
		condition = null;
		Dictionary<string, RuleParameter> dictionary = new Dictionary<string, RuleParameter>();
		int num = 0;
		foreach (XmlNode childNode in xmlNode.ChildNodes)
		{
			if (childNode != null && childNode.HasAttributeWithValue("type", "Value") && childNode.HasAttribute<string>("datatype", out var attributeValue) && TryParseParameterValues(childNode, attributeValue, out var values))
			{
				dictionary[_conditionParameters[functionName][num].ParameterName] = new RuleParameter
				{
					Name = _conditionParameters[functionName][num].ParameterName,
					Values = values
				};
				num++;
			}
		}
		ConditionType conditionType;
		if (!dictionary.Any())
		{
			if (functionName == "VideoOS.IsFailover")
			{
				conditionType = ConditionType.FailoverActive;
			}
			else
			{
				if (!(functionName == "VideoOS.IsNotFailover"))
				{
					return false;
				}
				conditionType = ConditionType.FailoverInactive;
			}
		}
		else
		{
			conditionType = _conditionParameters[functionName][0].ConditionType;
		}
		condition = new ConditionModel
		{
			ConditionType = conditionType,
			Parameters = dictionary
		};
		return true;
	}

	private static bool TryParseFunctionName(XmlNode node, out string value)
	{
		value = string.Empty;
		if (!node.HasAttribute<string>("name", out value))
		{
			return false;
		}
		if (_conditionParameters.Keys.Contains(value, StringComparer.OrdinalIgnoreCase))
		{
			return true;
		}
		return false;
	}

	private static bool TryParseParameterValues(XmlNode xmlNode, string dataType, out string[] values)
	{
		values = null;
		if (string.IsNullOrEmpty(xmlNode.InnerText))
		{
			return false;
		}
		if (dataType.Equals("String", StringComparison.Ordinal))
		{
			values = (from s in xmlNode.InnerText.Split(',')
				select s.Trim()).ToArray();
			return true;
		}
		if (dataType.Equals("StringArray", StringComparison.Ordinal))
		{
			List<string> list = new List<string>();
			foreach (XmlNode childNode in xmlNode.ChildNodes)
			{
				if (childNode.HasAttributeWithValue("datatype", "String"))
				{
					list.Add(childNode.InnerXml);
				}
			}
			values = list.ToArray();
			return true;
		}
		return false;
	}
}
