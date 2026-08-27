using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public abstract class ServerFileSystemResponse : CommonResponseBase
{
	[DataMember]
	public ServerFileSystemOperation Operation { get; public set; }

	[DataMember]
	public ServerFileSystemResponseStatus Status { get; public set; }

	[DataMember]
	public string Message { get; public set; }

	public ServerFileSystemResponse(ServerFileSystemOperation operation)
	{
		Operation = operation;
		Status = ServerFileSystemResponseStatus.Success;
		Message = string.Empty;
	}

	public ServerFileSystemResponse(ServerFileSystemOperation operation, ServerFileSystemResponseStatus status, string message)
	{
		Operation = operation;
		Status = status;
		Message = message;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1} - {2})", GetType().Name, Operation, Status);
	}
}
