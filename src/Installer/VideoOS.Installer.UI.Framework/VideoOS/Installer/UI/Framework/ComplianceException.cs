using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.UI.Framework;

[Serializable]
public class ComplianceException : Exception
{
	public ComplianceException()
	{
	}

	public ComplianceException(string message)
		: base(message)
	{
	}

	public ComplianceException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ComplianceException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
