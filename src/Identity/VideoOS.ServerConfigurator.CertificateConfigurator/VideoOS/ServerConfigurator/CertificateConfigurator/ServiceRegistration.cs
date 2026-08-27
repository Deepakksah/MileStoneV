using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public class ServiceRegistration : RegistrationBase<ServerEndpointConfiguration, IRegisterServiceV3, RegisterServiceStatus>
{
	private class ServiceRegistrationCompatibilityProvider : ICompatibilityProvider
	{
		private class RegisterServiceV2Wrapper<T> : IRegisterServiceV3, IServerCleanup
		{
			private IRegisterServiceV2 _registerServceV2;

			public T Interface => (T)(object)this;

			public RegisterServiceV2Wrapper(IRegisterServiceV2 registerServceV2)
			{
				if (!(this is T))
				{
					throw new InvalidOperationException(FormattableString.Invariant($"This class does not implement the requested type. The requested type is {typeof(T)}"));
				}
				_registerServceV2 = registerServceV2;
			}

			public Status CleanUp()
			{
				return _registerServceV2.CleanUp();
			}

			public Status Initialize(VideoOS.ServerConfigurator.CertificateRegistration.ILogger logger)
			{
				return _registerServceV2.Initialize(logger);
			}

			public Status PostRegisterService()
			{
				return _registerServceV2.PostRegisterService();
			}

			public Status PreRegisterService()
			{
				return _registerServceV2.PreRegisterService();
			}

			public Status RegisterThisService(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, string hostToRegister, bool isSecure, Credential credentials)
			{
				return _registerServceV2.RegisterThisService(unsecureServerToRegisterTo, hostToRegister, isSecure, credentials);
			}
		}

		private class RegisterServiceV1Wrapper<T> : IRegisterServiceV3, IServerCleanup
		{
			private IRegisterService _registerServceV1;

			public T Interface => (T)(object)this;

			public RegisterServiceV1Wrapper(IRegisterService registerServceV1)
			{
				if (!(this is T))
				{
					throw new InvalidOperationException(FormattableString.Invariant($"This class does not implement the requested type. The requested type is {typeof(T)}"));
				}
				_registerServceV1 = registerServceV1;
			}

			public Status CleanUp()
			{
				return _registerServceV1.CleanUp();
			}

			public Status Initialize(VideoOS.ServerConfigurator.CertificateRegistration.ILogger logger)
			{
				return _registerServceV1.Initialize(logger);
			}

			public Status PostRegisterService()
			{
				return _registerServceV1.PostRegisterService();
			}

			public Status PreRegisterService()
			{
				return _registerServceV1.PreRegisterService();
			}

			public Status RegisterThisService(Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, string hostToRegister, bool isSecure, Credential credentials)
			{
				return _registerServceV1.RegisterThisService(unsecureServerToRegisterTo, isSecure, credentials);
			}
		}

		public T GetCompatibleType<T>(object value)
		{
			if (value is T)
			{
				return (T)value;
			}
			if (!(value is IRegisterService registerServceV))
			{
				if (value is IRegisterServiceV2 registerServceV2)
				{
					return new RegisterServiceV2Wrapper<T>(registerServceV2).Interface;
				}
				throw new InvalidOperationException(FormattableString.Invariant(FormattableStringFactory.Create("The provided value cannot be convertet to the requested type. Values is {0}, requested type is {1}", value?.GetType().ToString() ?? "null", typeof(T))));
			}
			return new RegisterServiceV1Wrapper<T>(registerServceV).Interface;
		}
	}

	private readonly Uri _serverToRegisterTo;

	private readonly Uri _unsecureServerToRegisterTo;

	private readonly Uri _identityProvider;

	private readonly string _hostToRegister;

	private readonly Credential _credentials;

	private readonly IEnumerable<CertificateGroup> _certificateGroups;

	private readonly ServiceRegistrationCompatibilityProvider _serviceRegistrationCompatibilityProvider;

	protected override ICompatibilityProvider CompatibilityProvider => _serviceRegistrationCompatibilityProvider;

	public ServiceRegistration(IEnumerable<ServerEndpointConfiguration> serverEndpoints, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration, NetworkCredential credentials, Uri serverToRegisterTo, Uri unsecureServerToRegisterTo, Uri identityProvider, string hostToRegister, IEnumerable<CertificateGroup> certificateGroups)
		: base(serverEndpoints, registrationLogger, logger, innerRegistration)
	{
		_serverToRegisterTo = serverToRegisterTo ?? throw new ArgumentNullException("serverToRegisterTo");
		_unsecureServerToRegisterTo = unsecureServerToRegisterTo ?? throw new ArgumentNullException("unsecureServerToRegisterTo");
		_identityProvider = identityProvider ?? throw new ArgumentNullException("identityProvider");
		_hostToRegister = hostToRegister ?? throw new ArgumentNullException("hostToRegister");
		_credentials = credentials;
		_certificateGroups = certificateGroups ?? throw new ArgumentNullException("certificateGroups");
		_serviceRegistrationCompatibilityProvider = new ServiceRegistrationCompatibilityProvider();
	}

	protected override ImplementationDefinition ImplementationDefinition(ServerEndpointConfiguration endpointConfiguration)
	{
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		return endpointConfiguration.ThisServerRegistrar;
	}

	protected override StatusCode ExecuteInitialize(IRegisterServiceV3 target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.Initialize(registrationLogger).StatusCode;
	}

	protected override Status PreExecute(IRegisterServiceV3 target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PreRegisterService();
	}

	protected override Status Execute(IRegisterServiceV3 target, ServerEndpointConfiguration endpointConfiguration)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		if (endpointConfiguration == null)
		{
			throw new ArgumentNullException("endpointConfiguration");
		}
		bool isSecure = _certificateGroups.Any((CertificateGroup cg) => cg.Id == endpointConfiguration.CertificateGroupId && cg.UseEncryption);
		return target.RegisterThisService(_serverToRegisterTo, _unsecureServerToRegisterTo, _identityProvider, _hostToRegister, isSecure, _credentials);
	}

	protected override Status PostExecute(IRegisterServiceV3 target)
	{
		if (target == null)
		{
			throw new ArgumentNullException("target");
		}
		return target.PostRegisterService();
	}

	protected override bool IsSuccess(RegisterServiceStatus result)
	{
		return result.ResultType().SuccessOrWarning();
	}

	protected override RegisterServiceStatus ResultFromStatusCode(StatusCode statusCode)
	{
		return RegisterServiceStatusFromStatusCode(statusCode);
	}

	private static RegisterServiceStatus RegisterServiceStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => RegisterServiceStatus.Success, 
			StatusCode.UnspecifiedErrorRegistration => RegisterServiceStatus.UnspecifiedError, 
			StatusCode.UnhandledException => RegisterServiceStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => RegisterServiceStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => RegisterServiceStatus.CouldNotStopServer, 
			StatusCode.ManagementServerNotFound => RegisterServiceStatus.ManagementServerNotFound, 
			StatusCode.VmsAdminCredentialsNeeded => RegisterServiceStatus.InsufficientCredentials, 
			StatusCode.ServiceRegistrationNotFound => RegisterServiceStatus.ServiceRegistrationNotFound, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	public static void Cleanup(ServerEndpointConfiguration serverEndpoint, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger)
	{
		StatusCode result;
		foreach (IServerCleanup value in RegisterServiceCleanUpProxy.GetEndpointInterfaces(new ServerEndpointConfiguration[1] { serverEndpoint }, logger, registrationLogger, contiueOnError: true, out result).Values)
		{
			value.CleanUp();
		}
	}
}
