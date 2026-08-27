using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[EntityName("ManagementServer")]
[FunctionHandler("ClientLogOnAllowed")]
[FunctionHandler("ClientLogOnAllowedCurrentUser")]
[FunctionHandler("DeviceSearch")]
public class ServerInformationGeneric : IGenericObjectSupport2, IGenericFunction
{
	public GenericObject2 QueryObject2(IGeminiRequestContext requestContext, string entityName, string id, string[] propertiesToLoad, int clientVersion)
	{
		ServerInformation serverInformation = ServerInformation.GetServerInformation(requestContext);
		if (string.IsNullOrEmpty(serverInformation.PrivateViewGroupId))
		{
			ServerGroup serverGroup = ServerGroup.QueryPrivateViewGroup(requestContext, requestContext.AuthenticatedUserSid);
			serverInformation.PrivateViewGroupId = serverGroup.Id;
		}
		return ConvertToGenericObject2(serverInformation);
	}

	public GenericObject2[] QueryObjects(IGeminiRequestContext requestContext, string[] ids, string[] propertiesToLoad, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public string[] QueryObjectCollectionIds(IGeminiRequestContext requestContext, string entityName, string parentEntityName, string parentId, string[] propertiesToLoad, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 CreateObject2(IGeminiRequestContext requestContext, string entityName, ObjectProperty2[] properties, CustomProperty[] customProperties, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public GenericObject2 UpdateObject2(IGeminiRequestContext requestContext, GenericObject2 genericObject, CustomProperty[] customProperties, int clientVersion)
	{
		string text = string.Empty;
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Name"))
		{
			text = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Name");
			if (text == null)
			{
				text = string.Empty;
			}
		}
		string text2 = string.Empty;
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Description"))
		{
			text2 = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Description");
			if (text2 == null)
			{
				text2 = string.Empty;
			}
		}
		ServerAddress[] addresses = null;
		if (GenericObjectHelper2.Contains(genericObject.Properties, "Addresses"))
		{
			addresses = ServerAddresses.GetFromXml(GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "Addresses")).ToArray();
		}
		return ConvertToGenericObject2(ServerInformation.UpdateServerInformation(requestContext, text, text2, addresses, customProperties));
	}

	public void DeleteObject2(IGeminiRequestContext requestContext, string id, int clientVersion)
	{
		throw new NotSupportedException();
	}

	public static int GetServerStatus()
	{
		return ServerInformation.GetServerStatus();
	}

	private static GenericObject2 ConvertToGenericObject2(ServerInformation serverInformation)
	{
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("Id", serverInformation.Id));
		list.Add(new ObjectProperty2("Addresses", ServerAddresses.GetAsXml(serverInformation.Addresses)));
		list.Add(new ObjectProperty2("ComputerName", serverInformation.ComputerName));
		list.Add(new ObjectProperty2("Description", serverInformation.Description));
		list.Add(new ObjectProperty2("DomainName", serverInformation.DomainName));
		list.Add(new ObjectProperty2("LastModified", serverInformation.LastModified));
		list.Add(new ObjectProperty2("LastStatusHandshake", serverInformation.LastStatusHandshake));
		list.Add(new ObjectProperty2("MasterSiteId", serverInformation.MasterSite));
		list.Add(new ObjectProperty2("Name", serverInformation.Name));
		list.Add(new ObjectProperty2("PhysicalMemory", serverInformation.PhysicalMemory));
		list.Add(new ObjectProperty2("Platform", serverInformation.Platform));
		list.Add(new ObjectProperty2("Processors", serverInformation.Processors));
		list.Add(new ObjectProperty2("ServiceAccount", serverInformation.ServiceAccount));
		list.Add(new ObjectProperty2("SynchronizationStatus", serverInformation.SynchronizationStatus));
		list.Add(new ObjectProperty2("TimeZone", serverInformation.TimeZone));
		list.Add(new ObjectProperty2("Version", serverInformation.Version.ToString()));
		list.Add(new ObjectProperty2("PrivateUserGroupId", serverInformation.PrivateViewGroupId));
		return new GenericObject2("909749AE-88C4-4780-A381-383D691E98FA", "ManagementServer", list.ToArray(), new RelationProperty[0]);
	}

	public GenericObject2[] Execute(IGeminiRequestContext requestContext, string functionName, GenericObject2[] parameters, int clientVersion)
	{
		return functionName switch
		{
			"ClientLogOnAllowed" => ClientLogOnAllowed(requestContext, parameters, current: false), 
			"ClientLogOnAllowedCurrentUser" => ClientLogOnAllowed(requestContext, parameters, current: true), 
			"DeviceSearch" => DeviceSearch(requestContext, parameters), 
			_ => throw VmoExceptionFactory.VMO60311_VmoFunctionNotSupportedException(functionName ?? "<Null>"), 
		};
	}

	private static GenericObject2[] ClientLogOnAllowed(IGeminiRequestContext requestContext, GenericObject2[] parameters, bool current)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		ClientType property = GenericObjectHelper2.GetProperty<ClientType>(genericObject.Properties, "ClientApplicationType");
		string userSid;
		if (current)
		{
			userSid = requestContext.AuthenticatedUserSid;
		}
		else
		{
			string property2 = GenericObjectHelper2.GetProperty<string>(genericObject.Properties, "DomainAccountName");
			userSid = IdentityAccessor.GetSidBySearchFactor(requestContext, SearchFactor.AccountName, property2);
		}
		bool allowClientLogOn = Role.AllowClientLogOn(requestContext, userSid, property);
		return ConvertClientAllowedToGenericObject2s(userSid, allowClientLogOn);
	}

