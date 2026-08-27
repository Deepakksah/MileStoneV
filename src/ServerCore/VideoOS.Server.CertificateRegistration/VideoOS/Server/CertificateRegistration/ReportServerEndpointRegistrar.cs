using VideoOS.CertificateRegistration.Common;
using VideoOS.Common.Integration;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.Server.CertificateRegistration;

public class ReportServerEndpointRegistrar : ServerEndpointRegistrarBase
{
	public ReportServerEndpointRegistrar()
		: this(OemResources.Instance.ReportWebServer.ApplicationName)
	{
	}

	public ReportServerEndpointRegistrar(string serviceName)
		: base(serviceName)
	{
	}

	protected override Status ModifyConfiguration(bool encryptionEnabled, string thumbprint)
	{
		return Status.SuccessStatus();
	}

	public override Status PreExecute()
	{
		return Status.SuccessStatus();
	}

	public override Status PostExecute()
	{
		return Status.SuccessStatus();
	}
}
