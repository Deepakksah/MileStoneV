using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.Globalization;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageFileLocation : WizardPageBase
{
	private string _targetDir;

	private readonly bool _useCommonProductLocation;

	private List<string> _languages;

	private const int _gbSize = 1024;

	private IContainer components;

	private Label labelHeadline;

	private Button buttonBrowseForFileLocation;

	private TextBox textBoxFileLocation;

	private Label labeFileLocation;

	private Label labelSpaceAvailable;

	private ComboBox comboBoxProductLanguage;

	private Label labelProductLanguage;

	private Label labelWarning;

	private Panel panelWarningIcon;

	public WizardPageFileLocation()
	{
		InitializeComponent();
		if (InstallerManager.Instance.TryGetPreviousMilestoneProductInstallPath(out var installationPath))
		{
			_useCommonProductLocation = true;
			_targetDir = installationPath;
		}
		else
		{
			_targetDir = InstallerManager.Instance.Parameters["TARGETDIR"];
		}
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			labelProductLanguage.Visible = false;
			comboBoxProductLanguage.Visible = false;
		}
		else if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			labeFileLocation.Visible = false;
			textBoxFileLocation.Visible = false;
			buttonBrowseForFileLocation.Visible = false;
			labelSpaceAvailable.Visible = false;
		}
		LoadResources();
	}

	private void LoadResources()
	{
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			labelHeadline.Text = Resources.FileLocation_LabelHeadLine_HardwareManufacturerMode;
		}
		else if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			labelHeadline.Text = Resources.FileLocation_LabelHeadLine_FinalizeMode;
		}
		else
		{
			labelHeadline.Text = Resources.FileLocation_LabelHeadLine;
		}
		labelProductLanguage.Text = Resources.FileLocation_LabelProductLanguage;
		UpdateSpaceAvailable(showError: false);
		labeFileLocation.Text = Resources.FileLocation_LabelFileLocation;
		buttonBrowseForFileLocation.Text = Resources.ButtonBrowse;
		textBoxFileLocation.Text = _targetDir;
		labelWarning.Text = ((InstallerManager.Instance.InstallerConfig.InstallerInfo.ID == "6CC1DA5E-431A-4D05-B2B0-53179EC5534B") ? Resources.FileLocation_FailoverWarningText : Resources.FileLocation_WarningText);
		if (InstallerManager.Instance.InstallerConfig.InstallerSettings.DisableChangingTargetDir || _useCommonProductLocation)
		{
			textBoxFileLocation.Enabled = false;
			buttonBrowseForFileLocation.Enabled = false;
		}
		if (InstallerManager.Instance.InstallerConfig.InstallerSettings.DisableWarning)
		{
			labelWarning.Hide();
			panelWarningIcon.Hide();
		}
	}

	private void buttonBrowseForFileLocation_Click(object sender, EventArgs e)
	{
		FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
		if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
		{
			_targetDir = folderBrowserDialog.SelectedPath;
			UpdateSpaceAvailable(showError: false);
			textBoxFileLocation.Text = folderBrowserDialog.SelectedPath;
		}
	}

	public override Dictionary<string, string> CollectParameters()
	{
		Dictionary<string, string> obj = new Dictionary<string, string> { { "TARGETDIR", _targetDir } };
		GlobalData.SetValue("TARGETDIR", _targetDir, "Filelocator.CollectParameters()");
		obj.Add("LANGUAGE", _languages[comboBoxProductLanguage.SelectedIndex]);
		return obj;
	}

	private bool UpdateSpaceAvailable(bool showError)
	{
		if (CommonHelper.IsValidPath(_targetDir))
		{
			try
			{
				double num = InstallerManager.Instance.GetFreeSpaceBytes(_targetDir) / 1048576.0;
				int digits = 1;
				string format = Resources.FileLocation_LabelSpaceAveliable;
				double num2 = 2.0;
				if (num < 1024.0)
				{
					format = Resources.FileLocation_LabelSpaceAveliableMb;
					digits = 0;
					num2 = 1.0;
				}
				else if (1024.0 <= num && num <= 10240.0)
				{
					format = Resources.FileLocation_LabelSpaceAveliable;
					digits = 2;
					num2 = 1024.0;
				}
				else if (10240.0 <= num && num <= 1048576.0)
				{
					format = Resources.FileLocation_LabelSpaceAveliable;
					digits = 0;
					num2 = 1024.0;
				}
				else if (1048576.0 <= num && num <= 10485760.0)
				{
					format = Resources.FileLocation_LabelSpaceAveliableTb;
					digits = 2;
					num2 = Math.Pow(1024.0, 2.0);
				}
				else if (10485760.0 <= num)
				{
					format = Resources.FileLocation_LabelSpaceAveliableTb;
					digits = 0;
					num2 = Math.Pow(1024.0, 2.0);
				}
				labelSpaceAvailable.Text = string.Format(format, Math.Round(num / num2, digits).ToString());
				return true;
			}
			catch (Win32Exception ex)
			{
				if (showError)
				{
					ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, Resources.ErrorMessage_GeneralErrorLabelSubLine, null, string.Format(Resources.ErrorMessage_GeneralErrorLabelMessage, ex.Message), enablePreviousButton: true);
					ErrorHandler.ShowError();
					return false;
				}
				labelSpaceAvailable.Text = string.Format(Resources.FileLocation_LabelSpaceAveliable, "-");
			}
			return false;
		}
		labelSpaceAvailable.Text = string.Format(Resources.FileLocation_LabelSpaceAveliable, "-");
		return false;
	}

	private List<string> GetProductLanguages(List<List<string>> supportedlanguages)
	{
		List<string> list = supportedlanguages[0];
		for (int i = 1; i < supportedlanguages.Count; i++)
		{
			List<string> list2 = new List<string>();
			foreach (string item in list)
			{
				if (supportedlanguages[i].Contains(item))
				{
					list2.Add(item);
				}
			}
			list = list2;
		}
		return list;
	}

	private void FillComboBoxLanguages()
	{
		ICultureProvider cultureProvider = new CultureProvider(_languages, throwOnUnsupported: false);
		foreach (Exception exception in cultureProvider.Exceptions)
		{
			LogManager.Write(exception.TypeAndMessage(), TraceEventType.Warning);
		}
		IOrderedEnumerable<CultureInfo> source = ((ICultureProvider)new RegionFreeCultures(cultureProvider)).SupportedCultures.OrderBy((CultureInfo x) => x.NativeName);
		comboBoxProductLanguage.Items.Clear();
		IEnumerable<string> source2 = source.Select((CultureInfo x) => x.NativeName);
		ComboBox.ObjectCollection items = comboBoxProductLanguage.Items;
		object[] items2 = source2.ToArray();
		items.AddRange(items2);
		_languages.Clear();
		_languages.AddRange(source.Select((CultureInfo x) => x.Name));
		comboBoxProductLanguage.SelectedItem = source.FirstOrDefault((CultureInfo x) => x.Name == InstallerManager.Instance.InstallInfo.InstallerLanguage)?.NativeName ?? source2.FirstOrDefault();
	}

	private void textBoxFileLocation_TextChanged(object sender, EventArgs e)
	{
		_targetDir = textBoxFileLocation.Text;
		if (textBoxFileLocation.Text.Length == 0)
		{
			Wizard.NextButton.Enabled = false;
		}
		else if (Wizard != null)
		{
			Wizard.NextButton.Enabled = true;
		}
		textBoxFileLocation.Focus();
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override bool OnBeforeContinue()
	{
		if (!CommonHelper.IsValidPath(textBoxFileLocation.Text))
		{
			MessageHelper.ShowInstallerMessageBox(Resources.ErrorMessage_InvalidPathCaption, Resources.ErrorMessage_InvalidPathText, InstallerMessageBoxIcon.Error);
			textBoxFileLocation.Focus();
			return false;
		}
		if (!UpdateSpaceAvailable(showError: true))
		{
			return false;
		}
		if (InstallerManager.Instance.IsEnoughOfSpaceOnDrive() != 0.0)
		{
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_NotEnoughSpaceHeadLine, string.Format(Resources.ErrorMessage_NotEnoughSpaceSubLine, VersionFormat.GenerateProductNameWithVersion(), InstallerManager.Instance.GetRequiredSpace(), InstallerManager.Instance.SpaceAvailableOnDrive), null, null, enablePreviousButton: true);
			ErrorHandler.ShowError();
			return false;
		}
		RunConfirm();
		InstallerManager.Instance.VerifyParameters();
		return true;
	}

	private static void RunConfirm()
	{
		if (InstallerManager.Instance.IsModify)
		{
			return;
		}
		List<string> list = new List<string>();
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (installerPackageType.PreInstallActions == null || !installerPackageType.PreInstallActions.ConfirmInstall)
			{
				continue;
			}
			var (text, version) = NameAndVersionOfPackageInstalled(installerPackageType);
			if (!string.IsNullOrEmpty(text) && string.Compare(text, installerPackageType.ProductManifest.PackageData.DisplayName, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				Version version2;
				try
				{
					version2 = ConvertProductVersionInfo(installerPackageType.ProductManifest.PackageData.Version);
				}
				catch (Exception ex)
				{
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed while extracting version information from package info. Package name: {0}, error: {1}", installerPackageType.ProductManifest.PackageData.DisplayName, ex));
					throw new UIFrameworkErrorException(ex, enablePreviousButtong: false);
				}
				if (version >= version2)
				{
					list.Add(installerPackageType.ID);
					LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Skipping package, newer product version installed. Installed: {0} ({1}), Package: {2} ({3})", text, version, installerPackageType.ProductManifest.PackageData.DisplayName, version2));
				}
				else if (MessageHelper.ShowInstallerMessageBox(Resources.Install_ConfirmInstallTitle, string.Format(Resources.Install_ConfirmInstallText, text, installerPackageType.ProductManifest.PackageData.DisplayName), InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) == DialogResult.No)
				{
					list.Add(installerPackageType.ID);
				}
			}
		}
		PackageRefType[] uninstallerSequencer = InstallerManager.Instance.InstallerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType in uninstallerSequencer)
		{
			if ((packageRefType.Condition != ConditionType.Skip || packageRefType.Condition != ConditionType.Disabled) && packageRefType.AllowUnInstall && list.Contains(packageRefType.ID))
			{
				packageRefType.Condition = ConditionType.Skip;
			}
		}
		uninstallerSequencer = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType2 in uninstallerSequencer)
		{
			if ((packageRefType2.Condition != ConditionType.Skip || packageRefType2.Condition != ConditionType.Disabled) && packageRefType2.AllowUnInstall && list.Contains(packageRefType2.ID))
			{
				packageRefType2.Condition = ConditionType.Skip;
			}
		}
	}

	private static Version ConvertProductVersionInfo(ProductVersionInfoType productVersionInfo)
	{
		int major = int.Parse(productVersionInfo.Major, CultureInfo.InvariantCulture);
		int minor = int.Parse(productVersionInfo.Minor, CultureInfo.InvariantCulture);
		int build = int.Parse(productVersionInfo.Build ?? "1", CultureInfo.InvariantCulture);
		int revision = int.Parse(productVersionInfo.Revision ?? "1", CultureInfo.InvariantCulture);
		return new Version(major, minor, build, revision);
	}

	private static (string productName, Version productVersion) NameAndVersionOfPackageInstalled(InstallerPackageType package)
	{
		string productName = string.Empty;
		Version item = new Version(0, 0, 0, 0);
		string fileName = Path.Combine(InstallerManager.Instance.CurrentDirectory, package.ProductManifest.PackageData.PrimaryFile[0]);
		if (ComponentInstalledHelper.ComponentIsInstalled(package.ProductManifest.PackageData.ProductInfo.ProductCode, fileName, ref productName) != ComponentInstalledHelper.InstallationState.NotInstalled && Version.TryParse(ClMsiHandler.GetProductVersionFromRegistry(package.ProductManifest.PackageData.ProductInfo.ProductCode), out var result))
		{
			item = result;
		}
		return (productName: productName, productVersion: item);
	}

	private void textBoxFileLocation_Leave(object sender, EventArgs e)
	{
		UpdateSpaceAvailable(showError: false);
	}

	private void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		if (comboBoxProductLanguage.Items.Count == 0)
		{
			List<List<string>> list = new List<List<string>>();
			InstallerPluginType[] installerPlugins = InstallerManager.Instance.InstallerConfig.InstallerPlugins;
			foreach (InstallerPluginType installerPluginType in installerPlugins)
			{
				if (installerPluginType.SupportedLanguages != null && installerPluginType.SupportedLanguages.Length != 0)
				{
					List<string> list2 = new List<string>();
					string[] supportedLanguages = installerPluginType.SupportedLanguages;
					foreach (string item in supportedLanguages)
					{
						list2.Add(item);
					}
					list.Add(list2);
				}
			}
			_languages = GetProductLanguages(list);
			FillComboBoxLanguages();
		}
		if (!_useCommonProductLocation)
		{
			Dictionary<string, string> dictionary = GlobalData.ToDict();
			if (dictionary.ContainsKey("TARGETDIR"))
			{
				InstallerManager.Instance.Parameters["TARGETDIR"] = dictionary["TARGETDIR"];
				_targetDir = dictionary["TARGETDIR"];
			}
		}
		if (InstallerManager.Instance.InstallerConfig.InstallerInfo.TargetPlatform == PlatformType.X64.ToString())
		{
			_targetDir = Regex.Replace(_targetDir, "^" + Regex.Escape("%programfiles(x86)%"), "%programfiles%", RegexOptions.IgnoreCase);
			_targetDir = Regex.Replace(_targetDir, "^" + Regex.Escape(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86)), Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), RegexOptions.IgnoreCase);
		}
		UpdateSpaceAvailable(showError: false);
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = Resources.btn_Cancel;
		Wizard.NextButton.Text = Resources.btn_Install;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
		textBoxFileLocation.Text = ((string.Compare(InstallerManager.Instance.Parameters["TARGETDIR"], _targetDir) == 0 || !CommonHelper.IsValidPath(_targetDir)) ? InstallerManager.Instance.Parameters["TARGETDIR"] : _targetDir);
		textBoxFileLocation.Focus();
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageFileLocation));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.buttonBrowseForFileLocation = new System.Windows.Forms.Button();
		this.labeFileLocation = new System.Windows.Forms.Label();
		this.textBoxFileLocation = new System.Windows.Forms.TextBox();
		this.labelSpaceAvailable = new System.Windows.Forms.Label();
		this.labelProductLanguage = new System.Windows.Forms.Label();
		this.comboBoxProductLanguage = new System.Windows.Forms.ComboBox();
		this.labelWarning = new System.Windows.Forms.Label();
		this.panelWarningIcon = new System.Windows.Forms.Panel();
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.labelProductLanguage);
		base.WizardPage.Controls.Add(this.labelSpaceAvailable);
		base.WizardPage.Controls.Add(this.buttonBrowseForFileLocation);
		base.WizardPage.Controls.Add(this.textBoxFileLocation);
		base.WizardPage.Controls.Add(this.labeFileLocation);
		base.WizardPage.Controls.Add(this.labelHeadline);
		base.WizardPage.Controls.Add(this.comboBoxProductLanguage);
		base.WizardPage.Controls.Add(this.panelWarningIcon);
		base.WizardPage.Controls.Add(this.labelWarning);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("1365801c-1f97-4772-967f-d94a394a6ba5");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		this.labelHeadline.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		componentResourceManager.ApplyResources(this.buttonBrowseForFileLocation, "buttonBrowseForFileLocation");
		this.buttonBrowseForFileLocation.Name = "buttonBrowseForFileLocation";
		this.buttonBrowseForFileLocation.UseVisualStyleBackColor = true;
		this.buttonBrowseForFileLocation.Click += new System.EventHandler(buttonBrowseForFileLocation_Click);
		componentResourceManager.ApplyResources(this.labeFileLocation, "labeFileLocation");
		this.labeFileLocation.Name = "labeFileLocation";
		componentResourceManager.ApplyResources(this.textBoxFileLocation, "textBoxFileLocation");
		this.textBoxFileLocation.Name = "textBoxFileLocation";
		this.textBoxFileLocation.TextChanged += new System.EventHandler(textBoxFileLocation_TextChanged);
		this.textBoxFileLocation.Leave += new System.EventHandler(textBoxFileLocation_Leave);
		componentResourceManager.ApplyResources(this.labelSpaceAvailable, "labelSpaceAvailable");
		this.labelSpaceAvailable.Name = "labelSpaceAvailable";
		componentResourceManager.ApplyResources(this.labelProductLanguage, "labelProductLanguage");
		this.labelProductLanguage.Name = "labelProductLanguage";
		this.comboBoxProductLanguage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
		componentResourceManager.ApplyResources(this.comboBoxProductLanguage, "comboBoxProductLanguage");
		this.comboBoxProductLanguage.FormattingEnabled = true;
		this.comboBoxProductLanguage.Name = "comboBoxProductLanguage";
		componentResourceManager.ApplyResources(this.labelWarning, "labelWarning");
		this.labelWarning.Name = "labelWarning";
		componentResourceManager.ApplyResources(this.panelWarningIcon, "panelWarningIcon");
		this.panelWarningIcon.Name = "panelWarningIcon";
		base.Name = "WizardPageFileLocation";
		this.PageName = "FileLocatorPage";
		base.WizardPage.ResumeLayout(false);
		base.WizardPage.PerformLayout();
		base.ResumeLayout(false);
	}
}
