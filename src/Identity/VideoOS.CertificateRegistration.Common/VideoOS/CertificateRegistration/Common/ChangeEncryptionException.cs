using System;
using System.Runtime.Serialization;

namespace VideoOS.CertificateRegistration.Common;

[Serializable]
public class ChangeEncryptionException : Exception
{
	public ChangeEncryptionException()
	{
	}

	public ChangeEncryptionException(string message)
		: base(message)
	{
	}

	public ChangeEncryptionException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected ChangeEncryptionException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
