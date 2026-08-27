using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class PrivacyMask : IConfigurationItem
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

	public Dictionary<string, string> GridSizeValues => base.Wrapper.GetEnumValues("GridSize");

	public string GridSize
	{
		get
		{
			return base.Wrapper.GetPropertyValue("GridSize");
		}
		set
		{
			base.Wrapper.SetPropertyValue("GridSize", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public string PrivacyMaskRegions
	{
		get
		{
			return base.Wrapper.GetPropertyValue("PrivacyMaskRegions");
		}
		set
		{
			base.Wrapper.SetPropertyValue("PrivacyMaskRegions", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public PrivacyMask(ServerId serverId, ConfigurationItem privacyMask)
		: base(serverId, privacyMask)
	{
	}

	public PrivacyMask(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public PrivacyMask(FQID fqid)
		: base(fqid)
	{
	}
}
