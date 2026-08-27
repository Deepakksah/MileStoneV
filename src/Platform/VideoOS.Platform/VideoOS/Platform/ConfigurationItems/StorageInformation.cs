using System;
using System.Diagnostics.CodeAnalysis;
using VideoOS.ConfigurationApi.ClientService;

namespace VideoOS.Platform.ConfigurationItems;

[ExcludeFromCodeCoverage]
public class StorageInformation : IConfigurationItem
{
	public string AnyTablesWithData => base.Wrapper.GetPropertyValue("AnyTablesWithData");

	public DateTime LastUpdated
	{
		get
		{
			if (DateTime.TryParse(base.Wrapper.GetPropertyValue("LastUpdated"), out var result))
			{
				return result;
			}
			return DateTime.MinValue;
		}
	}

	public int LockedUsedSpaceProperty
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("LockedUsedSpaceProperty");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public int UsedSpace
	{
		get
		{
			string propertyValue = base.Wrapper.GetPropertyValue("UsedSpace");
			int result = 0;
			if (int.TryParse(propertyValue, out result))
			{
				return result;
			}
			return 0;
		}
	}

	public StorageInformation(ServerId serverId, ConfigurationItem storage)
		: base(serverId, storage)
	{
	}

	public StorageInformation(ServerId serverId, string path)
		: base(serverId, path)
	{
	}

	public StorageInformation(FQID fqid)
		: base(fqid)
	{
	}
}
