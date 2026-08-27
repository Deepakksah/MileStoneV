using VideoOS.IM.Server.Registrars.Properties;

namespace VideoOS.IM.Server.Registrars.IIS.SSL;

public interface ISslHelperWrapper
{
	void EnableSsl([NotNull] string websiteId, [NotNull] string virtualDirectoryName);

	void DisableSsl([NotNull] string websiteId, [NotNull] string virtualDirectoryName);
}
