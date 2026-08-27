#define TRACE
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Rules;
using VideoOS.Rules.Interfaces;

namespace VideoOS.Server.RuleActions;

public sealed class SetMonitorCamerasRuleAction : RuleActionBase
{
	private sealed class SmartWallCmdCommandManager
	{
		private class SmartWallCmdData
		{
			public Guid[] DeviceIds;

			public Guid[] DeviceGroupIds;

			public Guid MonitorId;

			public Guid VideoWallId;

			public int ViewPosition;

			public Guid LayoutId;
		}

		private static readonly IResponseSink _nullResponse = new NullResponseSink();

		private static readonly object _syncLock = new object();

		private bool _isWorking;

		private Queue<SmartWallCmdData> _smartWallDatas = new Queue<SmartWallCmdData>();

		private Thread _smartWallCommandThreadThread;

		private int _processingDelay;

		public void EnqueueChange(Guid videoWallId, Guid monitorId, Guid[] deviceIds, Guid[] deviceGroupIds, int viewPosition, Guid layoutId)
		{
			lock (_syncLock)
			{
				Server.Instance.Logger.LogInformation("SmartWallAction Command Added. WallId={WallId}, MonitorId={MonitorId}, ViewPosition={ViewPosition}, LayoutId={LayoutId}, Devices={Devices}", videoWallId, monitorId, viewPosition, layoutId, string.Join("|", deviceIds));
				if (!_isWorking)
				{
					_processingDelay = ServerConfig.Instance.SmartWallProcessDelay;
					_smartWallCommandThreadThread = new Thread(SmartWallCommandProcessor);
					_smartWallCommandThreadThread.Name = "SmartWallCmdCommandThread";
					_smartWallCommandThreadThread.IsBackground = true;
					_smartWallCommandThreadThread.Start();
					Server.Instance.Logger.LogInformation("SmartWallCmdCommandManager: SmartWallAction Thread Started! ProcessingDelay: {ProcessingDelay} ms", _processingDelay);
				}
				SmartWallCmdData item = new SmartWallCmdData
				{
					VideoWallId = videoWallId,
					MonitorId = monitorId,
					DeviceIds = deviceIds,
					DeviceGroupIds = deviceGroupIds,
					ViewPosition = viewPosition,
					LayoutId = layoutId
				};
				_smartWallDatas.Enqueue(item);
			}
		}

		public void SmartWallCommandProcessor()
		{
			try
			{
				_isWorking = true;
				while (!Server.Instance.IsClosing)
				{
					SmartWallCmdData[] array = null;
					lock (_syncLock)
					{
						array = _smartWallDatas.ToArray();
						_smartWallDatas.Clear();
					}
					if (array != null && array.Length != 0)
					{
						IEnumerable<IGrouping<Tuple<Guid, Guid, Guid, int>, SmartWallCmdData>> enumerable = from d in array
							group d by new Tuple<Guid, Guid, Guid, int>(d.VideoWallId, d.MonitorId, d.LayoutId, d.ViewPosition);
						foreach (IGrouping<Tuple<Guid, Guid, Guid, int>, SmartWallCmdData> item2 in enumerable)
						{
							SmartWallCmdData[] array2 = item2.ToArray();
							if (array2 != null && array2.Length != 0)
							{
								Guid[] array3 = array2.SelectMany((SmartWallCmdData d) => d.DeviceIds).Distinct().ToArray();
								Guid[] deviceGroupIds = array2.SelectMany((SmartWallCmdData d) => d.DeviceGroupIds).Distinct().ToArray();
								SmartWallCmdData smartWallCmdData = array2.FirstOrDefault();
								int viewPosition = smartWallCmdData.ViewPosition;
								Guid item = item2.Key.Item2;
								Guid layoutId = smartWallCmdData.LayoutId;
								Server.Instance.Logger.LogInformation("SmartWallAction Processed: WallId={WallId}, MonitorId={MonitorId}, ViewPosition={ViewPosition}, LayoutId={LayoutId}, Devices={Devices}", item2.Key.Item1, item, viewPosition, layoutId, string.Join("|", array3));
								ICallContextInfo state = Server.Instance.CreateServerServiceCallContextUserInfo();
								VideoWallCommand command = new VideoWallCommand(smartWallCmdData.VideoWallId, item, layoutId, viewPosition, array3.ToArray(), deviceGroupIds);
								CommandProcessor.ProcessAsync(command, state, _nullResponse);
							}
						}
					}
					Thread.Sleep(_processingDelay);
				}
			}
			catch (Exception ex)
			{
				Trace.WriteLine(ex.ToString());
				throw;
			}
			finally
			{
				_isWorking = false;
			}
		}
	}

	private static readonly SmartWallCmdCommandManager _smartWallCmdCommandManager = new SmartWallCmdCommandManager();

	private Guid videoWallId;

	private Guid monitorId;

	private Guid layoutId;

	private int viewPosition;

	private Guid[] deviceIds;

	public override bool HandlesAllActionTargets => true;

	public SetMonitorCamerasRuleAction(IRuleEngine ruleEngine, Guid videoWallId, Guid monitorId, Guid layoutId, int viewPosition, Guid[] siteDeviceIds, TimeSpan delay)
		: base(ruleEngine, delay)
	{
		this.videoWallId = videoWallId;
		this.monitorId = monitorId;
		this.layoutId = layoutId;
		deviceIds = siteDeviceIds;
		this.viewPosition = viewPosition;
	}

	public override void Execute(ActionTargetCollection expandedActionTargets, TriggerEvent triggerEvent)
	{
		if (expandedActionTargets == null)
		{
			throw new ArgumentNullException("expandedActionTargets");
		}
		if (triggerEvent == null)
		{
			throw new ArgumentNullException("triggerEvent");
		}
		List<Guid> list = new List<Guid>(deviceIds);
		foreach (ActionTarget expandedActionTarget in expandedActionTargets)
		{
			if (expandedActionTarget.TargetType == ActionTargetType.Device)
			{
				list.AddRange(expandedActionTarget.TargetItems);
			}
		}
		if (list.Count > 0)
		{
			Server.Instance.Logger.LogInformation("SmartWallAction processed. WallId={WallId}, MonitorId={MonitorId}, ViewPosition={ViewPosition}, LayoutId={LayoutId}, Devices={Devices}", videoWallId, monitorId, viewPosition, layoutId, string.Join("|", deviceIds));
			_smartWallCmdCommandManager.EnqueueChange(videoWallId, monitorId, list.ToArray(), new Guid[0], viewPosition, layoutId);
		}
	}
}
