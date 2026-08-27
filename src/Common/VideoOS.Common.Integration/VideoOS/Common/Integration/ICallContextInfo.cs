namespace VideoOS.Common.Integration;

public interface ICallContextInfo
{
	string LocationIPAddress { get; }

	string UserDomainAccountName { get; }

	string UserSid { get; }
}
