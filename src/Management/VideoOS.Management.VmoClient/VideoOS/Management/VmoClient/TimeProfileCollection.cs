using System;

namespace VideoOS.Management.VmoClient;

public class TimeProfileCollection : VmoObjectGuidBasedCollection<TimeProfile>
{
	public TimeProfileCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, TimeProfile[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
