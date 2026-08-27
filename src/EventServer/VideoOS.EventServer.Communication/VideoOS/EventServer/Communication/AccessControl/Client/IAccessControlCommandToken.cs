using System.CodeDom.Compiler;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[GeneratedCode("System.ServiceModel", "4.0.0.0")]
[ServiceContract(Namespace = "http://videoos.net/2/EventServerAccessControlCommandToken", ConfigurationName = "VideoOS.EventServer.Communication.AccessControl.Client.IAccessControlCommandToken", SessionMode = SessionMode.NotAllowed)]
public interface IAccessControlCommandToken
{
	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPlugins", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginsResp GetAccessControlPlugins(string token, GetAccessControlPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPlugins", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginsResponse")]
	Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(string token, GetAccessControlPluginsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginIcons", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginIconsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginIconsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginIconsResp GetAccessControlPluginIcons(string token, GetAccessControlPluginIconsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginIcons", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginIconsResponse")]
	Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(string token, GetAccessControlPluginIconsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlPluginCategoriesResp GetAccessControlPluginCategories(string token, GetAccessControlPluginCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlPluginCategoriesResponse")]
	Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(string token, GetAccessControlPluginCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystems", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlSystemsResp GetAccessControlSystems(string token, GetAccessControlSystemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystems", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemsResponse")]
	Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(string token, GetAccessControlSystemsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateAccessControlSystemProperties", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateAccessControlSystemPropertiesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateAccessControlSystemPropertiesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ValidateAccessControlSystemPropertiesResp ValidateAccessControlSystemProperties(string token, ValidateAccessControlSystemPropertiesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateAccessControlSystemProperties", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateAccessControlSystemPropertiesResponse")]
	Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(string token, ValidateAccessControlSystemPropertiesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CreateAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CreateAccessControlSystemResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CreateAccessControlSystemAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	CreateAccessControlSystemResp CreateAccessControlSystem(string token, CreateAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CreateAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CreateAccessControlSystemResponse")]
	Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(string token, CreateAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/DeleteAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/DeleteAccessControlSystemResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/DeleteAccessControlSystemAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	DeleteAccessControlSystemResp DeleteAccessControlSystem(string token, DeleteAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/DeleteAccessControlSystem", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/DeleteAccessControlSystemResponse")]
	Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(string token, DeleteAccessControlSystemParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlSystemConfigurationResp GetAccessControlSystemConfiguration(string token, GetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlSystemConfigurationResponse")]
	Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(string token, GetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetAccessControlSystemConfigurationResp SetAccessControlSystemConfiguration(string token, SetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlSystemConfigurationResponse")]
	Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(string token, SetAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/StartFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/StartFetchAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/StartFetchAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	StartFetchAccessControlSystemConfigurationResp StartFetchAccessControlSystemConfiguration(string token, StartFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/StartFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/StartFetchAccessControlSystemConfigurationResponse")]
	Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(string token, StartFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CancelFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CancelFetchAccessControlSystemConfigurationResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CancelFetchAccessControlSystemConfigurationAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	CancelFetchAccessControlSystemConfigurationResp CancelFetchAccessControlSystemConfiguration(string token, CancelFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CancelFetchAccessControlSystemConfiguration", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/CancelFetchAccessControlSystemConfigurationResponse")]
	Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(string token, CancelFetchAccessControlSystemConfigurationParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetCredentialHoldersResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetCredentialHoldersAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetCredentialHoldersResp GetCredentialHolders(string token, GetCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetCredentialHoldersResponse")]
	Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(string token, GetCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetCredentialHolderImage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetCredentialHolderImageResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetCredentialHolderImageAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetCredentialHolderImageResp SetCredentialHolderImage(string token, SetCredentialHolderImageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetCredentialHolderImage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetCredentialHolderImageResponse")]
	Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(string token, SetCredentialHolderImageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SearchCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SearchCredentialHoldersResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SearchCredentialHoldersAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SearchCredentialHoldersResp SearchCredentialHolders(string token, SearchCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SearchCredentialHolders", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SearchCredentialHoldersResponse")]
	Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(string token, SearchCredentialHoldersParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetStates", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetStatesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetStatesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetStatesResp GetStates(string token, GetStatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetStates", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetStatesResponse")]
	Task<GetStatesResp> GetStatesAsync(string token, GetStatesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ExecuteCommand", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ExecuteCommandResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ExecuteCommandAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ExecuteCommandResp ExecuteCommand(string token, ExecuteCommandParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ExecuteCommand", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ExecuteCommandResponse")]
	Task<ExecuteCommandResp> ExecuteCommandAsync(string token, ExecuteCommandParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetGlobalSettingsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetGlobalSettingsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetGlobalSettingsResp GetGlobalSettings(string token, GetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetGlobalSettingsResponse")]
	Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(string token, GetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetGlobalSettingsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetGlobalSettingsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetGlobalSettingsResp SetGlobalSettings(string token, SetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetGlobalSettings", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetGlobalSettingsResponse")]
	Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(string token, SetGlobalSettingsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetUserDefinedCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetUserDefinedCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetUserDefinedCategoriesResp GetUserDefinedCategories(string token, GetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetUserDefinedCategoriesResponse")]
	Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(string token, GetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetUserDefinedCategoriesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetUserDefinedCategoriesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	SetUserDefinedCategoriesResp SetUserDefinedCategories(string token, SetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetUserDefinedCategories", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetUserDefinedCategoriesResponse")]
	Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(string token, SetUserDefinedCategoriesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateUserCredentials", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateUserCredentialsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateUserCredentialsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	ValidateUserCredentialsResp ValidateUserCredentials(string token, ValidateUserCredentialsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateUserCredentials", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/ValidateUserCredentialsResponse")]
	Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(string token, ValidateUserCredentialsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEvents", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventsResp GetAccessControlEvents(string token, GetAccessControlEventsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEvents", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsResponse")]
	Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(string token, GetAccessControlEventsParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsWithDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsWithDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventsWithDowntimesResp GetAccessControlEventsWithDowntimes(string token, GetAccessControlEventsWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventsWithDowntimesResponse")]
	Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(string token, GetAccessControlEventsWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventPageResp GetAccessControlEventPage(string token, GetAccessControlEventPageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPage", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageResponse")]
	Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(string token, GetAccessControlEventPageParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageWithDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageWithDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventPageWithDowntimesResp GetAccessControlEventPageWithDowntimes(string token, GetAccessControlEventPageWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageWithDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventPageWithDowntimesResponse")]
	Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(string token, GetAccessControlEventPageWithDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventCount", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventCountResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventCountAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetAccessControlEventCountResp GetAccessControlEventCount(string token, GetAccessControlEventCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventCount", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetAccessControlEventCountResponse")]
	Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(string token, GetAccessControlEventCountParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetDowntimesResponse")]
	[FaultContract(typeof(AccessControlServiceFault), Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetDowntimesAccessControlServiceFaultFault", Name = "AccessControlServiceFault", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
	GetDowntimesResp GetDowntimes(string token, GetDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetDowntimes", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/GetDowntimesResponse")]
	Task<GetDowntimesResp> GetDowntimesAsync(string token, GetDowntimesParams parameters);

	[OperationContract(Action = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlUnitPosition", ReplyAction = "http://videoos.net/2/EventServerAccessControlCommandToken/IAccessControlCommandToken/SetAccessControlUnitPositionResponse")]
	Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(string token, SetAccessControlUnitPositionParams parameters);
}
