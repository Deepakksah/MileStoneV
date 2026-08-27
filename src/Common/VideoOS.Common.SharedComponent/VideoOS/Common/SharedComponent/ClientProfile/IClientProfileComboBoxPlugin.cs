namespace VideoOS.Common.SharedComponent.ClientProfile;

public interface IClientProfileComboBoxPlugin
{
	ValueItem[] GetValues();

	string GetDefaultValueKey();
}
