using System;
using System.Runtime.Remoting.Lifetime;

namespace VideoOS.ServerConfigurator.Common;

public class InfiniteSponsor : MarshalByRefObject, ISponsor
{
	public override object InitializeLifetimeService()
	{
		return null;
	}

	public TimeSpan Renewal(ILease lease)
	{
		return TimeSpan.FromMinutes(10.0);
	}
}
