using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class InputEventGroup : IConfigurationItem
{
	public InputEventFolder InputEventFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "InputEventFolder")
				{
					return new InputEventFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public InputEventGroupFolder InputEventGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "InputEventGroupFolder")
				{
					return new InputEventGroupFolder(base.ServerId, configurationItem);
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

	public InputEventGroup(ServerId serverId, ConfigurationItem inputEventGroup)
		: base(serverId, inputEventGroup)
	{
	}

	public InputEventGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public InputEventGroup(FQID fqid)
		: base(fqid)
	{
	}
}
