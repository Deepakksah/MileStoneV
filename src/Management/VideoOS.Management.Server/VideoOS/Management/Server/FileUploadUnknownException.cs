using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class FileUploadUnknownException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66115;

	protected override int ErrorCodeDefault => 66115;

	public FileUploadUnknownException()
		: base(VmoResources.VMO66115)
	{
	}

	[SqlError(66115)]
	public FileUploadUnknownException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	public FileUploadUnknownException(string message, Exception inner)
		: base(message, inner)
	{
	}

	protected FileUploadUnknownException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
