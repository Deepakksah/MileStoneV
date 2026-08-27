using System;
using System.Runtime.Serialization;

namespace VideoOS.Installer.RecorderTaskPlugin;

[Serializable]
public class PerformanceCounterCategoryException : Exception
{
	public PerformanceCounterCategoryException()
	{
	}

	public PerformanceCounterCategoryException(string message)
		: base(message)
	{
	}

	public PerformanceCounterCategoryException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PerformanceCounterCategoryException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
