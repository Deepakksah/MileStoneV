using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadCannotCreateFileException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66112;

	protected override int ErrorCodeDefault => 66112;

	public FileUploadCannotCreateFileException()
		: base(VmoResources.VMO66112)
	{
	}

	[SqlError(66112)]
	public FileUploadCannotCreateFileException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadCannotCreateFileException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadCannotCreateFileException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
