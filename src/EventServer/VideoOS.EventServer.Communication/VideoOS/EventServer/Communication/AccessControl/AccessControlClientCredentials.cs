using System;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.AccessControl.Client;
using VideoOS.EventServer.Communication.Extensions;
using VideoOS.EventServer.Communication.Headers;
using VideoOS.Management.Common;
using VideoOS.Platform;
using VideoOS.Platform.AccessControl.Internal.Data;
using VideoOS.Platform.Login;

namespace VideoOS.EventServer.Communication.AccessControl;

public class AccessControlClientCredentials : AbstractAccessControlClient<AccessControlCommandCredentialsClient>
{
	private readonly TimeSpan _requestTimeout;

	public AccessControlClientCredentials(ServerId serverId, Version eventServerVersion, Uri uri)
		: this(serverId, eventServerVersion, uri, TimeSpan.FromMinutes(1.0))
	{
	}

	public AccessControlClientCredentials(ServerId serverId, Version eventServerVersion, Uri uri, TimeSpan requestTimeout)
		: base(serverId, eventServerVersion, uri)
	{
		_requestTimeout = requestTimeout;
	}

	protected override AccessControlCommandCredentialsClient CreateClient()
	{
		WSHttpBinding wSHttpBinding = BindingFactory.CreateWSHttpBindingCredentials(base.Uri.IsHttps());
		wSHttpBinding.SendTimeout = _requestTimeout;
		UriBuilder uriBuilder = new UriBuilder(base.Uri)
		{
			Path = "Central/AccessControlServiceCredentials"
		};
		AccessControlCommandCredentialsClient accessControlCommandCredentialsClient = new AccessControlCommandCredentialsClient(wSHttpBinding, new EndpointAddress(identity: CreateSpnIdentity(SpnFactory.GetSpn(uriBuilder.Uri)), uri: uriBuilder.Uri, addressHeaders: Array.Empty<AddressHeader>()));
		LoginSettings loginSettings = LoginSettingsCache.GetLoginSettings(base.ServerId);
		if (loginSettings == null)
		{
			throw new LoginSettingsNotFoundException(base.ServerId);
		}
		accessControlCommandCredentialsClient.ClientCredentials.Windows.ClientCredential = loginSettings.NetworkCredential;
		accessControlCommandCredentialsClient.EnsureLargeObjectGraphsAllowed<AccessControlCommandCredentialsClient, IAccessControlCommandCredentials>();
		ClientSessionIdHeaderHelper.AddClientSessionIdHeaderBehavior(accessControlCommandCredentialsClient);
		return accessControlCommandCredentialsClient;
	}

	public override async Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(GetAccessControlPluginsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlPluginsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(GetAccessControlPluginIconsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlPluginIconsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(GetAccessControlPluginCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlPluginCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(GetAccessControlSystemsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlSystemsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(ValidateAccessControlSystemPropertiesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.ValidateAccessControlSystemPropertiesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(CreateAccessControlSystemParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.CreateAccessControlSystemAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(DeleteAccessControlSystemParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.DeleteAccessControlSystemAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(GetAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(SetAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SetAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(StartFetchAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.StartFetchAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(CancelFetchAccessControlSystemConfigurationParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.CancelFetchAccessControlSystemConfigurationAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(GetCredentialHoldersParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetCredentialHoldersAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(SetCredentialHolderImageParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SetCredentialHolderImageAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(SearchCredentialHoldersParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SearchCredentialHoldersAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetStatesResp> GetStatesAsync(GetStatesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetStatesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ExecuteCommandResp> ExecuteCommandAsync(ExecuteCommandParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.ExecuteCommandAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(GetGlobalSettingsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetGlobalSettingsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(SetGlobalSettingsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SetGlobalSettingsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(GetUserDefinedCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetUserDefinedCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(SetUserDefinedCategoriesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SetUserDefinedCategoriesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(ValidateUserCredentialsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.ValidateUserCredentialsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(GetAccessControlEventsParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlEventsAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(GetAccessControlEventsWithDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlEventsWithDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(GetAccessControlEventPageParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlEventPageAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(GetAccessControlEventPageWithDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlEventPageWithDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(GetAccessControlEventCountParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetAccessControlEventCountAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<GetDowntimesResp> GetDowntimesAsync(GetDowntimesParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.GetDowntimesAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}

	public override async Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(SetAccessControlUnitPositionParams parameters)
	{
		return await CallWithExceptionHandling((AccessControlCommandCredentialsClient client) => client.SetAccessControlUnitPositionAsync(parameters)).ConfigureAwait(continueOnCapturedContext: false);
	}
}
