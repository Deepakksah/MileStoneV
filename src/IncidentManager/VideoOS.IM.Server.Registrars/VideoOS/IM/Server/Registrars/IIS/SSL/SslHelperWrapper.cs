using VideoOS.IM.Server.Registrars.Properties;
using VideoOS.Server.Admin.Utilities;
using VideoOS.Utilities.Validation;

namespace VideoOS.IM.Server.Registrars.IIS.SSL;

public class SslHelperWrapper : ISslHelperWrapper
{
	public void DisableSsl([NotNull] string websiteId, [NotNull] string virtualDirectoryName)
	{
		websiteId.NullCheck("", "DisableSsl", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IIS\\SSL\\SslHelperWrapper.cs", 13);
		virtualDirectoryName.NullCheck("", "DisableSsl", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IIS\\SSL\\SslHelperWrapper.cs", 14);
		SslHelper.DisableSsl(websiteId, virtualDirectoryName);
	}

	public void EnableSsl([NotNull] string websiteId, [NotNull] string virtualDirectoryName)
	{
		websiteId.NullCheck("", "EnableSsl", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IIS\\SSL\\SslHelperWrapper.cs", 22);
		virtualDirectoryName.NullCheck("", "EnableSsl", "C:\\actions-runner\\_work\\XPCO\\XPCO\\IncidentManager\\Binaries\\Server\\prod\\Registrars\\IIS\\SSL\\SslHelperWrapper.cs", 23);
		SslHelper.EnableSsl(websiteId, virtualDirectoryName);
	}
}
