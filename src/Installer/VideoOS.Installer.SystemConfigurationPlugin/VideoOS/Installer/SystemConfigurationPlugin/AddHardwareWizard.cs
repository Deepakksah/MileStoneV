using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Windows.Forms;
using System.Windows.Forms.Integration;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;
using VideoOS.Installer.UI.Framework;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddHardwareWizard : WizardPageBase
{
	private ElementHost _elementHost;

	private ScanHardwareControl _wpfControl = new ScanHardwareControl();

	private AddHardwareViewModel _viewModel;

	private BackgroundWorker _addThread;

	private IContainer components;

	public AddHardwareWizard()
	{
		InitializeComponent();
		PageName = "AddHardwarePage";
		_elementHost = new ElementHost();
		_elementHost.Dock = DockStyle.Fill;
		WizardPage.Controls.Add(_elementHost);
		_wpfControl = new ScanHardwareControl();
		_viewModel = new AddHardwareViewModel();
		_wpfControl.DataContext = _viewModel;
		_elementHost.Child = _wpfControl;
		LoadResources();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	private void LoadResources()
	{
		_wpfControl.labelHeadLine.Text = Resources.HardwareAddHeader;
		_viewModel.Header = Resources.HardwareAddInfo;
		_viewModel.SetProgress(0);
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		Wizard.CancelButton.Visible = false;
		Wizard.HideNavigationButtons();
		_addThread = new BackgroundWorker();
		_addThread.DoWork += _viewModel.InitStartAddingHardware;
		_addThread.RunWorkerCompleted += addThread_RunWorkerCompleted;
		_addThread.WorkerReportsProgress = true;
		_addThread.RunWorkerAsync();
	}

	private void addThread_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
	{
		Wizard.MoveToNextPage();
	}

	protected override void Dispose(bool disposing)
	{
		if (disposing)
		{
			if (_elementHost != null)
			{
				if (_elementHost.Child != null)
				{
					_elementHost.Child = null;
				}
				_elementHost.Dispose();
				_elementHost = null;
			}
			if (_addThread != null)
			{
				_addThread.Dispose();
				_addThread = null;
			}
			if (components != null)
			{
				components.Dispose();
				components = null;
			}
			if (_viewModel != null)
			{
				_viewModel = null;
			}
		}
		base.Dispose(disposing);
	}

	public override Dictionary<string, string> CollectParameters()
	{
		return new Dictionary<string, string> { 
		{
			ServerParameters.AddedHardware.ToString(),
			HardwareConfigurations.GetAsXml(_viewModel.AddedHardwareList.ToArray())
		} };
	}

	public static string AddedCameraListToString(List<string> cameras)
	{
		if (cameras == null)
		{
			throw new ArgumentNullException("cameras");
		}
		return string.Join(",", cameras.ToArray());
	}

	public static List<string> AddedCameraStringToList(string cameras)
	{
		if (cameras == null)
		{
			throw new ArgumentNullException("cameras");
		}
		return new List<string>(cameras.Split(','));
	}

	private void InitializeComponent()
	{
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Dock = System.Windows.Forms.DockStyle.Fill;
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("45984772-2c05-466c-adb8-fbca0067e62e");
		base.WizardPage.Location = new System.Drawing.Point(0, 0);
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.Size = new System.Drawing.Size(634, 492);
		base.WizardPage.TabIndex = 0;
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "AddHardwareWizard";
		this.PageName = "AddHardwareWizardPage";
		base.ResumeLayout(false);
	}
}
