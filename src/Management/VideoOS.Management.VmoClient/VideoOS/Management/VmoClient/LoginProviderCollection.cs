using System;

namespace VideoOS.Management.VmoClient;

public class LoginProviderCollection : VmoObjectGuidBasedCollection<LoginProvider>
{
	public LoginProviderCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, LoginProvider[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
