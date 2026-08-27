using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common.Dism;

[Serializable]
public class DismException : Exception
{
	[ExcludeFromCodeCoverage]
	public DismException()
	{
	}

	protected DismException(SerializationInfo serializationInfo, StreamingContext streamingContext)
		: base(serializationInfo, streamingContext)
	{
	}

	public DismException(string message)
		: base(message)
	{
	}

	[ExcludeFromCodeCoverage]
	public DismException(string message, Exception inner)
		: base(message, inner)
	{
	}
}
