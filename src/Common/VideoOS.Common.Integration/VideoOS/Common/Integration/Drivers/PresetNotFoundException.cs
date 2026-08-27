using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Drivers;

[Serializable]
public class PresetNotFoundException : Exception
{
	public PresetNotFoundException()
	{
	}

	public PresetNotFoundException(string message)
		: base(message)
	{
	}

	public PresetNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PresetNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
