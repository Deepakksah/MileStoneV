using System;
using VideoOS.Server.Admin.Utilities;

namespace VideoOS.Server.CertificateRegistration;

public class IisProgressEventArgs : EventArgs
{
	public IisProgressStep Step { get; }

	public IisProgressEventArgs(IisProgressStep step)
	{
		Step = step;
	}

	public IisProgressEventArgs(SslStatusEventArgs args)
	{
		if (args == null)
		{
			throw new ArgumentNullException("args");
		}
		switch (args.Step)
		{
		case SslStatusStep.CreateSelfSignedCertificate:
			Step = IisProgressStep.CreateSelfSignedCertificate;
			break;
		case SslStatusStep.CreateSslCertificateConfiguration:
			Step = IisProgressStep.CreateSslCertificateConfiguration;
			break;
		case SslStatusStep.DeleteSslCertificateConfiguration:
			Step = IisProgressStep.DeleteSslCertificateConfiguration;
			break;
		case SslStatusStep.CreateSslBinding:
			Step = IisProgressStep.CreateSslBinding;
			break;
		case SslStatusStep.DeleteSslBinding:
			Step = IisProgressStep.DeleteSslBinding;
			break;
		default:
			Step = IisProgressStep.Unknown;
			break;
		}
	}
}
