using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi;

public class FailoverServerInformation
{
	public List<ServerGroup> FailoverGroups = new List<ServerGroup>();

	public List<FailoverServer> FailoverServers = new List<FailoverServer>();

	public List<FailoverServer> UnusedFailoverServers = new List<FailoverServer>();

	public FailoverServer HotStandbyForCurrent;

	public FailoverServerInformation(IGeminiRequestContext requestContext, string currentRecorderId)
	{
		ServerGroup[] array = ServerGroup.QueryServerGroupsByParentGroupId(requestContext, ServerGroup.EnumStructureType.FailoverGroup, "6A85FFC3-C134-4338-A072-024817CB5820");
		FailoverServer[] array2;
		foreach (ServerGroup serverGroup in array)
		{
			FailoverGroups.Add(serverGroup);
			array2 = FailoverServer.QueryFailoverServers(requestContext, serverGroup.Id);
			foreach (FailoverServer item in array2)
			{
				FailoverServers.Add(item);
			}
		}
		Guid guid = new Guid(currentRecorderId);
		array2 = FailoverServer.QueryFailoverServers(requestContext, "6A85FFC3-C134-4338-A072-024817CB5820");
		foreach (FailoverServer failoverServer in array2)
		{
			if (failoverServer.HotStandbyRecorderId == Guid.Empty.ToString())
			{
				UnusedFailoverServers.Add(failoverServer);
			}
			else if (new Guid(failoverServer.HotStandbyRecorderId) == guid)
			{
				UnusedFailoverServers.Add(failoverServer);
				HotStandbyForCurrent = failoverServer;
			}
		}
	}

	public bool GroupExist(string id)
	{
		if (id == Guid.Empty.ToString())
		{
			return true;
		}
		return FailoverGroups.FirstOrDefault((ServerGroup g) => string.Equals(g.Id, id, StringComparison.OrdinalIgnoreCase)) != null;
	}

	public bool UnusedServerExist(string id)
	{
		return UnusedFailoverServers.FirstOrDefault((FailoverServer g) => string.Equals(g.Id, id, StringComparison.OrdinalIgnoreCase)) != null;
	}
}
