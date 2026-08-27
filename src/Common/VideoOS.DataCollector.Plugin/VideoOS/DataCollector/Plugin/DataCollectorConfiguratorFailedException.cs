using System;
using System.Runtime.Serialization;

namespace VideoOS.DataCollector.Plugin;

[Serializable]
public class DataCollectorConfiguratorFailedException : Exception
{
	public DataCollectorConfiguratorFailedException()
	{
	}

	public DataCollectorConfiguratorFailedException(string message)
		: base(message)
	{
	}

	public DataCollectorConfiguratorFailedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected DataCollectorConfiguratorFailedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
