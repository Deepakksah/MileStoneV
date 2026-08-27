using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Threading;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class UpdateEvidenceLockSizesCommandHandler : ICommandHandler, IAdvancedThreadPoolCommandHandler
{
	private readonly IServerEnvironment _serverEnvironment;

	private ILogger Logger => _serverEnvironment.Server.Logger;

	private IServerConfig ServerConfig => _serverEnvironment.ServerConfig;

	private IServer Server => _serverEnvironment.Server;

	private IDataComponentFactory DataComponentFactory => _serverEnvironment.DataComponentFactory;

	private IMessageManager MessageManager => _serverEnvironment.MessageManager;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	SmartThreadPool IAdvancedThreadPoolCommandHandler.ThreadPool { get; } = new SmartThreadPool(5000, 3, 0, 262144)
	{
		Name = "Update evidence lock sizes"
	};

	public ICommandHandler Clone()
	{
		return this;
	}

	public UpdateEvidenceLockSizesCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is UpdateEvidenceLockSizesCommand updateEvidenceLockSizesCommand))
		{
			throw new ArgumentNullException("command");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (!Server.IsClosing)
		{
			if (updateEvidenceLockSizesCommand.EvidenceLockId == Guid.Empty)
			{
				UpdateAllEvidenceLockSizes(callContextInfo);
			}
			else
			{
				UpdateSizesForEvidenceLock(updateEvidenceLockSizesCommand.EvidenceLockId, callContextInfo);
			}
			responseSink.AddResponse(new SuccessResponse());
		}
	}

	private void UpdateSizesForEvidenceLock(Guid evidenceLockId, ICallContextInfo callContextInfo)
	{
		Logger.LogDebug("Start updating size for a single evidence lock. Evidence lock id: {EvidenceLockId}", evidenceLockId);
		UpdateSizesForEvidenceLocks(new Guid[1] { evidenceLockId }.ToList(), callContextInfo);
		Logger.LogDebug("Completed updating size for a single evidence lock. Evidence lock id: {EvidenceLockId}", evidenceLockId);
	}

	private void UpdateAllEvidenceLockSizes(ICallContextInfo callContextInfo)
	{
		Logger.LogDebug("Start updating size for all evidence locks.");
		List<Guid> evidenceLockIds;
		using (IEvidenceLockComponent evidenceLockComponent = DataComponentFactory.CreateEvidenceLockComponent())
		{
			try
			{
				evidenceLockIds = evidenceLockComponent.QueryAllEvidenceLockIds();
			}
			catch (DatabaseException ex)
			{
				Logger.LogWarning("Error while getting evidence lock data from database.\r\nException: {Exception}", ex);
				return;
			}
		}
		UpdateSizesForEvidenceLocks(evidenceLockIds, callContextInfo);
		Logger.LogDebug("Completed updating size for all evidence locks.");
	}

	private void UpdateSizesForEvidenceLocks(List<Guid> evidenceLockIds, ICallContextInfo callContextInfo)
	{
		using IEvidenceLockComponent evidenceLockComponent = DataComponentFactory.CreateEvidenceLockComponent();
		int num = 0;
		int batchSize = ServerConfig.EvidenceLockUpdateConfig.BatchSize;
		while (num < evidenceLockIds.Count && !Server.IsClosing)
		{
			IEnumerable<Guid> evidenceLockIds2 = SkipFast(evidenceLockIds, num).Take(batchSize);
			List<DeviceEvidenceLockData> source;
			try
			{
				source = evidenceLockComponent.QueryDeviceEvidenceLocks(evidenceLockIds2);
			}
			catch (DatabaseException ex)
			{
				Logger.LogWarning("Error while getting evidence lock data from database.\r\nException: {Exception}", ex);
				continue;
			}
			foreach (KeyValuePair<Guid, List<DeviceEvidenceLockData>> item in (from d in source
				group d by d.IdEvidenceLock).ToDictionary((IGrouping<Guid, DeviceEvidenceLockData> g) => g.Key, (IGrouping<Guid, DeviceEvidenceLockData> g) => g.ToList()))
			{
				if (_serverEnvironment.ServerConfig.EvidenceLockUpdateConfig.TimeBetweenEvidenceLocks != Timeout.InfiniteTimeSpan)
				{
					Thread.Sleep(_serverEnvironment.ServerConfig.EvidenceLockUpdateConfig.TimeBetweenEvidenceLocks);
				}
				if (Server.IsClosing)
				{
					return;
				}
				Guid key = item.Key;
				List<DeviceEvidenceLockData> value = item.Value;
				UpdateEvidenceLockSize(key, value, callContextInfo);
			}
			num += batchSize;
			if (Server.IsClosing)
			{
				break;
			}
			if (ServerConfig.EvidenceLockUpdateConfig.TimeBetweenBatches != Timeout.InfiniteTimeSpan)
			{
				Thread.Sleep(_serverEnvironment.ServerConfig.EvidenceLockUpdateConfig.TimeBetweenBatches);
			}
			if (Server.IsClosing)
			{
				break;
			}
		}
	}

	private static IEnumerable<Guid> SkipFast(List<Guid> source, int count)
	{
		for (int i = count; i < source.Count; i++)
		{
			yield return source[i];
		}
	}

	private void UpdateEvidenceLockSize(Guid evidenceLockId, IEnumerable<DeviceEvidenceLockData> deviceEvidenceLocks, ICallContextInfo callContextInfo)
	{
		SynchronizedCollection<QueryDeviceEvidenceLockSizesResponse> allValidResponses = new SynchronizedCollection<QueryDeviceEvidenceLockSizesResponse>();
		Dictionary<Guid, QueryDeviceEvidenceLockSizesCommand> source = PrepareCommands(deviceEvidenceLocks, evidenceLockId);
		Parallel.ForEach(source, delegate(KeyValuePair<Guid, QueryDeviceEvidenceLockSizesCommand> command)
		{
			Guid key = command.Key;
			QueryDeviceEvidenceLockSizesCommand value = command.Value;
			try
			{
				IMessageManagerResponseCollector messageManagerResponseCollector = MessageManager.SendCommand(key, value, callContextInfo);
				ResponseBase nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
				if (nextResponse is QueryDeviceEvidenceLockSizesResponse item)
				{
					allValidResponses.Add(item);
				}
			}
			catch (TimeoutException ex)
			{
				Logger.LogWarning("Timeout while waiting for a response.\r\nException: {Exception}", ex);
			}
			catch (RecorderNotFoundException ex2)
			{
				Logger.LogWarning("Recorder not found. Recorder service id: {ServiceId}.\r\nException:{Exception}", key, ex2);
			}
			catch (NotSupportedByLegacyRecorderException ex3)
			{
				Logger.LogWarning("Legacy recorder does not support UpdateEvidenceLockSizesCommand. Recorder service id: {ServiceId}.\r\nException:{Exception}", key, ex3);
			}
		});
		List<DeviceEvidenceLockSize> list = ProcessResponses(deviceEvidenceLocks, allValidResponses);
		if (list.Count > 0)
		{
			UpdateEvidenceLockSizesInTheDatabase(evidenceLockId, list);
		}
	}

	private Dictionary<Guid, QueryDeviceEvidenceLockSizesCommand> PrepareCommands(IEnumerable<DeviceEvidenceLockData> deviceEvidenceLocks, Guid evidenceLockId)
	{
		Dictionary<Guid, QueryDeviceEvidenceLockSizesCommand> dictionary = new Dictionary<Guid, QueryDeviceEvidenceLockSizesCommand>();
		foreach (Guid item in deviceEvidenceLocks.Select((DeviceEvidenceLockData l) => l.IdRecorder).Distinct())
		{
			Guid guid = RecorderCommManager.QueryActiveRecorderServiceId(item);
			if (guid != Guid.Empty)
			{
				dictionary[guid] = new QueryDeviceEvidenceLockSizesCommand
				{
					EvidenceLockId = evidenceLockId
				};
			}
		}
		return dictionary;
	}

	private static List<DeviceEvidenceLockSize> ProcessResponses(IEnumerable<DeviceEvidenceLockData> deviceEvidenceLocks, IEnumerable<QueryDeviceEvidenceLockSizesResponse> allValidResponses)
	{
		List<DeviceEvidenceLockSize> list = new List<DeviceEvidenceLockSize>();
		Dictionary<Guid, List<DeviceEvidenceLockSize>> dictionary = (from s in allValidResponses.SelectMany((QueryDeviceEvidenceLockSizesResponse r) => r.Sizes)
			where s.Size >= 0
			group s by s.DeviceId).ToDictionary((IGrouping<Guid, DeviceEvidenceLockSize> g) => g.Key, (IGrouping<Guid, DeviceEvidenceLockSize> g) => g.ToList());
		if (!dictionary.Any())
		{
			return list;
		}
		foreach (IGrouping<Guid, DeviceEvidenceLockData> item in from d in deviceEvidenceLocks
			group d by d.IdDevice)
		{
			Guid key = item.Key;
			if (!dictionary.TryGetValue(key, out var value) || !value.Any())
			{
				break;
			}
			if (item.Count() == value.Count())
			{
				long size = value.Sum((DeviceEvidenceLockSize s) => s.Size);
				list.Add(new DeviceEvidenceLockSize(key, size));
			}
		}
		return list;
	}

	private void UpdateEvidenceLockSizesInTheDatabase(Guid evidenceLockId, IEnumerable<DeviceEvidenceLockSize> deviceEvidenceLockSizes)
	{
		try
		{
			using IEvidenceLockComponent evidenceLockComponent = DataComponentFactory.CreateEvidenceLockComponent();
			evidenceLockComponent.UpdateEvidenceLockSize(evidenceLockId, deviceEvidenceLockSizes);
		}
		catch (DatabaseException ex)
		{
			Logger.LogWarning("Error while updating evidence lock size.\r\nException: {Exception}", ex);
		}
	}
}
