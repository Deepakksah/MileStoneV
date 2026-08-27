using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.UI.Framework;

public class WizardPageControl : UserControl, IWizardPage
{
	public virtual WizardPage Page => null;

	public virtual IPageSetupActions PageSetupActions { get; set; }

	public virtual IFlowManager FlowManager { get; set; }

	public virtual string PageName { get; set; }

	public virtual Wizard Wizard { get; set; }

	public virtual Dictionary<string, string> CollectParameters()
	{
		return null;
	}

	public virtual void SetParameters(Dictionary<string, string> parameters)
	{
	}

	public virtual List<string> GetEventParameter()
	{
		return null;
	}

	protected void ReloadResourcesInternal(Control control, ComponentResourceManager resourceManager)
	{
		if (control.Controls != null && control.Controls.Count > 0)
		{
			foreach (Control control2 in control.Controls)
			{
				ReloadResourcesInternal(control2, resourceManager);
			}
		}
		resourceManager.ApplyResources(control, control.Name);
	}

	public virtual void ReloadResources(CultureInfo culture)
	{
		ReloadResourcesInternal(this, new ComponentResourceManager(GetType()));
	}

	public virtual bool OnBeforeContinue()
	{
		return true;
	}

	public virtual void OnBeforeCancel()
	{
	}

	public virtual bool OnBeforeActivate()
	{
		return true;
	}

	public virtual bool OnBeforeActivate(Dictionary<string, string> parameters)
	{
		return OnBeforeActivate();
	}

	public virtual string OnTriggerEvent()
	{
		return null;
	}

	public virtual bool OnBeforeActivateOnPrevious()
	{
		return true;
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.Name = "WizardPageControl";
		base.ResumeLayout(false);
	}
}
