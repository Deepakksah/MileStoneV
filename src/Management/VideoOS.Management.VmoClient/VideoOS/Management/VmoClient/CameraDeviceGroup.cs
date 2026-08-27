using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("CameraDeviceGroup")]
public class CameraDeviceGroup : DeviceGroup
{
	public CameraDeviceGroupCollection CameraDeviceGroupMembers { get; private set; }

	public CameraDeviceCollection CameraDeviceMembers { get; private set; }

	public CameraDeviceGroup ParentCameraDeviceGroup
	{
		get
		{
			if (!string.IsNullOrEmpty(GetPropertyValue<string>("ParentGroupId")))
			{
				return GetObjectById<CameraDeviceGroup>(GetPropertyValue<string>("ParentGroupId"));
			}
			return null;
		}
	}

	public CameraDeviceGroup(string internalId, VmoPropertyCollection objectProperties, VmoRelationCollection relations, IVmoRepositories repositories, bool isInitialized)
		: base(internalId, objectProperties, relations, repositories, isInitialized)
	{
		CameraDeviceMembers = new CameraDeviceCollection(LoadDevices, this, isInitialized);
		CameraDeviceGroupMembers = new CameraDeviceGroupCollection(LoadDeviceGroups, this, isInitialized);
	}

	public CameraDeviceGroup[] LoadDeviceGroups(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<CameraDeviceGroup, CameraDeviceGroup>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new CameraDeviceGroup[0];
		}
	}

	public CameraDevice[] LoadDevices(bool refreshCachedObjects)
	{
		try
		{
			return base.Repositories.GetObjectsByParentId<CameraDeviceGroup, CameraDevice>(base.Id, refreshCachedObjects);
		}
		catch (NotSupportedException)
		{
			return new CameraDevice[0];
		}
	}
}
