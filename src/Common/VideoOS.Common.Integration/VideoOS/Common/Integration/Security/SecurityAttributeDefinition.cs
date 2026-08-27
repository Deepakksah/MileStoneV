using System;
using System.Globalization;
using System.Xml;

namespace VideoOS.Common.Integration.Security;

public class SecurityAttributeDefinition
{
	private XmlDocument _definitionXml;

	public XmlDocument DefinitionXml => _definitionXml;

	public SecurityAttributeDefinition(string definitionXml)
	{
		_definitionXml = new XmlDocument();
		try
		{
			_definitionXml.LoadXml(definitionXml);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception)
		{
		}
	}

	public object GetDefaultAttributeValue()
	{
		XmlNode xmlNode = _definitionXml.DocumentElement.SelectSingleNode("enumeration/defaultvalue");
		if (xmlNode == null)
		{
			xmlNode = _definitionXml.DocumentElement.SelectSingleNode("data/defaultvalue");
		}
		if (xmlNode != null)
		{
			if (int.TryParse(xmlNode.InnerText, out var result))
			{
				return result;
			}
			return xmlNode.InnerText;
		}
		return null;
	}

	public object GetAdministratorAttributeValue()
	{
		XmlNode xmlNode = _definitionXml.DocumentElement.SelectSingleNode("enumeration/administratorvalue");
		if (xmlNode != null)
		{
			return int.Parse(xmlNode.InnerText, CultureInfo.InvariantCulture);
		}
		XmlNode xmlNode2 = _definitionXml.DocumentElement.SelectSingleNode("data/administratorvalue");
		if (xmlNode2 != null)
		{
			return int.Parse(xmlNode2.InnerText, CultureInfo.InvariantCulture);
		}
		return null;
	}
}
