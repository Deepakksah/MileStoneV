using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SiteHomeSiteDeletedException : VmoServiceException
{
	public const int SqlErrorCode = 60404;

	protected override int ErrorCodeDefault => 60404;

	public SiteHomeSiteDeletedException()
		: base(FormatMessage())
	{
	}

	[SqlError(60404)]
	public SiteHomeSiteDeletedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	private static string FormatMessage()
	{
		return VmoResources.VMO60404;
	}
}
