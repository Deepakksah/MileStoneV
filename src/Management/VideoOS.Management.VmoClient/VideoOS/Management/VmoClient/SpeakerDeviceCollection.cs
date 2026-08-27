using System;

namespace VideoOS.Management.VmoClient;

public class SpeakerDeviceCollection : VmoObjectGuidBasedCollection<SpeakerDevice>
{
	public SpeakerDeviceCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, SpeakerDevice[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
