using System;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Management.Server.Properties;

namespace VideoOS.Management.Server.ConfigurationApi;

public static class MethodRepository
{
	public const string MethodIdRemoveDeviceGroupMember = "RemoveDeviceGroupMember";

	public const string MethodIdAddDeviceGroup = "AddDeviceGroup";

	public const string MethodIdRemoveDeviceGroup = "RemoveDeviceGroup";

	public const string MethodIdAddHardware = "AddHardware";

	public const string MethodIdGetOtherDevicesWithMediaOnRecorder = "GetOtherDevicesWithMediaOnRecorder";

	public const string MethodIdGetOtherRecordingServersWithMedia = "GetOtherRecordingServersWithMedia";

	public const string MethodIdGetOtherRecordingStoragesWithMedia = "GetOtherRecordingStoragesWithMedia";

	public const string MethodIdReadDrivesOnRecorder = "ReadDrivesOnRecorder";

	public const string MethodIdReadDirectoriesOnRecorder = "ReadDirectoriesOnRecorder";

	public const string MethodIdCreateDirectoryOnRecorder = "CreateDirectoryOnRecorder";

	public const string MethodIdRetrieveEdgeStorage = "RetrieveEdgeStorage";

	public const string MethodIdGetDevicesUsingStorage = "GetDevicesUsingStorage";

	public const string MethodIdTaskCleanup = "TaskCleanup";

	public const string MethodIdTaskStop = "TaskStop";

	public const string MethodIdAddBasicUser = "AddBasicUser";

	public const string MethodIdRemoveBasicUser = "RemoveBasicUser";

	public const string MethodIdChangePasswordBasicUser = "ChangePasswordBasicUser";

	public const string MethodIdAddPtzPreset = "AddPtzPreset";

	public const string MethodIdRemovePtzPreset = "RemovePtzPreset";

	public const string MethodIdDefaultPtzPreset = "DefaultPtzPreset";

	public const string MethodIdAddStream = "AddStream";

	public const string MethodIdRemoveStream = "RemoveStream";

	public const string MethodIdAddRole = "AddRole";

	public const string MethodIdRemoveRole = "RemoveRole";

	public const string MethodIdCopyRole = "CopyRole";

	public const string MethodIdAddRoleMember = "AddRoleMember";

	public const string MethodIdRemoveRoleMember = "RemoveRoleMember";

	public const string MethodIdAddRoleClaim = "AddRoleClaim";

	public const string MethodIdRemoveRoleClaim = "RemoveRoleClaim";

	public const string MethodIdAddLoginProvider = "AddLoginProvider";

	public const string MethodIdRemoveLoginProvider = "RemoveLoginProvider";

	public const string MethodIdAddRegisteredClaim = "AddRegisteredClaim";

	public const string MethodIdRemoveRegisteredClaim = "RemoveRegisteredClaim";

	public const string MethodIdReadPasswordHardware = "ReadPasswordHardware";

	public const string MethodIdChangePasswordHardware = "ChangePasswordHardware";

	public const string MethodIdUpdateFirmwareHardware = "UpdateFirmwareHardware";

	public const string MethodIdMoveHardware = "MoveHardware";

	public const string MethodIdUpdateHardware = "UpdateHardware";

	public const string MethodIdReplaceHardware = "ReplaceHardware";

	public const string MethodIdAddStorage = "AddStorage";

	public const string MethodIdRemoveStorage = "RemoveStorage";

	public const string MethodIdReadStorageInformation = "ReadStorageInformation";

	public const string MethodIdAddArchiveStorage = "AddArchiveStorage";

	public const string MethodIdRemoveArchiveStorage = "RemoveArchiveStorage";

	public const string MethodIdReadArchiveStorageInformation = "ReadArchiveStorageInformation";

	public const string MethodIdSetStorageAsDefault = "SetStorageAsDefault";

	public const string MethodIdSetFramerateReductionArchiveStorage = "SetFramerateReductionArchiveStorage";

	public const string MethodIdMountArchiveStorage = "MountArchiveStorage";

	public const string MethodIdUnmountArchiveStorage = "UnmountArchiveStorage";

	public const string MethodIdDisableEncryption = "DisableEncryption";

	public const string MethodIdChangeEncryptionMethod = "ChangeEncryptionMethod";

	public const string MethodIdChangePasswordStorageEncryption = "ChangePasswordStorageEncryption";

	public const string MethodIdChangeSecurityPermissions = "ChangeSecurityPermissions";

	public const string MethodIdMoveArchiveStorage = "MoveArchiveStorage";

	public const string MethodIdEnableEncryption = "EnableEncryption";

	public const string MethodIdTruncateStorage = "TruncateStorage";

	public const string MethodIdTruncateStorageOnOtherRecordingServer = "TruncateStorageOnOtherRecordingServer";

	public const string MethodIdMoveRecordingStorage = "MoveRecordingStorage";

	public const string MethodIdChangeDeviceRecordingStorage = "ChangeDeviceRecordingStorage";

	public const string MethodIdRemoveRecordingServer = "RemoveRecordingServer";

	public const string MethodIdValidateStorageEncryptionPassword = "ValidateStorageEncryptionPassword";

	public const string MethodIdHardwareScan = "HardwareScan";

	public const string MethodIdDeleteHardware = "DeleteHardware";

	public const string MethodIdSetClientTimeProfile = "SetClientTimeProfile";

	public const string MethodIdGetClientTimeProfile = "GetClientTimeProfile";

	public const string MethodIdLoadTasks = "LoadTasks";

	public const string MethodIdClientLogOnSupported = "ClientLogOnSupported";

	public const string MethodIdHardwareScanExpress = "HardwareScanExpress";

	public const string MethodIdChangeOverallSecurityPermissions = "ChangeOverallSecurityPermissions";

	public const string MethodIdAddVideoWall = "AddVideoWall";

	public const string MethodIdRemoveVideoWall = "RemoveVideoWall";

	public const string MethodIdAddMonitor = "AddMonitor";

	public const string MethodIdRemoveMonitor = "RemoveMonitor";

	public const string MethodIdApplyCameras = "ApplyCameras";

	public const string MethodIdApplyCamerasAndLayout = "ApplyCamerasAndLayout";

	public const string MethodIdRemoveCameras = "RemoveCameras";

	public const string MethodIdApplyViewItemsXml = "ApplyViewItemsXml";

	public const string MethodIdApplyMonitorState = "ApplyMonitorState";

	public const string MethodIdApplyLayout = "ApplyLayout";

	public const string MethodIdApplyTextAndLayout = "ApplyTextAndLayout";

	public const string MethodIdAddVideoWallPreset = "AddVideoWallPreset";

