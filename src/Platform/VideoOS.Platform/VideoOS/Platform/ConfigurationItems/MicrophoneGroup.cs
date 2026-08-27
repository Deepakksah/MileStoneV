using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class MicrophoneGroup : IConfigurationItem
{
	public MicrophoneFolder MicrophoneFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MicrophoneFolder")
				{
					return new MicrophoneFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public MicrophoneGroupFolder MicrophoneGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "MicrophoneGroupFolder")
				{
					return new MicrophoneGroupFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

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

	public bool BuiltIn
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("BuiltIn");
			bool result = false;
			if (bool.TryParse(propertyValue, out result))
			{
				return result;
			}
			return false;
		}
	}

	public MicrophoneGroup(ServerId serverId, ConfigurationItem microphoneGroup)
		: base(serverId, microphoneGroup)
	{
	}

	public MicrophoneGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public MicrophoneGroup(FQID fqid)
		: base(fqid)
	{
	}
}
