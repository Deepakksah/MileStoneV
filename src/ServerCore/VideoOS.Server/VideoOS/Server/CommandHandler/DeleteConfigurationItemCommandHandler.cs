using System;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Plugins;

namespace VideoOS.Server.CommandHandler;

public sealed class DeleteConfigurationItemCommandHandler : ICommandHandler
{
	private IServerEnvironment _serverEnvironment;

	private IConfigurationDataSetLoader _dataSetLoader;

	private ILocalCommandProcessor LocalCommandProcessor => _serverEnvironment.LocalCommandProcessor;

	private IServerConfigurationLookup ServerConfigurationLookup => _serverEnvironment.ServerConfigurationLookup;

	public DeleteConfigurationItemCommandHandler(IServerEnvironment serverEnvironment, IConfigurationDataSetLoader dataSetLoader)
	{
		_serverEnvironment = serverEnvironment;
		_dataSetLoader = dataSetLoader;
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is DeleteConfigurationItemCommand command2))
		{
			throw new ArgumentException("Command/Command handler type mismatch", "command");
		}
		if (!(state is ICallContextInfo callContextInfo))
		{
			throw new ArgumentException("Object that implements ICallContextInfo expected.", "state");
		}
		if (responseSink == null)
		{
			throw new ArgumentNullException("responseSink", "A response sink was not passed to the Execute() method.");
		}
		ResponseBase response = ProcessCommand(command2, callContextInfo, responseSink);
		responseSink.AddResponse(response);
	}

	private ResponseBase ProcessCommand(DeleteConfigurationItemCommand command, ICallContextInfo callContextInfo, IResponseSink responseSink)
	{
		if (command.PrimaryIds == null || command.PrimaryIds.Count() == 0)
		{
			return new SuccessResponse();
		}
		using ConfigurationManager configurationManager = new ConfigurationManager(_dataSetLoader);
		ConfigurationDataSet changes;
		using (new SendHeartbeatResponseTimer(sendHeartbeat: true, responseSink))
		{
			if (command.TableTypeName == ConfigurationManager.HardwareDataTable.ToString())
			{
				try
				{
					DeleteHardware(command, configurationManager);
				}
				catch (DatabaseConcurrencyException exception)
				{
					return new ExceptionResponse(exception);
				}
			}
			else if (command.TableTypeName == ConfigurationManager.RecordersDataTable.ToString())
			{
				if (!DeleteRecorders(command, configurationManager, out var errorResponse))
				{
					return errorResponse;
				}
			}
			else
			{
				if (!(command.TableTypeName == ConfigurationManager.DeviceGroupsDataTable.ToString()))
				{
					throw new ArgumentException("DeleteConfigurationItemCommandHandler: TableType not supported.", "command");
				}
				DeleteDeviceGroups(command, configurationManager);
			}
			changes = configurationManager.Dataset.GetChanges();
		}
		ConfigurationManagerDataTransferObject configurationManagerDto = new ConfigurationManagerDataTransferObject(changes);
		UpdateConfigurationManagerCommand command2 = new UpdateConfigurationManagerCommand(configurationManagerDto)
		{
			PersistChanges = true,
			SendHeartbeat = true
		};
		IResponseCollector responseCollector = LocalCommandProcessor.ProcessCommandAsync(command2, callContextInfo);
		ResponseBase nextResponse;
		do
		{
			nextResponse = responseCollector.GetNextResponse(TimeSpan.FromSeconds(60.0));
			responseSink.AddResponse(new HeartBeatResponse());
		}
		while (nextResponse is HeartBeatResponse);
		return nextResponse;
	}

	private static void DeleteHardware(DeleteConfigurationItemCommand cmd, ConfigurationManager configurationManager)
	{
		configurationManager.BeginLoadData();
		Guid guid = new Guid(cmd.SecondaryIds[0]);
		List<Guid> list = cmd.PrimaryIds.Select((string i) => new Guid(i)).ToList();
		if (list.Count > 1)
		{
			Recorder recorder = configurationManager.Recorders[guid];
			recorder.EnsureDevicesLoaded();
			Guid[] hardwareIds = recorder.Hardware.Select((IPHardware h) => h.ID).ToArray();
			configurationManager.EnsureHardwareDefinitionLoadedByHardware(hardwareIds);
			Guid[] deviceIds = (from d in recorder.Hardware.SelectMany((IPHardware h) => h.Devices)
				where d.DeviceType == "Camera"
				select d.ID).ToArray();
			configurationManager.EnsureRecorderPluginDeviceLoadedByRecorderPluginIdAndDeviceIds(WellKnownPluginIds.MotionPluginId, deviceIds);
			configurationManager.EnsureDevicePrivacyMaskItemsLoaded(deviceIds);
			Guid[] deviceIds2 = (from d in recorder.Hardware.SelectMany((IPHardware h) => h.Devices)
				where RegisteredDeviceType.IsRecordingDeviceType(d.DeviceType)
				select d.ID).ToArray();
			configurationManager.EnsureDeviceTrackItemsLoaded(deviceIds2);
		}
		DeleteHardware(configurationManager, guid, list);
		configurationManager.EndLoadData();
	}

	private static void DeleteHardware(ConfigurationManager configurationManager, Guid recorderId, IEnumerable<Guid> hardwareIds)
	{
		foreach (Guid hardwareId in hardwareIds)
		{
			IPHardware iPHardware = configurationManager.FindHardwareByHardwareId(hardwareId);
			if (iPHardware == null)
			{
				continue;
			}
			Recorder recorder = iPHardware.Recorder;
			if (recorder != null)
			{
				if (recorder.ID != recorderId)
				{
					throw new DatabaseConcurrencyException();
				}
				recorder.Hardware.Remove(iPHardware);
			}
		}
	}

	private static void DeleteDeviceGroups(DeleteConfigurationItemCommand cmd, ConfigurationManager configurationManager)
	{
		string[] primaryIds = cmd.PrimaryIds;
		foreach (string g in primaryIds)
		{
			DeviceGroup deviceGroup = configurationManager.FindDeviceGroupByDeviceGroupId(new Guid(g));
			if (deviceGroup != null)
			{
				if (deviceGroup.Parent == null)
				{
					configurationManager.DeviceGroups.Remove(deviceGroup);
				}
				else
				{
					deviceGroup.Parent.DeviceGroups.Remove(deviceGroup);
				}
			}
		}
	}

	private bool DeleteRecorders(DeleteConfigurationItemCommand cmd, ConfigurationManager configurationManager, out ResponseBase errorResponse)
	{
		configurationManager.BeginLoadData();
		string[] primaryIds = cmd.PrimaryIds;
		foreach (string g in primaryIds)
		{
			Guid guid = new Guid(g);
			if (!cmd.UserAcceptedToDeleteEvidenceLockedData && ServerConfigurationLookup.RecorderHasLockedDeviceDataDistribution(guid))
			{
				string recorderName = ServerConfigurationLookup.FindRecorderDisplayName(guid);
				errorResponse = EvidenceLockFailedRemoveResponse.CreateRemoveRecordingServerFailedResponse(recorderName);
				return false;
			}
			Recorder recorder = configurationManager.Recorders[guid];
			recorder.EnsureDevicesLoaded();
			Guid[] hardwareIds = recorder.Hardware.Select((IPHardware h) => h.ID).ToArray();
			configurationManager.EnsureHardwareDefinitionLoadedByHardware(hardwareIds);
			Guid[] deviceIds = (from d in recorder.Hardware.SelectMany((IPHardware h) => h.Devices)
				where d.DeviceType == "Camera"
				select d.ID).ToArray();
			configurationManager.EnsureRecorderPluginDeviceLoadedByRecorderPluginIdAndDeviceIds(WellKnownPluginIds.MotionPluginId, deviceIds);
			configurationManager.EnsureDevicePrivacyMaskItemsLoaded(deviceIds);
			Guid[] deviceIds2 = (from d in recorder.Hardware.SelectMany((IPHardware h) => h.Devices)
				where RegisteredDeviceType.IsRecordingDeviceType(d.DeviceType)
				select d.ID).ToArray();
			configurationManager.EnsureDeviceTrackItemsLoaded(deviceIds2);
			configurationManager.Recorders.Remove(guid);
		}
		configurationManager.EndLoadData();
		errorResponse = null;
		return true;
	}
}
