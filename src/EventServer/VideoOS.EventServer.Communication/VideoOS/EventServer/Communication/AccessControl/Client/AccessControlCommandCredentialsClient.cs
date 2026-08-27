using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AccessControlCommandCredentialsClient : ClientBase<IAccessControlCommandCredentials>, IAccessControlCommandCredentials
{
	public AccessControlCommandCredentialsClient()
	{
	}

	public AccessControlCommandCredentialsClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AccessControlCommandCredentialsClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AccessControlCommandCredentialsClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AccessControlCommandCredentialsClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetAccessControlPluginsResp GetAccessControlPlugins(GetAccessControlPluginsParams parameters)
	{
		return base.Channel.GetAccessControlPlugins(parameters);
	}

	public Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(GetAccessControlPluginsParams parameters)
	{
		return base.Channel.GetAccessControlPluginsAsync(parameters);
	}

	public GetAccessControlPluginIconsResp GetAccessControlPluginIcons(GetAccessControlPluginIconsParams parameters)
	{
		return base.Channel.GetAccessControlPluginIcons(parameters);
	}

	public Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(GetAccessControlPluginIconsParams parameters)
	{
		return base.Channel.GetAccessControlPluginIconsAsync(parameters);
	}

	public GetAccessControlPluginCategoriesResp GetAccessControlPluginCategories(GetAccessControlPluginCategoriesParams parameters)
	{
		return base.Channel.GetAccessControlPluginCategories(parameters);
	}

	public Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(GetAccessControlPluginCategoriesParams parameters)
	{
		return base.Channel.GetAccessControlPluginCategoriesAsync(parameters);
	}

	public GetAccessControlSystemsResp GetAccessControlSystems(GetAccessControlSystemsParams parameters)
	{
		return base.Channel.GetAccessControlSystems(parameters);
	}

	public Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(GetAccessControlSystemsParams parameters)
	{
		return base.Channel.GetAccessControlSystemsAsync(parameters);
	}

	public ValidateAccessControlSystemPropertiesResp ValidateAccessControlSystemProperties(ValidateAccessControlSystemPropertiesParams parameters)
	{
		return base.Channel.ValidateAccessControlSystemProperties(parameters);
	}

	public Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(ValidateAccessControlSystemPropertiesParams parameters)
	{
		return base.Channel.ValidateAccessControlSystemPropertiesAsync(parameters);
	}

	public CreateAccessControlSystemResp CreateAccessControlSystem(CreateAccessControlSystemParams parameters)
	{
		return base.Channel.CreateAccessControlSystem(parameters);
	}

	public Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(CreateAccessControlSystemParams parameters)
	{
		return base.Channel.CreateAccessControlSystemAsync(parameters);
	}

	public DeleteAccessControlSystemResp DeleteAccessControlSystem(DeleteAccessControlSystemParams parameters)
	{
		return base.Channel.DeleteAccessControlSystem(parameters);
	}

	public Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(DeleteAccessControlSystemParams parameters)
	{
		return base.Channel.DeleteAccessControlSystemAsync(parameters);
	}

	public GetAccessControlSystemConfigurationResp GetAccessControlSystemConfiguration(GetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.GetAccessControlSystemConfiguration(parameters);
	}

	public Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(GetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.GetAccessControlSystemConfigurationAsync(parameters);
	}

	public SetAccessControlSystemConfigurationResp SetAccessControlSystemConfiguration(SetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.SetAccessControlSystemConfiguration(parameters);
	}

	public Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(SetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.SetAccessControlSystemConfigurationAsync(parameters);
	}

	public StartFetchAccessControlSystemConfigurationResp StartFetchAccessControlSystemConfiguration(StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.StartFetchAccessControlSystemConfiguration(parameters);
	}

	public Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.StartFetchAccessControlSystemConfigurationAsync(parameters);
	}

	public CancelFetchAccessControlSystemConfigurationResp CancelFetchAccessControlSystemConfiguration(CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.CancelFetchAccessControlSystemConfiguration(parameters);
	}

	public Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.CancelFetchAccessControlSystemConfigurationAsync(parameters);
	}

	public GetCredentialHoldersResp GetCredentialHolders(GetCredentialHoldersParams parameters)
	{
		return base.Channel.GetCredentialHolders(parameters);
	}

	public Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(GetCredentialHoldersParams parameters)
	{
		return base.Channel.GetCredentialHoldersAsync(parameters);
	}

	public SetCredentialHolderImageResp SetCredentialHolderImage(SetCredentialHolderImageParams parameters)
	{
		return base.Channel.SetCredentialHolderImage(parameters);
	}

	public Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(SetCredentialHolderImageParams parameters)
	{
		return base.Channel.SetCredentialHolderImageAsync(parameters);
	}

	public SearchCredentialHoldersResp SearchCredentialHolders(SearchCredentialHoldersParams parameters)
	{
		return base.Channel.SearchCredentialHolders(parameters);
	}

	public Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(SearchCredentialHoldersParams parameters)
	{
		return base.Channel.SearchCredentialHoldersAsync(parameters);
	}

	public GetStatesResp GetStates(GetStatesParams parameters)
	{
		return base.Channel.GetStates(parameters);
	}

	public Task<GetStatesResp> GetStatesAsync(GetStatesParams parameters)
	{
		return base.Channel.GetStatesAsync(parameters);
	}

	public ExecuteCommandResp ExecuteCommand(ExecuteCommandParams parameters)
	{
		return base.Channel.ExecuteCommand(parameters);
	}

	public Task<ExecuteCommandResp> ExecuteCommandAsync(ExecuteCommandParams parameters)
	{
		return base.Channel.ExecuteCommandAsync(parameters);
	}

	public GetGlobalSettingsResp GetGlobalSettings(GetGlobalSettingsParams parameters)
	{
		return base.Channel.GetGlobalSettings(parameters);
	}

	public Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(GetGlobalSettingsParams parameters)
	{
		return base.Channel.GetGlobalSettingsAsync(parameters);
	}

	public SetGlobalSettingsResp SetGlobalSettings(SetGlobalSettingsParams parameters)
	{
		return base.Channel.SetGlobalSettings(parameters);
	}

	public Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(SetGlobalSettingsParams parameters)
	{
		return base.Channel.SetGlobalSettingsAsync(parameters);
	}

	public GetUserDefinedCategoriesResp GetUserDefinedCategories(GetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.GetUserDefinedCategories(parameters);
	}

	public Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(GetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.GetUserDefinedCategoriesAsync(parameters);
	}

	public SetUserDefinedCategoriesResp SetUserDefinedCategories(SetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.SetUserDefinedCategories(parameters);
	}

	public Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(SetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.SetUserDefinedCategoriesAsync(parameters);
	}

	public ValidateUserCredentialsResp ValidateUserCredentials(ValidateUserCredentialsParams parameters)
	{
		return base.Channel.ValidateUserCredentials(parameters);
	}

	public Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(ValidateUserCredentialsParams parameters)
	{
		return base.Channel.ValidateUserCredentialsAsync(parameters);
	}

	public GetAccessControlEventsResp GetAccessControlEvents(GetAccessControlEventsParams parameters)
	{
		return base.Channel.GetAccessControlEvents(parameters);
	}

	public Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(GetAccessControlEventsParams parameters)
	{
		return base.Channel.GetAccessControlEventsAsync(parameters);
	}

	public GetAccessControlEventsWithDowntimesResp GetAccessControlEventsWithDowntimes(GetAccessControlEventsWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventsWithDowntimes(parameters);
	}

	public Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(GetAccessControlEventsWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventsWithDowntimesAsync(parameters);
	}

	public GetAccessControlEventPageResp GetAccessControlEventPage(GetAccessControlEventPageParams parameters)
	{
		return base.Channel.GetAccessControlEventPage(parameters);
	}

	public Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(GetAccessControlEventPageParams parameters)
	{
		return base.Channel.GetAccessControlEventPageAsync(parameters);
	}

	public GetAccessControlEventPageWithDowntimesResp GetAccessControlEventPageWithDowntimes(GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventPageWithDowntimes(parameters);
	}

	public Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventPageWithDowntimesAsync(parameters);
	}

	public GetAccessControlEventCountResp GetAccessControlEventCount(GetAccessControlEventCountParams parameters)
	{
		return base.Channel.GetAccessControlEventCount(parameters);
	}

	public Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(GetAccessControlEventCountParams parameters)
	{
		return base.Channel.GetAccessControlEventCountAsync(parameters);
	}

	public GetDowntimesResp GetDowntimes(GetDowntimesParams parameters)
	{
		return base.Channel.GetDowntimes(parameters);
	}

	public Task<GetDowntimesResp> GetDowntimesAsync(GetDowntimesParams parameters)
	{
		return base.Channel.GetDowntimesAsync(parameters);
	}

	public Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(SetAccessControlUnitPositionParams parameters)
	{
		return base.Channel.SetAccessControlUnitPositionAsync(parameters);
	}
}
