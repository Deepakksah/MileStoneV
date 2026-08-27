using System.IO;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Xml;
using VideoOS.Utilities.OemResource;

namespace VideoOS.GatewayService.Utilities.Oem;

public class GatewayServiceOemResource : IGatewayServiceOemResource
{
	private readonly OemResourcePackage _oemResourcePackage;

	private static string _resourceFilePath => Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "resources.xml");

	public string CompanyNameShort => GetResourceString("CompanyNameShort");

	public string ApiGatewayDisplayName => GetResourceString("ApiGatewayDisplayName");

	public string ApiGatewayInstallDir => GetResourceString("ApiGatewayInstallDir");

	public GatewayServiceOemResource()
	{
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.Load(_resourceFilePath);
		_oemResourcePackage = new OemResourcePackage(xmlDocument);
		if (!_oemResourcePackage.ValidateSignature())
		{
			throw new InvalidOemResourceException("Invalid OEM Resource file. File might have been tampered with");
		}
	}

	protected string GetResourceString([CallerMemberName] string name = "")
	{
		return (_oemResourcePackage.GetResourceItem(name)?.ResourceValueAsObject as string) ?? throw new InvalidOemResourceException("Unable to find OEM Resource, name: " + name);
	}
}
