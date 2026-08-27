using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using VideoOS.Common.Integration;

namespace VideoOS.Common.Controls;

public class MultipleCertificateSelectionControl : UserControl
{
	public class ServiceEncryptionSettings
	{
		private MultipleCertificateSelectionViewModel.ServiceViewModel serviceSettingsViewModel;

		public bool EncryptionEnabled
		{
			get
			{
				return serviceSettingsViewModel.EncryptionEnabledResolved;
			}
			set
			{
				serviceSettingsViewModel.EncryptionEnabled = value;
			}
		}

		[Browsable(false)]
		[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
		public CertificateSelection2 CertificateSelection
		{
			get
			{
				return serviceSettingsViewModel.CertificateSelectionResolved;
			}
			set
			{
				serviceSettingsViewModel.CertificateSelection = value;
			}
		}

		public ServiceEncryptionSettings(MultipleCertificateSelectionViewModel.ServiceViewModel serviceSettingsViewModel)
		{
			this.serviceSettingsViewModel = serviceSettingsViewModel;
		}
	}

	private readonly MultipleCertificateSelectionViewModel viewModel;

	private bool isUpdatingFromViewModel;

	private IContainer components;

	private TableLayoutPanel tableLayoutPanel;

	private Panel allServicesPanel;

	private Panel individualSettingsPanel;

	private CertificateSelectionControl secondaryServiceCertificateControl;

	private CertificateSelectionControl primaryServiceCertificateControl;

	private Panel sameSettingsPanel;

	private CertificateSelectionControl allServicesCertificateControl;

	private Label spaceLabel;

	private Panel radioButtonsPanel;

	private RadioButton configureServersSameRadioButton;

