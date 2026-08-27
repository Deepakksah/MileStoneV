using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class ToolOptionAviGeneration : IConfigurationItem
{
	public string Compressor
	{
		get
		{
			return base.Wrapper.GetPropertyValue("Compressor");
		}
		set
		{
			base.Wrapper.SetPropertyValue("Compressor", value);
			base.Wrapper.IsDirty = true;
		}
	}

	public ToolOptionAviGeneration(ServerId serverId, ConfigurationItem toolOptionAviGeneration)
		: base(serverId, toolOptionAviGeneration)
	{
	}

	public ToolOptionAviGeneration(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public ToolOptionAviGeneration(FQID fqid)
		: base(fqid)
	{
	}
}
