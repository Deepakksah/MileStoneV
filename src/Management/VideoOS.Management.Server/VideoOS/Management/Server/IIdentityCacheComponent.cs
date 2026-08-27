using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

public interface IIdentityCacheComponent : IIdentityCacheDataProvider, IDisposable
{
	bool UpdateIdentity(Identity identity, bool updateMembers, ICollection<Identity> membersList, DateTime currentUtcTime, bool finalCall, ref bool initialCall, ICollection<string> failingDomains);

	void DeleteIdentity(string sid, bool isAdSync);

	void CleanupDeletedIdentities(int expiryDay);
}
