using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public interface IExportEntry
{
	int State { get; }

	ExportParams Params { get; }

	DateTime QueuedOnUTC { get; }

	DateTime CompletedOnUTC { get; }

	string CameraName { get; }

	string Username { get; }

	string UserHash { get; }

	bool UserAd { get; }

	string Password { get; }

	string FilePath { get; }

	string FileName { get; }

	long SizeBytes { get; }

	string ThumbsName { get; }

	string HashR { get; }

	string HashU { get; }

	string UserSid { get; }

	string FullUserName { get; }

	ExportInitType ExportInitType { get; }

	Guid ProcessingServerId { get; }
}
