using System;
using System.Collections.Generic;
using VideoOS.CertificateRegistration.Common;
using VideoOS.GatewayService.Utilities;

namespace VideoOS.GatewayService.Registrars;

public class RegistrationParametersHandler : IRegistrationParametersHandler
{
	private readonly string _serviceName;

	private readonly IConfigFileHelper _configFileHelper;

	private readonly string _appConfigPath;

	private readonly IReadOnlyCollection<string> _scopes;

	public RegistrationParametersHandler(string serviceName, IConfigFileHelper configFileHelper, string appConfigPath, IReadOnlyCollection<string> scopes)
	{
		_serviceName = serviceName;
		_configFileHelper = configFileHelper;
		_appConfigPath = appConfigPath;
		_scopes = scopes;
	}

	public RegistrationParameters ReadRegistrationParameters(Uri authorizationServerAddress)
	{
		return new RegistrationParameters(_configFileHelper.GetClientRegistrationId(_appConfigPath) ?? Guid.NewGuid(), _serviceName, _scopes, authorizationServerAddress);
	}

	public bool WriteRegistrationParameters(RegistrationParameters parameters)
	{
		if (_configFileHelper.GetClientRegistrationId(_appConfigPath) != parameters.ClientId)
		{
			_configFileHelper.SetClientRegistrationId(_appConfigPath, parameters.ClientId);
		}
		return true;
	}
}
