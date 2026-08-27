using System;
using System.Collections.Generic;
using VideoOS.ServerConfigurator.CertificateRegistration;
using VideoOS.ServerConfigurator.Common;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.ServerConfigurator.CertificateConfigurator;

public abstract class RegistrationBase<TEndpointConfiguration, TInterface, TResult> : IRegistration<TInterface>, IInnerRegistration where TEndpointConfiguration : EndpointConfiguration where TInterface : class where TResult : struct
{
	private readonly IEnumerable<TEndpointConfiguration> _endpoints;

	private readonly VideoOS.ServerConfigurator.CertificateRegistration.ILogger _registrationLogger;

	private readonly VideoOS.ServerConfigurator.Interfaces.ILogger _logger;

	private readonly IInnerRegistration _innerRegistration;

	private TResult? _initializeresult;

	private Dictionary<Guid, TInterface> _interfaces;

	protected virtual ICompatibilityProvider CompatibilityProvider => DefaultCompatibilityProvider.Instance;

	protected virtual IRegistrationActivator RegistrationActivator => VideoOS.ServerConfigurator.Common.Activator.Instance;

	protected RegistrationBase(IEnumerable<TEndpointConfiguration> endpoints, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger, VideoOS.ServerConfigurator.Interfaces.ILogger logger, IInnerRegistration innerRegistration)
	{
		_endpoints = endpoints;
		_registrationLogger = registrationLogger;
		_logger = logger;
		_innerRegistration = innerRegistration;
	}

	protected abstract TResult ResultFromStatusCode(StatusCode statusCode);

	protected abstract bool IsSuccess(TResult result);

	protected abstract ImplementationDefinition ImplementationDefinition(TEndpointConfiguration endpointConfiguration);

	private StatusCode LocalExecuteInitialize(TInterface @interface, VideoOS.ServerConfigurator.Interfaces.ILogger logger, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger)
	{
		try
		{
			return ExecuteInitialize(@interface, registrationLogger);
		}
		catch (Exception ex)
		{
			logger.LogError(FormattableString.Invariant($"Could not initialize Registrar : {@interface.GetType()}. Initialize method threw exception: {ex}."));
			return StatusCode.UnhandledException;
		}
	}

	protected abstract StatusCode ExecuteInitialize(TInterface target, VideoOS.ServerConfigurator.CertificateRegistration.ILogger registrationLogger);

	public virtual bool Initialize(Dictionary<Type, object> results)
	{
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		if (_initializeresult.HasValue)
		{
			return IsSuccess(_initializeresult.Value);
		}
		_interfaces = GetEndpointInterfaces(_endpoints, _logger, contiueOnError: false, out var result);
		_initializeresult = ResultFromStatusCode(result);
		results[_initializeresult.GetType()] = _initializeresult;
		bool flag = IsSuccess(_initializeresult.Value);
		if (flag && _innerRegistration != null && !_innerRegistration.Initialize(results))
		{
			return false;
		}
		return flag;
	}

	public virtual RegistrationResult Register()
	{
		Dictionary<Type, object> results = new Dictionary<Type, object>();
		Register(results);
		return new RegistrationResult(results);
	}

	public virtual bool Register(Dictionary<Type, object> results)
	{
		if (results == null)
		{
			throw new ArgumentNullException("results");
		}
		if (!Initialize(results))
		{
			return false;
		}
		var (val, preRegisteredTargets) = PreExecute(_interfaces);
		if (IsSuccess(val))
		{
			val = Execute(_endpoints, _interfaces);
		}
		bool flag = true;
		if (IsSuccess(val) && _innerRegistration != null)
		{
			flag = _innerRegistration.Register(results);
		}
		TResult val2 = PostExecute(preRegisteredTargets);
		if (!IsSuccess(val2) && IsSuccess(val))
		{
			val = val2;
		}
		results[val.GetType()] = val;
		return IsSuccess(val) && flag;
	}

	private Status LocalPostExecute(TInterface target)
	{
		try
		{
			return PostExecute(target);
		}
		catch (Exception ex)
		{
			_logger.LogError(FormattableString.Invariant($"Could not post-execute Registrar : {target.GetType()}. Post-Execute method threw exception: {ex}."));
			return Status.ErrorStatus(StatusCode.UnhandledException);
		}
	}

	protected abstract Status PostExecute(TInterface target);

	private TResult PostExecute(IEnumerable<TInterface> preRegisteredTargets)
	{
		return ResultFromStatusCode(ForEach(preRegisteredTargets, (TInterface target) => LocalPostExecute(target)));
	}

	private Status LocalExecute(TInterface target, TEndpointConfiguration endpoint)
	{
		try
		{
			return Execute(target, endpoint);
		}
		catch (Exception ex)
		{
			_logger.LogError(FormattableString.Invariant($"Could not execute Registrar : {target.GetType()}. Execute method threw exception: {ex}."));
			return Status.ErrorStatus(StatusCode.UnhandledException);
		}
	}

	protected abstract Status Execute(TInterface target, TEndpointConfiguration endpoint);

	private TResult Execute(IEnumerable<TEndpointConfiguration> endpoints, Dictionary<Guid, TInterface> interfaces)
	{
		StatusCode statusCode = WhileSuccessOrWarning(endpoints, interfaces, (TInterface target, TEndpointConfiguration endpoint) => LocalExecute(target, endpoint));
		return ResultFromStatusCode(statusCode);
	}

