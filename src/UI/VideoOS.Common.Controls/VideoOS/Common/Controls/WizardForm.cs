using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(true)]
[ToolboxBitmap(typeof(WizardForm), "VideoOS.UI.Controls.Wizards.WizardForm.bmp")]
[Designer(typeof(WizardFormDesigner))]
public class WizardForm : UserControl
{
	[Flags]
	public enum TWizardsButtons
	{
		[Browsable(false)]
		None = 0,
		[Description("Enable or Disable Previous Button")]
		Previous = 1,
		[Description("Enable or Disable Next/Finish button")]
		Next = 2,
		[Description("If checked then Next button become a Finish Button")]
		Finish = 4,
		[Description("Show or not Help button")]
		Help = 0x10,
		[Description("Enable or Disable Cancel button")]
		Cancel = 0x20,
		[Browsable(false)]
		All = 0x33,
		[Browsable(false)]
		AllShort = 0x23,
		[Browsable(false)]
		AllShortFinishDisabled = 0x25,
		[Browsable(false)]
		AllShortFinishEnabled = 0x27,
		[Browsable(false)]
		AllFinishEnabled = 0x37,
		[Browsable(false)]
		AllFinishDisabled = 0x35
	}

	public enum THelpPosition
	{
		AlignLeft,
		AlignRight
	}

	public class CollectionComapre : IComparer
	{
		public int Compare(object x, object y)
		{
			WizardPageBase wizardPageBase = (WizardPageBase)x;
			WizardPageBase wizardPageBase2 = (WizardPageBase)y;
			if (wizardPageBase.Index <= wizardPageBase2.Index)
			{
				if (wizardPageBase.Index >= wizardPageBase2.Index)
				{
					return 0;
				}
				return -1;
			}
			return 1;
		}
	}

	public class WizardPagesCollection : CollectionBase
	{
		private WizardForm m_parent;

		public WizardPageBase this[int index]
		{
			get
			{
				return base.InnerList[index] as WizardPageBase;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				value.WizardPageParent = m_parent;
				value.Index = index;
				base.InnerList[index] = value;
				RaiseChanged(value);
			}
		}

		public event EventHandler Changed;

		public WizardPagesCollection(WizardForm parent)
		{
			m_parent = parent;
		}

		public int Add(WizardPageBase value)
		{
			if (Contains(value))
			{
				return IndexOf(value);
			}
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			value.WizardPageParent = m_parent;
			int num = base.InnerList.Add(value);
			if (value.Index < 0)
			{
				value.Index = num;
			}
			RaiseChanged(value);
			return num;
		}

		public void AddRange(WizardPageBase[] array)
		{
			if (array == null)
			{
				throw new ArgumentNullException("array");
			}
			foreach (WizardPageBase value in array)
			{
				Add(value);
			}
		}

		public bool Contains(WizardPageBase value)
		{
			return base.InnerList.Contains(value);
		}

		public int IndexOf(WizardPageBase value)
		{
			return base.InnerList.IndexOf(value);
		}

		public void Insert(int index, WizardPageBase value)
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			value.WizardPageParent = m_parent;
			base.InnerList.Insert(index, value);
			if (value.Index < 0)
			{
				value.Index = index;
			}
			RaiseChanged(value);
		}

		public void Remove(WizardPageBase value)
		{
			base.InnerList.Remove(value);
			RaiseChanged();
		}

		protected void RaiseChanged(WizardPageBase value)
		{
			if (this.Changed != null)
			{
				this.Changed(value, EventArgs.Empty);
			}
		}

		protected void RaiseChanged()
		{
			if (this.Changed != null)
			{
				this.Changed(this, EventArgs.Empty);
			}
		}

		public void Sort()
		{
			base.InnerList.Sort(new CollectionComapre());
		}

		protected override void OnClearComplete()
		{
			RaiseChanged();
		}

		protected override void OnSetComplete(int index, object oldValue, object newValue)
		{
			WizardPageBase wizardPageBase = (WizardPageBase)newValue;
			wizardPageBase.WizardPageParent = m_parent;
			if (wizardPageBase.Index < 0)
			{
				wizardPageBase.Index = index;
			}
			base.OnSetComplete(index, oldValue, wizardPageBase);
			RaiseChanged(wizardPageBase);
		}

