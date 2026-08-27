using System;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication.Internal;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
public abstract class DatabaseResponseDto
{
	public DatabaseOperationDto DatabaseOperationType;

	public DatabaseResponseStatusDto Status;

	public string Message;

	protected void WriteDto(DatabaseResponse databaseResponse)
	{
		if (databaseResponse == null)
		{
			throw new ArgumentNullException("databaseResponse");
		}
		DatabaseOperationType = DtoConvert.ToDatabaseOperationDto(databaseResponse.DatabaseOperationType);
		Status = DtoConvert.ToDatabaseResponseStatusDto(databaseResponse.Status);
		Message = databaseResponse.Message;
	}
}
