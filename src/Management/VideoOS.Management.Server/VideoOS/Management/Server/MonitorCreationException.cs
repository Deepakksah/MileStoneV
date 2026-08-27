using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class MonitorCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60210;

	protected override int ErrorCodeDefault => 60210;

	public MonitorCreationException(string monitorName, string videoWallName)
		: base(FormatMessage(monitorName, videoWallName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "MonitorName", monitorName));
		base.MessageParameters.Add(1, new MessageParameter(1, "VideoWallName", videoWallName));
	}

	[SqlError(60210)]
	public MonitorCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), databaseError.ExtractString("videoWallName"))
	{
	}

	private static string FormatMessage(string monitorName, string videoWallName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60210, monitorName, videoWallName);
	}
}
