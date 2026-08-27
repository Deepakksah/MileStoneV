using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.LogServer.Communication.Legacy.Registration.Data;
using VideoOS.LogServer.Communication.Legacy.Registration.Data.Serialization;
using VideoOS.LogServer.Communication.LogWriterServiceReference;

namespace VideoOS.LogServer.Communication.Legacy.Registration;

public static class LegacyMessageXmlParser
{
	private class LegacyLogMessageEnumComparer : EqualityComparer<LegacyLogMessageEnumEntry>
	{
		public override bool Equals(LegacyLogMessageEnumEntry s1, LegacyLogMessageEnumEntry s2)
		{
			return string.Equals(s1?.Id, s2?.Id, StringComparison.CurrentCultureIgnoreCase);
		}

		public override int GetHashCode(LegacyLogMessageEnumEntry s)
		{
			return (s?.Id?.GetHashCode()).GetValueOrDefault();
		}
	}

	private static readonly Dictionary<string, LogType> _supportedLogTypesMapping = new Dictionary<string, LogType>
	{
		{
			"Audit",
			LogType.Audit
		},
		{
			"System",
			LogType.System
		},
		{
			"Rule",
			LogType.Rule
		},
		{
			"Rules",
			LogType.Rule
		}
	};

	public static LegacyMessageXml ExtractMessagesFromXml(Stream xmlStream)
	{
		XmlDocument xmlDocument = new XmlDocument();
		try
		{
			xmlDocument.Load(xmlStream);
			return ExtractMessagesFromDocument(xmlDocument);
		}
		catch (Exception ex)
		{
			throw new LegacyMessageXmlParserException(ex.Message, ex);
		}
	}

	private static LegacyMessageXml ExtractMessagesFromDocument(XmlDocument xmlDocument)
	{
		XmlNode xmlNode = xmlDocument.SelectSingleNode("LogMessages");
		LegacyMessageXml legacyMessageXml = new LegacyMessageXml();
		if (xmlNode != null && xmlNode.HasChildNodes)
		{
			legacyMessageXml.Version = xmlNode.SelectSingleNode("version")?.InnerText ?? string.Empty;
			legacyMessageXml.Culture = xmlNode.SelectSingleNode("culture")?.InnerText ?? string.Empty;
			legacyMessageXml.Categories = ExtractCategoriesFromXml(xmlNode);
			legacyMessageXml.SourceTypes = ExtractSourceTypesFromXml(xmlNode);
			Console.WriteLine(legacyMessageXml.Culture);
			legacyMessageXml.Entries = ExtractMessagesFromGroup(xmlNode.SelectNodes("//group"));
		}
		return legacyMessageXml;
	}

