using System;
using System.Configuration;
using System.Globalization;
using System.ServiceModel.Configuration;
using VideoOS.Utilities.Validation;

namespace VideoOS.Utilities.Configuration;

public sealed class ConfigurationFileController : IConfigurationFileController
{
	private readonly string _configName;

	public ConfigurationFileController([ValidatedNotNull] string configName)
	{
		_configName = configName;
	}

	public System.Configuration.Configuration CreateConfiguration()
	{
		return ConfigurationManager.OpenMappedExeConfiguration(new ExeConfigurationFileMap
		{
			ExeConfigFilename = _configName
		}, ConfigurationUserLevel.None);
	}

	public ServiceElement GetServiceElementConfiguration([ValidatedNotNull] System.Configuration.Configuration openMappedConfiguration, [ValidatedNotNull] string serviceName)
	{
		if (serviceName == null)
		{
			throw new ArgumentNullException("serviceName");
		}
		ServiceModelSectionGroup obj = ServiceModelSectionGroup.GetSectionGroup(openMappedConfiguration) ?? throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expected to find a service model section in {0}.", openMappedConfiguration.FilePath));
		if (obj.Services.Services.Count < 1)
		{
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "No services found in service section of {0}.", openMappedConfiguration.FilePath));
		}
		foreach (ServiceElement service in obj.Services.Services)
		{
			if (serviceName.Equals(service.Name))
			{
				return service;
			}
		}
		return null;
	}

	public Uri GetUriAppSetting([ValidatedNotNull] string keyName)
	{
		System.Configuration.Configuration configuration = CreateConfiguration();
		if (!Uri.TryCreate((configuration.AppSettings.Settings[keyName] ?? throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "Expected to find {0} in app settings section of {1}.", keyName, configuration.FilePath))).Value, UriKind.Absolute, out var result))
		{
			throw new InvalidOperationException(string.Format(CultureInfo.InvariantCulture, "The {0} setting in {1} is not a valid URI.", keyName, configuration.FilePath));
		}
		return result;
	}

	public void SetOrUpdateAppSetting([ValidatedNotNull] string keyName, [ValidatedNotNull] string value)
	{
		System.Configuration.Configuration configuration = CreateConfiguration();
		if (configuration.AppSettings.Settings[keyName] == null)
		{
			configuration.AppSettings.Settings.Add(keyName, value);
		}
		else
		{
			configuration.AppSettings.Settings[keyName].Value = value;
		}
		configuration.Save(ConfigurationSaveMode.Modified);
	}
}
