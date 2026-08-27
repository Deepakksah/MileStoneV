using System;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Management;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Security.AccessControl;
using System.Security.Authentication;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoOS.Common.Crypto;
using VideoOS.Common.EnvironmentServices;
using VideoOS.IdentityServer.Common;
using VideoOS.IdentityServer.Common.Exceptions;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.Utilities.Security;

namespace VideoOS.CertificateRegistration.Common;

public abstract class ClientEndpointRegistrarBase : WindowsServiceConfiguratorBase, IClientRegistrationV2, IClientCleanup
{
	private const int WAIT_FOR_SERVICE_TIMEOUT = 180;

	protected IRegistrationParametersHandler ParametersHandler;

	private readonly ISecretProvider _secretProvider;

	private readonly IAccount _account;

	private readonly string _serviceName;

	private readonly IRegistrationClientFactory _registrationClientFactory;

	protected ClientEndpointRegistrarBase(IRegistrationParametersHandler parametersHandler, ISecretProvider secretProvider, string serviceName)
		: this(new RegistrationClientFactory(), parametersHandler, secretProvider, serviceName, handleServiceStartStop: true, new AccountWrapper())
	{
	}

	protected ClientEndpointRegistrarBase(IRegistrationParametersHandler parametersHandler, ISecretProvider secretProvider, string serviceName, bool handleServiceStartStop)
		: this(new RegistrationClientFactory(), parametersHandler, secretProvider, serviceName, handleServiceStartStop, new AccountWrapper())
	{
	}

	protected ClientEndpointRegistrarBase(IRegistrationClientFactory registrationClientFactory, IRegistrationParametersHandler parametersHandler, ISecretProvider secretProvider, string serviceName, bool handleServiceStartStop, IAccount account)
		: base(handleServiceStartStop ? serviceName : null, new TimeSpan(0, 0, 180), new ServiceControllerFactory())
	{
		_registrationClientFactory = registrationClientFactory ?? throw new ArgumentNullException("registrationClientFactory");
		ParametersHandler = parametersHandler ?? throw new ArgumentNullException("parametersHandler");
		_secretProvider = secretProvider ?? throw new ArgumentNullException("secretProvider");
		_account = account ?? throw new ArgumentNullException("account");
		if (serviceName == null)
		{
			throw new ArgumentNullException("serviceName");
		}
		if (string.IsNullOrEmpty(serviceName))
		{
			throw new ArgumentException("serviceName");
		}
		_serviceName = serviceName;
	}

