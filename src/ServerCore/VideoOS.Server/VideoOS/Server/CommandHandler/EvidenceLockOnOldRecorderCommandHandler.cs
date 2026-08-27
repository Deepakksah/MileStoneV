using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public sealed class EvidenceLockOnOldRecorderCommandHandler : ICommandHandler
{
	private class RecorderServiceEntry
	{
		public long TaskNo { get; set; }

		public bool UseVer2Command { get; }

		public RecorderServiceEntry(long taskNo, bool UseVer2Command)
		{
			TaskNo = taskNo;
			this.UseVer2Command = UseVer2Command;
		}
	}

	private IServerEnvironment _serverEnvironment;

	private Dictionary<Guid, RecorderServiceEntry> _recorderServices = new Dictionary<Guid, RecorderServiceEntry>();

	private ICallContextInfo _callContextInfo;

	private IServer Server => _serverEnvironment.Server;

	private IServerConfig ServerConfig => _serverEnvironment.ServerConfig;

	private IServerConfigurationLookup ConfigurationLookup => _serverEnvironment.ServerConfigurationLookup;

	private IDataComponentFactory DataComponentFactory => _serverEnvironment.DataComponentFactory;

	private IRecorderCommManager RecorderCommManager => _serverEnvironment.RecorderCommManager;

	private IMessageManager MessageManager => _serverEnvironment.MessageManager;

	private ITaskHandling TaskHandling => _serverEnvironment.TaskHandling;

	public EvidenceLockOnOldRecorderCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		_serverEnvironment = serverEnvironment;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is EvidenceLockOnOldRecorderCommand2))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		_callContextInfo = state as ICallContextInfo;
		if (_callContextInfo == null)
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		RunLoopUntilRecordersAreUpdated();
		Server.Logger.LogInformation("EvidenceLockOnOldRecorderCommandHandler: Done monitoring old version recorder services");
	}

	private void RunLoopUntilRecordersAreUpdated()
	{
		bool flag = true;
		while (flag && !Server.IsClosing)
		{
			try
			{
				_recorderServices.Clear();
				using (IRecordingServerComponent recordingServerComponent = DataComponentFactory.CreateRecordingServerComponent())
				{
					Version minVersionFilter = new Version(20, 2, 0, 1);
					Version cmdVersion2Filter = new Version(23, 1, 0, 1);
					(from rsv in recordingServerComponent.QueryRecorderServiceVersions()
						where minVersionFilter <= rsv.Version && rsv.Version < Server.Version
						select rsv).ToList().ForEach(delegate(RecorderServiceVersion sv)
					{
						_recorderServices.Add(sv.ServiceId, new RecorderServiceEntry(0L, sv.Version >= cmdVersion2Filter));
					});
				}
				Server.Logger.LogInformation("EvidenceLockOnOldRecorderCommandHandler: monitoring {RecorderServices} old version recorder services", _recorderServices.Count());
				DateTime utcNow = DateTime.UtcNow;
				while (_recorderServices.Any() && DateTime.UtcNow - utcNow < TimeSpan.FromHours(24.0))
				{
					Guid[] array = _recorderServices.Keys.ToArray();
					foreach (Guid guid in array)
					{
						if (Server.IsClosing)
						{
							return;
						}
						RecorderCompatibilityInfo recorderCompatibilityStatus = RecorderCommManager.GetRecorderCompatibilityStatus(guid);
						if (recorderCompatibilityStatus != null)
						{
							if (recorderCompatibilityStatus.IsConfigurationCompatible)
							{
								_recorderServices.Remove(guid);
								Server.Logger.LogInformation("EvidenceLockOnOldRecorderCommandHandler: Now monitoring {RecorderServices} old version recorder services", _recorderServices.Count());
							}
							else if (recorderCompatibilityStatus.Connected)
							{
								ProcessAnyDiffgramsForService(guid);
							}
						}
					}
					if (ServerConfig.EvidenceLockOldRecorderConfig.WaitTimeInCheckLoop > TimeSpan.Zero)
					{
						if (Server.IsClosing)
						{
							return;
						}
						Thread.Sleep(ServerConfig.EvidenceLockOldRecorderConfig.WaitTimeInCheckLoop);
					}
				}
				flag = _recorderServices.Any();
			}
			catch (Exception ex)
			{
				Server.Logger.LogError("EvidenceLockOnOldRecorderCommandHandler had unexpected error: {Exception}", ex);
				if (ServerConfig.EvidenceLockOldRecorderConfig.WaitTimeInCheckLoop > TimeSpan.Zero)
				{
					if (Server.IsClosing)
					{
						break;
					}
					Thread.Sleep(ServerConfig.EvidenceLockOldRecorderConfig.WaitTimeInCheckLoop);
				}
			}
		}
	}

	private void ProcessAnyDiffgramsForService(Guid serviceId)
	{
		ServerTaskDto[] pendingTasks = TaskHandling.GetPendingTasks(serviceId, _recorderServices[serviceId].TaskNo);
		ServerTaskDto[] array = pendingTasks;
		foreach (ServerTaskDto serverTaskDto in array)
		{
			bool flag = false;
			switch (serverTaskDto.TaskType)
			{
			case ServerTaskTypeDto.ConfigurationDiffGram:
			{
				using (ConfigurationDataSet configurationDataSet = ReadDataSet(serverTaskDto.TaskXml))
				{
					flag = configurationDataSet.DeviceTrackEvidenceLock.Rows.Count <= 0 || ((!_recorderServices[serviceId].UseVer2Command) ? ProcessDiffgramForRecorderVersion1(configurationDataSet, serviceId) : ProcessDiffgramForRecorderVersion2(configurationDataSet, serviceId));
				}
				break;
			}
			case ServerTaskTypeDto.SecurityDiffGram:
				flag = true;
				break;
			default:
				_recorderServices[serviceId].TaskNo = serverTaskDto.SequenceNumber;
				break;
			}
			if (flag)
			{
				TaskHandling.RemoveTask(serverTaskDto.SequenceNumber);
				_recorderServices[serviceId].TaskNo = serverTaskDto.SequenceNumber;
			}
		}
	}

	private bool ProcessDiffgramForRecorderVersion2(ConfigurationDataSet dataSet, Guid serviceId)
	{
		bool flag = false;
		foreach (ConfigurationDataSet.DeviceTrackEvidenceLockRow item in dataSet.DeviceTrackEvidenceLock)
		{
			EvidenceLockOperation evidenceLockOperation = EvidenceLockOperation.Add;
			evidenceLockOperation = item.RowState switch
			{
				DataRowState.Added => EvidenceLockOperation.Add, 
				DataRowState.Deleted => EvidenceLockOperation.Delete, 
				DataRowState.Modified => EvidenceLockOperation.Edit, 
				_ => throw new InvalidOperationException("Unexpected row state: " + item.RowState), 
			};
			EvidenceLockOnOldRecorderCommand2 cmd = new EvidenceLockOnOldRecorderCommand2
			{
				ServerVersion = Server.Version,
				Operation = evidenceLockOperation,
				IDMarkedData = item.GetIDMarkedData(),
				IDDevice = new Guid(item.GetIDDevice()),
				IDTrack = new Guid(item.GetIDTrack()),
				StartTime = item.GetStartTime(),
				EndTime = item.GetEndTime()
			};
			flag = SendCommandToService(serviceId, cmd);
			if (!flag)
			{
				break;
			}
		}
		return flag;
	}

	private bool ProcessDiffgramForRecorderVersion1(ConfigurationDataSet dataSet, Guid serviceId)
	{
		Dictionary<Guid, List<ConfigurationDataSet.DeviceTrackEvidenceLockRow>> dictionary = (from g in dataSet.DeviceTrackEvidenceLock
			group g by new Guid(g.GetIDDevice())).ToDictionary((IGrouping<Guid, ConfigurationDataSet.DeviceTrackEvidenceLockRow> g) => g.Key, (IGrouping<Guid, ConfigurationDataSet.DeviceTrackEvidenceLockRow> g) => g.ToList());
		try
		{
			foreach (KeyValuePair<Guid, List<ConfigurationDataSet.DeviceTrackEvidenceLockRow>> item in dictionary)
			{
				Guid deviceId = item.Key;
				Guid edgeTrackId = ConfigurationLookup.QueryDeviceEdgeTrackIdByDeviceId(deviceId);
				if (edgeTrackId == Guid.Empty)
				{
					continue;
				}
				List<ConfigurationDataSet.DeviceTrackEvidenceLockRow> source = item.Value.Where((ConfigurationDataSet.DeviceTrackEvidenceLockRow dtel) => new Guid(dtel.GetIDTrack()) == deviceId || new Guid(dtel.GetIDTrack()) == edgeTrackId).ToList();
				if (!source.Any())
				{
					continue;
				}
				ConfigurationDataSet.DeviceTrackEvidenceLockRow firstRow = source.First();
				EvidenceLockOnOldRecorderCommand evidenceLockOnOldRecorderCommand = null;
				if (source.Count() == 2)
				{
					evidenceLockOnOldRecorderCommand = CreateCommandV1ForRecorderWith2Tracks(deviceId, edgeTrackId, firstRow);
				}
				else
				{
					evidenceLockOnOldRecorderCommand = CreateCommandV1ForRecorderWith1Track(deviceId, edgeTrackId, firstRow, serviceId);
					if (evidenceLockOnOldRecorderCommand == null)
					{
						continue;
					}
				}
				if (!SendCommandToService(serviceId, evidenceLockOnOldRecorderCommand))
				{
					return false;
				}
			}
		}
		catch (DatabaseException)
		{
			return false;
		}
		return true;
	}

	private EvidenceLockOnOldRecorderCommand CreateCommandV1ForRecorderWith2Tracks(Guid deviceId, Guid edgeTrackId, ConfigurationDataSet.DeviceTrackEvidenceLockRow firstRow)
	{
		EvidenceLockOperation evidenceLockOperation = EvidenceLockOperation.Add;
		evidenceLockOperation = firstRow.RowState switch
		{
			DataRowState.Added => EvidenceLockOperation.Add, 
			DataRowState.Modified => EvidenceLockOperation.Edit, 
			DataRowState.Deleted => EvidenceLockOperation.Delete, 
			_ => throw new InvalidOperationException("Unexpected row state: " + firstRow.RowState), 
		};
		return new EvidenceLockOnOldRecorderCommand
		{
			ServerVersion = Server.Version,
			Operation = evidenceLockOperation,
			IDMarkedData = firstRow.GetIDMarkedData(),
			IDDevice = deviceId,
			IDEdgeTrack = edgeTrackId,
			StartTime = firstRow.GetStartTime(),
			EndTime = firstRow.GetEndTime(),
			UseDeviceTrack = (evidenceLockOperation != EvidenceLockOperation.Delete),
			UseEdgeTrack = (evidenceLockOperation != EvidenceLockOperation.Delete)
		};
	}

	private EvidenceLockOnOldRecorderCommand CreateCommandV1ForRecorderWith1Track(Guid deviceId, Guid edgeTrackId, ConfigurationDataSet.DeviceTrackEvidenceLockRow firstRow, Guid serviceId)
	{
		Guid guid = new Guid(firstRow.GetIDTrack());
		bool useDeviceTrack = false;
		bool useEdgeTrack = false;
		EvidenceLockOperation operation = EvidenceLockOperation.Add;
		switch (firstRow.RowState)
		{
		case DataRowState.Added:
			operation = EvidenceLockOperation.Add;
			useDeviceTrack = guid == deviceId;
			useEdgeTrack = guid == edgeTrackId;
			break;
		case DataRowState.Modified:
			operation = EvidenceLockOperation.Edit;
			useDeviceTrack = guid == deviceId;
			useEdgeTrack = guid == edgeTrackId;
			break;
		case DataRowState.Deleted:
		{
			Guid guid2 = ConfigurationLookup.FindRecorderIdFromServiceId(serviceId);
			if (guid2 == Guid.Empty)
			{
				return null;
			}
			DeviceDataDistribution[] source = ConfigurationLookup.QueryDeviceDataDistribution(deviceId, guid2);
			bool flag = source.Any((DeviceDataDistribution dd) => dd.TrackId == deviceId);
			bool flag2 = source.Any((DeviceDataDistribution dd) => dd.TrackId == edgeTrackId);
			if ((guid == deviceId) ? flag2 : flag)
			{
				operation = EvidenceLockOperation.Edit;
				useDeviceTrack = guid != deviceId;
				useEdgeTrack = guid != edgeTrackId;
			}
			else
			{
				operation = EvidenceLockOperation.Delete;
				useDeviceTrack = false;
				useEdgeTrack = false;
			}
			break;
		}
		}
		return new EvidenceLockOnOldRecorderCommand
		{
			ServerVersion = Server.Version,
			Operation = operation,
			IDMarkedData = firstRow.GetIDMarkedData(),
			IDDevice = deviceId,
			IDEdgeTrack = edgeTrackId,
			StartTime = firstRow.GetStartTime(),
			EndTime = firstRow.GetEndTime(),
			UseDeviceTrack = useDeviceTrack,
			UseEdgeTrack = useEdgeTrack
		};
	}

	private bool SendCommandToService(Guid serviceId, CommonCommandBase cmd)
	{
		ResponseBase responseBase = new FailResponse();
		try
		{
			IMessageManagerResponseCollector messageManagerResponseCollector = ((!(cmd.GetType() == typeof(EvidenceLockOnOldRecorderCommand2))) ? MessageManager.SendCommand(serviceId, (EvidenceLockOnOldRecorderCommand)cmd, _callContextInfo) : MessageManager.SendCommand(serviceId, (EvidenceLockOnOldRecorderCommand2)cmd, _callContextInfo));
			responseBase = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
		}
		catch (TimeoutException)
		{
		}
		catch (RecorderNotFoundException)
		{
		}
		catch (NotSupportedByLegacyRecorderException ex3)
		{
			Server.Logger.LogError("EvidenceLockOnOldRecorderCommandHandler had unexpected error, removing ServiceId {ServiceId}: {Exception}", serviceId, ex3);
			_recorderServices.Remove(serviceId);
		}
		return responseBase is SuccessResponse;
	}

	private static ConfigurationDataSet ReadDataSet(string dataSetXml)
	{
		ConfigurationDataSet configurationDataSet = new ConfigurationDataSet();
		try
		{
			using TextReader input = new StringReader(dataSetXml);
			using XmlReader reader = XmlReader.Create(input);
			configurationDataSet.ReadXml(reader, XmlReadMode.DiffGram);
			return configurationDataSet;
		}
		catch
		{
			configurationDataSet?.Dispose();
			throw;
		}
	}
}
