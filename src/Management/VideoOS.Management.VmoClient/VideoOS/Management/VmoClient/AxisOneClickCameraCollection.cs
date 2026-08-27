using System;

namespace VideoOS.Management.VmoClient;

public class AxisOneClickCameraCollection : VmoObjectIntBasedCollection<HostedHardware>
{
	public AxisOneClickCameraCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, HostedHardware[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
