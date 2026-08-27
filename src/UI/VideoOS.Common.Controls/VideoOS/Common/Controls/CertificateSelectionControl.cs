using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Windows.Forms;
using VideoOS.Common.Controls.Properties;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class CertificateSelectionControl : UserControl
{
	private int initialEncryptionCheckBoxHeight;

	private readonly CertificateSelectionControlViewModel _viewModel = new CertificateSelectionControlViewModel();

	private string _encryptionCheckBoxText;

	private IContainer components;

	private CheckBox encryptionCheckBox;

	private Button certificateDetailsButton;

	private Button selectCertificateButton;

	private TextBox selectedCertificateDisplayNameTextBox;

	private TableLayoutPanel tableLayoutPanel1;

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public CertificateSelection2 CertificateSelection
	{
		get
		{
			return _viewModel.CertificateSelection;
		}
		set
		{
			if (value == null)
			{
				throw new ArgumentNullException("value");
			}
			_viewModel.CertificateSelection = value;
			UpdateViewModelState();
		}
	}

	public bool EncryptionEnabled
	{
		get
		{
			return _viewModel.EncryptionCheckBoxChecked;
		}
		set
		{
			_viewModel.EncryptionCheckBoxChecked = value;
			UpdateViewModelState();
		}
	}

	public bool IsSelectionValid => _viewModel.IsSelectionValid;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Text shown for the 'Enable encryption' check box")]
	[Category("Appearance")]
	[Localizable(true)]
	public string EncryptionCheckBoxText
	{
		get
		{
			return _encryptionCheckBoxText;
		}
		set
		{
			encryptionCheckBox.Text = (_encryptionCheckBoxText = value);
			AdjustCheckBoxHeight(encryptionCheckBox, initialEncryptionCheckBoxHeight);
		}
	}

	private bool IsUpdatingModel { get; set; }

	private string SelectedCertificateDisplayName
	{
		get
		{
			switch (_viewModel.SelectionType)
			{
			case CertificateSelectionType.Certificate:
				if (string.IsNullOrEmpty(_viewModel.SelectedCertificateData.FriendlyName))
				{
					return _viewModel.SelectedCertificateData.SimpleSubjectName;
				}
				return _viewModel.SelectedCertificateData.FriendlyName;
			case CertificateSelectionType.NoSelection:
				return Resources.CertificateSelectionControl_Selection_NoSelectionText;
			case CertificateSelectionType.CertificateNotFound:
				return Resources.CertificateSelectionControl_Selection_CertificatenotFoundText;
			default:
				throw new ArgumentException("Unsupported certificate type...");
			}
		}
	}

	public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

	public CertificateSelectionControl()
	{
		InitializeComponent();
		UpdateViewModelState();
	}

	protected override void OnLoad(EventArgs e)
	{
		base.OnLoad(e);
		initialEncryptionCheckBoxHeight = encryptionCheckBox.Height;
		AdjustCheckBoxHeight(encryptionCheckBox, initialEncryptionCheckBoxHeight);
	}

	public void ReloadResources()
	{
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(CertificateSelectionControl));
		if (string.IsNullOrEmpty(EncryptionCheckBoxText))
		{
			encryptionCheckBox.Text = componentResourceManager.GetString("encryptionCheckBox.Text");
		}
		AdjustCheckBoxHeight(encryptionCheckBox, initialEncryptionCheckBoxHeight);
		certificateDetailsButton.Text = componentResourceManager.GetString("certificateDetailsButton.Text");
		selectCertificateButton.Text = componentResourceManager.GetString("selectCertificateButton.Text");
		UpdateViewModelState();
	}

	private void CertificateSelectionControl_Resize(object sender, EventArgs e)
	{
		AdjustCheckBoxHeight(encryptionCheckBox, initialEncryptionCheckBoxHeight);
	}

	private static void AdjustCheckBoxHeight(CheckBox checkBox, int initialHeight)
	{
		if (initialHeight == 0)
		{
			return;
		}
		using Graphics graphics = checkBox.CreateGraphics();
		int num = (int)Math.Ceiling(((double)graphics.MeasureString(checkBox.Text, checkBox.Font).Width + 10.0) / (double)checkBox.Width);
		checkBox.Height = num * (initialHeight - 2) + 2;
	}

	private void UpdateViewModelState()
	{
		if (IsUpdatingModel)
		{
			return;
		}
		try
		{
			IsUpdatingModel = true;
			SuspendLayout();
			encryptionCheckBox.Checked = _viewModel.EncryptionCheckBoxChecked;
			certificateDetailsButton.Enabled = _viewModel.CertificateDetailsButtonEnabled;
			selectCertificateButton.Enabled = _viewModel.SelectCertificateButtonEnabled;
			selectedCertificateDisplayNameTextBox.Text = SelectedCertificateDisplayName;
			ResumeLayout();
		}
		finally
		{
			IsUpdatingModel = false;
		}
	}

	private void encryptionCheckBox_CheckedChanged(object sender, EventArgs e)
	{
		if (!IsUpdatingModel)
		{
			_viewModel.EncryptionCheckBoxChecked = encryptionCheckBox.Checked;
			UpdateViewModelState();
			OnSelectionChanged(new SelectionChangedEventArgs(_viewModel.IsSelectionValid));
		}
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		return new Size(proposedSize.Width, base.Padding.Top + encryptionCheckBox.Margin.Top + encryptionCheckBox.Height + encryptionCheckBox.Margin.Bottom + certificateDetailsButton.Margin.Top + certificateDetailsButton.Height + certificateDetailsButton.Margin.Bottom + base.Padding.Bottom);
	}

	private void certificateDetailsButton_Click(object sender, EventArgs e)
	{
		if (_viewModel.SelectedCertificateData != null)
		{
			X509Certificate2UI.DisplayCertificate(_viewModel.SelectedCertificateData.Certificate, base.Handle);
		}
	}

	private void selectCertificateButton_Click(object sender, EventArgs e)
	{
		X509Certificate2Collection x509Certificate2Collection = X509Certificate2UI.SelectFromCollection(GetAvailableCertificatesToDisplay(), Resources.CertificateSelectionControl_ChooseCertificateWindowTitle, Resources.CertificateSelectionControl_ChooseCertificateWindowMessage, X509SelectionFlag.SingleSelection, base.Handle);
		if (x509Certificate2Collection.Count == 0)
		{
			_viewModel.CertificateSelection = CertificateSelection2.NoSelection;
		}
		else
		{
			_viewModel.CertificateSelection = CertificateSelection2.FromCertificate(x509Certificate2Collection[0]);
		}
		UpdateViewModelState();
		OnSelectionChanged(new SelectionChangedEventArgs(_viewModel.IsSelectionValid));
	}

	private void OnSelectionChanged(SelectionChangedEventArgs e)
	{
		this.SelectionChanged?.Invoke(this, e);
	}

	private X509Certificate2Collection GetAvailableCertificatesToDisplay()
	{
		return new X509Certificate2Collection(_viewModel.AvailableCertificates.Select((CertificateData certificateData) => certificateData.Certificate).ToArray());
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.CertificateSelectionControl));
		this.encryptionCheckBox = new System.Windows.Forms.CheckBox();
		this.certificateDetailsButton = new System.Windows.Forms.Button();
		this.selectCertificateButton = new System.Windows.Forms.Button();
		this.selectedCertificateDisplayNameTextBox = new System.Windows.Forms.TextBox();
		this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
		this.tableLayoutPanel1.SuspendLayout();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.encryptionCheckBox, "encryptionCheckBox");
		this.tableLayoutPanel1.SetColumnSpan(this.encryptionCheckBox, 3);
		this.encryptionCheckBox.Name = "encryptionCheckBox";
		this.encryptionCheckBox.UseVisualStyleBackColor = true;
		this.encryptionCheckBox.CheckedChanged += new System.EventHandler(encryptionCheckBox_CheckedChanged);
		componentResourceManager.ApplyResources(this.certificateDetailsButton, "certificateDetailsButton");
		this.certificateDetailsButton.Name = "certificateDetailsButton";
		this.certificateDetailsButton.UseVisualStyleBackColor = true;
		this.certificateDetailsButton.Click += new System.EventHandler(certificateDetailsButton_Click);
		componentResourceManager.ApplyResources(this.selectCertificateButton, "selectCertificateButton");
		this.selectCertificateButton.Name = "selectCertificateButton";
		this.selectCertificateButton.UseVisualStyleBackColor = true;
		this.selectCertificateButton.Click += new System.EventHandler(selectCertificateButton_Click);
		componentResourceManager.ApplyResources(this.selectedCertificateDisplayNameTextBox, "selectedCertificateDisplayNameTextBox");
		this.selectedCertificateDisplayNameTextBox.Name = "selectedCertificateDisplayNameTextBox";
		this.selectedCertificateDisplayNameTextBox.ReadOnly = true;
		this.selectedCertificateDisplayNameTextBox.TabStop = false;
		componentResourceManager.ApplyResources(this.tableLayoutPanel1, "tableLayoutPanel1");
		this.tableLayoutPanel1.Controls.Add(this.encryptionCheckBox, 0, 0);
		this.tableLayoutPanel1.Controls.Add(this.selectedCertificateDisplayNameTextBox, 0, 1);
		this.tableLayoutPanel1.Controls.Add(this.certificateDetailsButton, 2, 1);
		this.tableLayoutPanel1.Controls.Add(this.selectCertificateButton, 1, 1);
		this.tableLayoutPanel1.Name = "tableLayoutPanel1";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tableLayoutPanel1);
		base.Name = "CertificateSelectionControl";
		base.Resize += new System.EventHandler(CertificateSelectionControl_Resize);
		this.tableLayoutPanel1.ResumeLayout(false);
		this.tableLayoutPanel1.PerformLayout();
		base.ResumeLayout(false);
	}
}
