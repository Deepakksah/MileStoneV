using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadCannotOpenFileException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66113;

	protected override int ErrorCodeDefault => 66113;

	public FileUploadCannotOpenFileException()
		: base(VmoResources.VMO66113)
	{
	}

	[SqlError(66113)]
	public FileUploadCannotOpenFileException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadCannotOpenFileException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadCannotOpenFileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
