using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Channels;
using System.Xml;
using System.Xml.XPath;
using VideoOS.Common.Integration;
using VideoOS.Common.Integration.AviConfig;
using VideoOS.Common.Integration.Command;
using VideoOS.Common.Integration.Configuration;
using VideoOS.Common.Integration.Definition;
using VideoOS.Common.Integration.Plugins;
using VideoOS.Common.Integration.Security;
using VideoOS.Common.Integration.ServerAPI;
using VideoOS.Management.Common;
using VideoOS.Management.Server.GetConfiguration;
using VideoOS.Management.Server.Log;
using VideoOS.Management.Server.Properties;
using VideoOS.Server.Webservice.Shared;
using VideoOS.Utilities.Network;
using VideoOS.WebService.Common.ServerCommandService;

namespace VideoOS.Management.Server;

[ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerCall, UseSynchronizationContext = false)]
[AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Required)]
public class ServerProxyService : GeminiWebServiceBase, IServerProxyService
{
	public class GetResponsesSecurityVisitor : ICommonResponseVisitor
	{
		private IGeminiRequestContext requestContext;

		public GetResponsesSecurityVisitor(IGeminiRequestContext requestContext)
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}
			this.requestContext = requestContext;
		}

		public void NotSupported(ResponseBase response)
		{
			throw new NotImplementedException();
		}

		public void Visit(QueryDeviceEvidenceLockSizesResponse response)
		{
			NotSupported(response);
		}

		public void Visit(ExceptionResponse response)
		{
			NotSupported(response);
		}

		public void Visit(RecorderNotFoundResponse response)
		{
		}

		public void Visit(RecorderUnavailableResponse response)
		{
		}

		public void Visit(RecorderNotSupportedResponse response)
		{
		}

		public void Visit(RecorderNotLicensedResponse response)
		{
		}

		public void Visit(SuccessResponse response)
		{
		}

		public void Visit(FailResponse response)
		{
		}

		public void Visit(BoolResponse response)
		{
		}

		public void Visit(ErrorResponse response)
		{
		}

		public void Visit(WarningResponse response)
		{
		}

		public void Visit(ExceptionResponseDataContract response)
		{
		}

		public void Visit(TimeoutResponse response)
		{
		}

		public void Visit(HeartBeatResponse response)
		{
		}

		public void Visit(ProgressResponse response)
		{
		}

		public void Visit(SynchronizeDevicePresetsResponse response)
		{
		}

		public void Visit(HardwareSetupInformationResponse response)
		{
		}

		public void Visit(HardwareAlreadyAddedResponse response)
		{
		}

		public void Visit(BackupConfigStatusResponse response)
		{
		}

		public void Visit(BackupConfigCompletedResponse response)
		{
		}

		public void Visit(BackupConfigAbortedResponse response)
		{
		}

		public void Visit(BackupConfigFailedResponse response)
		{
		}

		public void Visit(ReplaceHardwareResponse response)
		{
		}

		public void Visit(ServerFileSystemResponseQueryDrives response)
		{
		}

		public void Visit(ServerFileSystemResponseQueryDirectories response)
		{
		}

		public void Visit(ServerFileSystemResponseCreateDirectory response)
		{
		}

		public void Visit(DatabaseResponseDiskUsages response)
		{
		}

		public void Visit(DatabaseResponseArchiveInformation response)
		{
		}

		public void Visit(DatabaseResponseTrackIdsWithoutMediaOnRecorder response)
		{
		}

		public void Visit(HardwareScanProgressResponse response)
		{
		}

		public void Visit(GetAbsolutePtzPositionResponse response)
		{
		}

		public void Visit(PtzReserveSessionResponse response)
		{
		}

		public void Visit(ReleasePtzSessionResponse response)
		{
		}

		public void Visit(InitializeDiscoveryComponentFailedResponse response)
		{
		}

		public void Visit(RegisterSessionFailedResponse response)
		{
		}

		public void Visit(StartSessionFailedResponse response)
		{
		}

		public void Visit(DiscoveryComponentFailedResponse response)
		{
		}

		public void Visit(DatabaseResponseDiskUsage response)
		{
		}

		public void Visit(HardwareUnsupportedResponse response)
		{
		}

		public void Visit(HardwareSetupErrorResponse response)
		{
		}

		public void Visit(EvidenceLockFailedRemoveResponse response)
		{
		}

		public void Visit(PrepareMoveHardwareResponse response)
		{
		}

		public void Visit(GetHardwareNetworkSettingsResponse response)
		{
		}

		public void Visit(GetHardwareUsersResponse response)
		{
		}

		public void Visit(DiscoveryComponentSecureScanUnsupportedResponse response)
		{
		}

		public void Visit(FileUploadResponse response)
		{
		}

		public void Visit(FirmwareUpgradeSuccessResponse response)
		{
		}

		public void Visit(MsFailoverStatusResponse response)
		{
		}

		public void Visit(RecorderStatusResponse response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			List<RecorderStatusInfo> list = new List<RecorderStatusInfo>();
			RecorderStatusInfo[] recorderStatusInfoList = response.RecorderStatusInfoList;
			foreach (RecorderStatusInfo recorderStatusInfo in recorderStatusInfoList)
			{
				if (requestContext.GetSecurityManagerService().HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recorderStatusInfo.Id.ToString()))
				{
					list.Add(recorderStatusInfo);
				}
			}
			if (list.Count != response.RecorderStatusInfoList.Length)
			{
				response.RecorderStatusInfoList = list.ToArray();
			}
		}

		public void Visit(FailoverStatusResponse response)
		{
			if (response == null)
			{
				throw new ArgumentNullException("response");
			}
			requestContext.GetSecurityManagerService().CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_READ", response.FailoverServerId.ToString());
		}
	}

	public class SendCommandSecurityVisitor : ICommandVisitor
	{
		private IGeminiRequestContext requestContext;

		private IDeviceTypeLookup deviceTypeLookup;

		private ISecurityManagerService securityManager;

		public event EventHandler<TableInfoEventArgs> ProcessingTable;

		private void OnProcessingTable(TableInfoEventArgs e)
		{
			if (this.ProcessingTable != null)
			{
				this.ProcessingTable(this, e);
			}
		}

		public SendCommandSecurityVisitor(IGeminiRequestContext requestContext, IDeviceTypeLookup deviceTypeLookup)
		{
			if (requestContext == null)
			{
				throw new ArgumentNullException("requestContext");
			}
			if (deviceTypeLookup == null)
			{
				throw new ArgumentNullException("deviceTypeLookup");
			}
			this.requestContext = requestContext;
			this.deviceTypeLookup = deviceTypeLookup;
			securityManager = this.requestContext.GetSecurityManagerService();
		}

		public void Visit(AnalyzeUpdateSecurityManagerCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateHardwareReadOnlySettingsCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateRecorderStatusCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(SendNotificationCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateUserSecurityTokensCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(SetMatrixViewCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(SetUserSecurityTokenCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(TriggerEventAsGlobalCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(EventSubscriptionStateChangedCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(TriggerCustomEventBulkCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ReportMediaStatusCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ReportMediaCommittedToDatabaseCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ActivateArchivingCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(AddBookmarkCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateHardwareDriverCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateSecurityConfigurationCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ReplaceHardwareUpdateSystemConfigurationCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(SetRecorderShutdownStateCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(SendMultiNotificationCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ConfigureRulesCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ActivateUserDefinedEventOnRemoteSiteCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(RecorderAttachCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(VideoWallCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ExecuteRuleActionsCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(TriggerCustomEventCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(EdgeJobUpdatedCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateRecorderTaskProgressCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(RecorderTaskSuccessCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(RecorderTaskStoppedCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(RecorderTaskFailedCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(RemoveRecorderTaskCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(UpdateSystemConfigurationCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(ReportManualRecordingStateCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(MIPActionExecuteCommand command)
		{
			throw new NotImplementedException();
		}

		public void Visit(MIPAction2ExecuteCommand command)
		{
			throw new NotImplementedException();
		}

		public void Visit(PrepareMoveHardwareCommand command)
		{
			throw new NotImplementedException();
		}

		public void Visit(ChangeHardwarePasswordCommand command)
		{
			throw new NotImplementedException();
		}

		public void Visit(AuditLogCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
		}

		public void Visit(BackupConfigCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckApplicationPermission(requestContext, "BACKUP_CONFIGURATION");
		}

		public void Visit(DatabaseOperationCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command.DatabaseOperationType == DatabaseOperation.DiskUsageInformationTablesPerStorage)
			{
				List<Guid> list = new List<Guid>();
				foreach (Guid deviceId in command.DeviceIds)
				{
					Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId.ToString());
					if (securityManager.HasObjectPermission(requestContext, namespaceId, "GENERIC_READ", deviceId.ToString()))
					{
						list.Add(deviceId);
					}
				}
				if (list.Count != command.DeviceIds.Count())
				{
					command.ReplaceDeviceIds(list.ToArray());
				}
				return;
			}
			throw new NotSupportedException();
		}

		public void Visit(DeleteConfigurationItemCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command.TableTypeName == ConfigurationManager.HardwareDataTable.ToString())
			{
				string[] primaryIds = command.PrimaryIds;
				foreach (string objectId in primaryIds)
				{
					if (!securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", "/"))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "DELETE", objectId);
					}
				}
			}
			else if (command.TableTypeName == ConfigurationManager.RecordersDataTable.ToString())
			{
				string[] primaryIds = command.PrimaryIds;
				foreach (string text in primaryIds)
				{
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "DELETE", text);
					using GeminiRequestContext geminiRequestContext = GeminiApplicationCore.RuntimeServiceHost.CreateSystemContext();
					string[] array = Hardware.QueryHardwareIdsByRecorder(geminiRequestContext, text);
					foreach (string objectId2 in array)
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "DELETE", objectId2);
					}
				}
			}
			else if (command.TableTypeName == ConfigurationManager.DeviceGroupsDataTable.ToString())
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		public void Visit(HardwareScanCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", command.RecorderId.ToString());
		}

		public void Visit(SetHardwareNetworkSettingsCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", command.HardwareId.ToString());
		}

		public void Visit(GetHardwareNetworkSettingsCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", command.HardwareId.ToString());
		}

		public void Visit(GetHardwareUsersCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", command.HardwareId.ToString());
		}

		public void Visit(PingDeviceCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", command.RecorderId.ToString());
		}

		public void Visit(AddHardwareUserCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", command.HardwareId.ToString());
		}

		public void Visit(SetupInitialHardwareUserCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", command.RecorderId.ToString());
		}

		public void Visit(DeleteHardwareUserCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", command.HardwareId.ToString());
		}

		public void Visit(FirmwareUpgradeCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", command.HardwareId.ToString());
		}

		public void Visit(GetAbsolutePtzPositionCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "PTZ_CONTROL", command.DeviceId.ToString());
		}

		public void Visit(PtzReserveSessionCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "RESERVE_PTZ_SESSION", command.DeviceId.ToString());
		}

		public void Visit(PtzReleaseSessionCommand command)
		{
		}

		public void Visit(PtzStopSessionCommand command)
		{
		}

		public void Visit(HardwareSetupInformationCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", command.RecorderId.ToString());
			command.Password = TryDecryptPassword(command.Password);
		}

		public void Visit(RecorderStatusCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckApplicationPermission(requestContext, "GENERIC_READ");
		}

		public void Visit(ReplaceHardwareCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", command.OldHardwareID.ToString());
			command.Password = TryDecryptPassword(command.Password);
		}

		public void Visit(ServerFileSystemOperationCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			if (command.ServerType == ServerFileSystemServerType.RecordingServer)
			{
				switch (command.Operation)
				{
				case ServerFileSystemOperation.QueryDrives:
				case ServerFileSystemOperation.QueryDirectories:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", command.ServerId.ToString());
					break;
				case ServerFileSystemOperation.CreateDirectory:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", command.ServerId.ToString());
					break;
				}
			}
			else if (command.ServerType == ServerFileSystemServerType.ManagementServer)
			{
				switch (command.Operation)
				{
				case ServerFileSystemOperation.QueryDrives:
				case ServerFileSystemOperation.QueryDirectories:
					securityManager.CheckApplicationPermission(requestContext, "GENERIC_READ");
					break;
				case ServerFileSystemOperation.CreateDirectory:
					securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
					break;
				}
			}
		}

		public void Visit(SynchronizeDevicePresetsCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", command.DeviceId.ToString());
		}

		public void Visit(UpdateConfigurationManagerCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			ConfigurationDataSet dataSet = command.ConfigurationManagerDto.DataSet;
			if (dataSet != null)
			{
				string[] tablesIgnored = new string[1] { dataSet.RecordingServerServiceRecorder.TableName };
				Visit_UpdateConfigurationManagerCommand_TablesIgnored(tablesIgnored);
				string[] tablesNotSupported = new string[23]
				{
					dataSet.ArchiveStorage.TableName,
					dataSet.DeviceDataDistribution.TableName,
					dataSet.DeviceTrackEvidenceLock.TableName,
					dataSet.DeviceRestrictedMedia.TableName,
					dataSet.DeviceTrackInfo.TableName,
					dataSet.DeviceTrackDefinition.TableName,
					dataSet.ExternalPlugins.TableName,
					dataSet.FailoverInfo.TableName,
					dataSet.HardwareBlock.TableName,
					dataSet.HardwareDriverRecorder.TableName,
					dataSet.HostedHardware.TableName,
					dataSet.HostingServer.TableName,
					dataSet.NotificationTypes.TableName,
					dataSet.MIPObject.TableName,
					dataSet.RecorderInfo.TableName,
					dataSet.RecorderPlugins.TableName,
					dataSet.RecordingStorage.TableName,
					dataSet.RegisteredDeviceTypes.TableName,
					dataSet.RuleVariables.TableName,
					dataSet.AudioMessage.TableName,
					dataSet.ServerGroup.TableName,
					dataSet.ServerGroupMembership.TableName,
					dataSet.DriverNumbers.TableName
				};
				Visit_UpdateConfigurationManagerCommand_TablesNotSupported(dataSet, tablesNotSupported);
				Visit_UpdateConfigurationManagerCommand_RecordingServerService(dataSet.RecordingServerService);
				Visit_UpdateConfigurationManagerCommand_RecordingServerServiceConfigurable(dataSet.RecordingServerServiceConfigurable);
				Visit_UpdateConfigurationManagerCommand_Recorders(dataSet.Recorders);
				Visit_UpdateConfigurationManagerCommand_Hardware(dataSet.Hardware, out var deletedHardwareIds);
				Visit_UpdateConfigurationManagerCommand_Compressors(dataSet.Compressors);
				Visit_UpdateConfigurationManagerCommand_DeviceGroups(dataSet.DeviceGroups);
				Visit_UpdateConfigurationManagerCommand_DeviceGroupMembers(dataSet.DeviceGroupMembers, deletedHardwareIds.Count() > 0);
				Visit_UpdateConfigurationManagerCommand_Devices(dataSet.Devices, out var devicesAdded, out var deviceDeleted);
				Visit_UpdateConfigurationManagerCommand_DeviceTracks(dataSet.DeviceTrack, devicesAdded, deviceDeleted);
				Visit_UpdateConfigurationManagerCommand_DeviceLinks(dataSet.DeviceLinks, devicesAdded, deviceDeleted);
				Visit_UpdateConfigurationManagerCommand_DevicePrivacyMask(dataSet.DevicePrivacyMask);
				Visit_UpdateConfigurationManagerCommand_EventTypes(dataSet.EventTypes);
				Visit_UpdateConfigurationManagerCommand_HardwareDefinition(dataSet.HardwareDefinition);
				Visit_UpdateConfigurationManagerCommand_License(dataSet.License);
				Visit_UpdateConfigurationManagerCommand_Matrix(dataSet.Matrix);
				Visit_UpdateConfigurationManagerCommand_NotificationProfiles(dataSet.NotificationProfiles);
				Visit_UpdateConfigurationManagerCommand_NotificationSettings(dataSet.NotificationSettings);
				Visit_UpdateConfigurationManagerCommand_PatrollingProfiles(dataSet.PatrollingProfiles);
				Visit_UpdateConfigurationManagerCommand_RecorderPluginDevice(dataSet.RecorderPluginDevice);
				Visit_UpdateConfigurationManagerCommand_RecordingStorageDevice(dataSet.RecordingStorageDevice);
				Visit_UpdateConfigurationManagerCommand_RegisteredServers(dataSet.RegisteredServers);
				Visit_UpdateConfigurationManagerCommand_Rules(dataSet.Rules);
				Visit_UpdateConfigurationManagerCommand_SystemSettings(dataSet.SystemSettings);
				Visit_UpdateConfigurationManagerCommand_TimeProfile(dataSet.TimeProfile);
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_TablesIgnored(string[] tablesIgnored)
		{
			foreach (string tableName in tablesIgnored)
			{
				OnProcessingTable(new TableInfoEventArgs(tableName));
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_TablesNotSupported(ConfigurationDataSet changes, string[] tablesNotSupported)
		{
			foreach (string text in tablesNotSupported)
			{
				OnProcessingTable(new TableInfoEventArgs(text));
				if (changes.Tables[text].Rows.Count > 0)
				{
					throw new NotSupportedException();
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_Compressors(ConfigurationDataSet.CompressorsDataTable compressorsChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(compressorsChanges.TableName));
			foreach (ConfigurationDataSet.CompressorsRow compressorsChange in compressorsChanges)
			{
				if (compressorsChange.RowState == DataRowState.Deleted)
				{
					throw new NotSupportedException();
				}
			}
			if (compressorsChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_RegisteredServers(ConfigurationDataSet.RegisteredServersDataTable registeredServerChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(registeredServerChanges.TableName));
			if (registeredServerChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_DeviceGroups(ConfigurationDataSet.DeviceGroupsDataTable deviceGroups)
		{
			OnProcessingTable(new TableInfoEventArgs(deviceGroups.TableName));
			if (deviceGroups.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_DeviceGroupMembers(ConfigurationDataSet.DeviceGroupMembersDataTable deviceGroupMembers, bool recorderRemoved)
		{
			OnProcessingTable(new TableInfoEventArgs(deviceGroupMembers.TableName));
			if (deviceGroupMembers.Count > 0 && !recorderRemoved)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_RecordingServerService(ConfigurationDataSet.RecordingServerServiceDataTable recorderServerServiceChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(recorderServerServiceChanges.TableName));
			foreach (ConfigurationDataSet.RecordingServerServiceRow recorderServerServiceChange in recorderServerServiceChanges)
			{
				if (recorderServerServiceChange.RowState == DataRowState.Modified && recorderServerServiceChange.IsFailoverUdpPortChanged())
				{
					using IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent();
					string objectId = recordingServerComponent.QueryRecordingServerIdFromRecordingServerServiceId(recorderServerServiceChange.pkid);
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", objectId);
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_RecordingServerServiceConfigurable(ConfigurationDataSet.RecordingServerServiceConfigurableDataTable recorderServerServiceConfigurableChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(recorderServerServiceConfigurableChanges.TableName));
			foreach (ConfigurationDataSet.RecordingServerServiceConfigurableRow recorderServerServiceConfigurableChange in recorderServerServiceConfigurableChanges)
			{
				RecordingServerServiceInfo recordingServerServiceInfo;
				using (IRecordingServerComponent recordingServerComponent = requestContext.CreateRecordingServerComponent())
				{
					recordingServerServiceInfo = recordingServerComponent.QueryRecordingServerServiceInfo(recorderServerServiceConfigurableChange.GetPKID());
				}
				switch (recorderServerServiceConfigurableChange.RowState)
				{
				case DataRowState.Added:
					throw new NotSupportedException();
				case DataRowState.Deleted:
					switch (recordingServerServiceInfo.ServerType)
					{
					case RecordingServerServiceInfo.ServiceServerType.Recorder:
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "DELETE", recordingServerServiceInfo.ServerId);
						break;
					default:
						throw new NotSupportedException();
					}
					break;
				case DataRowState.Modified:
					switch (recordingServerServiceInfo.ServerType)
					{
					case RecordingServerServiceInfo.ServiceServerType.Recorder:
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recordingServerServiceInfo.ServerId);
						break;
					case RecordingServerServiceInfo.ServiceServerType.Failover:
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.FailoverServer, "GENERIC_WRITE", recordingServerServiceInfo.ServerId);
						break;
					default:
						throw new NotSupportedException();
					}
					break;
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_Recorders(ConfigurationDataSet.RecordersDataTable recorderChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(recorderChanges.TableName));
			foreach (ConfigurationDataSet.RecordersRow recorderChange in recorderChanges)
			{
				switch (recorderChange.RowState)
				{
				case DataRowState.Added:
					throw new NotSupportedException();
				case DataRowState.Deleted:
				{
					string idRecorder = recorderChange.GetIdRecorder(DataRowVersion.Original);
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "DELETE", idRecorder);
					break;
				}
				case DataRowState.Modified:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", recorderChange.IDRecorder);
					break;
				}
			}
		}

		private string TryDecryptPassword(string password)
		{
			try
			{
				password = requestContext.GetEncryptionService().Decrypt(password);
			}
			catch (Exception)
			{
			}
			return password;
		}

		private void Visit_UpdateConfigurationManagerCommand_Hardware(ConfigurationDataSet.HardwareDataTable hardwareChanges, out string[] deletedHardwareIds)
		{
			List<string> list = new List<string>();
			OnProcessingTable(new TableInfoEventArgs(hardwareChanges.TableName));
			foreach (ConfigurationDataSet.HardwareRow hardwareChange in hardwareChanges)
			{
				switch (hardwareChange.RowState)
				{
				case DataRowState.Added:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", hardwareChange.IDRecorder);
					break;
				case DataRowState.Deleted:
				{
					string iDHardware = hardwareChange.GetIDHardware(DataRowVersion.Original);
					list.Add(iDHardware);
					if (!securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "ADMIN_HARDWARE", "/"))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "DELETE", iDHardware);
					}
					break;
				}
				case DataRowState.Modified:
				{
					Dictionary<string, Guid> devicesWithEventChanges = GetDevicesWithEventChanges(hardwareChange);
					foreach (string key in devicesWithEventChanges.Keys)
					{
						securityManager.CheckObjectPermission(requestContext, devicesWithEventChanges[key], "GENERIC_WRITE", key);
					}
					if (IsHardwareChanged(hardwareChange))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_WRITE", hardwareChange.IDHardware);
					}
					hardwareChange.EncryptedPassword = TryDecryptPassword(hardwareChange.EncryptedPassword);
					break;
				}
				}
			}
			deletedHardwareIds = list.ToArray();
		}

		private static Dictionary<string, Guid> GetDevicesWithEventChanges(ConfigurationDataSet.HardwareRow hardwareRow)
		{
			Dictionary<string, Guid> dictionary = new Dictionary<string, Guid>();
			if (hardwareRow.GetEventSettings(DataRowVersion.Original) == hardwareRow.GetEventSettings(DataRowVersion.Current))
			{
				return dictionary;
			}
			string eventSettings = hardwareRow.GetEventSettings(DataRowVersion.Original);
			DriverSettings originalSettings = new DriverSettings(eventSettings);
			var enumerable = (from hwid in originalSettings.GetSectionNames()
				where originalSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventUserData.ToString()) != null
				select hwid).Select(delegate(string hwid)
			{
				string valueByTypeId = originalSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventSource.ToString());
				using StringReader textReader = new StringReader(originalSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventUserData.ToString()));
				XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
				return new
				{
					Hwid = hwid,
					DeviceHwid = valueByTypeId,
					SourceType = xPathNavigator.SelectSingleNode("/data/sourcetype").Value,
					SourceId = xPathNavigator.SelectSingleNode("/data/sourceid").Value,
					EventId = xPathNavigator.SelectSingleNode("/data/eventid").Value
				};
			});
			string eventSettings2 = hardwareRow.GetEventSettings(DataRowVersion.Current);
			DriverSettings currentSettings = new DriverSettings(eventSettings2);
			var enumerable2 = (from hwid in currentSettings.GetSectionNames()
				where currentSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventUserData.ToString()) != null
				select hwid).Select(delegate(string hwid)
			{
				string valueByTypeId = currentSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventSource.ToString());
				using StringReader textReader = new StringReader(currentSettings.GetValueByTypeId(hwid, TypeReferenceIds.EventUserData.ToString()));
				XPathNavigator xPathNavigator = new XPathDocument(textReader).CreateNavigator();
				return new
				{
					Hwid = hwid,
					DeviceHwid = valueByTypeId,
					SourceType = xPathNavigator.SelectSingleNode("/data/sourcetype").Value,
					SourceId = xPathNavigator.SelectSingleNode("/data/sourceid").Value,
					EventId = xPathNavigator.SelectSingleNode("/data/eventid").Value
				};
			});
			foreach (var item in enumerable.Except(enumerable2).Union(enumerable2.Except(enumerable)))
			{
				if (!dictionary.ContainsKey(item.SourceId))
				{
					dictionary.Add(item.SourceId, GetDeviceSecurityNamespace(item.DeviceHwid));
				}
			}
			foreach (var item2 in enumerable.Intersect(enumerable2))
			{
				string[] sectionSettingNames = originalSettings.GetSectionSettingNames(item2.Hwid);
				foreach (string name in sectionSettingNames)
				{
					string text = originalSettings.Get(item2.Hwid, name);
					string text2 = currentSettings.Get(item2.Hwid, name);
					if (text != null && text2 != null && !string.Equals(text, text2, StringComparison.Ordinal) && !dictionary.ContainsKey(item2.SourceId))
					{
						dictionary.Add(item2.SourceId, GetDeviceSecurityNamespace(item2.DeviceHwid));
					}
				}
			}
			return dictionary;
		}

		private static Guid GetDeviceSecurityNamespace(string hwid)
		{
			if (hwid == null)
			{
				throw new ArgumentNullException(hwid);
			}
			if (hwid == "hardware")
			{
				return SecurityNamespaceIdentifier.Hardware;
			}
			DeviceHwid obj = Hwid.ParseHwid(hwid) as DeviceHwid;
			if (obj == null)
			{
				throw new ArgumentException("Not a valid device hwid");
			}
			return obj.Type switch
			{
				DeviceTypeEnum.Camera => SecurityNamespaceIdentifier.Camera, 
				DeviceTypeEnum.Input => SecurityNamespaceIdentifier.Input, 
				DeviceTypeEnum.Output => SecurityNamespaceIdentifier.Output, 
				DeviceTypeEnum.AudioInput => SecurityNamespaceIdentifier.Microphone, 
				DeviceTypeEnum.AudioOutput => SecurityNamespaceIdentifier.Speaker, 
				DeviceTypeEnum.Metadata => SecurityNamespaceIdentifier.Metadata, 
				_ => throw new InvalidSettingException(), 
			};
		}

		private static bool IsHardwareChanged(ConfigurationDataSet.HardwareRow hardwareRow)
		{
			for (int i = 0; i < hardwareRow.Table.Columns.Count; i++)
			{
				if (!hardwareRow.Table.Columns[i].ColumnName.Equals("LastModified") && !hardwareRow.Table.Columns[i].ColumnName.Equals("EventSettings") && !hardwareRow[i, DataRowVersion.Original].Equals(hardwareRow[i, DataRowVersion.Current]))
				{
					return true;
				}
			}
			return false;
		}

		private void Visit_UpdateConfigurationManagerCommand_HardwareDefinition(ConfigurationDataSet.HardwareDefinitionDataTable hardwareDefinitionChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(hardwareDefinitionChanges.TableName));
			foreach (ConfigurationDataSet.HardwareDefinitionRow hardwareDefinitionChange in hardwareDefinitionChanges)
			{
				switch (hardwareDefinitionChange.RowState)
				{
				case DataRowState.Deleted:
				case DataRowState.Modified:
					throw new NotSupportedException();
				}
			}
		}

		private static bool IsCameraChanged(ConfigurationDataSet.DevicesRow deviceRow)
		{
			for (int i = 0; i < deviceRow.Table.Columns.Count; i++)
			{
				if (!deviceRow.Table.Columns[i].ColumnName.Equals("LastModified") && !deviceRow.Table.Columns[i].ColumnName.Equals("Presets") && !deviceRow[i, DataRowVersion.Original].Equals(deviceRow[i, DataRowVersion.Current]))
				{
					return true;
				}
			}
			return false;
		}

		private static bool IsCameraPresetChanged(ConfigurationDataSet.DevicesRow deviceRow, out bool lockedChanged)
		{
			lockedChanged = false;
			if (string.Equals(deviceRow.GetPresetSettings(DataRowVersion.Original), deviceRow.GetPresetSettings(DataRowVersion.Current), StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			PtzPresetCollection ptzPresetCollection = PtzPresetCollection.FromXml(deviceRow.GetPresetSettings(DataRowVersion.Original));
			PtzPresetCollection ptzPresetCollection2 = PtzPresetCollection.FromXml(deviceRow.GetPresetSettings(DataRowVersion.Current));
			foreach (PtzPreset preset in ptzPresetCollection2)
			{
				PtzPreset ptzPreset = ptzPresetCollection.FirstOrDefault((PtzPreset f) => f.Id == preset.Id);
				if (ptzPreset == null)
				{
					if (preset.Locked)
					{
						lockedChanged = true;
						return true;
					}
					continue;
				}
				if (ptzPreset.Locked != preset.Locked)
				{
					lockedChanged = true;
					return true;
				}
				if (ptzPreset.Locked && !ptzPreset.EqualsTo(preset))
				{
					lockedChanged = true;
					return true;
				}
			}
			foreach (PtzPreset preset2 in ptzPresetCollection)
			{
				if (ptzPresetCollection2.FirstOrDefault((PtzPreset f) => f.Id == preset2.Id) == null && preset2.Locked)
				{
					lockedChanged = true;
					return true;
				}
			}
			return true;
		}

		private void Visit_UpdateConfigurationManagerCommand_Devices(ConfigurationDataSet.DevicesDataTable deviceChanges, out string[] devicesAdded, out string[] deviceDeleted)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			OnProcessingTable(new TableInfoEventArgs(deviceChanges.TableName));
			foreach (ConfigurationDataSet.DevicesRow deviceChange in deviceChanges)
			{
				switch (deviceChange.RowState)
				{
				case DataRowState.Added:
					list.Add(deviceChange.IDDevice);
					break;
				case DataRowState.Deleted:
				{
					string iDDevice = deviceChange.GetIDDevice(DataRowVersion.Original);
					list2.Add(iDDevice);
					break;
				}
				case DataRowState.Modified:
					if (RegisteredDeviceType.IsCamera(deviceChange.DeviceType))
					{
						if (IsCameraChanged(deviceChange))
						{
							securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", deviceChange.IDDevice);
						}
						if (IsCameraPresetChanged(deviceChange, out var lockedChanged))
						{
							securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", deviceChange.IDDevice);
							if (lockedChanged)
							{
								securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_LOCK_PTZ_PRESET", deviceChange.IDDevice);
							}
						}
					}
					else if (RegisteredDeviceType.IsMicrophone(deviceChange.DeviceType))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "GENERIC_WRITE", deviceChange.IDDevice);
					}
					else if (RegisteredDeviceType.IsSpeaker(deviceChange.DeviceType))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "GENERIC_WRITE", deviceChange.IDDevice);
					}
					else if (RegisteredDeviceType.IsInput(deviceChange.DeviceType))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Input, "GENERIC_WRITE", deviceChange.IDDevice);
					}
					else if (RegisteredDeviceType.IsOutput(deviceChange.DeviceType))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Output, "GENERIC_WRITE", deviceChange.IDDevice);
					}
					else if (RegisteredDeviceType.IsMetadata(deviceChange.DeviceType))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "GENERIC_WRITE", deviceChange.IDDevice);
					}
					break;
				}
			}
			devicesAdded = list.ToArray();
			deviceDeleted = list2.ToArray();
		}

		private void Visit_UpdateConfigurationManagerCommand_DeviceTracks(ConfigurationDataSet.DeviceTrackDataTable deviceTrackChanges, string[] devicesAdded, string[] devicesDeleted)
		{
			OnProcessingTable(new TableInfoEventArgs(deviceTrackChanges.TableName));
			foreach (ConfigurationDataSet.DeviceTrackRow deviceTrackChange in deviceTrackChanges)
			{
				string deviceId = deviceTrackChange.GetIDDevice();
				switch (deviceTrackChange.RowState)
				{
				case DataRowState.Added:
					if (devicesAdded == null || !devicesAdded.Any((string f) => f.Equals(deviceId, StringComparison.OrdinalIgnoreCase)))
					{
						Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					}
					break;
				case DataRowState.Modified:
				{
					Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
					securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					break;
				}
				case DataRowState.Deleted:
					if (devicesDeleted == null || !devicesDeleted.Any((string f) => f.Equals(deviceId, StringComparison.OrdinalIgnoreCase)))
					{
						Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					}
					break;
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_DeviceLinks(ConfigurationDataSet.DeviceLinksDataTable deviceLinksChanges, string[] devicesAdded, string[] devicesDeleted)
		{
			OnProcessingTable(new TableInfoEventArgs(deviceLinksChanges.TableName));
			foreach (ConfigurationDataSet.DeviceLinksRow row in deviceLinksChanges)
			{
				string deviceId;
				switch (row.RowState)
				{
				case DataRowState.Added:
					if (devicesAdded == null || !devicesAdded.Any((string f) => f == row.IDDevice || f == row.IDDeviceLinked))
					{
						Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, row.IDDevice);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", row.IDDevice);
						namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, row.IDDeviceLinked);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", row.IDDeviceLinked);
					}
					break;
				case DataRowState.Modified:
				{
					Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, row.IDDevice);
					securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", row.IDDevice);
					namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, row.IDDeviceLinked);
					securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", row.IDDeviceLinked);
					deviceId = row.GetIDDevice(DataRowVersion.Original);
					namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
					securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					deviceId = row.GetIDDeviceLinked(DataRowVersion.Original);
					namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
					securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					break;
				}
				case DataRowState.Deleted:
				{
					deviceId = row.GetIDDevice(DataRowVersion.Original);
					string deviceLinkId = row.GetIDDeviceLinked(DataRowVersion.Original);
					if (devicesDeleted == null || !devicesDeleted.Any((string f) => f == deviceId || f == deviceLinkId))
					{
						Guid namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
						deviceId = row.GetIDDeviceLinked(DataRowVersion.Original);
						namespaceId = deviceTypeLookup.QueryDeviceSecurityNamespaceId(requestContext, deviceId);
						securityManager.CheckObjectPermission(requestContext, namespaceId, "GENERIC_WRITE", deviceId);
					}
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_DevicePrivacyMask(ConfigurationDataSet.DevicePrivacyMaskDataTable devicePrivacyMaskChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(devicePrivacyMaskChanges.TableName));
			foreach (ConfigurationDataSet.DevicePrivacyMaskRow devicePrivacyMaskChange in devicePrivacyMaskChanges)
			{
				DataRowState rowState = devicePrivacyMaskChange.RowState;
				if (rowState != DataRowState.Added && rowState != DataRowState.Deleted && rowState == DataRowState.Modified)
				{
					throw new NotSupportedException();
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_RecorderPluginDevice(ConfigurationDataSet.RecorderPluginDeviceDataTable recorderPluginDeviceChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(recorderPluginDeviceChanges.TableName));
			foreach (ConfigurationDataSet.RecorderPluginDeviceRow recorderPluginDeviceChange in recorderPluginDeviceChanges)
			{
				switch (recorderPluginDeviceChange.RowState)
				{
				case DataRowState.Added:
					if (!(new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.MotionPluginId))
					{
						if (!(new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.ImmerVisionPlugin) && !(new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.GenericDewarpPlugin))
						{
							throw new NotSupportedException();
						}
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", recorderPluginDeviceChange.IDDevice);
					}
					break;
				case DataRowState.Modified:
					if (new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.MotionPluginId)
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", recorderPluginDeviceChange.IDDevice);
						break;
					}
					if (new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.ImmerVisionPlugin || new Guid(recorderPluginDeviceChange.IDRecorderPlugin) == WellKnownPluginIds.GenericDewarpPlugin)
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", recorderPluginDeviceChange.IDDevice);
						break;
					}
					throw new NotSupportedException();
				case DataRowState.Deleted:
				{
					string iDRecorderPlugin = recorderPluginDeviceChange.GetIDRecorderPlugin(DataRowVersion.Original);
					if (!(new Guid(iDRecorderPlugin) == WellKnownPluginIds.MotionPluginId))
					{
						if (!(new Guid(iDRecorderPlugin) == WellKnownPluginIds.ImmerVisionPlugin) && !(new Guid(iDRecorderPlugin) == WellKnownPluginIds.GenericDewarpPlugin))
						{
							throw new NotSupportedException();
						}
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_WRITE", recorderPluginDeviceChange.GetIDDevice(DataRowVersion.Original));
					}
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_PatrollingProfiles(ConfigurationDataSet.PatrollingProfilesDataTable patrollingProfileChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(patrollingProfileChanges.TableName));
			foreach (ConfigurationDataSet.PatrollingProfilesRow patrollingProfileChange in patrollingProfileChanges)
			{
				switch (patrollingProfileChange.RowState)
				{
				case DataRowState.Added:
				case DataRowState.Modified:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", patrollingProfileChange.IDDevice);
					break;
				case DataRowState.Deleted:
				{
					string iDDevice = patrollingProfileChange.GetIDDevice(DataRowVersion.Original);
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "MANAGE_PTZ_PRESET", iDDevice);
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_RecordingStorageDevice(ConfigurationDataSet.RecordingStorageDeviceDataTable recordingStorageDeviceChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(recordingStorageDeviceChanges.TableName));
			foreach (ConfigurationDataSet.RecordingStorageDeviceRow recordingStorageDeviceChange in recordingStorageDeviceChanges)
			{
				switch (recordingStorageDeviceChange.RowState)
				{
				case DataRowState.Deleted:
				case DataRowState.Modified:
					throw new NotSupportedException();
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_Rules(ConfigurationDataSet.RulesDataTable ruleChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(ruleChanges.TableName));
			foreach (ConfigurationDataSet.RulesRow ruleChange in ruleChanges)
			{
				switch (ruleChange.RowState)
				{
				case DataRowState.Added:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "CREATE_RULE", "/");
					break;
				case DataRowState.Modified:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "GENERIC_WRITE", ruleChange.IDRule);
					break;
				case DataRowState.Deleted:
				{
					string iDRule = ruleChange.GetIDRule(DataRowVersion.Original);
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "DELETE", iDRule);
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_TimeProfile(ConfigurationDataSet.TimeProfileDataTable timeProfileChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(timeProfileChanges.TableName));
			if (timeProfileChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_SystemSettings(ConfigurationDataSet.SystemSettingsDataTable systemSettingsChanges)
		{
			bool flag = false;
			OnProcessingTable(new TableInfoEventArgs(systemSettingsChanges.TableName));
			foreach (ConfigurationDataSet.SystemSettingsRow systemSettingsChange in systemSettingsChanges)
			{
				if (systemSettingsChange.RowState == DataRowState.Deleted)
				{
					throw new NotSupportedException();
				}
				if (systemSettingsChange.Name.Equals("ManagementClientUIProfilesXml", StringComparison.OrdinalIgnoreCase))
				{
					flag = true;
				}
			}
			if (flag)
			{
				securityManager.CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
				if (systemSettingsChanges.Count > 1)
				{
					securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
				}
			}
			else if (systemSettingsChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_EventTypes(ConfigurationDataSet.EventTypesDataTable eventTypeChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(eventTypeChanges.TableName));
			foreach (ConfigurationDataSet.EventTypesRow eventTypeChange in eventTypeChanges)
			{
				switch (eventTypeChange.RowState)
				{
				case DataRowState.Added:
					if (!string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.External, StringComparison.Ordinal) && !string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal))
					{
						throw new NotSupportedException();
					}
					if (string.Equals(eventTypeChange.GeneratorSubType, EventGeneratorSubtypeName.UserDefined, StringComparison.OrdinalIgnoreCase))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "CREATE_USER_DEFINED_EVENT", "/");
					}
					if (string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal) && !string.Equals(eventTypeChange.GeneratorSubType, EventGeneratorSubtypeName.ICP, StringComparison.OrdinalIgnoreCase))
					{
						throw new NotSupportedException();
					}
					break;
				case DataRowState.Modified:
					if (!string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.External, StringComparison.Ordinal) && !string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal))
					{
						throw new NotSupportedException();
					}
					if (string.Equals(eventTypeChange.GeneratorSubType, EventGeneratorSubtypeName.UserDefined, StringComparison.OrdinalIgnoreCase))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_WRITE", eventTypeChange.IDEventType);
					}
					if (string.Equals(eventTypeChange.GeneratorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal) && !string.Equals(eventTypeChange.GeneratorSubType, EventGeneratorSubtypeName.ICP, StringComparison.OrdinalIgnoreCase))
					{
						throw new NotSupportedException();
					}
					break;
				case DataRowState.Deleted:
				{
					string generatorType = eventTypeChange.GetGeneratorType(DataRowVersion.Original);
					if (!string.Equals(generatorType, EventGeneratorTypeName.External, StringComparison.Ordinal) && !string.Equals(generatorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal))
					{
						throw new NotSupportedException();
					}
					string iDEventType = eventTypeChange.GetIDEventType(DataRowVersion.Original);
					string generatorSubType = eventTypeChange.GetGeneratorSubType(DataRowVersion.Original);
					if (string.Equals(generatorSubType, EventGeneratorSubtypeName.UserDefined, StringComparison.OrdinalIgnoreCase))
					{
						securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "DELETE", iDEventType);
					}
					if (string.Equals(generatorType, EventGeneratorTypeName.HardwareDriver, StringComparison.Ordinal) && !string.Equals(generatorSubType, EventGeneratorSubtypeName.ICP, StringComparison.OrdinalIgnoreCase))
					{
						throw new NotSupportedException();
					}
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_License(ConfigurationDataSet.LicenseDataTable licenseChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(licenseChanges.TableName));
			if (licenseChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_Matrix(ConfigurationDataSet.MatrixDataTable matrixChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(matrixChanges.TableName));
			foreach (ConfigurationDataSet.MatrixRow matrixChange in matrixChanges)
			{
				switch (matrixChange.RowState)
				{
				case DataRowState.Added:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "CREATE_MATRIX", "/");
					break;
				case DataRowState.Modified:
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_WRITE", matrixChange.IDMatrix);
					break;
				case DataRowState.Deleted:
				{
					string iDMatrix = matrixChange.GetIDMatrix(DataRowVersion.Original);
					securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "DELETE", iDMatrix);
					break;
				}
				}
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_NotificationProfiles(ConfigurationDataSet.NotificationProfilesDataTable notificationProfileChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(notificationProfileChanges.TableName));
			if (notificationProfileChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		private void Visit_UpdateConfigurationManagerCommand_NotificationSettings(ConfigurationDataSet.NotificationSettingsDataTable notificationSettingsChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(notificationSettingsChanges.TableName));
			if (notificationSettingsChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
			}
		}

		public void Visit(UpdateSecurityManagerCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			SecurityDataSet dataSet = command.SecurityManagerDto.DataSet;
			if (dataSet != null)
			{
				Visit_UpdateSecurityManagerCommand_SecurityPermissions(dataSet.SecurityPermissions);
				Visit_UpdateSecurityManagerCommand_SecurityAttributes(dataSet.SecurityAttributes);
				Visit_UpdateSecurityManagerCommand_SecurityPermissionsDevice(dataSet.SecurityPermissionsDevice);
				Visit_UpdateSecurityManagerCommand_SecurityPermissionsMatrix(dataSet.SecurityPermissionsMatrix);
				Visit_UpdateSecurityManagerCommand_TimeProfileDefaultRoleRelation(dataSet.TimeProfileDefaultRoleRelation);
				if (dataSet.HasChanges() && requestContext is GeminiRequestContext geminiRequestContext)
				{
					geminiRequestContext.GetService<SecurityManagerService>().ForceReloadOfAllNamespaces(geminiRequestContext);
				}
			}
		}

		private void Visit_UpdateSecurityManagerCommand_SecurityAttributes(SecurityDataSet.SecurityAttributesDataTable securityAttributeChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(securityAttributeChanges.TableName));
			if (securityAttributeChanges.Count > 0)
			{
				throw new NotSupportedException();
			}
		}

		private void Visit_UpdateSecurityManagerCommand_SecurityPermissions(SecurityDataSet.SecurityPermissionsDataTable securityPermissionsChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(securityPermissionsChanges.TableName));
			if (securityPermissionsChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
			}
		}

		private void Visit_UpdateSecurityManagerCommand_SecurityPermissionsDevice(SecurityDataSet.SecurityPermissionsDeviceDataTable securityPermissionsDeviceChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(securityPermissionsDeviceChanges.TableName));
			foreach (SecurityDataSet.SecurityPermissionsDeviceRow securityPermissionsDeviceChange in securityPermissionsDeviceChanges)
			{
				string deviceId = securityPermissionsDeviceChange.GetDeviceId();
				IGeminiSecurityNamespace geminiSecurityNamespace = deviceTypeLookup.QueryDeviceTypeFromSecurityAttributeValue(securityPermissionsDeviceChange.GetSecurityAttributeId()) switch
				{
					DeviceType.Camera => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Camera), 
					DeviceType.Microphone => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Microphone), 
					DeviceType.Speaker => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Speaker), 
					DeviceType.Input => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Input), 
					DeviceType.Output => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Output), 
					DeviceType.Metadata => requestContext.GetSecurityManagerService().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Metadata), 
					DeviceType.Unknown => throw new UnauthorizedAccessException(VmoResources.VMO61008), 
					_ => throw new NotSupportedException(), 
				};
				if (!securityManager.HasObjectPermission(requestContext, geminiSecurityNamespace.NamespaceInfo.NamespaceGuid, "ADMIN_SECURITY", deviceId, checkAdministrator: true))
				{
					string roleId = securityPermissionsDeviceChange.GetRoleId();
					string roleName = requestContext.GetNameLookupService().GetRoleName(requestContext, new Guid(roleId));
					string objectName = requestContext.GetNameLookupService().GetObjectName(requestContext, geminiSecurityNamespace.NamespaceInfo.NamespaceId, deviceId);
					SecuredSecurityManager.FailedPermissionAuditLog(requestContext, geminiSecurityNamespace, deviceId, new Guid(roleId), roleName, objectName);
					throw new UnauthorizedAccessException(VmoResources.VMO61008);
				}
			}
		}

		private void Visit_UpdateSecurityManagerCommand_SecurityPermissionsMatrix(SecurityDataSet.SecurityPermissionsMatrixDataTable securityPermissionsMatrixChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(securityPermissionsMatrixChanges.TableName));
			foreach (SecurityDataSet.SecurityPermissionsMatrixRow securityPermissionsMatrixChange in securityPermissionsMatrixChanges)
			{
				string matrixId = securityPermissionsMatrixChange.GetMatrixId();
				if (!securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "ADMIN_SECURITY", matrixId))
				{
					string roleId = securityPermissionsMatrixChange.GetRoleId();
					string roleName = requestContext.GetService<NameLookupService>().GetRoleName(requestContext, new Guid(roleId));
					string objectName = requestContext.GetService<NameLookupService>().GetObjectName(requestContext, SecurityNamespaceIdentifier.Matrix.ToString(), matrixId);
					IGeminiSecurityNamespace securityNamespace = requestContext.GetService<SecurityManagerService>().GetSecurityNamespace(requestContext, SecurityNamespaceIdentifier.Matrix);
					SecuredSecurityManager.FailedPermissionAuditLog(requestContext, securityNamespace, matrixId, new Guid(roleId), roleName, objectName);
					throw new UnauthorizedAccessException(VmoResources.VMO61008);
				}
			}
		}

		private void Visit_UpdateSecurityManagerCommand_TimeProfileDefaultRoleRelation(SecurityDataSet.TimeProfileDefaultRoleRelationDataTable timeProfileDefaultRoleRelationChanges)
		{
			OnProcessingTable(new TableInfoEventArgs(timeProfileDefaultRoleRelationChanges.TableName));
			if (timeProfileDefaultRoleRelationChanges.Count > 0)
			{
				securityManager.CheckApplicationPermission(requestContext, "ADMIN_SECURITY");
			}
		}

		public void Visit(QueryDeviceEvidenceLockSizesCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(EvidenceLockOnOldRecorderCommand evidenceLockOnOldRecorderCommand)
		{
			throw new NotSupportedException();
		}

		public void Visit(EvidenceLockOnOldRecorderCommand2 evidenceLockOnOldRecorderCommand)
		{
			throw new NotSupportedException();
		}

		public void Visit(FileUploadCommand command)
		{
			throw new NotSupportedException();
		}

		public void Visit(FileUploadToRecorderCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_WRITE", command.RecorderId.ToString());
		}

		public void Visit(UploadFileChunkCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckApplicationPermission(requestContext, "GENERIC_WRITE");
		}

		public void Visit(CheckFilePresentOnRecorderCommand command)
		{
			throw new NotImplementedException();
		}

		public void Visit(CheckExternalRecorderRegistrationCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", command.HardwareId.ToString());
		}

		public void Visit(CheckMsFailoverStatusCommand command)
		{
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			securityManager.CheckApplicationPermission(requestContext, "GENERIC_READ");
		}
	}

	public class TableInfoEventArgs : EventArgs
	{
		public string TableName { get; private set; }

		public TableInfoEventArgs(string tableName)
		{
			TableName = tableName;
		}
	}

	public ServerProxyService()
		: base(connect: false)
	{
	}

	public SecurityToken Login(string currentToken)
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("Login");
			methodInformation.AddParameter("currentToken", currentToken);
			EnterWebMethod(methodInformation);
			BasicCallContextInfo callContextInfo = GetCallContextInfo();
			bool flag = false;
			if (!string.IsNullOrEmpty(currentToken))
			{
				using TokenValidator tokenValidator = new TokenValidator(base.RequestContext);
				flag = tokenValidator.CheckToken(currentToken) == TokenStatus.Ok;
			}
			SecurityToken securityToken = ServerSecurityToken.ManagementLogin(base.RequestContext, callContextInfo.LocationIPAddress);
			IDualAuthorizationHandling dualAuthorizationHandling = new DualAuthorizationHandling();
			bool flag2 = false;
			string authorizationId = string.Empty;
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				authorizationId = GetAuthorizationIdFromHeader();
				flag2 = dualAuthorizationHandling.CheckDualAuthorization(base.RequestContext, authorizationId, AuthorizationActionType.LogOn);
			}
			if (flag2)
			{
				dualAuthorizationHandling.ExtendCookieTime(base.RequestContext, authorizationId, securityToken.Expires);
			}
			if (!flag)
			{
				using VmoLogManager vmoLogManager = new VmoLogManager(base.RequestContext);
				vmoLogManager.LogEntryAuditLogger.AuditLoginSuccessfully(LogEntryAuditLogger.LoginFrom.Administration, base.RequestContext.UserDomainAccountName, callContextInfo.LocationIPAddress);
			}
			CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => serverWebServiceClient.GetVmoServerPort(), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Server Service communication failed: Login()");
			});
			return securityToken;
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void Logout(string currentToken)
	{
		try
		{
			if (string.IsNullOrEmpty(currentToken))
			{
				return;
			}
			using (TokenValidator tokenValidator = new TokenValidator(base.RequestContext))
			{
				SecurityToken token = tokenValidator.GetToken(currentToken);
				if (token != null && token.ManagementClientToken)
				{
					BasicCallContextInfo callContextInfo = GetCallContextInfo();
					using VmoLogManager vmoLogManager = new VmoLogManager(base.RequestContext);
					vmoLogManager.LogEntryAuditLogger.AuditLogoutManagementClient(base.RequestContext.UserDomainAccountName, callContextInfo.LocationIPAddress);
				}
			}
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				string authorizationIdFromHeader = GetAuthorizationIdFromHeader();
				((IDualAuthorizationHandling)new DualAuthorizationHandling()).DeleteCookie((IGeminiRequestContext)base.RequestContext, authorizationIdFromHeader, AuthorizationActionType.LogOn);
			}
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string Authorize(string userDomainName, AuthorizationActionType type)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("Authorize");
			methodInformation.AddParameter("userDomainName", userDomainName);
			methodInformation.AddParameter("type", type);
			EnterWebMethod(methodInformation);
			if (GeminiApplicationCore.FeatureCacheManager.IsFeatureLicensed(base.RequestContext, "DualAuthorization"))
			{
				string sidFromAccountName = IdentityAccessor.GetSidFromAccountName(base.RequestContext, userDomainName);
				return ((IDualAuthorizationHandling)new DualAuthorizationHandling()).Authorize((IGeminiRequestContext)base.RequestContext, sidFromAccountName, userDomainName, type);
			}
			return string.Empty;
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Change[] QueryChanges(DateTime changesFrom)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryChanges");
			methodInformation.AddParameter("changesFrom", changesFrom);
			EnterWebMethod(methodInformation);
			IGeminiRequestContext requestContext = base.RequestContext;
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
			return ClientConfigurationProvider.QueryChanges(base.RequestContext, changesFrom);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RecorderInfo QueryRecorderInfo(Guid recorderId, string serverHostAddress)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRecorderInfo");
			methodInformation.AddParameter("recorderId", recorderId);
			methodInformation.AddParameter("serverHostAddress", serverHostAddress);
			EnterWebMethod(methodInformation);
			IGeminiRequestContext requestContext = base.RequestContext;
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
			bool isPublicRequest = IPHelper.IsNatRequest(requestContext.RemoteIPAddress, requestContext.LocalIPAddress);
			return ClientConfigurationProvider.GetRecorderInfoOnly(requestContext, recorderId, isPublicRequest);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void RegisterPluginIntegrations(string token, string instanceId, IntegrationType integrationType, PluginIntegration[] pluginIntegrations)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RegisterPluginIntegrations");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("instanceId", instanceId);
			methodInformation.AddParameter("integrationType", integrationType);
			methodInformation.AddArrayParameter("integrationType", pluginIntegrations);
			EnterWebMethod(methodInformation);
			IntegrationRegistration.CreatePluginRegistrations(base.RequestContext, instanceId, integrationType, pluginIntegrations);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedDataReference MarkedDataGetNewReference(string token, Guid[] deviceIds, bool live)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataGetNewReference");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("live", live);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataGetNewReference(base.RequestContext, deviceIds, live);
		}
		catch (Exception ex)
		{
			new WcfWebServiceManagerExceptionHandler().HandleException(ex, null);
			throw;
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult MarkedDataCreate(string token, Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataCreate");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("markedDataId", markedDataId);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeTag", timeTag);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("markedDataType", markedDataType);
			methodInformation.AddParameter("retentionExpire", retentionExpire);
			methodInformation.AddParameter("retentionOption", retentionOption);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataCreate(base.RequestContext, markedDataId, deviceIds, timeStart, timeTag, timeEnd, useRetention, reference, header, description, markedDataType, retentionExpire, retentionOption);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult MarkedDataUpdate(Guid markedDataId, Guid[] deviceIds, DateTime timeStart, DateTime timeTag, DateTime timeEnd, string reference, string header, string description, int markedDataType, bool useRetention, DateTime retentionExpire, RetentionOption retentionOption)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataUpdate");
			methodInformation.AddParameter("markedDataId", markedDataId);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("timeStart", timeStart);
			methodInformation.AddParameter("timeTag", timeTag);
			methodInformation.AddParameter("timeEnd", timeEnd);
			methodInformation.AddParameter("reference", reference);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("markedDataType", markedDataType);
			methodInformation.AddParameter("useRetention", useRetention);
			methodInformation.AddParameter("retentionExpire", retentionExpire);
			methodInformation.AddParameter("retentionOption", retentionOption);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataUpdate(base.RequestContext, markedDataId, deviceIds, timeStart, timeTag, timeEnd, useRetention, reference, header, description, markedDataType, retentionExpire, retentionOption);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public MarkedDataResult[] MarkedDataDelete(string token, Guid[] markedDataIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataDelete");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("markedDataIds", markedDataIds);
			EnterWebMethod(methodInformation);
			List<MarkedDataResult> list = new List<MarkedDataResult>();
			foreach (Guid markedDataId in markedDataIds)
			{
				MarkedDataResult item = ServerCommandServiceMarkedDataHandling.MarkedDataDelete(base.RequestContext, markedDataId);
				list.Add(item);
			}
			return list.ToArray();
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData MarkedDataGet(string token, Guid markedDataId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataGet");
			methodInformation.AddParameter("token", token);
			methodInformation.AddParameter("markedDataId", markedDataId);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataGet(base.RequestContext, markedDataId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch(string token, Guid[] deviceIds, string searchText, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataSearch");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFrom", retentionExpireFrom);
			methodInformation.AddParameter("retentionExpireTo", retentionExpireTo);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataSearch(base.RequestContext, deviceIds, searchText, searchHeader: true, searchDescription: true, searchReference: true, searchUser: true, null, null, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.MarkedData[] MarkedDataSearch2(string token, Guid[] deviceIds, string searchText, bool searchHeader, bool searchDescription, bool searchReference, bool searchUser, string headerFilter, string descriptionFilter, string[] users, DateTime createdFromTime, DateTime createdToTime, DateTime markedDataFromTime, DateTime markedDataToTime, DateTime tagTimeFromTime, DateTime tagTimeToTime, DateTime retentionExpireFrom, DateTime retentionExpireTo, int pageIndex, int pageSize, SortOrderOption sortOrder, bool sortAscending)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("MarkedDataSearch2");
			methodInformation.AddParameter("token", token);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("searchText", searchText);
			methodInformation.AddParameter("searchHeader", searchHeader);
			methodInformation.AddParameter("searchDescription", searchDescription);
			methodInformation.AddParameter("searchReference", searchReference);
			methodInformation.AddParameter("searchUser", searchUser);
			methodInformation.AddParameter("headerFilter", headerFilter);
			methodInformation.AddParameter("descriptionFilter", descriptionFilter);
			methodInformation.AddArrayParameter("users", users);
			methodInformation.AddParameter("createdFromTime", createdFromTime);
			methodInformation.AddParameter("createdToTime", createdToTime);
			methodInformation.AddParameter("markedDataFromTime", markedDataFromTime);
			methodInformation.AddParameter("markedDataToTime", markedDataToTime);
			methodInformation.AddParameter("tagTimeFromTime", tagTimeFromTime);
			methodInformation.AddParameter("tagTimeToTime", tagTimeToTime);
			methodInformation.AddParameter("retentionExpireFrom", retentionExpireFrom);
			methodInformation.AddParameter("retentionExpireTo", retentionExpireTo);
			methodInformation.AddParameter("pageIndex", pageIndex);
			methodInformation.AddParameter("pageSize", pageSize);
			methodInformation.AddParameter("sortOrder", sortOrder);
			methodInformation.AddParameter("sortAscending", sortAscending);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceMarkedDataHandling.MarkedDataSearch(base.RequestContext, deviceIds, searchText, searchHeader, searchDescription, searchReference, searchUser, headerFilter, descriptionFilter, users, createdFromTime, createdToTime, markedDataFromTime, markedDataToTime, tagTimeFromTime, tagTimeToTime, retentionExpireFrom, retentionExpireTo, pageIndex, pageSize, sortOrder, sortAscending);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public bool RecordingServerContainMarkedData(string recordingServerId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RecordingServerContainMarkedData");
			methodInformation.AddParameter("recordingServerId", recordingServerId);
			EnterWebMethod(methodInformation);
			IGeminiRequestContext requestContext = base.RequestContext;
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
			return ServerCommandServiceMarkedDataHandling.RecordingServerContainMarkedData(base.RequestContext, recordingServerId);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaCreate(Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaCreate");
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("startTime", startTime);
			methodInformation.AddParameter("endTime", endTime);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaCreate(base.RequestContext, id, deviceIds, header, description, startTime, endTime);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaUpdate(Guid id, Guid[] deviceIdsToAdd, Guid[] deviceIdsToRemove, string header, string description, DateTime startTime, DateTime endTime)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaUpdate");
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("deviceIdsToAdd", deviceIdsToAdd);
			methodInformation.AddArrayParameter("deviceIdsToRemove", deviceIdsToRemove);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("startTime", startTime);
			methodInformation.AddParameter("endTime", endTime);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaUpdate(base.RequestContext, id, deviceIdsToAdd, deviceIdsToRemove, header, description, startTime, endTime);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaDelete(Guid id)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaDelete");
			methodInformation.AddParameter("id", id);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaDelete(base.RequestContext, id);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public VideoOS.WebService.Common.ServerCommandService.RestrictedMedia[] QueryAll()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAll");
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.QueryAll(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveModeEnter(Guid[] deviceIds, DateTime startTime)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveModeEnter");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("startTime", startTime);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveModeEnter(base.RequestContext, deviceIds, startTime);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaResult RestrictedMediaLiveModeExit(Guid id, Guid[] deviceIds, string header, string description, DateTime startTime, DateTime endTime)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveModeExit");
			methodInformation.AddParameter("id", id);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("header", header);
			methodInformation.AddParameter("description", description);
			methodInformation.AddParameter("startTime", startTime);
			methodInformation.AddParameter("endTime", endTime);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveModeExit(base.RequestContext, id, deviceIds, header, description, startTime, endTime);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveUpdate(Guid[] deviceIds, DateTime startTime)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveUpdate");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			methodInformation.AddParameter("startTime", startTime);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveUpdate(base.RequestContext, deviceIds, startTime);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLiveResult RestrictedMediaLiveDelete(Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("RestrictedMediaLiveDelete");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.RestrictedMediaLiveDelete(base.RequestContext, deviceIds);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public RestrictedMediaLive[] QueryAllLive()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryAllLive");
			EnterWebMethod(methodInformation);
			return ServerCommandServiceRestrictedMediaHandling.QueryAllLive(base.RequestContext);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetServerVersion()
	{
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetServerVersion");
			EnterWebMethod(methodInformation);
			return GetServerVersion(base.RequestContext);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static string GetServerVersion(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		return ServerVersion.GetServerVersion(Assembly.GetExecutingAssembly());
	}

	public string GetUserHostAddress()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetUserHostAddress");
			EnterWebMethod(methodInformation);
			return GetUserHostAddress(base.RequestContext);
		}
		catch (OutOfMemoryException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			throw HandleException(ex2);
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static string GetUserHostAddress(IGeminiRequestContext requestContext)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		string text = requestContext.LocalIPAddress;
		if (!IPAddress.TryParse(text, out var _))
		{
			text = string.Empty;
		}
		string remoteIPAddress = requestContext.RemoteIPAddress;
		if (IPHelper.IsNatRequest(remoteIPAddress, text))
		{
			return remoteIPAddress;
		}
		return string.Empty;
	}

	public void SendCommand(Guid proxyId, CommandBase command)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SendCommand");
			methodInformation.AddParameter("proxyId", proxyId);
			methodInformation.AddParameter("command", command);
			EnterWebMethod(methodInformation);
			if (command == null)
			{
				throw new ArgumentNullException("command");
			}
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				IDeviceTypeLookup instance = DeviceTypeLookup.Instance;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				IVisitableCommand obj = (command as IVisitableCommand) ?? throw new ArgumentException("The command type is not supported.");
				SendCommandSecurityVisitor visitor = new SendCommandSecurityVisitor(requestContext, instance);
				obj.Accept(visitor);
				BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				serverWebServiceClient.SendCommand(proxyId, command, basicCallContextInfo);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: SendCommand()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ResponseBase[] GetResponses(Guid proxyId, TimeSpan timeout)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetResponses");
			methodInformation.AddParameter("proxyId", proxyId);
			methodInformation.AddParameter("timeout", timeout);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				ResponseBase[] responses = serverWebServiceClient.GetResponses(proxyId, timeout);
				if (responses == null)
				{
					return (ResponseBase[])null;
				}
				List<ResponseBase> list = new List<ResponseBase>();
				GetResponsesSecurityVisitor visitor = new GetResponsesSecurityVisitor(requestContext);
				ResponseBase[] array = responses;
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i] is CommonResponseBase commonResponseBase)
					{
						try
						{
							commonResponseBase.Accept(visitor);
						}
						catch (UnauthorizedAccessException)
						{
							continue;
						}
						list.Add(commonResponseBase);
					}
				}
				return list.ToArray();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetResponses('{0}', '{1}')", proxyId.ToString(), timeout.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void ReleaseGetResponses(Guid proxyId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ReleaseGetResponses");
			methodInformation.AddParameter("proxyId", proxyId);
			EnterWebMethod(methodInformation);
			IGeminiRequestContext requestContext = base.RequestContext;
			requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				serverWebServiceClient.ReleaseGetResponses(proxyId);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: ReleaseGetResponses('{0}')", proxyId.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByRole(Guid roleId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSecurityConfigurationDtoByRole");
			methodInformation.AddParameter("roleId", roleId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetSecurityConfigurationDtoByRole(roleId);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetSecurityConfigurationDtoByRole('{0}')", roleId.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public SecurityManagerDataTransferObject GetSecurityConfigurationDtoByTables(string[] tableNames)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSecurityConfigurationDtoByTables");
			methodInformation.AddArrayParameter("tableNames", tableNames);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetSecurityConfigurationDtoByTables(tableNames);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetSecurityConfigurationDtoByTables('[{0}]')", string.Join(",", tableNames));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void Log(LogEntry logEntry)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("Log");
			methodInformation.AddParameter("logEntry", logEntry);
			EnterWebMethod(methodInformation);
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				serverWebServiceClient.Log(logEntry);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: Log('ID = {0}')", logEntry.LogId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public void LogLogs(LogEntry[] logEntries)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("LogLogs");
			methodInformation.AddArrayParameter("logEntries", logEntries);
			EnterWebMethod(methodInformation);
			CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				serverWebServiceClient.LogLogs(logEntries);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: LogLogs('Count = {0}, IDs=[{1}]')", logEntries.Length, string.Join(",", logEntries.Select((LogEntry e) => e.LogId)));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public LogQueryResult QueryLog(LogQuery logQuery)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryLog");
			methodInformation.AddParameter("logQuery", logQuery);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.QueryLog(logQuery);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryLog('{0}')", logQuery.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public byte[] QueryLogExport(LogQuery logQuery, int codePage, CultureInfo culture)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryLogExport");
			methodInformation.AddParameter("logQuery", logQuery);
			methodInformation.AddParameter("codePage", codePage);
			methodInformation.AddParameter("culture", culture);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.QueryLogExport(logQuery, codePage, culture);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryLogExport('{0}', '{1}', '{2}')", logQuery, codePage, culture.DisplayName);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public LogEntryField[] QueryLogFilter(LogFilter logFilter)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryLogFilter");
			methodInformation.AddParameter("logFilter", logFilter);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.QueryLogFilter(logFilter);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryLogFilter('{0}')", logFilter.ConditionAsString);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public byte[] GetLicenseRequestFromConfiguration()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetLicenseRequestFromConfiguration");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetLicenseRequestFromConfiguration();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetLicenseRequestFromConfiguration()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetSystemInformationXml()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetSystemInformationXml");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetSystemInformationXml();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetSystemInformationXml()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetNavigationXml()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetNavigationXml");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetNavigationXml();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetNavigationXml()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string GetLicenseInformation()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetLicenseInformation");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetLicenseInformation();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetLicenseInformation()')");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public AviCodec64[] GetInstalledCodecs()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetInstalledCodecs");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.GetInstalledCodecs();
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetInstalledCodecs()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public string SendMail(MailGeneratorData mailGeneratorData)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("SendMail");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				return serverWebServiceClient.SendMail(mailGeneratorData);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: SendMail('{0}')", mailGeneratorData.MailServer);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public ConfigurationManagerDataTransferObject GetLicenseByLicenseId(Guid licenseId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetLicenseByLicenseId");
			methodInformation.AddParameter("licenseId", licenseId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetLicenseByLicenseId(base.RequestContext, serverWebServiceClient, licenseId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetLicenseByLicenseId('{0}')", licenseId.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetLicenseByLicenseId(IGeminiRequestContext requestContext, IServerWebService serverWebService, Guid licenseId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebService.GetLicenseByLicenseId(licenseId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(string digest)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareDefinitionByDigest");
			methodInformation.AddArrayParameter("digest", digest);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareDefinitionByDigest(base.RequestContext, serverWebServiceClient, digest), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareDefinitionByDigest('{0}')", digest);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareDefinitionByDigest(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, string digest)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebServiceClient.GetHardwareDefinitionByDigest(digest, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHardwareByRecorderId(Guid recorderId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareByRecorderId");
			methodInformation.AddParameter("recorderId", recorderId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareByRecorderId(base.RequestContext, serverWebServiceClient, recorderId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareByRecorderId('{0}')", recorderId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareByRecorderId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid recorderId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject hardwareByRecorderId = serverWebServiceClient.GetHardwareByRecorderId(recorderId, basicCallContextInfo);
		FilterHardware(requestContext, hardwareByRecorderId);
		return hardwareByRecorderId;
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareId(Guid hardwareId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareByHardwareId");
			methodInformation.AddParameter("hardwareId", hardwareId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareByHardwareId(base.RequestContext, serverWebServiceClient, hardwareId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareByHardwareId('{0}')", hardwareId.ToString());
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareByHardwareId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid hardwareId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject hardwareByHardwareId = serverWebServiceClient.GetHardwareByHardwareId(hardwareId, basicCallContextInfo);
		FilterHardware(requestContext, hardwareByHardwareId);
		return hardwareByHardwareId;
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(Guid[] hardwareIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareByHardwareIds");
			methodInformation.AddArrayParameter("hardwareIds", hardwareIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareByHardwareIds(base.RequestContext, serverWebServiceClient, hardwareIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareByHardwareIds('[{0}]')", string.Join(",", hardwareIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareByHardwareIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid[] hardwareIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject hardwareByHardwareIds = serverWebServiceClient.GetHardwareByHardwareIds(hardwareIds, basicCallContextInfo);
		FilterHardware(requestContext, hardwareByHardwareIds);
		return hardwareByHardwareIds;
	}

	public ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDevicePrivacyMaskByDeviceIds");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDevicePrivacyMaskByDeviceIds(base.RequestContext, serverWebServiceClient, deviceIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDevicePrivacyMaskByDeviceIds('[{0}]')", string.Join(",", deviceIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDevicePrivacyMaskByDeviceIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject devicePrivacyMaskByDeviceIds = serverWebServiceClient.GetDevicePrivacyMaskByDeviceIds(deviceIds, basicCallContextInfo);
		FilterPrivacyMask(requestContext, devicePrivacyMaskByDeviceIds);
		return devicePrivacyMaskByDeviceIds;
	}

	public ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceTracksByDeviceIds");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceTracksByDeviceIds(base.RequestContext, serverWebServiceClient, deviceIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceTracksByDeviceIds('[{0}]')", string.Join(",", deviceIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceTracksByDeviceIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceTracksByDeviceIds = serverWebServiceClient.GetDeviceTracksByDeviceIds(deviceIds, basicCallContextInfo);
		FilterPrivacyMask(requestContext, deviceTracksByDeviceIds);
		return deviceTracksByDeviceIds;
	}

	public ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(int hostedHardwareId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareByHostedHardwareId");
			methodInformation.AddParameter("hostedHardwareId", hostedHardwareId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareByHostedHardwareId(base.RequestContext, serverWebServiceClient, hostedHardwareId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareByHostedHardwareId('{0}')", hostedHardwareId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareByHostedHardwareId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, int hostedHardwareId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject hardwareByHostedHardwareId = serverWebServiceClient.GetHardwareByHostedHardwareId(hostedHardwareId, basicCallContextInfo);
		FilterHardware(requestContext, hardwareByHostedHardwareId);
		return hardwareByHostedHardwareId;
	}

	public ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(Guid deviceGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHardwareByDeviceGroupId");
			methodInformation.AddParameter("deviceGroupId", deviceGroupId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHardwareByDeviceGroupId(base.RequestContext, serverWebServiceClient, deviceGroupId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHardwareByDeviceGroupId('{0}')", deviceGroupId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHardwareByDeviceGroupId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceGroupId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject hardwareByDeviceGroupId = serverWebServiceClient.GetHardwareByDeviceGroupId(deviceGroupId, basicCallContextInfo);
		FilterHardware(requestContext, hardwareByDeviceGroupId);
		return hardwareByDeviceGroupId;
	}

	public ConfigurationManagerDataTransferObject GetDevicesByHardwareId(Guid hardwareId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDevicesByHardwareId");
			methodInformation.AddParameter("hardwareId", hardwareId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDevicesByHardwareId(base.RequestContext, serverWebServiceClient, hardwareId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDevicesByHardwareId('{0}')", hardwareId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDevicesByHardwareId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid hardwareId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject devicesByHardwareId = serverWebServiceClient.GetDevicesByHardwareId(hardwareId, basicCallContextInfo);
		FilterDevices(requestContext, devicesByHardwareId);
		return devicesByHardwareId;
	}

	public ConfigurationManagerDataTransferObject GetDevicesByRecorderId(Guid recorderId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDevicesByRecorderId");
			methodInformation.AddParameter("recorderId", recorderId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDevicesByRecorderId(base.RequestContext, serverWebServiceClient, recorderId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDevicesByRecorderId('{0}')", recorderId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDevicesByRecorderId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid recorderId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject devicesByRecorderId = serverWebServiceClient.GetDevicesByRecorderId(recorderId, basicCallContextInfo);
		FilterDevices(requestContext, devicesByRecorderId);
		return devicesByRecorderId;
	}

	public ConfigurationManagerDataTransferObject GetDeviceByDeviceId(Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceByDeviceId");
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceByDeviceId(base.RequestContext, serverWebServiceClient, deviceId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceByDeviceId('{0}')", deviceId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceByDeviceId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceByDeviceId = serverWebServiceClient.GetDeviceByDeviceId(deviceId, basicCallContextInfo);
		FilterDevices(requestContext, deviceByDeviceId);
		return deviceByDeviceId;
	}

	public ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDevicesByDeviceIds");
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDevicesByDeviceIds(base.RequestContext, serverWebServiceClient, deviceIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDevicesByDeviceIds('[{0}]')", string.Join(",", deviceIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDevicesByDeviceIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject devicesByDeviceIds = serverWebServiceClient.GetDevicesByDeviceIds(deviceIds, basicCallContextInfo);
		FilterDevices(requestContext, devicesByDeviceIds);
		return devicesByDeviceIds;
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(Guid deviceGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceGroupByDeviceGroupId");
			methodInformation.AddParameter("deviceGroupId", deviceGroupId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceGroupByDeviceGroupId(base.RequestContext, serverWebServiceClient, deviceGroupId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceGroupByDeviceGroupId('{0}')", deviceGroupId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceGroupByDeviceGroupId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceGroupId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebServiceClient.GetDeviceGroupByDeviceGroupId(deviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(Guid parentDeviceGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceGroupsByParentDeviceGroupId");
			methodInformation.AddParameter("parentDeviceGroupId", parentDeviceGroupId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceGroupsByParentDeviceGroupId(base.RequestContext, serverWebServiceClient, parentDeviceGroupId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceGroupsByParentDeviceGroupId('{0}')", parentDeviceGroupId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceGroupsByParentDeviceGroupId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid parentDeviceGroupId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebServiceClient.GetDeviceGroupsByParentDeviceGroupId(parentDeviceGroupId, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(Guid deviceGroupId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceGroupMembersByDeviceGroupId");
			methodInformation.AddParameter("deviceGroupId", deviceGroupId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceGroupMembersByDeviceGroupId(base.RequestContext, serverWebServiceClient, deviceGroupId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceGroupMembersByDeviceGroupId('{0}')", deviceGroupId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceGroupId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceGroupId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceGroupMembersByDeviceGroupId = serverWebServiceClient.GetDeviceGroupMembersByDeviceGroupId(deviceGroupId, basicCallContextInfo);
		FilterDeviceGroupMembers(requestContext, deviceGroupMembersByDeviceGroupId);
		return deviceGroupMembersByDeviceGroupId;
	}

	public ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceGroupMembersByDeviceId");
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceGroupMembersByDeviceId(base.RequestContext, serverWebServiceClient, deviceId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceGroupMembersByDeviceId('{0}')", deviceId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceGroupMembersByDeviceId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceGroupMembersByDeviceId = serverWebServiceClient.GetDeviceGroupMembersByDeviceId(deviceId, basicCallContextInfo);
		FilterDeviceGroupMembers(requestContext, deviceGroupMembersByDeviceId);
		return deviceGroupMembersByDeviceId;
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(Guid recorderPluginID, Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceRecorderPlugin");
			methodInformation.AddParameter("recorderPluginID", recorderPluginID);
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceRecorderPlugin(base.RequestContext, serverWebServiceClient, recorderPluginID, deviceId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceRecorderPlugin('{0}', '{1}')", recorderPluginID, deviceId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceRecorderPlugin(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid recorderPluginID, Guid deviceId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceRecorderPlugin = serverWebServiceClient.GetDeviceRecorderPlugin(recorderPluginID, deviceId, basicCallContextInfo);
		FilterDevicePlugins(requestContext, deviceRecorderPlugin);
		return deviceRecorderPlugin;
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(Guid recorderPluginID, Guid[] deviceIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceRecorderPlugins");
			methodInformation.AddParameter("recorderPluginID", recorderPluginID);
			methodInformation.AddArrayParameter("deviceIds", deviceIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceRecorderPlugins(base.RequestContext, serverWebServiceClient, recorderPluginID, deviceIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceRecorderPlugins('{0}','[{1}]')", recorderPluginID, string.Join(",", deviceIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceRecorderPlugins(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid recorderPluginID, Guid[] deviceIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceRecorderPlugins = serverWebServiceClient.GetDeviceRecorderPlugins(recorderPluginID, deviceIds, basicCallContextInfo);
		FilterDevicePlugins(requestContext, deviceRecorderPlugins);
		return deviceRecorderPlugins;
	}

	public ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(Guid deviceId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetDeviceRecorderPluginsByDeviceId");
			methodInformation.AddParameter("deviceId", deviceId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetDeviceRecorderPluginsByDeviceId(base.RequestContext, serverWebServiceClient, deviceId), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetDeviceRecorderPluginsByDeviceId('{0}')", deviceId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetDeviceRecorderPluginsByDeviceId(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, Guid deviceId)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject deviceRecorderPluginsByDeviceId = serverWebServiceClient.GetDeviceRecorderPluginsByDeviceId(deviceId, basicCallContextInfo);
		FilterDevicePlugins(requestContext, deviceRecorderPluginsByDeviceId);
		return deviceRecorderPluginsByDeviceId;
	}

	public ConfigurationManagerDataTransferObject GetHostedHardwareByIds(int[] hostedHardwareIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHostedHardwareByIds");
			methodInformation.AddArrayParameter("hostedHardwareIds", hostedHardwareIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHostedHardwareByIds(base.RequestContext, serverWebServiceClient, hostedHardwareIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHostedHardwareByIds('[{0}]')", string.Join(",", hostedHardwareIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHostedHardwareByIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, int[] hostedHardwareIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebServiceClient.GetHostedHardwareByIds(hostedHardwareIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetHostingServersByIds(int[] hostingServerIds)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetHostingServersByIds");
			methodInformation.AddArrayParameter("hostingServerIds", hostingServerIds);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetHostingServersByIds(base.RequestContext, serverWebServiceClient, hostingServerIds), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetHostingServersByIds('{0}')", string.Join(",", hostingServerIds));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetHostingServersByIds(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, int[] hostingServerIds)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		return serverWebServiceClient.GetHostingServersByIds(hostingServerIds, basicCallContextInfo);
	}

	public ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(string[] tableNames)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("GetConfigurationDataSetTables");
			methodInformation.AddArrayParameter("tableNames", tableNames);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling((ServerWebServiceClient serverWebServiceClient) => GetConfigurationDataSetTables(base.RequestContext, serverWebServiceClient, tableNames), delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: GetConfigurationDataSetTables('[{0}]')", string.Join(",", tableNames));
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public static ConfigurationManagerDataTransferObject GetConfigurationDataSetTables(IGeminiRequestContext requestContext, IServerWebService serverWebServiceClient, string[] tableNames)
	{
		requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
		BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
		ConfigurationManagerDataTransferObject configurationDataSetTables = serverWebServiceClient.GetConfigurationDataSetTables(tableNames, basicCallContextInfo);
		FilterTables(requestContext, configurationDataSetTables);
		return configurationDataSetTables;
	}

	private static void FilterTables(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		FilterSystemSettings(requestContext, dto);
		FilterRecorders(requestContext, dto);
		FilterHardware(requestContext, dto);
		FilterDevices(requestContext, dto);
		FilterRecordingStorageDevice(requestContext, dto);
		FilterDeviceGroupMembers(requestContext, dto);
		FilterDevicePlugins(requestContext, dto);
		FilterPatrollingProfiles(requestContext, dto);
		FilterDeviceLinks(requestContext, dto);
		FilterUserDefinedEvents(requestContext, dto);
		FilterMatrix(requestContext, dto);
		FilterRules(requestContext, dto);
	}

	private static void FilterRecorders(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.RecordersRow recorder in dto.DataSet.Recorders)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.RecordingServer, "GENERIC_READ", recorder.IDRecorder))
			{
				recorder.Delete();
			}
		}
		dto.DataSet.Recorders.AcceptChanges();
	}

	private static void FilterHardware(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		bool flag = securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "VIEW_HARDWARE_PASSWORDS", "/");
		foreach (ConfigurationDataSet.HardwareRow item in dto.DataSet.Hardware)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Hardware, "GENERIC_READ", item.IDHardware))
			{
				item.Delete();
			}
			if (!flag)
			{
				item.EncryptedPassword = requestContext.GetEncryptionService().Encrypt(item.EncryptedPassword);
			}
		}
		dto.DataSet.Hardware.AcceptChanges();
	}

	private static void FilterSystemSettings(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		foreach (ConfigurationDataSet.SystemSettingsRow systemSetting in dto.DataSet.SystemSettings)
		{
			if (systemSetting.Name == "SmtpPassword")
			{
				systemSetting.Setting = requestContext.GetEncryptionService().Encrypt(systemSetting.Setting);
			}
			if (systemSetting.Name == "ActivationCredentials" && !string.IsNullOrEmpty(systemSetting.Setting))
			{
				XmlDocument xmlDocument = new XmlDocument();
				xmlDocument.LoadXml(systemSetting.Setting);
				XmlNode lastChild = xmlDocument.LastChild.LastChild;
				lastChild.InnerText = requestContext.GetEncryptionService().Encrypt(lastChild.InnerText);
				systemSetting.Setting = lastChild.InnerText;
			}
		}
		dto.DataSet.SystemSettings.AcceptChanges();
	}

	private static void FilterPrivacyMask(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.DevicePrivacyMaskRow item in dto.DataSet.DevicePrivacyMask)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", item.IDDevice))
			{
				item.Delete();
			}
		}
		dto.DataSet.DevicePrivacyMask.AcceptChanges();
	}

	private static void FilterDevices(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.DevicesRow device in dto.DataSet.Devices)
		{
			bool flag = false;
			if (device.DeviceType switch
			{
				"Camera" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				"AudioInput" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				"AudioOutput" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				"Metadata" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				"Input" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Input, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				"Output" => securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Output, "GENERIC_READ", device.IDDevice) ? 1 : 0, 
				_ => throw new NotSupportedException(), 
			} == 0)
			{
				device.Delete();
			}
		}
		dto.DataSet.Devices.AcceptChanges();
	}

	private static void FilterRecordingStorageDevice(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.RecordingStorageDeviceRow item in dto.DataSet.RecordingStorageDevice)
		{
			if (!HasDeviceReadPermission(requestContext, securityManagerService, item.device_id))
			{
				item.Delete();
			}
			dto.DataSet.RecordingStorageDevice.AcceptChanges();
		}
	}

	private static void FilterDeviceGroupMembers(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.DeviceGroupMembersRow deviceGroupMember in dto.DataSet.DeviceGroupMembers)
		{
			if (!HasDeviceReadPermission(requestContext, securityManagerService, deviceGroupMember.IDDevice))
			{
				deviceGroupMember.Delete();
			}
		}
		dto.DataSet.DeviceGroupMembers.AcceptChanges();
	}

	private static void FilterDevicePlugins(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.RecorderPluginDeviceRow item in dto.DataSet.RecorderPluginDevice)
		{
			if (!HasDeviceReadPermission(requestContext, securityManagerService, item.IDDevice))
			{
				item.Delete();
			}
		}
		dto.DataSet.RecorderPluginDevice.AcceptChanges();
	}

	private static void FilterPatrollingProfiles(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.PatrollingProfilesRow patrollingProfile in dto.DataSet.PatrollingProfiles)
		{
			if (!HasDeviceReadPermission(requestContext, securityManagerService, patrollingProfile.IDDevice))
			{
				patrollingProfile.Delete();
			}
		}
		dto.DataSet.PatrollingProfiles.AcceptChanges();
	}

	private static void FilterDeviceLinks(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.DeviceLinksRow deviceLink in dto.DataSet.DeviceLinks)
		{
			if (!HasDeviceReadPermission(requestContext, securityManagerService, deviceLink.IDDevice) || !HasDeviceReadPermission(requestContext, securityManagerService, deviceLink.IDDeviceLinked))
			{
				deviceLink.Delete();
			}
		}
		dto.DataSet.DeviceLinks.AcceptChanges();
	}

	private static void FilterUserDefinedEvents(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.EventTypesRow eventType in dto.DataSet.EventTypes)
		{
			if (string.Equals(eventType.GeneratorType, EventGeneratorTypeName.External, StringComparison.OrdinalIgnoreCase) && string.Equals(eventType.GeneratorSubType, EventGeneratorSubtypeName.UserDefined, StringComparison.OrdinalIgnoreCase) && !securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.UserDefinedEvent, "GENERIC_READ", eventType.IDEventType))
			{
				eventType.Delete();
			}
		}
		dto.DataSet.EventTypes.AcceptChanges();
	}

	private static void FilterMatrix(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.MatrixRow item in dto.DataSet.Matrix)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Matrix, "GENERIC_READ", item.IDMatrix))
			{
				item.Delete();
			}
		}
		dto.DataSet.Matrix.AcceptChanges();
	}

	private static void FilterRules(IGeminiRequestContext requestContext, ConfigurationManagerDataTransferObject dto)
	{
		ISecurityManagerService securityManagerService = requestContext.GetSecurityManagerService();
		foreach (ConfigurationDataSet.RulesRow rule in dto.DataSet.Rules)
		{
			if (!securityManagerService.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Rule, "GENERIC_READ", rule.IDRule))
			{
				rule.Delete();
			}
		}
		dto.DataSet.Rules.AcceptChanges();
	}

	private static bool HasDeviceReadPermission(IGeminiRequestContext requestContext, ISecurityManagerService securityManager, string deviceId)
	{
		DeviceType deviceType = DeviceType.Unknown;
		try
		{
			deviceType = DeviceTypeLookup.Instance.QueryDeviceTypeFromDeviceId(requestContext, deviceId);
		}
		catch (DatabaseConnectionException)
		{
			throw;
		}
		bool flag = false;
		return deviceType switch
		{
			DeviceType.Camera => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Camera, "GENERIC_READ", deviceId), 
			DeviceType.Microphone => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Microphone, "GENERIC_READ", deviceId), 
			DeviceType.Speaker => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Speaker, "GENERIC_READ", deviceId), 
			DeviceType.Metadata => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Metadata, "GENERIC_READ", deviceId), 
			DeviceType.Input => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Input, "GENERIC_READ", deviceId), 
			DeviceType.Output => securityManager.HasObjectPermission(requestContext, SecurityNamespaceIdentifier.Output, "GENERIC_READ", deviceId), 
			DeviceType.Unknown => false, 
			_ => throw new NotSupportedException(), 
		};
	}

	public Guid[] QueryRolesForDefaultTimeProfile(Guid timeProfileId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRolesForDefaultTimeProfile");
			methodInformation.AddParameter("timeProfileId", timeProfileId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				return serverWebServiceClient.QueryRolesForDefaultTimeProfile(timeProfileId, basicCallContextInfo);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryRolesForDefaultTimeProfile('{0}')", timeProfileId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public Guid[] QueryRolesForPermissionsWithTimeProfile(Guid timeProfileId)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryRolesForPermissionsWithTimeProfile");
			methodInformation.AddParameter("timeProfileId", timeProfileId);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				return serverWebServiceClient.QueryRolesForPermissionsWithTimeProfile(timeProfileId, basicCallContextInfo);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryRolesForPermissionsWithTimeProfile('{0}')", timeProfileId);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public GetFileResult ReadFile(string fileName, long index, long count)
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("ReadFile");
			methodInformation.AddParameter("fileName", fileName);
			methodInformation.AddParameter("index", index);
			methodInformation.AddParameter("count", count);
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				return serverWebServiceClient.ReadFile(fileName, index, count, basicCallContextInfo);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: ReadFile('{0}', '{1}', '{2}')", fileName, index, count);
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	public int[] QueryHostedHardwareIdsNotAdded()
	{
		CheckConnect();
		try
		{
			MethodInformation methodInformation = new MethodInformation("QueryHostedHardwareIdsNotAdded");
			EnterWebMethod(methodInformation);
			return CallWithRetryExceptionHandling(delegate(ServerWebServiceClient serverWebServiceClient)
			{
				IGeminiRequestContext requestContext = base.RequestContext;
				requestContext.GetSecurityManagerService().CheckApplicationPermission(requestContext, "GENERIC_READ");
				BasicCallContextInfo basicCallContextInfo = new BasicCallContextInfo(requestContext.AuthenticatedUserSid, requestContext.UserDomainAccountName, requestContext.RemoteIPAddress);
				return serverWebServiceClient.QueryHostedHardwareIdsNotAdded(basicCallContextInfo);
			}, delegate
			{
				GeminiTrace.Error(TraceType.Configuration, "Recording Server communication failed: QueryHostedHardwareIdsNotAdded()");
			});
		}
		finally
		{
			LeaveWebMethod();
		}
	}

	private TResult CallWithRetryExceptionHandling<TResult>(Func<ServerWebServiceClient, TResult> call, Action exceptionTraceCall)
	{
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = base.RequestContext.GetService<ServerClientService>().GetClient();
			return serverWebServiceManager.CallWithRetry(call, exceptionTraceCall);
		}
		finally
		{
			base.RequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
	}

	private void CallWithRetryExceptionHandling(Action<ServerWebServiceClient> call, Action exceptionTraceCall)
	{
		ServerWebServiceManager serverWebServiceManager = null;
		try
		{
			serverWebServiceManager = base.RequestContext.GetService<ServerClientService>().GetClient();
			serverWebServiceManager.CallWithRetry(call, exceptionTraceCall);
		}
		finally
		{
			base.RequestContext.GetService<ServerClientService>().ReleaseClient(serverWebServiceManager);
		}
	}

	private static string GetAuthorizationIdFromHeader()
	{
		MessageHeaders incomingMessageHeaders = OperationContext.Current.IncomingMessageHeaders;
		int num = incomingMessageHeaders.FindHeader("AuthorizationId", "http://schemas.videoos/ServerProxyService/2008/06/Vmo/01");
		if (num != -1)
		{
			return incomingMessageHeaders.GetHeader<string>(num);
		}
		return string.Empty;
	}

	private BasicCallContextInfo GetCallContextInfo()
	{
		MessageHeaders incomingMessageHeaders = OperationContext.Current.IncomingMessageHeaders;
		int num = incomingMessageHeaders.FindHeader("HostAddress", "http://videoos.net/2/XProtectCSServerCommand");
		return new BasicCallContextInfo(locationIPAddress: (num != -1) ? incomingMessageHeaders.GetHeader<string>(num) : base.RequestContext.RemoteIPAddress, userSid: base.RequestContext.AuthenticatedUserSid, userDomainAccountName: base.RequestContext.UserDomainAccountName);
	}
}