	private static GenericObject2[] ConvertClientAllowedToGenericObject2s(string userSid, bool allowClientLogOn)
	{
		List<ObjectProperty2> list = new List<ObjectProperty2>();
		list.Add(new ObjectProperty2("ClientLogOnAllowed", allowClientLogOn));
		GenericObject2 genericObject = new GenericObject2(userSid, null, list.ToArray(), null);
		return new GenericObject2[1] { genericObject };
	}

	private static GenericObject2[] DeviceSearch(IGeminiRequestContext requestContext, GenericObject2[] parameters)
	{
		GenericObject2 genericObject = parameters.FirstOrDefault();
		Validation.CheckObjectReference(genericObject, "parameters");
		Validation.CheckObjectReference(genericObject.Properties, "properties");
		GenericObject2 genericObject2 = GenericObjectHelper2.DeserializeNestedProperties(genericObject);
		string property = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "DeviceNameContains");
		string property2 = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "DeviceShortNameContains");
		string property3 = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "DeviceDescriptionContains");
		string property4 = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "HardwareAddressContains");
		string property5 = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "DeviceIdEquals");
		string property6 = GenericObjectHelper2.GetProperty<string>(genericObject2.Properties, "HardwareIdEquals");
		bool property7 = GenericObjectHelper2.GetProperty<bool>(genericObject2.Properties, "IncludeDisabledDevices");
		string[] deviceTypes = (from p in GenericObjectHelper2.GetNestedProperties(genericObject2.Properties, "DeviceTypes")
			select p.Value as string).ToArray();
		DeviceBasicInfoContract[] source = Device.QueryDevicesInfoBySearch(requestContext, property, property2, property3, property4, deviceTypes, property5, property6, property7);
		if (!source.Any())
		{
			return Array.Empty<GenericObject2>();
		}
		IEnumerable<string> ids = source.Select((DeviceBasicInfoContract d) => d.HardwareId).Distinct(StringComparer.OrdinalIgnoreCase);
		Hardware[] source2 = Hardware.QueryHardwareByIds(requestContext, ids);
		IEnumerable<string> source3 = source2.Select((Hardware h) => h.RecordingServerId).Distinct(StringComparer.OrdinalIgnoreCase);
		RecordingServer[] source4 = RecordingServer.QueryRecordingServersByIds(requestContext, source3.ToArray());
		return ConvertToGenericObject2("RecordingServer", source4.Select((RecordingServer r) => (Id: r.Id, Name: r.Name, ParentId: ""))).Concat(ConvertToGenericObject2("Hardware", source2.Select((Hardware h) => (Id: h.Id, Name: h.Name, ParentId: h.RecordingServerId)))).Concat(ConvertToGenericObject2("CameraDevice", from d in source
			where d.DeviceType == DeviceType.Camera
			select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId))).Concat(ConvertToGenericObject2("MicrophoneDevice", from d in source
			where d.DeviceType == DeviceType.Microphone
			select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId)))
			.Concat(ConvertToGenericObject2("SpeakerDevice", from d in source
				where d.DeviceType == DeviceType.Speaker
				select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId)))
			.Concat(ConvertToGenericObject2("MetadataDevice", from d in source
				where d.DeviceType == DeviceType.Metadata
				select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId)))
			.Concat(ConvertToGenericObject2("InputDevice", from d in source
				where d.DeviceType == DeviceType.Input
				select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId)))
			.Concat(ConvertToGenericObject2("OutputDevice", from d in source
				where d.DeviceType == DeviceType.Output
				select (Id: d.Id, Name: d.Name, ParentId: d.HardwareId)))
			.ToArray();
	}

	private static GenericObject2[] ConvertToGenericObject2(string entityName, IEnumerable<(string Id, string Name, string ParentId)> items)
	{
		return items.Select(((string Id, string Name, string ParentId) i) => new GenericObject2(i.Id, null, new ObjectProperty2[3]
		{
			new ObjectProperty2("ItemType", entityName),
			new ObjectProperty2("Name", i.Name),
			new ObjectProperty2("ParentId", i.ParentId)
		}, Array.Empty<RelationProperty>())).ToArray();
	}
}
