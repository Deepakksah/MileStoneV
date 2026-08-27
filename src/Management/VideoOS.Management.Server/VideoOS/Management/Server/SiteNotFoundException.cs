using System;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class SiteNotFoundException : VmoServiceException
{
	public const int SqlErrorCode = 60400;

	protected override int ErrorCodeDefault => 60400;

	public SiteNotFoundException()
		: base(FormatMessage())
	{
	}

	[SqlError(60400)]
	public SiteNotFoundException(DatabaseException ex, DatabaseError databaseError)
		: this()
	{
	}

	private static string FormatMessage()
	{
		return VmoResources.VMO60400;
	}
}
