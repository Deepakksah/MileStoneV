using System.Globalization;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.DatabasePlugin;

public class DatabaseValidationControl : WizardPageBase
{
	public DatabaseValidationControl()
	{
		PageName = "DatabaseValidationPage";
	}

	public override void ReloadResources(CultureInfo culture)
	{
	}

	public override bool OnBeforeActivateOnPrevious()
	{
		return false;
	}
}
