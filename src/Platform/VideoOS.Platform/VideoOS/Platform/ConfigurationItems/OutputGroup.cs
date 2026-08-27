using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class OutputGroup : IConfigurationItem
{
	public OutputFolder OutputFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "OutputFolder")
				{
					return new OutputFolder(base.ServerId, configurationItem);
				}
			}
			return null;
		}
	}

	public OutputGroupFolder OutputGroupFolder
	{
		get
		{
			ConfigurationItem[] array = base.Wrapper.Children();
			foreach (ConfigurationItem configurationItem in array)
			{
				if (configurationItem.ItemType == "OutputGroupFolder")
				{
					return new OutputGroupFolder(base.ServerId, configurationItem);
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

	public OutputGroup(ServerId serverId, ConfigurationItem outputGroup)
		: base(serverId, outputGroup)
	{
	}

	public OutputGroup(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public OutputGroup(FQID fqid)
		: base(fqid)
	{
	}
}