	public const string MethodIdRemoveVideoWallPreset = "RemoveVideoWallPreset";

	public const string MethodIdActivateVideoWallPreset = "Activate";

	public const string MethodIdAddAudioMessage = "AddAudioMessage";

	public const string MethodIdRemoveAudioMessage = "RemoveAudioMessage";

	public const string MethodIdGetAudioMessageContent = "GetAudioMessageContent";

	public const string MethodIdSetAudioMessageContent = "SetAudioMessageContent";

	public const string MethodIdAddUserDefinedEvent = "AddUserDefinedEvent";

	public const string MethodIdRemoveUserDefinedEvent = "RemoveUserDefinedEvent";

	public const string MethodIdAddAnalyticsEvent = "AddAnalyticsEvent";

	public const string MethodIdRemoveAnalyticsEvent = "RemoveAnalyticsEvent";

	public const string MethodIdAddGenericEvent = "AddGenericEvent";

	public const string MethodIdRemoveGenericEvent = "RemoveGenericEvent";

	public const string MethodIdAddGenericEventDataSource = "AddGenericEventDataSource";

	public const string MethodIdRemoveGenericEventDataSource = "RemoveGenericEventDataSource";

	public const string MethodIdAddGisMapLocation = "AddGisMapLocation";

	public const string MethodIdRemoveGisMapLocation = "RemoveGisMapLocation";

	public const string MethodIdAddTimeProfile = "AddTimeProfile";

	public const string MethodIdRemoveTimeProfile = "RemoveTimeProfile";

	public const string MethodIdAddAppointment = "AddAppointment";

	public const string MethodIdRemoveAppointment = "RemoveAppointment";

	public const string MethodIdAddRecurringAppointment = "AddRecurringAppointment";

	public const string MethodIdRemoveRecurringAppointment = "RemoveRecurringAppointment";

	public const string MethodIdAddDeviceGroupMember = "AddDeviceGroupMember";

	public const string MethodIdAddRecurringVariance = "AddRecurringVariance";

	[Obsolete]
	public const string MethodIdRemoveRecurringVariance = "RemoveRecurringVariance";

	public const string MethodIdAddLayout = "AddLayout";

	public const string MethodIdRemoveLayout = "RemoveLayout";

	public const string MethodIdAddMIPKind = "AddMIPKind";

	public const string MethodIdRemoveMIPKind = "RemoveMIPKind";

	public const string MethodIdAddMIPItem = "AddMIPItem";

	public const string MethodIdRemoveMIPItem = "RemoveMIPItem";

	public const string MethodIdGetMIPOptionProperty = "GetMIPOptionProperty";

	public const string MethodIdSetMIPOptionProperty = "SetMIPOptionProperty";

	public const string MethodIdMIPItemDataVersionSum = "MIPItemDataVersionSum";

	public const string MethodIdAddAlarmDefinition = "AddAlarmDefinition";

	public const string MethodIdRemoveAlarmDefinition = "RemoveAlarmDefinition";

	public const string MethodIdGetCustomFieldsForRegistrationNumber = "MethodIdGetCustomFieldsForRegistrationNumber";

	public const string MethodIdGetRegistrationNumbersInfo = "MethodIdGetRegistrationNumbersInfo";

	public const string MethodIdAddOrEditRegistrationNumbersInfo = "MethodIdAddOrEditRegistrationNumbersInfo";

	public const string MethodIdDeleteRegistrationNumbers = "MethodIdDeleteRegistrationNumbers";

	public const string MethodIdDeleteAllRegistrationNumbers = "MethodIdDeleteAllRegistrationNumbers";

	public const string MethodIdAddLprMatchList = "MethodIdAddLprMatchList";

	public const string MethodIdRemoveLprMatchList = "MethodIdRemoveLprMatchList";

	public const string MethodIdAddSaveSearch = "AddSaveSearch";

	public const string MethodIdUpdateSaveSearch = "UpdateSaveSearch";

	public const string MethodIdRemoveSaveSearch = "RemoveSaveSearch";

	public const string MethodIdGetSaveSearch = "GetSaveSearch";

	public const string MethodIdFindSaveSearches = "FindSaveSearches";

	public const string MethodIdAddRule = "AddRule";

	public const string MethodIdRemoveRule = "RemoveRule";

	public const string MethodIdGetInternalUnitPath = "GetInternalUnitPath";

	public const string MethodIdAddBasicOwnerInfo = "AddBasicOwnerInfo";

	public const string MethodIdRemoveBasicOwnerInfo = "RemoveBasicOwnerInfo";

	public const string MethodIdActivateLicense = "ActivateLicense";

	public const string MethodIdRequestLicense = "RequestLicense";

	public const string MethodIdChangeLicense = "ChangeLicense";

	public const string MethodIdUpdateLicense = "UpdateLicense";

	public const string MethodIdStopAutomaticLicenseActivation = "StopAutomaticLicenseActivation";

	public const string MethodIdUploadFileChunk = "UploadFileChunk";

	public const string MethodIdAddViewGroup = "AddViewGroup";

	public const string MethodIdRemoveViewGroup = "RemoveViewGroup";

	public const string MethodIdAddView = "AddView";

	public const string MethodIdRemoveView = "RemoveView";

	public const string MethodIdGetDevicePresets = "GetDevicePresets";

	public const string MethodIdAddPatrollingEntry = "AddPatrollingEntry";

	public const string MethodIdRemovePatrollingEntry = "RemovePatrollingEntry";

	public const string MethodIdAddPatrollingProfile = "AddPatrollingProfile";

	public const string MethodIdRemovePatrollingProfile = "RemovePatrollingProfile";

	public const string MethodIdAddFailoverGroup = "AddFailoverGroup";

	public const string MethodIdRemoveFailoverGroup = "RemoveFailoverGroup";

	public const string MethodIdMoveFailoverGroup = "MoveFailoverGroup";

	public const string MethodIdAddClientProfile = "AddClientProfile";

	public const string MethodIdRemoveClientProfile = "RemoveClientProfile";

	public const string MethodIdClientProfileUpPriority = "ClientProfileUpPriority";

	public const string MethodIdClientProfileDownPriority = "ClientProfileDownPriority";

	public const string MethodIdSetClientProfile = "SetClientProfile";

	public const string MethodIdAddStateGroup = "AddStateGroup";

	public const string MethodIdRemoveStateGroup = "RemoveStateGroup";

	public const string MethodIdAddMatrix = "AddMatrix";

	public const string MethodIdRemoveMatrix = "RemoveMatrix";

	public const string MethodIdMatrixCommand = "MatrixCommand";

	public const string MethodIdAddEvidenceLockProfile = "AddEvidenceLockProfile";

