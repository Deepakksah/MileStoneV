using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using Microsoft.Extensions.Logging;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Communication;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.Threading;
using VideoOS.Common.Interfaces.Licensing;
using VideoOS.Management.Common.Tasks;
using VideoOS.Server.DataComponents;
using VideoOS.Server.DataComponents.ConfigurationObjects;
using VideoOS.Server.Properties;
using VideoOS.Server.RecorderCommunication;
using VideoOS.Server.ServiceChannel;
using VideoOS.Utilities.Database;

namespace VideoOS.Server.CommandHandler;

public class MoveHardwareTaskCommandHandler : ICommandHandler, IAdvancedThreadPoolCommandHandler
{
	private class SynchrounousProgress : IProgress<int>
	{
		private Action<int> action;

		public SynchrounousProgress(Action<int> action)
		{
			this.action = action;
		}

		public void Report(int value)
		{
			action(value);
		}
	}

	private class ConfiguredServices
	{
		public Guid SourceRecorderServiceId { get; private set; }

		public Guid SourceRecorderHotstandbyServiceId { get; private set; }

		public Guid DestinationRecorderServiceId { get; private set; }

		public Guid DestinationRecorderHotstandbyServiceId { get; private set; }

		public bool SourceRecorderHotstandbyServiceConfigured => SourceRecorderHotstandbyServiceId != Guid.Empty;

		public bool DestinationRecorderHotstandbyServiceConfigured => DestinationRecorderHotstandbyServiceId != Guid.Empty;

		public ConfiguredServices(Guid sourceRecorderServiceId, Guid sourceRecorderHotstandbyServiceId, Guid destinationRecorderServiceId, Guid destinationRecorderHotstandbyServiceId)
		{
			SourceRecorderServiceId = sourceRecorderServiceId;
			SourceRecorderHotstandbyServiceId = sourceRecorderHotstandbyServiceId;
			DestinationRecorderServiceId = destinationRecorderServiceId;
			DestinationRecorderHotstandbyServiceId = destinationRecorderHotstandbyServiceId;
		}

		public Guid[] ConfiguredSourceRecorderServiceIds()
		{
			List<Guid> list = new List<Guid>();
			list.Add(SourceRecorderServiceId);
			if (SourceRecorderHotstandbyServiceConfigured)
			{
				list.Add(SourceRecorderHotstandbyServiceId);
			}
			return list.ToArray();
		}

		public Guid[] ConfiguredDestinationRecorderServiceIds()
		{
			List<Guid> list = new List<Guid>();
			list.Add(DestinationRecorderServiceId);
			if (DestinationRecorderHotstandbyServiceConfigured)
			{
				list.Add(DestinationRecorderHotstandbyServiceId);
			}
			return list.ToArray();
		}
	}

	private class SubTaskResult
	{
		public bool Success { get; private set; }

		public TaskResponseBase TaskResponse { get; private set; }

		public PrepareMoveHardwareResponse PrepareMoveHardwareResponse { get; private set; }

		public static SubTaskResult SuccessResult()
		{
			return new SubTaskResult
			{
				Success = true,
				TaskResponse = null,
				PrepareMoveHardwareResponse = null
			};
		}

		public static SubTaskResult SuccessResult(PrepareMoveHardwareResponse prepareMoveHardwareResponse)
		{
			if (prepareMoveHardwareResponse == null)
			{
				throw new ArgumentNullException("prepareMoveHardwareResponse");
			}
			return new SubTaskResult
			{
				Success = true,
				TaskResponse = null,
				PrepareMoveHardwareResponse = prepareMoveHardwareResponse
			};
		}

		public static SubTaskResult CancelledResult()
		{
			return new SubTaskResult
			{
				Success = false,
				TaskResponse = new TaskStoppedResponse(),
				PrepareMoveHardwareResponse = null
			};
		}

		public static SubTaskResult FailResult(TaskResponseBase taskResponse)
		{
			if (taskResponse == null)
			{
				throw new ArgumentNullException("taskResponse");
			}
			return new SubTaskResult
			{
				Success = false,
				TaskResponse = taskResponse,
				PrepareMoveHardwareResponse = null
			};
		}
	}

	private IServerEnvironment environment;

	private readonly object modifyConfigurationLock = new object();

	private IServerConfig ServerConfig => environment.ServerConfig;

	private IRecorderCommManager RecorderCommManager => environment.RecorderCommManager;

	private IMessageManager MessageManager => environment.MessageManager;

	private IServiceChannelManager ServiceChannelManager => environment.ServiceChannelManager;

	private IConfigurationDataLoader ConfigurationDataLoader => environment.ConfigurationDataLoader;

	private IConfigurationDataSetComponent ConfigurationDataSetComponent => environment.ConfigurationDataSetComponent;

	private IDataComponentFactory DataComponentFactory => environment.DataComponentFactory;

	private ILicenseManager LicenseManager => environment.LicenseManager;

	private ILogger Logger => environment.Server.Logger;

	public SmartThreadPool ThreadPool { get; private set; }

	public MoveHardwareTaskCommandHandler(IServerEnvironment serverEnvironment)
	{
		if (serverEnvironment == null)
		{
			throw new ArgumentNullException("serverEnvironment");
		}
		environment = serverEnvironment;
		ThreadPool = new SmartThreadPool(60000, ServerConfig.MoveHardwareSimultaneousTasks, 0, 262144)
		{
			Name = "Move hardware"
		};
	}

	public ICommandHandler Clone()
	{
		return this;
	}

	public void Execute(CommandBase command, object state, IResponseSink responseSink)
	{
		if (!(command is MoveHardwareTaskCommand cmd))
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
		TaskResponseBase response = ProcessCommand(cmd, responseSink, callContextInfo);
		responseSink.AddResponse(response);
	}

