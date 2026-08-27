using System;

namespace VideoOS.Platform.Proxy.EvidenceLockApi;

public class PatchEvidenceLockDataModel
{
	public string description { get; set; }

	public DateTime? startTime { get; set; }

	public DateTime? endTime { get; set; }

	public DateTime? tagTime { get; set; }

	public string header { get; set; }

	public string reference { get; set; }

	public DateTime? retentionExpire { get; set; }

	public RetentionOptionDataModel retentionOption { get; set; }

	public PathDataModel[] devicePaths { get; set; }
}