	public const string MethodIdRemoveEvidenceLockProfile = "RemoveEvidenceLockProfile";

	public const string MethodIdQueryUsersWithAccess = "QueryUsersWithAccess";

	public const string MethodIdAddMailNotificationProfile = "AddMailNotificationProfile";

	public const string MethodIdRemoveMailNotificationProfile = "RemoveMailNotificationProfile";

	public const string MethodIdApplyViewToView = "ApplyViewToView";

	public const string MethodIdClearView = "ClearView";

	public const string MethodIdClearViewItem = "ClearViewItem";

	public const string MethodIdQueryIsPermittedTimeBasedAction = "QueryIsPermittedTimeBasedAction";

	public const string MethodIdAddAccessControlGroup = "AddAccessControlGroup";

	public const string MethodIdRemoveAccessControlGroup = "RemoveAccessControlGroup";

	public const string MethodIdAddAccessControlGroupMember = "AddAccessControlGroupMember";

	public const string MethodIdRemoveAccessControlGroupMember = "RemoveAccessControlGroupMember";

	private static MethodInfo[] _methodInfoArray = new MethodInfo[175]
	{
		new MethodInfo
		{
			MethodId = "AddDeviceGroupMember",
			DisplayName = ConfigurationApiResources.MethodAddDeviceMember,
			TranslationId = "MethodAddDeviceMember"
		},
		new MethodInfo
		{
			MethodId = "RemoveDeviceGroupMember",
			DisplayName = ConfigurationApiResources.MethodRemoveDeviceMember,
			TranslationId = "MethodRemoveDeviceMember"
		},
		new MethodInfo
		{
			MethodId = "AddDeviceGroup",
			DisplayName = ConfigurationApiResources.MethodAddGroup,
			TranslationId = "MethodAddGroup"
		},
		new MethodInfo
		{
			MethodId = "RemoveDeviceGroup",
			DisplayName = ConfigurationApiResources.MethodRemoveGroup,
			TranslationId = "MethodRemoveGroup"
		},
		new MethodInfo
		{
			MethodId = "AddHardware",
			DisplayName = ConfigurationApiResources.MethodIdAddHardware,
			TranslationId = "MethodIdAddHardware",
			ToolTipTranslationId = "MethodIdAddHardwareToolTip"
		},
		new MethodInfo
		{
			MethodId = "RemoveRecordingServer",
			DisplayName = ConfigurationApiResources.MethodRemoveRecordingServer,
			TranslationId = "MethodRemoveRecordingServer",
			ToolTipTranslationId = "MethodRemoveRecordingServerToolTip"
		},
		new MethodInfo
		{
			MethodId = "GetOtherDevicesWithMediaOnRecorder",
			DisplayName = ConfigurationApiResources.MethodGetOtherDevicesWithMediaOnRecorder,
			TranslationId = "MethodGetOtherDevicesWithMediaOnRecorder"
		},
		new MethodInfo
		{
			MethodId = "GetOtherRecordingServersWithMedia",
			DisplayName = ConfigurationApiResources.MethodGetOtherRecordingServersWithMedia,
			TranslationId = "MethodGetOtherRecordingServersWithMedia"
		},
		new MethodInfo
		{
			MethodId = "GetOtherRecordingStoragesWithMedia",
			DisplayName = ConfigurationApiResources.MethodGetOtherRecordingStoragesWithMedia,
			TranslationId = "MethodGetOtherRecordingStoragesWithMedia"
		},
		new MethodInfo
		{
			MethodId = "ReadDrivesOnRecorder",
			DisplayName = ConfigurationApiResources.MethodReadDrivesOnRecorder,
			TranslationId = "MethodReadDrivesOnRecorder"
		},
		new MethodInfo
		{
			MethodId = "ReadDirectoriesOnRecorder",
			DisplayName = ConfigurationApiResources.MethodReadDirectoriesOnRecorder,
			TranslationId = "MethodReadDirectoriesOnRecorder"
		},
		new MethodInfo
		{
			MethodId = "CreateDirectoryOnRecorder",
			DisplayName = ConfigurationApiResources.MethodCreateDirectoryOnRecorder,
			TranslationId = "MethodCreateDirectoryOnRecorder"
		},
		new MethodInfo
		{
			MethodId = "GetDevicesUsingStorage",
			DisplayName = ConfigurationApiResources.MethodGetDevicesUsingStorage,
			TranslationId = "MethodGetDevicesUsingStorage"
		},
		new MethodInfo
		{
			MethodId = "TaskCleanup",
			DisplayName = ConfigurationApiResources.MethodIdTaskCleanup,
			TranslationId = "MethodIdTaskCleanup"
		},
		new MethodInfo
		{
			MethodId = "TaskStop",
			DisplayName = ConfigurationApiResources.MethodIdTaskStop,
			TranslationId = "MethodIdTaskStop"
		},
		new MethodInfo
		{
			MethodId = "AddBasicUser",
			DisplayName = ConfigurationApiResources.WizardAddBasicUser,
			TranslationId = "WizardAddBasicUser"
		},
		new MethodInfo
		{
			MethodId = "RemoveBasicUser",
			DisplayName = ConfigurationApiResources.WizardRemoveBasicUser,
			TranslationId = "WizardRemoveBasicUser"
		},
		new MethodInfo
		{
			MethodId = "ChangePasswordBasicUser",
			DisplayName = ConfigurationApiResources.MethodChangePasswordBasicUser,
			TranslationId = "MethodChangePasswordBasicUser"
		},
		new MethodInfo
		{
			MethodId = "AddPtzPreset",
			DisplayName = ConfigurationApiResources.WizardAddPtzPreset,
			TranslationId = "WizardAddPtzPreset"
		},
		new MethodInfo
		{
			MethodId = "RemovePtzPreset",
			DisplayName = ConfigurationApiResources.WizardRemovePtzPreset,
			TranslationId = "WizardRemovePtzPreset"
		},
		new MethodInfo
		{
			MethodId = "DefaultPtzPreset",
			DisplayName = ConfigurationApiResources.MethodSetDefault,
			TranslationId = "MethodSetDefault"
		},
		new MethodInfo
		{
			MethodId = "AddStream",
			DisplayName = ConfigurationApiResources.WizardAddSteam,
			TranslationId = "WizardAddSteam"
		},
		new MethodInfo
		{
			MethodId = "RemoveStream",
			DisplayName = ConfigurationApiResources.WizardRemoveStream,
			TranslationId = "WizardRemoveStream"
		},
		new MethodInfo
		{
			MethodId = "AddRole",
			DisplayName = ConfigurationApiResources.MethodAddRole,
			TranslationId = "MethodAddRole"
		},
		new MethodInfo
		{
			MethodId = "RemoveRole",
			DisplayName = ConfigurationApiResources.MethodRemoveRole,
			TranslationId = "MethodRemoveRole"
		},
		new MethodInfo
		{
			MethodId = "CopyRole",
			DisplayName = ConfigurationApiResources.MethodCopyRole,
			TranslationId = "MethodCopyRole"
		},
		new MethodInfo
		{
			MethodId = "AddRoleMember",
			DisplayName = ConfigurationApiResources.WizardAddRoleMember,
			TranslationId = "WizardAddRoleMember"
		},
		new MethodInfo
		{
			MethodId = "RemoveRoleMember",
			DisplayName = ConfigurationApiResources.WizardRemoveRoleMember,
			TranslationId = "WizardRemoveRoleMember"
		},
		new MethodInfo
		{
			MethodId = "AddRoleClaim",
			DisplayName = ConfigurationApiResources.MethodAddRoleClaim,
			TranslationId = "MethodAddRoleClaim"
		},
		new MethodInfo
		{
			MethodId = "RemoveRoleClaim",
			DisplayName = ConfigurationApiResources.MethodRemoveRoleClaim,
			TranslationId = "MethodRemoveRoleClaim"
		},
		new MethodInfo
		{
			MethodId = "AddLoginProvider",
			DisplayName = ConfigurationApiResources.MethodAddLoginProvider,
			TranslationId = "MethodAddLoginProvider"
		},
		new MethodInfo
		{
			MethodId = "RemoveLoginProvider",
			DisplayName = ConfigurationApiResources.MethodRemoveLoginProvider,
			TranslationId = "MethodRemoveLoginProvider"
		},
		new MethodInfo
		{
			MethodId = "AddRegisteredClaim",
			DisplayName = ConfigurationApiResources.MethodAddRegisteredClaim,
			TranslationId = "MethodAddRegisteredClaim"
		},
		new MethodInfo
		{
			MethodId = "RemoveRegisteredClaim",
			DisplayName = ConfigurationApiResources.MethodRemoveRegisteredClaim,
			TranslationId = "MethodRemoveRegisteredClaim"
		},
		new MethodInfo
		{
			MethodId = "ReadPasswordHardware",
			DisplayName = ConfigurationApiResources.MethodReadPasswordHardware,
			TranslationId = "MethodReadPasswordHardware"
		},
		new MethodInfo
		{
			MethodId = "ChangePasswordHardware",
			DisplayName = ConfigurationApiResources.MethodChangePasswordHardware,
			TranslationId = "MethodChangePasswordHardware"
		},
		new MethodInfo
		{
			MethodId = "UpdateFirmwareHardware",
			DisplayName = ConfigurationApiResources.MethodUpdateFirmwareHardware,
			TranslationId = "MethodUpdateFirmwareHardware"
		},
		new MethodInfo
		{
			MethodId = "AddStorage",
			DisplayName = ConfigurationApiResources.MethodAddStorage,
			TranslationId = "MethodAddStorage"
		},
		new MethodInfo
		{
			MethodId = "RemoveStorage",
			DisplayName = ConfigurationApiResources.MethodRemoveStorage,
			TranslationId = "MethodRemoveStorage"
		},
		new MethodInfo
		{
			MethodId = "ReadStorageInformation",
			DisplayName = ConfigurationApiResources.MethodReadStorageInformation,
			TranslationId = "MethodReadStorageInformation"
		},
		new MethodInfo
		{
			MethodId = "AddArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodAddArchiveStorage,
			TranslationId = "MethodAddArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "RemoveArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodRemoveArchiveStorage,
			TranslationId = "MethodRemoveArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "ReadArchiveStorageInformation",
			DisplayName = ConfigurationApiResources.MethodReadArchiveStorageInformation,
			TranslationId = "MethodReadArchiveStorageInformation"
		},
		new MethodInfo
		{
			MethodId = "SetStorageAsDefault",
			DisplayName = ConfigurationApiResources.MethodSetStorageAsDefault,
			TranslationId = "MethodSetStorageAsDefault"
		},
		new MethodInfo
		{
			MethodId = "SetFramerateReductionArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodSetFramerateReductionArchiveStorage,
			TranslationId = "MethodSetFramerateReductionArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "DisableEncryption",
			DisplayName = ConfigurationApiResources.MethodDisableEncryption,
			TranslationId = "MethodDisableEncryption"
		},
		new MethodInfo
		{
			MethodId = "ChangeSecurityPermissions",
			DisplayName = ConfigurationApiResources.MethodChangeSecurityPermissions,
			TranslationId = "MethodChangeSecurityPermissions"
		},
		new MethodInfo
		{
			MethodId = "MoveArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodMoveArchiveStorage,
			TranslationId = "MethodMoveArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "MountArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodMountArchiveStorage,
			TranslationId = "MethodMountArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "UnmountArchiveStorage",
			DisplayName = ConfigurationApiResources.MethodUnmountArchiveStorage,
			TranslationId = "MethodUnmountArchiveStorage"
		},
		new MethodInfo
		{
			MethodId = "EnableEncryption",
			DisplayName = ConfigurationApiResources.MethodEnableEncryption,
			TranslationId = "MethodEnableEncryption"
		},
		new MethodInfo
		{
			MethodId = "ChangeEncryptionMethod",
			DisplayName = ConfigurationApiResources.MethodChangeEncryptionMethod,
			TranslationId = "MethodChangeEncryptionMethod"
		},
		new MethodInfo
		{
			MethodId = "ChangePasswordStorageEncryption",
			DisplayName = ConfigurationApiResources.MethodChangeEncryptionPassword,
			TranslationId = "MethodChangeEncryptionPassword"
		},
		new MethodInfo
		{
			MethodId = "RetrieveEdgeStorage",
			DisplayName = ConfigurationApiResources.MethodRetrieveEdgeStorage,
			TranslationId = "MethodRetrieveEdgeStorage"
		},
		new MethodInfo
		{
			MethodId = "TruncateStorage",
			DisplayName = ConfigurationApiResources.MethodTruncateStorage,
			TranslationId = "MethodTruncateStorage"
		},
		new MethodInfo
		{
			MethodId = "TruncateStorageOnOtherRecordingServer",
			DisplayName = ConfigurationApiResources.MethodTruncateStorageOnOtherRecordingServer,
			TranslationId = "MethodTruncateStorageOnOtherRecordingServer"
		},
		new MethodInfo
		{
			MethodId = "MoveRecordingStorage",
			DisplayName = ConfigurationApiResources.MethodMoveRecordingStorage,
			TranslationId = "MethodMoveRecordingStorage"
		},
		new MethodInfo
		{
			MethodId = "ChangeDeviceRecordingStorage",
			DisplayName = ConfigurationApiResources.MethodChangeDeviceRecordingStorage,
			TranslationId = "MethodChangeDeviceRecordingStorage"
		},
		new MethodInfo
		{
			MethodId = "ValidateStorageEncryptionPassword",
			DisplayName = ConfigurationApiResources.MethodValidateStorageEncryptionPassword,
			TranslationId = "MethodValidateStorageEncryptionPassword"
		},
		new MethodInfo
		{
			MethodId = "HardwareScan",
			DisplayName = ConfigurationApiResources.MethodHardwareScan,
			TranslationId = "MethodHardwareScan"
		},
		new MethodInfo
		{
			MethodId = "DeleteHardware",
			DisplayName = ConfigurationApiResources.MethodDeleteHardware,
			TranslationId = "MethodDeleteHardware"
		},
		new MethodInfo
		{
			MethodId = "MoveHardware",
			DisplayName = ConfigurationApiResources.MethodMoveHardware,
			TranslationId = "MethodMoveHardware"
		},
		new MethodInfo
		{
			MethodId = "UpdateHardware",
			DisplayName = ConfigurationApiResources.MethodUpdateHardware,
			TranslationId = "MethodUpdateHardware"
		},
		new MethodInfo
		{
			MethodId = "ReplaceHardware",
			DisplayName = ConfigurationApiResources.MethodReplaceHardware,
			TranslationId = "MethodReplaceHardware"
		},
		new MethodInfo
		{
			MethodId = "SetClientTimeProfile",
			DisplayName = ConfigurationApiResources.MethodSetClientLoginTimeProfile,
			TranslationId = "MethodSetClientLoginTimeProfile"
		},
		new MethodInfo
		{
			MethodId = "GetClientTimeProfile",
			DisplayName = ConfigurationApiResources.MethodGetClientLoginTimeProfile,
			TranslationId = "MethodGetClientLoginTimeProfile"
		},
		new MethodInfo
		{
			MethodId = "LoadTasks",
			DisplayName = ConfigurationApiResources.MethodLoadTasks,
			TranslationId = "MethodLoadTasks"
		},
		new MethodInfo
		{
			MethodId = "ClientLogOnSupported",
			DisplayName = ConfigurationApiResources.MethodClientLogOnSupported,
			TranslationId = "MethodClientLogOnSupported"
		},
		new MethodInfo
		{
			MethodId = "ChangeOverallSecurityPermissions",
			DisplayName = ConfigurationApiResources.MethodChangeOverallSecurityPermissions,
			TranslationId = "MethodChangeOverallSecurityPermissions"
		},
		new MethodInfo
		{
			MethodId = "HardwareScanExpress",
			DisplayName = ConfigurationApiResources.MethodHardwareScanExpress,
			TranslationId = "MethodHardwareScanExpress"
		},
		new MethodInfo
		{
			MethodId = "AddVideoWall",
			DisplayName = ConfigurationApiResources.MethodAddVideoWall,
			TranslationId = "MethodAddVideoWall"
		},
		new MethodInfo
		{
			MethodId = "RemoveVideoWall",
			DisplayName = ConfigurationApiResources.MethodRemoveVideoWall,
			TranslationId = "MethodRemoveVideoWall"
		},
		new MethodInfo
		{
			MethodId = "AddMonitor",
			DisplayName = ConfigurationApiResources.MethodAddMonitor,
			TranslationId = "MethodAddMonitor"
		},
		new MethodInfo
		{
			MethodId = "RemoveMonitor",
			DisplayName = ConfigurationApiResources.MethodRemoveMonitor,
			TranslationId = "MethodRemoveMonitor"
		},
		new MethodInfo
		{
			MethodId = "ApplyCameras",
			DisplayName = ConfigurationApiResources.MethodApplyCameras,
			TranslationId = "MethodApplyCameras"
		},
		new MethodInfo
		{
			MethodId = "ApplyCamerasAndLayout",
			DisplayName = ConfigurationApiResources.MethodApplyCamerasAndLayout,
			TranslationId = "MethodApplyCamerasAndLayout"
		},
		new MethodInfo
		{
			MethodId = "RemoveCameras",
			DisplayName = ConfigurationApiResources.MethodRemoveCameras,
			TranslationId = "MethodRemoveCameras"
		},
		new MethodInfo
		{
			MethodId = "ApplyViewItemsXml",
			DisplayName = ConfigurationApiResources.MethodApplyViewItemsXml,
			TranslationId = "MethodApplyViewItemsXml"
		},
		new MethodInfo
		{
			MethodId = "ApplyMonitorState",
			DisplayName = ConfigurationApiResources.MethodApplyMonitorState,
			TranslationId = "MethodApplyMonitorState",
			ToolTipTranslationId = "MethodIdApplyMonitorStateToolTip"
		},
		new MethodInfo
		{
			MethodId = "ApplyLayout",
			DisplayName = ConfigurationApiResources.MethodApplyLayout,
			TranslationId = "MethodApplyLayout"
		},
		new MethodInfo
		{
			MethodId = "ApplyTextAndLayout",
			DisplayName = ConfigurationApiResources.MethodApplyTextAndLayout,
			TranslationId = "MethodApplyTextAndLayout"
		},
		new MethodInfo
		{
			MethodId = "AddVideoWallPreset",
			DisplayName = ConfigurationApiResources.MethodAddVideoWallPreset,
			TranslationId = "MethodAddVideoWallPreset"
		},
		new MethodInfo
		{
			MethodId = "RemoveVideoWallPreset",
			DisplayName = ConfigurationApiResources.MethodRemoveVideoWallPreset,
			TranslationId = "MethodRemoveVideoWallPreset"
		},
		new MethodInfo
		{
			MethodId = "Activate",
			DisplayName = ConfigurationApiResources.MethodActivateVideoWallPreset,
			TranslationId = "MethodActivateVideoWallPreset"
		},
		new MethodInfo
		{
			MethodId = "GetAudioMessageContent",
			DisplayName = ConfigurationApiResources.MethodGetContentAudioMessage,
			TranslationId = "MethodGetContentAudioMessage"
		},
		new MethodInfo
		{
			MethodId = "SetAudioMessageContent",
			DisplayName = ConfigurationApiResources.MethodSetContentAudioMessage,
			TranslationId = "MethodSetContentAudioMessage"
		},
		new MethodInfo
		{
			MethodId = "AddAudioMessage",
			DisplayName = ConfigurationApiResources.MethodAddAudioMessage,
			TranslationId = "MethodAddAudioMessage"
		},
		new MethodInfo
		{
			MethodId = "RemoveAudioMessage",
			DisplayName = ConfigurationApiResources.MethodRemoveAudioMessage,
			TranslationId = "MethodRemoveAudioMessage"
		},
		new MethodInfo
		{
			MethodId = "AddUserDefinedEvent",
			DisplayName = ConfigurationApiResources.MethodAddUserDefinedEvent,
			TranslationId = "MethodAddUserDefinedEvent"
		},
		new MethodInfo
		{
			MethodId = "RemoveUserDefinedEvent",
			DisplayName = ConfigurationApiResources.MethodRemoveUserDefinedEvent,
			TranslationId = "MethodRemoveUserDefinedEvent"
		},
		new MethodInfo
		{
			MethodId = "AddAnalyticsEvent",
			DisplayName = ConfigurationApiResources.MethodAddAnalyticsEvent,
			TranslationId = "MethodAddAnalyticsEvent"
		},
		new MethodInfo
		{
			MethodId = "RemoveAnalyticsEvent",
			DisplayName = ConfigurationApiResources.MethodRemoveAnalyticsEvent,
			TranslationId = "MethodRemoveAnalyticsEvent"
		},
		new MethodInfo
		{
			MethodId = "AddGenericEvent",
			DisplayName = ConfigurationApiResources.MethodAddGenericEvent,
			TranslationId = "MethodAddGenericEvent"
		},
		new MethodInfo
		{
			MethodId = "RemoveGenericEvent",
			DisplayName = ConfigurationApiResources.MethodRemoveGenericEvent,
			TranslationId = "MethodRemoveGenericEvent"
		},
		new MethodInfo
		{
			MethodId = "AddGenericEventDataSource",
			DisplayName = ConfigurationApiResources.MethodAddGenericEventDataSource,
			TranslationId = "MethodAddGenericEventDataSource"
		},
		new MethodInfo
		{
			MethodId = "RemoveGenericEventDataSource",
			DisplayName = ConfigurationApiResources.MethodRemoveGenericEventDataSource,
			TranslationId = "MethodRemoveGenericEventDataSource"
		},
		new MethodInfo
		{
			MethodId = "AddGisMapLocation",
			DisplayName = ConfigurationApiResources.MethodIdAddGisMapLocation,
			TranslationId = "MethodIdAddGisMapLocation"
		},
		new MethodInfo
		{
			MethodId = "RemoveGisMapLocation",
			DisplayName = ConfigurationApiResources.MethodIdRemoveGisMapLocation,
			TranslationId = "MethodIdRemoveGisMapLocation"
		},
		new MethodInfo
		{
			MethodId = "AddTimeProfile",
			DisplayName = ConfigurationApiResources.MethodAddTimeProfile,
			TranslationId = "MethodAddTimeProfile"
		},
		new MethodInfo
		{
			MethodId = "RemoveTimeProfile",
			DisplayName = ConfigurationApiResources.MethodRemoveTimeProfile,
			TranslationId = "MethodRemoveTimeProfile"
		},
		new MethodInfo
		{
			MethodId = "AddAppointment",
			DisplayName = ConfigurationApiResources.MethodAddAppointment,
			TranslationId = "MethodAddAppointment"
		},
		new MethodInfo
		{
			MethodId = "RemoveAppointment",
			DisplayName = ConfigurationApiResources.MethodRemoveAppointment,
			TranslationId = "MethodRemoveAppointment"
		},
		new MethodInfo
		{
			MethodId = "AddRecurringAppointment",
			DisplayName = ConfigurationApiResources.MethodAddRecurringAppointment,
			TranslationId = "MethodAddRecurringAppointment"
		},
		new MethodInfo
		{
			MethodId = "RemoveRecurringAppointment",
			DisplayName = ConfigurationApiResources.MethodRemoveRecurringAppointment,
			TranslationId = "MethodRemoveRecurringAppointment"
		},
		new MethodInfo
		{
			MethodId = "AddRecurringVariance",
			DisplayName = ConfigurationApiResources.MethodAddRecurringVariance,
			TranslationId = "MethodAddRecurringVariance"
		},
		new MethodInfo
		{
			MethodId = "RemoveRecurringVariance",
			DisplayName = ConfigurationApiResources.MethodRemoveRecurringVariance,
			TranslationId = "MethodRemoveRecurringVariance"
		},
		new MethodInfo
		{
			MethodId = "AddLayout",
			DisplayName = ConfigurationApiResources.MethodAddLayout,
			TranslationId = "MethodAddLayout"
		},
		new MethodInfo
		{
			MethodId = "RemoveLayout",
			DisplayName = ConfigurationApiResources.MethodRemoveLayout,
			TranslationId = "MethodRemoveLayout"
		},
		new MethodInfo
		{
			MethodId = "AddMIPKind",
			DisplayName = ConfigurationApiResources.MethodAddMIPKind,
			TranslationId = "MethodAddMIPKind"
		},
		new MethodInfo
		{
			MethodId = "RemoveMIPKind",
			DisplayName = ConfigurationApiResources.MethodRemoveMIPKind,
			TranslationId = "MethodRemoveMIPKind"
		},
		new MethodInfo
		{
			MethodId = "AddMIPItem",
			DisplayName = ConfigurationApiResources.MethodAddMIPItem,
			TranslationId = "MethodAddMIPItem"
		},
		new MethodInfo
		{
			MethodId = "RemoveMIPItem",
			DisplayName = ConfigurationApiResources.MethodRemoveMIPItem,
			TranslationId = "MethodRemoveMIPItem"
		},
		new MethodInfo
		{
			MethodId = "GetMIPOptionProperty",
			DisplayName = ConfigurationApiResources.MethodIdGetMIPOptionProperty,
			TranslationId = "MethodIdGetMIPOptionProperty"
		},
		new MethodInfo
		{
			MethodId = "SetMIPOptionProperty",
			DisplayName = ConfigurationApiResources.MethodIdSetMIPOptionProperty,
			TranslationId = "MethodIdSetMIPOptionProperty"
		},
		new MethodInfo
		{
			MethodId = "MIPItemDataVersionSum",
			DisplayName = ConfigurationApiResources.MethodIdMIPItemDataVersionSum,
			TranslationId = "MethodIdMIPItemDataVersionSum"
		},
		new MethodInfo
		{
			MethodId = "AddAlarmDefinition",
			DisplayName = ConfigurationApiResources.MethodAddAlarmDefinition,
			TranslationId = "MethodAddAlarmDefinition"
		},
		new MethodInfo
		{
			MethodId = "RemoveAlarmDefinition",
			DisplayName = ConfigurationApiResources.MethodRemoveAlarmDefinition,
			TranslationId = "MethodRemoveAlarmDefinition"
		},
		new MethodInfo
		{
			MethodId = "MethodIdGetCustomFieldsForRegistrationNumber",
			DisplayName = ConfigurationApiResources.MethodGetCustomFieldsForRegistrationNumber,
			TranslationId = "MethodGetCustomFieldsForRegistrationNumber"
		},
		new MethodInfo
		{
			MethodId = "MethodIdGetRegistrationNumbersInfo",
			DisplayName = ConfigurationApiResources.MethodGetRegistrationNumbersInfo,
			TranslationId = "MethodGetRegistrationNumbersInfo"
		},
		new MethodInfo
		{
			MethodId = "MethodIdAddOrEditRegistrationNumbersInfo",
			DisplayName = ConfigurationApiResources.MethodAddEditRegistrationNumbersInfo,
			TranslationId = "MethodAddEditRegistrationNumbersInfo"
		},
		new MethodInfo
		{
			MethodId = "MethodIdDeleteRegistrationNumbers",
			DisplayName = ConfigurationApiResources.MethodDeleteRegistrationNumbers,
			TranslationId = "MethodDeleteRegistrationNumbers"
		},
		new MethodInfo
		{
			MethodId = "MethodIdDeleteAllRegistrationNumbers",
			DisplayName = ConfigurationApiResources.MethodDeleteAllRegistrationNumbers,
			TranslationId = "MethodDeleteAllRegistrationNumbers"
		},
		new MethodInfo
		{
			MethodId = "MethodIdAddLprMatchList",
			DisplayName = ConfigurationApiResources.MethodAddLprMatchList,
			TranslationId = "MethodAddLprMatchList"
		},
		new MethodInfo
		{
			MethodId = "MethodIdRemoveLprMatchList",
			DisplayName = ConfigurationApiResources.MethodRemoveLprMatchList,
			TranslationId = "MethodRemoveLprMatchList"
		},
		new MethodInfo
		{
			MethodId = "AddSaveSearch",
			DisplayName = ConfigurationApiResources.MethodAddSearchTemplate,
			TranslationId = "MethodAddSearchTemplate"
		},
		new MethodInfo
		{
			MethodId = "UpdateSaveSearch",
			DisplayName = ConfigurationApiResources.MethodUpdateSearchTemplate,
			TranslationId = "MethodUpdateSearchTemplate"
		},
		new MethodInfo
		{
			MethodId = "RemoveSaveSearch",
			DisplayName = ConfigurationApiResources.MethodRemoveSearchTemplate,
			TranslationId = "MethodRemoveSearchTemplate"
		},
		new MethodInfo
		{
			MethodId = "GetSaveSearch",
			DisplayName = ConfigurationApiResources.MethodGetSearchTemplate,
			TranslationId = "MethodGetSearchTemplate"
		},
		new MethodInfo
		{
			MethodId = "FindSaveSearches",
			DisplayName = ConfigurationApiResources.MethodFindSearchTemplates,
			TranslationId = "MethodFindSearchTemplates"
		},
		new MethodInfo
		{
			MethodId = "AddRule",
			DisplayName = ConfigurationApiResources.MethodAddRule,
			TranslationId = "MethodAddRule"
		},
		new MethodInfo
		{
			MethodId = "RemoveRule",
			DisplayName = ConfigurationApiResources.MethodRemoveRule,
			TranslationId = "MethodRemoveRule"
		},
		new MethodInfo
		{
			MethodId = "GetInternalUnitPath",
			DisplayName = ConfigurationApiResources.MethodGetInternalUnitPath,
			TranslationId = "MethodGetInternalUnitPath"
		},
		new MethodInfo
		{
			MethodId = "AddBasicOwnerInfo",
			DisplayName = ConfigurationApiResources.MethodAddBasicOwnerInfo,
			TranslationId = "MethodAddBasicOwnerInfo"
		},
		new MethodInfo
		{
			MethodId = "RemoveBasicOwnerInfo",
			DisplayName = ConfigurationApiResources.MethodRemoveBasicOwnerInfo,
			TranslationId = "MethodRemoveBasicOwnerInfo"
		},
		new MethodInfo
		{
			MethodId = "ActivateLicense",
			DisplayName = ConfigurationApiResources.MethodActivateLicense,
			TranslationId = "MethodActivateLicense"
		},
		new MethodInfo
		{
			MethodId = "RequestLicense",
			DisplayName = ConfigurationApiResources.MethodRequestLicense,
			TranslationId = "MethodRequestLicense"
		},
		new MethodInfo
		{
			MethodId = "UpdateLicense",
			DisplayName = ConfigurationApiResources.MethodUpdateLicense,
			TranslationId = "MethodUpdateLicense"
		},
		new MethodInfo
		{
			MethodId = "ChangeLicense",
			DisplayName = ConfigurationApiResources.MethodChangeLicense,
			TranslationId = "MethodChangeLicense"
		},
		new MethodInfo
		{
			MethodId = "StopAutomaticLicenseActivation",
			DisplayName = ConfigurationApiResources.MethodStopAutomaticLicenseActivation,
			TranslationId = "MethodStopAutomaticLicenseActivation"
		},
		new MethodInfo
		{
			MethodId = "UploadFileChunk",
			DisplayName = ConfigurationApiResources.MethodUploadFileChunk,
			TranslationId = "MethodUploadFileChunk"
		},
		new MethodInfo
		{
			MethodId = "AddViewGroup",
			DisplayName = ConfigurationApiResources.MethodAddViewGroup,
			TranslationId = "MethodAddViewGroup"
		},
		new MethodInfo
		{
			MethodId = "RemoveViewGroup",
			DisplayName = ConfigurationApiResources.MethodRemoveViewGroup,
			TranslationId = "MethodRemoveViewGroup"
		},
		new MethodInfo
		{
			MethodId = "AddView",
			DisplayName = ConfigurationApiResources.MethodAddView,
			TranslationId = "MethodAddView"
		},
		new MethodInfo
		{
			MethodId = "RemoveView",
			DisplayName = ConfigurationApiResources.MethodRemoveView,
			TranslationId = "MethodRemoveView"
		},
		new MethodInfo
		{
			MethodId = "GetDevicePresets",
			DisplayName = ConfigurationApiResources.MethodGetDevicePresets,
			TranslationId = "MethodGetDevicePresets",
			ToolTipTranslationId = "MethodGetDevicePresetsToolTip"
		},
		new MethodInfo
		{
			MethodId = "AddPatrollingEntry",
			DisplayName = ConfigurationApiResources.MethodIdAddPatrollingEntry,
			TranslationId = "MethodIdAddPatrollingEntry"
		},
		new MethodInfo
		{
			MethodId = "RemovePatrollingEntry",
			DisplayName = ConfigurationApiResources.MethodIdRemovePatrollingEntry,
			TranslationId = "MethodIdRemovePatrollingEntry"
		},
		new MethodInfo
		{
			MethodId = "AddPatrollingProfile",
			DisplayName = ConfigurationApiResources.MethodIdAddPatrollingProfile,
			TranslationId = "MethodIdAddPatrollingProfile"
		},
		new MethodInfo
		{
			MethodId = "RemovePatrollingProfile",
			DisplayName = ConfigurationApiResources.MethodIdRemovePatrollingProfile,
			TranslationId = "MethodIdRemovePatrollingProfile"
		},
		new MethodInfo
		{
			MethodId = "AddFailoverGroup",
			DisplayName = ConfigurationApiResources.MethodIdAddFailoverGroup,
			TranslationId = "MethodIdAddFailoverGroup"
		},
		new MethodInfo
		{
			MethodId = "RemoveFailoverGroup",
			DisplayName = ConfigurationApiResources.MethodIdRemoveFailoverGroup,
			TranslationId = "MethodIdRemoveFailoverGroup"
		},
		new MethodInfo
		{
			MethodId = "MoveFailoverGroup",
			DisplayName = ConfigurationApiResources.MethodIdMoveFailoverGroup,
			TranslationId = "MethodIdMoveFailoverGroup"
		},
		new MethodInfo
		{
			MethodId = "AddClientProfile",
			DisplayName = ConfigurationApiResources.MethodIdAddClientProfile,
			TranslationId = "MethodIdAddClientProfile"
		},
		new MethodInfo
		{
			MethodId = "RemoveClientProfile",
			DisplayName = ConfigurationApiResources.MethodIdRemoveClientProfile,
			TranslationId = "MethodIdRemoveClientProfile"
		},
		new MethodInfo
		{
			MethodId = "ClientProfileUpPriority",
			DisplayName = ConfigurationApiResources.MethodClientProfileUpPriority,
			TranslationId = "MethodClientProfileUpPriority"
		},
		new MethodInfo
		{
			MethodId = "ClientProfileDownPriority",
			DisplayName = ConfigurationApiResources.MethodClientProfileDownPriority,
			TranslationId = "MethodClientProfileDownPriority"
		},
		new MethodInfo
		{
			MethodId = "SetClientProfile",
			DisplayName = ConfigurationApiResources.MethodSetClientProfile,
			TranslationId = "MethodSetClientProfile"
		},
		new MethodInfo
		{
			MethodId = "AddStateGroup",
			DisplayName = ConfigurationApiResources.MethodAddStateGroup,
			TranslationId = "MethodAddStateGroup"
		},
		new MethodInfo
		{
			MethodId = "RemoveStateGroup",
			DisplayName = ConfigurationApiResources.MethodRemoveStateGroup,
			TranslationId = "MethodRemoveStateGroup"
		},
		new MethodInfo
		{
			MethodId = "AddMatrix",
			DisplayName = ConfigurationApiResources.MethodAddMatrix,
			TranslationId = "MethodAddMatrix"
		},
		new MethodInfo
		{
			MethodId = "RemoveMatrix",
			DisplayName = ConfigurationApiResources.MethodRemoveMatrix,
			TranslationId = "MethodRemoveMatrix"
		},
		new MethodInfo
		{
			MethodId = "MatrixCommand",
			DisplayName = ConfigurationApiResources.MethodMatrixCommand,
			TranslationId = "MethodMatrixCommand"
		},
		new MethodInfo
		{
			MethodId = "AddEvidenceLockProfile",
			DisplayName = ConfigurationApiResources.MethodAddEvidenceLockProfile,
			TranslationId = "MethodAddEvidenceLockProfile"
		},
		new MethodInfo
		{
			MethodId = "RemoveEvidenceLockProfile",
			DisplayName = ConfigurationApiResources.MethodRemoveEvidenceLockProfile,
			TranslationId = "MethodRemoveEvidenceLockProfile"
		},
		new MethodInfo
		{
			MethodId = "QueryUsersWithAccess",
			DisplayName = ConfigurationApiResources.MethodQueryUsersWithAccess,
			TranslationId = "MethodQueryUsersWithAccess"
		},
		new MethodInfo
		{
			MethodId = "AddMailNotificationProfile",
			DisplayName = ConfigurationApiResources.MethodAddMailNotification,
			TranslationId = "MethodAddMailNotification"
		},
		new MethodInfo
		{
			MethodId = "RemoveMailNotificationProfile",
			DisplayName = ConfigurationApiResources.MethodRemoveMailNotification,
			TranslationId = "MethodRemoveMailNotification"
		},
		new MethodInfo
		{
			MethodId = "ApplyViewToView",
			DisplayName = ConfigurationApiResources.MethodApplyViewToView,
			TranslationId = "MethodApplyViewToView",
			ToolTipTranslationId = "MethodApplyViewToViewToolTip"
		},
		new MethodInfo
		{
			MethodId = "ClearView",
			DisplayName = ConfigurationApiResources.MethodClearView,
			TranslationId = "MethodClearView"
		},
		new MethodInfo
		{
			MethodId = "ClearViewItem",
			DisplayName = ConfigurationApiResources.MethodClearViewItem,
			TranslationId = "MethodClearViewItem"
		},
		new MethodInfo
		{
			MethodId = "QueryIsPermittedTimeBasedAction",
			DisplayName = ConfigurationApiResources.MethodQueryIsPermittedTimeBasedAction,
			TranslationId = "MethodQueryIsPermittedTimeBasedAction"
		},
		new MethodInfo
		{
			MethodId = "AddAccessControlGroup",
			DisplayName = ConfigurationApiResources.MethodAddAccessControlGroup,
			TranslationId = "MethodAddAccessControlGroup"
		},
		new MethodInfo
		{
			MethodId = "RemoveAccessControlGroup",
			DisplayName = ConfigurationApiResources.MethodRemoveAccessControlGroup,
			TranslationId = "MethodRemoveAccessControlGroup"
		},
		new MethodInfo
		{
			MethodId = "AddAccessControlGroupMember",
			DisplayName = ConfigurationApiResources.MethodAddAccessControlGroupMember,
			TranslationId = "MethodAddAccessControlGroupMember"
		},
		new MethodInfo
		{
			MethodId = "RemoveAccessControlGroupMember",
			DisplayName = ConfigurationApiResources.MethodRemoveAccessControlGroupMember,
			TranslationId = "MethodRemoveAccessControlGroupMember"
		}
	};

	public static MethodInfo[] GetAllMethods()
	{
		return _methodInfoArray;
	}

	public static MethodInfo GetMethod(string methodId)
	{
		return _methodInfoArray.FirstOrDefault((MethodInfo f) => f.MethodId == methodId);
	}
}
