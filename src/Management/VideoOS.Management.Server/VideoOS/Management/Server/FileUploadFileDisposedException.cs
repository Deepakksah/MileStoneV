using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadFileDisposedException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66111;

	protected override int ErrorCodeDefault => 66111;

	public FileUploadFileDisposedException()
		: base(VmoResources.VMO66111)
	{
	}

	[SqlError(66111)]
	public FileUploadFileDisposedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadFileDisposedException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadFileDisposedException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
