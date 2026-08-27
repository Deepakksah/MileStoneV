using System;
using System.Globalization;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("StorageInformation")]
public sealed class StorageInformation
{
	public long AvailableSpace { get; private set; }

	public long UsedSpace { get; private set; }

	public long LockedUsedSpace { get; private set; }

	public DateTimeOffset LastUpdated { get; private set; }

	public TableContainsAnyData AnyTablesWithData { get; private set; }

	public bool IsAvailable { get; private set; }

	public bool IsMounted { get; private set; }

	public StorageInformation(VmoPropertyCollection objectProperties)
	{
		AvailableSpace = objectProperties.GetPropertyValue<long>("AvailableSpace");
		UsedSpace = objectProperties.GetPropertyValue<long>("UsedSpace");
		LockedUsedSpace = objectProperties.GetPropertyValue<long>("LockedUsedSpace");
		if (objectProperties.ContainsKey("AnyTablesWithData"))
		{
			AnyTablesWithData = objectProperties.GetPropertyValue<TableContainsAnyData>("AnyTablesWithData");
		}
		else
		{
			AnyTablesWithData = TableContainsAnyData.Unknown;
		}
		if (DateTimeOffset.TryParseExact(objectProperties.GetPropertyValue<string>("LastUpdated"), "o", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal, out var result))
		{
			LastUpdated = result;
		}
		if (objectProperties.ContainsKey("IsAvailable"))
		{
			IsAvailable = objectProperties.GetPropertyValue<bool>("IsAvailable");
		}
		else
		{
			IsAvailable = true;
		}
		if (objectProperties.ContainsKey("IsMounted"))
		{
			IsMounted = objectProperties.GetPropertyValue<bool>("IsMounted");
		}
		else
		{
			IsMounted = true;
		}
	}
}
