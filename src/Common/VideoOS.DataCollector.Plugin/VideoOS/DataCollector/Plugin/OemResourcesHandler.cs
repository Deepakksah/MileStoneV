using System;
using System.IO;
using System.Reflection;
using System.Security;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.DataCollector.Plugin;

public class OemResourcesHandler
{
	private const string RESOURCE_FILENAME = "resources.xml";

	private OemResourcePackage Resource { get; set; }

	private string CompanyName => GetResourceString("CompanyNameShort");

	private string DefaultInstallDirectory => GetResourceString("DataCollectorServiceInstallDir");

	public string CommonApplicationDataFolder => Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), CompanyName), DefaultInstallDirectory);

	public OemResourcesHandler()
	{
		Resource = CreateOemResource(LoadResources());
	}

	private XmlDocument LoadResources()
	{
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(Path.Combine(directoryName, "resources.xml"));
		return xmlDocument;
	}

	private OemResourcePackage CreateOemResource(XmlDocument oemResourceXml)
	{
		OemResourcePackage oemResourcePackage = new OemResourcePackage(oemResourceXml);
		if (!oemResourcePackage.ValidateSignature())
		{
			throw new SecurityException("The OEM resource file is signed with an invalid signature.");
		}
		return oemResourcePackage;
	}

	private string GetResourceString(string name)
	{
		return (string)Resource.GetResourceItem(name).ResourceValueAsObject;
	}
}
