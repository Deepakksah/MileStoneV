using System;

namespace VideoOS.Management.VmoClient;

public class MicrophoneDeviceCollection : VmoObjectGuidBasedCollection<MicrophoneDevice>
{
	public MicrophoneDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, MicrophoneDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
