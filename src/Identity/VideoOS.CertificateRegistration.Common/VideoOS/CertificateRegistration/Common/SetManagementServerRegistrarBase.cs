using System;
using System.Net;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Common.EnvironmentServices.Web;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.CertificateRegistration.Common;

[CLSCompliant(false)]
public abstract class SetManagementServerRegistrarBase : WindowsServiceConfiguratorBase, ISetServerAddress, IClientCleanup
{
	private readonly IWebRequestFactory _webRequestFactory;

	protected SetManagementServerRegistrarBase(string serviceName, TimeSpan timeout, IServiceControllerFactory serviceControllerFactory, IWebRequestFactory webRequestFactory)
		: base(serviceName, timeout, serviceControllerFactory)
	{
		_webRequestFactory = webRequestFactory;
	}

	public Status CleanUp()
	{
		try
		{
			return LoggedIfSuccessOrWarning(ExecuteCleanup(), "CleanUp");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "CleanUp");
		}
	}

	protected virtual Status ExecuteCleanup()
	{
		return Status.SuccessStatus();
	}

	public Status PostSetServerAddress()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PostExecute(), "PostSetServerAddress");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PostSetServerAddress");
		}
	}

	public Status PreSetServerAddress()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreSetServerAddress");
		}
		catch (ServiceConfigurationException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "PreSetServerAddress");
		}
	}

	public Status SetServerAddress(Uri newManagementServerAddress)
	{
		if (newManagementServerAddress == null)
		{
			throw new ArgumentNullException("newManagementServerAddress");
		}
		try
		{
			Status status = VerifyServerIisConnection(newManagementServerAddress);
			if (!status.StatusCode.IsSuccess())
			{
				return LoggedStatus(status, FormattableString.Invariant($"Unable to verify the presence of the management server ({newManagementServerAddress})"), "SetServerAddress");
			}
			return LoggedIfSuccessOrWarning(Execute(newManagementServerAddress), "SetServerAddress");
		}
		catch (Exception exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "SetServerAddress");
		}
	}

	protected virtual Status VerifyServerIisConnection(Uri newManagementServerAddress)
	{
		UriBuilder uriBuilder = new UriBuilder(newManagementServerAddress)
		{
			Path = "/ManagementServer/ServiceRegistrationService.svc"
		};
		IWebResponse webResponse = null;
		try
		{
			IWebRequest webRequest = _webRequestFactory.Create(uriBuilder.Uri);
			webRequest.UseDefaultCredentials = true;
			webResponse = webRequest.GetResponse();
		}
		catch (VideoOS.Common.EnvironmentServices.Web.WebException ex)
		{
			if (ex.Response == null)
			{
				base.Logger?.LogWarning(FormattableString.Invariant($"Could not verify server iis connection. Exception: {ex}"));
				return Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration);
			}
			webResponse = ex.Response;
		}
		if (webResponse is IHttpWebResponse httpWebResponse)
		{
			if (httpWebResponse.StatusCode == HttpStatusCode.OK || httpWebResponse.StatusCode == HttpStatusCode.Forbidden || httpWebResponse.StatusCode == HttpStatusCode.Unauthorized)
			{
				base.Logger?.LogInfo(FormattableString.Invariant($"The response from {newManagementServerAddress} indicates that it is present"));
				return Status.SuccessStatus();
			}
			if (webResponse == null || httpWebResponse.StatusCode == HttpStatusCode.NotFound)
			{
				return Status.ErrorStatus(StatusCode.ManagementServerNotFound);
			}
		}
		base.Logger?.LogWarning(FormattableString.Invariant($"The response  was {webResponse}."));
		return Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration);
	}

	protected abstract Status Execute(Uri newManagementServerAddress);
}
