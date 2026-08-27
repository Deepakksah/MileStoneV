using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class Layout : IConfigurationItem
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

	public DateTime CreatedDate
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("CreatedDate"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public string DefinitionXml => base.Wrapper.GetPropertyValue("DefinitionXml");

	public Layout(ServerId serverId, ConfigurationItem layout)
		: base(serverId, layout)
	{
	}

	public Layout(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public Layout(FQID fqid)
		: base(fqid)
	{
	}
}
