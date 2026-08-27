using System;
using VideoOS.Common.Integration;

namespace VideoOS.Server.CommandHandler;

public class TaskCallContextInfo : ICallContextInfo
{
	public string LocationIPAddress { get; private set; }

	public string UserDomainAccountName { get; private set; }

	public string UserSid { get; private set; }

	public TaskCallContextInfo(string locationIpAddress, string userDomainAccountName, string userSid)
	{
		LocationIPAddress = locationIpAddress ?? throw new ArgumentNullException("locationIpAddress");
		UserDomainAccountName = userDomainAccountName ?? throw new ArgumentNullException("userDomainAccountName");
		UserSid = userSid ?? throw new ArgumentNullException("userSid");
	}
}
