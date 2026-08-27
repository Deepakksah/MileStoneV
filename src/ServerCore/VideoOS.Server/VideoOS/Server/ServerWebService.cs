using System;
using System.Collections;
using System.Globalization;
using System.ServiceModel;
using System.Text;
using System.Xml;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Security;
using VideoOS.Licensing;
using VideoOS.Management.Common;
using VideoOS.Rules.Interfaces;
using VideoOS.Server.RecorderCommunication;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false, IncludeExceptionDetailInFaults = true)]
public sealed class ServerWebService : IServerWebService
{
	private static IServerEnvironment _serverEnvironment = new ServerEnvironment();

	public bool CompatibilityModeOn { get; set; }

	private static IServerManagement ServerIServerManagement => Server.Instance;

	private static IServerCommandAPI ServerIServerClientAPI => Server.Instance;

	private static IServerEnterpriseServerConnector ServerIServerEnterpriseServerConnector => Server.Instance;

	private static IServerEnvironment ServerEnvironment => _serverEnvironment;

	public void ProcessPendingTasks(string recordingServerId)
	{
		try
		{
			CheckServiceSecurity();
			Server.Instance.ProcessPendingTasks(recordingServerId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public bool IsRecorderOnline(string recordingServerId)
	{
		try
		{
			CheckServiceSecurity();
			return Server.Instance.IsRecorderOnline(new Guid(recordingServerId));
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	private static ResponseBase ProcessCommand(CommandBase command, ICallContextInfo callContextInfo, TimeSpan timeout)
	{
		IResponseCollector responseCollector = ServerEnvironment.LocalCommandProcessor.ProcessCommandAsync(command, callContextInfo);
		try
		{
			return responseCollector.GetNextResponse(timeout);
		}
		catch (TimeoutException)
		{
			return null;
		}
	}

	public bool VerifyRecordingStoragePath(string recordingServerId, string recordingStorageId, string path, bool newPath)
	{
		DatabaseOperationCommand databaseOperationCommand = new DatabaseOperationCommand();
		databaseOperationCommand.VerifyPath(new Guid(recordingServerId), new Guid(recordingStorageId), path, newPath);
		return ProcessVerifyPathCommand(databaseOperationCommand);
	}

	public bool VerifyArchiveStoragePath(string recordingServerId, string archiveStorageId, string path, bool newPath)
	{
		DatabaseOperationCommand databaseOperationCommand = new DatabaseOperationCommand();
		databaseOperationCommand.VerifyNewArchivePath(new Guid(recordingServerId), new Guid(archiveStorageId), path, newPath);
		return ProcessVerifyPathCommand(databaseOperationCommand);
	}

	private static bool ProcessVerifyPathCommand(DatabaseOperationCommand command)
	{
		try
		{
			CheckServiceSecurity();
			ResponseBase responseBase = ProcessCommand(command, WcfCallContextInfo.Current, TimeSpan.FromSeconds(30.0));
			if (responseBase is SuccessResponse)
			{
				return true;
			}
			if (responseBase is ErrorResponse)
			{
				return false;
			}
			HandleDatabaseResponse(responseBase);
			return false;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public bool VerifyStorageAndArchiveOnline(string recordingServerId, string deviceId, string recordingStorageId, string archiveStorageId)
	{
		try
		{
			CheckServiceSecurity();
			DatabaseOperationCommand databaseOperationCommand = new DatabaseOperationCommand();
			databaseOperationCommand.VerifyStorageAndArchiveOnline(new Guid(recordingServerId), string.IsNullOrEmpty(deviceId) ? Guid.Empty : new Guid(deviceId), string.IsNullOrEmpty(recordingStorageId) ? Guid.Empty : new Guid(recordingStorageId), string.IsNullOrEmpty(archiveStorageId) ? Guid.Empty : new Guid(archiveStorageId));
			ResponseBase responseBase = ProcessCommand(databaseOperationCommand, WcfCallContextInfo.Current, TimeSpan.FromSeconds(30.0));
			if (responseBase is SuccessResponse)
			{
				return true;
			}
			if (responseBase is ErrorResponse)
			{
				return false;
			}
			HandleDatabaseResponse(responseBase);
			return false;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public string[] ReadDrivesOnRecorder(string recordingServerId)
	{
		try
		{
			CheckServiceSecurity();
			ServerFileSystemOperationCommand serverFileSystemOperationCommand = new ServerFileSystemOperationCommand();
			serverFileSystemOperationCommand.QueryRecorderDrives(new Guid(recordingServerId));
			ResponseBase responseBase = ProcessCommand(serverFileSystemOperationCommand, WcfCallContextInfo.Current, TimeSpan.FromSeconds(30.0));
			if (responseBase is ServerFileSystemResponseQueryDrives { Status: ServerFileSystemResponseStatus.Success } serverFileSystemResponseQueryDrives)
			{
				return serverFileSystemResponseQueryDrives.Drives;
			}
			throw HandleServerFileSystemResponseErrors(responseBase);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public string[] ReadDirectoriesOnRecorder(string recordingServerId, string path)
	{
		try
		{
			CheckServiceSecurity();
			ServerFileSystemOperationCommand serverFileSystemOperationCommand = new ServerFileSystemOperationCommand();
			serverFileSystemOperationCommand.QueryRecorderDirectories(new Guid(recordingServerId), path);
			ResponseBase responseBase = ProcessCommand(serverFileSystemOperationCommand, WcfCallContextInfo.Current, TimeSpan.FromSeconds(30.0));
			if (responseBase is ServerFileSystemResponseQueryDirectories { Status: ServerFileSystemResponseStatus.Success } serverFileSystemResponseQueryDirectories)
			{
				return serverFileSystemResponseQueryDirectories.Directories;
			}
			throw HandleServerFileSystemResponseErrors(responseBase);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public void CreateDirectoryOnRecorder(string recordingServerId, string path)
	{
		try
		{
			CheckServiceSecurity();
			ServerFileSystemOperationCommand serverFileSystemOperationCommand = new ServerFileSystemOperationCommand();
			serverFileSystemOperationCommand.CreateRecorderDirectory(new Guid(recordingServerId), path);
			ResponseBase responseBase = ProcessCommand(serverFileSystemOperationCommand, WcfCallContextInfo.Current, TimeSpan.FromSeconds(30.0));
			if (responseBase is ServerFileSystemResponseCreateDirectory { Status: ServerFileSystemResponseStatus.Success })
			{
				return;
			}
			throw HandleServerFileSystemResponseErrors(responseBase);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	private static Exception HandleServerFileSystemResponseErrors(ResponseBase response)
	{
		if (response == null)
		{
			return new FaultException<RecordingServerCommunicationTimeoutFault>(new RecordingServerCommunicationTimeoutFault());
		}
		if (response is RecorderUnavailableResponse)
		{
			return new FaultException<RecordingServerUnavailableFault>(new RecordingServerUnavailableFault());
		}
		return new FaultException<RecordingServerOperationFailedFault>(new RecordingServerOperationFailedFault());
	}

	private static void HandleDatabaseResponse(ResponseBase response)
	{
		if (response == null)
		{
			throw new FaultException<RecordingServerCommunicationTimeoutFault>(new RecordingServerCommunicationTimeoutFault());
		}
		if (response is RecorderUnavailableResponse)
		{
			throw new FaultException<RecordingServerUnavailableFault>(new RecordingServerUnavailableFault());
		}
		if (response is DatabaseResponseDiskUsage databaseResponseDiskUsage)
		{
			if (databaseResponseDiskUsage.Status == DatabaseResponseStatus.Error)
			{
				throw HandleException(new MediaStorageSubsystemException(databaseResponseDiskUsage.Message));
			}
		}
		else if (response is FailResponse)
		{
			throw HandleException(new MediaStorageSubsystemException("Operation failed"));
		}
	}

	public void TriggerUserDefinedEvent(string userDefinedEventId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			Server.Instance.EventTriggerNoSecurity(new Guid(userDefinedEventId), basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void TriggerPluginEvent(Guid pluginEventId, bool isDeviceEvent, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			EventTracer.Log.TraceMethodStart("TriggerPluginEvent");
			CheckServiceSecurity();
			TriggerPluginEventInternal(pluginEventId, isDeviceEvent, sourceId, metadataDeviceIds, metadataDeviceGroupIds, customData, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
		EventTracer.Log.TraceMethodStop("TriggerPluginEvent");
	}

	public void TriggerPluginEventBulk(PluginEventInfo[] pluginEvents, BasicCallContextInfo basicCallContextInfo)
	{
		if (pluginEvents == null)
		{
			return;
		}
		try
		{
			EventTracer.Log.TraceMethodStart("TriggerPluginEventBulk");
			CheckServiceSecurity();
			foreach (PluginEventInfo pluginEventInfo in pluginEvents)
			{
				try
				{
					TriggerPluginEventInternal(pluginEventInfo.PluginEventId, pluginEventInfo.IsDeviceEvent, pluginEventInfo.SourceId, pluginEventInfo.MetadataDeviceIds, pluginEventInfo.MetadataDeviceGroupIds, pluginEventInfo.CustomData, basicCallContextInfo);
				}
				catch (OutOfMemoryException)
				{
					throw;
				}
				catch (RecorderNotFoundException)
				{
				}
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex4)
		{
			throw HandleException(ex4);
		}
		EventTracer.Log.TraceMethodStop("TriggerPluginEventBulk");
	}

	private static void TriggerPluginEventInternal(Guid pluginEventId, bool isDeviceEvent, Guid sourceId, Guid[] metadataDeviceIds, Guid[] metadataDeviceGroupIds, string customData, BasicCallContextInfo basicCallContextInfo)
	{
		EventMetaDataCollection eventMetaDataCollection = null;
		bool flag = metadataDeviceIds != null && metadataDeviceIds.Length != 0;
		bool flag2 = metadataDeviceGroupIds != null && metadataDeviceGroupIds.Length != 0;
		bool flag3 = !string.IsNullOrEmpty(customData);
		if (flag || flag2 || flag3)
		{
			eventMetaDataCollection = new EventMetaDataCollection();
			if (flag || flag2)
			{
				StringBuilder stringBuilder = new StringBuilder();
				XmlWriter writer = XmlWriter.Create(stringBuilder, new XmlWriterSettings
				{
					OmitXmlDeclaration = true
				});
				try
				{
					writer.WriteStartElement("targetDevices");
					if (flag)
					{
						writer.WriteStartElement("deviceIds");
						Array.ForEach(metadataDeviceIds, delegate(Guid id)
						{
							writer.WriteElementString("deviceId", id.ToString());
						});
						writer.WriteEndElement();
					}
					if (flag2)
					{
						writer.WriteStartElement("deviceGroupIds");
						Array.ForEach(metadataDeviceIds, delegate(Guid id)
						{
							writer.WriteElementString("deviceGroupIds", id.ToString());
						});
						writer.WriteEndElement();
					}
					writer.WriteEndElement();
				}
				finally
				{
					if (writer != null)
					{
						((IDisposable)writer).Dispose();
					}
				}
				eventMetaDataCollection.SetMetadataAsString("TargetDevices", stringBuilder.ToString());
			}
			if (flag3)
			{
				eventMetaDataCollection.SetMetadataAsString("CustomData", customData);
			}
		}
		Server.Instance.EventTriggerWithMetaDataNoSecurity(pluginEventId, isDeviceEvent, sourceId, eventMetaDataCollection, basicCallContextInfo);
	}

	public void TriggerBuiltInEventForDevice(string eventId, string deviceId, DateTime time, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			Server.Instance.TriggerBuiltInEventForDevice(new Guid(eventId), new Guid(deviceId), time, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public string GetVmoServerName()
	{
		try
		{
			CheckSystemMembership();
			return ServerIServerClientAPI.GetVmoServerName();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public int GetVmoServerPort()
	{
		try
		{
			CheckSystemMembership();
			return ServerIServerClientAPI.GetVmoServerPort();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public int GetVmoServerSecurePort()
	{
		try
		{
			CheckSystemMembership();
			return ServerIServerClientAPI.GetVmoServerSecurePort();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void UpdateLicense(byte[] license, string ownerXml, string sitesInformation)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.UpdateLicense(license, ownerXml, sitesInformation);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (LicenseException ex2)
		{
			LicenseExceptionFault detail = new LicenseExceptionFault(ex2.Message, ex2.ErrorNumber, ex2.FeatureErrorNumber);
			throw new FaultException<LicenseExceptionFault>(detail);
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public void QueueAutomaticActivation()
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.QueueAutomaticActivation();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public int GetLastAutomaticActivationError()
	{
		try
		{
			CheckServiceSecurity();
			if (ServerIServerManagement.LastAutomaticActivationError == null)
			{
				return 0;
			}
			return ServerIServerManagement.LastAutomaticActivationError.ErrorNumber;
		}
		catch (Exception ex)
		{
			throw HandleException(ex);
		}
	}

	public GetAbsolutePtzPositionResponse GetAbsolutePtzPosition(string deviceId)
	{
		try
		{
			CheckServiceSecurity();
			GetAbsolutePtzPositionCommand command = GetAbsolutePtzPositionCommand.Create(new Guid(deviceId));
			ResponseBase responseBase = ProcessCommand(command, WcfCallContextInfo.Current, TimeSpan.FromMinutes(2.0));
			if (responseBase is GetAbsolutePtzPositionResponse result)
			{
				return result;
			}
			if (responseBase == null)
			{
				throw new FaultException<RecordingServerCommunicationTimeoutFault>(new RecordingServerCommunicationTimeoutFault());
			}
			if (responseBase is RecorderUnavailableResponse)
			{
				throw new FaultException<RecordingServerUnavailableFault>(new RecordingServerUnavailableFault());
			}
			if (responseBase is RecorderNotFoundResponse)
			{
				throw new FaultException<RecordingServerUnavailableFault>(new RecordingServerUnavailableFault());
			}
			if (responseBase is ErrorResponse errorResponse)
			{
				ErrorResponseFault detail = new ErrorResponseFault(errorResponse.Message, errorResponse.ErrorCode);
				throw new FaultException<ErrorResponseFault>(detail);
			}
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
		return null;
	}

	public ResponseBase UploadFileChunk(Guid transferId, byte[] chunkData, int offset, long fileSize, byte[] fileChecksum)
	{
		try
		{
			CheckServiceSecurity();
			UploadFileChunkCommand command = new UploadFileChunkCommand(transferId, chunkData, offset, fileSize, fileChecksum);
			ResponseBase responseBase = ProcessCommand(command, WcfCallContextInfo.Current, TimeSpan.FromMinutes(2.0));
			if (responseBase is ErrorResponse errorResponse)
			{
				ErrorResponseFault detail = new ErrorResponseFault(errorResponse.Message, errorResponse.ErrorCode);
				throw new FaultException<ErrorResponseFault>(detail);
			}
			return responseBase;
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (FaultException)
		{
			throw;
		}
		catch (Exception ex3)
		{
			throw HandleException(ex3);
		}
	}

	public void SendCommand(Guid proxyId, CommandBase command, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.SendCommand(proxyId, command, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetResponses(proxyId, timeout);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void ReleaseGetResponses(Guid proxyId)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.ReleaseGetResponses(proxyId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetSecurityConfigurationDtoByRole(roleId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetSecurityConfigurationDtoByTables(tableNames);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void RefreshIdentityCache()
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.RefreshIdentityCache();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void LogServerChanged()
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.LogServerChanged();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void Log(LogEntry logEntry)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.Log(logEntry);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void LogLogs(LogEntry[] logEntries)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.Log(logEntries);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public LogQueryResult QueryLog(LogQuery logQuery)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryLog(logQuery);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryLogExport(logQuery, codePage, culture);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public LogEntryField[] QueryLogFilter(LogFilter logFilter)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryLogFilter(logFilter);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public byte[] GetLicenseRequestFromConfiguration()
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetLicenseRequestFromConfiguration();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public string GetLicenseInformation()
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetLicenseInformation();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetLicenseByLicenseId(licenseId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareDefinitionByDigest(digest);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareByRecorderId(recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareByHardwareId(hardwareId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareByHardwareIds(hardwareIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareByHostedHardwareId(hostedHardwareId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHardwareByDeviceGroupId(deviceGroupId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDevicesByHardwareId(hardwareId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDevicesByRecorderId(recorderId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDevicesByDeviceIds(deviceIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceGroupByDeviceGroupId(deviceGroupId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceGroupsByParentDeviceGroupId(parentDeviceGroupId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceGroupMembersByDeviceGroupId(deviceGroupId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceGroupMembersByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceRecorderPlugin(recorderPluginID, deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceRecorderPlugins(recorderPluginID, deviceIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceRecorderPluginsByDeviceId(deviceId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDevicePrivacyMaskByDeviceIds(deviceIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetDeviceTracksByDeviceIds(deviceIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHostedHardwareByIds(hostedHardwareIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetHostingServersByIds(hostingServerIds);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetConfigurationDataSetTables(tableNames);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryRolesForDefaultTimeProfile(timeProfileId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryRolesForPermissionsWithTimeProfile(timeProfileId);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public GetFileResult ReadFile(string fileName, long index, long count, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.ReadFile(fileName, index, count);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public int[] QueryHostedHardwareIdsNotAdded(BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.QueryHostedHardwareIdsNotAdded();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public string GetSystemInformationXml()
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetSystemInformationXml();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public string GetNavigationXml()
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetNavigationXml();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public AviCodec64[] GetInstalledCodecs()
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.GetInstalledCodecs();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public string SendMail(MailGeneratorData mailGeneratorData)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerManagement.SendMail(mailGeneratorData);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void FireCustomEvent(string eventName, string eventData, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.FireCustomEvent(eventName, eventData, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public UserInfo GetUserInfo(string token, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerClientAPI.GetUserInfo(token);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public Identity GetUserIdentity(string token)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerClientAPI.GetUserIdentity(token);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void EventTriggerWithMetadata(string token, Guid eventId, KeyValue[] metadata, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerClientAPI.EventTriggerWithMetadata(token, eventId, metadata, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void MatrixMonitorActivateCamera(Guid matrixMonitorId, Guid deviceId, BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerClientAPI.MatrixMonitorActivateCamera(matrixMonitorId, deviceId, basicCallContextInfo);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public DictionaryEntry[] GetVmoServicesList(BasicCallContextInfo basicCallContextInfo)
	{
		try
		{
			CheckServiceSecurity();
			return ServerIServerClientAPI.GetVmoServicesList();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void ValidateToken(string token, string userHostAddress, out string tokenUserName, out string tokenPassword)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerEnterpriseServerConnector.ValidateToken(token, userHostAddress, out tokenUserName, out tokenPassword);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void ServerRegistrationChanged()
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.ServerRegistrationChanged();
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void MipItemPositionChanged(string fqid, string newPosition)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.MipItemPositionChanged(fqid, newPosition);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	public void MipItemChanged(string fqid, int changeType)
	{
		try
		{
			CheckServiceSecurity();
			ServerIServerManagement.MipItemChanged(fqid, changeType);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
	}

	private static FaultException<ExceptionDetail> HandleException(Exception ex)
	{
		ExceptionDetail exceptionDetail = new ExceptionDetail(ex);
		FaultCode code = new FaultCode(ex.GetType().Name);
		if (ex is ServerServiceException { ErrorCode: var errorCode })
		{
			code = new FaultCode(errorCode.ToString(CultureInfo.InvariantCulture));
		}
		if (ex is LicenseException { FeatureErrorNumber: var featureErrorNumber })
		{
			code = new FaultCode(featureErrorNumber.ToString(CultureInfo.InvariantCulture));
		}
		if (ex is UnauthorizedAccessException)
		{
			exceptionDetail = new ExceptionDetail(new ServerServiceException(ex.Message, ex));
			code = new FaultCode(51000.ToString(CultureInfo.InvariantCulture));
		}
		return new FaultException<ExceptionDetail>(exceptionDetail, exceptionDetail.Message, code);
	}

	private static void CheckServiceSecurity()
	{
		try
		{
			VmoSecurityManager.Instance.CheckAdministrator(WcfCallContextInfo.Current.UserSid);
		}
		catch (UnauthorizedAccessException innerException)
		{
			UnauthorizedException ex = new UnauthorizedException("Calling service user doesn't have administration security rights", innerException);
			throw ex;
		}
	}

	private static void CheckSystemMembership()
	{
		try
		{
			VmoSecurityManager.Instance.CheckMembership(WcfCallContextInfo.Current.UserSid);
		}
		catch (UnauthorizedAccessException innerException)
		{
			UnauthorizedException ex = new UnauthorizedException("Calling service user doesn't have administration security rights", innerException);
			throw ex;
		}
	}
}
