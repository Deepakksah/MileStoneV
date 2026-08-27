using System;

namespace VideoOS.Management.VmoClient;

public class UserDefinedEventCollection : VmoObjectGuidBasedCollection<UserDefinedEvent>
{
	public UserDefinedEventCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, UserDefinedEvent[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
