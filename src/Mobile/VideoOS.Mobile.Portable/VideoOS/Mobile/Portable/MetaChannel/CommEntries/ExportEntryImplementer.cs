using System;
using VideoOS.Mobile.Portable.VideoChannel.Params;

namespace VideoOS.Mobile.Portable.MetaChannel.CommEntries;

public class ExportEntryImplementer : IExportEntry
{
	public int State { get; set; }

	public ExportParams Params { get; set; }

	public DateTime QueuedOnUTC { get; set; }

	public DateTime CompletedOnUTC { get; set; }

	public string CameraName { get; set; }

	public string Username { get; set; }

	public string UserHash { get; set; }

	public string UserFullHash { get; set; }

	public bool UserAd { get; set; }

	public string FilePath { get; set; }

	public string FileName { get; set; }

	public string Password { get; set; }

	public long SizeBytes { get; set; }

	public string ThumbsName { get; set; }

	public string HashR { get; set; }

	public string HashU { get; set; }

	public string UserSid { get; set; }

	public string FullUserName { get; set; }

	public ExportInitType ExportInitType { get; set; }

	public Guid ProcessingServerId { get; set; }
}
