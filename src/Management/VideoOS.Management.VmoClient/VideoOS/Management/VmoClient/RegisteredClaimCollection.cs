using System;

namespace VideoOS.Management.VmoClient;

public class RegisteredClaimCollection : VmoObjectIntBasedCollection<RegisteredClaim>
{
	public RegisteredClaimCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, RegisteredClaim[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
