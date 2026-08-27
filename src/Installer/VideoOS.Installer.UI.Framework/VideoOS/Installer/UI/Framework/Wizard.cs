using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;
using VideoOS.Installer.UI.Framework.Properties;

namespace VideoOS.Installer.UI.Framework;

[Designer("VideoOS.Installer.UI.Designer.WizardDesigner, Wizard.Design")]
[ToolboxItem(true)]
[ToolboxBitmap(typeof(Wizard))]
public class Wizard : UserControl
{
	private Container components;

	public ButtonBackEventHandler OnButtonBack;

	public ButtonNextEventHandler OnButtonNext;

	private PageCollection _pages;

	private WizardPage _continueWizardPage;

	private WizardPage _activePage;

	public Panel NavigationPanel;

	private Button btnCancel;

	public Button btnNext;

	public Button btnBack;

	private Stack<int> _nextPageStack;

	public Control CancelButton => btnCancel;

	public Control NextButton => btnNext;

	public Control BackButton => btnBack;

	public WizardPage ContinueWizardPage => _continueWizardPage;

	public EventHandler HelpButtonEventHandler { get; set; }

	[Category("Wizard")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public PageCollection Pages => _pages;

	[Category("Wizard")]
	public int PageIndex
	{
		get
		{
			return _pages.IndexOf(_activePage);
		}
		set
		{
			if (_pages.Count == 0)
			{
				ActivatePage(-1);
				return;
			}
			if (value < -1 || value >= _pages.Count)
			{
				throw new ArgumentOutOfRangeException("PageIndex", value, "The page index must be between 0 and " + Convert.ToString(_pages.Count - 1));
			}
			ActivatePage(value);
		}
	}

	public WizardPage Page
	{
		get
		{
			return _activePage;
		}
		set
		{
			PageIndex = _pages.IndexOf(value);
		}
	}

	public int PreviousBackPage
	{
		get
		{
			if (base.DesignMode)
			{
				return PageIndex - 1;
			}
			return _nextPageStack.Peek();
		}
	}

	[Category("Wizard")]
	public bool AutoBackPathEnabled { get; set; }

	[Category("Wizard")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool NextEnabled
	{
		get
		{
			return btnNext.Enabled;
		}
		set
		{
			btnNext.Enabled = value;
		}
	}

	[Category("Wizard")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool BackEnabled
	{
		get
		{
			return btnBack.Enabled;
		}
		set
		{
			btnBack.Enabled = value;
		}
	}

	[Category("Wizard")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool CancelEnabled
	{
		get
		{
			return btnCancel.Enabled;
		}
		set
		{
			btnCancel.Enabled = value;
		}
	}

	[Category("Wizard")]
	[DefaultValue(false)]
	public bool AutoApplyWaitCursor { get; set; }

	public event CancelEventHandler CloseFromCancel;

	public Wizard()
	{
		_pages = new PageCollection(this);
		InitializeComponent();
		btnCancel.Text = Resources.btnCancel;
		btnBack.Text = Resources.btnPrevious;
	}

	private void Wizard_Load(object sender, EventArgs e)
	{
		if (!base.DesignMode && AutoBackPathEnabled)
		{
			_nextPageStack = new Stack<int>();
		}
		if (_continueWizardPage == null)
		{
			ActivatePage(0);
		}
		else
		{
			ActivatePage(_pages.IndexOf(_continueWizardPage));
		}
		Form form = FindForm();
		if (form != null && !base.DesignMode)
		{
			form.CancelButton = btnCancel;
		}
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.Framework.Wizard));
		this.NavigationPanel = new System.Windows.Forms.Panel();
		this.btnCancel = new System.Windows.Forms.Button();
		this.btnNext = new System.Windows.Forms.Button();
		this.btnBack = new System.Windows.Forms.Button();
		this.NavigationPanel.SuspendLayout();
		base.SuspendLayout();
		this.NavigationPanel.BackColor = System.Drawing.Color.Transparent;
		this.NavigationPanel.Controls.Add(this.btnCancel);
		this.NavigationPanel.Controls.Add(this.btnNext);
		this.NavigationPanel.Controls.Add(this.btnBack);
		componentResourceManager.ApplyResources(this.NavigationPanel, "NavigationPanel");
		this.NavigationPanel.Name = "NavigationPanel";
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		componentResourceManager.ApplyResources(this.btnNext, "btnNext");
		this.btnNext.Name = "btnNext";
		this.btnNext.Click += new System.EventHandler(btnNext_Click);
		this.btnNext.MouseDown += new System.Windows.Forms.MouseEventHandler(btnNext_MouseDown);
		componentResourceManager.ApplyResources(this.btnBack, "btnBack");
		this.btnBack.Name = "btnBack";
		this.btnBack.Click += new System.EventHandler(btnBack_Click);
		this.btnBack.MouseDown += new System.Windows.Forms.MouseEventHandler(btnBack_MouseDown);
		componentResourceManager.ApplyResources(this, "$this");
		base.Controls.Add(this.NavigationPanel);
		base.Name = "Wizard";
		base.Load += new System.EventHandler(Wizard_Load);
		this.NavigationPanel.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	protected void ActivatePage(int index)
	{
		if (index < 0 || index >= _pages.Count)
		{
			btnNext.Enabled = false;
			btnBack.Enabled = false;
		}
		else
		{
			WizardPage page = _pages[index];
			ActivatePage(page);
		}
	}

	public void SetContinueFromPage(WizardPage page)
	{
		_continueWizardPage = page;
	}

	public void ActivatePage(WizardPage page)
	{
		if (_activePage == page)
		{
			return;
		}
		if (_activePage != null)
		{
			_activePage.Visible = false;
		}
		_activePage = page;
		if (_activePage != null)
		{
			_activePage.Parent = this;
			if (!Contains(_activePage))
			{
				base.Container.Add(_activePage);
			}
			_activePage.Dock = DockStyle.Fill;
			_activePage.Visible = true;
			_activePage.BringToFront();
			_activePage.FocusFirstTabIndex();
		}
		if (PageIndex > 0)
		{
			btnBack.Show();
			btnBack.Enabled = true;
		}
		else
		{
			btnBack.Hide();
			btnBack.Enabled = false;
		}
		if (_pages.IndexOf(_activePage) < _pages.Count - 1 && !_activePage.FinishPage)
		{
			btnNext.Text = Resources.btnNext_Text;
			btnNext.Enabled = true;
			btnNext.DialogResult = DialogResult.None;
		}
		else
		{
			btnNext.Text = Resources.btnFinish_Text;
			if (base.DesignMode && _pages.IndexOf(_activePage) == _pages.Count - 1)
			{
				btnNext.Enabled = false;
			}
			else
			{
				btnNext.Enabled = true;
				btnNext.DialogResult = DialogResult.OK;
			}
		}
		if (_activePage != null)
		{
			_activePage.OnPageActivated(this);
		}
		if (_activePage != null)
		{
			_activePage.Invalidate();
		}
		else
		{
			Invalidate();
		}
	}

	private void btnNext_Click(object sender, EventArgs e)
	{
		MoveToNextPage();
	}

	private void btnNext_MouseDown(object sender, MouseEventArgs e)
	{
		if (base.DesignMode)
		{
			Next();
		}
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		MoveToPreviousPage();
	}

	private void btnBack_MouseDown(object sender, MouseEventArgs e)
	{
		if (base.DesignMode)
		{
			Back();
		}
	}

	private void btnHelp_Click(object sender, EventArgs e)
	{
		if (HelpButtonEventHandler != null)
		{
			HelpButtonEventHandler(sender, e);
		}
	}

	public void btnCancel_Click(object sender, EventArgs e)
	{
		CancelEventArgs e2 = new CancelEventArgs();
		if (this.CloseFromCancel != null)
		{
			this.CloseFromCancel(this, e2);
		}
		if (!e2.Cancel)
		{
			FindForm().Close();
		}
	}

	public void Next()
	{
		if (AutoApplyWaitCursor)
		{
			Cursor.Current = Cursors.WaitCursor;
		}
		try
		{
			int num = _activePage.OnCloseFromNext(this);
			if (PageIndex < _pages.Count - 1 && (!_activePage.FinishPage || base.DesignMode))
			{
				if (PageIndex != num)
				{
					if (_nextPageStack != null)
					{
						_nextPageStack.Push(PageIndex);
					}
					ActivatePage(num);
					_activePage.OnShowFromNext(this);
				}
			}
			else
			{
				_ = "btnNext_Click tried to go to page " + Convert.ToString(PageIndex + 1) + ", but I only have " + Convert.ToString(_pages.Count);
				if (!base.DesignMode)
				{
					base.ParentForm.Close();
				}
			}
		}
		finally
		{
			if (AutoApplyWaitCursor)
			{
				Cursor.Current = Cursors.Default;
			}
		}
	}

	public void NextTo(WizardPage page)
	{
		if (page != _activePage)
		{
			if (_nextPageStack != null)
			{
				_nextPageStack.Push(PageIndex);
			}
			ActivatePage(page);
			_activePage.OnShowFromNext(this);
		}
	}

	public void Back()
	{
		Cursor.Current = Cursors.WaitCursor;
		try
		{
			int num = _activePage.OnCloseFromBack(this);
			if (PageIndex != num)
			{
				if (_nextPageStack != null)
				{
					_nextPageStack.Pop();
				}
				ActivatePage(num);
				_activePage.OnShowFromBack(this);
			}
		}
		finally
		{
			Cursor.Current = Cursors.Default;
		}
	}

	public void BackTo(WizardPage page)
	{
		ActivatePage(page);
		_activePage.OnShowFromBack(this);
	}

	public void MoveToNextPage()
	{
		bool flag = true;
		if (OnButtonNext != null)
		{
			flag = OnButtonNext(this);
		}
		if (flag)
		{
			Next();
		}
	}

	public void MoveToPreviousPage()
	{
		bool flag = true;
		if (OnButtonBack != null)
		{
			flag = OnButtonBack(this);
		}
		if (flag)
		{
			Back();
		}
	}

	public void HideNavigationButtons()
	{
		btnBack.Hide();
		btnNext.Hide();
	}

	public void ShowNavigationButtons()
	{
		btnBack.Show();
		btnNext.Show();
	}

	public void ShowNextButton()
	{
		btnNext.Show();
	}

	public void ShowPreviousButton()
	{
		btnBack.Show();
	}

	public void HideNextButton()
	{
		btnNext.Hide();
	}

	public void HidePreviousButton()
	{
		btnBack.Hide();
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

	public void ReloadResources(CultureInfo culture)
	{
		Resources.Culture = culture;
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(Wizard));
		ReloadResourcesInternal(NavigationPanel, componentResourceManager);
		componentResourceManager.ApplyResources(this, "$this");
		NavigationPanel.PerformLayout();
	}
}
