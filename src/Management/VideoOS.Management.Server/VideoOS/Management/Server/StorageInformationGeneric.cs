using System.Collections.Generic;
using System.Globalization;

namespace VideoOS.Management.Server;

public sealed class StorageInformationGeneric
{
	public static GenericObject2 GetRecordingStorageInformation(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		return ConvertToGenericObject2(StorageDiskInformation.GetRecordingStorageInformation(requestContext, recordingStorageId), recordingStorageId);
	}

	public static GenericObject2 GetArchiveStorageInformation(IGeminiRequestContext requestContext, string archiveStorageId)
	{
		return ConvertToGenericObject2(StorageDiskInformation.GetArchiveStorageInformation(requestContext, archiveStorageId), archiveStorageId);
	}

	private static GenericObject2 ConvertToGenericObject2(StorageDiskInformation storageDiskInformation, string storageId)
	{
		if (storageDiskInformation == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("AvailableSpace", storageDiskInformation.AvailableSpace));
		list.Add(new ObjectProperty2("UsedSpace", storageDiskInformation.UsedSpace));
		list.Add(new ObjectProperty2("LastUpdated", storageDiskInformation.LastUpdated.ToUniversalTime().ToString("o", CultureInfo.InvariantCulture)));
		list.Add(new ObjectProperty2("LockedUsedSpace", storageDiskInformation.LockedUsedSpace));
		list.Add(new ObjectProperty2("AnyTablesWithData", storageDiskInformation.AnyTablesWithData.ToString()));
		list.Add(new ObjectProperty2("IsAvailable", storageDiskInformation.IsAvailable.ToString()));
		list.Add(new ObjectProperty2("IsMounted", storageDiskInformation.IsMounted.ToString()));
		return new GenericObject2(storageId, "StorageInformation", list.ToArray(), new RelationProperty[0]);
	}
}
