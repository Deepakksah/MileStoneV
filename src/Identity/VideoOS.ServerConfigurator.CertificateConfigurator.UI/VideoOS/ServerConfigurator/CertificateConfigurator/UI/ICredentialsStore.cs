using System.Net;

namespace VideoOS.ServerConfigurator.CertificateConfigurator.UI;

public interface ICredentialsStore
{
	NetworkCredential Credentials { get; }

	NetworkCredential PromptForCredentials();
}
