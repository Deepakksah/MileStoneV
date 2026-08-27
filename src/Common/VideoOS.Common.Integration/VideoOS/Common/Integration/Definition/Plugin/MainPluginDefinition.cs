using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Xml.Serialization;
using VideoOS.Common.Integration.Definition.Plugin.Schema;

namespace VideoOS.Common.Integration.Definition.Plugin;

public class MainPluginDefinition
{
	private definition _definition;

	public definition Base => _definition;

	public MainPluginDefinition(definition definition)
	{
		_definition = definition;
	}

	public PluginDefinition GetPluginDefinition(Guid id)
	{
		definitionPlugin[] plugin = _definition.plugin;
		foreach (definitionPlugin definitionPlugin2 in plugin)
		{
			if (new Guid(definitionPlugin2.referenceid) == id)
			{
				return new PluginDefinition(definitionPlugin2);
			}
		}
		return null;
	}

	public static MainPluginDefinition LoadXml(string Xml)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
		StringReader textReader = new StringReader(Xml);
		definition obj = (definition)xmlSerializer.Deserialize(textReader);
		InitDefinition(obj);
		return new MainPluginDefinition(obj);
	}

	public static MainPluginDefinition LoadFromFile(string filename)
	{
		XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
		FileStream fileStream = new FileStream(filename, FileMode.Open, FileAccess.Read, FileShare.Read);
		definition obj = (definition)xmlSerializer.Deserialize(fileStream);
		fileStream.Close();
		InitDefinition(obj);
		return new MainPluginDefinition(obj);
	}

	public string SaveAsXml()
	{
		lock (this)
		{
			CultureInfo currentCulture = Thread.CurrentThread.CurrentCulture;
			Thread.CurrentThread.CurrentCulture = CultureInfo.InvariantCulture;
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(definition));
			MemoryStream memoryStream = new MemoryStream();
			xmlSerializer.Serialize(memoryStream, _definition);
			Thread.CurrentThread.CurrentCulture = currentCulture;
			return Encoding.UTF8.GetString(memoryStream.ToArray());
		}
	}

	private static void InitDefinition(definition definition)
	{
		try
		{
			if (definition.plugin == null)
			{
				return;
			}
			definitionPlugin[] plugin = definition.plugin;
			foreach (definitionPlugin definitionPlugin2 in plugin)
			{
				if (definitionPlugin2.capabilities == null)
				{
					continue;
				}
				capabilityType[] capability = definitionPlugin2.capabilities.capability;
				foreach (capabilityType capabilityType2 in capability)
				{
					if (capabilityType2.type == capabilityTypeType.data)
					{
						InitCapabilityData(capabilityType2);
					}
				}
			}
		}
		catch (Exception)
		{
			throw;
		}
	}

	private static object InitCapabilityData(object data, Type t, dataTypeDatatype dt)
	{
		if (data is Array && data != null && ((XmlNode[])data)[0].Value != null)
		{
			data = Convert.ChangeType(((XmlNode[])data)[0].Value.ToString(), t, CultureInfo.InvariantCulture);
		}
		return data;
	}

	private static void InitCapabilityData(capabilityType ct)
	{
		if (ct.type == capabilityTypeType.data)
		{
			dataType dataType2 = (dataType)ct.Item;
			Type t = SchemaTypeToType(dataType2.datatype);
			dataType2.defaultvalue = InitCapabilityData(dataType2.defaultvalue, t, dataType2.datatype);
			dataType2.minvalue = InitCapabilityData(dataType2.minvalue, t, dataType2.datatype);
			dataType2.maxvalue = InitCapabilityData(dataType2.maxvalue, t, dataType2.datatype);
			dataType2.resolution = InitCapabilityData(dataType2.resolution, t, dataType2.datatype);
		}
	}

	public static Type SchemaTypeToType(dataTypeDatatype dataType)
	{
		Type result = null;
		switch (dataType)
		{
		case dataTypeDatatype.@int:
			result = typeof(int);
			break;
		case dataTypeDatatype.base64binary:
			result = typeof(byte[]);
			break;
		case dataTypeDatatype.boolean:
			result = typeof(bool);
			break;
		case dataTypeDatatype.dateTime:
			result = typeof(DateTime);
			break;
		case dataTypeDatatype.@double:
			result = typeof(double);
			break;
		case dataTypeDatatype.@float:
			result = typeof(float);
			break;
		case dataTypeDatatype.@string:
			result = typeof(string);
			break;
		}
		return result;
	}
}
