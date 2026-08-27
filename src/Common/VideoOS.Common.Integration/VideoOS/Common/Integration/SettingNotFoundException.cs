using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration;

[Serializable]
public class SettingNotFoundException : Exception
{
	public SettingNotFoundException()
	{
	}

	public SettingNotFoundException(string message)
		: base(message)
	{
	}

	public SettingNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected SettingNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
