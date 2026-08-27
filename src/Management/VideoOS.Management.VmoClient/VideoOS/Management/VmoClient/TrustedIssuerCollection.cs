using System;

namespace VideoOS.Management.VmoClient;

public class TrustedIssuerCollection : VmoObjectIntBasedCollection<TrustedIssuer>
{
	public TrustedIssuerCollection(LoadMethod loadMethod, VmoObject parent, bool isInitialized)
		: base((Func<bool, TrustedIssuer[]>)loadMethod.Invoke, parent, isInitialized)
	{
	}
}
