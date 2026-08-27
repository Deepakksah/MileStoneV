using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SiteHomeSiteCantBeDeletedException : VmoServiceException
{
	public const int SqlErrorCode = 60405;

	protected override int ErrorCodeDefault => 60405;

	public SiteHomeSiteCantBeDeletedException()
		: base(FormatMessage())
	{
	}

	[SqlError(60405)]
	public SiteHomeSiteCantBeDeletedException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	private static string FormatMessage()
	{
		return VmoResources.VMO60405;
	}
}
