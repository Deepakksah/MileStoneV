namespace VideoOS.Common.SharedComponent.ClientProfile.Handlers;

public interface IClientProfileSettingSpecificationNameHandler
{
	string Handle(string featureId, string clientProfileSettingSpecificationLocalizationId);

	bool CanHandle(string featureId, string clientProfileSettingSpecificationLocalizationId);
}
