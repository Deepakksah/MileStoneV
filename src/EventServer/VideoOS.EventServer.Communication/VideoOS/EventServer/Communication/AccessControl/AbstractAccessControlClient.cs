using System;
using System.ServiceModel;
using System.Threading.Tasks;
using VideoOS.EventServer.Communication.AccessControl.Client;
using VideoOS.Platform;
using VideoOS.Platform.AccessControl.Internal.Data;

namespace VideoOS.EventServer.Communication.AccessControl;

public abstract class AbstractAccessControlClient<T> : AccessControlClient, IDisposable where T : class, ICommunicationObject
{
	private const int DefaultRetryCount = 2;

	private readonly ClientPool<T> _clientPool;

	protected ServerId ServerId { get; private set; }

	protected Uri Uri { get; private set; }

	protected Version EventServerVersion { get; private set; }

	protected AbstractAccessControlClient(ServerId serverId, Version eventServerVersion, Uri uri)
	{
		ServerId = serverId;
		EventServerVersion = eventServerVersion;
		Uri = uri;
		_clientPool = new ClientPool<T>(CreateClient, 2);
	}

	protected abstract T CreateClient();

	protected async Task CallWithExceptionHandlingAsync(Func<T, Task> method)
	{
		try
		{
			await _clientPool.CallWithRetryAsync(method).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (FaultException<AccessControlServiceFault> error)
		{
			throw AccessControlFaultExceptionHandler.HandleException(error);
		}
	}

	protected async Task CallWithExceptionHandling(Func<T, Task> method, int retryCount)
	{
		try
		{
			await _clientPool.CallWithRetryAsync(method, retryCount).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (FaultException<AccessControlServiceFault> error)
		{
			throw AccessControlFaultExceptionHandler.HandleException(error);
		}
	}

	protected async Task<TRet> CallWithExceptionHandling<TRet>(Func<T, Task<TRet>> method) where TRet : class
	{
		try
		{
			return await _clientPool.CallWithRetryAsync(method).ConfigureAwait(continueOnCapturedContext: false);
		}
		catch (FaultException<AccessControlServiceFault> error)
		{
			throw AccessControlFaultExceptionHandler.HandleException(error);
		}
	}

	public abstract Task<GetAccessControlPluginsResp> GetAccessControlPluginsAsync(GetAccessControlPluginsParams parameters);

	public abstract Task<GetAccessControlPluginIconsResp> GetAccessControlPluginIconsAsync(GetAccessControlPluginIconsParams parameters);

	public abstract Task<GetAccessControlPluginCategoriesResp> GetAccessControlPluginCategoriesAsync(GetAccessControlPluginCategoriesParams parameters);

	public abstract Task<GetAccessControlSystemsResp> GetAccessControlSystemsAsync(GetAccessControlSystemsParams parameters);

	public abstract Task<ValidateAccessControlSystemPropertiesResp> ValidateAccessControlSystemPropertiesAsync(ValidateAccessControlSystemPropertiesParams parameters);

	public abstract Task<CreateAccessControlSystemResp> CreateAccessControlSystemAsync(CreateAccessControlSystemParams parameters);

	public abstract Task<DeleteAccessControlSystemResp> DeleteAccessControlSystemAsync(DeleteAccessControlSystemParams parameters);

	public abstract Task<GetAccessControlSystemConfigurationResp> GetAccessControlSystemConfigurationAsync(GetAccessControlSystemConfigurationParams parameters);

	public abstract Task<SetAccessControlSystemConfigurationResp> SetAccessControlSystemConfigurationAsync(SetAccessControlSystemConfigurationParams parameters);

	public abstract Task<StartFetchAccessControlSystemConfigurationResp> StartFetchAccessControlSystemConfigurationAsync(StartFetchAccessControlSystemConfigurationParams parameters);

	public abstract Task<CancelFetchAccessControlSystemConfigurationResp> CancelFetchAccessControlSystemConfigurationAsync(CancelFetchAccessControlSystemConfigurationParams parameters);

	public abstract Task<GetCredentialHoldersResp> GetCredentialHoldersAsync(GetCredentialHoldersParams parameters);

	public abstract Task<SetCredentialHolderImageResp> SetCredentialHolderImageAsync(SetCredentialHolderImageParams parameters);

	public abstract Task<SearchCredentialHoldersResp> SearchCredentialHoldersAsync(SearchCredentialHoldersParams parameters);

	public abstract Task<GetStatesResp> GetStatesAsync(GetStatesParams parameters);

	public abstract Task<ExecuteCommandResp> ExecuteCommandAsync(ExecuteCommandParams parameters);

	public abstract Task<GetGlobalSettingsResp> GetGlobalSettingsAsync(GetGlobalSettingsParams parameters);

	public abstract Task<SetGlobalSettingsResp> SetGlobalSettingsAsync(SetGlobalSettingsParams parameters);

	public abstract Task<GetUserDefinedCategoriesResp> GetUserDefinedCategoriesAsync(GetUserDefinedCategoriesParams parameters);

	public abstract Task<SetUserDefinedCategoriesResp> SetUserDefinedCategoriesAsync(SetUserDefinedCategoriesParams parameters);

	public abstract Task<ValidateUserCredentialsResp> ValidateUserCredentialsAsync(ValidateUserCredentialsParams parameters);

	public abstract Task<GetAccessControlEventsResp> GetAccessControlEventsAsync(GetAccessControlEventsParams parameters);

	public abstract Task<GetAccessControlEventsWithDowntimesResp> GetAccessControlEventsWithDowntimesAsync(GetAccessControlEventsWithDowntimesParams parameters);

	public abstract Task<GetAccessControlEventPageResp> GetAccessControlEventPageAsync(GetAccessControlEventPageParams parameters);

	public abstract Task<GetAccessControlEventPageWithDowntimesResp> GetAccessControlEventPageWithDowntimesAsync(GetAccessControlEventPageWithDowntimesParams parameters);

	public abstract Task<GetAccessControlEventCountResp> GetAccessControlEventCountAsync(GetAccessControlEventCountParams parameters);

	public abstract Task<GetDowntimesResp> GetDowntimesAsync(GetDowntimesParams parameters);

	public abstract Task<SetAccessControlUnitPositionResp> SetAccessControlUnitPositionAsync(SetAccessControlUnitPositionParams parameters);

	protected EndpointIdentity CreateSpnIdentity(string spnName)
	{
		return EndpointIdentity.CreateSpnIdentity(spnName);
	}

	public void Dispose()
	{
		Dispose(disposing: true);
		GC.SuppressFinalize(this);
	}

	protected virtual void Dispose(bool disposing)
	{
		if (disposing)
		{
			_clientPool.Dispose();
		}
	}
}
