using System;

namespace VideoOS.Management.VmoClient;

public class CameraDeviceCollection : VmoObjectGuidBasedCollection<CameraDevice>
{
	public CameraDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, CameraDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
