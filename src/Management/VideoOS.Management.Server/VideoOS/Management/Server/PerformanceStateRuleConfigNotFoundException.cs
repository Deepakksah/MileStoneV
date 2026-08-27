using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class PerformanceStateRuleConfigNotFoundException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 65003;

	protected override int ErrorCodeDefault => 65003;

	public PerformanceStateRuleConfigNotFoundException()
		: this(VmoResources.VMO65003)
	{
	}

	public PerformanceStateRuleConfigNotFoundException(string message)
		: base(message)
	{
	}

	public PerformanceStateRuleConfigNotFoundException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected PerformanceStateRuleConfigNotFoundException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