	private TaskResponseBase ProcessCommand(MoveHardwareTaskCommand cmd, IResponseSink responseSink, ICallContextInfo callContextInfo)
	{
		string sessionId = Guid.NewGuid().ToString();
		DateTime utcNow = DateTime.UtcNow;
		int taskProgress = 5;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		SubTaskResult subTaskResult = QueryServiceIds(cmd, out var serviceIds);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		subTaskResult = CheckDestinationRecordingServer(cmd, serviceIds);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		taskProgress = 10;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		subTaskResult = CheckSourceRecordingServer(cmd, serviceIds);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		taskProgress = 20;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		subTaskResult = CheckHardwareNotConfiguredOnTargetRecordingServer(cmd);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		taskProgress = 30;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		subTaskResult = CheckAllTargetStoragesOnline(serviceIds.DestinationRecorderServiceId, cmd.DestinationRecorderId, callContextInfo);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		taskProgress = 35;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		subTaskResult = TryCreateHardwareBlock(cmd.HardwareId, sessionId, out var hardwareBlockId);
		if (!subTaskResult.Success)
		{
			return subTaskResult.TaskResponse;
		}
		taskProgress = 40;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		Logger.LogInformation("Move hardware created block. HardwareId: {HardwareId}; Block id: {BlockId}", cmd.HardwareId, hardwareBlockId);
		List<DeviceTrackId> list = new List<DeviceTrackId>();
		if (!cmd.IgnoreSourceRecordingServer)
		{
			TryNotifyRecorderProcessPendingTasksAsync(cmd.SourceRecorderId);
			subTaskResult = SendPrepareMoveHardwareCommandsAndWaitForResponse(cmd, callContextInfo, responseSink, serviceIds, utcNow, ref taskProgress, 60);
			if (!subTaskResult.Success)
			{
				TryDeleteHardwareBlock(hardwareBlockId);
				return subTaskResult.TaskResponse;
			}
			Logger.LogInformation("Move hardware prepare completed. HardwareId: {HardwareId}; Device tracks with live: '{DeviceTracks}'", cmd.HardwareId, string.Join(", ", subTaskResult.PrepareMoveHardwareResponse.DeviceTracksWithLiveTable.Select((DeviceTrackId t) => string.Format(CultureInfo.InvariantCulture, "{0} ({1})", t.TrackId, t.DeviceId))));
			list.AddRange(subTaskResult.PrepareMoveHardwareResponse.DeviceTracksWithLiveTable);
		}
		taskProgress = 60;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		int driverNumber = 0;
		MoveHardwareTaskResult moveHardwareTaskResult = new MoveHardwareTaskResult();
		lock (modifyConfigurationLock)
		{
			subTaskResult = LoadConfigurationDataSetByHardware(cmd.HardwareId, out var configurationDataSet);
			if (!subTaskResult.Success)
			{
				TryDeleteHardwareBlock(hardwareBlockId);
				return subTaskResult.TaskResponse;
			}
			if (configurationDataSet.Hardware.Count == 0)
			{
				TryDeleteHardwareBlock(hardwareBlockId);
				return CreateTaskFailedResponse(60267, VmoResources.VMO60267);
			}
			MakeMoveHardwareConfigurationDataSetChange(configurationDataSet, cmd.SourceRecorderId, cmd.DestinationRecorderId, cmd.DestinationStorageId, list);
			TouchAllLoadedRows(configurationDataSet, DateTime.UtcNow, new string[3]
			{
				configurationDataSet.DevicePrivacyMask.TableName,
				configurationDataSet.DeviceTrackEvidenceLock.TableName,
				configurationDataSet.DeviceTrackInfo.TableName
			});
			ConfigurationDataSet.HardwareRow hardwareRow = configurationDataSet.Hardware.Single();
			driverNumber = hardwareRow.DriverNumber;
			subTaskResult = LoadRecorderDeviceLists(cmd, configurationDataSet, out var sourceRecorderDeviceIdsBeforeMove, out var destinationRecorderDeviceIdsAfterMove);
			if (!subTaskResult.Success)
			{
				TryDeleteHardwareBlock(hardwareBlockId);
				return subTaskResult.TaskResponse;
			}
			subTaskResult = LoadSecurityDataSetByHardware(cmd.HardwareId, out var securityDataSet);
			if (!subTaskResult.Success)
			{
				TryDeleteHardwareBlock(hardwareBlockId);
				return subTaskResult.TaskResponse;
			}
			taskProgress = 70;
			ReportTaskProgress(responseSink, utcNow, taskProgress);
			ConfigurationDataSet changes = configurationDataSet.GetChanges();
			try
			{
				TryWriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
			}
			catch (DatabaseException ex)
			{
				Logger.LogError("Move hardware failed. Writing to the database failed. HardwareId: {HardwareId}; Exception: {Exception}", cmd.HardwareId, ex);
				TryDeleteHardwareBlock(hardwareBlockId);
				return CreateTaskFailedResponse(60272, VmoResources.VMO60272);
			}
			catch (DBConcurrencyException ex2)
			{
				TryDumpChangesAndCurrentConfiguration(cmd.HardwareId, changes);
				Logger.LogError("Move hardware failed. Writing to the database failed with a concurrency error. HardwareId: {HardwareId}; Exception: {Exception}", cmd.HardwareId, ex2);
				TryDeleteHardwareBlock(hardwareBlockId);
				return CreateTaskFailedResponse(60308, VmoResources.VMO60272);
			}
			ConfigurationDataSet changes2 = MakeLicenseConfigurationDataSetChanges(cmd.SourceRecorderId, cmd.DestinationRecorderId, configurationDataSet);
			try
			{
				TryWriteConfigurationDataSetChangesToDatabase(changes2, callContextInfo);
			}
			catch (DatabaseException ex3)
			{
				Logger.LogWarning("Move hardware completed but licenses are not persisted on database. Writing to the database failed. HardwareId: {HardwareId}; Exception: {Exception}", cmd.HardwareId, ex3);
				TaskWarning item = new TaskWarning(60326, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60326, cmd.HardwareName));
				moveHardwareTaskResult.Warnings.Add(item);
			}
			catch (DBConcurrencyException ex4)
			{
				Logger.LogWarning("Move hardware completed but licenses are not persisted on database. Writing to the database failed with a concurrency error. HardwareId: {HardwareId}; Exception: {Exception}", cmd.HardwareId, ex4);
				TaskWarning item2 = new TaskWarning(60326, string.Format(CultureInfo.InvariantCulture, VmoResources.VMO60326, cmd.HardwareName));
				moveHardwareTaskResult.Warnings.Add(item2);
			}
			taskProgress = 80;
			ReportTaskProgress(responseSink, utcNow, taskProgress);
			CreateSourceAndTargetRecorderDiffgrams(cmd, callContextInfo, serviceIds, configurationDataSet, securityDataSet, sourceRecorderDeviceIdsBeforeMove, destinationRecorderDeviceIdsAfterMove);
			taskProgress = 90;
			ReportTaskProgress(responseSink, utcNow, taskProgress);
		}
		TryDeleteHardwareBlock(hardwareBlockId);
		TryNotifyRecorderProcessPendingTasksAsync(cmd.DestinationRecorderId);
		taskProgress = 95;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		CheckForDriverIssues(driverNumber, serviceIds.SourceRecorderServiceId, serviceIds.DestinationRecorderServiceId, moveHardwareTaskResult);
		ServiceChannelManager.SendHardwareMovedMessageAsync(cmd.HardwareId, cmd.SourceRecorderId, cmd.DestinationRecorderId);
		UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector configurationChangedEventCollector = new UpdateConfigurationManagerCommandHandler.ConfigurationChangedEventCollector(environment.Server.HomeSiteId, environment.Server.EVSEventConsumer);
		configurationChangedEventCollector.AddEvent(cmd.HardwareId.ToString(), cmd.HardwareName, ChangeAction.Updated, "Hardware");
		configurationChangedEventCollector.FireConfigChangedEvent();
		taskProgress = 100;
		ReportTaskProgress(responseSink, utcNow, taskProgress);
		Logger.LogInformation("Move hardware Succeeded. HardwareId: {HardwareId}", cmd.HardwareId);
		return new TaskSuccessResponse(moveHardwareTaskResult.ToXml());
	}

	private ConfigurationDataSet MakeLicenseConfigurationDataSetChanges(Guid sourceRecorderId, Guid destinationRecorderId, ConfigurationDataSet configurationDataSet)
	{
		using ConfigurationManager configurationManager = new ConfigurationManager(new ConfigurationDataSetLoader(ConfigurationDataLoader));
		Guid[] array = new Guid[2] { sourceRecorderId, destinationRecorderId };
		foreach (Guid guid in array)
		{
			byte[] array2 = RequestRecorderLicense(guid);
			License license = null;
			try
			{
				license = configurationManager.Licenses[guid];
			}
			catch (ConfigurationItemNotFoundException)
			{
			}
			if (license == null)
			{
				configurationManager.Licenses.Add(guid, array2);
				continue;
			}
			license.DataAsBytes = array2;
			license.LastModified = DateTime.UtcNow;
		}
		ConfigurationDataSet changes = configurationManager.Dataset.GetChanges();
		configurationDataSet.Merge(changes);
		return changes;
	}

	private void TryWriteConfigurationDataSetChangesToDatabase(ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.EnterWriteLock();
		try
		{
			ConfigurationDataSetComponent.WriteConfigurationDataSetChangesToDatabase(changes, callContextInfo);
		}
		catch
		{
			throw;
		}
		finally
		{
			ConfigurationDataSetComponent.ConfigurationDataSetCacheLock.ExitWriteLock();
		}
	}

	private SubTaskResult CheckHardwareNotConfiguredOnTargetRecordingServer(MoveHardwareTaskCommand cmd)
	{
		HardwareConfigData hardware;
		HardwareConfigData[] source;
		using (IHardwareConfigDataComponent hardwareConfigDataComponent = DataComponentFactory.CreateHardwareConfigDataComponent())
		{
			try
			{
				hardware = hardwareConfigDataComponent.QueryHardwareById(cmd.HardwareId);
				source = hardwareConfigDataComponent.QueryHardwareByRecorderId(cmd.DestinationRecorderId);
			}
			catch (DatabaseException ex)
			{
				Logger.LogWarning("Move hardware task - Move failed. Error: {ExceptionMessage}", ex.Message);
				TaskFailedResponse taskResponse = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
				return SubTaskResult.FailResult(taskResponse);
			}
		}
		if (hardware == null)
		{
			TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60267, VmoResources.VMO60267);
			return SubTaskResult.FailResult(taskResponse2);
		}
		if (source.Any((HardwareConfigData h) => string.Equals(h.MacAddress, hardware.MacAddress, StringComparison.Ordinal)))
		{
			TaskFailedResponse taskResponse3 = CreateTaskFailedResponse(60312, VmoResources.VMO60312);
			return SubTaskResult.FailResult(taskResponse3);
		}
		return SubTaskResult.SuccessResult();
	}

	private SubTaskResult CheckAllTargetStoragesOnline(Guid recordingServerServiceId, Guid recordingServerId, ICallContextInfo callContextInfo)
	{
		DatabaseOperationCommand databaseOperationCommand = new DatabaseOperationCommand();
		databaseOperationCommand.VerifyAllStoragesOnline(recordingServerId);
		IMessageManagerResponseCollector messageManagerResponseCollector = MessageManager.SendCommand(recordingServerServiceId, databaseOperationCommand, callContextInfo);
		ResponseBase nextResponse;
		try
		{
			nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(30.0));
		}
		catch (TimeoutException)
		{
			Logger.LogInformation("Move hardware task - timeout while waiting for response - CheckAllTargetStoragesOnline");
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60328, VmoResources.VMO60328);
			return SubTaskResult.FailResult(taskResponse);
		}
		Logger.LogInformation("Move hardware task - Response received. Response: {Response}", (nextResponse == null) ? "<NULL>" : nextResponse.GetType().Name);
		if (!(nextResponse is SuccessResponse))
		{
			TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60328, VmoResources.VMO60328);
			return SubTaskResult.FailResult(taskResponse2);
		}
		return SubTaskResult.SuccessResult();
	}

	private static void ReportTaskProgress(IResponseSink responseSink, DateTime taskStartTime, int taskProgress)
	{
		responseSink.AddResponse(new TaskProgressResponse(taskStartTime, taskProgress));
	}

	private void CreateSourceAndTargetRecorderDiffgrams(MoveHardwareTaskCommand cmd, ICallContextInfo callContextInfo, ConfiguredServices configuredServices, ConfigurationDataSet configurationDataSet, SecurityDataSet securityDataSet, HashSet<string> sourceRecorderDeviceIdsBeforeMove, HashSet<string> destinationRecorderDeviceIdsAfterMove)
	{
		SecurityDataSet securityDataSet2 = null;
		SecurityDataSet securityDataSet3 = null;
		ConfigurationDataSet configurationDataSet2 = null;
		ConfigurationDataSet configurationDataSet3 = null;
		try
		{
			if (!cmd.IgnoreSourceRecordingServer)
			{
				securityDataSet2 = CreateSourceRecordingServerSecurityDiffgram(securityDataSet);
				configurationDataSet2 = CreateSourceRecordingServerConfigurationDiffgram(cmd.SourceRecorderId, configurationDataSet, sourceRecorderDeviceIdsBeforeMove);
			}
			else
			{
				RequestRecorderLicense(cmd.SourceRecorderId);
			}
			securityDataSet3 = CreateDestinationRecordingServerSecurityDiffgram(securityDataSet);
			configurationDataSet3 = CreateDestinationRecordingServerConfigurationDiffgram(cmd.DestinationRecorderId, configurationDataSet, destinationRecorderDeviceIdsAfterMove);
			if (!cmd.IgnoreSourceRecordingServer)
			{
				Guid[] serviceIds = configuredServices.ConfiguredSourceRecorderServiceIds();
				SendUpdateSystemConfigurationCommandAndWaitForResponse(serviceIds, configurationDataSet2, callContextInfo);
				SendUpdatesToRecorders(serviceIds, securityDataSet2, callContextInfo);
			}
			Guid[] serviceIds2 = configuredServices.ConfiguredDestinationRecorderServiceIds();
			SendUpdatesToRecorders(serviceIds2, securityDataSet3, callContextInfo);
			SendUpdateSystemConfigurationCommandAndWaitForResponse(serviceIds2, configurationDataSet3, callContextInfo);
		}
		finally
		{
			securityDataSet2?.Dispose();
			securityDataSet3?.Dispose();
			configurationDataSet3?.Dispose();
			configurationDataSet2?.Dispose();
		}
	}

	private void TryDumpChangesAndCurrentConfiguration(Guid hardwareId, ConfigurationDataSet changes)
	{
		try
		{
			Logger.LogInformation("Move Hardware: Loading data set to identify concurrency error.");
			ConfigurationDataSet configurationDataSet;
			SubTaskResult subTaskResult = LoadConfigurationDataSetByHardware(hardwareId, out configurationDataSet);
			if (subTaskResult.Success)
			{
				string text = DataSetAsXml(configurationDataSet, XmlWriteMode.IgnoreSchema);
				Logger.LogWarning("Move Hardware: DataSet:\r\n{DataSet}", text);
				string text2 = DataSetAsXml(changes, XmlWriteMode.DiffGram);
				Logger.LogWarning("Move Hardware: Changes:\r\n{Changes}", text2);
			}
		}
		catch (Exception ex)
		{
			Logger.LogWarning("Move Hardware: Failed to dump configuration and diffgram. Exception: {Exception}", ex);
		}
	}

	private static string DataSetAsXml(ConfigurationDataSet dataSet, XmlWriteMode mode)
	{
		StringBuilder stringBuilder = new StringBuilder();
		XmlWriterSettings xmlWriterSettings = new XmlWriterSettings();
		xmlWriterSettings.Encoding = Encoding.Unicode;
		using (XmlWriter xmlWriter = XmlWriter.Create(stringBuilder, xmlWriterSettings))
		{
			xmlWriter.WriteStartDocument(standalone: true);
			dataSet.WriteXml(xmlWriter, mode);
			xmlWriter.WriteEndDocument();
		}
		return stringBuilder.ToString();
	}

	private SubTaskResult LoadSecurityDataSetByHardware(Guid hardwareId, out SecurityDataSet securityDataSet)
	{
		securityDataSet = null;
		try
		{
			securityDataSet = ConfigurationDataSetComponent.LoadSecurityDataSetByHardware(hardwareId);
		}
		catch (DatabaseException ex)
		{
			Logger.LogError("Move hardware failed. Loading data from the database failed. HardwareId: {HardwareId}; Exception: {Exception}", hardwareId, ex);
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
			return SubTaskResult.FailResult(taskResponse);
		}
		return SubTaskResult.SuccessResult();
	}

	private SubTaskResult LoadRecorderDeviceLists(MoveHardwareTaskCommand cmd, ConfigurationDataSet configurationDataSet, out HashSet<string> sourceRecorderDeviceIdsBeforeMove, out HashSet<string> destinationRecorderDeviceIdsAfterMove)
	{
		sourceRecorderDeviceIdsBeforeMove = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		destinationRecorderDeviceIdsAfterMove = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
		try
		{
			using IConfigurationDataComponent configurationDataComponent = DataComponentFactory.CreateConfigurationDataComponent();
			Guid[] array = configurationDataComponent.QueryDeviceIdsByRecorder(cmd.SourceRecorderId);
			Guid[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				Guid guid = array2[i];
				sourceRecorderDeviceIdsBeforeMove.Add(guid.ToString());
			}
			Guid[] array3 = configurationDataComponent.QueryDeviceIdsByRecorder(cmd.DestinationRecorderId);
			Guid[] array4 = array3;
			for (int j = 0; j < array4.Length; j++)
			{
				Guid guid2 = array4[j];
				destinationRecorderDeviceIdsAfterMove.Add(guid2.ToString());
			}
			foreach (ConfigurationDataSet.DevicesRow device in configurationDataSet.Devices)
			{
				destinationRecorderDeviceIdsAfterMove.Add(device.IDDevice);
			}
		}
		catch (DatabaseException ex)
		{
			Logger.LogError("Move hardware failed. Loading data from the database failed. HardwareId: {HardwareId}; Exception: {Exception}", cmd.HardwareId, ex);
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
			return SubTaskResult.FailResult(taskResponse);
		}
		return SubTaskResult.SuccessResult();
	}

	private SubTaskResult LoadConfigurationDataSetByHardware(Guid hardwareId, out ConfigurationDataSet configurationDataSet)
	{
		configurationDataSet = null;
		try
		{
			configurationDataSet = ConfigurationDataSetComponent.LoadConfigurationDataSetByHardware(hardwareId);
			return SubTaskResult.SuccessResult();
		}
		catch (DatabaseException ex)
		{
			Logger.LogError("Move hardware failed. Loading data from the database failed. HardwareId: {HardwareId}; Exception: {Exception}", hardwareId, ex);
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
			return SubTaskResult.FailResult(taskResponse);
		}
	}

	private void CheckForDriverIssues(int driverNumber, Guid sourceRecorderServiceId, Guid destinationRecorderServiceId, MoveHardwareTaskResult result)
	{
		try
		{
			DriverConfigData driverConfigData = DriverConfigData.QueryByRecordingServerServiceAndNumber(DataComponentFactory, sourceRecorderServiceId, driverNumber);
			if (driverConfigData != null)
			{
				DriverConfigData driverConfigData2 = DriverConfigData.QueryByRecordingServerServiceAndNumber(DataComponentFactory, destinationRecorderServiceId, driverNumber);
				if (driverConfigData2 == null)
				{
					TaskWarning item = new TaskWarning(60298, VmoResources.VMO60298);
					result.Warnings.Add(item);
				}
				else if (!string.Equals(driverConfigData.Version, driverConfigData2.Version, StringComparison.Ordinal) || !string.Equals(driverConfigData.XmlVersion, driverConfigData2.XmlVersion, StringComparison.Ordinal))
				{
					TaskWarning item2 = new TaskWarning(60299, VmoResources.VMO60299);
					result.Warnings.Add(item2);
				}
			}
		}
		catch (DatabaseException)
		{
			TaskWarning item3 = new TaskWarning(60300, VmoResources.VMO60300);
			result.Warnings.Add(item3);
		}
	}

	private SubTaskResult CheckDestinationRecordingServer(MoveHardwareTaskCommand cmd, ConfiguredServices configuredServices)
	{
		if (!RecorderCommManager.IsRecorderAttachedAndConnectedNotInFailover(cmd.DestinationRecorderId))
		{
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60261, VmoResources.VMO60261);
			return SubTaskResult.FailResult(taskResponse);
		}
		if (configuredServices.DestinationRecorderHotstandbyServiceConfigured && !RecorderCommManager.IsRecorderAttachedAndConnected(configuredServices.DestinationRecorderHotstandbyServiceId))
		{
			Logger.LogError("Move hardware failed. Destination hot standby recording server not connected. HardwareId: {HardwareId}; Service id: {ServiceId}", cmd.HardwareId, configuredServices.DestinationRecorderHotstandbyServiceId);
			TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60261, VmoResources.VMO60261);
			return SubTaskResult.FailResult(taskResponse2);
		}
		if (RecorderCommManager.IsRecorderIncompatible(cmd.DestinationRecorderId))
		{
			TaskFailedResponse taskResponse3 = CreateTaskFailedResponse(60262, VmoResources.VMO60262);
			return SubTaskResult.FailResult(taskResponse3);
		}
		return SubTaskResult.SuccessResult();
	}

	private SubTaskResult CheckSourceRecordingServer(MoveHardwareTaskCommand cmd, ConfiguredServices configuredServices)
	{
		if (cmd.IgnoreSourceRecordingServer)
		{
			if (RecorderCommManager.IsRecorderNotDisconnected(cmd.SourceRecorderId))
			{
				TaskFailedResponse taskResponse = CreateTaskFailedResponse(60273, VmoResources.VMO60273);
				return SubTaskResult.FailResult(taskResponse);
			}
		}
		else
		{
			if (!RecorderCommManager.IsRecorderAttachedAndConnectedNotInFailover(cmd.SourceRecorderId))
			{
				TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60264, VmoResources.VMO60264);
				return SubTaskResult.FailResult(taskResponse2);
			}
			if (RecorderCommManager.IsRecorderIncompatible(cmd.SourceRecorderId))
			{
				TaskFailedResponse taskResponse3 = CreateTaskFailedResponse(60265, VmoResources.VMO60265);
				return SubTaskResult.FailResult(taskResponse3);
			}
			if (configuredServices.SourceRecorderHotstandbyServiceConfigured && !RecorderCommManager.IsRecorderAttachedAndConnected(configuredServices.SourceRecorderHotstandbyServiceId))
			{
				Logger.LogError("Move hardware failed. Source hot standby recording server not connected. HardwareId: {HardwareId}; Service id: {ServiceId}", cmd.HardwareId, configuredServices.SourceRecorderHotstandbyServiceId);
				TaskFailedResponse taskResponse4 = CreateTaskFailedResponse(60264, VmoResources.VMO60264);
				return SubTaskResult.FailResult(taskResponse4);
			}
		}
		return SubTaskResult.SuccessResult();
	}

	private SubTaskResult QueryServiceIds(MoveHardwareTaskCommand cmd, out ConfiguredServices serviceIds)
	{
		serviceIds = null;
		try
		{
			using IConfigurationDataComponent configurationDataComponent = DataComponentFactory.CreateConfigurationDataComponent();
			Guid sourceRecorderServiceId = configurationDataComponent.QueryServiceIdFromRecorderId(cmd.SourceRecorderId, RecordingServerServiceType.RecordingServer, activeServiceOnly: false);
			Guid sourceRecorderHotstandbyServiceId = configurationDataComponent.QueryServiceIdFromRecorderId(cmd.SourceRecorderId, RecordingServerServiceType.HotStandbyServer, activeServiceOnly: false);
			Guid destinationRecorderServiceId = configurationDataComponent.QueryServiceIdFromRecorderId(cmd.DestinationRecorderId, RecordingServerServiceType.RecordingServer, activeServiceOnly: false);
			Guid destinationRecorderHotstandbyServiceId = configurationDataComponent.QueryServiceIdFromRecorderId(cmd.DestinationRecorderId, RecordingServerServiceType.HotStandbyServer, activeServiceOnly: false);
			serviceIds = new ConfiguredServices(sourceRecorderServiceId, sourceRecorderHotstandbyServiceId, destinationRecorderServiceId, destinationRecorderHotstandbyServiceId);
		}
		catch (DatabaseException ex)
		{
			Logger.LogInformation("Move hardware task - Failed to communicate with the database. Exception: {Exception}", ex);
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
			return SubTaskResult.FailResult(taskResponse);
		}
		if (serviceIds.SourceRecorderServiceId == Guid.Empty)
		{
			Logger.LogInformation("Move hardware task - Failed to find the service id for the main source recorder.");
			TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60266, VmoResources.VMO60266);
			return SubTaskResult.FailResult(taskResponse2);
		}
		if (serviceIds.DestinationRecorderServiceId == Guid.Empty)
		{
			Logger.LogInformation("Move hardware task - Failed to find the service id for the main destination recorder.");
			TaskFailedResponse taskResponse3 = CreateTaskFailedResponse(60263, VmoResources.VMO60263);
			return SubTaskResult.FailResult(taskResponse3);
		}
		return SubTaskResult.SuccessResult();
	}

	private static void MakeMoveHardwareConfigurationDataSetChange(ConfigurationDataSet dataSet, Guid sourceRecorderId, Guid destinationRecorderId, Guid destinationStorageId, IEnumerable<DeviceTrackId> deviceTracksWithLiveTableOnSourceRecorder)
	{
		ConfigurationDataSet.HardwareRow hardwareRow = dataSet.Hardware.Single();
		hardwareRow.IDRecorder = destinationRecorderId.ToString();
		foreach (ConfigurationDataSet.RecordingStorageDeviceRow item in dataSet.RecordingStorageDevice)
		{
			item.recordingstorage_id = destinationStorageId.ToString();
		}
		foreach (ConfigurationDataSet.DevicesRow device in dataSet.Devices)
		{
			if (!RegisteredDeviceType.IsRecordingDeviceType(device.DeviceType))
			{
				continue;
			}
			Guid deviceId = new Guid(device.IDDevice);
			EnumerableRowCollection<ConfigurationDataSet.DeviceDataDistributionRow> enumerableRowCollection = dataSet.DeviceDataDistribution.Where((ConfigurationDataSet.DeviceDataDistributionRow r) => new Guid(r.GetRecorderId()) == destinationRecorderId && new Guid(r.GetDeviceId()) == deviceId);
			foreach (ConfigurationDataSet.DeviceDataDistributionRow item2 in enumerableRowCollection)
			{
				item2.Delete();
			}
			foreach (DeviceTrackId item3 in deviceTracksWithLiveTableOnSourceRecorder.Where((DeviceTrackId d) => d.DeviceId == deviceId))
			{
				dataSet.DeviceDataDistribution.AddDeviceDataDistributionRow(item3.DeviceId.ToString(), sourceRecorderId.ToString(), item3.TrackId);
			}
		}
	}

	private static void TouchAllLoadedRows(DataSet dataSet, DateTime lastModifiedTimeStamp, IEnumerable<string> tablesToIgnore)
	{
		foreach (DataTable table in dataSet.Tables)
		{
			if (tablesToIgnore.Any((string s) => string.Equals(s, table.TableName, StringComparison.Ordinal)))
			{
				continue;
			}
			if (table.Columns.Contains("LastModified"))
			{
				foreach (DataRow row in table.Rows)
				{
					row["LastModified"] = lastModifiedTimeStamp;
				}
				continue;
			}
			if (table.Columns.Contains("last_modified"))
			{
				foreach (DataRow row2 in table.Rows)
				{
					row2["last_modified"] = lastModifiedTimeStamp;
				}
				continue;
			}
			foreach (DataRow row3 in table.Rows)
			{
				if (row3.RowState == DataRowState.Unchanged)
				{
					row3.SetModified();
				}
			}
		}
	}

	private void TryNotifyRecorderProcessPendingTasksAsync(Guid recorderId)
	{
		Task.Factory.StartNew(delegate
		{
			try
			{
				RecorderCommManager.ProcessPendingTasks(recorderId);
			}
			catch (OutOfMemoryException)
			{
				throw;
			}
			catch (Exception ex2)
			{
				Logger.LogWarning("Failed to notify recorder about pending tasks. Error: {Exception}", ex2);
			}
		}, TaskCreationOptions.LongRunning);
	}

	private static SecurityDataSet CreateSourceRecordingServerSecurityDiffgram(SecurityDataSet dataSet)
	{
		SecurityDataSet securityDataSet = dataSet.Copy() as SecurityDataSet;
		foreach (DataTable table in securityDataSet.Tables)
		{
			foreach (DataRow row in table.Rows)
			{
				row.Delete();
			}
		}
		return securityDataSet.GetChanges();
	}

	private static SecurityDataSet CreateDestinationRecordingServerSecurityDiffgram(SecurityDataSet dataSet)
	{
		SecurityDataSet securityDataSet = dataSet.Copy() as SecurityDataSet;
		securityDataSet.AcceptChanges();
		foreach (DataTable table in securityDataSet.Tables)
		{
			foreach (DataRow row in table.Rows)
			{
				row.SetAdded();
			}
		}
		return securityDataSet.GetChanges();
	}

	private static ConfigurationDataSet CreateSourceRecordingServerConfigurationDiffgram(Guid recorderId, ConfigurationDataSet dataSet, HashSet<string> devicesOnRecorder)
	{
		ConfigurationDataSet configurationDataSet = dataSet.Copy() as ConfigurationDataSet;
		ConfigurationDataSet.LicenseRow[] array = configurationDataSet.License.ToArray();
		foreach (ConfigurationDataSet.LicenseRow licenseRow in array)
		{
			if (licenseRow.IDLicense != recorderId && licenseRow.RowState != DataRowState.Deleted)
			{
				licenseRow.Delete();
				licenseRow.AcceptChanges();
			}
		}
		ConfigurationDataSet.HardwareDefinitionRow[] array2 = configurationDataSet.HardwareDefinition.ToArray();
		foreach (ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionRow in array2)
		{
			hardwareDefinitionRow.Delete();
		}
		configurationDataSet.HardwareDefinition.AcceptChanges();
		ConfigurationDataSet.DeviceLinksRow[] array3 = configurationDataSet.DeviceLinks.ToArray();
		foreach (ConfigurationDataSet.DeviceLinksRow deviceLinksRow in array3)
		{
			deviceLinksRow.AcceptChanges();
			if (devicesOnRecorder.Contains(deviceLinksRow.IDDevice) && devicesOnRecorder.Contains(deviceLinksRow.IDDeviceLinked))
			{
				deviceLinksRow.Delete();
			}
		}
		ConfigurationDataSet.DeviceTrackEvidenceLockRow[] array4 = configurationDataSet.DeviceTrackEvidenceLock.ToArray();
		foreach (ConfigurationDataSet.DeviceTrackEvidenceLockRow deviceTrackEvidenceLockRow in array4)
		{
			ConfigurationDataSet.DeviceDataDistributionRow deviceDataDistributionRow = configurationDataSet.DeviceDataDistribution.FindByrecorder_idtrack_id(recorderId.ToString(), deviceTrackEvidenceLockRow.IDTrack);
			if (deviceDataDistributionRow != null)
			{
				deviceTrackEvidenceLockRow.Delete();
				deviceTrackEvidenceLockRow.AcceptChanges();
			}
			else
			{
				deviceTrackEvidenceLockRow.Delete();
			}
		}
		ConfigurationDataSet.DeviceTrackInfoRow[] array5 = configurationDataSet.DeviceTrackInfo.ToArray();
		foreach (ConfigurationDataSet.DeviceTrackInfoRow deviceTrackInfoRow in array5)
		{
			deviceTrackInfoRow.Delete();
		}
		configurationDataSet.DeviceTrackInfo.AcceptChanges();
		foreach (ConfigurationDataSet.DeviceDataDistributionRow item in configurationDataSet.DeviceDataDistribution.Where((ConfigurationDataSet.DeviceDataDistributionRow ddd2) => ddd2.RowState == DataRowState.Added && new Guid(ddd2.recorder_id) == recorderId))
		{
			ConfigurationDataSet.DevicesRow devicesRow = configurationDataSet.Devices.FindByIDDevice(item.device_id);
			ConfigurationDataSet.DeviceTrackRow deviceTrackRow = configurationDataSet.DeviceTrack.FindByIDTrack(item.track_id);
			configurationDataSet.DeviceTrackInfo.AddDeviceTrackInfoRow(recorderId, new Guid(item.device_id), item.track_id, devicesRow.Name, devicesRow.DeviceType, deviceTrackRow.IDTrackDefinition);
		}
		foreach (DataTable table in configurationDataSet.Tables)
		{
			if (string.Equals(table.TableName, configurationDataSet.License.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.HardwareDefinition.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceLinks.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceTrackInfo.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceTrackEvidenceLock.TableName, StringComparison.Ordinal))
			{
				continue;
			}
			foreach (DataRow row in table.Rows)
			{
				if (row.RowState == DataRowState.Added)
				{
					row.AcceptChanges();
				}
				else if (row.RowState != DataRowState.Deleted)
				{
					row.Delete();
				}
			}
		}
		return configurationDataSet.GetChanges();
	}

	private static ConfigurationDataSet CreateDestinationRecordingServerConfigurationDiffgram(Guid recorderId, ConfigurationDataSet dataSet, HashSet<string> devicesOnRecorder)
	{
		ConfigurationDataSet configurationDataSet = dataSet.Copy() as ConfigurationDataSet;
		ConfigurationDataSet.LicenseRow[] array = configurationDataSet.License.ToArray();
		foreach (ConfigurationDataSet.LicenseRow licenseRow in array)
		{
			if (licenseRow.IDLicense != recorderId && licenseRow.RowState != DataRowState.Deleted)
			{
				licenseRow.Delete();
				licenseRow.AcceptChanges();
			}
		}
		configurationDataSet.DeviceLinks.AcceptChanges();
		ConfigurationDataSet.DeviceLinksRow[] array2 = configurationDataSet.DeviceLinks.ToArray();
		foreach (ConfigurationDataSet.DeviceLinksRow deviceLinksRow in array2)
		{
			if (devicesOnRecorder.Contains(deviceLinksRow.IDDevice) && devicesOnRecorder.Contains(deviceLinksRow.IDDeviceLinked))
			{
				deviceLinksRow.SetAdded();
			}
		}
		ConfigurationDataSet.DeviceDataDistributionDataTable deviceDataDistributionDataTable = (ConfigurationDataSet.DeviceDataDistributionDataTable)configurationDataSet.DeviceDataDistribution.GetChanges(DataRowState.Deleted);
		deviceDataDistributionDataTable?.RejectChanges();
		ConfigurationDataSet.DeviceTrackEvidenceLockRow[] array3 = configurationDataSet.DeviceTrackEvidenceLock.ToArray();
		foreach (ConfigurationDataSet.DeviceTrackEvidenceLockRow deviceTrackEvidenceLockRow in array3)
		{
			ConfigurationDataSet.DeviceDataDistributionRow deviceDataDistributionRow = deviceDataDistributionDataTable?.FindByrecorder_idtrack_id(recorderId.ToString(), deviceTrackEvidenceLockRow.IDTrack);
			if (deviceDataDistributionRow == null)
			{
				configurationDataSet.DeviceTrackEvidenceLock.AddDeviceTrackEvidenceLockRow(deviceTrackEvidenceLockRow.IDMarkedData, deviceTrackEvidenceLockRow.IDDevice, deviceTrackEvidenceLockRow.IDTrack, deviceTrackEvidenceLockRow.StartTime, deviceTrackEvidenceLockRow.EndTime);
			}
			deviceTrackEvidenceLockRow.Delete();
			deviceTrackEvidenceLockRow.AcceptChanges();
		}
		ConfigurationDataSet.DeviceTrackInfoRow[] array4 = configurationDataSet.DeviceTrackInfo.ToArray();
		foreach (ConfigurationDataSet.DeviceTrackInfoRow deviceTrackInfoRow in array4)
		{
			if (deviceTrackInfoRow.IDRecorder == recorderId)
			{
				deviceTrackInfoRow.Delete();
				continue;
			}
			deviceTrackInfoRow.Delete();
			deviceTrackInfoRow.AcceptChanges();
		}
		foreach (DataTable table in configurationDataSet.Tables)
		{
			if (string.Equals(table.TableName, configurationDataSet.License.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceLinks.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceTrackInfo.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceTrackEvidenceLock.TableName, StringComparison.Ordinal) || string.Equals(table.TableName, configurationDataSet.DeviceDataDistribution.TableName, StringComparison.Ordinal))
			{
				continue;
			}
			table.AcceptChanges();
			foreach (DataRow row in table.Rows)
			{
				row.SetAdded();
			}
		}
		return configurationDataSet.GetChanges();
	}

	private byte[] RequestRecorderLicense(Guid recorderId)
	{
		lock (LicenseManager)
		{
			ILicenseManagerCache requiredService = ServiceContainer.Instance.GetRequiredService<ILicenseManagerCache>();
			requiredService.ClearAllCaches();
			return LicenseManager.GetRecorderLicense(recorderId);
		}
	}

	private SubTaskResult TryCreateHardwareBlock(Guid hardwareId, string sessionId, out int hardwareBlockId)
	{
		hardwareBlockId = 0;
		using IHardwareConfigDataComponent hardwareConfigDataComponent = DataComponentFactory.CreateHardwareConfigDataComponent();
		try
		{
			hardwareBlockId = hardwareConfigDataComponent.CreateHardwareBlock(hardwareId, DateTime.UtcNow.AddMinutes(2.0), "Move hardware", sessionId);
		}
		catch (HardwareUpdateException ex)
		{
			Logger.LogWarning("Move hardware task - Move failed. Error: {ExceptionMessage}", ex.Message);
			TaskFailedResponse taskResponse = CreateTaskFailedResponse(60267, VmoResources.VMO60267);
			return SubTaskResult.FailResult(taskResponse);
		}
		catch (DatabaseException ex2)
		{
			Logger.LogWarning("Move hardware task - Move failed. Error: {ExceptionMessage}", ex2.Message);
			TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60268, VmoResources.VMO60268);
			return SubTaskResult.FailResult(taskResponse2);
		}
		return SubTaskResult.SuccessResult();
	}

	private bool TryDeleteHardwareBlock(int hardwareBlockId)
	{
		using IHardwareConfigDataComponent hardwareConfigDataComponent = DataComponentFactory.CreateHardwareConfigDataComponent();
		try
		{
			hardwareConfigDataComponent.DeleteHardwareBlock(hardwareBlockId);
		}
		catch (HardwareUpdateException ex)
		{
			Logger.LogWarning("Move hardware task - Failed to remove hardware block. Id: {BlockId}. Error: {ExceptionMessage}", hardwareBlockId, ex.Message);
			return false;
		}
		catch (DatabaseException ex2)
		{
			Logger.LogWarning("Move hardware task - Failed to remove hardware block. Id: {BlockId}. Error: {ExceptionMessage}", hardwareBlockId, ex2.Message);
			return false;
		}
		return true;
	}

	private void SendUpdatesToRecorders(Guid[] serviceIds, SecurityDataSet changes, ICallContextInfo callContextInfo)
	{
		if (changes == null)
		{
			return;
		}
		Guid[] source = RecorderCommManager.QueryRecorderServiceIds();
		foreach (Guid item in source.Where((Guid serviceId) => serviceIds.Contains(serviceId)))
		{
			try
			{
				RecorderCommManager.UpdateSecurityConfigurationAsync(item, new SecurityConfigurationDto(changes), callContextInfo);
			}
			catch (RecorderNotFoundException)
			{
			}
			catch (NotSupportedByLegacyRecorderException)
			{
			}
		}
	}

	private bool SendUpdateSystemConfigurationCommandAndWaitForResponse(Guid[] serviceIds, ConfigurationDataSet changes, ICallContextInfo callContextInfo)
	{
		using (CancellationTokenSource cancellationTokenSource = new CancellationTokenSource())
		{
			List<Task<ResponseBase>> list = new List<Task<ResponseBase>>();
			foreach (Guid recordingServerServiceId in serviceIds)
			{
				Task<ResponseBase> item = SendUpdateSystemConfigurationCommandAndWaitForResponseAsync(recordingServerServiceId, changes, callContextInfo, cancellationTokenSource.Token);
				list.Add(item);
			}
			Task[] tasks = list.ToArray();
			Task.WaitAll(tasks, cancellationTokenSource.Token);
			foreach (Task<ResponseBase> item2 in list)
			{
				if (!(item2.Result is SuccessResponse))
				{
					return false;
				}
			}
		}
		return true;
	}

	private Task<ResponseBase> SendUpdateSystemConfigurationCommandAndWaitForResponseAsync(Guid recordingServerServiceId, ConfigurationDataSet changes, ICallContextInfo callContextInfo, CancellationToken cancellation)
	{
		return Task.Factory.StartNew(delegate
		{
			SystemConfigurationDto systemConfigurationDto = new SystemConfigurationDto(changes);
			UpdateSystemConfigurationCommand command = new UpdateSystemConfigurationCommand(systemConfigurationDto);
			using IMessageManagerResponseCollector messageManagerResponseCollector = MessageManager.SendCommand(recordingServerServiceId, command, callContextInfo);
			if (!cancellation.IsCancellationRequested)
			{
				ResponseBase nextResponse;
				try
				{
					nextResponse = messageManagerResponseCollector.GetNextResponse(TimeSpan.FromSeconds(30.0));
				}
				catch (TimeoutException)
				{
					return new TimeoutResponse();
				}
				Logger.LogInformation("Move hardware task - Response received. Response: {Response}", (nextResponse == null) ? "<NULL>" : nextResponse.GetType().Name);
				return nextResponse;
			}
			return new SuccessResponse();
		}, cancellation, TaskCreationOptions.LongRunning, TaskScheduler.Default);
	}

	private SubTaskResult SendPrepareMoveHardwareCommandsAndWaitForResponse(MoveHardwareTaskCommand cmd, ICallContextInfo callContextInfo, IResponseSink responseSink, ConfiguredServices serviceIds, DateTime taskStartTime, ref int taskProgress, int maxTaskProgress)
	{
		int lastProgress = 0;
		int taskProgressLocal = taskProgress;
		SynchrounousProgress progress = new SynchrounousProgress(delegate(int value)
		{
			lock (responseSink)
			{
				taskProgressLocal = Math.Min(maxTaskProgress, taskProgressLocal + value);
				if (taskProgressLocal != lastProgress)
				{
					responseSink.AddResponse(new TaskProgressResponse(taskStartTime, taskProgressLocal));
					lastProgress = taskProgressLocal;
				}
			}
		});
		using CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
		Task<SubTaskResult> task = null;
		Task<SubTaskResult> task2 = null;
		task = SendPrepareMoveHardwareCommandAndWaitForResponseAsync(serviceIds.SourceRecorderServiceId, cmd, callContextInfo, progress, cancellationTokenSource.Token);
		if (serviceIds.SourceRecorderHotstandbyServiceConfigured)
		{
			task2 = SendPrepareMoveHardwareCommandAndWaitForResponseAsync(serviceIds.SourceRecorderHotstandbyServiceId, cmd, callContextInfo, progress, cancellationTokenSource.Token);
		}
		if (task2 == null)
		{
			task.Wait();
			taskProgress = taskProgressLocal;
			return task.Result;
		}
		int num = Task.WaitAny(task, task2);
		Task<SubTaskResult> task3 = ((num == 0) ? task : task2);
		Task<SubTaskResult> task4 = ((num == 0) ? task2 : task);
		if (!task3.Result.Success)
		{
			cancellationTokenSource.Cancel();
			taskProgress = taskProgressLocal;
			return task3.Result;
		}
		taskProgress = taskProgressLocal;
		if (!task4.Result.Success)
		{
			return task4.Result;
		}
		return task.Result;
	}

	private Task<SubTaskResult> SendPrepareMoveHardwareCommandAndWaitForResponseAsync(Guid recordingServerServiceId, MoveHardwareTaskCommand cmd, ICallContextInfo callContextInfo, IProgress<int> progress, CancellationToken cancellation)
	{
		return Task.Factory.StartNew(delegate
		{
			PrepareMoveHardwareCommand command = new PrepareMoveHardwareCommand(cmd.HardwareId, (int)ServerConfig.MoveHardwarePrepareMoveHardwareTimeout.TotalSeconds + 10);
			using (IMessageManagerResponseCollector messageManagerResponseCollector = MessageManager.SendCommand(recordingServerServiceId, command, callContextInfo))
			{
				while (!cancellation.IsCancellationRequested)
				{
					ResponseBase nextResponse;
					try
					{
						nextResponse = messageManagerResponseCollector.GetNextResponse(ServerConfig.MoveHardwarePrepareMoveHardwareTimeout);
					}
					catch (TimeoutException)
					{
						Logger.LogInformation("Move hardware task - timeout while waiting for response");
						TaskFailedResponse taskResponse = CreateTaskFailedResponse(60271, VmoResources.VMO60271);
						return SubTaskResult.FailResult(taskResponse);
					}
					Logger.LogInformation("Move hardware task - Response received. Response: {Response}", (nextResponse == null) ? "<NULL>" : nextResponse.GetType().Name);
					if (nextResponse is HeartBeatResponse)
					{
						progress.Report(2);
					}
					else
					{
						if (nextResponse is PrepareMoveHardwareResponse prepareMoveHardwareResponse)
						{
							return SubTaskResult.SuccessResult(prepareMoveHardwareResponse);
						}
						if (nextResponse is ErrorResponse)
						{
							TaskFailedResponse taskResponse2 = CreateTaskFailedResponse(60327, VmoResources.VMO60327);
							return SubTaskResult.FailResult(taskResponse2);
						}
						if (nextResponse is TimeoutResponse)
						{
							TaskFailedResponse taskResponse3 = CreateTaskFailedResponse(60271, VmoResources.VMO60271);
							return SubTaskResult.FailResult(taskResponse3);
						}
					}
				}
			}
			return SubTaskResult.CancelledResult();
		}, cancellation, TaskCreationOptions.LongRunning, TaskScheduler.Default);
	}

	private static TaskFailedResponse CreateTaskFailedResponse(int errorCode, string errorText, params string[] arguments)
	{
		TaskError taskError = new TaskError();
		taskError.ErrorCode = errorCode;
		taskError.ErrorText = errorText;
		for (int i = 0; i < arguments.Length; i++)
		{
			taskError.Arguments.Add(i, arguments[i]);
		}
		return new TaskFailedResponse(isFatal: true, taskError.ErrorText, taskError);
	}
}
