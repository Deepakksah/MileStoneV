using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SiteMasterAlreadyAttachedException : VmoServiceException
{
	public const int SqlErrorCode = 60401;

	protected override int ErrorCodeDefault => 60401;

	public SiteMasterAlreadyAttachedException()
		: base(FormatMessage())
	{
	}

	[SqlError(60401)]
	public SiteMasterAlreadyAttachedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	private static string FormatMessage()
	{
		return VmoResources.VMO60401;
	}
}