	private RadioButton configureServersIndividuallyRadioButton;

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Text shown for the 'Enable encryption' check box for the all services ")]
	[Category("Appearance")]
	[Localizable(true)]
	public string EncryptionCheckBoxTextAllServices
	{
		get
		{
			return allServicesCertificateControl.EncryptionCheckBoxText;
		}
		set
		{
			allServicesCertificateControl.EncryptionCheckBoxText = value;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Text shown for the 'Enable encryption' check box for the primary service")]
	[Category("Appearance")]
	[Localizable(true)]
	public string EncryptionCheckBoxTextPrimaryService
	{
		get
		{
			return primaryServiceCertificateControl.EncryptionCheckBoxText;
		}
		set
		{
			primaryServiceCertificateControl.EncryptionCheckBoxText = value;
		}
	}

	[Browsable(true)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
	[Description("Text shown for the 'Enable encryption' check box for the secondary service")]
	[Category("Appearance")]
	[Localizable(true)]
	public string EncryptionCheckBoxTextSecondaryService
	{
		get
		{
			return secondaryServiceCertificateControl.EncryptionCheckBoxText;
		}
		set
		{
			secondaryServiceCertificateControl.EncryptionCheckBoxText = value;
		}
	}

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ServiceEncryptionSettings PrimaryService { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public ServiceEncryptionSettings SecondaryService { get; }

	[Browsable(false)]
	[DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
	public bool IsSelectionValid => viewModel.IsSelectionValid;

	public event EventHandler<SelectionChangedEventArgs> SelectionChanged;

	private void OnSelectionChanged(SelectionChangedEventArgs e)
	{
		this.SelectionChanged?.Invoke(this, e);
	}

	public MultipleCertificateSelectionControl()
	{
		InitializeComponent();
		viewModel = new MultipleCertificateSelectionViewModel();
		PrimaryService = new ServiceEncryptionSettings(viewModel.PrimaryServiceViewModel);
		SecondaryService = new ServiceEncryptionSettings(viewModel.SecondaryServiceViewModel);
		allServicesCertificateControl.SelectionChanged += delegate
		{
			CertificateControl_SelectionChanged(allServicesCertificateControl, viewModel.AllServicesViewModel);
		};
		primaryServiceCertificateControl.SelectionChanged += delegate
		{
			CertificateControl_SelectionChanged(primaryServiceCertificateControl, viewModel.PrimaryServiceViewModel);
		};
		secondaryServiceCertificateControl.SelectionChanged += delegate
		{
			CertificateControl_SelectionChanged(secondaryServiceCertificateControl, viewModel.SecondaryServiceViewModel);
		};
	}

	private void CertificateControl_SelectionChanged(CertificateSelectionControl control, MultipleCertificateSelectionViewModel.ServiceViewModel serviceViewModel)
	{
		serviceViewModel.EncryptionEnabled = control.EncryptionEnabled;
		serviceViewModel.CertificateSelection = control.CertificateSelection;
		serviceViewModel.IsSelectionValid = control.IsSelectionValid;
		OnSelectionChanged(new SelectionChangedEventArgs(IsSelectionValid));
	}

	private void UpdateStateFromViewModel()
	{
		if (isUpdatingFromViewModel)
		{
			return;
		}
		try
		{
			isUpdatingFromViewModel = true;
			SuspendLayout();
			switch (viewModel.SelectedMode)
			{
			case MultipleCertificateSelectionViewModel.Mode.SameCertificateForAllServices:
				configureServersSameRadioButton.Checked = true;
				allServicesPanel.Visible = true;
				individualSettingsPanel.Visible = false;
				break;
			case MultipleCertificateSelectionViewModel.Mode.IndividualCertificateForEachService:
				configureServersIndividuallyRadioButton.Checked = true;
				allServicesPanel.Visible = false;
				individualSettingsPanel.Visible = true;
				break;
			}
			allServicesCertificateControl.EncryptionEnabled = viewModel.AllServicesViewModel.EncryptionEnabled;
			allServicesCertificateControl.CertificateSelection = viewModel.AllServicesViewModel.CertificateSelection;
			primaryServiceCertificateControl.EncryptionEnabled = viewModel.PrimaryServiceViewModel.EncryptionEnabled;
			primaryServiceCertificateControl.CertificateSelection = viewModel.PrimaryServiceViewModel.CertificateSelection;
			secondaryServiceCertificateControl.EncryptionEnabled = viewModel.SecondaryServiceViewModel.EncryptionEnabled;
			secondaryServiceCertificateControl.CertificateSelection = viewModel.SecondaryServiceViewModel.CertificateSelection;
			ResumeLayout();
		}
		finally
		{
			isUpdatingFromViewModel = false;
		}
	}

	private void configureServersSameRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		UpdateMode();
	}

	private void configureServersIndividuallyRadioButton_CheckedChanged(object sender, EventArgs e)
	{
		UpdateMode();
	}

	private void UpdateMode()
	{
		MultipleCertificateSelectionViewModel.Mode mode = (configureServersIndividuallyRadioButton.Checked ? MultipleCertificateSelectionViewModel.Mode.IndividualCertificateForEachService : MultipleCertificateSelectionViewModel.Mode.SameCertificateForAllServices);
		if (viewModel.SelectedMode != mode)
		{
			viewModel.SelectedMode = mode;
			UpdateStateFromViewModel();
			OnSizeChanged(EventArgs.Empty);
			OnSelectionChanged(new SelectionChangedEventArgs(IsSelectionValid));
		}
	}

	private void MultiServerCertificateControl_Load(object sender, EventArgs e)
	{
		ResolveMode();
	}

	public void ResolveMode()
	{
		viewModel.ResolveSelectedMode();
		UpdateStateFromViewModel();
		viewModel.AllServicesViewModel.IsSelectionValid = allServicesCertificateControl.IsSelectionValid;
		viewModel.PrimaryServiceViewModel.IsSelectionValid = primaryServiceCertificateControl.IsSelectionValid;
		viewModel.SecondaryServiceViewModel.IsSelectionValid = secondaryServiceCertificateControl.IsSelectionValid;
		OnSelectionChanged(new SelectionChangedEventArgs(IsSelectionValid));
	}

	public void ReloadResources()
	{
		ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof(MultipleCertificateSelectionControl));
		configureServersSameRadioButton.Text = componentResourceManager.GetString("configureServersSameRadioButton.Text");
		configureServersIndividuallyRadioButton.Text = componentResourceManager.GetString("configureServersIndividuallyRadioButton.Text");
		allServicesCertificateControl.ReloadResources();
		primaryServiceCertificateControl.ReloadResources();
		secondaryServiceCertificateControl.ReloadResources();
	}

	public override Size GetPreferredSize(Size proposedSize)
	{
		return viewModel.SelectedMode switch
		{
			MultipleCertificateSelectionViewModel.Mode.SameCertificateForAllServices => new Size(proposedSize.Width, GetPreferredHeight(radioButtonsPanel) + GetPreferredHeight(allServicesCertificateControl)), 
			MultipleCertificateSelectionViewModel.Mode.IndividualCertificateForEachService => new Size(proposedSize.Width, GetPreferredHeight(radioButtonsPanel) + GetPreferredHeight(primaryServiceCertificateControl) + GetPreferredHeight(spaceLabel) + GetPreferredHeight(secondaryServiceCertificateControl)), 
			_ => base.GetPreferredSize(proposedSize), 
		};
	}

	private static int GetPreferredHeight(Control control)
	{
		return control.Margin.Top + control.Height + control.Margin.Bottom;
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Common.Controls.MultipleCertificateSelectionControl));
		this.tableLayoutPanel = new System.Windows.Forms.TableLayoutPanel();
		this.individualSettingsPanel = new System.Windows.Forms.Panel();
		this.spaceLabel = new System.Windows.Forms.Label();
		this.allServicesPanel = new System.Windows.Forms.Panel();
		this.sameSettingsPanel = new System.Windows.Forms.Panel();
		this.radioButtonsPanel = new System.Windows.Forms.Panel();
		this.configureServersIndividuallyRadioButton = new System.Windows.Forms.RadioButton();
		this.configureServersSameRadioButton = new System.Windows.Forms.RadioButton();
		this.secondaryServiceCertificateControl = new VideoOS.Common.Controls.CertificateSelectionControl();
		this.primaryServiceCertificateControl = new VideoOS.Common.Controls.CertificateSelectionControl();
		this.allServicesCertificateControl = new VideoOS.Common.Controls.CertificateSelectionControl();
		this.tableLayoutPanel.SuspendLayout();
		this.individualSettingsPanel.SuspendLayout();
		this.allServicesPanel.SuspendLayout();
		this.radioButtonsPanel.SuspendLayout();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.tableLayoutPanel, "tableLayoutPanel");
		this.tableLayoutPanel.Controls.Add(this.individualSettingsPanel, 0, 2);
		this.tableLayoutPanel.Controls.Add(this.allServicesPanel, 1, 1);
		this.tableLayoutPanel.Controls.Add(this.radioButtonsPanel, 0, 0);
		this.tableLayoutPanel.Name = "tableLayoutPanel";
		componentResourceManager.ApplyResources(this.individualSettingsPanel, "individualSettingsPanel");
		this.individualSettingsPanel.Controls.Add(this.secondaryServiceCertificateControl);
		this.individualSettingsPanel.Controls.Add(this.spaceLabel);
		this.individualSettingsPanel.Controls.Add(this.primaryServiceCertificateControl);
		this.individualSettingsPanel.Name = "individualSettingsPanel";
		componentResourceManager.ApplyResources(this.spaceLabel, "spaceLabel");
		this.spaceLabel.Name = "spaceLabel";
		componentResourceManager.ApplyResources(this.allServicesPanel, "allServicesPanel");
		this.allServicesPanel.Controls.Add(this.allServicesCertificateControl);
		this.allServicesPanel.Name = "allServicesPanel";
		componentResourceManager.ApplyResources(this.sameSettingsPanel, "sameSettingsPanel");
		this.sameSettingsPanel.Name = "sameSettingsPanel";
		componentResourceManager.ApplyResources(this.radioButtonsPanel, "radioButtonsPanel");
		this.radioButtonsPanel.Controls.Add(this.configureServersSameRadioButton);
		this.radioButtonsPanel.Controls.Add(this.configureServersIndividuallyRadioButton);
		this.radioButtonsPanel.Name = "radioButtonsPanel";
		componentResourceManager.ApplyResources(this.configureServersIndividuallyRadioButton, "configureServersIndividuallyRadioButton");
		this.configureServersIndividuallyRadioButton.Name = "configureServersIndividuallyRadioButton";
		this.configureServersIndividuallyRadioButton.TabStop = true;
		this.configureServersIndividuallyRadioButton.UseVisualStyleBackColor = true;
		this.configureServersIndividuallyRadioButton.CheckedChanged += new System.EventHandler(configureServersIndividuallyRadioButton_CheckedChanged);
		componentResourceManager.ApplyResources(this.configureServersSameRadioButton, "configureServersSameRadioButton");
		this.configureServersSameRadioButton.Name = "configureServersSameRadioButton";
		this.configureServersSameRadioButton.TabStop = true;
		this.configureServersSameRadioButton.UseVisualStyleBackColor = true;
		this.configureServersSameRadioButton.CheckedChanged += new System.EventHandler(configureServersSameRadioButton_CheckedChanged);
		componentResourceManager.ApplyResources(this.secondaryServiceCertificateControl, "secondaryServiceCertificateControl");
		this.secondaryServiceCertificateControl.EncryptionEnabled = true;
		this.secondaryServiceCertificateControl.Name = "secondaryServiceCertificateControl";
		componentResourceManager.ApplyResources(this.primaryServiceCertificateControl, "primaryServiceCertificateControl");
		this.primaryServiceCertificateControl.EncryptionEnabled = true;
		this.primaryServiceCertificateControl.Name = "primaryServiceCertificateControl";
		componentResourceManager.ApplyResources(this.allServicesCertificateControl, "allServicesCertificateControl");
		this.allServicesCertificateControl.EncryptionEnabled = true;
		this.allServicesCertificateControl.Name = "allServicesCertificateControl";
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Controls.Add(this.tableLayoutPanel);
		base.Name = "MultipleCertificateSelectionControl";
		base.Load += new System.EventHandler(MultiServerCertificateControl_Load);
		this.tableLayoutPanel.ResumeLayout(false);
		this.tableLayoutPanel.PerformLayout();
		this.individualSettingsPanel.ResumeLayout(false);
		this.individualSettingsPanel.PerformLayout();
		this.allServicesPanel.ResumeLayout(false);
		this.allServicesPanel.PerformLayout();
		this.radioButtonsPanel.ResumeLayout(false);
		this.radioButtonsPanel.PerformLayout();
		base.ResumeLayout(false);
	}
}