	private static IEnumerable<LegacyLogMessageEnumEntry> ExtractCategoriesFromXml(XmlNode root)
	{
		foreach (XmlNode item in root.SelectNodes("//userEnum"))
		{
			if (!string.Equals(item.Attributes["uname"]?.Value, "Category", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LegacyLogMessageUserEnumEntry));
			using TextReader textReader = new StringReader(item.OuterXml);
			return ((LegacyLogMessageUserEnumEntry)xmlSerializer.Deserialize(textReader)).LegacyLogMessageEnumEntries.Where((LegacyLogMessageEnumEntry x) => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Value)).Distinct(new LegacyLogMessageEnumComparer());
		}
		return new List<LegacyLogMessageEnumEntry>();
	}

	private static IEnumerable<LegacyLogMessageEnumEntry> ExtractSourceTypesFromXml(XmlNode root)
	{
		XmlNodeList xmlNodeList = root.SelectNodes("//userEnum");
		List<LegacyLogMessageEnumEntry> result = new List<LegacyLogMessageEnumEntry>();
		foreach (XmlNode item in xmlNodeList)
		{
			string a = item.Attributes["uname"]?.Value;
			if (!string.Equals(a, "RESOURCETYPE", StringComparison.OrdinalIgnoreCase) && !string.Equals(a, "SOURCETYPE", StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(LegacyLogMessageUserEnumEntry));
			using TextReader textReader = new StringReader(item.OuterXml);
			((LegacyLogMessageUserEnumEntry)xmlSerializer.Deserialize(textReader)).LegacyLogMessageEnumEntries.Where((LegacyLogMessageEnumEntry x) => !string.IsNullOrEmpty(x.Id) && !string.IsNullOrEmpty(x.Value)).ToList().ForEach(delegate(LegacyLogMessageEnumEntry l)
			{
				result.Add(l);
			});
		}
		return result.Distinct(new LegacyLogMessageEnumComparer());
	}

	private static IEnumerable<Entry> ExtractMessagesFromGroup(XmlNodeList nodeList)
	{
		XmlSerializer serializer = new XmlSerializer(typeof(Group));
		foreach (XmlNode node in nodeList)
		{
			using TextReader sr = new StringReader(node.OuterXml);
			Group obj = (Group)serializer.Deserialize(sr);
			string logType = obj.Name ?? string.Empty;
			string groupCategory = obj.Category ?? string.Empty;
			SetEntrySourceTypes(node, obj.Entries);
			if (!_supportedLogTypesMapping.ContainsKey(logType))
			{
				continue;
			}
			foreach (Entry entry in obj.Entries)
			{
				entry.LogType = _supportedLogTypesMapping[logType];
				entry.Category = groupCategory;
				yield return entry;
			}
		}
	}

	private static void SetEntrySourceTypes(XmlNode groupXmlNode, IEnumerable<Entry> entries)
	{
		string defaultValue = LookupSourceTypeFromXmlNode(groupXmlNode);
		Dictionary<string, XmlNode> dictionary = new Dictionary<string, XmlNode>();
		foreach (XmlNode childNode in groupXmlNode.ChildNodes)
		{
			string text = childNode?.Attributes?.GetNamedItem("uname")?.Value;
			if (!string.IsNullOrEmpty(text))
			{
				dictionary.Add(text, childNode);
			}
		}
		foreach (Entry entry in entries)
		{
			entry.SourceType = LookupSourceTypeFromXmlNode(GetXmlElementForEntry(entry, dictionary), defaultValue);
		}
	}

	private static XmlNode GetXmlElementForEntry(Entry entry, Dictionary<string, XmlNode> childNodes)
	{
		if (!string.IsNullOrEmpty(entry.UName) && childNodes.TryGetValue(entry.UName, out var value))
		{
			return value;
		}
		return null;
	}

	private static string LookupSourceTypeFromXmlNode(XmlNode xmlNode, string defaultValue = "")
	{
		if (xmlNode == null)
		{
			return defaultValue;
		}
		string text = "field";
		string newValue = "value";
		string text2 = string.Empty;
		string text3 = string.Empty;
		foreach (XmlAttribute attribute in xmlNode.Attributes)
		{
			if (string.IsNullOrEmpty(attribute.Name) || !attribute.Name.StartsWith(text, StringComparison.OrdinalIgnoreCase))
			{
				continue;
			}
			string text4 = attribute.Name.Replace(text, newValue);
			if (!string.IsNullOrEmpty(text4))
			{
				if (string.Equals(attribute.Value, "SourceType", StringComparison.OrdinalIgnoreCase))
				{
					text2 = xmlNode.Attributes.GetNamedItem(text4)?.Value;
				}
				else if (string.Equals(attribute.Value, "ResourceType", StringComparison.OrdinalIgnoreCase))
				{
					text3 = xmlNode.Attributes.GetNamedItem(text4)?.Value;
				}
			}
		}
		if (!string.IsNullOrEmpty(text2))
		{
			return text2;
		}
		if (!string.IsNullOrEmpty(text3))
		{
			return text3;
		}
		return defaultValue;
	}
}