		protected override void OnInsertComplete(int index, object value)
		{
			WizardPageBase wizardPageBase = (WizardPageBase)value;
			wizardPageBase.WizardPageParent = m_parent;
			if (wizardPageBase.Index < 0)
			{
				wizardPageBase.Index = index;
			}
			base.OnInsertComplete(index, wizardPageBase);
			RaiseChanged(wizardPageBase);
		}

		protected override void OnRemoveComplete(int index, object value)
		{
			RaiseChanged();
		}
	}

	public class EventWizardCancelArgs : EventArgs
	{
		private string m_strMessage;

		private string m_strTitle;

		public string Message
		{
			get
			{
				return m_strMessage;
			}
			set
			{
				m_strMessage = value;
			}
		}

		public string Title
		{
			get
			{
				return m_strTitle;
			}
			set
			{
				m_strTitle = value;
			}
		}

		private EventWizardCancelArgs()
		{
		}

		public EventWizardCancelArgs(string title, string message)
		{
			m_strTitle = title;
			m_strMessage = message;
		}
	}

	public class EventNextArgs : EventArgs
	{
		private WizardPageBase m_page;

		private int m_iStep = 1;

		public WizardPageBase CurrentPage => m_page;

		public int Step
		{
			get
			{
				return m_iStep;
			}
			set
			{
				m_iStep = value;
			}
		}

		private EventNextArgs()
		{
		}

		public EventNextArgs(WizardPageBase page)
		{
			m_page = page;
		}

		public EventNextArgs(WizardPageBase page, int step)
			: this(page)
		{
			m_iStep = step;
		}
	}

	public delegate void WizardCancelEventHandler(object sender, EventWizardCancelArgs e);

	public delegate void WizardNextEventHandler(object sender, EventNextArgs e);

	public static string DEF_FINISH = "&Finish";

	public static string DEF_NEXT = "&Next >";

	public static string DEF_BACK = "< &Back";

	public static string DEF_HELP = "&Help";

	public static string DEF_CANCEL = "&Cancel";

	public static string DEF_CLOSE = "Do you want to close Wizard and loose all Data entered on Wizard Pages?";

	public static string DEF_WARNING = "Warning";

	private TWizardsButtons m_eButtons = TWizardsButtons.All;

	private THelpPosition m_eHelpPosition;

	private bool m_bFireEvents = true;

	private bool m_bShowConfirm = true;

	private bool m_bFinishCalled;

	private bool m_bCancelCalled;

	private WizardPagesCollection m_pages;

	private int m_iShowIndex;

	private int m_iLastShown = -1;

	private WizardPageBase m_CurrentPage;

	private Panel pnlHeader;

	private PictureBox imgHeader;

	private Label lblTitle;

	private Label lblDescription;

	private LineFrame lineDownLimit;

	private Panel pnlBody;

	private Button btnCancel;

	private Button btnNextFinish;

	private Button btnBack;

	private Button btnHelp;

	private Panel pnlMain;

	private Container components;

	[Browsable(true)]
	[Category("Wizard")]
	[Description("Gets/Sets buttons states in wizard control for current Wizard Page")]
	[DefaultValue(typeof(TWizardsButtons), "All")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	[Editor("UtilityLibrary.Designers.FlagsEditor, UtilityLibrary.Designers, Version=1.0.4.0, Culture=neutral, PublicKeyToken=fe06e967d3cf723d", typeof(UITypeEditor))]
	public TWizardsButtons WizardButtons
	{
		get
		{
			return m_eButtons;
		}
		set
		{
			if (value != m_eButtons)
			{
				m_eButtons = value;
				OnWizardButtonChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard")]
	[Description("Gets/Sets Help button align on wizard Control")]
	[DefaultValue(typeof(THelpPosition), "AlignLeft")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public THelpPosition HelpAlign
	{
		get
		{
			return m_eHelpPosition;
		}
		set
		{
			if (value != m_eHelpPosition)
			{
				m_eHelpPosition = value;
				OnHelpAlignChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard")]
	[Description("Gets collection of wizard pages")]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
	public WizardPagesCollection Pages => m_pages;

	[Browsable(false)]
	[Description("Gets/Sets QUIET Mode of control")]
	[DefaultValue(false)]
	public bool QuietMode
	{
		get
		{
			return !m_bFireEvents;
		}
		set
		{
			m_bFireEvents = !value;
		}
	}

	[Browsable(true)]
	[Category("Wizard")]
	[Description("Gets/Sets Does Wizard Show confirmation dialog on Cancel button Press")]
	[DefaultValue(true)]
	public bool ShowCancelConfirm
	{
		get
		{
			return m_bShowConfirm;
		}
		set
		{
			m_bShowConfirm = value;
		}
	}

	[Browsable(true)]
	[Category("Wizard")]
	[Description("Gets/Sets Current page of wizard")]
	public int PageIndex
	{
		get
		{
			return m_iShowIndex;
		}
		set
		{
			if (value != m_iShowIndex && value < m_pages.Count && value >= 0)
			{
				m_iShowIndex = value;
				ShowWizardPage();
			}
		}
	}

	[Category("Property Changed")]
	public event EventHandler WizardButtonsChanged;

	[Category("Property Changed")]
	public event EventHandler HelpAlignChanged;

	[Category("Wizard")]
	public event WizardNextEventHandler Next;

	[Category("Wizard")]
	public event WizardNextEventHandler Finish;

	[Category("Wizard")]
	public event WizardNextEventHandler Back;

	[Category("Wizard")]
	public event WizardCancelEventHandler Cancel;

	[Category("Wizard")]
	public event WizardNextEventHandler PageShown;

	[Category("Wizard")]
	public event EventHandler Help;

	[Category("Wizard")]
	public event EventHandler WizardClosed;

	public WizardForm()
	{
		InitializeComponent();
		m_pages = new WizardPagesCollection(this);
		m_pages.Changed += PagesCollectionChanged;
		btnBack.Text = DEF_BACK;
		btnHelp.Text = DEF_HELP;
		btnNextFinish.Text = DEF_NEXT;
		btnCancel.Text = DEF_CANCEL;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.WizardForm));
		this.btnCancel = new System.Windows.Forms.Button();
		this.pnlHeader = new System.Windows.Forms.Panel();
		this.lblTitle = new System.Windows.Forms.Label();
		this.lblDescription = new System.Windows.Forms.Label();
		this.imgHeader = new System.Windows.Forms.PictureBox();
		this.btnNextFinish = new System.Windows.Forms.Button();
		this.btnBack = new System.Windows.Forms.Button();
		this.lineDownLimit = new VideoOS.Common.Controls.LineFrame();
		this.pnlBody = new System.Windows.Forms.Panel();
		this.btnHelp = new System.Windows.Forms.Button();
		this.pnlMain = new System.Windows.Forms.Panel();
		this.pnlHeader.SuspendLayout();
		((System.ComponentModel.ISupportInitialize)this.imgHeader).BeginInit();
		this.pnlMain.SuspendLayout();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.btnCancel, "btnCancel");
		this.btnCancel.Name = "btnCancel";
		this.btnCancel.Click += new System.EventHandler(btnCancel_Click);
		this.btnCancel.KeyDown += new System.Windows.Forms.KeyEventHandler(WizardForm_KeyDown);
		this.pnlHeader.BackColor = System.Drawing.Color.White;
		this.pnlHeader.Controls.Add(this.lblTitle);
		this.pnlHeader.Controls.Add(this.lblDescription);
		this.pnlHeader.Controls.Add(this.imgHeader);
		componentResourceManager.ApplyResources(this.pnlHeader, "pnlHeader");
		this.pnlHeader.Name = "pnlHeader";
		componentResourceManager.ApplyResources(this.lblTitle, "lblTitle");
		this.lblTitle.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.lblTitle.Name = "lblTitle";
		componentResourceManager.ApplyResources(this.lblDescription, "lblDescription");
		this.lblDescription.FlatStyle = System.Windows.Forms.FlatStyle.System;
		this.lblDescription.Name = "lblDescription";
		componentResourceManager.ApplyResources(this.imgHeader, "imgHeader");
		this.imgHeader.Name = "imgHeader";
		this.imgHeader.TabStop = false;
		componentResourceManager.ApplyResources(this.btnNextFinish, "btnNextFinish");
		this.btnNextFinish.BackColor = System.Drawing.SystemColors.Control;
		this.btnNextFinish.Cursor = System.Windows.Forms.Cursors.Default;
		this.btnNextFinish.Name = "btnNextFinish";
		this.btnNextFinish.UseVisualStyleBackColor = false;
		this.btnNextFinish.Click += new System.EventHandler(btnNextFinish_Click);
		this.btnNextFinish.KeyDown += new System.Windows.Forms.KeyEventHandler(WizardForm_KeyDown);
		componentResourceManager.ApplyResources(this.btnBack, "btnBack");
		this.btnBack.Name = "btnBack";
		this.btnBack.Click += new System.EventHandler(btnBack_Click);
		this.btnBack.KeyDown += new System.Windows.Forms.KeyEventHandler(WizardForm_KeyDown);
		componentResourceManager.ApplyResources(this.lineDownLimit, "lineDownLimit");
		this.lineDownLimit.Name = "lineDownLimit";
		this.lineDownLimit.TabStop = false;
		componentResourceManager.ApplyResources(this.pnlBody, "pnlBody");
		this.pnlBody.Name = "pnlBody";
		componentResourceManager.ApplyResources(this.btnHelp, "btnHelp");
		this.btnHelp.Name = "btnHelp";
		this.btnHelp.Click += new System.EventHandler(btnHelp_Click);
		this.btnHelp.KeyDown += new System.Windows.Forms.KeyEventHandler(WizardForm_KeyDown);
		componentResourceManager.ApplyResources(this.pnlMain, "pnlMain");
		this.pnlMain.Controls.Add(this.pnlBody);
		this.pnlMain.Controls.Add(this.pnlHeader);
		this.pnlMain.Name = "pnlMain";
		base.Controls.Add(this.pnlMain);
		base.Controls.Add(this.lineDownLimit);
		base.Controls.Add(this.btnBack);
		base.Controls.Add(this.btnNextFinish);
		base.Controls.Add(this.btnCancel);
		base.Controls.Add(this.btnHelp);
		base.Name = "WizardForm";
		componentResourceManager.ApplyResources(this, "$this");
		base.Load += new System.EventHandler(WizardForm_Load);
		base.KeyDown += new System.Windows.Forms.KeyEventHandler(WizardForm_KeyDown);
		this.pnlHeader.ResumeLayout(false);
		((System.ComponentModel.ISupportInitialize)this.imgHeader).EndInit();
		this.pnlMain.ResumeLayout(false);
		base.ResumeLayout(false);
	}

	protected void ShowWizardPage()
	{
		if (m_iShowIndex == m_iLastShown)
		{
			return;
		}
		pnlMain.SuspendLayout();
		pnlBody.SuspendLayout();
		pnlMain.Controls.Clear();
		pnlBody.Controls.Clear();
		if (m_pages.Count > 0)
		{
			m_pages.Sort();
			if (m_iShowIndex < 0)
			{
				m_iShowIndex = 0;
			}
			if (m_iShowIndex >= m_pages.Count)
			{
				m_iShowIndex = m_pages.Count - 1;
			}
			m_CurrentPage = m_pages[m_iLastShown = m_iShowIndex];
			if (m_CurrentPage.WizardPageParent != this)
			{
				m_CurrentPage.WizardPageParent = this;
			}
			if (m_CurrentPage.HeaderImage != null)
			{
				imgHeader.Image = m_CurrentPage.HeaderImage;
			}
			else if (m_CurrentPage.ImageList != null && m_CurrentPage.ImageIndex >= 0 && m_CurrentPage.ImageIndex < m_CurrentPage.ImageList.Images.Count)
			{
				imgHeader.Image = m_CurrentPage.ImageList.Images[m_CurrentPage.ImageIndex];
			}
			else
			{
				imgHeader.Image = null;
			}
			lblTitle.Text = m_CurrentPage.Title;
			lblDescription.Text = m_CurrentPage.Description;
			m_CurrentPage.Changed += WizardPageAttributesChanged;
			if (!m_CurrentPage.WelcomePage)
			{
				pnlMain.Controls.AddRange(new Control[2] { pnlBody, pnlHeader });
				pnlBody.Width = pnlMain.Width - 16;
				pnlBody.Height = pnlMain.Height - pnlHeader.Height - 8;
				if (m_CurrentPage.Width != pnlBody.Width || m_CurrentPage.Height != pnlBody.Height)
				{
					m_CurrentPage.Width = pnlBody.Width;
					m_CurrentPage.Height = pnlBody.Height;
				}
				pnlBody.Controls.Add(m_CurrentPage);
				pnlBody.Controls[0].Focus();
			}
			else
			{
				if (m_CurrentPage.Width != pnlMain.Width || m_CurrentPage.Height != pnlMain.Height)
				{
					m_CurrentPage.Size = pnlMain.Size;
				}
				pnlMain.Controls.Add(m_CurrentPage);
				pnlMain.Controls[0].Focus();
			}
			if (m_CurrentPage.FinishPage)
			{
				m_eButtons |= TWizardsButtons.Finish;
			}
			else
			{
				m_eButtons &= ~TWizardsButtons.Finish;
			}
			ChangeButtonsStates();
		}
		else
		{
			pnlMain.Controls.AddRange(new Control[2] { pnlBody, pnlHeader });
		}
		pnlBody.ResumeLayout();
		pnlMain.ResumeLayout();
		OnPageShown();
	}

	protected void ChangeButtonsStates()
	{
		int top = btnNextFinish.Top;
		int left = base.Width;
		bool flag = (m_eButtons & TWizardsButtons.Help) == TWizardsButtons.Help;
		bool enabled = (m_eButtons & TWizardsButtons.Cancel) == TWizardsButtons.Cancel;
		bool enabled2 = (m_eButtons & TWizardsButtons.Next) == TWizardsButtons.Next;
		bool enabled3 = (m_eButtons & TWizardsButtons.Previous) == TWizardsButtons.Previous;
		bool flag2 = (m_eButtons & TWizardsButtons.Finish) == TWizardsButtons.Finish;
		btnHelp.Visible = flag;
		btnHelp.Enabled = flag;
		if (flag)
		{
			if (m_eHelpPosition == THelpPosition.AlignLeft)
			{
				btnHelp.Left = 8;
				btnHelp.Top = top;
				btnHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
			}
			else
			{
				btnHelp.Left = base.Width - btnHelp.Width - 8;
				left = btnHelp.Left;
				btnHelp.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
			}
			if (m_CurrentPage != null)
			{
				btnHelp.Enabled = m_CurrentPage.HelpAvailable;
			}
		}
		btnCancel.Left = left - btnCancel.Width - 8;
		btnCancel.Top = top;
		btnCancel.Enabled = enabled;
		left = btnCancel.Left;
		btnNextFinish.Left = left - btnNextFinish.Width - 8;
		btnNextFinish.Top = top;
		btnNextFinish.Enabled = enabled2;
		btnNextFinish.Text = (flag2 ? DEF_FINISH : DEF_NEXT);
		btnNextFinish.DialogResult = (flag2 ? DialogResult.OK : DialogResult.None);
		left = btnNextFinish.Left;
		btnBack.Left = left - btnBack.Width;
		btnBack.Top = top;
		btnBack.Enabled = enabled3;
		if (m_CurrentPage != null && m_CurrentPage.Index == 0 && btnBack.Enabled)
		{
			btnBack.Enabled = false;
		}
	}

	public void WizardPageAttributesChanged(object sender, EventArgs e)
	{
		((WizardPageBase)sender).Changed -= WizardPageAttributesChanged;
		m_iLastShown = -1;
		ShowWizardPage();
	}

	public bool OnDesignedWizardButtons(Point p)
	{
		Rectangle rectangle = new Rectangle(btnNextFinish.Location, btnNextFinish.Size);
		Rectangle rectangle2 = new Rectangle(btnBack.Location, btnBack.Size);
		if (!rectangle.Contains(p))
		{
			return rectangle2.Contains(p);
		}
		return true;
	}

	public Rectangle GetCurrentPageRect()
	{
		if (m_CurrentPage != null)
		{
			if (m_CurrentPage.WelcomePage)
			{
				return pnlMain.ClientRectangle;
			}
			return pnlBody.ClientRectangle;
		}
		return Rectangle.Empty;
	}

	public void MoveNextStep()
	{
		m_iShowIndex++;
		ShowWizardPage();
	}

	public void MoveBackStep()
	{
		m_iShowIndex--;
		ShowWizardPage();
	}

	public void StepNext(int count)
	{
		if (count != 0)
		{
			m_iShowIndex += count;
			ShowWizardPage();
		}
	}

	public void StepBack(int count)
	{
		if (count != 0)
		{
			m_iShowIndex -= count;
			ShowWizardPage();
		}
	}

	protected void AttachControlEvents(object sender, ControlEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		e.Control.KeyDown += WizardForm_KeyDown;
	}

	protected void DetachControlEvents(object sender, ControlEventArgs e)
	{
		if (e == null)
		{
			throw new ArgumentNullException("e");
		}
		e.Control.KeyDown -= WizardForm_KeyDown;
	}

	protected virtual void PagesCollectionChanged(object sender, EventArgs e)
	{
		if (m_pages.Count > 0)
		{
			if (sender is WizardPageBase)
			{
				WizardPageBase obj = (WizardPageBase)sender;
				obj.ControlAdded -= AttachControlEvents;
				obj.ControlRemoved -= DetachControlEvents;
				obj.ControlAdded += AttachControlEvents;
				obj.ControlRemoved += DetachControlEvents;
			}
			m_iLastShown = -1;
			ShowWizardPage();
		}
	}

	protected virtual void OnWizardButtonChanged()
	{
		if (this.WizardButtonsChanged != null && m_bFireEvents)
		{
			this.WizardButtonsChanged(this, EventArgs.Empty);
		}
		ChangeButtonsStates();
	}

	protected virtual void OnHelpAlignChanged()
	{
		if (this.HelpAlignChanged != null && m_bFireEvents)
		{
			this.HelpAlignChanged(this, EventArgs.Empty);
		}
		ChangeButtonsStates();
	}

	protected virtual void OnNextPressed()
	{
		EventNextArgs eventNextArgs = new EventNextArgs(m_CurrentPage);
		if (this.Next != null && m_bFireEvents)
		{
			this.Next(m_CurrentPage, eventNextArgs);
		}
		StepNext(eventNextArgs.Step);
	}

	protected virtual void OnFinishPressed()
	{
		EventNextArgs e = new EventNextArgs(m_CurrentPage);
		if (this.Finish != null && m_bFireEvents)
		{
			m_bFinishCalled = true;
			this.Finish(m_CurrentPage, e);
		}
	}

	protected virtual void OnBackPressed()
	{
		EventNextArgs eventNextArgs = new EventNextArgs(m_CurrentPage);
		if (this.Back != null && m_bFireEvents)
		{
			this.Back(m_CurrentPage, eventNextArgs);
		}
		StepBack(eventNextArgs.Step);
	}

	protected virtual DialogResult OnCancelPressed()
	{
		EventWizardCancelArgs eventWizardCancelArgs = new EventWizardCancelArgs(DEF_WARNING, DEF_CLOSE);
		if (this.Cancel != null && m_bFireEvents)
		{
			this.Cancel(this, eventWizardCancelArgs);
		}
		if (m_bShowConfirm && MessageBox.Show(this, eventWizardCancelArgs.Message, eventWizardCancelArgs.Title, MessageBoxButtons.YesNo) == DialogResult.Yes)
		{
			m_bCancelCalled = true;
			OnWizardClosed();
			return DialogResult.Yes;
		}
		return DialogResult.No;
	}

	protected virtual void OnHelpPressed()
	{
		if (this.Help != null && m_bFireEvents)
		{
			this.Help(m_CurrentPage, EventArgs.Empty);
		}
	}

	protected virtual void OnWizardClosed()
	{
		if (this.WizardClosed != null && m_bFireEvents)
		{
			this.WizardClosed(m_CurrentPage, EventArgs.Empty);
		}
	}

	protected virtual void OnPageShown()
	{
		EventNextArgs e = new EventNextArgs(m_CurrentPage, m_pages.IndexOf(m_CurrentPage));
		if (this.PageShown != null && m_bFireEvents)
		{
			this.PageShown(m_CurrentPage, e);
		}
	}

	private void btnCancel_Click(object sender, EventArgs e)
	{
		if (btnCancel.Visible && btnCancel.Enabled)
		{
			OnCancelPressed();
		}
	}

	private void btnNextFinish_Click(object sender, EventArgs e)
	{
		if (!btnNextFinish.Visible || !btnNextFinish.Enabled)
		{
			return;
		}
		if (m_CurrentPage != null)
		{
			if (m_CurrentPage.FinishPage && !base.DesignMode)
			{
				OnFinishPressed();
			}
			else
			{
				OnNextPressed();
			}
		}
		else
		{
			btnNextFinish.Enabled = false;
		}
	}

	private void btnBack_Click(object sender, EventArgs e)
	{
		if (btnBack.Visible && btnBack.Enabled)
		{
			OnBackPressed();
		}
	}

	private void btnHelp_Click(object sender, EventArgs e)
	{
		if (btnHelp.Visible && btnHelp.Enabled)
		{
			OnHelpPressed();
		}
	}

	private void WizardForm_KeyDown(object sender, KeyEventArgs e)
	{
		if ((e.Modifiers & Keys.Alt) == Keys.Alt)
		{
			if (e.KeyCode == Keys.N && btnNextFinish.Enabled)
			{
				btnNextFinish.Focus();
			}
			else if (e.KeyCode == Keys.B && btnBack.Enabled)
			{
				btnBack.Focus();
			}
			else if (e.KeyCode == Keys.C && btnCancel.Enabled)
			{
				btnCancel.Focus();
			}
			else if (e.KeyCode == Keys.H && btnHelp.Enabled)
			{
				btnHelp.Focus();
			}
		}
		if ((e.Modifiers & (Keys.Shift | Keys.Control | Keys.Alt)) != Keys.None)
		{
			return;
		}
		if (e.KeyCode == Keys.Escape)
		{
			if (!(sender is ComboBox) || !((ComboBox)sender).DroppedDown)
			{
				btnCancel_Click(sender, e);
			}
		}
		else
		{
			if (e.KeyCode != Keys.Return)
			{
				return;
			}
			if (sender.Equals(btnCancel))
			{
				btnCancel_Click(sender, e);
			}
			else if (sender.Equals(btnBack))
			{
				btnBack_Click(sender, e);
			}
			else if (sender.Equals(btnHelp))
			{
				btnHelp_Click(sender, e);
			}
			else if (sender is TextBoxBase)
			{
				if (!((TextBoxBase)sender).Multiline)
				{
					btnNextFinish_Click(sender, e);
				}
			}
			else if (sender is ComboBox)
			{
				if (!((ComboBox)sender).DroppedDown)
				{
					btnNextFinish_Click(sender, e);
				}
			}
			else
			{
				btnNextFinish_Click(sender, e);
			}
		}
	}

	private void WizardForm_Closing(object sender, CancelEventArgs e)
	{
		if (m_bFinishCalled || m_bCancelCalled)
		{
			m_bFinishCalled = (m_bCancelCalled = false);
		}
		else if (btnCancel.Visible && btnCancel.Enabled && OnCancelPressed() == DialogResult.No)
		{
			e.Cancel = true;
		}
	}

	private void WizardForm_Load(object sender, EventArgs e)
	{
		FindForm().KeyDown += WizardForm_KeyDown;
		FindForm().AcceptButton = btnNextFinish;
	}
}
