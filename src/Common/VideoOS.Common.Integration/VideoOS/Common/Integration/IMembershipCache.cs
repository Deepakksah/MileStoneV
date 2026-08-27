using System;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

public interface IMembershipCache
{
	bool IsMember(Func<IIdentityCacheDataProvider> getDataProvider, string userSid, string groupSid);

	string[] MemberOf(Func<IIdentityCacheDataProvider> getDataProvider, string userSid, QueryMembership queryMembership);

	string[] Members(Func<IIdentityCacheDataProvider> getDataProvider, string groupSid, QueryMembership queryMembership);
}
