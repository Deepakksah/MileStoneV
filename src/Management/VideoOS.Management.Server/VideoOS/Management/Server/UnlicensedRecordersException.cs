using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server;

[Serializable]
public class UnlicensedRecordersException : VmoServiceException
{
	protected override int ErrorCodeDefault => 64036;

	public UnlicensedRecordersException()
		: base(VmoResources.VMO64036)
	{
	}

	protected UnlicensedRecordersException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}

	public UnlicensedRecordersException(string message)
		: base(message)
	{
	}

	public UnlicensedRecordersException(string message, Exception ex)
		: base(message, ex)
	{
	}
}
