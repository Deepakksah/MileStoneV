using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Newtonsoft.Json.Linq;
using VideoOS.Common.EnvironmentServices;
using VideoOS.ServerConfigurator.CertificateRegistration;

namespace VideoOS.GatewayService.Utilities;

public class ConfigFileHelper : IConfigFileHelper
{
	private ILogger _logger;

	private readonly IFileService _fileService;

	private static int _systemIoExceptionCounter = 0;

	private const int MaxSystemIoExceptions = 3;

	private static object _threadLock = new object();

	public ConfigFileHelper()
		: this((ILogger)(object)NullLogger.Instance)
	{
	}

	public ConfigFileHelper(ILogger logger)
		: this((ILogger)(object)new ILoggerWrapper(logger))
	{
	}

	public ConfigFileHelper(ILogger logcomponent)
		: this(logcomponent, new FileService())
	{
	}

	public ConfigFileHelper(ILogger logcomponent, IFileService fileService)
	{
		_logger = logcomponent;
		_fileService = fileService;
	}

	public bool UpdateManagementServerUri(string gatewayConfigPath, Uri managementServerUri)
	{
		try
		{
			UpdateConfigItem(gatewayConfigPath, "ReverseProxy.Clusters.managementserver.Destinations.hostname.Address", JToken.op_Implicit(managementServerUri));
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(_logger, FormattableString.Invariant($"Error updating ManagementServer address in config file: {ex.Message}"), Array.Empty<object>());
			return false;
		}
		return true;
	}

	public bool UpdateDllSearchPath(string gatewayConfigPath, string dllPath)
	{
		try
		{
			UpdateConfigItem(gatewayConfigPath, "GatewayConfigurations.DllSearchPath", JToken.op_Implicit(dllPath));
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(_logger, FormattableString.Invariant($"Error updating Path of dlls in config file: {ex.Message}"), Array.Empty<object>());
			return false;
		}
		return true;
	}

	public bool UpdateGatewayServiceUri(string gatewayConfigPath, Uri gatewayServiceUri)
	{
		try
		{
			UpdateConfigItem(gatewayConfigPath, "ReverseProxy.Clusters.apigateway.Destinations.hostname.Address", JToken.op_Implicit(gatewayServiceUri));
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogDebug(_logger, FormattableString.Invariant($"Error updating GatewayService address in config file: {ex.Message}"), Array.Empty<object>());
			return false;
		}
		return true;
	}

	private void UpdateConfigItem(string configPath, string item, JToken newValue)
	{
		try
		{
			lock (_threadLock)
			{
				JObject obj = JObject.Parse(_fileService.ReadAllText(configPath));
				obj.SetValue(item, newValue);
				string contents = ((object)obj).ToString();
				_fileService.WriteAllText(configPath, contents);
				_systemIoExceptionCounter = 0;
			}
		}
		catch (IOException innerException)
		{
			_systemIoExceptionCounter++;
			if (_systemIoExceptionCounter >= 3)
			{
				_systemIoExceptionCounter = 0;
				throw new IOException("Failed to UpdateConfigItem, due to System.IO.Exception", innerException);
			}
			Task.Delay(1000).Wait();
			UpdateConfigItem(configPath, item, newValue);
		}
	}

	private JToken ReadConfigItem(string configPath, string item)
	{
		try
		{
			lock (_threadLock)
			{
				JObject obj = JObject.Parse(_fileService.ReadAllText(configPath));
				_systemIoExceptionCounter = 0;
				return ((JToken)obj).SelectToken(item);
			}
		}
		catch (IOException innerException)
		{
			_systemIoExceptionCounter++;
			if (_systemIoExceptionCounter >= 3)
			{
				_systemIoExceptionCounter = 0;
				throw new IOException("Failed to ReadConfigItem, due to System.IO.Exception", innerException);
			}
			Task.Delay(1000).Wait();
			return ReadConfigItem(configPath, item);
		}
	}

	public void SetClientRegistrationId(string appConfigFile, Guid id)
	{
		UpdateConfigItem(appConfigFile, "ClientRegistration.RegistrationId", JToken.op_Implicit(id));
	}

	public Guid? GetClientRegistrationId(string appConfigPath)
	{
		return (Guid?)ReadConfigItem(appConfigPath, "ClientRegistration.RegistrationId");
	}
}
