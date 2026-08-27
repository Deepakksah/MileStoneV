using System;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.InstallerWizard;

[Serializable]
public class MobileServerCertificateRegistrationException : Exception
{
	public MobileServerCertificateRegistrationException()
	{
	}

	public MobileServerCertificateRegistrationException(string message)
		: base(message)
	{
	}

	public MobileServerCertificateRegistrationException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected MobileServerCertificateRegistrationException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
