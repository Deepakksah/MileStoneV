using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadQuotaExceededException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66114;

	protected override int ErrorCodeDefault => 66114;

	public FileUploadQuotaExceededException()
		: base(VmoResources.VMO66114)
	{
	}

	[SqlError(66114)]
	public FileUploadQuotaExceededException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadQuotaExceededException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadQuotaExceededException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
