namespace VideoOS.Common.SharedComponent.ClientProfile;

public interface IClientProfileValidatePlugin
{
	bool TryValidation(string settingValue, out int errorCode);

	string GetErrorText(int errorCode);
}
