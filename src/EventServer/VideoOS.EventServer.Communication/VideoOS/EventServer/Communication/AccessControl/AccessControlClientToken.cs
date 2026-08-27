using System;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.AccessControl.Client;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Platform;
using VideoOS.Platform.AccessControl.Internal.Data;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AccessControl;

public class AccessControlClientToken : AbstractAccessControlClient<AccessControlCommandTokenClient>
{
	private readonly TimeSpan _requestTimeout;

	public AccessControlClientToken(ServerId serverId, Version eventServerVersion, Uri uri)
		: this(serverId, eventServerVersion, uri, TimeSpan.FromMinutes(1.0))
	{
	}

	public AccessControlClientToken(ServerId serverId, Version eventServerVersion, Uri uri, TimeSpan requestTimeout)
		: base(serverId, eventServerVersion, uri)
	{
		_requestTimeout = requestTimeout;
	}

	private string GetToken()
	{
		return (LoginSettingsCache.GetLoginSettings(base.ServerId) ?? throw new LoginSettingsNotFoundException(base.ServerId)).Token;
	}

	protected override AccessControlCommandTokenClient CreateClient()
	{
		BasicHttpBinding basicHttpBinding = BindingFactory.CreateBasicHttpBindingToken(base.Uri.IsHttps());
		basicHttpBinding.SendTimeout = _requestTimeout;
		UriBuilder uriBuilder = new UriBuilder(base.Uri)
		{
			Path = "Central/AccessControlServiceToken"
		};
		string spnName = "AccessControlServiceToken";
		AccessControlCommandTokenClient accessControlCommandTokenClient = new AccessControlCommandTokenClient(basicHttpBinding, new EndpointAddress(uriBuilder.Uri, CreateSpnIdentity(spnName)));
		accessControlCommandTokenClient.EnsureLargeObjectGraphsAllowed<AccessControlCommandTokenClient, IAccessControlCommandToken>();
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(accessControlCommandTokenClient);
		return accessControlCommandTokenClient;
	}

	public override async Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(GetAccessControlPluginsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlPluginsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(GetAccessControlPluginIconsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlPluginIconsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(GetAccessControlPluginCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlPluginCategoriesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(GetAccessControlSystemsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlSystemsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(ValidateAccessControlSystemPropertiesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.ValidateAccessControlSystemPropertiesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(CreateAccessControlSystemParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.CreateAccessControlSystemAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(DeleteAccessControlSystemParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.DeleteAccessControlSystemAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(GetAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlSystemConfigurationAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(SetAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SetAccessControlSystemConfigurationAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.StartFetchAccessControlSystemConfigurationAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.CancelFetchAccessControlSystemConfigurationAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(GetCredentialHoldersParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetCredentialHoldersAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(SetCredentialHolderImageParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SetCredentialHolderImageAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(SearchCredentialHoldersParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SearchCredentialHoldersAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetStatesResp> GetStatesAsync(GetStatesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetStatesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ExecuteCommandResp> ExecuteCommandAsync(ExecuteCommandParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.ExecuteCommandAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(GetGlobalSettingsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetGlobalSettingsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(SetGlobalSettingsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SetGlobalSettingsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(GetUserDefinedCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetUserDefinedCategoriesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(SetUserDefinedCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SetUserDefinedCategoriesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(ValidateUserCredentialsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.ValidateUserCredentialsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(GetAccessControlEventsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlEventsAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(GetAccessControlEventsWithDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlEventsWithDowntimesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(GetAccessControlEventPageParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlEventPageAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlEventPageWithDowntimesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(GetAccessControlEventCountParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetAccessControlEventCountAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetDowntimesResp> GetDowntimesAsync(GetDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.GetDowntimesAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(SetAccessControlUnitPositionParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandTokenClient client) => client.SetAccessControlUnitPositionAsync(GetToken(), parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}
}
