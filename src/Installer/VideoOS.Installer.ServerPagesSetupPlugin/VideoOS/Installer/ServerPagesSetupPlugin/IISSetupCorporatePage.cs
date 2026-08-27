using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Windows.Forms;
using VideoOS.Installer.UI;

namespace VideoOS.Installer.ServerPagesSetupPlugin;

[SuppressMessage("Microsoft.Naming", "CA1709:Identifiers should be cased correctly", Scope = "member", Target = "VideoOS.Installer.ServerPagesSetupPlugin.#IISSetupCorporatePage", Justification = "IIS is the way it is used by Milestone")]
public class IISSetupCorporatePage : WizardPageIISSetup
{
	private IContainer components;

	public IISSetupCorporatePage()
	{
		InitializeComponent();
		PageName = "IISSetupCorporatePage";
	}

	public override bool OnBeforeContinue()
	{
		bool num = base.OnBeforeContinue();
		if (num)
		{
			WebsiteHelper.CreateRegistryEntries(base.WebsiteToUse);
			WebsiteHelper.UpdateManagementAppSettings(base.WebsiteToUse.HttpPort, base.WebsiteToUse.HttpsPort, "OnBeforeContinue");
		}
		return num;
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing && components != null)
		{
			components.Dispose();
		}
		base.Dispose(disposing);
	}

	private void InitializeComponent()
	{
		this.components = new System.ComponentModel.Container();
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
	}
}
