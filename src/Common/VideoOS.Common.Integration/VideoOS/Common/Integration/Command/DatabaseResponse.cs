using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[KnownType(typeof(DatabaseResponseDiskUsage))]
[KnownType(typeof(DatabaseResponseDiskUsages))]
[KnownType(typeof(DatabaseResponseArchiveInformation))]
public abstract class DatabaseResponse : CommonResponseBase
{
	[DataMember]
	public DatabaseOperation DatabaseOperationType { get; private set; }

	[DataMember]
	public DatabaseResponseStatus Status { get; private set; }

	[DataMember]
	public string Message { get; private set; }

	public DatabaseResponse(DatabaseOperation databaseOperationType)
	{
		DatabaseOperationType = databaseOperationType;
		Status = DatabaseResponseStatus.Success;
		Message = string.Empty;
	}

	public DatabaseResponse(DatabaseOperation databaseOperationType, DatabaseResponseStatus status, string message)
	{
		DatabaseOperationType = databaseOperationType;
		Status = status;
		Message = message;
	}

	public override string ToString()
	{
		return string.Format(CultureInfo.InvariantCulture, "{0} ({1} - {2})", GetType().Name, DatabaseOperationType, Status);
	}
}
