using System;
using System.Globalization;
using VideoOS.Management.Common;
using VideoOS.Management.Server.Properties;
using VideoOS.Utilities.Database;

namespace VideoOS.Management.Server;

[Serializable]
public class VideoWallPresetCreationException : AdministrationSubsystemException
{
	public const int SqlErrorCode = 60204;

	protected override int ErrorCodeDefault => 60204;

	public VideoWallPresetCreationException(string videoWallPresetName, string videoWallName)
		: base(FormatMessage(videoWallPresetName, videoWallName))
	{
		base.MessageParameters.Add(0, new MessageParameter(0, "VideoWallPresetName", videoWallPresetName));
		base.MessageParameters.Add(1, new MessageParameter(1, "VideoWallName", videoWallName));
	}

	[SqlError(60204)]
	public VideoWallPresetCreationException(DatabaseException ex, DatabaseError databaseError)
		: this(databaseError.ExtractString("name"), databaseError.ExtractString("videoWallName"))
	{
	}

	private static string FormatMessage(string videoWallPresetName, string videoWallName)
	{
		return string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60204, videoWallPresetName, videoWallName);
	}
}
