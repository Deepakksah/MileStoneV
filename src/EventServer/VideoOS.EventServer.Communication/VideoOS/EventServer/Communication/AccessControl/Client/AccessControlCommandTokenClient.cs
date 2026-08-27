using System.CodeDom.Compiler;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.ServiceModel", "4.0.0.0")]
public class AccessControlCommandTokenClient : ClientBase<IAccessControlCommandToken>, IAccessControlCommandToken
{
	public AccessControlCommandTokenClient()
	{
	}

	public AccessControlCommandTokenClient(string endpointConfigurationName)
		: base(endpointConfigurationName)
	{
	}

	public AccessControlCommandTokenClient(string endpointConfigurationName, string remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AccessControlCommandTokenClient(string endpointConfigurationName, EndpointAddress remoteAddress)
		: base(endpointConfigurationName, remoteAddress)
	{
	}

	public AccessControlCommandTokenClient(Binding binding, EndpointAddress remoteAddress)
		: base(binding, remoteAddress)
	{
	}

	public GetAccessControlPluginsResp GetAccessControlPlugins(string token, GetAccessControlPluginsParams parameters)
	{
		return base.Channel.GetAccessControlPlugins(token, parameters);
	}

	public Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(string token, GetAccessControlPluginsParams parameters)
	{
		return base.Channel.GetAccessControlPluginsAsync(token, parameters);
	}

	public GetAccessControlPluginIconsResp GetAccessControlPluginIcons(string token, GetAccessControlPluginIconsParams parameters)
	{
		return base.Channel.GetAccessControlPluginIcons(token, parameters);
	}

	public Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(string token, GetAccessControlPluginIconsParams parameters)
	{
		return base.Channel.GetAccessControlPluginIconsAsync(token, parameters);
	}

	public GetAccessControlPluginCategoriesResp GetAccessControlPluginCategories(string token, GetAccessControlPluginCategoriesParams parameters)
	{
		return base.Channel.GetAccessControlPluginCategories(token, parameters);
	}

	public Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(string token, GetAccessControlPluginCategoriesParams parameters)
	{
		return base.Channel.GetAccessControlPluginCategoriesAsync(token, parameters);
	}

	public GetAccessControlSystemsResp GetAccessControlSystems(string token, GetAccessControlSystemsParams parameters)
	{
		return base.Channel.GetAccessControlSystems(token, parameters);
	}

	public Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(string token, GetAccessControlSystemsParams parameters)
	{
		return base.Channel.GetAccessControlSystemsAsync(token, parameters);
	}

	public ValidateAccessControlSystemPropertiesResp ValidateAccessControlSystemProperties(string token, ValidateAccessControlSystemPropertiesParams parameters)
	{
		return base.Channel.ValidateAccessControlSystemProperties(token, parameters);
	}

	public Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(string token, ValidateAccessControlSystemPropertiesParams parameters)
	{
		return base.Channel.ValidateAccessControlSystemPropertiesAsync(token, parameters);
	}

	public CreateAccessControlSystemResp CreateAccessControlSystem(string token, CreateAccessControlSystemParams parameters)
	{
		return base.Channel.CreateAccessControlSystem(token, parameters);
	}

	public Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(string token, CreateAccessControlSystemParams parameters)
	{
		return base.Channel.CreateAccessControlSystemAsync(token, parameters);
	}

	public DeleteAccessControlSystemResp DeleteAccessControlSystem(string token, DeleteAccessControlSystemParams parameters)
	{
		return base.Channel.DeleteAccessControlSystem(token, parameters);
	}

	public Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(string token, DeleteAccessControlSystemParams parameters)
	{
		return base.Channel.DeleteAccessControlSystemAsync(token, parameters);
	}

	public GetAccessControlSystemConfigurationResp GetAccessControlSystemConfiguration(string token, GetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.GetAccessControlSystemConfiguration(token, parameters);
	}

	public Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(string token, GetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.GetAccessControlSystemConfigurationAsync(token, parameters);
	}

	public SetAccessControlSystemConfigurationResp SetAccessControlSystemConfiguration(string token, SetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.SetAccessControlSystemConfiguration(token, parameters);
	}

	public Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(string token, SetAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.SetAccessControlSystemConfigurationAsync(token, parameters);
	}

	public StartFetchAccessControlSystemConfigurationResp StartFetchAccessControlSystemConfiguration(string token, StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.StartFetchAccessControlSystemConfiguration(token, parameters);
	}

	public Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(string token, StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.StartFetchAccessControlSystemConfigurationAsync(token, parameters);
	}

	public CancelFetchAccessControlSystemConfigurationResp CancelFetchAccessControlSystemConfiguration(string token, CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.CancelFetchAccessControlSystemConfiguration(token, parameters);
	}

	public Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(string token, CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return base.Channel.CancelFetchAccessControlSystemConfigurationAsync(token, parameters);
	}

	public GetCredentialHoldersResp GetCredentialHolders(string token, GetCredentialHoldersParams parameters)
	{
		return base.Channel.GetCredentialHolders(token, parameters);
	}

	public Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(string token, GetCredentialHoldersParams parameters)
	{
		return base.Channel.GetCredentialHoldersAsync(token, parameters);
	}

	public SetCredentialHolderImageResp SetCredentialHolderImage(string token, SetCredentialHolderImageParams parameters)
	{
		return base.Channel.SetCredentialHolderImage(token, parameters);
	}

	public Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(string token, SetCredentialHolderImageParams parameters)
	{
		return base.Channel.SetCredentialHolderImageAsync(token, parameters);
	}

	public SearchCredentialHoldersResp SearchCredentialHolders(string token, SearchCredentialHoldersParams parameters)
	{
		return base.Channel.SearchCredentialHolders(token, parameters);
	}

	public Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(string token, SearchCredentialHoldersParams parameters)
	{
		return base.Channel.SearchCredentialHoldersAsync(token, parameters);
	}

	public GetStatesResp GetStates(string token, GetStatesParams parameters)
	{
		return base.Channel.GetStates(token, parameters);
	}

	public Task<GetStatesResp> GetStatesAsync(string token, GetStatesParams parameters)
	{
		return base.Channel.GetStatesAsync(token, parameters);
	}

	public ExecuteCommandResp ExecuteCommand(string token, ExecuteCommandParams parameters)
	{
		return base.Channel.ExecuteCommand(token, parameters);
	}

	public Task<ExecuteCommandResp> ExecuteCommandAsync(string token, ExecuteCommandParams parameters)
	{
		return base.Channel.ExecuteCommandAsync(token, parameters);
	}

	public GetGlobalSettingsResp GetGlobalSettings(string token, GetGlobalSettingsParams parameters)
	{
		return base.Channel.GetGlobalSettings(token, parameters);
	}

	public Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(string token, GetGlobalSettingsParams parameters)
	{
		return base.Channel.GetGlobalSettingsAsync(token, parameters);
	}

	public SetGlobalSettingsResp SetGlobalSettings(string token, SetGlobalSettingsParams parameters)
	{
		return base.Channel.SetGlobalSettings(token, parameters);
	}

	public Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(string token, SetGlobalSettingsParams parameters)
	{
		return base.Channel.SetGlobalSettingsAsync(token, parameters);
	}

	public GetUserDefinedCategoriesResp GetUserDefinedCategories(string token, GetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.GetUserDefinedCategories(token, parameters);
	}

	public Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(string token, GetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.GetUserDefinedCategoriesAsync(token, parameters);
	}

	public SetUserDefinedCategoriesResp SetUserDefinedCategories(string token, SetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.SetUserDefinedCategories(token, parameters);
	}

	public Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(string token, SetUserDefinedCategoriesParams parameters)
	{
		return base.Channel.SetUserDefinedCategoriesAsync(token, parameters);
	}

	public ValidateUserCredentialsResp ValidateUserCredentials(string token, ValidateUserCredentialsParams parameters)
	{
		return base.Channel.ValidateUserCredentials(token, parameters);
	}

	public Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(string token, ValidateUserCredentialsParams parameters)
	{
		return base.Channel.ValidateUserCredentialsAsync(token, parameters);
	}

	public GetAccessControlEventsResp GetAccessControlEvents(string token, GetAccessControlEventsParams parameters)
	{
		return base.Channel.GetAccessControlEvents(token, parameters);
	}

	public Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(string token, GetAccessControlEventsParams parameters)
	{
		return base.Channel.GetAccessControlEventsAsync(token, parameters);
	}

	public GetAccessControlEventsWithDowntimesResp GetAccessControlEventsWithDowntimes(string token, GetAccessControlEventsWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventsWithDowntimes(token, parameters);
	}

	public Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(string token, GetAccessControlEventsWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventsWithDowntimesAsync(token, parameters);
	}

	public GetAccessControlEventPageResp GetAccessControlEventPage(string token, GetAccessControlEventPageParams parameters)
	{
		return base.Channel.GetAccessControlEventPage(token, parameters);
	}

	public Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(string token, GetAccessControlEventPageParams parameters)
	{
		return base.Channel.GetAccessControlEventPageAsync(token, parameters);
	}

	public GetAccessControlEventPageWithDowntimesResp GetAccessControlEventPageWithDowntimes(string token, GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventPageWithDowntimes(token, parameters);
	}

	public Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(string token, GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return base.Channel.GetAccessControlEventPageWithDowntimesAsync(token, parameters);
	}

	public GetAccessControlEventCountResp GetAccessControlEventCount(string token, GetAccessControlEventCountParams parameters)
	{
		return base.Channel.GetAccessControlEventCount(token, parameters);
	}

	public Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(string token, GetAccessControlEventCountParams parameters)
	{
		return base.Channel.GetAccessControlEventCountAsync(token, parameters);
	}

	public GetDowntimesResp GetDowntimes(string token, GetDowntimesParams parameters)
	{
		return base.Channel.GetDowntimes(token, parameters);
	}

	public Task<GetDowntimesResp> GetDowntimesAsync(string token, GetDowntimesParams parameters)
	{
		return base.Channel.GetDowntimesAsync(token, parameters);
	}

	public Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(string token, SetAccessControlUnitPositionParams parameters)
	{
		return base.Channel.SetAccessControlUnitPositionAsync(token, parameters);
	}
}
