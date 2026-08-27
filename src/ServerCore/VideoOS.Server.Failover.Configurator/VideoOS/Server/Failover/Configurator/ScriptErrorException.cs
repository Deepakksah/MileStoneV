using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Runtime.Serialization;
using System.Security.Permissions;

namespace VideoOS.Server.Failover.Configurator;

[Serializable]
public class ScriptErrorException : Exception
{
	public IEnumerable<ErrorRecord> Errors { get; }

	public ScriptErrorException()
	{
	}

	public ScriptErrorException(IEnumerable<ErrorRecord> errors)
	{
		Errors = errors;
	}

	public ScriptErrorException(string message)
		: base(message)
	{
	}

	public ScriptErrorException(string message, Exception innerException)
		: base(message, innerException)
	{
	}

	protected ScriptErrorException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	[SecurityPermission(SecurityAction.Demand, SerializationFormatter = true)]
	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		base.GetObjectData(info, context);
	}
}
