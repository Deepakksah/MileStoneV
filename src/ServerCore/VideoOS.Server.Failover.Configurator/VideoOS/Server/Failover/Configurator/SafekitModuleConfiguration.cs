using System.IO;
using System.Xml;
using Newtonsoft.Json;
using VideoOS.Utilities.Validation;

namespace VideoOS.Server.Failover.Configurator;

public class SafekitModuleConfiguration
{
	public class IdpSettings
	{
		[JsonProperty]
		public string Authority { get; private set; }

		private IdpSettings()
		{
		}
	}

	public string VirtualAddress { get; }

	public static SafekitModuleConfiguration Load(string path)
	{
		path.NullCheck("", "Load", "C:\\actions-runner\\_work\\XPCO\\XPCO\\Corporate\\VideoOS.Server.Failover.Configurator\\SafekitModuleConfiguration.cs", 16);
		string xml = File.ReadAllText(path);
		XmlDocument xmlDocument = new XmlDocument();
		xmlDocument.LoadXml(xml);
		return new SafekitModuleConfiguration((xmlDocument.SelectSingleNode("safe/service/vip/interface_list/interface/real_interface/virtual_addr") ?? throw new SafeKitVirtualAddressNotFoundException()).Attributes["addr"].Value);
	}

	private SafekitModuleConfiguration(string address)
	{
		VirtualAddress = address;
	}
}
