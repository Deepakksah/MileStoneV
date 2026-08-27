using System;
using VideoOS.Common.SharedComponent.ClientProfile.Handlers;

namespace VideoOS.Common.SharedComponent.ClientProfile.Providers;

public sealed class ClientProfileSettingSpecificationProviderName : IClientProfileSettingSpecificationProviderName
{
	private static readonly Lazy<ClientProfileSettingSpecificationProviderName> LazyInstance = new Lazy<ClientProfileSettingSpecificationProviderName>(() => new ClientProfileSettingSpecificationProviderName());

	private IClientProfileSettingSpecificationNameHandler ClientProfileSettingSpecificationNameHandler { get; }

	public static ClientProfileSettingSpecificationProviderName Instance => LazyInstance.Value;

	private ClientProfileSettingSpecificationProviderName()
	{
		ClientProfileSettingSpecificationNameHandler = new SmartMapClientProfileSettingSpecificationNameHandler(new LocalizedClientProfileSettingSpecificationNameHandler());
	}

	public string GetClientProfileSettingSpecificationName(string featureId, string clientProfileSettingSpecificationLocalizationId)
	{
		return ClientProfileSettingSpecificationNameHandler.Handle(featureId, clientProfileSettingSpecificationLocalizationId);
	}
}
