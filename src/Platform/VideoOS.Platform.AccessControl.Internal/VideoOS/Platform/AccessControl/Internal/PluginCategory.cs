using System;
using System.Linq;
using VideoOS.Platform.AccessControl.Internal.Providers;
using VideoOS.Platform.AccessControl.Internal.Providers.Strategies;

namespace VideoOS.Platform.AccessControl.Internal;

public class PluginCategory : Category
{
	public Guid AccessControlPluginId { get; private set; }

	public string ExternalId { get; private set; }

	public static PluginCategory[] GetPluginCategories(Guid accessControlPluginId, string externalId, string name, Type accessControlType)
	{
		return GuidProvider.Instance.GuidProviderStrategies.Select((IGuidProviderStrategy gps) => new PluginCategory(accessControlPluginId, externalId, name, accessControlType, gps)).ToArray();
	}

	public PluginCategory(Guid accessControlPluginId, string externalId, string name, Type accessControlType, IGuidProviderStrategy guidProviderStrategy)
		: base(CreateCategoryId(accessControlPluginId, externalId, guidProviderStrategy), name, accessControlType)
	{
		AccessControlPluginId = accessControlPluginId;
		ExternalId = externalId;
	}

	public PluginCategory(Guid accessControlPluginId, string externalId, string name, Type accessControlType)
		: base(CreateCategoryId(accessControlPluginId, externalId, new Sha1ProviderStrategy()), name, accessControlType)
	{
		AccessControlPluginId = accessControlPluginId;
		ExternalId = externalId;
	}

	public static Guid CreateCategoryId(Guid accessControlPluginId, string externalId, IGuidProviderStrategy guidProviderStrategy)
	{
		return guidProviderStrategy.Create(accessControlPluginId, externalId);
	}
}
