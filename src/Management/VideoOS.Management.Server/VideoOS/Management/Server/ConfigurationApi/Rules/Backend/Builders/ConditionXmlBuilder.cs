using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;
using VideoOS.Rules.Adapters.XmlHelper;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Builders;

public class ConditionXmlBuilder : IConditionXmlBuilder
{
	private static readonly Dictionary<ConditionType, string> _conditionTypeToFunctionName = new Dictionary<ConditionType, string>
	{
		{
			ConditionType.WithinTimeProfile,
			"VideoOS.WithinTimeProfile"
		},
		{
			ConditionType.OutsideTimeProfile,
			"VideoOS.NotWithinTimeProfile"
		},
		{
			ConditionType.TimeBetween,
			"VideoOS.TimeOfDayBetween"
		},
		{
			ConditionType.DaysOfWeek,
			"VideoOS.WeekdayIsOneOf"
		},
		{
			ConditionType.WithinWindow,
			"VideoOS.MotionEventIsFromWindow"
		},
		{
			ConditionType.FailoverActive,
			"VideoOS.IsFailover"
		},
		{
			ConditionType.FailoverInactive,
			"VideoOS.IsNotFailover"
		}
	};

	private static readonly Dictionary<ConditionType, string[]> _conditionTypeToParameters = new Dictionary<ConditionType, string[]>
	{
		{
			ConditionType.WithinTimeProfile,
			new string[1] { "WithinTimeProfile" }
		},
		{
			ConditionType.OutsideTimeProfile,
			new string[1] { "OutsideTimeProfile" }
		},
		{
			ConditionType.TimeBetween,
			new string[2] { "StartTime", "EndTime" }
		},
		{
			ConditionType.DaysOfWeek,
			new string[1] { "DayOfWeekIsOneOf" }
		},
		{
			ConditionType.WithinWindow,
			new string[1] { "EventIsWithinWindow" }
		},
		{
			ConditionType.FailoverActive,
			new string[0]
		},
		{
			ConditionType.FailoverInactive,
			new string[0]
		}
	};

	public string GetXml(IDictionary<ConditionType, ConditionModel> conditions)
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = null;
		try
		{
			stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
			XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			xmlTextWriter.WriteStartElement("Conditions");
			xmlTextWriter.WriteStartElement("TimeCondition");
			if (conditions != null)
			{
				if (conditions.Any((KeyValuePair<ConditionType, ConditionModel> c) => c.Key == ConditionType.Always))
				{
					BuildAlwaysXml(xmlTextWriter);
				}
				List<KeyValuePair<ConditionType, ConditionModel>> list = conditions.Where((KeyValuePair<ConditionType, ConditionModel> c) => c.Key != ConditionType.Always && c.Key != ConditionType.WithinWindow).ToList();
				foreach (KeyValuePair<ConditionType, ConditionModel> item in list)
				{
					BuildTimeConditionXml(item.Value, xmlTextWriter);
					if (!item.Equals(list.Last()))
					{
						AddAndToken(xmlTextWriter);
					}
				}
			}
			xmlTextWriter.WriteEndElement();
			xmlTextWriter.WriteStartElement("Condition");
			if (conditions != null)
			{
				KeyValuePair<ConditionType, ConditionModel> keyValuePair = conditions.FirstOrDefault((KeyValuePair<ConditionType, ConditionModel> c) => c.Key == ConditionType.WithinWindow);
				if (!keyValuePair.Equals(default(KeyValuePair<ConditionType, ConditionModel>)))
				{
					BuildWithinWindowConditionXml(keyValuePair.Value, xmlTextWriter);
				}
			}
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

	private static void BuildWithinWindowConditionXml(ConditionModel condition, XmlTextWriter wr)
	{
		wr.WriteStartElement("Region");
		wr.WriteAttributeString("name", "VideoOS.MotionEventIsFromWindow");
		wr.WriteRaw(RuleXmlHelper.GetParenthesisLeftToken());
		wr.WriteRaw(RuleXmlHelper.MakeConditionXml("VideoOS.GetMetaData", new string[1] { "EventMotionWindow" }, new string[1] { "String" }));
		wr.WriteRaw(RuleXmlHelper.GetOperatorToken("Equals"));
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Value");
		wr.WriteAttributeString("datatype", "String");
		wr.WriteString(condition.Parameters.First().Value.Value);
		wr.WriteEndElement();
		wr.WriteRaw(RuleXmlHelper.GetParenthesisRightToken());
		wr.WriteEndElement();
	}

	private static void BuildTimeConditionXml(ConditionModel condition, XmlTextWriter wr)
	{
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Function");
		wr.WriteAttributeString("name", _conditionTypeToFunctionName[condition.ConditionType]);
		string[] array = _conditionTypeToParameters[condition.ConditionType];
		foreach (string key in array)
		{
			if (condition.ConditionType == ConditionType.DaysOfWeek)
			{
				AddStringArrayValueToken(wr, condition.Parameters[key].Values);
			}
			else
			{
				AddStringValueToken(wr, condition.Parameters[key].Value);
			}
		}
		if (IsTimeProfileCondition(condition.ConditionType))
		{
			AddRuleEngineToken(wr);
		}
		wr.WriteEndElement();
	}

	private static void AddStringValueToken(XmlTextWriter wr, string value)
	{
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Value");
		wr.WriteAttributeString("datatype", "String");
		wr.WriteRaw(value);
		wr.WriteEndElement();
	}

	private static void AddStringArrayValueToken(XmlTextWriter wr, string[] values)
	{
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Value");
		wr.WriteAttributeString("datatype", "StringArray");
		foreach (string data in values)
		{
			wr.WriteStartElement("Value");
			wr.WriteAttributeString("type", "datatype");
			wr.WriteAttributeString("datatype", "String");
			wr.WriteRaw(data);
			wr.WriteEndElement();
		}
		wr.WriteEndElement();
	}

	private static void AddRuleEngineToken(XmlTextWriter wr)
	{
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Value");
		wr.WriteAttributeString("datatype", "Object");
		wr.WriteRaw("RuleEngine");
		wr.WriteEndElement();
	}

	private static void AddAndToken(XmlTextWriter wr)
	{
		wr.WriteStartElement("Token");
		wr.WriteAttributeString("type", "Operator");
		wr.WriteRaw("And");
		wr.WriteEndElement();
	}

	private static bool IsTimeProfileCondition(ConditionType condition)
	{
		if (condition != ConditionType.OutsideTimeProfile)
		{
			return condition == ConditionType.WithinTimeProfile;
		}
		return true;
	}

	private static void BuildAlwaysXml(XmlTextWriter wr)
	{
		wr.WriteStartElement("Region");
		wr.WriteAttributeString("name", "VideoOS.Always");
		wr.WriteRaw(RuleXmlHelper.GetValueToken("True", "Boolean"));
		wr.WriteEndElement();
	}
}
