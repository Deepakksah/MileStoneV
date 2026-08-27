using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;

namespace VideoOS.Platform;

[Serializable]
[ExcludeFromCodeCoverage]
public class CommunicationMIPException : MIPException
{
	public int ErrorNumber { get; set; }

	public int SubErrorNumber { get; set; }

	public int ErrorCode { get; set; }

	public string ExtraMessage { get; set; }

	public override string Message => base.Message + " - " + ExtraMessage;

	public CommunicationMIPException(string message)
		: base(message)
	{
		ErrorCode = 0;
		SubErrorNumber = 0;
		ErrorNumber = 0;
	}

	public CommunicationMIPException(string message, Exception innerException)
		: base(message, innerException)
	{
		ErrorCode = 0;
		SubErrorNumber = 0;
		ErrorNumber = 0;
	}

	public CommunicationMIPException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
		ErrorCode = (int)info.GetValue("ErrorCode", typeof(int));
		SubErrorNumber = (int)info.GetValue("SubErrorNumber", typeof(int));
		ErrorNumber = (int)info.GetValue("ErrorNumber", typeof(int));
		ExtraMessage = (string)info.GetValue("ExtraMessage", typeof(string));
	}

	public override void GetObjectData(SerializationInfo info, StreamingContext context)
	{
		if (info != null)
		{
			info.AddValue("ErrorCode", ErrorCode);
			info.AddValue("SubErrorNumber", SubErrorNumber);
			info.AddValue("ErrorNumber", ErrorNumber);
			info.AddValue("ExtraMessage", ExtraMessage);
		}
		base.GetObjectData(info, context);
	}

	public void FormatExtraMessage()
	{
		string extraMessage = "";
		if (ErrorCode == 10061)
		{
			extraMessage = "UnreachableServerException";
		}
		if (ErrorNumber == 52000)
		{
			extraMessage = "ServiceNotStartedException";
		}
		if (ErrorNumber == 51000)
		{
			extraMessage = ((SubErrorNumber != 51100) ? "UnauthorizedException" : "DualAuthorizationRequiredException");
		}
		if (ErrorNumber == 51100)
		{
			extraMessage = "DualAuthorizationRequiredException";
		}
		if (ErrorNumber == 50000)
		{
			extraMessage = "TimeProfileException";
		}
		if (ErrorNumber == 55001)
		{
			extraMessage = "PtzPresetDuplicateNameException";
		}
		if (ErrorNumber == 55002)
		{
			extraMessage = "PtzPresetUsedInPatrollingProfileException";
		}
		if (ErrorNumber == 54100)
		{
			extraMessage = "DualAuthorizationRejectedException(NotMemberOfAuthorizationRole)";
		}
		if (ErrorNumber == 54000)
		{
			extraMessage = "DualAuthorizationRejectedException(SelfAuthorization)";
		}
		ExtraMessage = extraMessage;
	}
}
