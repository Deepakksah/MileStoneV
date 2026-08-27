using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("RecordingStorage")]
public class RecordingStorageGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(RecordingStorage.QueryRecordingStorage(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(RecordingStorage.QueryRecordingStoragesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "RecordingServer" && entityName == "RecordingStorage")
		{
			return RecordingStorage.QueryRecordingStorageIds(requestContext, parentId);
		}
		if ((parentEntityName == "Device" || parentEntityName == "CameraDevice") && entityName == "RecordingStorage")
		{
			return RecordingStorage.QueryRecordingStorageIdsByDeviceId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		RecordingStorage recordingStorage = ConvertToRecordingStorage(properties);
		recordingStorage.CustomProperties = customProperties;
		return ConvertToGenericObject2(RecordingStorage.CreateRecordingStorage(requestContext, recordingStorage));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		RecordingStorage recordingStorage = ConvertToRecordingStorage(genericObject);
		recordingStorage.CustomProperties = customProperties;
		return ConvertToGenericObject2(RecordingStorage.UpdateRecordingStorage(requestContext, recordingStorage));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Task.DeleteRecordingStorage(requestContext, id, deleteIfOffline: false);
	}

	public static void SetRecordingStorageAsDefault(IGeminiRequestContext requestContext, string recordingStorageId)
	{
		RecordingStorage.SetRecordingStorageAsDefault(requestContext, recordingStorageId);
	}

	public static bool ValidateEncryptionPassword(IGeminiRequestContext requestContext, string recordingStorageId, string password)
	{
		return RecordingStorage.ValidateEncryptionPassword(requestContext, recordingStorageId, password);
	}

	private static GenericObject2 ConvertToGenericObject2(RecordingStorage recordingStorage)
	{
		if (recordingStorage != null)
		{
			List<ObjectProperty2> list = new List<ObjectProperty2>();
			list.Add(new ObjectProperty2("Id", recordingStorage.Id));
			list.Add(new ObjectProperty2("Name", recordingStorage.Name));
			list.Add(new ObjectProperty2("Description", recordingStorage.Description));
			list.Add(new ObjectProperty2("CreatedDate", recordingStorage.CreatedDate));
			list.Add(new ObjectProperty2("LastModified", recordingStorage.LastModified));
			list.Add(new ObjectProperty2("Path", recordingStorage.Path));
			list.Add(new ObjectProperty2("IsDefault", recordingStorage.IsDefault));
			list.Add(new ObjectProperty2("RetainMinutes", recordingStorage.RetainMinutes));
			list.Add(new ObjectProperty2("MaxSize", recordingStorage.MaxSize));
			list.Add(new ObjectProperty2("SpaceUsed", recordingStorage.SpaceUsed));
			list.Add(new ObjectProperty2("SpaceAvailable", recordingStorage.SpaceAvailable));
			list.Add(new ObjectProperty2("Signing", recordingStorage.Signing));
			list.Add(new ObjectProperty2("EncryptionMethod", recordingStorage.Encryption.ToString()));
			list.Add(new ObjectProperty2("RecordingServerId", recordingStorage.RecordingServerId));
			List<RelationProperty> list2 = new List<RelationProperty>();
			list2.Add(new RelationProperty("RecordingServer", recordingStorage.RecordingServerId));
			return new GenericObject2(recordingStorage.Id, "RecordingStorage", list.ToArray(), list2.ToArray());
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2s(RecordingStorage[] recordingStorages)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (RecordingStorage recordingStorage in recordingStorages)
		{
			if (recordingStorage != null)
			{
				list.Add(ConvertToGenericObject2(recordingStorage));
			}
		}
		return list.ToArray();
	}

	private static RecordingStorage ConvertToRecordingStorage(GenericObject2 genericObject)
	{
		return ConvertToRecordingStorage(genericObject.Properties);
	}

	private static RecordingStorage ConvertToRecordingStorage(ObjectProperty2[] properties)
	{
		RecordingStorage recordingStorage = new RecordingStorage();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			recordingStorage.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			recordingStorage.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			recordingStorage.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "CreatedDate"))
		{
			recordingStorage.CreatedDate = GenericObjectHelper2.GetProperty<DateTime>(properties, "CreatedDate");
		}
		if (GenericObjectHelper2.Contains(properties, "LastModified"))
		{
			recordingStorage.LastModified = GenericObjectHelper2.GetProperty<DateTime>(properties, "LastModified");
		}
		if (GenericObjectHelper2.Contains(properties, "Path"))
		{
			recordingStorage.Path = GenericObjectHelper2.GetProperty<string>(properties, "Path");
		}
		if (GenericObjectHelper2.Contains(properties, "IsDefault"))
		{
			recordingStorage.IsDefault = GenericObjectHelper2.GetProperty<bool>(properties, "IsDefault");
		}
		if (GenericObjectHelper2.Contains(properties, "RetainMinutes"))
		{
			recordingStorage.RetainMinutes = GenericObjectHelper2.GetProperty<int>(properties, "RetainMinutes");
		}
		if (GenericObjectHelper2.Contains(properties, "MaxSize"))
		{
			recordingStorage.MaxSize = GenericObjectHelper2.GetProperty<long>(properties, "MaxSize");
		}
		if (GenericObjectHelper2.Contains(properties, "SpaceUsed"))
		{
			recordingStorage.SpaceUsed = GenericObjectHelper2.GetProperty<int>(properties, "SpaceUsed");
		}
		if (GenericObjectHelper2.Contains(properties, "SpaceAvailable"))
		{
			recordingStorage.SpaceAvailable = GenericObjectHelper2.GetProperty<int>(properties, "SpaceAvailable");
		}
		if (GenericObjectHelper2.Contains(properties, "Signing"))
		{
			recordingStorage.Signing = GenericObjectHelper2.GetProperty<bool>(properties, "Signing");
		}
		if (GenericObjectHelper2.Contains(properties, "EncryptionMethod"))
		{
			recordingStorage.Encryption = GenericObjectHelper2.GetProperty<EncryptionMethod>(properties, "EncryptionMethod");
		}
		if (GenericObjectHelper2.Contains(properties, "RecordingServerId"))
		{
			recordingStorage.RecordingServerId = GenericObjectHelper2.GetProperty<string>(properties, "RecordingServerId");
		}
		return recordingStorage;
	}
}
