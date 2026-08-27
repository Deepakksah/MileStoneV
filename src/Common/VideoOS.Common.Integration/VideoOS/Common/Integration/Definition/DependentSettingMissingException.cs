using System;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Definition;

[Serializable]
public class DependentSettingMissingException : Exception
{
	public DependentSettingMissingException()
	{
	}

	public DependentSettingMissingException(string message)
		: base(message)
	{
	}

	public DependentSettingMissingException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected DependentSettingMissingException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
