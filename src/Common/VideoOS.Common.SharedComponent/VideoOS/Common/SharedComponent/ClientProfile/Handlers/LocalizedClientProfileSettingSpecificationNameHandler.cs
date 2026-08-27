using System;

namespace VideoOS.Common.SharedComponent.ClientProfile.Handlers;

public sealed class LocalizedClientProfileSettingSpecificationNameHandler : IClientProfileSettingSpecificationNameHandler
{
	public string Handle(string featureId, string clientProfileSettingSpecificationLocalizationId)
	{
		if (clientProfileSettingSpecificationLocalizationId == null)
		{
			throw new ArgumentNullException("clientProfileSettingSpecificationLocalizationId");
		}
		return ResourcesUtility.GetResourceString(clientProfileSettingSpecificationLocalizationId);
	}

	public bool CanHandle(string featureId, string clientProfileSettingSpecificationLocalizationId)
	{
		return true;
	}
}
