using System.Linq;
using Microsoft.Web.Administration;
using VideoOS.IM.Server.Registrars.Properties;
using VideoOS.Utilities.Security;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars.Identities;

public sealed class AppPoolIdentityProvider : IAppPoolIdentityProvider
{
	[NotNull]
	private readonly IAccount _account;

	public AppPoolIdentityProvider([NotNull] IAccount account)
	{
		account.NullCheck("", ".ctor", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\Identities\\AppPoolIdentityProvider.cs", 19);
		_account = account;
	}

	public string GetAppPoolIdentity([NotNull] string appPoolName)
	{
		appPoolName.NullCheck("", "GetAppPoolIdentity", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\Identities\\AppPoolIdentityProvider.cs", 33);
		int processId = new ServerManager().WorkerProcesses.Single((WorkerProcess w) => w.AppPoolName == appPoolName).ProcessId;
		return _account.GetProcessOwner(processId);
	}
}
