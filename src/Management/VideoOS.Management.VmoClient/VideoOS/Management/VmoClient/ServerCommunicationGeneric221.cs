using VideoOS.Management.Common;
using VideoOS.Management.VmoClient.Proxy;

namespace VideoOS.Management.VmoClient;

public class ServerCommunicationGeneric221 : ServerCommunicationGenericBase
{
	public ServerCommunicationGeneric221(IVmoClient vmoClient)
		: base(vmoClient)
	{
	}

	public override Identity[] QueryRoleMembersExtended(string roleId, QueryMembership queryMembership, bool includeMappedExternalUsers)
	{
		return _vmoClient.ServerConnection.GetClientProxy<SecurityProxy>().QueryRoleMembersExtended(roleId, queryMembership, includeMappedExternalUsers);
	}
}
