using System;
using System.Collections.Generic;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("RecordingServer")]
public class RecordingServerGeneric : IGenericObjectSupport2
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2(RecordingServer.QueryRecordingServer(requestContext, id));
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		return ConvertToGenericObject2s(RecordingServer.QueryRecordingServersByIds(requestContext, ids));
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		if (parentEntityName == "ManagementServer" && entityName == "RecordingServer")
		{
			return RecordingServer.QueryRecordingServerIds(requestContext);
		}
		if (parentEntityName == "RecordingServerGroup" && entityName == "RecordingServer")
		{
			return RecordingServer.QueryRecordingServerIdsByServerGroupId(requestContext, parentId);
		}
		return null;
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		properties.TryGetProperty<string>("Id", out var value);
		properties.TryGetProperty<string>("Name", out var value2);
		properties.TryGetProperty<string>("HostName", out var value3);
		properties.TryGetProperty<string>("WebServerUri", out var value4);
		properties.TryGetProperty<string>("TimeZoneName", out var value5);
		properties.TryGetProperty<string>("ScanRanges", out var value6);
		((IEnumerable<ObjectProperty2>)properties).TryGetProperty<int>("FailoverPort", out int? _);
		properties.TryGetProperty<string>("DefaultStorageName", out var value8);
		properties.TryGetProperty<string>("DefaultStoragePath", out var value9);
		((IEnumerable<ObjectProperty2>)properties).TryGetProperty<int>("DefaultStorageRetainMinutes", out int? value10);
		((IEnumerable<ObjectProperty2>)properties).TryGetProperty<long>("DefaultStorageMaxSizeInMB", out long? value11);
		((IEnumerable<ObjectProperty2>)properties).TryGetProperty<int>("DefaultFailoverUdpPort", out int? value12);
		return ConvertToGenericObject2(RecordingServer.CreateRecordingServer(requestContext, GeminiResourceComponent.GetGuid(value), value2, value3, value4, value5, value6, value8, value9, value10, value11, value12));
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		RecordingServer recordingServer = ConvertToRecordingServer(genericObject.Properties);
		return ConvertToGenericObject2(RecordingServer.UpdateRecordingServer(requestContext, recordingServer.Id, recordingServer.Name, recordingServer.Description, recordingServer.SynchronizationTime, customProperties));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	private static GenericObject2 ConvertToGenericObject2(RecordingServer recordingServer)
	{
		if (recordingServer == null)
		{
			return null;
		}
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", recordingServer.Id));
		list.Add(new ObjectProperty2("Name", recordingServer.Name));
		list.Add(new ObjectProperty2("Description", recordingServer.Description));
		list.Add(new ObjectProperty2("SynchronizationTime", recordingServer.SynchronizationTime));
		list.Add(new ObjectProperty2("LastModified", recordingServer.LastModified));
		list.Add(new ObjectProperty2("HostName", recordingServer.HostName));
		list.Add(new ObjectProperty2("Enabled", recordingServer.Enabled));
		list.Add(new ObjectProperty2("TimeZoneName", recordingServer.TimeZoneName));
		list.Add(new ObjectProperty2("MulticastSettings", recordingServer.MulticastSettings));
		list.Add(new ObjectProperty2("DevicePackInfo", recordingServer.DevicePackInfo));
		list.Add(new ObjectProperty2("X64", recordingServer.X64));
		list.Add(new ObjectProperty2("PrimaryFailoverGroup", recordingServer.PrimaryFailoverGroup));
		list.Add(new ObjectProperty2("SecondaryFailoverGroup", recordingServer.SecondaryFailoverGroup));
		list.Add(new ObjectProperty2("FailoverPort", recordingServer.FailoverPort));
		list.Add(new ObjectProperty2("FailoverHostName", recordingServer.FailoverHostName));
		list.Add(new ObjectProperty2("FailoverId", recordingServer.FailoverId));
		list.Add(new ObjectProperty2("ScanRanges", recordingServer.ScanRanges));
		list.Add(new ObjectProperty2("ScanRangeWarning", recordingServer.ScanRangeWarning));
		list.Add(new ObjectProperty2("WebServerUri", recordingServer.WebServerUri));
		list.Add(new ObjectProperty2("WebServerUriConfiguration", recordingServer.ActiveWebServerUriConfiguration));
		return new GenericObject2(recordingServer.Id, "RecordingServer", list.ToArray(), null);
	}

	private static GenericObject2[] ConvertToGenericObject2s(RecordingServer[] RecordingServers)
	{
		List<GenericObject2> list = new List<GenericObject2>();
		foreach (RecordingServer recordingServer in RecordingServers)
		{
			list.Add(ConvertToGenericObject2(recordingServer));
		}
		return list.ToArray();
	}

	private static RecordingServer ConvertToRecordingServer(ObjectProperty2[] properties)
	{
		RecordingServer recordingServer = new RecordingServer();
		if (GenericObjectHelper2.Contains(properties, "Id"))
		{
			recordingServer.Id = GenericObjectHelper2.GetProperty<string>(properties, "Id");
		}
		if (GenericObjectHelper2.Contains(properties, "Name"))
		{
			recordingServer.Name = GenericObjectHelper2.GetProperty<string>(properties, "Name");
		}
		if (GenericObjectHelper2.Contains(properties, "Description"))
		{
			recordingServer.Description = GenericObjectHelper2.GetProperty<string>(properties, "Description");
		}
		if (GenericObjectHelper2.Contains(properties, "SynchronizationTime"))
		{
			recordingServer.SynchronizationTime = GenericObjectHelper2.GetProperty<int>(properties, "SynchronizationTime");
		}
		return recordingServer;
	}
}
