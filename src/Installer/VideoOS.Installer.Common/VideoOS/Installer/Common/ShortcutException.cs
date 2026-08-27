using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.Common;

[Serializable]
public class ShortcutException : Exception
{
	public ShortcutException()
	{
	}

	public ShortcutException(string message)
		: base(message)
	{
	}

	public ShortcutException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ShortcutException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
