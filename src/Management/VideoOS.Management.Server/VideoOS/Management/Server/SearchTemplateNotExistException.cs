using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SearchTemplateNotExistException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66052;

	protected override int ErrorCodeDefault => 66052;

	public SearchTemplateNotExistException()
		: base(VmoResources.VMO66052)
	{
	}

	[SqlError(66052)]
	public SearchTemplateNotExistException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected SearchTemplateNotExistException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
