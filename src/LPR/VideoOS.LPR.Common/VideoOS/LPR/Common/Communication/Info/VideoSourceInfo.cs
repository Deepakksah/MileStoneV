using System;

namespace VideoOS.LPR.Common.Communication.Info;

public class VideoSourceInfo
{
	public Guid Id { get; set; }

	public Guid SiteId { get; set; }

	public string Name { get; set; }

	public Guid SourceCameraId { get; set; }
}
