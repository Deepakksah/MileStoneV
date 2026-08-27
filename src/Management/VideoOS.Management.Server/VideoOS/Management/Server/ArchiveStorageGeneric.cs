using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("ArchiveStorage")]
public class ArchiveStorageGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(ArchiveStorage.QueryArchiveStorage(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(ArchiveStorage.QueryArchiveStoragesByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "RecordingServer" && entityName == "ArchiveStorage")
		{
			return ArchiveStorage.QueryArchiveStorageIdsByRecordingServerId(requestContext, parentId);
		}
		if (parentEntityName == "RecordingStorage" && entityName == "ArchiveStorage")
		{
			return ArchiveStorage.QueryArchiveStorageIdsByRecordingStorageId(requestContext, parentId);
		}
		if ((parentEntityName == "Device" || parentEntityName == "CameraDevice") && entityName == "ArchiveStorage")
		{
			return ArchiveStorage.QueryArchiveStorageIdsByDeviceId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		ArchiveStorageDataContract archiveStorageDataContract = ConvertToArchiveStorage(properties);
		archiveStorageDataContract.CustomProperties = customProperties;
		return ConvertToGenericObject2(ArchiveStorage.CreateArchiveStorage(requestContext, archiveStorageDataContract));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		ArchiveStorageDataContract archiveStorageDataContract = ConvertToArchiveStorage(genericObject);
		archiveStorageDataContract.CustomProperties = customProperties;
		return ConvertToGenericObject2(ArchiveStorage.UpdateArchiveStorage(requestContext, archiveStorageDataContract));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		Task.DeleteArchiveStorage(requestContext, id, deleteIfOffline: false);
	}

	public static void EnableFramerateReduction(IGeminiRequestContext requestContext, string archiveStorageId, bool enable)
	{
		ArchiveStorage.EnableFramerateReduction(requestContext, archiveStorageId, enable);
	}

	private static GenericObject2 ConvertToGenericObject2(ArchiveStorageDataContract archiveStorage)
	{
		if (archiveStorage != null)
		{
			ObjectProperty2[] properties = new ObjectProperty2[15]
			{
				new ObjectProperty2("Id", archiveStorage.Id),
				new ObjectProperty2("Name", archiveStorage.Name),
				new ObjectProperty2("Description", archiveStorage.Description),
				new ObjectProperty2("CreatedDate", archiveStorage.CreatedDate),
				new ObjectProperty2("LastModified", archiveStorage.LastModified),
				new ObjectProperty2("Path", archiveStorage.Path),
				new ObjectProperty2("IsDefault", archiveStorage.IsDefault),
				new ObjectProperty2("RetainMinutes", archiveStorage.RetainMinutes),
				new ObjectProperty2("MaxSize", archiveStorage.MaxSize),
				new ObjectProperty2("FramerateReductionEnabled", archiveStorage.FramerateReductionEnabled),
				new ObjectProperty2("TargetFramerate", archiveStorage.TargetFramerate),
				new ObjectProperty2("ArchiveSchedule", archiveStorage.ArchiveSchedule),
				new ObjectProperty2("Mounted", archiveStorage.Mounted),
				new ObjectProperty2("RecordingStorageId", archiveStorage.RecordingStorageId),
				new ObjectProperty2("RecordingServerId", archiveStorage.RecordingServerId)
			};
			RelationProperty[] relations = new RelationProperty[2]
			{
				new RelationProperty("RecordingServer", archiveStorage.RecordingServerId),
				new RelationProperty("RecordingStorage", archiveStorage.RecordingStorageId)
			};
			return new GenericObject2(archiveStorage.Id, "ArchiveStorage", properties, relations);
		}
		return null;
	}

	private static GenericObject2[] ConvertToGenericObject2s(IEnumerable<ArchiveStorageDataContract> archiveStorages)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (ArchiveStorageDataContract archiveStorage in archiveStorages)
		{
			if (archiveStorage != null)
			{
				list.Add(ConvertToGenericObject2(archiveStorage));
			}
		}
		return list.ToArray();
	}

	private static ArchiveStorageDataContract ConvertToArchiveStorage(GenericObject2 genericObject)
	{
		return ConvertToArchiveStorage(genericObject.Properties);
	}

	private static ArchiveStorageDataContract ConvertToArchiveStorage(ObjectProperty2[] properties)
	{
		return new ArchiveStorageDataContract
		{
			Id = properties.GetPropertyOrDefault<string>("Id"),
			Name = properties.GetPropertyOrDefault<string>("Name"),
			Description = properties.GetPropertyOrDefault<string>("Description"),
			CreatedDate = properties.GetPropertyOrNullable<DateTime>("CreatedDate").GetValueOrDefault(),
			LastModified = properties.GetPropertyOrNullable<DateTime>("LastModified").GetValueOrDefault(),
			Path = properties.GetPropertyOrDefault<string>("Path"),
			IsDefault = (properties.GetPropertyOrNullable<bool>("IsDefault") == true),
			RetainMinutes = properties.GetPropertyOrNullable<int>("RetainMinutes").GetValueOrDefault(),
			MaxSize = properties.GetPropertyOrNullable<long>("MaxSize").GetValueOrDefault(),
			FramerateReductionEnabled = (properties.GetPropertyOrNullable<bool>("FramerateReductionEnabled") == true),
			TargetFramerate = properties.GetPropertyOrNullable<double>("TargetFramerate").GetValueOrDefault(),
			ArchiveSchedule = properties.GetPropertyOrDefault<string>("ArchiveSchedule"),
			Mounted = (properties.GetPropertyOrNullable<bool>("Mounted") ?? true),
			RecordingStorageId = properties.GetPropertyOrDefault<string>("RecordingStorageId"),
			RecordingServerId = properties.GetPropertyOrDefault<string>("RecordingServerId")
		};
	}
}
