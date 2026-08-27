using VideoOS.Common.Interfaces.Communication;

namespace VideoOS.Common.Interfaces;

public interface ICommandLineArgs
{
	bool GetSilentLoginFromCommandLine { get; }

	string GetServerAddressFromCommandLine { get; }

	ConnectionAuthenticationType GetAuthenticationType { get; }

	bool HasAuthenticationType { get; }

	string GetLoginProviderId { get; }

	string GetUserName { get; }

	string GetDomain { get; }

	string GetPassword { get; }

	void SetAllowInsecureConnectionsFromCommandLine();

	IConnectionInfo GetConnectionInfoFromCommandLine(IUriHandler uriHandler);
}
