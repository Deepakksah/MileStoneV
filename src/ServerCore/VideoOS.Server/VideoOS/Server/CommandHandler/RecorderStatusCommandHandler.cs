using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration.Command;
using VideoOS.Management.Common;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Failover;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;
using VideoOS.Utilities.Telemetry;

namespace VideoOS.Server.CommandHandler;

public sealed class RecorderStatusCommandHandler : ICommandHandler
{
	private static readonly ILogger Logger = Telemetry.GetILogger("RecorderStatus");

	private Server _server;

	private FailoverStatusManager _failoverStatusManager;

	private Dictionary<Guid, IResponseSink> _responseSinkMap = new Dictionary<Guid, IResponseSink>();

	private object _syncRoot = new object();

	public RecorderStatusCommandHandler(Server server)
	{
		_server = server;
		_server.RecorderCommManager.RecorderStatusChanged += RecorderCommManager_RecorderStatusChanged;
		_failoverStatusManager = new FailoverStatusManager(new ServerEnvironment());
		_failoverStatusManager.FailoverStatusChanged += FailoverStatusManager_FailoverStatusChanged;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is RecorderStatusCommand))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		try
		{
			lock (_syncRoot)
			{
				Guid key = (Guid)command.InvocationInfo;
				_responseSinkMap[key] = responseSink;
				RecorderStatusInfo[] recorderStatusAllRecorders = _server.RecorderCommManager.GetRecorderStatusAllRecorders();
				responseSink.AddResponse(new RecorderStatusResponse(recorderStatusAllRecorders));
				if (Logger.IsEnabled(LogLevel.Debug))
				{
					StringBuilder stringBuilder = new StringBuilder();
					WriteToStringBuilder(recorderStatusAllRecorders, stringBuilder);
					Logger.LogDebug("Sent RecorderStatusResponse. {RecorderStatusResponse}", stringBuilder.ToString());
				}
				foreach (FailoverStatusResponse item in _failoverStatusManager.CreateFailoverStatusResponseList())
				{
					responseSink.AddResponse(item);
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			string resourceString = ExceptionHandling.GetResourceString(ex2);
			if (string.IsNullOrEmpty(resourceString))
			{
				responseSink.AddResponse(new FailResponse());
			}
			else
			{
				responseSink.AddResponse(new ErrorResponse(0, resourceString));
			}
		}
	}

	private static void WriteToStringBuilder(RecorderStatusInfo[] recorderinfo, StringBuilder sb)
	{
		sb.AppendFormat(CultureInfo.InvariantCulture, "Info list count: {0}", recorderinfo.Length);
		foreach (RecorderStatusInfo recorderStatusInfo in recorderinfo)
		{
			sb.AppendFormat(CultureInfo.InvariantCulture, "\r\n    RecorderStatusResponse. {0}; {1}; {2}; {3}; {4}", recorderStatusInfo.Id, recorderStatusInfo.ServiceId, recorderStatusInfo.Status, recorderStatusInfo.ConnectionStatus, recorderStatusInfo.AttachStatus);
		}
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	private void FailoverStatusManager_FailoverStatusChanged(object sender, FailoverStatusChangedEventArgs e)
	{
		FailoverStatusResponse response = new FailoverStatusResponse(e.FailoverServer.Id, e.CurrentStatus);
		SendResponse(response);
		if (Logger.IsEnabled(LogLevel.Debug))
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "Failover status  : {0}", e.FailoverServer.Id);
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n  current status: {0}", e.CurrentStatus.ToString());
			stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n  current watch items:", e.CurrentStatus.ToString());
			if (e.CurrentStatus.RecorderWatchItems != null)
			{
				RecorderWatchItemDto[] recorderWatchItems = e.CurrentStatus.RecorderWatchItems;
				foreach (RecorderWatchItemDto recorderWatchItemDto in recorderWatchItems)
				{
					stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n    - {0}, {1}, {2}", recorderWatchItemDto.RecorderId, recorderWatchItemDto.RecorderEndPoint, recorderWatchItemDto.Errors.ToString());
				}
			}
			if (e.PreviousStatus == null)
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n  previous status: <NULL>");
			}
			else
			{
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n  previous status: {0}", e.PreviousStatus.ToString());
				stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n  previous watch items:", e.PreviousStatus.ToString());
				if (e.CurrentStatus.RecorderWatchItems != null)
				{
					RecorderWatchItemDto[] recorderWatchItems2 = e.PreviousStatus.RecorderWatchItems;
					foreach (RecorderWatchItemDto recorderWatchItemDto2 in recorderWatchItems2)
					{
						stringBuilder.AppendFormat(CultureInfo.InvariantCulture, "\r\n    - {0}, {1}, {2}", recorderWatchItemDto2.RecorderId, recorderWatchItemDto2.RecorderEndPoint, recorderWatchItemDto2.Errors);
					}
				}
			}
			Logger.LogInformation("{FailoverStatus}", stringBuilder.ToString());
		}
		else
		{
			Logger.LogInformation("Failover status: {FailoverServerId}, {CurrentStatus}", e.FailoverServer.Id, e.CurrentStatus);
		}
	}

	private void RecorderCommManager_RecorderStatusChanged(object sender, RecorderStatusEventArgs e)
	{
		RecorderStatusInfo[] recorderStatus = _server.RecorderCommManager.GetRecorderStatus(e.RecorderId, e.ServiceId);
		List<RecorderStatusInfo> list = new List<RecorderStatusInfo>();
		IEnumerable<IGrouping<Guid, RecorderStatusInfo>> enumerable = from s in recorderStatus
			group s by s.Id;
		foreach (IGrouping<Guid, RecorderStatusInfo> item in enumerable)
		{
			Guid key = item.Key;
			if (item.Count() > 1)
			{
				RecordingServer activeRecorder = null;
				try
				{
					activeRecorder = RecordingServer.QueryActiveRecordingServer(key);
				}
				catch (DatabaseException ex)
				{
					Logger.LogWarning("Failed to send recorder status for recorder. Could not determine the active recorder. Recorder id: {RecorderId}; Exception: {Exception}", key, ex);
				}
				if (activeRecorder != null)
				{
					RecorderStatusInfo recorderStatusInfo = item.FirstOrDefault((RecorderStatusInfo s) => s.ServiceId == activeRecorder.ServiceId);
					if (recorderStatusInfo != null)
					{
						list.Add(recorderStatusInfo);
					}
				}
				else
				{
					Logger.LogInformation("No active recorder service found for recorder to send status for. Recorder Id: {RecorderId}", key);
				}
			}
			else
			{
				list.Add(item.First());
			}
		}
		RecorderStatusInfo[] array = list.ToArray();
		RecorderStatusResponse response = new RecorderStatusResponse(array);
		SendResponse(response);
		StringBuilder stringBuilder = new StringBuilder();
		WriteToStringBuilder(array, stringBuilder);
		Logger.LogInformation("Sent RecorderStatusResponse. {RecorderStatusResponse}", stringBuilder.ToString());
	}

	private void SendResponse(ResponseBase response)
	{
		lock (_syncRoot)
		{
			List<Guid> list = new List<Guid>();
			foreach (KeyValuePair<Guid, IResponseSink> item in _responseSinkMap)
			{
				if (_server.ProxyResponseManager.IsProxyRegistered(item.Key))
				{
					item.Value.AddResponse(response);
				}
				else
				{
					list.Add(item.Key);
				}
			}
			foreach (Guid item2 in list)
			{
				_responseSinkMap.Remove(item2);
			}
		}
	}
}
