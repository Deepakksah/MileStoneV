using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.LicenseInfo;

[Serializable]
public class InvalidFeatureParameterException : Exception, ISerializable
{
	public InvalidFeatureParameterException()
	{
	}

	public InvalidFeatureParameterException(string message)
		: base(message)
	{
	}

	public InvalidFeatureParameterException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected InvalidFeatureParameterException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
