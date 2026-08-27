using System;
using System.Collections.Generic;
using VideoOS.Common.Integration.Communication;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class VideoWallModel : CommandModelBase
{
	public VideoWallOperationDto OperationType { get; set; }

	public Guid VideoWallId { get; set; }

	public Guid MonitorId { get; set; }

	public Guid PresetId { get; set; }

	public Guid ViewId { get; set; }

	public Guid LayoutId { get; set; }

	public int ViewPosition { get; set; }

	public IEnumerable<Guid> DeviceIds { get; set; }

	public IEnumerable<Guid> DeviceGroupIds { get; set; }

	public string MonitorText { get; set; }
}
