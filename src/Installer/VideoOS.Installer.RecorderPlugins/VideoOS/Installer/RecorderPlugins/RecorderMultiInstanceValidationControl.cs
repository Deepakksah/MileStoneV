using System.Globalization;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.RecorderPlugins;

public class RecorderMultiInstanceValidationControl : WizardPageBase
{
	public RecorderMultiInstanceValidationControl()
	{
		PageName = "RecorderMultiInstanceValidationPage";
	}

	public override void ReloadResources(CultureInfo culture)
	{
	}
}
