using System;
using System.Configuration;

namespace VideoOS.IM.Server.Service.ConfigSettings;

public sealed class ConfigSettingsService : IConfigSettingsService
{
	private static readonly Lazy<IConfigSettingsService> LazyInstance = new Lazy<IConfigSettingsService>(() => new ConfigSettingsService());

	public static IConfigSettingsService Instance => LazyInstance.Value;

	public Guid ClientRegistrationId => Guid.Parse(ConfigurationManager.AppSettings["ClientRegistrationId"]);

	public Uri AuthorizationServerUri => new Uri(ConfigurationManager.AppSettings["AuthorizationServerUri"]);

	private ConfigSettingsService()
	{
	}
}
