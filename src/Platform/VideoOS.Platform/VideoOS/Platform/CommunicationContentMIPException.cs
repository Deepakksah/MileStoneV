using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public sealed class CommunicationContentMIPException : MIPException
{
	public int Status { get; private set; }

	public CommunicationContentMIPException(string message)
		: base(message)
	{
	}

	public CommunicationContentMIPException(int status, string message)
		: base(message)
	{
		Status = status;
	}

	public CommunicationContentMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	public CommunicationContentMIPException(int status, string message, Exception innerException)
		: base(message, innerException)
	{
	}

	private CommunicationContentMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		Status = (int)info.GetValue("Status", typeof(int));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
		info.AddValue("Status", Status);
	}
}
