using System.Windows.Forms;

namespace VideoOS.Common.SharedComponent.ClientProfile;

public interface IClientProfileButtonPlugin
{
	DialogResult Execute(string settingValue);

	string GetValue();

	string GetReportDisplayValue(string settingValue);

	string DisplayTextForEmptyValue();

	string DisplayTextForNoneEmptyValue();
}