	private Status LocalPreExecute(TInterface target)
	{
		try
		{
			return PreExecute(target);
		}
		catch (Exception ex)
		{
			_logger.LogError(FormattableString.Invariant($"Could not pre-execute Registrar : {target.GetType()}. Pre-Execute method threw exception: {ex}."));
			return Status.ErrorStatus(StatusCode.UnhandledException);
		}
	}

	protected abstract Status PreExecute(TInterface target);

	private (TResult statusCode, IEnumerable<TInterface> preAppliedClients) PreExecute(Dictionary<Guid, TInterface> interfaces)
	{
		List<TInterface> preRegisteredTargets = new List<TInterface>();
		StatusCode statusCode = WhileSuccess(interfaces.Values, delegate(TInterface target)
		{
			preRegisteredTargets.Add(target);
			return LocalPreExecute(target);
		});
		return (statusCode: ResultFromStatusCode(statusCode), preAppliedClients: preRegisteredTargets);
	}

	private static RegisterClientStatus RegisterClientStatusFromStatusCode(StatusCode result)
	{
		return result switch
		{
			StatusCode.Success => RegisterClientStatus.Success, 
			StatusCode.VmsAdminCredentialsNeeded => RegisterClientStatus.InsufficientCredentials, 
			StatusCode.AuthorizationServerNotFound => RegisterClientStatus.AuthorizationServerNotFound, 
			StatusCode.UnspecifiedErrorRegistration => RegisterClientStatus.UnspecifiedError, 
			StatusCode.CouldNotStartServer => RegisterClientStatus.CouldNotStartServer, 
			StatusCode.CouldNotStopServer => RegisterClientStatus.CouldNotStopServer, 
			_ => throw new InvalidOperationException(FormattableString.Invariant($"Unexpected value {result}")), 
		};
	}

	private Dictionary<Guid, TInterface> GetEndpointInterfaces(IEnumerable<TEndpointConfiguration> endpoints, VideoOS.ServerConfigurator.Interfaces.ILogger logger, bool contiueOnError, out StatusCode result)
	{
		Dictionary<Guid, TInterface> dictionary = new Dictionary<Guid, TInterface>();
		result = StatusCode.Success;
		foreach (TEndpointConfiguration endpoint in endpoints)
		{
			ImplementationDefinition implementationDefinition = ImplementationDefinition(endpoint);
			if (implementationDefinition == null)
			{
				continue;
			}
			(StatusCode, TInterface, bool) tuple = CreateInstance(logger, RegistrationActivator, implementationDefinition, CompatibilityProvider);
			TInterface val;
			(result, val, _) = tuple;
			if (tuple.Item3)
			{
				if (result.IsSuccess())
				{
					result = LocalExecuteInitialize(val, logger, _registrationLogger);
				}
				if (result.IsSuccess())
				{
					dictionary[endpoint.Id] = val;
				}
				else if (!contiueOnError)
				{
					break;
				}
			}
			else
			{
				dictionary[endpoint.Id] = val;
			}
		}
		return dictionary;
	}

	private static (StatusCode result, TInterface @interface, bool newInstance) CreateInstance(VideoOS.ServerConfigurator.Interfaces.ILogger logger, IRegistrationActivator activator, ImplementationDefinition implementationDefinition, ICompatibilityProvider compatibilityProvider)
	{
		StatusCode item = StatusCode.Success;
		TInterface item2 = null;
		bool newInstance = false;
		try
		{
			item2 = activator.CreateInstance<TInterface>(implementationDefinition.AssemblyQualifiedName, implementationDefinition.FileLocation, logger, out newInstance, compatibilityProvider);
		}
		catch (Exception ex)
		{
			logger.LogError(FormattableString.Invariant($"Could not create endpoint Registrar : {implementationDefinition.AssemblyQualifiedName} in location {implementationDefinition.FileLocation}. Constructor threw exception: {ex}."));
			item = StatusCode.UnhandledException;
		}
		return (result: item, @interface: item2, newInstance: newInstance);
	}

	private static StatusCode WhileSuccess(IEnumerable<TInterface> endpointInterfaces, Func<TInterface, Status> function)
	{
		StatusCode statusCode = StatusCode.Success;
		foreach (TInterface endpointInterface in endpointInterfaces)
		{
			statusCode = function(endpointInterface).StatusCode;
			if (!statusCode.IsSuccess())
			{
				return statusCode;
			}
		}
		return statusCode;
	}

	private static StatusCode WhileSuccessOrWarning(IEnumerable<TEndpointConfiguration> endpoints, Dictionary<Guid, TInterface> interfaces, Func<TInterface, TEndpointConfiguration, Status> function)
	{
		Status status = Status.SuccessStatus();
		Status status2 = null;
		foreach (TEndpointConfiguration endpoint in endpoints)
		{
			if (!interfaces.ContainsKey(endpoint.Id))
			{
				continue;
			}
			TInterface arg = interfaces[endpoint.Id];
			status = function(arg, endpoint);
			if (!status.StatusCode.IsSuccess())
			{
				if (!status.StatusCode.IsWarning())
				{
					return status.StatusCode;
				}
				status2 = status;
			}
		}
		return status2?.StatusCode ?? status.StatusCode;
	}

	private static StatusCode ForEach<T>(IEnumerable<T> endpointInterfaces, Func<T, Status> function)
	{
		StatusCode statusCode = StatusCode.Success;
		foreach (T endpointInterface in endpointInterfaces)
		{
			StatusCode statusCode2 = function(endpointInterface).StatusCode;
			if (!statusCode2.IsSuccess() && statusCode.IsSuccess())
			{
				statusCode = statusCode2;
			}
		}
		return statusCode;
	}
}
