using System;
using System.Globalization;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnexpectedDatabaseResultException : GeminiServiceException
{
	protected override int ErrorCodeDefault => 60160;

	public UnexpectedDatabaseResultException()
	{
	}

	public UnexpectedDatabaseResultException(string message, Exception ex)
		: base(message, ex)
	{
	}

	public UnexpectedDatabaseResultException(string storedProcedureName)
		: base(string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60160, storedProcedureName))
	{
		base.EventId = GeminiEventId.UnexpectedDatabaseResult;
	}

	protected UnexpectedDatabaseResultException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
