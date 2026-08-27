using System;
using System.Runtime.Serialization;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SearchTemplateUpdateException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 66050;

	protected override int ErrorCodeDefault => 66050;

	public SearchTemplateUpdateException()
		: base(VmoResources.VMO66050)
	{
	}

	[SqlError(66050)]
	public SearchTemplateUpdateException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	protected SearchTemplateUpdateException(SerializationInfo info, StreamingContext context)
		: base(info, context)
	{
	}
}
