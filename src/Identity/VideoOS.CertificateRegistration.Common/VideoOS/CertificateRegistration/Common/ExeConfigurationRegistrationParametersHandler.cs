using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;

namespace VideoOS.CertificateRegistration.Common;

public class ExeConfigurationRegistrationParametersHandler : IRegistrationParametersHandler
{
	private readonly string _configFileName;

	private readonly IReadOnlyCollection<string> _scopes;

	private readonly string _clientName;

	private const string ClientRegistrationKeyName = "ClientRegistrationId";

	private const string AuthorizationServerKeyName = "AuthorizationServerUri";

	public ExeConfigurationRegistrationParametersHandler(string configFileName, string clientName, IEnumerable<string> scopes)
	{
		if (scopes == null)
		{
			throw new ArgumentNullException("scopes");
		}
		if (string.IsNullOrEmpty(configFileName))
		{
			throw new ArgumentException("Value cannot be null or empty.", "configFileName");
		}
		if (!File.Exists(configFileName))
		{
			throw new ArgumentException("Could not find " + configFileName + ".");
		}
		_configFileName = configFileName;
		_scopes = scopes.ToList().AsReadOnly();
		_clientName = clientName;
	}

	public RegistrationParameters ReadRegistrationParameters(Uri authorizationServerAddress)
	{
		return new RegistrationParameters(EnsureClientRegistrationId(), _clientName, _scopes, authorizationServerAddress);
	}

	public bool WriteRegistrationParameters(RegistrationParameters parameters)
	{
		AddOrUpdateAppSettings("ClientRegistrationId", parameters.ClientId.ToString());
		AddOrUpdateAppSettings("AuthorizationServerUri", parameters.AuthorizationServerAddress.ToString());
		return true;
	}

	private Guid EnsureClientRegistrationId()
	{
		KeyValueConfigurationElement appSetting = GetAppSetting("ClientRegistrationId");
		if (appSetting == null)
		{
			AddOrUpdateAppSettings("ClientRegistrationId", "");
			appSetting = GetAppSetting("ClientRegistrationId");
		}
		Guid guid = Guid.NewGuid();
		if (string.IsNullOrEmpty(appSetting.Value))
		{
			AddOrUpdateAppSettings("ClientRegistrationId", guid.ToString());
			appSetting = GetAppSetting("ClientRegistrationId");
		}
		if (Guid.TryParse(appSetting.Value, out var result))
		{
			return result;
		}
		throw new InvalidOperationException("The value " + appSetting.Value + " in " + _configFileName + " is not a valid client registration id.");
	}

	private KeyValueConfigurationElement GetAppSetting(string settingKey)
	{
		return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = _configFileName
		}, ConfigurationUserLevel.None).AppSettings.Settings["ClientRegistrationId"];
	}

	private void AddOrUpdateAppSettings(string key, string value)
	{
		Configuration configuration = ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = _configFileName
		}, ConfigurationUserLevel.None);
		KeyValueConfigurationCollection settings = configuration.AppSettings.Settings;
		if (settings[key] == null)
		{
			settings.Add(key, value);
		}
		else
		{
			settings[key].Value = value;
		}
		configuration.Save(ConfigurationSaveMode.Modified);
	}
}
