using System;

namespace VideoOS.Management.VmoClient;

public class CameraDeviceGroupCollection : VmoObjectGuidBasedCollection<CameraDeviceGroup>
{
	public CameraDeviceGroupCollection(LoadMethod loadDeviceGroups, VmoObject parent, bool isInitialized)
		: base((Func<bool, CameraDeviceGroup[]>)loadDeviceGroups.Invoke, parent, isInitialized)
	{
	}
}
