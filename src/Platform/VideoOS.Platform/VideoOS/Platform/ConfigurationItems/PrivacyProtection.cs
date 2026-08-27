using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PrivacyProtection : IConfigurationItem
{
	public bool Enabled
	{
		get
		{
			return base.Wrapper.ConfigurationItem.EnableProperty.Enabled;
		}
		set
		{
			base.Wrapper.ConfigurationItem.EnableProperty.Enabled = value;
			base.Wrapper.IsDirty = true;
		}
	}

	public string Id => base.Wrapper.GetPropertyValue("Id");

	public string PrivacyMaskXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PrivacyMaskXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrivacyMaskXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public PrivacyProtection(ServerId serverId, ConfigurationItem privacyProtection)
		: base(serverId, privacyProtection)
	{
	}

	public PrivacyProtection(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PrivacyProtection(FQID fqid)
		: base(fqid)
	{
	}

	public PrivacyProtection()
	{
	}
}
