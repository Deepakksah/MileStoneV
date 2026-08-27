using System;

namespace VideoOS.Platform.Proxy.EvidenceLockApi;

public class EvidenceLockDataModel
{
	public string description { get; set; }

	public string id { get; set; }

	public DateTime created { get; set; }

	public DateTime lastModified { get; set; }

	public DateTime? startTime { get; set; }

	public DateTime? tagTime { get; set; }

	public DateTime? endTime { get; set; }

	public string header { get; set; }

	public string reference { get; set; }

	public string user { get; set; }

	public DateTime retentionExpire { get; set; }

	public long retentionSize { get; set; }

	public RetentionOptionDataModel retentionOption { get; set; }

	public PathDataModel[] devicePaths { get; set; }

	public RelationsDataModel relations { get; set; }
}
