using System;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
public class VideoWallCommand : CommonCommandBase, ICommandWithSource
{
	public VideoWallOperation OperationType { get; private set; }

	public Guid VideoWallId { get; private set; }

	public Guid MonitorId { get; private set; }

	public Guid PresetId { get; private set; }

	public Guid ViewId { get; private set; }

	public Guid LayoutId { get; private set; }

	public int ViewPosition { get; private set; }

	public Guid[] DeviceIds { get; private set; }

	public Guid[] DeviceGroupIds { get; private set; }

	public string MonitorText { get; private set; }

	public Guid SourceId => VideoWallId;

	public VideoWallCommand(VideoWallOperation operationType, Guid videoWallId, Guid monitorId, Guid presetId, Guid viewId, Guid layoutId, int viewPosition, Guid[] deviceIds, Guid[] deviceGroupIds, string monitorText)
	{
		OperationType = operationType;
		VideoWallId = videoWallId;
		MonitorId = monitorId;
		PresetId = presetId;
		ViewId = viewId;
		LayoutId = layoutId;
		ViewPosition = viewPosition;
		DeviceIds = deviceIds;
		DeviceGroupIds = deviceGroupIds;
		MonitorText = monitorText;
	}

	public VideoWallCommand(Guid videoWallId, Guid presetId)
	{
		OperationType = VideoWallOperation.SetPreset;
		VideoWallId = videoWallId;
		PresetId = presetId;
	}

	public VideoWallCommand(Guid videoWallId, Guid monitorId, Guid layoutId, int viewPosition, Guid[] deviceIds, Guid[] deviceGroupIds)
	{
		OperationType = VideoWallOperation.SetMonitorCameras;
		VideoWallId = videoWallId;
		MonitorId = monitorId;
		LayoutId = layoutId;
		ViewPosition = viewPosition;
		DeviceIds = deviceIds;
		DeviceGroupIds = deviceGroupIds;
	}

	public VideoWallCommand(Guid videoWallId, Guid monitorId, Guid viewId)
	{
		OperationType = VideoWallOperation.SetMonitorView;
		VideoWallId = videoWallId;
		MonitorId = monitorId;
		ViewId = viewId;
	}

	public VideoWallCommand(Guid videoWallId, Guid monitorId, string monitorText, Guid layoutId, int viewPosition)
	{
		OperationType = VideoWallOperation.SetMonitorText;
		VideoWallId = videoWallId;
		MonitorId = monitorId;
		MonitorText = monitorText;
		LayoutId = layoutId;
		ViewPosition = viewPosition;
	}

	public VideoWallCommand(Guid videoWallId, Guid monitorId, Guid[] deviceIds, Guid[] deviceGroupIds)
	{
		OperationType = VideoWallOperation.RemoveMonitorCameras;
		VideoWallId = videoWallId;
		MonitorId = monitorId;
		DeviceIds = deviceIds;
		DeviceGroupIds = deviceGroupIds;
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
