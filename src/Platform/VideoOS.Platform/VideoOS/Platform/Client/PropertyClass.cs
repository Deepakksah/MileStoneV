using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;

namespace VideoOS.Platform.Client;

public abstract class PropertyClass
{
	protected SortedList _properties = new SortedList();

	protected SortedList _temporaryProperties = new SortedList();

	public PropertyClass()
	{
	}

	public virtual bool Update(XmlNode xmlNode, List<Guid> idUpdateForChanges, bool force)
	{
		bool result = false;
		XmlNode xmlNode2 = ((xmlNode.Name == "properties") ? xmlNode : xmlNode.SelectSingleNode("properties"));
		if (xmlNode2 == null)
		{
			xmlNode2 = xmlNode.SelectSingleNode("viewitem/properties");
		}
		int num = 0;
		foreach (XmlNode item in xmlNode2)
		{
			num++;
			if (_properties.ContainsKey(item.Attributes["name"].InnerText))
			{
				string obj = _properties[item.Attributes["name"].InnerText] as string;
				string innerText = item.Attributes["value"].InnerText;
				if (obj != innerText)
				{
					result = true;
				}
			}
			else
			{
				result = true;
			}
		}
		if (num != _properties.Count)
		{
			result = true;
		}
		_properties.Clear();
		foreach (XmlNode item2 in xmlNode2)
		{
			_properties.Add(item2.Attributes["name"].InnerText, item2.Attributes["value"].InnerText);
		}
		return result;
	}

	public virtual bool CompareContent(PropertyClass property)
	{
		foreach (string key in _properties.Keys)
		{
			if (property._properties.ContainsKey(key))
			{
				if (!property._properties[key].Equals(_properties[key]))
				{
					return false;
				}
				continue;
			}
			return false;
		}
		return true;
	}

	public void ClearProperties()
	{
		_properties = new SortedList();
	}

	public void SetProperty(string key, string value)
	{
		if (_properties.Contains(key))
		{
			if (value == null)
			{
				_properties.Remove(key);
			}
			else
			{
				_properties[key] = value;
			}
		}
		else if (value != null)
		{
			_properties.Add(key, value);
		}
	}

	public string GetProperty(string key)
	{
		return (string)_properties[key];
	}

	public ICollection GetPropertyKeys()
	{
		return _properties.Keys;
	}

	public void SetTemporaryProperty(string key, object value)
	{
		if (_temporaryProperties.Contains(key))
		{
			_temporaryProperties[key] = value;
		}
		else
		{
			_temporaryProperties.Add(key, value);
		}
	}

	public object GetTemporaryProperty(string key)
	{
		return _temporaryProperties[key];
	}

	public virtual void Serialize(XmlTextWriter xmlTextWriter)
	{
		xmlTextWriter.WriteStartElement("properties");
		for (int i = 0; i < _properties.Count; i++)
		{
			xmlTextWriter.WriteStartElement("property");
			xmlTextWriter.WriteAttributeString("name", _properties.GetKey(i).ToString());
			xmlTextWriter.WriteAttributeString("value", _properties.GetByIndex(i).ToString());
			xmlTextWriter.WriteEndElement();
		}
		xmlTextWriter.WriteEndElement();
	}

	public virtual string SerializeToString()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		Serialize(xmlTextWriter);
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument.OuterXml;
	}

	public virtual XmlNode SerializeToXmlNode()
	{
		MemoryStream memoryStream = new MemoryStream();
		XmlTextWriter xmlTextWriter = new XmlTextWriter(memoryStream, Encoding.UTF8);
		Serialize(xmlTextWriter);
		xmlTextWriter.Flush();
		memoryStream.Position = 0L;
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(memoryStream);
		return xmlDocument.FirstChild;
	}

	public virtual void Deserialize(XmlNode xmlNode)
	{
		XmlNode xmlNode2 = ((xmlNode.Name == "properties") ? xmlNode : xmlNode.SelectSingleNode("properties"));
		if (xmlNode2 == null)
		{
			xmlNode2 = xmlNode.SelectSingleNode("viewitem/properties");
		}
		if (xmlNode2 == null)
		{
			return;
		}
		foreach (XmlNode item in xmlNode2)
		{
			_properties[item.Attributes["name"].InnerText] = item.Attributes["value"].InnerText;
		}
	}
}
