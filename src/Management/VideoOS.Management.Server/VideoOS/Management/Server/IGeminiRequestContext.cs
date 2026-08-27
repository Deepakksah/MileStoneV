using System;
using System.Collections.Generic;
using VideoOS.Common.Integration;
using VideoOS.Management.Server.ConfigurationApi.Security;

namespace VideoOS.Management.Server;

public interface IGeminiRequestContext
{
	string AuthenticatedUserDomain { get; set; }

	string AuthenticatedUserName { get; set; }

	string AuthenticatedUserSid { get; set; }

	string AuthenticationType { get; set; }

	string UserDomainAccountName { get; }

	bool IsSystemContext { get; }

	bool IsBasicUser { get; }

	bool IsExternalVmsUser { get; }

	DateTime UserUpdatedAt { get; }

	IdentityDescriptor UserContext { get; }

	string LocalIPAddress { get; set; }

	string RemoteIPAddress { get; set; }

	string RemotePort { get; set; }

	ITimeRequest RequestTimer { get; }

	ITraceRequest RequestTracer { get; }

	ILogRequest RequestLogger { get; }

	long CpuCycles { get; }

	MethodInformation Method { get; }

	int MethodExecutionThreadId { get; }

	Dictionary<string, object> Items { get; }

	IDeviceTypeLookup DeviceTypeLookup { get; }

	ISecurityObjectMapCache SecurityObjectMapCache { get; }

	IFeatureCacheManager FeatureCacheManager { get; }

	TReturnType GetService<TService, TReturnType>() where TService : class, IGeminiServerService;

	TService GetService<TService>() where TService : class, IGeminiServerService;

	TComponent CreateComponent<TComponent>() where TComponent : GeminiResourceComponent, new();

	void EnterMethod(MethodInformation methodInformation);

	void LeaveMethod();

	ISystemMonitorEventQueueService GetSystemMonitorEventQueueService();

	INameLookupService GetNameLookupService();

	ISecurityManagerService GetSecurityManagerService();

	IIntegrationService GetIntegrationService();

	IIdentityService GetIdentityService();

	IGeminiIdentityService GetGeminiIdentityService();

	ITokenCacheService GetTokenCacheService();

	IEncryptionService GetEncryptionService();

	IManagementServerComponent CreateManagementServerComponent();

	ISecurityComponent CreateSecurityComponent();

	ISecurityTokenComponent CreateSecurityTokenComponent();

	IDeviceComponent CreateDeviceComponent();

	IDeviceGroupComponent CreateDeviceGroupComponent();

	IHardwareComponent CreateHardwareComponent();

	IHardwareDefinitionComponent CreateHardwareDefinitionComponent();

	IHardwareDriverComponent CreateHardwareDriverComponent();

	IDriverSettingsComponent CreateDriverSettingsComponent();

	IRecordingStorageComponent CreateRecordingStorageComponent();

	IArchiveStorageComponent CreateArchiveStorageComponent();

	IPatrollingProfileComponent CreatePatrollingProfileComponent();

	IIdentityCacheComponent CreateIdentityCacheComponent();

	ILicenseComponent CreateLicenseComponent();

	IUserDefinedEventComponent CreateUserDefinedEventComponent();

	IRoleComponent CreateRoleComponent();

	IBasicUserComponent CreateBasicUserComponent();

	IRecordingServerComponent CreateRecordingServerComponent();

	IAuthorizeCookieComponent CreateAuthorizeCookieComponent();

	IPerformanceStateRuleConfigComponent CreatePerformanceStateRuleConfigComponent();

	ISiteComponent CreateSiteComponent();

	IDataCollectionComponent CreateDataCollectionComponent();

	IRetentionOptionRoleRelationComponent CreateRetentionOptionRoleRelationComponent();

	IVideoWallComponent CreateVideoWallComponent();

	IMonitorComponent CreateMonitorComponent();

	IClientProfileComponent CreateClientProfileComponent();

	IFailoverServerComponent CreateFailoverServerComponent();

	IHostedHardwareComponent CreateHostedHardwareComponent();

	IHostingServerComponent CreateHostingServerComponent();

	ITaskComponent CreateTaskComponent();

	IRegisteredServerComponent CreateRegisteredServerComponent();

	ICustomPropertyComponent CreateCustomPropertyComponent();

	ICustomSettingComponent CreateCustomSettingComponent();

	IBasicOwnerInfoComponent CreateBasicOwnerInfoComponent();

	IMarkedDataComponent CreateMarkedDataComponent();

	IRestrictedMediaComponent CreateRestrictedMediaComponent();

	IServerGroupComponent CreateServerGroupComponent();

	IPluginEventComponent CreatePluginEventComponent();

	IIntegrationRegistrationsComponent CreateIntegrationRegistrationsComponent();

	ITimeProfileComponent CreateTimeProfileComponent();

	IMIPActionComponent CreateMIPActionComponent();

	IMIPObjectComponent CreateMIPObjectComponent();

	IMIPItemComponent CreateMIPItemComponent();

	IMIPKindComponent CreateMIPKindComponent();

	IMIPServerGroupComponent CreateMIPServerGroupComponent();

	IVideoWallFunctionalityComponent CreateVideoWallFunctionalityComponent();

	IVideoWallPresetComponent CreateVideoWallPresetComponent();

	IViewComponent CreateViewComponent();

	IViewLayoutComponent CreateViewLayoutComponent();

	IAudioMessageComponent<AudioMessage> CreateAudioMessageComponent();

	IRuleComponent CreateRuleComponent();

	IRuleSourceComponent CreateRuleSourceComponent();

	IEventTypeComponent CreateEventTypeComponent();

	IDatabaseSettings GetDatabaseSettings();

	ISqlLogProcessor CreateLogManager();

	IEventTypeDataComponent CreateEventTypeDataComponent();

	IRecorderDataComponent CreateRecorderDataComponent();

	IHardwareDataComponent CreateHardwareDataComponent();

	IConfigurationDataComponent CreateConfigurationDataComponent();

	IDeviceDataComponent CreateDeviceDataComponent();

	IDeviceTrackDataComponent CreateDeviceTrackDataComponent();

	IDeviceDisabledDataComponent CreateDeviceDisabledDataComponent();

	IDeviceGroupDataComponent CreateDeviceGroupDataComponent();

	IDeviceLinkDataComponent CreateDeviceLinkDataComponent();

	IDeviceRecorderPluginDataComponent CreateDeviceRecorderPluginDataComponent();

	IRecorderPluginDataComponent CreateRecorderPluginDataComponent();

	IExternalPluginsDataComponent CreateExternalPluginsDataComponent();

	IMatrixViewDataComponent CreateMatrixViewDataComponent();

	IMetadataTypeDataComponent CreateMetadataTypeDataComponent();

	ISearchTemplateComponent CreateSearchTemplateDataComponent();

	IMetadataSearchComponent CreateMetadataSearchComponent();

	ILogServerConfigurationComponent CreateLogServerConfigurationComponent();

	IRegisteredClaimComponent CreateRegisteredClaimComponent();

	IStateGroupComponent CreateStateGroupComponent();

	ICompressorComponent CreateCompressorComponent();

	IMailNotificationProfileComponent CreateMailNotificationProfileComponent();

	IAccessControlMemberComponent CreateAccessControlMemberComponent();
}
