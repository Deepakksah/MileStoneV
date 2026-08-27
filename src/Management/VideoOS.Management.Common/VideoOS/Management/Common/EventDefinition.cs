using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.XPath;

namespace VideoOS.Management.Common;

public class EventDefinition
{
	public static readonly HashSet<Guid> _external_System_events = new HashSet<Guid>
	{
		new Guid("77B1E70D-BA8D-4bb8-9EE8-43B09746D82A"),
		new Guid("85867627-B287-4439-9E55-A63701E1715B"),
		new Guid("7605F8B0-7F5F-4432-B223-0BB2DC3F1F5C")
	};

	public Guid ID { get; set; }

	public string Name { get; set; }

	public string DisplayName { get; set; }

	public string DisplayNameReferenceId { get; set; }

	public string Description { get; set; }

	public string DescriptionReferenceId { get; set; }

	public string GeneratorType { get; set; }

	public string GeneratorSubtype { get; set; }

	public string GeneratorID { get; set; }

	public string GeneratorName { get; set; }

	public string GeneratorGroupId { get; set; }

	public string GeneratorGroupName { get; set; }

	public Guid? StateGroupId { get; set; }

	public string State { get; set; }

	public EventSource[] Sources { get; set; }

	public bool OccursGlobally { get; set; }

	public bool BuiltIn { get; set; }

	public Guid CounterEventID { get; set; }

	public EventDefinition(Guid id, string name, string displayName, string displayNameReferenceId, string description, string descriptionReferenceId, string generatorType, string generatorSubtype, string generatorId, string generatorName, string sourceType, EventSource.FilterTypes filter, bool occursGlobally, bool builtIn, Guid counterEventId)
		: this(id, name, displayName, displayNameReferenceId, description, descriptionReferenceId, generatorType, generatorSubtype, generatorId, generatorName, string.Empty, string.Empty, null, string.Empty, sourceType, filter, occursGlobally, builtIn, counterEventId)
	{
	}

	public EventDefinition(Guid id, string name, string displayName, string displayNameReferenceId, string description, string descriptionReferenceId, string generatorType, string generatorSubtype, string generatorId, string generatorName, string generatorGroupId, string generatorGroupName, Guid? stateGroupId, string state, string sourceType, EventSource.FilterTypes filter, bool occursGlobally, bool builtIn, Guid counterEventId)
	{
		ID = id;
		Name = name;
		DisplayName = displayName;
		DisplayNameReferenceId = displayNameReferenceId;
		Description = description;
		DescriptionReferenceId = descriptionReferenceId;
		GeneratorType = generatorType;
		GeneratorSubtype = generatorSubtype;
		GeneratorID = generatorId;
		GeneratorName = generatorName;
		GeneratorGroupId = generatorGroupId;
		GeneratorGroupName = generatorGroupName;
		StateGroupId = stateGroupId;
		State = state;
		Sources = new EventSource[1]
		{
			new EventSource(sourceType, filter)
		};
		OccursGlobally = occursGlobally;
		BuiltIn = builtIn;
		CounterEventID = counterEventId;
	}

