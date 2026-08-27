using System;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

public interface IIdentityCacheDataProvider : IDisposable
{
	Identity ReadIdentity(string sid);

	Membership[] GetMemberships();
}