	public Status PreRegisterClient()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PreExecute(), "PreRegisterClient");
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			return LoggedErrorStatus(ex, StatusCode.UnspecifiedErrorRegistration, null, "PreRegisterClient");
		}
	}

	public Status RegisterClient(Credential identityServerCredentials, Uri authorizationService)
	{
		try
		{
			base.Logger?.LogInfo(FormattableString.Invariant(FormattableStringFactory.Create("Register Client to {0} as {1}", authorizationService, identityServerCredentials?.UserName ?? "Current User")));
			return LoggedIfSuccessOrWarning(Execute(identityServerCredentials, authorizationService), "RegisterClient");
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			Status status = ResolveAndLogException(ex, "RegisterClient");
			return LoggedErrorStatus(ex, status.StatusCode, null, "RegisterClient");
		}
	}

	protected virtual string GetAccountUser()
	{
		return _account.GetServiceLogOnUser(_serviceName);
	}

	protected virtual Status Execute(NetworkCredential identityServerCredentials, Uri authorizationService)
	{
		if (authorizationService == null)
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), "The authorizationService parameter cannot be null", "Execute");
		}
		RegistrationParameters parameters = ParametersHandler.ReadRegistrationParameters(authorizationService);
		IRegistrationClient client = _registrationClientFactory.Create(parameters.AuthorizationServerAddress, identityServerCredentials);
		try
		{
			base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - About to register client: \"{parameters.AuthorizationServerAddress}\""));
			RegisterClientResponse registerClientResponse = RegisterClientWithRetry(() => client.RegisterClient(parameters.ClientId.ToString(), parameters.ClientName, parameters.Scopes), base.Logger);
			base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - register client finished"));
			FileSystemAccessRule rule = new FileSystemAccessRule(GetAccountUser(), FileSystemRights.Read | FileSystemRights.Write | FileSystemRights.Delete, AccessControlType.Allow);
			using (SecretHandler secretHandler = new SecretHandler(_secretProvider))
			{
				secretHandler.WriteSecret(Encoding.UTF8.GetBytes(registerClientResponse.Secret), rule);
			}
			base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - write registration secret finished"));
		}
		finally
		{
			if (client != null)
			{
				client.Dispose();
			}
		}
		if (!ParametersHandler.WriteRegistrationParameters(parameters))
		{
			return LoggedStatus(Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), null, "Execute");
		}
		base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - update config finished"));
		return Status.SuccessStatus();
	}

	public static RegisterClientResponse RegisterClientWithRetry(Func<Task<RegisterClientResponse>> registerClient, ILogger logger)
	{
		RegisterClientResponse registerClientResponse = null;
		Exception ex = null;
		Stopwatch stopwatch = Stopwatch.StartNew();
		long num = 30000L;
		int millisecondsTimeout = 100;
		bool flag = true;
		bool flag2 = false;
		while (flag || (registerClientResponse == null && stopwatch.ElapsedMilliseconds < num))
		{
			if (!flag)
			{
				logger?.LogInfo(FormattableString.Invariant($"Retrying Client registration - Remaining time to timeout: {num - stopwatch.ElapsedMilliseconds} - {ex?.GetType()}:{ex?.Message}"));
				Thread.Sleep(millisecondsTimeout);
			}
			flag = false;
			try
			{
				Task<RegisterClientResponse> task = registerClient();
				task.ConfigureAwait(continueOnCapturedContext: false);
				registerClientResponse = task.Result;
			}
			catch (AggregateException ex2) when (ex2.InnerException is TaskCanceledException)
			{
				if (!flag2)
				{
					logger?.LogInfo("Retrying Client registration timeout has been has been extended");
				}
				flag2 = true;
				num = 600000L;
				millisecondsTimeout = 10000;
			}
			catch (AggregateException ex3) when (ex3.InnerException is ForbiddenException || ex3.InnerException is AdminIdentityNotFoundException || ex3.InnerException is UnauthorizedException)
			{
				ex = ex3;
				break;
			}
			catch (AggregateException ex4)
			{
				ex = ex4.InnerException ?? ex4;
			}
			catch (Exception ex5)
			{
				ex = ex5;
			}
		}
		if (registerClientResponse != null)
		{
			return registerClientResponse;
		}
		throw ex ?? new InvalidOperationException("ClientRegistration failed, Response was null");
	}

	public Status PostRegisterClient()
	{
		try
		{
			return LoggedIfSuccessOrWarning(PostExecute(), "PostRegisterClient");
		}
		catch (Exception ex) when (!(ex is OutOfMemoryException))
		{
			return LoggedErrorStatus(ex, StatusCode.UnspecifiedErrorRegistration, null, "PostRegisterClient");
		}
	}

	public Status CleanUp()
	{
		AdditionalCleanup();
		try
		{
			return LoggedIfSuccessOrWarning(ExecuteCleanup(), "CleanUp");
		}
		catch (RemoveFileSecretException exception)
		{
			return LoggedErrorStatus(exception, StatusCode.UnspecifiedErrorRegistration, null, "CleanUp");
		}
	}

	protected virtual void AdditionalCleanup()
	{
	}

	protected virtual Status ExecuteCleanup()
	{
		base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - About to remove secret"));
		RemoveIdentityProviderSecret();
		base.Logger?.LogInfo(FormattableString.Invariant($"'{_serviceName} - Secret has been removed"));
		return Status.SuccessStatus();
	}

	private void RemoveIdentityProviderSecret()
	{
		_secretProvider.DeleteTempSecret();
		_secretProvider.DeletePermSecret();
	}

	private Status ResolveAndLogException(Exception ex, [CallerMemberName] string action = "")
	{
		var (status, description) = ResolveException(ex, out var _);
		return LoggedStatus(status, description, action);
	}

	public static (Status status, string message) ResolveException(Exception ex, out bool handled)
	{
		handled = true;
		if (ex is AggregateException)
		{
			if (ex.InnerException != null)
			{
				return ResolveException(ex.InnerException, out handled);
			}
			return (status: Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), message: "Registration failed, Exception = " + ex);
		}
		if (ex is UnauthorizedException || ex is ForbiddenException)
		{
			return (status: Status.ErrorStatus(StatusCode.VmsAdminCredentialsNeeded), message: "You are not allowed to perform the requested action, Exception = " + ex);
		}
		if (ex is UriFormatException)
		{
			return (status: Status.ErrorStatus(StatusCode.AuthorizationServerAddressInvalid), message: "Hostname is invalid, Exception = " + ex);
		}
		if (ex is TaskCanceledException)
		{
			return (status: Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), message: "Registration failed, Exception = " + ex);
		}
		if (ex is IdpException ex2)
		{
			return ResolveUnsuccessfulHttpStatusException(ex2);
		}
		HttpRequestException ex3 = (HttpRequestException)(object)((ex is HttpRequestException) ? ex : null);
		if (ex3 != null)
		{
			return ResolveHttpRequestException(ex3);
		}
		if (ex is SecretException)
		{
			return (status: Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), message: "Unable to save Secret, Exception = " + ex);
		}
		if (ex is ManagementException)
		{
			return (status: Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), message: "Registration failed, Exception = " + ex);
		}
		handled = false;
		return (status: Status.ErrorStatus(StatusCode.UnspecifiedErrorRegistration), message: "Registration failed, Exception = " + ex);
	}

	private static (Status status, string message) ResolveUnsuccessfulHttpStatusException(IdpException ex)
	{
		string item;
		StatusCode statusCode;
		if (ex.StatusCode == 401 || ex.StatusCode == 403)
		{
			item = "You are not allowed to perform the requested action, registration failed with error code: " + ex.StatusCode.ToString(CultureInfo.InvariantCulture);
			statusCode = StatusCode.VmsAdminCredentialsNeeded;
		}
		else
		{
			statusCode = StatusCode.UnspecifiedErrorRegistration;
			item = "Registration Failed with error code " + ex.StatusCode.ToString(CultureInfo.InvariantCulture);
		}
		return (status: Status.ErrorStatus(statusCode), message: item);
	}

	private static (Status status, string message) ResolveHttpRequestException(HttpRequestException ex)
	{
		StatusCode statusCode = StatusCode.UnspecifiedErrorRegistration;
		string text = "Registration Failed.";
		if (((Exception)(object)ex).InnerException is WebException)
		{
			WebException ex2 = (WebException)((Exception)(object)ex).InnerException;
			if (ex2.InnerException is AuthenticationException)
			{
				text = "The certificate used by the server is not valid.";
			}
			else if (ex2.InnerException is SocketException)
			{
				text = "No connection to the authorization server.";
				statusCode = StatusCode.AuthorizationServerNotFound;
			}
			else if (ex2.InnerException is Win32Exception)
			{
				text = "You are not allowed to perform the requested action (401).";
				statusCode = StatusCode.VmsAdminCredentialsNeeded;
			}
		}
		string item = text + " Exception = " + (object)ex;
		return (status: Status.ErrorStatus(statusCode), message: item);
	}
}