	public EventDefinition(string definitionXml)
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(definitionXml);
		XmlNode xmlNode = xmlDocument.SelectSingleNode("EventType/Id");
		ID = new Guid(xmlNode.InnerText);
		xmlNode = xmlDocument.SelectSingleNode("EventType/Name");
		Name = xmlNode.InnerText;
		XmlNodeList xmlNodeList = xmlDocument.SelectNodes("EventType/DisplayName");
		foreach (XmlNode item in xmlNodeList)
		{
			DisplayName = item.InnerText;
			XmlAttribute xmlAttribute = item.Attributes["referenceid"];
			if (xmlAttribute != null)
			{
				DisplayNameReferenceId = xmlAttribute.InnerText;
			}
		}
		xmlNodeList = xmlDocument.SelectNodes("EventType/Description");
		foreach (XmlNode item2 in xmlNodeList)
		{
			Description = item2.InnerText;
			XmlAttribute xmlAttribute2 = item2.Attributes["referenceid"];
			if (xmlAttribute2 != null)
			{
				DescriptionReferenceId = xmlAttribute2.InnerText;
			}
		}
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/Type");
		GeneratorType = xmlNode.InnerText;
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/Subtype");
		if (xmlNode == null)
		{
			xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/SubType");
		}
		if (xmlNode != null)
		{
			GeneratorSubtype = xmlNode.InnerText;
		}
		else if (_external_System_events.Contains(ID))
		{
			GeneratorSubtype = "System";
		}
		else if (GeneratorType.Equals("External"))
		{
			GeneratorSubtype = "UserDefined";
		}
		else
		{
			GeneratorSubtype = "";
		}
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/Id");
		GeneratorID = xmlNode.InnerText;
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/Name");
		GeneratorName = xmlNode.InnerText;
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/GroupId");
		GeneratorGroupId = ((xmlNode != null) ? xmlNode.InnerText : string.Empty);
		xmlNode = xmlDocument.SelectSingleNode("EventType/Generator/GroupName");
		GeneratorGroupName = ((xmlNode != null) ? xmlNode.InnerText : string.Empty);
		xmlNode = xmlDocument.SelectSingleNode("EventType/StateGroupId");
		StateGroupId = ((!string.IsNullOrEmpty(xmlNode?.InnerText)) ? new Guid?(new Guid(xmlNode.InnerText)) : ((Guid?)null));
		State = xmlDocument.SelectSingleNode("EventType/State")?.InnerText;
		xmlNodeList = xmlDocument.SelectNodes("EventType/Sources/Source");
		List<EventSource> list = new List<EventSource>();
		for (int i = 0; i < xmlNodeList.Count; i++)
		{
			xmlNode = xmlNodeList[i];
			string innerText = xmlNode.SelectSingleNode("Type").InnerText;
			List<EventSourceFilter> list2 = new List<EventSourceFilter>();
			foreach (XmlNode item3 in xmlNode.SelectNodes("Filters/Filter"))
			{
				XmlAttribute xmlAttribute3 = item3.Attributes["type"];
				if (xmlAttribute3 != null)
				{
					list2.Add(new EventSourceFilter((EventSource.FilterTypes)Enum.Parse(typeof(EventSource.FilterTypes), xmlAttribute3.InnerText), item3.InnerText));
				}
			}
			list.Add(new EventSource(innerText, list2.ToArray()));
		}
		Sources = list.ToArray();
		xmlNode = xmlDocument.SelectSingleNode("EventType/OccursGlobally");
		OccursGlobally = bool.Parse(xmlNode.InnerText);
		xmlNode = xmlDocument.SelectSingleNode("EventType/BuiltIn");
		if (xmlNode != null)
		{
			BuiltIn = bool.Parse(xmlNode.InnerText);
		}
		else
		{
			BuiltIn = GeneratorSubtype.Equals("System");
		}
		xmlNode = xmlDocument.SelectSingleNode("EventType/CounterEventId");
		CounterEventID = new Guid(xmlNode.InnerText);
	}

	public void SaveAsXml(XmlWriter writer, bool includeHeader)
	{
		if (writer == null)
		{
			throw new ArgumentNullException("writer");
		}
		if (includeHeader)
		{
			writer.WriteStartDocument();
		}
		writer.WriteStartElement("EventType");
		writer.WriteElementString("Id", ID.ToString());
		writer.WriteElementString("Name", Name);
		writer.WriteStartElement("DisplayName");
		writer.WriteAttributeString("referenceid", DisplayNameReferenceId);
		writer.WriteValue(DisplayName);
		writer.WriteEndElement();
		writer.WriteStartElement("Description");
		writer.WriteAttributeString("referenceid", DescriptionReferenceId);
		writer.WriteValue(Description);
		writer.WriteEndElement();
		writer.WriteStartElement("Parameters");
		writer.WriteEndElement();
		writer.WriteStartElement("Generator");
		writer.WriteElementString("Type", GeneratorType);
		writer.WriteElementString("Subtype", GeneratorSubtype);
		writer.WriteElementString("Id", GeneratorID);
		writer.WriteElementString("Name", GeneratorName);
		writer.WriteElementString("GroupId", GeneratorGroupId);
		writer.WriteElementString("GroupName", GeneratorGroupName);
		writer.WriteEndElement();
		writer.WriteElementString("StateGroupId", StateGroupId.ToString());
		writer.WriteElementString("State", State);
		writer.WriteStartElement("Sources");
		EventSource[] sources = Sources;
		foreach (EventSource eventSource in sources)
		{
			writer.WriteStartElement("Source");
			writer.WriteElementString("Type", eventSource.Type);
			writer.WriteStartElement("Filters");
			if (eventSource.Filter != EventSource.FilterTypes.None)
			{
				foreach (EventSourceFilter filter in eventSource.Filters)
				{
					writer.WriteStartElement("Filter");
					writer.WriteAttributeString("type", filter.FilterType.ToString());
					writer.WriteValue(filter.Value);
					writer.WriteEndElement();
				}
			}
			writer.WriteEndElement();
			writer.WriteEndElement();
		}
		writer.WriteEndElement();
		writer.WriteElementString("OccursGlobally", OccursGlobally.ToString());
		writer.WriteElementString("BuiltIn", BuiltIn.ToString());
		writer.WriteElementString("CounterEventId", CounterEventID.ToString());
		writer.WriteEndElement();
		if (includeHeader)
		{
			writer.WriteEndDocument();
		}
	}

	public string SaveAsXml()
	{
		StringBuilder stringBuilder = new StringBuilder();
		StringWriter stringWriter = new StringWriter(stringBuilder, CultureInfo.InvariantCulture);
		try
		{
			using XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
			stringWriter = null;
			xmlTextWriter.Formatting = Formatting.Indented;
			SaveAsXml(xmlTextWriter, includeHeader: true);
		}
		finally
		{
			stringWriter?.Dispose();
		}
		return stringBuilder.ToString();
	}

	public bool SourceListContains(string sourceType)
	{
		EventSource[] sources = Sources;
		for (int i = 0; i < sources.Length; i++)
		{
			if (sources[i].Type == sourceType)
			{
				return true;
			}
		}
		return false;
	}

	public bool CheckAllSourcesInList(string[] list)
	{
		if (list == null)
		{
			throw new ArgumentNullException("list");
		}
		bool flag = true;
		EventSource[] sources = Sources;
		foreach (EventSource eventSource in sources)
		{
			bool flag2 = false;
			foreach (string text in list)
			{
				flag2 |= eventSource.Type == text;
			}
			flag = flag && flag2;
		}
		return flag;
	}

	public bool CheckForSpecificFilter(int index, EventSource.FilterTypes filter)
	{
		return (Sources[index].Filter | filter) != EventSource.FilterTypes.None;
	}

	public string[] GetSourceListWithoutFilters()
	{
		string[] array = new string[Sources.Length];
		for (int i = 0; i < Sources.Length; i++)
		{
			array[i] = Sources[i].Type;
		}
		return array;
	}

	public static DefinitionValues GetDefinitionValues(string eventDefinitionXml)
	{
		string displayName = string.Empty;
		bool result;
		using (StringReader textReader = new StringReader(eventDefinitionXml))
		{
			XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
			XPathNavigator xPathNavigator2 = xPathNavigator.SelectSingleNode("/EventType/DisplayName");
			if (xPathNavigator2 != null)
			{
				displayName = xPathNavigator2.Value;
			}
			if (!bool.TryParse(xPathNavigator.SelectSingleNode("/EventType/OccursGlobally").Value, out result))
			{
				result = false;
			}
		}
		return new DefinitionValues
		{
			DisplayName = displayName,
			OccursGlobally = result
		};
	}
}
