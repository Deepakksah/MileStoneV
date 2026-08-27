using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MonitorPreset : IConfigurationItem
{
	public string Id => base.Wrapper.GetPropertyValue("Id");

	public DateTime LastModified
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastModified"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string DefinitionXml
	{
		get
		{
			return base.Wrapper.GetPropertyValue("DefinitionXml");
		}
		set
		{
			base.Wrapper.SetPropertyValue("DefinitionXml", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public MonitorPreset(ServerId serverId, ConfigurationItem monitorPreset)
		: base(serverId, monitorPreset)
	{
	}

	public MonitorPreset(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MonitorPreset(FQID fqid)
		: base(fqid)
	{
	}
}
