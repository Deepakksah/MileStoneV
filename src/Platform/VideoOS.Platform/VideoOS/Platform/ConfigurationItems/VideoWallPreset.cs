using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class VideoWallPreset : IConfigurationItem
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

	public VideoWallPreset(ServerId serverId, ConfigurationItem videoWallPreset)
		: base(serverId, videoWallPreset)
	{
	}

	public VideoWallPreset(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public VideoWallPreset(FQID fqid)
		: base(fqid)
	{
	}

	public ServerTask Activate()
	{
		try
		{
			return new ServerTask(base.Wrapper.ExecuteMethod("Activate"));
		}
		catch (ArgumentException ex)
		{
			EnvironmentManager.Instance.Log(error: false, "Activate", "Unable to call method 'Activate':" + ex.Message);
			return null;
		}
	}
}
