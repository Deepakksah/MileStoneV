using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Resources;
using System.Runtime.CompilerServices;

namespace VideoOS.Common.Integration.Properties;

[GeneratedCode("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
[DebuggerNonUserCode]
[CompilerGenerated]
public class LogDefinitions
{
	private static ResourceManager resourceMan;

	private static CultureInfo resourceCulture;

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static ResourceManager ResourceManager
	{
		get
		{
			if (resourceMan == null)
			{
				resourceMan = new ResourceManager("VideoOS.Common.Integration.Properties.LogDefinitions", typeof(LogDefinitions).Assembly);
			}
			return resourceMan;
		}
	}

	[EditorBrowsable(EditorBrowsableState.Advanced)]
	public static CultureInfo Culture
	{
		get
		{
			return resourceCulture;
		}
		set
		{
			resourceCulture = value;
		}
	}

	public static string ArchiveDatabaseStorageAvailable => ResourceManager.GetString("ArchiveDatabaseStorageAvailable", resourceCulture);

	public static string ArchiveDatabaseStorageUnavailable => ResourceManager.GetString("ArchiveDatabaseStorageUnavailable", resourceCulture);

	public static string ArchiveStorageDeleteBegin => ResourceManager.GetString("ArchiveStorageDeleteBegin", resourceCulture);

	public static string ArchiveStorageDeleteFailedLastRetry => ResourceManager.GetString("ArchiveStorageDeleteFailedLastRetry", resourceCulture);

	public static string ArchiveStorageDeleteFailedNoRetry => ResourceManager.GetString("ArchiveStorageDeleteFailedNoRetry", resourceCulture);

	public static string ArchiveStorageDeleteFailedRetry => ResourceManager.GetString("ArchiveStorageDeleteFailedRetry", resourceCulture);

	public static string ArchiveStorageDeleteSuccess => ResourceManager.GetString("ArchiveStorageDeleteSuccess", resourceCulture);

	public static string ArchiveStorageMountBegin => ResourceManager.GetString("ArchiveStorageMountBegin", resourceCulture);

	public static string ArchiveStorageMountFailed => ResourceManager.GetString("ArchiveStorageMountFailed", resourceCulture);

	public static string ArchiveStorageMountSuccess => ResourceManager.GetString("ArchiveStorageMountSuccess", resourceCulture);

	public static string ArchiveStorageMoveBegin => ResourceManager.GetString("ArchiveStorageMoveBegin", resourceCulture);

	public static string ArchiveStorageMoveFailedLastRetry => ResourceManager.GetString("ArchiveStorageMoveFailedLastRetry", resourceCulture);

	public static string ArchiveStorageMoveFailedNoRetry => ResourceManager.GetString("ArchiveStorageMoveFailedNoRetry", resourceCulture);

	public static string ArchiveStorageMoveFailedRetry => ResourceManager.GetString("ArchiveStorageMoveFailedRetry", resourceCulture);

	public static string ArchiveStorageMoveSuccess => ResourceManager.GetString("ArchiveStorageMoveSuccess", resourceCulture);

	public static string ArchiveStorageUnmountBegin => ResourceManager.GetString("ArchiveStorageUnmountBegin", resourceCulture);

	public static string ArchiveStorageUnmountFailed => ResourceManager.GetString("ArchiveStorageUnmountFailed", resourceCulture);

	public static string ArchiveStorageUnmountSuccess => ResourceManager.GetString("ArchiveStorageUnmountSuccess", resourceCulture);

	public static string AuditAudioMessageContentChanged => ResourceManager.GetString("AuditAudioMessageContentChanged", resourceCulture);

	public static string AuditAudioMessageCreated => ResourceManager.GetString("AuditAudioMessageCreated", resourceCulture);

	public static string AuditAudioMessageDeleted => ResourceManager.GetString("AuditAudioMessageDeleted", resourceCulture);

	public static string AuditAuthorizationFailedInsufficientRights => ResourceManager.GetString("AuditAuthorizationFailedInsufficientRights", resourceCulture);

	public static string AuditAuthorizationFailedSelfAuthorization => ResourceManager.GetString("AuditAuthorizationFailedSelfAuthorization", resourceCulture);

	public static string AuditAuthorizationSucessful => ResourceManager.GetString("AuditAuthorizationSucessful", resourceCulture);

	public static string AuditAuthorizeRecorder => ResourceManager.GetString("AuditAuthorizeRecorder", resourceCulture);

	public static string AuditAutomaticactivationCredentialsChanged => ResourceManager.GetString("AuditAutomaticactivationCredentialsChanged", resourceCulture);

	public static string AuditAutomaticactivationDisabled => ResourceManager.GetString("AuditAutomaticactivationDisabled", resourceCulture);

	public static string AuditAutomaticactivationEnabled => ResourceManager.GetString("AuditAutomaticactivationEnabled", resourceCulture);

	public static string AuditBasicInfoCreated => ResourceManager.GetString("AuditBasicInfoCreated", resourceCulture);

	public static string AuditBasicInfoDeleted => ResourceManager.GetString("AuditBasicInfoDeleted", resourceCulture);

	public static string AuditBasicInfoEdited => ResourceManager.GetString("AuditBasicInfoEdited", resourceCulture);

	public static string AuditBasicUserChangeOwnPassword => ResourceManager.GetString("AuditBasicUserChangeOwnPassword", resourceCulture);

	public static string AuditBasicUserCreated => ResourceManager.GetString("AuditBasicUserCreated", resourceCulture);

	public static string AuditBasicUserCreatedIdentityProvider => ResourceManager.GetString("AuditBasicUserCreatedIdentityProvider", resourceCulture);

	public static string AuditBasicUserDeleted => ResourceManager.GetString("AuditBasicUserDeleted", resourceCulture);

	public static string AuditBasicUserDeletedIdentityProvider => ResourceManager.GetString("AuditBasicUserDeletedIdentityProvider", resourceCulture);

	public static string AuditBasicUserDescriptionChanged => ResourceManager.GetString("AuditBasicUserDescriptionChanged", resourceCulture);

	public static string AuditBasicUserDisabled => ResourceManager.GetString("AuditBasicUserDisabled", resourceCulture);

	public static string AuditBasicUserEdited => ResourceManager.GetString("AuditBasicUserEdited", resourceCulture);

	public static string AuditBasicUserEnabled => ResourceManager.GetString("AuditBasicUserEnabled", resourceCulture);

	public static string AuditBasicUserEnabledIdentityProvider => ResourceManager.GetString("AuditBasicUserEnabledIdentityProvider", resourceCulture);

	public static string AuditBasicUserForcePasswordChange => ResourceManager.GetString("AuditBasicUserForcePasswordChange", resourceCulture);

	public static string AuditBasicUserForcePasswordChangeCleared => ResourceManager.GetString("AuditBasicUserForcePasswordChangeCleared", resourceCulture);

	public static string AuditBasicUserForcePasswordChangeIdentityProvider => ResourceManager.GetString("AuditBasicUserForcePasswordChangeIdentityProvider", resourceCulture);

	public static string AuditBasicUserLockedOut => ResourceManager.GetString("AuditBasicUserLockedOut", resourceCulture);

	public static string AuditBasicUserLockedOutCleared => ResourceManager.GetString("AuditBasicUserLockedOutCleared", resourceCulture);

	public static string AuditBasicUserPasswordChanged => ResourceManager.GetString("AuditBasicUserPasswordChanged", resourceCulture);

	public static string AuditBasicUserRenamed => ResourceManager.GetString("AuditBasicUserRenamed", resourceCulture);

	public static string AuditBasicUserSystemLockedOut => ResourceManager.GetString("AuditBasicUserSystemLockedOut", resourceCulture);

	public static string AuditBookmarkCreated => ResourceManager.GetString("AuditBookmarkCreated", resourceCulture);

	public static string AuditBookmarkDeleted => ResourceManager.GetString("AuditBookmarkDeleted", resourceCulture);

	public static string AuditBookmarkEdited => ResourceManager.GetString("AuditBookmarkEdited", resourceCulture);

	public static string AuditBookmarkViewed => ResourceManager.GetString("AuditBookmarkViewed", resourceCulture);

	public static string AuditClaimCreated => ResourceManager.GetString("AuditClaimCreated", resourceCulture);

	public static string AuditClaimDeleted => ResourceManager.GetString("AuditClaimDeleted", resourceCulture);

	public static string AuditClaimUpdated => ResourceManager.GetString("AuditClaimUpdated", resourceCulture);

	public static string AuditClientBrowseSequence => ResourceManager.GetString("AuditClientBrowseSequence", resourceCulture);

	public static string AuditClientLiveFeedRequested => ResourceManager.GetString("AuditClientLiveFeedRequested", resourceCulture);

	public static string AuditClientLiveFeedTerminated => ResourceManager.GetString("AuditClientLiveFeedTerminated", resourceCulture);

	public static string AuditClientProfileCreated => ResourceManager.GetString("AuditClientProfileCreated", resourceCulture);

	public static string AuditClientProfileDeleted => ResourceManager.GetString("AuditClientProfileDeleted", resourceCulture);

	public static string AuditClientProfileEdited => ResourceManager.GetString("AuditClientProfileEdited", resourceCulture);

	public static string AuditClientProfileRenamed => ResourceManager.GetString("AuditClientProfileRenamed", resourceCulture);

	public static string AuditClientRedirectUrisSet => ResourceManager.GetString("AuditClientRedirectUrisSet", resourceCulture);

	public static string AuditConfigReportExported => ResourceManager.GetString("AuditConfigReportExported", resourceCulture);

	public static string AuditConfigurationExported => ResourceManager.GetString("AuditConfigurationExported", resourceCulture);

	public static string AuditConfigurationRestored => ResourceManager.GetString("AuditConfigurationRestored", resourceCulture);

	public static string AuditCreateRecordingServer => ResourceManager.GetString("AuditCreateRecordingServer", resourceCulture);

	public static string AuditDevice360LensSettingsChanged => ResourceManager.GetString("AuditDevice360LensSettingsChanged", resourceCulture);

	public static string AuditDevice360LensSettingsDisabled => ResourceManager.GetString("AuditDevice360LensSettingsDisabled", resourceCulture);

	public static string AuditDevice360LensSettingsEnabled => ResourceManager.GetString("AuditDevice360LensSettingsEnabled", resourceCulture);

	public static string AuditDeviceAdded => ResourceManager.GetString("AuditDeviceAdded", resourceCulture);

	public static string AuditDeviceArchiveSettingsChanged => ResourceManager.GetString("AuditDeviceArchiveSettingsChanged", resourceCulture);

	public static string AuditDeviceCoverageDepthChanged => ResourceManager.GetString("AuditDeviceCoverageDepthChanged", resourceCulture);

	public static string AuditDeviceCoverageDirectionChanged => ResourceManager.GetString("AuditDeviceCoverageDirectionChanged", resourceCulture);

	public static string AuditDeviceCoverageFieldOfViewChanged => ResourceManager.GetString("AuditDeviceCoverageFieldOfViewChanged", resourceCulture);

	public static string AuditDeviceDatabaseDelete => ResourceManager.GetString("AuditDeviceDatabaseDelete", resourceCulture);

	public static string AuditDeviceDatabaseMove => ResourceManager.GetString("AuditDeviceDatabaseMove", resourceCulture);

	public static string AuditDeviceDisabled => ResourceManager.GetString("AuditDeviceDisabled", resourceCulture);

	public static string AuditDeviceEnabled => ResourceManager.GetString("AuditDeviceEnabled", resourceCulture);

	public static string AuditDeviceFailoverSettingChanged => ResourceManager.GetString("AuditDeviceFailoverSettingChanged", resourceCulture);

	public static string AuditDeviceGisPointChanged => ResourceManager.GetString("AuditDeviceGisPointChanged", resourceCulture);

	public static string AuditDeviceIconChanged => ResourceManager.GetString("AuditDeviceIconChanged", resourceCulture);

	public static string AuditDeviceMotionDetectionSettingsChanged => ResourceManager.GetString("AuditDeviceMotionDetectionSettingsChanged", resourceCulture);

	public static string AuditDeviceMulticastEnabledChanged => ResourceManager.GetString("AuditDeviceMulticastEnabledChanged", resourceCulture);

	public static string AuditDevicePrivacyMaskSettingsChanged => ResourceManager.GetString("AuditDevicePrivacyMaskSettingsChanged", resourceCulture);

	public static string AuditDevicePtzExtendedManualSessionTimeoutChanged => ResourceManager.GetString("AuditDevicePtzExtendedManualSessionTimeoutChanged", resourceCulture);

	public static string AuditDevicePtzManuelSessionTimeoutChanged => ResourceManager.GetString("AuditDevicePtzManuelSessionTimeoutChanged", resourceCulture);

	public static string AuditDevicePtzReservedSessionTimeoutChanged => ResourceManager.GetString("AuditDevicePtzReservedSessionTimeoutChanged", resourceCulture);

	public static string AuditDeviceRecordingDisabled => ResourceManager.GetString("AuditDeviceRecordingDisabled", resourceCulture);

	public static string AuditDeviceRecordingEdgeStorageDisabled => ResourceManager.GetString("AuditDeviceRecordingEdgeStorageDisabled", resourceCulture);

	public static string AuditDeviceRecordingEdgeStorageEnabled => ResourceManager.GetString("AuditDeviceRecordingEdgeStorageEnabled", resourceCulture);

	public static string AuditDeviceRecordingEdgeStoragePlaybackDisabled => ResourceManager.GetString("AuditDeviceRecordingEdgeStoragePlaybackDisabled", resourceCulture);

	public static string AuditDeviceRecordingEdgeStoragePlaybackEnabled => ResourceManager.GetString("AuditDeviceRecordingEdgeStoragePlaybackEnabled", resourceCulture);

	public static string AuditDeviceRecordingEnabled => ResourceManager.GetString("AuditDeviceRecordingEnabled", resourceCulture);

	public static string AuditDeviceRecordingPrebufferDisabled => ResourceManager.GetString("AuditDeviceRecordingPrebufferDisabled", resourceCulture);

	public static string AuditDeviceRecordingPrebufferEnabled => ResourceManager.GetString("AuditDeviceRecordingPrebufferEnabled", resourceCulture);

	public static string AuditDeviceRecordingStorageSelected => ResourceManager.GetString("AuditDeviceRecordingStorageSelected", resourceCulture);

	public static string AuditDeviceRecordSettingsChanged => ResourceManager.GetString("AuditDeviceRecordSettingsChanged", resourceCulture);

	public static string AuditDeviceRemoved => ResourceManager.GetString("AuditDeviceRemoved", resourceCulture);

	public static string AuditDeviceRenamed => ResourceManager.GetString("AuditDeviceRenamed", resourceCulture);

	public static string AuditDeviceReplaced => ResourceManager.GetString("AuditDeviceReplaced", resourceCulture);

	public static string AuditDeviceReplacedNoDatabase => ResourceManager.GetString("AuditDeviceReplacedNoDatabase", resourceCulture);

	public static string AuditDeviceRequestEdgeStorageRetrieval => ResourceManager.GetString("AuditDeviceRequestEdgeStorageRetrieval", resourceCulture);

	public static string AuditDeviceSettingsChanged => ResourceManager.GetString("AuditDeviceSettingsChanged", resourceCulture);

	public static string AuditDeviceShortcutChanged => ResourceManager.GetString("AuditDeviceShortcutChanged", resourceCulture);

	public static string AuditDeviceShortNameChanged => ResourceManager.GetString("AuditDeviceShortNameChanged", resourceCulture);

	public static string AuditDeviceStreamSettingsChanged => ResourceManager.GetString("AuditDeviceStreamSettingsChanged", resourceCulture);

	public static string AuditDirectoryCreatedOnRecorder => ResourceManager.GetString("AuditDirectoryCreatedOnRecorder", resourceCulture);

	public static string AuditExportCameraCancelled => ResourceManager.GetString("AuditExportCameraCancelled", resourceCulture);

	public static string AuditExportCameraCompleted => ResourceManager.GetString("AuditExportCameraCompleted", resourceCulture);

	public static string AuditExportCameraToAvi => ResourceManager.GetString("AuditExportCameraToAvi", resourceCulture);

	public static string AuditExportCameraToDBNoEncryption => ResourceManager.GetString("AuditExportCameraToDBNoEncryption", resourceCulture);

	public static string AuditExportCameraToDBWithEncryption => ResourceManager.GetString("AuditExportCameraToDBWithEncryption", resourceCulture);

	public static string AuditExportMetadataCancelled => ResourceManager.GetString("AuditExportMetadataCancelled", resourceCulture);

	public static string AuditExportMetadataCompleted => ResourceManager.GetString("AuditExportMetadataCompleted", resourceCulture);

	public static string AuditExportMetadataToDBNoEncryption => ResourceManager.GetString("AuditExportMetadataToDBNoEncryption", resourceCulture);

	public static string AuditExportMetadataToDBWithEncryption => ResourceManager.GetString("AuditExportMetadataToDBWithEncryption", resourceCulture);

	public static string AuditExportMicrophoneCancelled => ResourceManager.GetString("AuditExportMicrophoneCancelled", resourceCulture);

	public static string AuditExportMicrophoneCompleted => ResourceManager.GetString("AuditExportMicrophoneCompleted", resourceCulture);

	public static string AuditExportMicrophoneToAvi => ResourceManager.GetString("AuditExportMicrophoneToAvi", resourceCulture);

	public static string AuditExportMicrophoneToDBNoEncryption => ResourceManager.GetString("AuditExportMicrophoneToDBNoEncryption", resourceCulture);

	public static string AuditExportMicrophoneToDBWithEncryption => ResourceManager.GetString("AuditExportMicrophoneToDBWithEncryption", resourceCulture);

	public static string AuditExportSpeakerCancelled => ResourceManager.GetString("AuditExportSpeakerCancelled", resourceCulture);

	public static string AuditExportSpeakerCompleted => ResourceManager.GetString("AuditExportSpeakerCompleted", resourceCulture);

	public static string AuditExportSpeakerToAvi => ResourceManager.GetString("AuditExportSpeakerToAvi", resourceCulture);

	public static string AuditExportSpeakerToDBNoEncryption => ResourceManager.GetString("AuditExportSpeakerToDBNoEncryption", resourceCulture);

	public static string AuditExportSpeakerToDBWithEncryption => ResourceManager.GetString("AuditExportSpeakerToDBWithEncryption", resourceCulture);

	public static string AuditFileTransferToRecorderStarted => ResourceManager.GetString("AuditFileTransferToRecorderStarted", resourceCulture);

	public static string AuditFileUploadStarted => ResourceManager.GetString("AuditFileUploadStarted", resourceCulture);

	public static string AuditGenericEventsActivated => ResourceManager.GetString("AuditGenericEventsActivated", resourceCulture);

	public static string AuditGenericEventsDeactivated => ResourceManager.GetString("AuditGenericEventsDeactivated", resourceCulture);

	public static string AuditHardwareAdded => ResourceManager.GetString("AuditHardwareAdded", resourceCulture);

	public static string AuditHardwareDisabled => ResourceManager.GetString("AuditHardwareDisabled", resourceCulture);

	public static string AuditHardwareEnabled => ResourceManager.GetString("AuditHardwareEnabled", resourceCulture);

	public static string AuditHardwareFirmwareUpdateStarted => ResourceManager.GetString("AuditHardwareFirmwareUpdateStarted", resourceCulture);

	public static string AuditHardwareMoved => ResourceManager.GetString("AuditHardwareMoved", resourceCulture);

	public static string AuditHardwarePasswordChanged => ResourceManager.GetString("AuditHardwarePasswordChanged", resourceCulture);

	public static string AuditHardwarePasswordRead => ResourceManager.GetString("AuditHardwarePasswordRead", resourceCulture);

	public static string AuditHardwarePasswordReadFailedInsufficientRights => ResourceManager.GetString("AuditHardwarePasswordReadFailedInsufficientRights", resourceCulture);

	public static string AuditHardwarePreConfigurationStarted => ResourceManager.GetString("AuditHardwarePreConfigurationStarted", resourceCulture);

	public static string AuditHardwareRemoved => ResourceManager.GetString("AuditHardwareRemoved", resourceCulture);

	public static string AuditHardwareRenamed => ResourceManager.GetString("AuditHardwareRenamed", resourceCulture);

	public static string AuditHardwareReplaced => ResourceManager.GetString("AuditHardwareReplaced", resourceCulture);

	public static string AuditHardwareSettingsChanged => ResourceManager.GetString("AuditHardwareSettingsChanged", resourceCulture);

	public static string AuditHostedHardwareAdded => ResourceManager.GetString("AuditHostedHardwareAdded", resourceCulture);

	public static string AuditHostedHardwareDeleted => ResourceManager.GetString("AuditHostedHardwareDeleted", resourceCulture);

	public static string AuditHostedHardwareModified => ResourceManager.GetString("AuditHostedHardwareModified", resourceCulture);

	public static string AuditHostedHardwareRenamed => ResourceManager.GetString("AuditHostedHardwareRenamed", resourceCulture);

	public static string AuditHostingServerAdded => ResourceManager.GetString("AuditHostingServerAdded", resourceCulture);

	public static string AuditHostingServerDeleted => ResourceManager.GetString("AuditHostingServerDeleted", resourceCulture);

	public static string AuditHostingServerModified => ResourceManager.GetString("AuditHostingServerModified", resourceCulture);

	public static string AuditHostingServerRenamed => ResourceManager.GetString("AuditHostingServerRenamed", resourceCulture);

	public static string AuditIdentityProviderAdministratorLoginFailed => ResourceManager.GetString("AuditIdentityProviderAdministratorLoginFailed", resourceCulture);

	public static string AuditIdentityProviderRegistrationChanged => ResourceManager.GetString("AuditIdentityProviderRegistrationChanged", resourceCulture);

	public static string AuditIdentityProviderRegistrationSuccess => ResourceManager.GetString("AuditIdentityProviderRegistrationSuccess", resourceCulture);

	public static string AuditLiveCameraDenied => ResourceManager.GetString("AuditLiveCameraDenied", resourceCulture);

	public static string AuditLiveCameraDeniedTimeBased => ResourceManager.GetString("AuditLiveCameraDeniedTimeBased", resourceCulture);

	public static string AuditLiveCameraEnded => ResourceManager.GetString("AuditLiveCameraEnded", resourceCulture);

	public static string AuditLiveCameraStarted => ResourceManager.GetString("AuditLiveCameraStarted", resourceCulture);

	public static string AuditLiveMetadataDenied => ResourceManager.GetString("AuditLiveMetadataDenied", resourceCulture);

	public static string AuditLiveMetadataDeniedTimeBased => ResourceManager.GetString("AuditLiveMetadataDeniedTimeBased", resourceCulture);

	public static string AuditLiveMetadataEnded => ResourceManager.GetString("AuditLiveMetadataEnded", resourceCulture);

	public static string AuditLiveMetadataStarted => ResourceManager.GetString("AuditLiveMetadataStarted", resourceCulture);

	public static string AuditLiveMicrophoneDenied => ResourceManager.GetString("AuditLiveMicrophoneDenied", resourceCulture);

	public static string AuditLiveMicrophoneDeniedTimeBased => ResourceManager.GetString("AuditLiveMicrophoneDeniedTimeBased", resourceCulture);

	public static string AuditLiveMicrophoneEnded => ResourceManager.GetString("AuditLiveMicrophoneEnded", resourceCulture);

	public static string AuditLiveMicrophoneStarted => ResourceManager.GetString("AuditLiveMicrophoneStarted", resourceCulture);

	public static string AuditLiveSpeakerDenied => ResourceManager.GetString("AuditLiveSpeakerDenied", resourceCulture);

	public static string AuditLiveSpeakerDeniedTimeBased => ResourceManager.GetString("AuditLiveSpeakerDeniedTimeBased", resourceCulture);

	public static string AuditLiveSpeakerEnded => ResourceManager.GetString("AuditLiveSpeakerEnded", resourceCulture);

	public static string AuditLiveSpeakerStarted => ResourceManager.GetString("AuditLiveSpeakerStarted", resourceCulture);

	public static string AuditLiveSpeakToSpeakerDenied => ResourceManager.GetString("AuditLiveSpeakToSpeakerDenied", resourceCulture);

	public static string AuditLiveSpeakToSpeakerDeniedTimeBased => ResourceManager.GetString("AuditLiveSpeakToSpeakerDeniedTimeBased", resourceCulture);

	public static string AuditLiveSpeakToSpeakerEnded => ResourceManager.GetString("AuditLiveSpeakToSpeakerEnded", resourceCulture);

	public static string AuditLiveSpeakToSpeakerStarted => ResourceManager.GetString("AuditLiveSpeakToSpeakerStarted", resourceCulture);

	public static string AuditLoginCallingApplictionUnknown => ResourceManager.GetString("AuditLoginCallingApplictionUnknown", resourceCulture);

	public static string AuditLoginLogoutTimeout => ResourceManager.GetString("AuditLoginLogoutTimeout", resourceCulture);

	public static string AuditLoginProviderCreated => ResourceManager.GetString("AuditLoginProviderCreated", resourceCulture);

	public static string AuditLoginProviderDeleted => ResourceManager.GetString("AuditLoginProviderDeleted", resourceCulture);

	public static string AuditLoginProviderUpdated => ResourceManager.GetString("AuditLoginProviderUpdated", resourceCulture);

	public static string AuditLoginSuccess => ResourceManager.GetString("AuditLoginSuccess", resourceCulture);

	public static string AuditLoginSuccessFromAdministration => ResourceManager.GetString("AuditLoginSuccessFromAdministration", resourceCulture);

	public static string AuditLoginSuccessFromCommandApi => ResourceManager.GetString("AuditLoginSuccessFromCommandApi", resourceCulture);

	public static string AuditLoginSuccessFromExternalSource => ResourceManager.GetString("AuditLoginSuccessFromExternalSource", resourceCulture);

	public static string AuditLoginSuccessFromHttpRequest => ResourceManager.GetString("AuditLoginSuccessFromHttpRequest", resourceCulture);

	public static string AuditLoginTimeBased => ResourceManager.GetString("AuditLoginTimeBased", resourceCulture);

	public static string AuditLoginUnSuccessfulIncorrectPassword => ResourceManager.GetString("AuditLoginUnSuccessfulIncorrectPassword", resourceCulture);

	public static string AuditLoginUnSuccessfulUnknownUser => ResourceManager.GetString("AuditLoginUnSuccessfulUnknownUser", resourceCulture);

	public static string AuditLoginUserNotAdminPriviligesOnTheSystem => ResourceManager.GetString("AuditLoginUserNotAdminPriviligesOnTheSystem", resourceCulture);

	public static string AuditLoginUserNotAllowed => ResourceManager.GetString("AuditLoginUserNotAllowed", resourceCulture);

	public static string AuditLoginUserNotAllowedCommandApiAccess => ResourceManager.GetString("AuditLoginUserNotAllowedCommandApiAccess", resourceCulture);

	public static string AuditLoginUserNotAllowedExternalAccess => ResourceManager.GetString("AuditLoginUserNotAllowedExternalAccess", resourceCulture);

	public static string AuditLoginUserNotAllowedHttpRequestAccess => ResourceManager.GetString("AuditLoginUserNotAllowedHttpRequestAccess", resourceCulture);

	public static string AuditLoginUserNotRegisteredOnTheSystem => ResourceManager.GetString("AuditLoginUserNotRegisteredOnTheSystem", resourceCulture);

	public static string AuditLogout => ResourceManager.GetString("AuditLogout", resourceCulture);

	public static string AuditLogoutSuccessFromAdministration => ResourceManager.GetString("AuditLogoutSuccessFromAdministration", resourceCulture);

	public static string AuditLogoutTimeBased => ResourceManager.GetString("AuditLogoutTimeBased", resourceCulture);

	public static string AuditManualRecordingStarted => ResourceManager.GetString("AuditManualRecordingStarted", resourceCulture);

	public static string AuditManualRecordingStopped => ResourceManager.GetString("AuditManualRecordingStopped", resourceCulture);

	public static string AuditMapCreated => ResourceManager.GetString("AuditMapCreated", resourceCulture);

	public static string AuditMapDeleted => ResourceManager.GetString("AuditMapDeleted", resourceCulture);

	public static string AuditMapDeviceServerPlaced => ResourceManager.GetString("AuditMapDeviceServerPlaced", resourceCulture);

	public static string AuditMapDeviceServerRemoved => ResourceManager.GetString("AuditMapDeviceServerRemoved", resourceCulture);

	public static string AuditMapDisplayed => ResourceManager.GetString("AuditMapDisplayed", resourceCulture);

	public static string AuditMapHotZoneLinkCreated => ResourceManager.GetString("AuditMapHotZoneLinkCreated", resourceCulture);

	public static string AuditMapHotZoneLinkRemoved => ResourceManager.GetString("AuditMapHotZoneLinkRemoved", resourceCulture);

	public static string AuditMapImageReplaced => ResourceManager.GetString("AuditMapImageReplaced", resourceCulture);

	public static string AuditMapRenamed => ResourceManager.GetString("AuditMapRenamed", resourceCulture);

	public static string AuditMarkedDataCreated => ResourceManager.GetString("AuditMarkedDataCreated", resourceCulture);

	public static string AuditMarkedDataDeleted => ResourceManager.GetString("AuditMarkedDataDeleted", resourceCulture);

	public static string AuditMarkedDataUpdate => ResourceManager.GetString("AuditMarkedDataUpdate", resourceCulture);

	public static string AuditMediaRestrictionCreated => ResourceManager.GetString("AuditMediaRestrictionCreated", resourceCulture);

	public static string AuditMediaRestrictionDeleted => ResourceManager.GetString("AuditMediaRestrictionDeleted", resourceCulture);

	public static string AuditMediaRestrictionLiveDeleted => ResourceManager.GetString("AuditMediaRestrictionLiveDeleted", resourceCulture);

	public static string AuditMediaRestrictionLiveModeEntered => ResourceManager.GetString("AuditMediaRestrictionLiveModeEntered", resourceCulture);

	public static string AuditMediaRestrictionLiveModeExited => ResourceManager.GetString("AuditMediaRestrictionLiveModeExited", resourceCulture);

	public static string AuditMediaRestrictionLiveUpdated => ResourceManager.GetString("AuditMediaRestrictionLiveUpdated", resourceCulture);

	public static string AuditMediaRestrictionUpdated => ResourceManager.GetString("AuditMediaRestrictionUpdated", resourceCulture);

	public static string AuditMetadataSearchCategoryIsDisabled => ResourceManager.GetString("AuditMetadataSearchCategoryIsDisabled", resourceCulture);

	public static string AuditMetadataSearchCategoryIsEnabled => ResourceManager.GetString("AuditMetadataSearchCategoryIsEnabled", resourceCulture);

	public static string AuditMetadataSearchFilterIsDisabled => ResourceManager.GetString("AuditMetadataSearchFilterIsDisabled", resourceCulture);

	public static string AuditMetadataSearchFilterIsEnabled => ResourceManager.GetString("AuditMetadataSearchFilterIsEnabled", resourceCulture);

	public static string AuditOutputActivated => ResourceManager.GetString("AuditOutputActivated", resourceCulture);

	public static string AuditOutputActivatedTimeBased => ResourceManager.GetString("AuditOutputActivatedTimeBased", resourceCulture);

	public static string AuditPerfCountStateRuleCreated => ResourceManager.GetString("AuditPerfCountStateRuleCreated", resourceCulture);

	public static string AuditPerfCountStateRuleDeleted => ResourceManager.GetString("AuditPerfCountStateRuleDeleted", resourceCulture);

	public static string AuditPerfCountStateRuleUpdated => ResourceManager.GetString("AuditPerfCountStateRuleUpdated", resourceCulture);

	public static string AuditPlaybackCameraEdgeRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackCameraEdgeRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackCameraRecordings => ResourceManager.GetString("AuditPlaybackCameraRecordings", resourceCulture);

	public static string AuditPlaybackCameraRecordingsDenied => ResourceManager.GetString("AuditPlaybackCameraRecordingsDenied", resourceCulture);

	public static string AuditPlaybackCameraRecordingsDeniedTimeBased => ResourceManager.GetString("AuditPlaybackCameraRecordingsDeniedTimeBased", resourceCulture);

	public static string AuditPlaybackCameraRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackCameraRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackMetadataEdgeRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackMetadataEdgeRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackMetadataRecordings => ResourceManager.GetString("AuditPlaybackMetadataRecordings", resourceCulture);

	public static string AuditPlaybackMetadataRecordingsDenied => ResourceManager.GetString("AuditPlaybackMetadataRecordingsDenied", resourceCulture);

	public static string AuditPlaybackMetadataRecordingsDeniedTimeBased => ResourceManager.GetString("AuditPlaybackMetadataRecordingsDeniedTimeBased", resourceCulture);

	public static string AuditPlaybackMetadataRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackMetadataRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackMicrophoneEdgeRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackMicrophoneEdgeRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackMicrophoneRecordings => ResourceManager.GetString("AuditPlaybackMicrophoneRecordings", resourceCulture);

	public static string AuditPlaybackMicrophoneRecordingsDenied => ResourceManager.GetString("AuditPlaybackMicrophoneRecordingsDenied", resourceCulture);

	public static string AuditPlaybackMicrophoneRecordingsDeniedTimeBased => ResourceManager.GetString("AuditPlaybackMicrophoneRecordingsDeniedTimeBased", resourceCulture);

	public static string AuditPlaybackMicrophoneRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackMicrophoneRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackSpeakerEdgeRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackSpeakerEdgeRecordingsOneLogOnly", resourceCulture);

	public static string AuditPlaybackSpeakerRecordings => ResourceManager.GetString("AuditPlaybackSpeakerRecordings", resourceCulture);

	public static string AuditPlaybackSpeakerRecordingsDenied => ResourceManager.GetString("AuditPlaybackSpeakerRecordingsDenied", resourceCulture);

	public static string AuditPlaybackSpeakerRecordingsDeniedTimeBased => ResourceManager.GetString("AuditPlaybackSpeakerRecordingsDeniedTimeBased", resourceCulture);

	public static string AuditPlaybackSpeakerRecordingsOneLogOnly => ResourceManager.GetString("AuditPlaybackSpeakerRecordingsOneLogOnly", resourceCulture);

	public static string AuditPtzActivatePatrollingProfileDenied => ResourceManager.GetString("AuditPtzActivatePatrollingProfileDenied", resourceCulture);

	public static string AuditPtzActivatePatrollingProfileGranted => ResourceManager.GetString("AuditPtzActivatePatrollingProfileGranted", resourceCulture);

	public static string AuditPTZControlTimeBased => ResourceManager.GetString("AuditPTZControlTimeBased", resourceCulture);

	public static string AuditPTZMoveAbsolute => ResourceManager.GetString("AuditPTZMoveAbsolute", resourceCulture);

	public static string AuditPTZMoveCenterZoomAbsolutePositioned => ResourceManager.GetString("AuditPTZMoveCenterZoomAbsolutePositioned", resourceCulture);

	public static string AuditPTZMoveCenterZoomRelativePositioned => ResourceManager.GetString("AuditPTZMoveCenterZoomRelativePositioned", resourceCulture);

	public static string AuditPTZMoveHome => ResourceManager.GetString("AuditPTZMoveHome", resourceCulture);

	public static string AuditPTZMoveRelative => ResourceManager.GetString("AuditPTZMoveRelative", resourceCulture);

	public static string AuditPTZMoveStart => ResourceManager.GetString("AuditPTZMoveStart", resourceCulture);

	public static string AuditPTZMoveStop => ResourceManager.GetString("AuditPTZMoveStop", resourceCulture);

	public static string AuditPTZMoveToPresetPosition => ResourceManager.GetString("AuditPTZMoveToPresetPosition", resourceCulture);

	public static string AuditPTZMoveZoomButtonsActivated => ResourceManager.GetString("AuditPTZMoveZoomButtonsActivated", resourceCulture);

	public static string AuditPtzNewSessionDenied => ResourceManager.GetString("AuditPtzNewSessionDenied", resourceCulture);

	public static string AuditPtzNewSessionGranted => ResourceManager.GetString("AuditPtzNewSessionGranted", resourceCulture);

	public static string AuditPtzPatrollingProfileChanged => ResourceManager.GetString("AuditPtzPatrollingProfileChanged", resourceCulture);

	public static string AuditPtzPatrollingProfileCreated => ResourceManager.GetString("AuditPtzPatrollingProfileCreated", resourceCulture);

	public static string AuditPtzPatrollingProfileDeleted => ResourceManager.GetString("AuditPtzPatrollingProfileDeleted", resourceCulture);

	public static string AuditPtzPresetChanged => ResourceManager.GetString("AuditPtzPresetChanged", resourceCulture);

	public static string AuditPtzPresetCreated => ResourceManager.GetString("AuditPtzPresetCreated", resourceCulture);

	public static string AuditPtzPresetDeleted => ResourceManager.GetString("AuditPtzPresetDeleted", resourceCulture);

	public static string AuditPTZRectangleZoom => ResourceManager.GetString("AuditPTZRectangleZoom", resourceCulture);

	public static string AuditPtzReleaseDenied => ResourceManager.GetString("AuditPtzReleaseDenied", resourceCulture);

	public static string AuditPtzReleaseGranted => ResourceManager.GetString("AuditPtzReleaseGranted", resourceCulture);

	public static string AuditPtzReserveDenied => ResourceManager.GetString("AuditPtzReserveDenied", resourceCulture);

	public static string AuditPtzReserveGranted => ResourceManager.GetString("AuditPtzReserveGranted", resourceCulture);

	public static string AuditRecorderRemoved => ResourceManager.GetString("AuditRecorderRemoved", resourceCulture);

	public static string AuditRecorderRenamed => ResourceManager.GetString("AuditRecorderRenamed", resourceCulture);

	public static string AuditRecordingSequencesRequested => ResourceManager.GetString("AuditRecordingSequencesRequested", resourceCulture);

	public static string AuditRecordingStorageAddArchive => ResourceManager.GetString("AuditRecordingStorageAddArchive", resourceCulture);

	public static string AuditRecordingStorageArchiveMounted => ResourceManager.GetString("AuditRecordingStorageArchiveMounted", resourceCulture);

	public static string AuditRecordingStorageArchiveMoved => ResourceManager.GetString("AuditRecordingStorageArchiveMoved", resourceCulture);

	public static string AuditRecordingStorageArchiveUnmounted => ResourceManager.GetString("AuditRecordingStorageArchiveUnmounted", resourceCulture);

	public static string AuditRecordingStorageCreated => ResourceManager.GetString("AuditRecordingStorageCreated", resourceCulture);

	public static string AuditRecordingStorageDeleteArchive => ResourceManager.GetString("AuditRecordingStorageDeleteArchive", resourceCulture);

	public static string AuditRecordingStorageDeleted => ResourceManager.GetString("AuditRecordingStorageDeleted", resourceCulture);

	public static string AuditRecordingStorageEditArchive => ResourceManager.GetString("AuditRecordingStorageEditArchive", resourceCulture);

	public static string AuditRecordingStorageEdited => ResourceManager.GetString("AuditRecordingStorageEdited", resourceCulture);

	public static string AuditRecordingStorageMoved => ResourceManager.GetString("AuditRecordingStorageMoved", resourceCulture);

	public static string AuditRetentionOptionRoleRelationUpdated => ResourceManager.GetString("AuditRetentionOptionRoleRelationUpdated", resourceCulture);

	public static string AuditRoleAnonymityDuringPtzSessionsGranted => ResourceManager.GetString("AuditRoleAnonymityDuringPtzSessionsGranted", resourceCulture);

	public static string AuditRoleAnonymityDuringPtzSessionsRevoked => ResourceManager.GetString("AuditRoleAnonymityDuringPtzSessionsRevoked", resourceCulture);

	public static string AuditRoleClaimAdded => ResourceManager.GetString("AuditRoleClaimAdded", resourceCulture);

	public static string AuditRoleClaimRemoved => ResourceManager.GetString("AuditRoleClaimRemoved", resourceCulture);

	public static string AuditRoleClientLoginGranted => ResourceManager.GetString("AuditRoleClientLoginGranted", resourceCulture);

	public static string AuditRoleClientLoginRevoked => ResourceManager.GetString("AuditRoleClientLoginRevoked", resourceCulture);

	public static string AuditRoleClientProfileChanged => ResourceManager.GetString("AuditRoleClientProfileChanged", resourceCulture);

	public static string AuditRoleCreated => ResourceManager.GetString("AuditRoleCreated", resourceCulture);

	public static string AuditRoleDefaultTimeProfileChanged => ResourceManager.GetString("AuditRoleDefaultTimeProfileChanged", resourceCulture);

	public static string AuditRoleDeleted => ResourceManager.GetString("AuditRoleDeleted", resourceCulture);

	public static string AuditRoleEdited => ResourceManager.GetString("AuditRoleEdited", resourceCulture);

	public static string AuditRoleGroupAdded => ResourceManager.GetString("AuditRoleGroupAdded", resourceCulture);

	public static string AuditRoleGroupRemoved => ResourceManager.GetString("AuditRoleGroupRemoved", resourceCulture);

	public static string AuditRoleLoginAuthorizationRequiredGranted => ResourceManager.GetString("AuditRoleLoginAuthorizationRequiredGranted", resourceCulture);

	public static string AuditRoleLoginAuthorizationRequiredRevoked => ResourceManager.GetString("AuditRoleLoginAuthorizationRequiredRevoked", resourceCulture);

	public static string AuditRoleObjectSecurityChangeAccessDenied => ResourceManager.GetString("AuditRoleObjectSecurityChangeAccessDenied", resourceCulture);

	public static string AuditRoleObjectSecurityGranted => ResourceManager.GetString("AuditRoleObjectSecurityGranted", resourceCulture);

	public static string AuditRoleObjectSecurityPriorityChanged => ResourceManager.GetString("AuditRoleObjectSecurityPriorityChanged", resourceCulture);

	public static string AuditRoleObjectSecurityPriorityGranted => ResourceManager.GetString("AuditRoleObjectSecurityPriorityGranted", resourceCulture);

	public static string AuditRoleObjectSecurityPriorityRevoked => ResourceManager.GetString("AuditRoleObjectSecurityPriorityRevoked", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedNoRestrictionToDays", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedNoRestrictionToMinutes", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToDays", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToMinutes", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionDaysToNoRestriction", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToDays", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToMinutes", resourceCulture);

	public static string AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction => ResourceManager.GetString("AuditRoleObjectSecurityRestrictedChangedRestrictionMinutesToNoRestriction", resourceCulture);

	public static string AuditRoleObjectSecurityRevoked => ResourceManager.GetString("AuditRoleObjectSecurityRevoked", resourceCulture);

	public static string AuditRoleObjectSecuritySystemTimeProfileToProfileChange => ResourceManager.GetString("AuditRoleObjectSecuritySystemTimeProfileToProfileChange", resourceCulture);

	public static string AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange => ResourceManager.GetString("AuditRoleObjectSecuritySystemTimeProfileToSystemProfileChange", resourceCulture);

	public static string AuditRoleObjectSecurityTimeProfileToProfileChange => ResourceManager.GetString("AuditRoleObjectSecurityTimeProfileToProfileChange", resourceCulture);

	public static string AuditRoleObjectSecurityTimeProfileToSystemProfileChange => ResourceManager.GetString("AuditRoleObjectSecurityTimeProfileToSystemProfileChange", resourceCulture);

	public static string AuditRoleOnlyAllowLoginWithinTimeProfileChanged => ResourceManager.GetString("AuditRoleOnlyAllowLoginWithinTimeProfileChanged", resourceCulture);

	public static string AuditRoleOverallSecurityChangeAccessDenied => ResourceManager.GetString("AuditRoleOverallSecurityChangeAccessDenied", resourceCulture);

	public static string AuditRoleOverallSecurityChanged => ResourceManager.GetString("AuditRoleOverallSecurityChanged", resourceCulture);

	public static string AuditRoleOverallSecurityDenied => ResourceManager.GetString("AuditRoleOverallSecurityDenied", resourceCulture);

	public static string AuditRoleOverallSecurityGranted => ResourceManager.GetString("AuditRoleOverallSecurityGranted", resourceCulture);

	public static string AuditRoleOverallSecurityRevoked => ResourceManager.GetString("AuditRoleOverallSecurityRevoked", resourceCulture);

	public static string AuditRoleOverallSecurityRevokedDenied => ResourceManager.GetString("AuditRoleOverallSecurityRevokedDenied", resourceCulture);

	public static string AuditRoleRenamed => ResourceManager.GetString("AuditRoleRenamed", resourceCulture);

	public static string AuditRoleUserAdded => ResourceManager.GetString("AuditRoleUserAdded", resourceCulture);

	public static string AuditRoleUserRemoved => ResourceManager.GetString("AuditRoleUserRemoved", resourceCulture);

	public static string AuditRuleCreated => ResourceManager.GetString("AuditRuleCreated", resourceCulture);

	public static string AuditRuleDeleted => ResourceManager.GetString("AuditRuleDeleted", resourceCulture);

	public static string AuditRuleDisabled => ResourceManager.GetString("AuditRuleDisabled", resourceCulture);

	public static string AuditRuleEdited => ResourceManager.GetString("AuditRuleEdited", resourceCulture);

	public static string AuditRuleEnabled => ResourceManager.GetString("AuditRuleEnabled", resourceCulture);

	public static string AuditRuleRenamed => ResourceManager.GetString("AuditRuleRenamed", resourceCulture);

	public static string AuditSearchCreated => ResourceManager.GetString("AuditSearchCreated", resourceCulture);

	public static string AuditSearchDeleted => ResourceManager.GetString("AuditSearchDeleted", resourceCulture);

	public static string AuditSearchEdited => ResourceManager.GetString("AuditSearchEdited", resourceCulture);

	public static string AuditSearchRenamed => ResourceManager.GetString("AuditSearchRenamed", resourceCulture);

	public static string AuditSiteMasterAuthorized => ResourceManager.GetString("AuditSiteMasterAuthorized", resourceCulture);

	public static string AuditSiteMasterAuthorizedDenied => ResourceManager.GetString("AuditSiteMasterAuthorizedDenied", resourceCulture);

	public static string AuditSiteMasterDeleted => ResourceManager.GetString("AuditSiteMasterDeleted", resourceCulture);

	public static string AuditSiteRenamed => ResourceManager.GetString("AuditSiteRenamed", resourceCulture);

	public static string AuditSiteSlaveCreated => ResourceManager.GetString("AuditSiteSlaveCreated", resourceCulture);

	public static string AuditSiteSlaveDeleted => ResourceManager.GetString("AuditSiteSlaveDeleted", resourceCulture);

	public static string AuditSystemDashboardConfigurationCreated => ResourceManager.GetString("AuditSystemDashboardConfigurationCreated", resourceCulture);

	public static string AuditSystemDashboardConfigurationDeleted => ResourceManager.GetString("AuditSystemDashboardConfigurationDeleted", resourceCulture);

	public static string AuditSystemDashboardConfigurationUpdated => ResourceManager.GetString("AuditSystemDashboardConfigurationUpdated", resourceCulture);

	public static string AuditTimeProfileCreated => ResourceManager.GetString("AuditTimeProfileCreated", resourceCulture);

	public static string AuditTimeProfileDeleted => ResourceManager.GetString("AuditTimeProfileDeleted", resourceCulture);

	public static string AuditTimeProfileEdited => ResourceManager.GetString("AuditTimeProfileEdited", resourceCulture);

	public static string AuditTimeProfileRenamed => ResourceManager.GetString("AuditTimeProfileRenamed", resourceCulture);

	public static string AuditTrustedIssuerCreated => ResourceManager.GetString("AuditTrustedIssuerCreated", resourceCulture);

	public static string AuditTrustedIssuerDeleted => ResourceManager.GetString("AuditTrustedIssuerDeleted", resourceCulture);

	public static string AuditTrustedIssuerUpdated => ResourceManager.GetString("AuditTrustedIssuerUpdated", resourceCulture);

	public static string AuditUserDefinedEventActivated => ResourceManager.GetString("AuditUserDefinedEventActivated", resourceCulture);

	public static string AuditUserDefinedEventActivatedTimeBased => ResourceManager.GetString("AuditUserDefinedEventActivatedTimeBased", resourceCulture);

	public static string AuditUserLoggedIn => ResourceManager.GetString("AuditUserLoggedIn", resourceCulture);

	public static string AuditUserLoggedOut => ResourceManager.GetString("AuditUserLoggedOut", resourceCulture);

	public static string AuditUserLoginFailed => ResourceManager.GetString("AuditUserLoginFailed", resourceCulture);

	public static string AuditUserLoginFailedLockedOutByAdmin => ResourceManager.GetString("AuditUserLoginFailedLockedOutByAdmin", resourceCulture);

	public static string AuditUserLoginFailedPasswordChange => ResourceManager.GetString("AuditUserLoginFailedPasswordChange", resourceCulture);

	public static string AuditUserLoginFailedUnknownUser => ResourceManager.GetString("AuditUserLoginFailedUnknownUser", resourceCulture);

	public static string AuditUserTriggeredEvent => ResourceManager.GetString("AuditUserTriggeredEvent", resourceCulture);

	public static string AuditVideoWallCreated => ResourceManager.GetString("AuditVideoWallCreated", resourceCulture);

	public static string AuditVideoWallDeleted => ResourceManager.GetString("AuditVideoWallDeleted", resourceCulture);

	public static string AuditVideoWallEdited => ResourceManager.GetString("AuditVideoWallEdited", resourceCulture);

	public static string AuditVideoWallMonitorAdded => ResourceManager.GetString("AuditVideoWallMonitorAdded", resourceCulture);

	public static string AuditVideoWallMonitorCameraRemoved => ResourceManager.GetString("AuditVideoWallMonitorCameraRemoved", resourceCulture);

	public static string AuditVideoWallMonitorDeleted => ResourceManager.GetString("AuditVideoWallMonitorDeleted", resourceCulture);

	public static string AuditVideoWallMonitorEdited => ResourceManager.GetString("AuditVideoWallMonitorEdited", resourceCulture);

	public static string AuditVideoWallMonitorLayoutCameraChanged => ResourceManager.GetString("AuditVideoWallMonitorLayoutCameraChanged", resourceCulture);

	public static string AuditVideoWallMonitorLayoutChanged => ResourceManager.GetString("AuditVideoWallMonitorLayoutChanged", resourceCulture);

	public static string AuditVideoWallMonitorLayoutCleared => ResourceManager.GetString("AuditVideoWallMonitorLayoutCleared", resourceCulture);

	public static string AuditVideoWallMonitorLayoutMonitorApplied => ResourceManager.GetString("AuditVideoWallMonitorLayoutMonitorApplied", resourceCulture);

	public static string AuditVideoWallMonitorLayoutMonitorAppliedTimeBased => ResourceManager.GetString("AuditVideoWallMonitorLayoutMonitorAppliedTimeBased", resourceCulture);

	public static string AuditVideoWallMonitorLayoutPositionCleared => ResourceManager.GetString("AuditVideoWallMonitorLayoutPositionCleared", resourceCulture);

	public static string AuditVideoWallMonitorLayoutPresetActivated => ResourceManager.GetString("AuditVideoWallMonitorLayoutPresetActivated", resourceCulture);

	public static string AuditVideoWallMonitorLayoutViewApplied => ResourceManager.GetString("AuditVideoWallMonitorLayoutViewApplied", resourceCulture);

	public static string AuditVideoWallMonitorPresetEdited => ResourceManager.GetString("AuditVideoWallMonitorPresetEdited", resourceCulture);

	public static string AuditVideoWallMonitorRenamed => ResourceManager.GetString("AuditVideoWallMonitorRenamed", resourceCulture);

	public static string AuditVideoWallPresetActivate => ResourceManager.GetString("AuditVideoWallPresetActivate", resourceCulture);

	public static string AuditVideoWallPresetActivateTimeBased => ResourceManager.GetString("AuditVideoWallPresetActivateTimeBased", resourceCulture);

	public static string AuditVideoWallPresetAdded => ResourceManager.GetString("AuditVideoWallPresetAdded", resourceCulture);

	public static string AuditVideoWallPresetDeleted => ResourceManager.GetString("AuditVideoWallPresetDeleted", resourceCulture);

	public static string AuditVideoWallPresetRenamed => ResourceManager.GetString("AuditVideoWallPresetRenamed", resourceCulture);

	public static string AuditVideoWallRenamed => ResourceManager.GetString("AuditVideoWallRenamed", resourceCulture);

	public static string AuditVideoWallTextViewItemCreated => ResourceManager.GetString("AuditVideoWallTextViewItemCreated", resourceCulture);

	public static string AutomaticActivationFailed => ResourceManager.GetString("AutomaticActivationFailed", resourceCulture);

	public static string AutomaticActivationSuccess => ResourceManager.GetString("AutomaticActivationSuccess", resourceCulture);

	public static string AutoProvisionedUserCreated => ResourceManager.GetString("AutoProvisionedUserCreated", resourceCulture);

	public static string AutoProvisionedUserUpdated => ResourceManager.GetString("AutoProvisionedUserUpdated", resourceCulture);

	public static string CodecDoesNotSupportThisMedia => ResourceManager.GetString("CodecDoesNotSupportThisMedia", resourceCulture);

	public static string DatabaseConnectionLost => ResourceManager.GetString("DatabaseConnectionLost", resourceCulture);

	public static string DatabaseConnectionRestored => ResourceManager.GetString("DatabaseConnectionRestored", resourceCulture);

	public static string DeviceClearOverflowMode => ResourceManager.GetString("DeviceClearOverflowMode", resourceCulture);

	public static string DeviceCommunicationError => ResourceManager.GetString("DeviceCommunicationError", resourceCulture);

	public static string DeviceCommunicationEstablished => ResourceManager.GetString("DeviceCommunicationEstablished", resourceCulture);

	public static string DeviceCommunicationStopped => ResourceManager.GetString("DeviceCommunicationStopped", resourceCulture);

	public static string DeviceDriverLoadFailed => ResourceManager.GetString("DeviceDriverLoadFailed", resourceCulture);

	public static string DeviceDriverProxyFailure => ResourceManager.GetString("DeviceDriverProxyFailure", resourceCulture);

	public static string DeviceEmptyTableBegin => ResourceManager.GetString("DeviceEmptyTableBegin", resourceCulture);

	public static string DeviceEmptyTableFailedLastRetry => ResourceManager.GetString("DeviceEmptyTableFailedLastRetry", resourceCulture);

	public static string DeviceEmptyTableFailedNoRetry => ResourceManager.GetString("DeviceEmptyTableFailedNoRetry", resourceCulture);

	public static string DeviceEmptyTableFailedRetry => ResourceManager.GetString("DeviceEmptyTableFailedRetry", resourceCulture);

	public static string DeviceEmptyTableSuccess => ResourceManager.GetString("DeviceEmptyTableSuccess", resourceCulture);

	public static string DeviceFirmwareUpdateFailed => ResourceManager.GetString("DeviceFirmwareUpdateFailed", resourceCulture);

	public static string DeviceFirmwareVersionUpdated => ResourceManager.GetString("DeviceFirmwareVersionUpdated", resourceCulture);

	public static string DeviceSetOverflowMode => ResourceManager.GetString("DeviceSetOverflowMode", resourceCulture);

	public static string DeviceSettingsChanged => ResourceManager.GetString("DeviceSettingsChanged", resourceCulture);

	public static string DeviceSettingsChangedError => ResourceManager.GetString("DeviceSettingsChangedError", resourceCulture);

	public static string EdgeStorageRetrievalBegin => ResourceManager.GetString("EdgeStorageRetrievalBegin", resourceCulture);

	public static string EdgeStorageRetrievalFailed => ResourceManager.GetString("EdgeStorageRetrievalFailed", resourceCulture);

	public static string EdgeStorageRetrievalStopped => ResourceManager.GetString("EdgeStorageRetrievalStopped", resourceCulture);

	public static string EdgeStorageRetrievalSuccess => ResourceManager.GetString("EdgeStorageRetrievalSuccess", resourceCulture);

	public static string EMailError => ResourceManager.GetString("EMailError", resourceCulture);

	public static string EMailErrorRecorderUnavailable => ResourceManager.GetString("EMailErrorRecorderUnavailable", resourceCulture);

	public static string Enum_AccessGranted_Default => ResourceManager.GetString("Enum_AccessGranted_Default", resourceCulture);

	public static string Enum_AccessGranted_Denied => ResourceManager.GetString("Enum_AccessGranted_Denied", resourceCulture);

	public static string Enum_AccessGranted_Granted => ResourceManager.GetString("Enum_AccessGranted_Granted", resourceCulture);

	public static string Enum_AuditType_Bookmark => ResourceManager.GetString("Enum_AuditType_Bookmark", resourceCulture);

	public static string Enum_AuditType_DatabaseAccess => ResourceManager.GetString("Enum_AuditType_DatabaseAccess", resourceCulture);

	public static string Enum_AuditType_Export => ResourceManager.GetString("Enum_AuditType_Export", resourceCulture);

	public static string Enum_AuditType_IOAndEvents => ResourceManager.GetString("Enum_AuditType_IOAndEvents", resourceCulture);

	public static string Enum_AuditType_LiveAudio => ResourceManager.GetString("Enum_AuditType_LiveAudio", resourceCulture);

	public static string Enum_AuditType_LiveMetadata => ResourceManager.GetString("Enum_AuditType_LiveMetadata", resourceCulture);

	public static string Enum_AuditType_LiveVideo => ResourceManager.GetString("Enum_AuditType_LiveVideo", resourceCulture);

	public static string Enum_AuditType_Login => ResourceManager.GetString("Enum_AuditType_Login", resourceCulture);

	public static string Enum_AuditType_Logout => ResourceManager.GetString("Enum_AuditType_Logout", resourceCulture);

	public static string Enum_AuditType_Management => ResourceManager.GetString("Enum_AuditType_Management", resourceCulture);

	public static string Enum_AuditType_ManualRecording => ResourceManager.GetString("Enum_AuditType_ManualRecording", resourceCulture);

	public static string Enum_AuditType_MapViewing => ResourceManager.GetString("Enum_AuditType_MapViewing", resourceCulture);

	public static string Enum_AuditType_MetadataSearch => ResourceManager.GetString("Enum_AuditType_MetadataSearch", resourceCulture);

	public static string Enum_AuditType_PlaybackAudio => ResourceManager.GetString("Enum_AuditType_PlaybackAudio", resourceCulture);

	public static string Enum_AuditType_PlaybackMetadata => ResourceManager.GetString("Enum_AuditType_PlaybackMetadata", resourceCulture);

	public static string Enum_AuditType_PlaybackVideo => ResourceManager.GetString("Enum_AuditType_PlaybackVideo", resourceCulture);

	public static string Enum_AuditType_Ptz => ResourceManager.GetString("Enum_AuditType_Ptz", resourceCulture);

	public static string Enum_AuditType_RecordingSequences => ResourceManager.GetString("Enum_AuditType_RecordingSequences", resourceCulture);

	public static string Enum_AuditType_ResourceAccess => ResourceManager.GetString("Enum_AuditType_ResourceAccess", resourceCulture);

	public static string Enum_AuditType_SaveSearches => ResourceManager.GetString("Enum_AuditType_SaveSearches", resourceCulture);

	public static string Enum_AuditType_Security => ResourceManager.GetString("Enum_AuditType_Security", resourceCulture);

	public static string Enum_AuditType_SmartWallControl => ResourceManager.GetString("Enum_AuditType_SmartWallControl", resourceCulture);

	public static string Enum_AuditType_Unknown => ResourceManager.GetString("Enum_AuditType_Unknown", resourceCulture);

	public static string Enum_Category_Bookmark => ResourceManager.GetString("Enum_Category_Bookmark", resourceCulture);

	public static string Enum_Category_EdgeStorage => ResourceManager.GetString("Enum_Category_EdgeStorage", resourceCulture);

	public static string Enum_Category_Export => ResourceManager.GetString("Enum_Category_Export", resourceCulture);

	public static string Enum_Category_ExternalComponents => ResourceManager.GetString("Enum_Category_ExternalComponents", resourceCulture);

	public static string Enum_Category_ExternalIDP => ResourceManager.GetString("Enum_Category_ExternalIDP", resourceCulture);

	public static string Enum_Category_Failover => ResourceManager.GetString("Enum_Category_Failover", resourceCulture);

	public static string Enum_Category_HardwareAndDevices => ResourceManager.GetString("Enum_Category_HardwareAndDevices", resourceCulture);

	public static string Enum_Category_IOAndEvents => ResourceManager.GetString("Enum_Category_IOAndEvents", resourceCulture);

	public static string Enum_Category_License => ResourceManager.GetString("Enum_Category_License", resourceCulture);

	public static string Enum_Category_Live => ResourceManager.GetString("Enum_Category_Live", resourceCulture);

	public static string Enum_Category_Management => ResourceManager.GetString("Enum_Category_Management", resourceCulture);

	public static string Enum_Category_ManualRecording => ResourceManager.GetString("Enum_Category_ManualRecording", resourceCulture);

	public static string Enum_Category_MapViewing => ResourceManager.GetString("Enum_Category_MapViewing", resourceCulture);

	public static string Enum_Category_MediaStorage => ResourceManager.GetString("Enum_Category_MediaStorage", resourceCulture);

	public static string Enum_Category_MetadataSearch => ResourceManager.GetString("Enum_Category_MetadataSearch", resourceCulture);

	public static string Enum_Category_Notification => ResourceManager.GetString("Enum_Category_Notification", resourceCulture);

	public static string Enum_Category_Playback => ResourceManager.GetString("Enum_Category_Playback", resourceCulture);

	public static string Enum_Category_Plugins => ResourceManager.GetString("Enum_Category_Plugins", resourceCulture);

	public static string Enum_Category_PTZ => ResourceManager.GetString("Enum_Category_PTZ", resourceCulture);

	public static string Enum_Category_RecorderCommunicationAndManagement => ResourceManager.GetString("Enum_Category_RecorderCommunicationAndManagement", resourceCulture);

	public static string Enum_Category_SaveSearches => ResourceManager.GetString("Enum_Category_SaveSearches", resourceCulture);

	public static string Enum_Category_Security => ResourceManager.GetString("Enum_Category_Security", resourceCulture);

	public static string Enum_Category_Shutdown => ResourceManager.GetString("Enum_Category_Shutdown", resourceCulture);

	public static string Enum_Category_SmartWallControl => ResourceManager.GetString("Enum_Category_SmartWallControl", resourceCulture);

	public static string Enum_Category_SqlServerConnectivity => ResourceManager.GetString("Enum_Category_SqlServerConnectivity", resourceCulture);

	public static string Enum_Category_Startup => ResourceManager.GetString("Enum_Category_Startup", resourceCulture);

	public static string Enum_Category_Unknown => ResourceManager.GetString("Enum_Category_Unknown", resourceCulture);

	public static string Enum_Category_Webserver => ResourceManager.GetString("Enum_Category_Webserver", resourceCulture);

	public static string Enum_EntrySeverity_Error => ResourceManager.GetString("Enum_EntrySeverity_Error", resourceCulture);

	public static string Enum_EntrySeverity_Info => ResourceManager.GetString("Enum_EntrySeverity_Info", resourceCulture);

	public static string Enum_EntrySeverity_Undefined => ResourceManager.GetString("Enum_EntrySeverity_Undefined", resourceCulture);

	public static string Enum_EntrySeverity_Warning => ResourceManager.GetString("Enum_EntrySeverity_Warning", resourceCulture);

	public static string Enum_EntryStatus_Failure => ResourceManager.GetString("Enum_EntryStatus_Failure", resourceCulture);

	public static string Enum_EntryStatus_StatusQuo => ResourceManager.GetString("Enum_EntryStatus_StatusQuo", resourceCulture);

	public static string Enum_EntryStatus_Success => ResourceManager.GetString("Enum_EntryStatus_Success", resourceCulture);

	public static string Enum_EventGeneratorType_Device => ResourceManager.GetString("Enum_EventGeneratorType_Device", resourceCulture);

	public static string Enum_EventGeneratorType_DriverDevice => ResourceManager.GetString("Enum_EventGeneratorType_DriverDevice", resourceCulture);

	public static string Enum_EventGeneratorType_DriverHardware => ResourceManager.GetString("Enum_EventGeneratorType_DriverHardware", resourceCulture);

	public static string Enum_EventGeneratorType_External => ResourceManager.GetString("Enum_EventGeneratorType_External", resourceCulture);

	public static string Enum_EventGeneratorType_Hardware => ResourceManager.GetString("Enum_EventGeneratorType_Hardware", resourceCulture);

	public static string Enum_EventGeneratorType_MIP => ResourceManager.GetString("Enum_EventGeneratorType_MIP", resourceCulture);

	public static string Enum_EventGeneratorType_MIPDevice => ResourceManager.GetString("Enum_EventGeneratorType_MIPDevice", resourceCulture);

	public static string Enum_EventGeneratorType_Recorder => ResourceManager.GetString("Enum_EventGeneratorType_Recorder", resourceCulture);

	public static string Enum_EventGeneratorType_RecorderPlugin => ResourceManager.GetString("Enum_EventGeneratorType_RecorderPlugin", resourceCulture);

	public static string Enum_EventGeneratorType_Server => ResourceManager.GetString("Enum_EventGeneratorType_Server", resourceCulture);

	public static string Enum_EventGeneratorType_System => ResourceManager.GetString("Enum_EventGeneratorType_System", resourceCulture);

	public static string Enum_EventGeneratorType_SystemDashboard => ResourceManager.GetString("Enum_EventGeneratorType_SystemDashboard", resourceCulture);

	public static string Enum_EventGeneratorType_Timer => ResourceManager.GetString("Enum_EventGeneratorType_Timer", resourceCulture);

	public static string Enum_EventGeneratorType_Unknown => ResourceManager.GetString("Enum_EventGeneratorType_Unknown", resourceCulture);

	public static string Enum_ResourceType_AudioMessage => ResourceManager.GetString("Enum_ResourceType_AudioMessage", resourceCulture);

	public static string Enum_ResourceType_BasicUser => ResourceManager.GetString("Enum_ResourceType_BasicUser", resourceCulture);

	public static string Enum_ResourceType_Camera => ResourceManager.GetString("Enum_ResourceType_Camera", resourceCulture);

	public static string Enum_ResourceType_ClientProfiles => ResourceManager.GetString("Enum_ResourceType_ClientProfiles", resourceCulture);

	public static string Enum_ResourceType_FederatedArchitecture => ResourceManager.GetString("Enum_ResourceType_FederatedArchitecture", resourceCulture);

	public static string Enum_ResourceType_GenericEvents => ResourceManager.GetString("Enum_ResourceType_GenericEvents", resourceCulture);

	public static string Enum_ResourceType_Hardware => ResourceManager.GetString("Enum_ResourceType_Hardware", resourceCulture);

	public static string Enum_ResourceType_IdentityProvider => ResourceManager.GetString("Enum_ResourceType_IdentityProvider", resourceCulture);

	public static string Enum_ResourceType_Input => ResourceManager.GetString("Enum_ResourceType_Input", resourceCulture);

	public static string Enum_ResourceType_MailNotification => ResourceManager.GetString("Enum_ResourceType_MailNotification", resourceCulture);

	public static string Enum_ResourceType_ManagementServer => ResourceManager.GetString("Enum_ResourceType_ManagementServer", resourceCulture);

	public static string Enum_ResourceType_Map => ResourceManager.GetString("Enum_ResourceType_Map", resourceCulture);

	public static string Enum_ResourceType_MarkedData => ResourceManager.GetString("Enum_ResourceType_MarkedData", resourceCulture);

	public static string Enum_ResourceType_Matrix => ResourceManager.GetString("Enum_ResourceType_Matrix", resourceCulture);

	public static string Enum_ResourceType_MediaRestriction => ResourceManager.GetString("Enum_ResourceType_MediaRestriction", resourceCulture);

	public static string Enum_ResourceType_Metadata => ResourceManager.GetString("Enum_ResourceType_Metadata", resourceCulture);

	public static string Enum_ResourceType_MetadataSearch => ResourceManager.GetString("Enum_ResourceType_MetadataSearch", resourceCulture);

	public static string Enum_ResourceType_Microphone => ResourceManager.GetString("Enum_ResourceType_Microphone", resourceCulture);

	public static string Enum_ResourceType_Output => ResourceManager.GetString("Enum_ResourceType_Output", resourceCulture);

	public static string Enum_ResourceType_PerformanceCounterRule => ResourceManager.GetString("Enum_ResourceType_PerformanceCounterRule", resourceCulture);

	public static string Enum_ResourceType_RecordingServers => ResourceManager.GetString("Enum_ResourceType_RecordingServers", resourceCulture);

	public static string Enum_ResourceType_RemoteConnectHardware => ResourceManager.GetString("Enum_ResourceType_RemoteConnectHardware", resourceCulture);

	public static string Enum_ResourceType_RemoteConnectServer => ResourceManager.GetString("Enum_ResourceType_RemoteConnectServer", resourceCulture);

	public static string Enum_ResourceType_RetentionOptionRoleRelation => ResourceManager.GetString("Enum_ResourceType_RetentionOptionRoleRelation", resourceCulture);

	public static string Enum_ResourceType_Roles => ResourceManager.GetString("Enum_ResourceType_Roles", resourceCulture);

	public static string Enum_ResourceType_Rules => ResourceManager.GetString("Enum_ResourceType_Rules", resourceCulture);

	public static string Enum_ResourceType_SaveSearches => ResourceManager.GetString("Enum_ResourceType_SaveSearches", resourceCulture);

	public static string Enum_ResourceType_Server => ResourceManager.GetString("Enum_ResourceType_Server", resourceCulture);

	public static string Enum_ResourceType_ServiceRegistration => ResourceManager.GetString("Enum_ResourceType_ServiceRegistration", resourceCulture);

	public static string Enum_ResourceType_SiteInfo => ResourceManager.GetString("Enum_ResourceType_SiteInfo", resourceCulture);

	public static string Enum_ResourceType_SmartWall => ResourceManager.GetString("Enum_ResourceType_SmartWall", resourceCulture);

	public static string Enum_ResourceType_SmartWallMonitor => ResourceManager.GetString("Enum_ResourceType_SmartWallMonitor", resourceCulture);

	public static string Enum_ResourceType_SmartWallPreset => ResourceManager.GetString("Enum_ResourceType_SmartWallPreset", resourceCulture);

	public static string Enum_ResourceType_Speaker => ResourceManager.GetString("Enum_ResourceType_Speaker", resourceCulture);

	public static string Enum_ResourceType_Storage => ResourceManager.GetString("Enum_ResourceType_Storage", resourceCulture);

	public static string Enum_ResourceType_SystemDashboardConfiguration => ResourceManager.GetString("Enum_ResourceType_SystemDashboardConfiguration", resourceCulture);

	public static string Enum_ResourceType_TimeProfile => ResourceManager.GetString("Enum_ResourceType_TimeProfile", resourceCulture);

	public static string Enum_ResourceType_TimeProfiles => ResourceManager.GetString("Enum_ResourceType_TimeProfiles", resourceCulture);

	public static string Enum_ResourceType_Unknown => ResourceManager.GetString("Enum_ResourceType_Unknown", resourceCulture);

	public static string Enum_ResourceType_UserDefinedEvents => ResourceManager.GetString("Enum_ResourceType_UserDefinedEvents", resourceCulture);

	public static string Enum_ServerLogType_Audit => ResourceManager.GetString("Enum_ServerLogType_Audit", resourceCulture);

	public static string Enum_SourceType_Database => ResourceManager.GetString("Enum_SourceType_Database", resourceCulture);

	public static string Enum_SourceType_Device => ResourceManager.GetString("Enum_SourceType_Device", resourceCulture);

	public static string Enum_SourceType_External => ResourceManager.GetString("Enum_SourceType_External", resourceCulture);

	public static string Enum_SourceType_Failover => ResourceManager.GetString("Enum_SourceType_Failover", resourceCulture);

	public static string Enum_SourceType_Hardware => ResourceManager.GetString("Enum_SourceType_Hardware", resourceCulture);

	public static string Enum_SourceType_IdentityProvider => ResourceManager.GetString("Enum_SourceType_IdentityProvider", resourceCulture);

	public static string Enum_SourceType_Recorder => ResourceManager.GetString("Enum_SourceType_Recorder", resourceCulture);

	public static string Enum_SourceType_Role => ResourceManager.GetString("Enum_SourceType_Role", resourceCulture);

	public static string Enum_SourceType_Server => ResourceManager.GetString("Enum_SourceType_Server", resourceCulture);

	public static string Enum_SourceType_System => ResourceManager.GetString("Enum_SourceType_System", resourceCulture);

	public static string Enum_SourceType_Unknown => ResourceManager.GetString("Enum_SourceType_Unknown", resourceCulture);

	public static string ErrorHardwareBlocked => ResourceManager.GetString("ErrorHardwareBlocked", resourceCulture);

	public static string ErrorHardwareNotEnabled => ResourceManager.GetString("ErrorHardwareNotEnabled", resourceCulture);

	public static string ErrorHardwareNotStarted => ResourceManager.GetString("ErrorHardwareNotStarted", resourceCulture);

	public static string ErrorHardwareReadOnly => ResourceManager.GetString("ErrorHardwareReadOnly", resourceCulture);

	public static string ErrorNoHardwareAccess => ResourceManager.GetString("ErrorNoHardwareAccess", resourceCulture);

	public static string ExternalPlugIn360LensDefinitionsAdded => ResourceManager.GetString("ExternalPlugIn360LensDefinitionsAdded", resourceCulture);

	public static string ExternalPlugIn360LensDefinitionsChanged => ResourceManager.GetString("ExternalPlugIn360LensDefinitionsChanged", resourceCulture);

	public static string ExternalPlugIn360LensDefinitionsRemoved => ResourceManager.GetString("ExternalPlugIn360LensDefinitionsRemoved", resourceCulture);

	public static string FailedToDistributeGlobalizedEvents => ResourceManager.GetString("FailedToDistributeGlobalizedEvents", resourceCulture);

	public static string FailedToDistributeRuleActions => ResourceManager.GetString("FailedToDistributeRuleActions", resourceCulture);

	public static string FailoverServerAuthorizationFailedWithLocalRecorder => ResourceManager.GetString("FailoverServerAuthorizationFailedWithLocalRecorder", resourceCulture);

	public static string FailoverServerAuthorizationFailedWithRecorder => ResourceManager.GetString("FailoverServerAuthorizationFailedWithRecorder", resourceCulture);

	public static string FailoverServerRecorderFailedToStart => ResourceManager.GetString("FailoverServerRecorderFailedToStart", resourceCulture);

	public static string FailoverServerRemoveFailoverServer => ResourceManager.GetString("FailoverServerRemoveFailoverServer", resourceCulture);

	public static string FailoverServerServiceHasReturnedFromFailoverState => ResourceManager.GetString("FailoverServerServiceHasReturnedFromFailoverState", resourceCulture);

	public static string FailoverServerStartingHotStandby => ResourceManager.GetString("FailoverServerStartingHotStandby", resourceCulture);

	public static string FailoverServerStoppingHotStandby => ResourceManager.GetString("FailoverServerStoppingHotStandby", resourceCulture);

	public static string FailoverServerTakingOverTheResponsibilityForRecorder => ResourceManager.GetString("FailoverServerTakingOverTheResponsibilityForRecorder", resourceCulture);

	public static string FailoverServerUnableToConnectEncryptedToLocalRecorder => ResourceManager.GetString("FailoverServerUnableToConnectEncryptedToLocalRecorder", resourceCulture);

	public static string FailoverServerUnableToConnectEncryptedToRecorder => ResourceManager.GetString("FailoverServerUnableToConnectEncryptedToRecorder", resourceCulture);

	public static string FailoverServerUnableToUpdateTheRecorderGuid => ResourceManager.GetString("FailoverServerUnableToUpdateTheRecorderGuid", resourceCulture);

	public static string Field_AccessGranted => ResourceManager.GetString("Field_AccessGranted", resourceCulture);

	public static string Field_AuditType => ResourceManager.GetString("Field_AuditType", resourceCulture);

	public static string Field_Description => ResourceManager.GetString("Field_Description", resourceCulture);

	public static string Field_EventType => ResourceManager.GetString("Field_EventType", resourceCulture);

	public static string Field_EventTypeId => ResourceManager.GetString("Field_EventTypeId", resourceCulture);

	public static string Field_GeneratorId => ResourceManager.GetString("Field_GeneratorId", resourceCulture);

	public static string Field_GeneratorName => ResourceManager.GetString("Field_GeneratorName", resourceCulture);

	public static string Field_GeneratorType => ResourceManager.GetString("Field_GeneratorType", resourceCulture);

	public static string Field_LocationIPAddress => ResourceManager.GetString("Field_LocationIPAddress", resourceCulture);

	public static string Field_LogEntryId => ResourceManager.GetString("Field_LogEntryId", resourceCulture);

	public static string Field_LogType => ResourceManager.GetString("Field_LogType", resourceCulture);

	public static string Field_ResourceId => ResourceManager.GetString("Field_ResourceId", resourceCulture);

	public static string Field_ResourceLocation => ResourceManager.GetString("Field_ResourceLocation", resourceCulture);

	public static string Field_ResourceName => ResourceManager.GetString("Field_ResourceName", resourceCulture);

	public static string Field_ResourceType => ResourceManager.GetString("Field_ResourceType", resourceCulture);

	public static string Field_RuleName => ResourceManager.GetString("Field_RuleName", resourceCulture);

	public static string Field_ServiceName => ResourceManager.GetString("Field_ServiceName", resourceCulture);

	public static string Field_SourceType => ResourceManager.GetString("Field_SourceType", resourceCulture);

	public static string Field_UserName => ResourceManager.GetString("Field_UserName", resourceCulture);

	public static string FileTransferredToRecorder => ResourceManager.GetString("FileTransferredToRecorder", resourceCulture);

	public static string FileTransferToRecorderFailed => ResourceManager.GetString("FileTransferToRecorderFailed", resourceCulture);

	public static string FileUploadedToServer => ResourceManager.GetString("FileUploadedToServer", resourceCulture);

	public static string FileUploadToServerFailed => ResourceManager.GetString("FileUploadToServerFailed", resourceCulture);

	public static string GenericRule => ResourceManager.GetString("GenericRule", resourceCulture);

	public static string HardwareCommunicationError => ResourceManager.GetString("HardwareCommunicationError", resourceCulture);

	public static string HardwareCommunicationEstablished => ResourceManager.GetString("HardwareCommunicationEstablished", resourceCulture);

	public static string HardwareCommunicationStopped => ResourceManager.GetString("HardwareCommunicationStopped", resourceCulture);

	public static string HardwareDriverNotFound => ResourceManager.GetString("HardwareDriverNotFound", resourceCulture);

	public static string HardwareErrorHardwareNotConfigured => ResourceManager.GetString("HardwareErrorHardwareNotConfigured", resourceCulture);

	public static string HardwareIncompatibleDriverFound => ResourceManager.GetString("HardwareIncompatibleDriverFound", resourceCulture);

	public static string HardwarePasswordChangeFailedHardwareBlocked => ResourceManager.GetString("HardwarePasswordChangeFailedHardwareBlocked", resourceCulture);

	public static string HardwarePasswordChangeFailedHardwareNotEnabled => ResourceManager.GetString("HardwarePasswordChangeFailedHardwareNotEnabled", resourceCulture);

	public static string HardwarePasswordChangeFailedHardwareNotStarted => ResourceManager.GetString("HardwarePasswordChangeFailedHardwareNotStarted", resourceCulture);

	public static string HardwarePasswordChangeFailedHardwareReadOnly => ResourceManager.GetString("HardwarePasswordChangeFailedHardwareReadOnly", resourceCulture);

	public static string HardwarePasswordChangeFailedNetworkIssue => ResourceManager.GetString("HardwarePasswordChangeFailedNetworkIssue", resourceCulture);

	public static string HardwarePasswordChangeFailedNoAccess => ResourceManager.GetString("HardwarePasswordChangeFailedNoAccess", resourceCulture);

	public static string HardwarePasswordChangeFailedOtherHardwareError => ResourceManager.GetString("HardwarePasswordChangeFailedOtherHardwareError", resourceCulture);

	public static string HardwarePasswordChangeFailedPasswordInvalidCharacter => ResourceManager.GetString("HardwarePasswordChangeFailedPasswordInvalidCharacter", resourceCulture);

	public static string HardwarePasswordChangeFailedPasswordOrUserLevelMayNotBeChanged => ResourceManager.GetString("HardwarePasswordChangeFailedPasswordOrUserLevelMayNotBeChanged", resourceCulture);

	public static string HardwarePasswordChangeFailedPasswordTooLong => ResourceManager.GetString("HardwarePasswordChangeFailedPasswordTooLong", resourceCulture);

	public static string HardwarePasswordChangeFailedPasswordTooShort => ResourceManager.GetString("HardwarePasswordChangeFailedPasswordTooShort", resourceCulture);

	public static string HardwarePasswordChangeFailedUnknownResult => ResourceManager.GetString("HardwarePasswordChangeFailedUnknownResult", resourceCulture);

	public static string HardwarePasswordChangeFailedUnsupported => ResourceManager.GetString("HardwarePasswordChangeFailedUnsupported", resourceCulture);

	public static string HardwarePasswordChangeFailedUpdateCredentials => ResourceManager.GetString("HardwarePasswordChangeFailedUpdateCredentials", resourceCulture);

	public static string HardwarePasswordChangeFailedUserAccessDenied => ResourceManager.GetString("HardwarePasswordChangeFailedUserAccessDenied", resourceCulture);

	public static string HardwarePasswordChangeFailedUserLevelAnonymousNotAllowed => ResourceManager.GetString("HardwarePasswordChangeFailedUserLevelAnonymousNotAllowed", resourceCulture);

	public static string HardwareUpdateDriverFailed => ResourceManager.GetString("HardwareUpdateDriverFailed", resourceCulture);

	public static string HardwareUpdateDriverNotPossible => ResourceManager.GetString("HardwareUpdateDriverNotPossible", resourceCulture);

	public static string HardwareUpdateDriverSuccess => ResourceManager.GetString("HardwareUpdateDriverSuccess", resourceCulture);

	public static string HardwareUpdateNoDefaultDatabase => ResourceManager.GetString("HardwareUpdateNoDefaultDatabase", resourceCulture);

	public static string HardwareUpdateNoDefinitionChanges => ResourceManager.GetString("HardwareUpdateNoDefinitionChanges", resourceCulture);

	public static string HardwareUpdatingDriver => ResourceManager.GetString("HardwareUpdatingDriver", resourceCulture);

	public static string InconsistentHardwareConfiguration => ResourceManager.GetString("InconsistentHardwareConfiguration", resourceCulture);

	public static string InitializeAudioMessageManagerFailed => ResourceManager.GetString("InitializeAudioMessageManagerFailed", resourceCulture);

	public static string InitializeDatabaseFailed => ResourceManager.GetString("InitializeDatabaseFailed", resourceCulture);

	public static string InitializeDriverEventListenerFailed => ResourceManager.GetString("InitializeDriverEventListenerFailed", resourceCulture);

	public static string InitializeDriverScanFailed => ResourceManager.GetString("InitializeDriverScanFailed", resourceCulture);

	public static string InitializeDriverServicesFailed => ResourceManager.GetString("InitializeDriverServicesFailed", resourceCulture);

	public static string InitializeEdgeStorageManagerFailed => ResourceManager.GetString("InitializeEdgeStorageManagerFailed", resourceCulture);

	public static string InitializeLicenseManagerFailed => ResourceManager.GetString("InitializeLicenseManagerFailed", resourceCulture);

	public static string InitializeLiveMulticastProcessorFailed => ResourceManager.GetString("InitializeLiveMulticastProcessorFailed", resourceCulture);

	public static string InitializeLoadCachedConfigurationFailed => ResourceManager.GetString("InitializeLoadCachedConfigurationFailed", resourceCulture);

	public static string InitializeLoadCachedSecurityConfigurationFailed => ResourceManager.GetString("InitializeLoadCachedSecurityConfigurationFailed", resourceCulture);

	public static string InitializeLogManagerFailed => ResourceManager.GetString("InitializeLogManagerFailed", resourceCulture);

	public static string InitializeMasterEncryptionKeyFailed => ResourceManager.GetString("InitializeMasterEncryptionKeyFailed", resourceCulture);

	public static string InitializeMediaDbConfigureFailed => ResourceManager.GetString("InitializeMediaDbConfigureFailed", resourceCulture);

	public static string InitializeMediaDbFailed => ResourceManager.GetString("InitializeMediaDbFailed", resourceCulture);

	public static string InitializePatrollingControllerFailed => ResourceManager.GetString("InitializePatrollingControllerFailed", resourceCulture);

	public static string InitializePipelinesFailed => ResourceManager.GetString("InitializePipelinesFailed", resourceCulture);

	public static string InitializeRecorderApplyConfigurationFailed => ResourceManager.GetString("InitializeRecorderApplyConfigurationFailed", resourceCulture);

	public static string InitializeRecorderCommandProcessorFailed => ResourceManager.GetString("InitializeRecorderCommandProcessorFailed", resourceCulture);

	public static string InitializeRecorderCommunicationFailed => ResourceManager.GetString("InitializeRecorderCommunicationFailed", resourceCulture);

	public static string InitializeRecorderRuleProcessorInitializeFailed => ResourceManager.GetString("InitializeRecorderRuleProcessorInitializeFailed", resourceCulture);

	public static string InitializeRecorderRuleProcessorStartFailed => ResourceManager.GetString("InitializeRecorderRuleProcessorStartFailed", resourceCulture);

	public static string InitializeRecorderSNMPManagerFailed => ResourceManager.GetString("InitializeRecorderSNMPManagerFailed", resourceCulture);

	public static string InitializeServerApplyConfigurationFailed => ResourceManager.GetString("InitializeServerApplyConfigurationFailed", resourceCulture);

	public static string InitializeServerCommandProcessorFailed => ResourceManager.GetString("InitializeServerCommandProcessorFailed", resourceCulture);

	public static string InitializeServerCommunicationFailed => ResourceManager.GetString("InitializeServerCommunicationFailed", resourceCulture);

	public static string InitializeServerConnectionFailed => ResourceManager.GetString("InitializeServerConnectionFailed", resourceCulture);

	public static string InitializeServerRuleProcessorFailed => ResourceManager.GetString("InitializeServerRuleProcessorFailed", resourceCulture);

	public static string InitializeServerSNMPManagerFailed => ResourceManager.GetString("InitializeServerSNMPManagerFailed", resourceCulture);

	public static string InitializeSmartSearchManagerFailed => ResourceManager.GetString("InitializeSmartSearchManagerFailed", resourceCulture);

	public static string InitializeStatusManagerFailed => ResourceManager.GetString("InitializeStatusManagerFailed", resourceCulture);

	public static string InitializeTaskManagerFailed => ResourceManager.GetString("InitializeTaskManagerFailed", resourceCulture);

	public static string InitializeVmoClientFailed => ResourceManager.GetString("InitializeVmoClientFailed", resourceCulture);

	public static string InitializeVmoServerObjectFailed => ResourceManager.GetString("InitializeVmoServerObjectFailed", resourceCulture);

	public static string InitializeWebServerFailed => ResourceManager.GetString("InitializeWebServerFailed", resourceCulture);

	public static string LegacyRecorderNoConfigurationUpdates => ResourceManager.GetString("LegacyRecorderNoConfigurationUpdates", resourceCulture);

	public static string LegacyRecorderNoEventUpdates => ResourceManager.GetString("LegacyRecorderNoEventUpdates", resourceCulture);

	public static string LegacyRecorderNoSecurityUpdates => ResourceManager.GetString("LegacyRecorderNoSecurityUpdates", resourceCulture);

	public static string LicenseChangedMacAddressIdentified => ResourceManager.GetString("LicenseChangedMacAddressIdentified", resourceCulture);

	public static string LicenseChangedMacAddressUpdateNotPossible => ResourceManager.GetString("LicenseChangedMacAddressUpdateNotPossible", resourceCulture);

	public static string LicenseChangedUpdatedHardwareMacAddressInvalid => ResourceManager.GetString("LicenseChangedUpdatedHardwareMacAddressInvalid", resourceCulture);

	public static string LicenseDeviceUnlicensed => ResourceManager.GetString("LicenseDeviceUnlicensed", resourceCulture);

	public static string LicenseExpired => ResourceManager.GetString("LicenseExpired", resourceCulture);

	public static string LicenseGetHardwareMacAddressFailed => ResourceManager.GetString("LicenseGetHardwareMacAddressFailed", resourceCulture);

	public static string LicenseHardwareUnlicensed => ResourceManager.GetString("LicenseHardwareUnlicensed", resourceCulture);

	public static string LicenseInitializeFailed => ResourceManager.GetString("LicenseInitializeFailed", resourceCulture);

	public static string LicenseInvalid => ResourceManager.GetString("LicenseInvalid", resourceCulture);

	public static string LicenseNoMacAddressForHardware => ResourceManager.GetString("LicenseNoMacAddressForHardware", resourceCulture);

	public static string LicenseUnLicensedServer => ResourceManager.GetString("LicenseUnLicensedServer", resourceCulture);

	public static string LicenseUpdateFailed => ResourceManager.GetString("LicenseUpdateFailed", resourceCulture);

	public static string LicenseValid => ResourceManager.GetString("LicenseValid", resourceCulture);

	public static string LoadCachedConfigurationNoData => ResourceManager.GetString("LoadCachedConfigurationNoData", resourceCulture);

	public static string LoadCachedSecurityConfigurationNoData => ResourceManager.GetString("LoadCachedSecurityConfigurationNoData", resourceCulture);

	public static string LoadExternalPluginConfigurationFailed => ResourceManager.GetString("LoadExternalPluginConfigurationFailed", resourceCulture);

	public static string MatrixCreated => ResourceManager.GetString("MatrixCreated", resourceCulture);

	public static string MatrixDeleted => ResourceManager.GetString("MatrixDeleted", resourceCulture);

	public static string MatrixLog => ResourceManager.GetString("MatrixLog", resourceCulture);

	public static string MatrixUpdated => ResourceManager.GetString("MatrixUpdated", resourceCulture);

	public static string MediaDatabaseArchivingError => ResourceManager.GetString("MediaDatabaseArchivingError", resourceCulture);

	public static string MediaDatabaseArchivingErrorUnknown => ResourceManager.GetString("MediaDatabaseArchivingErrorUnknown", resourceCulture);

	public static string MediaDatabaseDeleteError => ResourceManager.GetString("MediaDatabaseDeleteError", resourceCulture);

	public static string MediaDatabaseDeleteErrorUnknown => ResourceManager.GetString("MediaDatabaseDeleteErrorUnknown", resourceCulture);

	public static string MediaDatabaseMinimumDiskSpaceReached => ResourceManager.GetString("MediaDatabaseMinimumDiskSpaceReached", resourceCulture);

	public static string MediaDatabaseRepairTableCompleted => ResourceManager.GetString("MediaDatabaseRepairTableCompleted", resourceCulture);

	public static string MediaDatabaseRepairTableCompletedUnknown => ResourceManager.GetString("MediaDatabaseRepairTableCompletedUnknown", resourceCulture);

	public static string MediaDatabaseRepairTableError => ResourceManager.GetString("MediaDatabaseRepairTableError", resourceCulture);

	public static string MediaDatabaseRepairTableErrorUnknown => ResourceManager.GetString("MediaDatabaseRepairTableErrorUnknown", resourceCulture);

	public static string MediaDatabaseRepairTableStarted => ResourceManager.GetString("MediaDatabaseRepairTableStarted", resourceCulture);

	public static string MediaDatabaseRepairTableStartedUnknown => ResourceManager.GetString("MediaDatabaseRepairTableStartedUnknown", resourceCulture);

	public static string MediaDatabaseStorageAvailable => ResourceManager.GetString("MediaDatabaseStorageAvailable", resourceCulture);

	public static string MediaDatabaseStorageDataMoveBegin => ResourceManager.GetString("MediaDatabaseStorageDataMoveBegin", resourceCulture);

	public static string MediaDatabaseStorageDataMoveFailed => ResourceManager.GetString("MediaDatabaseStorageDataMoveFailed", resourceCulture);

	public static string MediaDatabaseStorageDataMoveFinishSuccess => ResourceManager.GetString("MediaDatabaseStorageDataMoveFinishSuccess", resourceCulture);

	public static string MediaDatabaseStorageDataMoveRetry => ResourceManager.GetString("MediaDatabaseStorageDataMoveRetry", resourceCulture);

	public static string MediaDatabaseStorageMergeError => ResourceManager.GetString("MediaDatabaseStorageMergeError", resourceCulture);

	public static string MediaDatabaseStorageMergeStarted => ResourceManager.GetString("MediaDatabaseStorageMergeStarted", resourceCulture);

	public static string MediaDatabaseStorageMergeStopped => ResourceManager.GetString("MediaDatabaseStorageMergeStopped", resourceCulture);

	public static string MediaDatabaseStorageUnavailable => ResourceManager.GetString("MediaDatabaseStorageUnavailable", resourceCulture);

	public static string MediaDatabaseTableSplitTookLong => ResourceManager.GetString("MediaDatabaseTableSplitTookLong", resourceCulture);

	public static string MediaDatabaseTableSplitTookLongUnknown => ResourceManager.GetString("MediaDatabaseTableSplitTookLongUnknown", resourceCulture);

	public static string MediaDatabaseUpgradeBegin => ResourceManager.GetString("MediaDatabaseUpgradeBegin", resourceCulture);

	public static string MediaDatabaseUpgradeFailedLastRetry => ResourceManager.GetString("MediaDatabaseUpgradeFailedLastRetry", resourceCulture);

	public static string MediaDatabaseUpgradeFailedNoRetry => ResourceManager.GetString("MediaDatabaseUpgradeFailedNoRetry", resourceCulture);

	public static string MediaDatabaseUpgradeFailedRetry => ResourceManager.GetString("MediaDatabaseUpgradeFailedRetry", resourceCulture);

	public static string MediaDatabaseUpgradeSuccess => ResourceManager.GetString("MediaDatabaseUpgradeSuccess", resourceCulture);

	public static string MsFailoverNodeStartedEventFired => ResourceManager.GetString("MsFailoverNodeStartedEventFired", resourceCulture);

	public static string NetworkSettingsManagementErrorNoSuitableInterfaceAddress => ResourceManager.GetString("NetworkSettingsManagementErrorNoSuitableInterfaceAddress", resourceCulture);

	public static string NetworkSettingsManagementErrorUnknown => ResourceManager.GetString("NetworkSettingsManagementErrorUnknown", resourceCulture);

	public static string NetworkSettingsManagementErrorUnsupported => ResourceManager.GetString("NetworkSettingsManagementErrorUnsupported", resourceCulture);

	public static string RecorderAdded => ResourceManager.GetString("RecorderAdded", resourceCulture);

	public static string RecorderConnected => ResourceManager.GetString("RecorderConnected", resourceCulture);

	public static string RecorderDisconnected => ResourceManager.GetString("RecorderDisconnected", resourceCulture);

	public static string RecorderFailed => ResourceManager.GetString("RecorderFailed", resourceCulture);

	public static string RecorderNotAllowedToStart => ResourceManager.GetString("RecorderNotAllowedToStart", resourceCulture);

	public static string RecorderNotCompatible => ResourceManager.GetString("RecorderNotCompatible", resourceCulture);

	public static string RecorderPluginNotConfigured => ResourceManager.GetString("RecorderPluginNotConfigured", resourceCulture);

	public static string RecorderSNMPServiceNotInstalled => ResourceManager.GetString("RecorderSNMPServiceNotInstalled", resourceCulture);

	public static string RecorderSNMPServiceNotStarted => ResourceManager.GetString("RecorderSNMPServiceNotStarted", resourceCulture);

	public static string RecorderStarted => ResourceManager.GetString("RecorderStarted", resourceCulture);

	public static string RecorderStopped => ResourceManager.GetString("RecorderStopped", resourceCulture);

	public static string RecordingStorageDeleteBegin => ResourceManager.GetString("RecordingStorageDeleteBegin", resourceCulture);

	public static string RecordingStorageDeleteFailedLastRetry => ResourceManager.GetString("RecordingStorageDeleteFailedLastRetry", resourceCulture);

	public static string RecordingStorageDeleteFailedNoRetry => ResourceManager.GetString("RecordingStorageDeleteFailedNoRetry", resourceCulture);

	public static string RecordingStorageDeleteFailedRetry => ResourceManager.GetString("RecordingStorageDeleteFailedRetry", resourceCulture);

	public static string RecordingStorageDeleteSuccess => ResourceManager.GetString("RecordingStorageDeleteSuccess", resourceCulture);

	public static string RecordingStorageMoveBegin => ResourceManager.GetString("RecordingStorageMoveBegin", resourceCulture);

	public static string RecordingStorageMoveFailedLastRetry => ResourceManager.GetString("RecordingStorageMoveFailedLastRetry", resourceCulture);

	public static string RecordingStorageMoveFailedNoRetry => ResourceManager.GetString("RecordingStorageMoveFailedNoRetry", resourceCulture);

	public static string RecordingStorageMoveFailedRetry => ResourceManager.GetString("RecordingStorageMoveFailedRetry", resourceCulture);

	public static string RecordingStorageMoveSuccess => ResourceManager.GetString("RecordingStorageMoveSuccess", resourceCulture);

	public static string RecordingStorageSetEncryptionMethodBegin => ResourceManager.GetString("RecordingStorageSetEncryptionMethodBegin", resourceCulture);

	public static string RecordingStorageSetEncryptionMethodSuccess => ResourceManager.GetString("RecordingStorageSetEncryptionMethodSuccess", resourceCulture);

	public static string RecordingStorageSetEncryptionPasswordBegin => ResourceManager.GetString("RecordingStorageSetEncryptionPasswordBegin", resourceCulture);

	public static string RecordingStorageSetEncryptionPasswordFailedLastRetry => ResourceManager.GetString("RecordingStorageSetEncryptionPasswordFailedLastRetry", resourceCulture);

	public static string RecordingStorageSetEncryptionPasswordFailedNoRetry => ResourceManager.GetString("RecordingStorageSetEncryptionPasswordFailedNoRetry", resourceCulture);

	public static string RecordingStorageSetEncryptionPasswordFailedRetry => ResourceManager.GetString("RecordingStorageSetEncryptionPasswordFailedRetry", resourceCulture);

	public static string RecordingStorageSetEncryptionPasswordSuccess => ResourceManager.GetString("RecordingStorageSetEncryptionPasswordSuccess", resourceCulture);

	public static string ScheduledHardwarePasswordChangeFailure => ResourceManager.GetString("ScheduledHardwarePasswordChangeFailure", resourceCulture);

	public static string ScheduledHardwarePasswordChangeSuccess => ResourceManager.GetString("ScheduledHardwarePasswordChangeSuccess", resourceCulture);

	public static string SendNotificationError => ResourceManager.GetString("SendNotificationError", resourceCulture);

	public static string ServerFailed => ResourceManager.GetString("ServerFailed", resourceCulture);

	public static string ServerSNMPServiceNotInstalled => ResourceManager.GetString("ServerSNMPServiceNotInstalled", resourceCulture);

	public static string ServerSNMPServiceNotStarted => ResourceManager.GetString("ServerSNMPServiceNotStarted", resourceCulture);

	public static string ServerStarted => ResourceManager.GetString("ServerStarted", resourceCulture);

	public static string ServerStopped => ResourceManager.GetString("ServerStopped", resourceCulture);

	public static string ShutdownCommandProcessorFailed => ResourceManager.GetString("ShutdownCommandProcessorFailed", resourceCulture);

	public static string ShutdownDriverEventListenerFailed => ResourceManager.GetString("ShutdownDriverEventListenerFailed", resourceCulture);

	public static string ShutdownDriverServicesFailed => ResourceManager.GetString("ShutdownDriverServicesFailed", resourceCulture);

	public static string ShutdownEdgeStorageFailed => ResourceManager.GetString("ShutdownEdgeStorageFailed", resourceCulture);

	public static string ShutdownEngineFailed => ResourceManager.GetString("ShutdownEngineFailed", resourceCulture);

	public static string ShutdownFailoverResponseServerFailed => ResourceManager.GetString("ShutdownFailoverResponseServerFailed", resourceCulture);

	public static string ShutdownMediaDbFailed => ResourceManager.GetString("ShutdownMediaDbFailed", resourceCulture);

	public static string ShutdownPatrollingControllerFailed => ResourceManager.GetString("ShutdownPatrollingControllerFailed", resourceCulture);

	public static string ShutdownRecorderCommandProcessorFailed => ResourceManager.GetString("ShutdownRecorderCommandProcessorFailed", resourceCulture);

	public static string ShutdownRecorderRuleProcessorFailed => ResourceManager.GetString("ShutdownRecorderRuleProcessorFailed", resourceCulture);

	public static string ShutdownRecorderSNMPManagerFailed => ResourceManager.GetString("ShutdownRecorderSNMPManagerFailed", resourceCulture);

	public static string ShutdownRuleProcessorFailed => ResourceManager.GetString("ShutdownRuleProcessorFailed", resourceCulture);

	public static string ShutdownServerCommunicationManagerFailed => ResourceManager.GetString("ShutdownServerCommunicationManagerFailed", resourceCulture);

	public static string ShutdownServerComplete => ResourceManager.GetString("ShutdownServerComplete", resourceCulture);

	public static string ShutdownServerSNMPManagerFailed => ResourceManager.GetString("ShutdownServerSNMPManagerFailed", resourceCulture);

	public static string ShutdownSmartSearchManagerFailed => ResourceManager.GetString("ShutdownSmartSearchManagerFailed", resourceCulture);

	public static string ShutdownStatusManagerFailed => ResourceManager.GetString("ShutdownStatusManagerFailed", resourceCulture);

	public static string ShutdownTaskManagerFailed => ResourceManager.GetString("ShutdownTaskManagerFailed", resourceCulture);

	public static string ShutdownWebServerFailed => ResourceManager.GetString("ShutdownWebServerFailed", resourceCulture);

	public static string TableUnrepairableAway => ResourceManager.GetString("TableUnrepairableAway", resourceCulture);

	public static string UserHandlingErrorAccessDenied => ResourceManager.GetString("UserHandlingErrorAccessDenied", resourceCulture);

	public static string UserHandlingErrorDeleteCurrentUserNotAllowed => ResourceManager.GetString("UserHandlingErrorDeleteCurrentUserNotAllowed", resourceCulture);

	public static string UserHandlingErrorUnknown => ResourceManager.GetString("UserHandlingErrorUnknown", resourceCulture);

	public static string UserHandlingErrorUnsupported => ResourceManager.GetString("UserHandlingErrorUnsupported", resourceCulture);

	public static string UserHandlingErrorUserFixed => ResourceManager.GetString("UserHandlingErrorUserFixed", resourceCulture);

	public static string UserHandlingErrorUserNameClash => ResourceManager.GetString("UserHandlingErrorUserNameClash", resourceCulture);

	public static string UserHandlingErrorUserNameNotRecognized => ResourceManager.GetString("UserHandlingErrorUserNameNotRecognized", resourceCulture);

	public static string UserHandlingErrorUserNotCreated => ResourceManager.GetString("UserHandlingErrorUserNotCreated", resourceCulture);

	public static string WebServerLoadCertificateFailed => ResourceManager.GetString("WebServerLoadCertificateFailed", resourceCulture);

	public LogDefinitions()
	{
	}
}
