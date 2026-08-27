using System;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Design;
using System.Windows.Forms;

namespace VideoOS.Common.Controls;

[ToolboxItem(false)]
[Designer(typeof(WizardPageDesigner))]
public class WizardPageBase : UserControl
{
	private Container components;

	private WizardForm m_parent;

	private int m_iIndex = -1;

	private string m_strTitle;

	private string m_strDescription;

	private Image m_imgHeader;

	private bool m_bFireEvents = true;

	private bool m_bFinishPage;

	private bool m_bHelpExists;

	private bool m_bIsWelcome;

	private ImageList m_imgList;

	private int m_iImageIndex = -1;

	[Browsable(true)]
	[DefaultValue(-1)]
	[Description("Order position of wizard page in Wizard Control")]
	[Category("Wizard Page")]
	public int Index
	{
		get
		{
			return m_iIndex;
		}
		set
		{
			if (value != m_iIndex)
			{
				m_iIndex = value;
				OnIndexChanged();
				OnChanged();
			}
		}
	}

	[Browsable(false)]
	[Description("Gets/Sets Page Parent control")]
	public WizardForm WizardPageParent
	{
		get
		{
			return m_parent;
		}
		set
		{
			if (value != m_parent)
			{
				m_parent = value;
				OnWizardPageParentChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page Title Text")]
	public string Title
	{
		get
		{
			return m_strTitle;
		}
		set
		{
			if (value != m_strTitle)
			{
				m_strTitle = value;
				OnTitleChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page description Text")]
	public string Description
	{
		get
		{
			return m_strDescription;
		}
		set
		{
			if (value != m_strDescription)
			{
				m_strDescription = value;
				OnDescriptionChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page special Image")]
	[DefaultValue(null)]
	public Image HeaderImage
	{
		get
		{
			return m_imgHeader;
		}
		set
		{
			if (value != m_imgHeader)
			{
				m_imgHeader = value;
				OnHeaderImageChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets wizard page special ImageList")]
	[DefaultValue(null)]
	public ImageList ImageList
	{
		get
		{
			return m_imgList;
		}
		set
		{
			if (value != m_imgList)
			{
				m_imgList = value;
				OnImageListChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets Image from ImageList which will be used as a HeaderImage")]
	[DefaultValue(-1)]
	[Editor("System.Windows.Forms.Design.ImageIndexEditor, System.Design, Version=1.0.3300.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	[TypeConverter("System.Windows.Forms.ImageIndexConverter")]
	public int ImageIndex
	{
		get
		{
			return m_iImageIndex;
		}
		set
		{
			if (value != m_iImageIndex)
			{
				m_iImageIndex = value;
				OnImageIndexChanged();
				OnChanged();
			}
		}
	}

	[Browsable(false)]
	[DefaultValue(false)]
	public bool QuietMode
	{
		get
		{
			return !m_bFireEvents;
		}
		set
		{
			if (value != m_bFireEvents)
			{
				m_bFireEvents = !value;
				OnQuietModeChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets Is wizard page last in Wizard Control")]
	[DefaultValue(false)]
	public bool FinishPage
	{
		get
		{
			return m_bFinishPage;
		}
		set
		{
			if (value != m_bFinishPage)
			{
				m_bFinishPage = value;
				OnFinishPageChanged();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets Is wizard page Has help Page")]
	[DefaultValue(false)]
	public bool HelpAvailable
	{
		get
		{
			return m_bHelpExists;
		}
		set
		{
			if (value != m_bHelpExists)
			{
				m_bHelpExists = value;
				OnHelpAvailableChangd();
				OnChanged();
			}
		}
	}

	[Browsable(true)]
	[Category("Wizard Page")]
	[Description("Gets/Sets Is wizard page is a Welcome Page")]
	[DefaultValue(false)]
	public bool WelcomePage
	{
		get
		{
			return m_bIsWelcome;
		}
		set
		{
			if (value != m_bIsWelcome)
			{
				m_bIsWelcome = value;
				OnWelcomePageChanged();
				OnChanged();
			}
		}
	}

	protected override Size DefaultSize => new Size(498, 328);

	[Category("Wizard Page")]
	public event EventHandler Changed;

	public WizardPageBase()
	{
		InitializeComponent();
		base.Dock = DockStyle.Fill;
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
		base.Name = "WizardPage";
		base.Size = new System.Drawing.Size(424, 248);
	}

	protected virtual void OnIndexChanged()
	{
	}

	protected virtual void OnWizardPageParentChanged()
	{
	}

	protected virtual void OnTitleChanged()
	{
	}

	protected virtual void OnDescriptionChanged()
	{
	}

	protected virtual void OnHeaderImageChanged()
	{
	}

	protected virtual void OnImageListChanged()
	{
	}

	protected virtual void OnImageIndexChanged()
	{
	}

	protected virtual void OnQuietModeChanged()
	{
	}

	protected virtual void OnFinishPageChanged()
	{
	}

	protected virtual void OnHelpAvailableChangd()
	{
	}

	protected virtual void OnWelcomePageChanged()
	{
	}

	protected virtual void OnChanged()
	{
		if (this.Changed != null && m_bFireEvents)
		{
			this.Changed(this, EventArgs.Empty);
		}
	}
}
