using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadInvalidChecksumException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66110;

	protected override int ErrorCodeDefault => 66110;

	public FileUploadInvalidChecksumException()
		: base(VmoResources.VMO66110)
	{
	}

	[SqlError(66110)]
	public FileUploadInvalidChecksumException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadInvalidChecksumException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadInvalidChecksumException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
