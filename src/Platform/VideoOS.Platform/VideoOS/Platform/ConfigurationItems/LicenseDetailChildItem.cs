using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseDetailChildItem : IConfigurationChildItem
{
	public string PluginId => base.Wrapper.GetPropertyValue("PluginId");

	public string LicenseType => base.Wrapper.GetPropertyValue("LicenseType");

	public string Activated => base.Wrapper.GetPropertyValue("Activated");

	public string ChangesWithoutActivation => base.Wrapper.GetPropertyValue("ChangesWithoutActivation");

	public int InGrace
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("InGrace");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public string GraceExpired => base.Wrapper.GetPropertyValue("GraceExpired");

	public string NotLicensed => base.Wrapper.GetPropertyValue("NotLicensed");

	public string FirstGraceExpirationDate => base.Wrapper.GetPropertyValue("FirstGraceExpirationDate");

	public string Note => base.Wrapper.GetPropertyValue("Note");

	public LicenseDetailChildItem(ServerId serverId, ConfigurationItem licenseDetailChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, licenseDetailChildItem, parentWrapper)
	{
	}

	public LicenseDetailChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseDetailChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
