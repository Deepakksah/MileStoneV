using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SearchTemplateDeleteException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66051;

	protected override int ErrorCodeDefault => 66051;

	public SearchTemplateDeleteException()
		: base(VmoResources.VMO66051)
	{
	}

	[SqlError(66051)]
	public SearchTemplateDeleteException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected SearchTemplateDeleteException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
