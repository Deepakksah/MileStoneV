using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/EventServerAccessControlCommandCredentials", ConfigurationName = "VideoOS.EventServer.Communication.AccessControl.Client.IAccessControlCommandCredentials", SessionMode = SessionMode.NotAllowed)]
public interface IAccessControlCommandCredentials
{
	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPlugins", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginsResp GetAccessControlPlugins(GetAccessControlPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPlugins", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginsResponse")]
	Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(GetAccessControlPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginIcons", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginIconsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginIconsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginIconsResp GetAccessControlPluginIcons(GetAccessControlPluginIconsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginIcons", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginIconsResponse")]
	Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(GetAccessControlPluginIconsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginCategoriesResp GetAccessControlPluginCategories(GetAccessControlPluginCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlPluginCategoriesResponse")]
	Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(GetAccessControlPluginCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystems", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlSystemsResp GetAccessControlSystems(GetAccessControlSystemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystems", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemsResponse")]
	Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(GetAccessControlSystemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateAccessControlSystemProperties", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateAccessControlSystemPropertiesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateAccessControlSystemPropertiesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ValidateAccessControlSystemPropertiesResp ValidateAccessControlSystemProperties(ValidateAccessControlSystemPropertiesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateAccessControlSystemProperties", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateAccessControlSystemPropertiesResponse")]
	Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(ValidateAccessControlSystemPropertiesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CreateAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CreateAccessControlSystemResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CreateAccessControlSystemAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	CreateAccessControlSystemResp CreateAccessControlSystem(CreateAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CreateAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CreateAccessControlSystemResponse")]
	Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(CreateAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/DeleteAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/DeleteAccessControlSystemResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/DeleteAccessControlSystemAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	DeleteAccessControlSystemResp DeleteAccessControlSystem(DeleteAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/DeleteAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/DeleteAccessControlSystemResponse")]
	Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(DeleteAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlSystemConfigurationResp GetAccessControlSystemConfiguration(GetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlSystemConfigurationResponse")]
	Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(GetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetAccessControlSystemConfigurationResp SetAccessControlSystemConfiguration(SetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlSystemConfigurationResponse")]
	Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(SetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/StartFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/StartFetchAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/StartFetchAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	StartFetchAccessControlSystemConfigurationResp StartFetchAccessControlSystemConfiguration(StartFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/StartFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/StartFetchAccessControlSystemConfigurationResponse")]
	Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(StartFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CancelFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CancelFetchAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CancelFetchAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	CancelFetchAccessControlSystemConfigurationResp CancelFetchAccessControlSystemConfiguration(CancelFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CancelFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/CancelFetchAccessControlSystemConfigurationResponse")]
	Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(CancelFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetCredentialHoldersResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetCredentialHoldersAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetCredentialHoldersResp GetCredentialHolders(GetCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetCredentialHoldersResponse")]
	Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(GetCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetCredentialHolderImage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetCredentialHolderImageResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetCredentialHolderImageAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetCredentialHolderImageResp SetCredentialHolderImage(SetCredentialHolderImageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetCredentialHolderImage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetCredentialHolderImageResponse")]
	Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(SetCredentialHolderImageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SearchCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SearchCredentialHoldersResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SearchCredentialHoldersAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SearchCredentialHoldersResp SearchCredentialHolders(SearchCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SearchCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SearchCredentialHoldersResponse")]
	Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(SearchCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetStates", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetStatesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetStatesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetStatesResp GetStates(GetStatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetStates", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetStatesResponse")]
	Task<GetStatesResp> GetStatesAsync(GetStatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ExecuteCommand", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ExecuteCommandResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ExecuteCommandAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ExecuteCommandResp ExecuteCommand(ExecuteCommandParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ExecuteCommand", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ExecuteCommandResponse")]
	Task<ExecuteCommandResp> ExecuteCommandAsync(ExecuteCommandParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetGlobalSettingsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetGlobalSettingsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetGlobalSettingsResp GetGlobalSettings(GetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetGlobalSettingsResponse")]
	Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(GetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetGlobalSettingsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetGlobalSettingsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetGlobalSettingsResp SetGlobalSettings(SetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetGlobalSettingsResponse")]
	Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(SetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetUserDefinedCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetUserDefinedCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetUserDefinedCategoriesResp GetUserDefinedCategories(GetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetUserDefinedCategoriesResponse")]
	Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(GetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetUserDefinedCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetUserDefinedCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetUserDefinedCategoriesResp SetUserDefinedCategories(SetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetUserDefinedCategoriesResponse")]
	Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(SetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateUserCredentials", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateUserCredentialsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateUserCredentialsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ValidateUserCredentialsResp ValidateUserCredentials(ValidateUserCredentialsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateUserCredentials", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/ValidateUserCredentialsResponse")]
	Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(ValidateUserCredentialsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEvents", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventsResp GetAccessControlEvents(GetAccessControlEventsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEvents", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsResponse")]
	Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(GetAccessControlEventsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsWithDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsWithDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventsWithDowntimesResp GetAccessControlEventsWithDowntimes(GetAccessControlEventsWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventsWithDowntimesResponse")]
	Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(GetAccessControlEventsWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventPageResp GetAccessControlEventPage(GetAccessControlEventPageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageResponse")]
	Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(GetAccessControlEventPageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageWithDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageWithDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventPageWithDowntimesResp GetAccessControlEventPageWithDowntimes(GetAccessControlEventPageWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventPageWithDowntimesResponse")]
	Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(GetAccessControlEventPageWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventCount", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventCountResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventCountAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventCountResp GetAccessControlEventCount(GetAccessControlEventCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventCount", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetAccessControlEventCountResponse")]
	Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(GetAccessControlEventCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetDowntimesResp GetDowntimes(GetDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/GetDowntimesResponse")]
	Task<GetDowntimesResp> GetDowntimesAsync(GetDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlUnitPosition", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandCredentials/IAccessControlCommandCredentials/SetAccessControlUnitPositionResponse")]
	Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(SetAccessControlUnitPositionParams parameters);
}
