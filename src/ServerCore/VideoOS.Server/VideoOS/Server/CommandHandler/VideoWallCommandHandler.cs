using System;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.VmoClient;

namespace VideoOS.Server.CommandHandler;

public sealed class VideoWallCommandHandler : ICommandHandler
{
	private static readonly Guid CurrentLayout = Guid.Empty;

	private readonly IVmoClientFactory _clientFactory;

	public VideoWallCommandHandler(IVmoClientFactory clientFactory)
	{
		_clientFactory = clientFactory;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is VideoWallCommand videoWallCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			VmoClientsPool.VmoTimedClient connectedClient = _clientFactory.GetConnectedClient();
			try
			{
				int tickCount = Environment.TickCount;
				Monitor monitor = null;
				switch (videoWallCommand.OperationType)
				{
				case VideoWallOperation.SetPreset:
				{
					VideoWallPreset videoWallPreset = null;
					try
					{
						videoWallPreset = connectedClient.Repositories.GetObjectById<VideoWallPreset>(videoWallCommand.PresetId, refreshCachedObjects: false);
					}
					catch (Exception ex4)
					{
						Server.Instance.Logger.LogWarning("Failed to execute VideoWall command, VideoWall '{VideoWallId}' Preset '{PresetId}'. Error: {ExceptionMessage}", videoWallCommand.VideoWallId, videoWallCommand.PresetId, ex4.Message);
						return;
					}
					videoWallPreset.Activate();
					break;
				}
				case VideoWallOperation.SetMonitorCameras:
					try
					{
						monitor = connectedClient.Repositories.GetObjectById<Monitor>(videoWallCommand.MonitorId, refreshCachedObjects: false);
					}
					catch (Exception ex2)
					{
						Server.Instance.Logger.LogWarning("Failed to execute VideoWall command, VideoWall '{VideoWallId}' Monitor '{MonitorId}'. Error: {ExceptionMessage}", videoWallCommand.VideoWallId, videoWallCommand.MonitorId, ex2.Message);
						return;
					}
					if (videoWallCommand.LayoutId != CurrentLayout)
					{
						monitor.ActivateCameras(videoWallCommand.DeviceIds, videoWallCommand.DeviceGroupIds, videoWallCommand.ViewPosition, videoWallCommand.LayoutId);
					}
					else
					{
						monitor.ActivateCameras(videoWallCommand.DeviceIds, videoWallCommand.DeviceGroupIds, videoWallCommand.ViewPosition);
					}
					break;
				case VideoWallOperation.SetMonitorText:
					try
					{
						monitor = connectedClient.Repositories.GetObjectById<Monitor>(videoWallCommand.MonitorId, refreshCachedObjects: false);
					}
					catch (Exception ex3)
					{
						Server.Instance.Logger.LogWarning("Failed to execute VideoWall command, VideoWall '{VideoWallId}' Monitor '{MonitorId}'. Error: {ExceptionMessage}", videoWallCommand.VideoWallId, videoWallCommand.MonitorId, ex3.Message);
						return;
					}
					monitor.DisplayText(videoWallCommand.MonitorText, videoWallCommand.ViewPosition, videoWallCommand.LayoutId);
					break;
				case VideoWallOperation.RemoveMonitorCameras:
					try
					{
						monitor = connectedClient.Repositories.GetObjectById<Monitor>(videoWallCommand.MonitorId, refreshCachedObjects: false);
					}
					catch (Exception ex)
					{
						Server.Instance.Logger.LogWarning("Failed to execute VideoWall command, VideoWall '{VideoWallId}' Monitor '{MonitorId}'. Error: {ExceptionMessage}", videoWallCommand.VideoWallId, videoWallCommand.MonitorId, ex.Message);
						return;
					}
					monitor.RemoveCameras(videoWallCommand.DeviceIds, videoWallCommand.DeviceGroupIds);
					break;
				default:
					Server.Instance.Logger.LogError("Failed to execute VideoWall command, operation type not expected !!!");
					break;
				}
				long num = Environment.TickCount - tickCount;
				if (num > 5000)
				{
					Server.Instance.Logger.LogWarning("VideoWall command long execution time = {ExecutionTime}ms", num);
				}
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex6)
			{
				Server.Instance.Logger.LogWarning("Failed to execute VideoWall command (inner). OperationType={OperationType} Error: {Exception}", videoWallCommand.OperationType, ex6);
			}
			finally
			{
				_clientFactory.ReleaseClient(connectedClient);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex8)
		{
			Server.Instance.Logger.LogWarning("Failed to execute VideoWall command (outer). Error: {Exception}", ex8);
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}
}
