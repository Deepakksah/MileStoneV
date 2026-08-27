using System;
using System.Collections;
using System.ComponentModel.Design;
using System.Drawing;
using System.Windows.Forms.Design;

namespace VideoOS.Common.Controls;

public class WizardFormDesigner : ParentControlDesigner
{
	private DesignerVerbCollection m_verbs;

	public override DesignerVerbCollection Verbs => m_verbs;

	public WizardFormDesigner()
	{
		m_verbs = new DesignerVerbCollection(new DesignerVerb[4]
		{
			new DesignerVerb("Add Welcome Page", OnAddWelcomeClick),
			new DesignerVerb("Add Page", OnAddPageClick),
			new DesignerVerb("Add Final Page", OnAddFinalClick),
			new DesignerVerb("Remove Page", OnRemoveClick)
		});
	}

	protected override void PreFilterProperties(IDictionary properties)
	{
		base.PreFilterProperties(properties);
	}

	protected override bool GetHitTest(Point point)
	{
		WizardForm wizardForm = (WizardForm)Control;
		if (wizardForm != null)
		{
			Point p = wizardForm.PointToClient(point);
			return wizardForm.OnDesignedWizardButtons(p);
		}
		return base.GetHitTest(point);
	}

	private void OnAddWelcomeClick(object sender, EventArgs e)
	{
		WizardForm obj = (WizardForm)Control;
		WizardWelcomePage value = new WizardWelcomePage();
		obj.Pages.Add(value);
		((IDesignerHost)GetService(typeof(IDesignerHost)))?.Container.Add(value);
	}

	private void OnAddPageClick(object sender, EventArgs e)
	{
		WizardForm obj = (WizardForm)Control;
		WizardPageBase value = new WizardPageBase();
		obj.Pages.Add(value);
		((IDesignerHost)GetService(typeof(IDesignerHost)))?.Container.Add(value);
	}

	private void OnAddFinalClick(object sender, EventArgs e)
	{
		WizardForm obj = (WizardForm)Control;
		WizardFinalPage value = new WizardFinalPage();
		obj.Pages.Add(value);
		((IDesignerHost)GetService(typeof(IDesignerHost)))?.Container.Add(value);
	}

	private void OnRemoveClick(object sender, EventArgs e)
	{
		WizardForm wizardForm = (WizardForm)Control;
		if (wizardForm.Pages.Count > 0 && wizardForm.PageIndex > 0)
		{
			wizardForm.Pages.RemoveAt(wizardForm.PageIndex);
		}
	}
}
