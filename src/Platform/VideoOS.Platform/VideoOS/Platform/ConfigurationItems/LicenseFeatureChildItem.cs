using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;
using VideoOS.Platform.Proxy.ConfigApi;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class LicenseFeatureChildItem : IConfigurationChildItem
{
	private Dictionary<string, string> _Parameters;

	public Dictionary<string, string> Parameters
	{
		get
		{
			if (_Parameters == null)
			{
				_Parameters = base.Wrapper.GetPropertyDictionaryString("Parameters");
			}
			return _Parameters;
		}
	}

	public LicenseFeatureChildItem(ServerId serverId, ConfigurationItem licenseFeatureChildItem, ConfigurationApiItemWrapper parentWrapper)
		: base(serverId, licenseFeatureChildItem, parentWrapper)
	{
	}

	public LicenseFeatureChildItem(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public LicenseFeatureChildItem(FQID fqid)
		: base(fqid)
	{
	}
}
