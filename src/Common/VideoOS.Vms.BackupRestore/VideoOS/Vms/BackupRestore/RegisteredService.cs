using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Xml;
using VideoOS.Management.Common;

namespace VideoOS.Vms.BackupRestore;

public class RegisteredService
{
	private const string RegisteredServiceXmlRootName = "RegisteredService";

	public Guid TypeId { get; set; }

	public Guid InstanceId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public bool Trusted { get; set; }

	public bool Enabled { get; set; }

	public string CustomData { get; set; }

	public IList<ServerAddress> Addresses { get; private set; }

	public RegisteredService(Guid typeId, Guid instanceId, string name, bool enabled, IList<ServerAddress> addresses)
	{
		TypeId = typeId;
		InstanceId = instanceId;
		Name = name;
		Description = string.Empty;
		Trusted = false;
		Enabled = enabled;
		CustomData = string.Empty;
		if (addresses == null)
		{
			Addresses = new List<ServerAddress>();
		}
		else
		{
			Addresses = addresses;
		}
	}

	private static void WriteAddressesAsXml(XmlWriter wr, IList<ServerAddress> addresses)
	{
		foreach (ServerAddress address in addresses)
		{
			wr.WriteStartElement("Address");
			wr.WriteAttributeString("URL", address.URL);
			wr.WriteAttributeString("Type", address.Type.ToString());
			wr.WriteEndElement();
		}
	}

	public static void WriteRegisteredServiceAsXml(XmlWriter xmlWriter, RegisteredService registeredService)
	{
		if (xmlWriter == null)
		{
			throw new ArgumentNullException("xmlWriter");
		}
		if (registeredService == null)
		{
			throw new ArgumentNullException("registeredService");
		}
		xmlWriter.WriteStartElement("RegisteredService");
		xmlWriter.WriteAttributeString("Name", registeredService.Name);
		xmlWriter.WriteAttributeString("TypeId", registeredService.TypeId.ToString("D", CultureInfo.InvariantCulture).ToUpperInvariant());
		xmlWriter.WriteAttributeString("InstanceId", registeredService.InstanceId.ToString("D", CultureInfo.InvariantCulture).ToUpperInvariant());
		xmlWriter.WriteAttributeString("Enabled", registeredService.Enabled.ToString(CultureInfo.InvariantCulture));
		xmlWriter.WriteAttributeString("Trusted", registeredService.Enabled.ToString(CultureInfo.InvariantCulture));
		WriteAddressesAsXml(xmlWriter, registeredService.Addresses);
		xmlWriter.WriteStartElement("Description");
		xmlWriter.WriteString(registeredService.Description);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteStartElement("CustomData");
		xmlWriter.WriteString(registeredService.CustomData);
		xmlWriter.WriteEndElement();
		xmlWriter.WriteEndElement();
	}

	public static RegisteredService ReadRegisteredServiceFromXml(string xml)
	{
		if (xml == null)
		{
			throw new ArgumentNullException("xml");
		}
		RegisteredService registeredService = new RegisteredService(Guid.Empty, Guid.Empty, null, enabled: false, null);
		using StringReader input = new StringReader(xml);
		XmlReaderSettings xmlReaderSettings = new XmlReaderSettings();
		xmlReaderSettings.IgnoreComments = true;
		xmlReaderSettings.IgnoreProcessingInstructions = true;
		xmlReaderSettings.IgnoreWhitespace = true;
		xmlReaderSettings.XmlResolver = null;
		xmlReaderSettings.ValidationType = ValidationType.Schema;
		using XmlReader xmlReader = XmlReader.Create(input, xmlReaderSettings);
		xmlReader.MoveToContent();
		if (!string.Equals(xmlReader.Name, "RegisteredService", StringComparison.Ordinal))
		{
			return null;
		}
		xmlReader.MoveToFirstAttribute();
		do
		{
			switch (xmlReader.Name)
			{
			case "Name":
				registeredService.Name = HttpUtility.HtmlDecode(xmlReader.Value);
				break;
			case "TypeId":
				registeredService.TypeId = XmlConvert.ToGuid(xmlReader.Value);
				break;
			case "InstanceId":
				registeredService.InstanceId = XmlConvert.ToGuid(xmlReader.Value);
				break;
			case "Enabled":
				registeredService.Enabled = XmlConvert.ToBoolean(xmlReader.Value.ToLower(CultureInfo.InvariantCulture));
				break;
			case "Trusted":
				registeredService.Trusted = XmlConvert.ToBoolean(xmlReader.Value.ToLower(CultureInfo.InvariantCulture));
				break;
			default:
				throw new NotSupportedException("Invalid attribute to the 'RegisteredService' element: '" + xmlReader.Name + "'.");
			}
		}
		while (xmlReader.MoveToNextAttribute());
		xmlReader.MoveToElement();
		while (xmlReader.Read() && string.Equals(xmlReader.Name, "Address", StringComparison.Ordinal))
		{
			ServerAddress serverAddress = new ServerAddress();
			xmlReader.MoveToFirstAttribute();
			do
			{
				string name = xmlReader.Name;
				if (!(name == "URL"))
				{
					if (name == "Type")
					{
						serverAddress.Type = GetServerAddressType(xmlReader.Value);
					}
				}
				else
				{
					serverAddress.URL = HttpUtility.HtmlDecode(xmlReader.Value);
				}
			}
			while (xmlReader.MoveToNextAttribute());
			registeredService.Addresses.Add(serverAddress);
		}
		registeredService.Description = HttpUtility.HtmlDecode(xmlReader.ReadInnerXml());
		registeredService.CustomData = HttpUtility.HtmlDecode(xmlReader.ReadInnerXml());
		return registeredService;
	}

	private static ServerAddressType GetServerAddressType(string serverAddressType)
	{
		if (Enum.GetNames(typeof(ServerAddressType)).Contains(serverAddressType))
		{
			return (ServerAddressType)Enum.Parse(typeof(ServerAddressType), serverAddressType, ignoreCase: true);
		}
		return ServerAddressType.Internal;
	}
}
