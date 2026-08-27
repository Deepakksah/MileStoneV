using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Management.Common;
using VideoOS.Management.Server.ConfigurationApi;

namespace VideoOS.Management.Server;

public sealed class DeviceGroup
{
	public string Id { get; set; }

	public string ParentGroupId { get; set; }

	public string Name { get; set; }

	public string Description { get; set; }

	public DeviceType DeviceType { get; set; }

	public bool BuiltIn { get; set; }

	public DateTime LastModified { get; set; }

	public string RelationIds { get; set; }

	public static DeviceGroup QueryDeviceGroupById(IGeminiRequestContext requestContext, string id, DeviceType deviceType = DeviceType.Unknown)
	{
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		if (requestContext.GetSecurityManagerService().HasApplicationPermission(requestContext, "GENERIC_READ"))
		{
			return deviceGroupComponent.QueryDeviceGroup(id);
		}
		return deviceGroupComponent.QueryDeviceGroupNonAdmin(id, deviceType);
	}

	public static bool DeviceGroupExists(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		Validation.CheckObjectId(ref id, "id", allowNullOrEmpty: false);
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		return deviceGroupComponent.QueryDeviceGroup(id) != null;
	}

	public static DeviceGroup[] QueryDeviceGroups(IGeminiRequestContext requestContext, string[] ids)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return QueryDeviceGroupsNoSecurity(requestContext, ids);
	}

	public static DeviceGroup[] QueryDeviceGroupsNoSecurity(IGeminiRequestContext requestContext, string[] ids)
	{
		List<string> list = new List<string>();
		for (int i = 0; i < ids.Length; i++)
		{
			string objectId = ids[i];
			Validation.CheckObjectId(ref objectId, "ids", allowNullOrEmpty: false);
			list.Add(objectId);
		}
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		return deviceGroupComponent.QueryDeviceGroups(list.ToArray());
	}

	public static DeviceGroup[] QueryDeviceGroupsNoSecurity(IGeminiRequestContext requestContext, DeviceType deviceType)
	{
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		return deviceGroupComponent.QueryDeviceGroups(deviceType);
	}

	public static string[] QueryDeviceGroupIds(IGeminiRequestContext requestContext, string parentGroupId, DeviceType deviceType)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		Validation.CheckObjectId(ref parentGroupId, "parentGroupId", allowNullOrEmpty: false);
		return deviceGroupComponent.QueryDeviceGroupIds(parentGroupId, deviceType);
	}

	public static List<Tuple<Guid, Guid>> QueryDeviceGroupParentAndGroupIdsNonAdmin(IGeminiRequestContext requestContext, string parentGroupId, DeviceType deviceType)
	{
		using IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent();
		return deviceGroupComponent.QueryDeviceGroupParentAndGroupIdsNonAdmin(parentGroupId, deviceType);
	}

	public static void UpdateDeviceGroupValidation(IGeminiRequestContext requestContext, string id, string name, string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "Id", allowNullOrEmpty: false);
		Validation.CheckName(name, "Name", allowNull: true);
		Validation.CheckDescription(ref description, "Description", allowNull: true);
	}

	public static DeviceGroup CreateDeviceGroup(IGeminiRequestContext requestContext, string id, string parentId, DeviceType deviceType, string name, string description)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		Validation.CheckObjectId(parentId, "parentId", allowNullOrEmpty: true);
		Validation.CheckName(name, "name", allowNull: false);
		Validation.CheckDescription(ref description, "description", allowNull: true);
		DeviceGroup deviceGroup;
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			deviceGroupComponent.CreateDeviceGroup(id, parentId, deviceType, name, description);
			deviceGroup = deviceGroupComponent.QueryDeviceGroup(id);
		}
		Integration.TryNotifyServerAsync();
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.GroupType, deviceGroup.Id, EventAction.Added, deviceGroup.Name);
		return deviceGroup;
	}

	public static void DeleteDeviceGroup(IGeminiRequestContext requestContext, string id)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(id, "id", allowNullOrEmpty: false);
		DeviceGroup deviceGroup;
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			deviceGroup = deviceGroupComponent.QueryDeviceGroup(id);
			deviceGroupComponent.DeleteDeviceGroup(id);
		}
		Integration.TryNotifyServerAsync();
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.GroupType, deviceGroup.Id, EventAction.Deleted, deviceGroup.Name);
	}

	public static DeviceGroup UpdateDeviceGroup(IGeminiRequestContext requestContext, string id, string name, string description)
	{
		UpdateDeviceGroupValidation(requestContext, id, name, description);
		DeviceGroup deviceGroup;
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			deviceGroupComponent.UpdateDeviceGroup(id, name, description);
			deviceGroup = deviceGroupComponent.QueryDeviceGroup(id);
		}
		Integration.TryNotifyServerAsync();
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.GroupType, deviceGroup.Id, EventAction.Updated, deviceGroup.Name);
		return deviceGroup;
	}

	public static void AddDeviceGroupMembers(IGeminiRequestContext requestContext, string deviceGroupId, string[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref deviceGroupId, "deviceGroupId", allowNullOrEmpty: false);
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		DeviceGroup deviceGroup = null;
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			deviceGroup = deviceGroupComponent.QueryDeviceGroup(deviceGroupId);
			deviceGroupComponent.AddDeviceGroupMembers(deviceGroupId, deviceIds);
		}
		Integration.TryNotifyServerAsync();
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.GroupType, deviceGroup.Id, EventAction.Updated, deviceGroup.Name);
	}

	public static void RemoveDeviceGroupMembers(IGeminiRequestContext requestContext, string deviceGroupId, string[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		Validation.CheckObjectId(ref deviceGroupId, "deviceGroupId", allowNullOrEmpty: false);
		Validation.CheckObjectIdArray(deviceIds, "deviceIds", allowNullOrEmptyIds: false);
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		DeviceGroup deviceGroup = null;
		using (IDeviceGroupComponent deviceGroupComponent = requestContext.CreateDeviceGroupComponent())
		{
			deviceGroup = deviceGroupComponent.QueryDeviceGroup(deviceGroupId);
			deviceGroupComponent.RemoveDeviceGroupMembers(deviceGroupId, deviceIds);
		}
		Integration.TryNotifyServerAsync();
		DeviceTypeRelation deviceTypeRelation = DeviceTypeRelation.Relations.FirstOrDefault((DeviceTypeRelation r) => r.DeviceType == deviceGroup.DeviceType);
		Integration.FireConfigurationChanged(requestContext, deviceTypeRelation.GroupType, deviceGroup.Id, EventAction.Updated, deviceGroup.Name);
	}
}
