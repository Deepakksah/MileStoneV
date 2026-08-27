using System;

namespace VideoOS.Management.VmoClient;

public class AudioMessageCollection : VmoObjectGuidBasedCollection<AudioMessage>
{
	public AudioMessageCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, AudioMessage[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
