using System;
using System.Collections.Generic;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ServerEndpointComparer : IEqualityComparer<ServerEndpoint>
{
	public bool Equals(ServerEndpoint x, ServerEndpoint y)
	{
		if (x.ConnectionString.Equals(y.ConnectionString, StringComparison.CurrentCultureIgnoreCase))
		{
			return x.Type.Equals(y.Type);
		}
		return false;
	}

	public int GetHashCode(ServerEndpoint obj)
	{
		return obj.ConnectionString.GetHashCode() ^ obj.Type.GetHashCode();
	}
}
