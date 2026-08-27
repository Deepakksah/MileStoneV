using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace VideoOS.Installer.UI.Framework;

[Designer("VideoOS.Installer.UI.Designer.WizardPageDesigner, Wizard.Design")]
public class WizardPage : Panel
{
	private bool _finishPage;

	[Category("Wizard")]
	public bool FinishPage
	{
		get
		{
			return _finishPage;
		}
		set
		{
			_finishPage = value;
		}
	}

	[Browsable(true)]
	public SizeF AutoScaleDimensions { get; set; }

	[Browsable(true)]
	public AutoScaleMode AutoScaleMode { get; set; }

	[Category("Wizard")]
	public Guid FlowId { get; set; }

	public event PageEventHandler CloseFromBack;

	public event PageEventHandler CloseFromNext;

	public event EventHandler ShowFromBack;

	public event EventHandler ShowFromNext;

	public event PageEventHandler PageActivated;

	public int OnCloseFromBack(Wizard wiz)
	{
		PageEventArgs e = ((!wiz.AutoBackPathEnabled) ? new PageEventArgs(wiz.PageIndex - 1, wiz.Pages) : new AutoBackPathPageEventArgs(wiz.PreviousBackPage, wiz.Page, wiz.Pages));
		if (this.CloseFromBack != null)
		{
			this.CloseFromBack(wiz, e);
		}
		return e.PageIndex;
	}

	public int OnCloseFromNext(Wizard wiz)
	{
		PageEventArgs e = new PageEventArgs(wiz.PageIndex + 1, wiz.Pages);
		if (this.CloseFromNext != null)
		{
			this.CloseFromNext(wiz, e);
		}
		return e.PageIndex;
	}

	public void OnShowFromBack(Wizard wiz)
	{
		if (this.ShowFromBack != null)
		{
			this.ShowFromBack(wiz, EventArgs.Empty);
		}
	}

	public void OnShowFromNext(Wizard wiz)
	{
		if (this.ShowFromNext != null)
		{
			this.ShowFromNext(wiz, EventArgs.Empty);
		}
	}

	public void OnPageActivated(Wizard wizard)
	{
		if (this.PageActivated != null)
		{
			this.PageActivated(wizard, new PageEventArgs(wizard.PageIndex + 1, wizard.Pages));
		}
	}

	protected override void Dispose(bool disposing)
	{
		base.Dispose(disposing);
	}

	public void FocusFirstTabIndex()
	{
		Control control = null;
		foreach (Control control2 in base.Controls)
		{
			if (control2.CanFocus && (control == null || control2.TabIndex < control.TabIndex))
			{
				control = control2;
			}
		}
		if (control != null)
		{
			control.Focus();
		}
		else
		{
			Focus();
		}
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.ResumeLayout(false);
	}
}
