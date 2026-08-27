using System;
using System.Collections;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Rules.Adapters.XmlHelper;

public static class RuleXmlHelper
{
	public static string MakeTypeXml(string ruleType, string eventType, string sources, string timerValue, string schedule)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using (StringWriter w = new StringWriter(stringBuilder, CultureInfo.InvariantCulture))
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(w);
			xmlTextWriter.WriteStartElement("RuleType");
			xmlTextWriter.WriteAttributeString("type", ruleType);
			if (!string.IsNullOrEmpty(eventType))
			{
				xmlTextWriter.WriteElementString("EventType", eventType);
			}
			if (!string.IsNullOrEmpty(schedule))
			{
				xmlTextWriter.WriteRaw(schedule);
			}
			if (!string.IsNullOrEmpty(sources))
			{
				SourcesHelper sourcesHelper = SourceXmlToLists(sources);
				xmlTextWriter.WriteStartElement("Sources");
				xmlTextWriter.WriteStartElement("SourceList");
				string[] sourceList = sourcesHelper.SourceList;
				foreach (string value in sourceList)
				{
					xmlTextWriter.WriteElementString("SourceId", value);
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteStartElement("GroupList");
				sourceList = sourcesHelper.GroupList;
				foreach (string value2 in sourceList)
				{
					xmlTextWriter.WriteElementString("GroupId", value2);
				}
				xmlTextWriter.WriteEndElement();
				xmlTextWriter.WriteEndElement();
			}
			if (!string.IsNullOrEmpty(timerValue))
			{
				xmlTextWriter.WriteElementString("TimerValue", timerValue);
			}
			xmlTextWriter.WriteEndElement();
		}
		return stringBuilder.ToString();
	}

	public static TypeHelper SeparateTypeXml(string xml)
	{
		string ruleType = string.Empty;
		string eventType = string.Empty;
		string sources = string.Empty;
		string timerValue = string.Empty;
		string scheduleValue = string.Empty;
		using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)))
		{
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element)
				{
					switch (xmlTextReader.Name)
					{
					case "RuleType":
						ruleType = xmlTextReader.GetAttribute("type");
						break;
					case "EventType":
						eventType = xmlTextReader.ReadString();
						break;
					case "Sources":
						sources = xmlTextReader.ReadOuterXml();
						break;
					case "TimerValue":
						timerValue = xmlTextReader.ReadString();
						break;
					case "Schedule":
						scheduleValue = xmlTextReader.ReadOuterXml();
						break;
					}
				}
			}
		}
		return new TypeHelper(ruleType, eventType, sources, timerValue, scheduleValue);
	}

	public static SourcesHelper SourceXmlToLists(string xmlSource)
	{
		ArrayList arrayList = new ArrayList();
		ArrayList arrayList2 = new ArrayList();
		ArrayList arrayList3 = new ArrayList();
		using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xmlSource)))
		{
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element)
				{
					switch (xmlTextReader.Name)
					{
					case "SourceId":
						arrayList.Add(xmlTextReader.ReadString());
						break;
					case "GroupId":
						arrayList2.Add(xmlTextReader.ReadString());
						break;
					case "SiteSourceId":
						arrayList3.Add(xmlTextReader.ReadString());
						break;
					}
				}
			}
		}
		return new SourcesHelper((string[])arrayList.ToArray(typeof(string)), (string[])arrayList2.ToArray(typeof(string)), (string[])arrayList3.ToArray(typeof(string)));
	}

	public static string SourceListToXml(SourcesHelper sourcesHelper)
	{
		if (sourcesHelper == null)
		{
			throw new ArgumentNullException("sourcesHelper");
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Sources");
		xmlTextWriter.WriteStartElement("SourceList");
		string[] sourceList = sourcesHelper.SourceList;
		foreach (string value in sourceList)
		{
			xmlTextWriter.WriteElementString("SourceId", value);
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("GroupList");
		sourceList = sourcesHelper.GroupList;
		foreach (string value2 in sourceList)
		{
			xmlTextWriter.WriteElementString("GroupId", value2);
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteStartElement("SiteSourceList");
		sourceList = sourcesHelper.SiteSourceList;
		foreach (string value3 in sourceList)
		{
			xmlTextWriter.WriteElementString("SiteSourceId", value3);
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static bool SourceIsTriggeringDevice(string xmlSource)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(xmlSource);
		bool flag = sourcesHelper.SourceList.Length == 1 && sourcesHelper.GroupList.Length == 0;
		if (flag)
		{
			flag = sourcesHelper.SourceList[0] == SpecialActionTargets.TriggeringDevice.ToString();
		}
		return flag;
	}

	public static bool SourceIsDevicesFromMetadata(string xmlSource)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(xmlSource);
		bool flag = sourcesHelper.SourceList.Length == 1 && sourcesHelper.GroupList.Length == 0;
		if (flag)
		{
			flag = sourcesHelper.SourceList[0] == SpecialActionTargets.DevicesFromMetaData.ToString();
		}
		return flag;
	}

	public static bool SourceIsNoneDevice(string xmlSource)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(xmlSource);
		bool flag = sourcesHelper.SourceList.Length == 1 && sourcesHelper.GroupList.Length == 0;
		if (flag)
		{
			flag = sourcesHelper.SourceList[0] == SpecialActionTargets.NoneDevice.ToString();
		}
		return flag;
	}

	public static string TriggeringSourceListXml()
	{
		return SourceListToXml(new SourcesHelper(new string[1] { SpecialActionTargets.TriggeringDevice.ToString() }, Array.Empty<string>()));
	}

	public static string NoneDeviceSourceListXml()
	{
		return SourceListToXml(new SourcesHelper(new string[1] { SpecialActionTargets.NoneDevice.ToString() }, Array.Empty<string>()));
	}

	public static string DevicesFromMetaDataSourceListXml()
	{
		return SourceListToXml(new SourcesHelper(new string[1] { SpecialActionTargets.DevicesFromMetaData.ToString() }, Array.Empty<string>()));
	}

	public static string EmptySourceListXml()
	{
		return SourceListToXml(new SourcesHelper(Array.Empty<string>(), Array.Empty<string>()));
	}

	public static void SourceSplitToCommaLists(string xmlSource, ref string sourceList, ref string groupList)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(xmlSource);
		sourceList = string.Empty;
		for (int i = 0; i < sourcesHelper.SourceList.Length; i++)
		{
			if (i == 0)
			{
				sourceList = sourcesHelper.SourceList[i];
			}
			else
			{
				sourceList = sourceList + "," + sourcesHelper.SourceList[i];
			}
		}
		groupList = string.Empty;
		for (int j = 0; j < sourcesHelper.GroupList.Length; j++)
		{
			if (j == 0)
			{
				groupList = sourcesHelper.GroupList[j];
			}
			else
			{
				groupList = groupList + "," + sourcesHelper.GroupList[j];
			}
		}
	}

	public static void SourceSplitToCommaLists(string xmlSource, ref string sourceList, ref string groupList, ref string siteDeviceList)
	{
		SourceSplitToCommaLists(xmlSource, ref sourceList, ref groupList);
		SourcesHelper sourcesHelper = SourceXmlToLists(xmlSource);
		siteDeviceList = string.Empty;
		for (int i = 0; i < sourcesHelper.SiteSourceList.Length; i++)
		{
			if (i == 0)
			{
				siteDeviceList = sourcesHelper.SiteSourceList[i];
			}
			else
			{
				siteDeviceList = siteDeviceList + "," + sourcesHelper.SiteSourceList[i];
			}
		}
	}

	public static string SourceCommaListsToSourceXml(string sourceList, string groupList)
	{
		if (sourceList == null)
		{
			throw new ArgumentNullException("sourceList");
		}
		if (groupList == null)
		{
			throw new ArgumentNullException("groupList");
		}
		return SourceCommaListsToSourceXml(sourceList, groupList, string.Empty);
	}

	public static string SourceCommaListsToSourceXml(string sourceList, string groupList, string siteDeviceList)
	{
		if (sourceList == null)
		{
			throw new ArgumentNullException("sourceList");
		}
		if (groupList == null)
		{
			throw new ArgumentNullException("groupList");
		}
		if (siteDeviceList == null)
		{
			throw new ArgumentNullException("siteDeviceList");
		}
		string[] sourceList2 = Array.Empty<string>();
		if (sourceList.Length > 0)
		{
			sourceList2 = sourceList.Split(',');
		}
		string[] groupList2 = Array.Empty<string>();
		if (groupList.Length > 0)
		{
			groupList2 = groupList.Split(',');
		}
		string[] siteSourceList = Array.Empty<string>();
		if (siteDeviceList.Length > 0)
		{
			siteSourceList = siteDeviceList.Split(',');
		}
		return SourceListToXml(new SourcesHelper(sourceList2, groupList2, siteSourceList));
	}

	public static bool SourceXmlIsIdentical(string sources1, string sources2)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(sources1);
		SourcesHelper sourcesHelper2 = SourceXmlToLists(sources2);
		return SourcesHelper.CheckIdentical(sourcesHelper, sourcesHelper2);
	}

	public static bool SourceXmlIsAllDifferent(string sources1, string sources2)
	{
		SourcesHelper sourcesHelper = SourceXmlToLists(sources1);
		SourcesHelper sourcesHelper2 = SourceXmlToLists(sources2);
		return SourcesHelper.CheckAllDifferent(sourcesHelper, sourcesHelper2);
	}

	public static string GetOperatorToken(string operatorType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Operator");
		xmlTextWriter.WriteString(operatorType);
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static string GetParenthesisLeftToken()
	{
		return GetParenthesisToken(leftParenthesis: true);
	}

	public static string GetParenthesisRightToken()
	{
		return GetParenthesisToken(leftParenthesis: false);
	}

	private static string GetParenthesisToken(bool leftParenthesis)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Parenthesis");
		if (leftParenthesis)
		{
			xmlTextWriter.WriteString("(");
		}
		else
		{
			xmlTextWriter.WriteString(")");
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static string GetValueToken(string parameterValue, string dataType)
	{
		StringBuilder stringBuilder = new StringBuilder();
		using XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Value");
		xmlTextWriter.WriteAttributeString("datatype", dataType);
		xmlTextWriter.WriteString(parameterValue);
		xmlTextWriter.WriteEndElement();
		return stringBuilder.ToString();
	}

	public static string MakeConditionXml(string conditionName, string[] parameterValues, string[] dataType)
	{
		if (parameterValues == null)
		{
			throw new ArgumentNullException("parameterValues");
		}
		if (dataType == null)
		{
			throw new ArgumentNullException("dataType");
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Function");
		xmlTextWriter.WriteAttributeString("name", conditionName);
		for (int i = 0; i < parameterValues.Length; i++)
		{
			xmlTextWriter.WriteStartElement("Token");
			xmlTextWriter.WriteAttributeString("type", "Value");
			xmlTextWriter.WriteAttributeString("datatype", dataType[i]);
			xmlTextWriter.WriteString(parameterValues[i]);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static string MakeConditionWithArrayXml(string conditionName, string[] parameterValues, string arrayType, string elementType)
	{
		if (parameterValues == null)
		{
			throw new ArgumentNullException("parameterValues");
		}
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Function");
		xmlTextWriter.WriteAttributeString("name", conditionName);
		xmlTextWriter.WriteStartElement("Token");
		xmlTextWriter.WriteAttributeString("type", "Value");
		xmlTextWriter.WriteAttributeString("datatype", arrayType);
		for (int i = 0; i < parameterValues.Length; i++)
		{
			xmlTextWriter.WriteStartElement("Value");
			xmlTextWriter.WriteAttributeString("datatype", elementType);
			xmlTextWriter.WriteString(parameterValues[i]);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static string[] ConditionXmlToTokenValueArray(string xml)
	{
		ArrayList arrayList = new ArrayList();
		using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)))
		{
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "Token" && xmlTextReader.GetAttribute("type") == "Value")
				{
					arrayList.Add(xmlTextReader.ReadString());
				}
			}
		}
		return (string[])arrayList.ToArray(typeof(string));
	}

	public static string[] ConditionArrayXmlToValueArray(string xml)
	{
		ArrayList arrayList = new ArrayList();
		using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)))
		{
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "Value")
				{
					arrayList.Add(xmlTextReader.ReadString());
				}
			}
		}
		return (string[])arrayList.ToArray(typeof(string));
	}

	public static string MakeActionXml(string actionName, string[] actionParameterNames, string[] actionParameterValues)
	{
		if (actionName == null)
		{
			throw new ArgumentNullException("actionName");
		}
		if (actionParameterNames == null)
		{
			throw new ArgumentNullException("actionParameterNames");
		}
		if (actionParameterValues == null)
		{
			throw new ArgumentNullException("actionParameterValues");
		}
		Guid guid = Guid.NewGuid();
		StringBuilder stringBuilder = new StringBuilder();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(new StringWriter(stringBuilder, CultureInfo.InvariantCulture));
		xmlTextWriter.WriteStartElement("Action");
		xmlTextWriter.WriteAttributeString("name", actionName);
		xmlTextWriter.WriteAttributeString("instanceId", guid.ToString());
		for (int i = 0; i < actionParameterNames.Length; i++)
		{
			xmlTextWriter.WriteStartElement("Parameter");
			xmlTextWriter.WriteAttributeString("name", actionParameterNames[i]);
			xmlTextWriter.WriteString(actionParameterValues[i]);
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
		xmlTextWriter.Close();
		return stringBuilder.ToString();
	}

	public static ActionParameters ActionXmlDecodeParameters(string xml)
	{
		Hashtable hashtable = new Hashtable();
		using (XmlTextReader xmlTextReader = new XmlTextReader(new StringReader(xml)))
		{
			while (xmlTextReader.Read())
			{
				if (xmlTextReader.NodeType == XmlNodeType.Element && xmlTextReader.Name == "Parameter")
				{
					string attribute = xmlTextReader.GetAttribute("name");
					string value = xmlTextReader.ReadString();
					hashtable.Add(attribute, value);
				}
			}
		}
		return new ActionParameters(hashtable);
	}

	public static DateTimeRange TimesRangeToDateTimes(string timeRange)
	{
		if (timeRange == null)
		{
			throw new ArgumentNullException("timeRange");
		}
		string[] array = timeRange.Split(',');
		return new DateTimeRange
		{
			StartTime = DateTime.Parse(array[0], CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
			EndTime = DateTime.Parse(array[1], CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal)
		};
	}

	public static string DateTimeToTimeRange(DateTime startTime, DateTime endTime)
	{
		return startTime.ToString("t", CultureInfo.InvariantCulture) + "," + endTime.ToString("t", CultureInfo.InvariantCulture);
	}

	public static bool SelectionIsTimeRangeFromMetadata(string selection)
	{
		return string.Equals("UseMetadata", selection, StringComparison.Ordinal);
	}

	public static bool SelectionIsFileFromMetadata(string selection)
	{
		return string.Equals("11111111-1111-1111-1111-111111111111", selection, StringComparison.Ordinal);
	}
}
