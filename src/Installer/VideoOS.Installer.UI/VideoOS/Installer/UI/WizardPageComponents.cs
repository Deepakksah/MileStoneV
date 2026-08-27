using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Description;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageComponents : WizardPageBase
{
	protected struct VisibilityCondition
	{
		public ConditionType Condition;

		public VisibilityType Visibility;
	}

	protected Dictionary<string, string> _components;

	protected List<int> _requiredPackages = new List<int>();

	protected PlatformType _platform;

	protected Dictionary<string, VisibilityCondition> _executable = new Dictionary<string, VisibilityCondition>();

	protected List<PageConditionType> _conditions = new List<PageConditionType>();

	protected List<string> _uncheckedPackages;

	private string _currentTrek;

	private const string ApiGatewayServicePackageId = "78D895AF-AEC4-4500-8509-C16AAB987B66";

	private IContainer components;

	private GroupBox groupBox1;

	private Label labelSubDescription;

	protected CheckedListBox checkedListBoxComponents;

	protected Label labelHeadline;

	protected Label labelHeadDescription;

	public WizardPageComponents()
	{
		InitializeComponent();
		_platform = InstallerManager.Instance.Platform;
	}

	protected virtual void LoadResources()
	{
		labelHeadDescription.ForeColor = Color.FromArgb(6, 85, 142);
		labelHeadline.Text = Resources.Components_LabelHeadLine;
	}

	public override void ReloadResources(CultureInfo culture)
	{
		base.ReloadResources(culture);
		LoadResources();
	}

	public override List<string> GetEventParameter()
	{
		PageConditionType[] pageConditions = InstallerManager.Instance.InstallerConfig.InstallerSettings.PageConditions;
		List<string> list = null;
		if (pageConditions != null)
		{
			PageConditionType[] array = pageConditions;
			foreach (PageConditionType pageConditionType in array)
			{
				if (pageConditionType.PageName == PageName)
				{
					if (list == null)
					{
						list = new List<string>();
					}
					list.Add($"{pageConditionType.NextPage},{pageConditionType.Event}");
					_conditions.Add(pageConditionType);
				}
			}
		}
		return list;
	}

	public override string OnTriggerEvent()
	{
		return CheckCondidtions();
	}

	protected string CheckCondidtions()
	{
		string result = null;
		if (_conditions != null && _conditions.Count != 0 && _uncheckedPackages != null)
		{
			foreach (PageConditionType condition in _conditions)
			{
				if (condition.ByPassIf == null)
				{
					continue;
				}
				PageConditionTypeByPassIf[] byPassIf = condition.ByPassIf;
				foreach (PageConditionTypeByPassIf pageConditionTypeByPassIf in byPassIf)
				{
					if (pageConditionTypeByPassIf.Condition == InstallStateType.NotInstalled && _uncheckedPackages.Contains(pageConditionTypeByPassIf.ID))
					{
						result = condition.Event;
					}
				}
			}
		}
		return result;
	}

	protected void FillLabel(string headLine, string subLine)
	{
		labelHeadDescription.Text = headLine;
		labelSubDescription.Text = subLine;
	}

	protected void checkedListBoxComponents_SelectedValueChanged(object sender, EventArgs e)
	{
		CheckedListBox checkedListBox = sender as CheckedListBox;
		if (checkedListBox.SelectedItem != null && _components.ContainsKey(checkedListBox.SelectedItem.ToString()))
		{
			FillLabel(checkedListBox.SelectedItem.ToString(), _components[checkedListBox.SelectedItem.ToString()]);
		}
	}

	protected virtual void Close(object sender, EventArgs e)
	{
		_uncheckedPackages = new List<string>();
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (installerPackageType.ProductManifest.Format != FormatType.Milestone)
			{
				continue;
			}
			bool flag = false;
			foreach (object checkedItem in checkedListBoxComponents.CheckedItems)
			{
				if (installerPackageType.ProductManifest.PackageData.DisplayName == checkedItem.ToString())
				{
					flag = true;
					break;
				}
			}
			if ((!flag && _executable.ContainsKey(installerPackageType.ID) && _executable[installerPackageType.ID].Visibility != VisibilityType.Hidden) || (_executable.ContainsKey(installerPackageType.ID) && _executable[installerPackageType.ID].Visibility == VisibilityType.Hidden && (_executable[installerPackageType.ID].Condition == ConditionType.Skip || _executable[installerPackageType.ID].Condition == ConditionType.Disabled)))
			{
				_uncheckedPackages.Add(installerPackageType.ID);
			}
		}
		string text = string.Empty;
		foreach (string uncheckedPackage in _uncheckedPackages)
		{
			text = text + uncheckedPackage + ";";
		}
		LogManager.Write("ComponentPage: Packages that are unchecked:" + ((text.Length != 0) ? text : "none"), TraceEventType.Information);
		UpdatePackageConfiguration(_currentTrek);
	}

	protected void UpdatePackageConfiguration(string trackId)
	{
		PackageRefType[] installExecuteSequence = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (_uncheckedPackages.Contains(packageRefType.ID))
			{
				packageRefType.Condition = ConditionType.Skip;
			}
			else if (packageRefType.Condition == ConditionType.Skip || packageRefType.Condition == ConditionType.Disabled)
			{
				packageRefType.Condition = ConditionType.Optional;
			}
			if (string.IsNullOrWhiteSpace(trackId) || packageRefType.TrekCustomisations == null)
			{
				continue;
			}
			TrekCustomisationsTypeTrekCustomisation trekCustomisationsTypeTrekCustomisation = null;
			TrekCustomisationsTypeTrekCustomisation[] trekCustomisations = packageRefType.TrekCustomisations;
			foreach (TrekCustomisationsTypeTrekCustomisation trekCustomisationsTypeTrekCustomisation2 in trekCustomisations)
			{
				if (trekCustomisationsTypeTrekCustomisation2.TrekId.ToUpperInvariant().Equals(trackId.ToUpperInvariant()))
				{
					trekCustomisationsTypeTrekCustomisation = trekCustomisationsTypeTrekCustomisation2;
					break;
				}
			}
			if (trekCustomisationsTypeTrekCustomisation != null)
			{
				if (_uncheckedPackages.Contains(packageRefType.ID))
				{
					trekCustomisationsTypeTrekCustomisation.Condition = ConditionType.Skip;
				}
				else if (trekCustomisationsTypeTrekCustomisation.Condition == ConditionType.Skip || trekCustomisationsTypeTrekCustomisation.Condition == ConditionType.Disabled)
				{
					trekCustomisationsTypeTrekCustomisation.Condition = packageRefType.Condition;
				}
			}
		}
		installExecuteSequence = InstallerManager.Instance.InstallerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType2 in installExecuteSequence)
		{
			if (_uncheckedPackages.Contains(packageRefType2.ID))
			{
				packageRefType2.Condition = ConditionType.Skip;
			}
			else
			{
				packageRefType2.Condition = ConditionType.Optional;
			}
		}
	}

	protected virtual void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		_currentTrek = (InstallerManager.Instance.Parameters.ContainsKey("TRECK_ID") ? InstallerManager.Instance.Parameters["TRECK_ID"] : null);
		if (_components == null)
		{
			LoadResources();
			LoadDescription();
		}
		CollectComponents();
		FillCheckBoxList();
		Wizard.NextButton.Show();
		Wizard.CancelButton.Text = Resources.btn_Cancel;
		if (Wizard.BackButton.Location == Wizard.NextButton.Location)
		{
			Wizard.BackButton.Location = new Point(Wizard.BackButton.Location.X - Wizard.BackButton.Width - 2, Wizard.BackButton.Location.Y);
		}
	}

	protected virtual void CollectComponents()
	{
		_components = new Dictionary<string, string>();
		PackageRefType[] installExecuteSequence = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (_currentTrek != null)
			{
				if (packageRefType.TrekCustomisations != null)
				{
					TrekCustomisationsTypeTrekCustomisation trekCustomisationsTypeTrekCustomisation = null;
					TrekCustomisationsTypeTrekCustomisation[] trekCustomisations = packageRefType.TrekCustomisations;
					foreach (TrekCustomisationsTypeTrekCustomisation trekCustomisationsTypeTrekCustomisation2 in trekCustomisations)
					{
						if (trekCustomisationsTypeTrekCustomisation2.TrekId == _currentTrek)
						{
							trekCustomisationsTypeTrekCustomisation = trekCustomisationsTypeTrekCustomisation2;
							break;
						}
					}
					if (trekCustomisationsTypeTrekCustomisation != null)
					{
						_executable[packageRefType.ID] = new VisibilityCondition
						{
							Condition = (trekCustomisationsTypeTrekCustomisation.ConditionSpecified ? trekCustomisationsTypeTrekCustomisation.Condition : packageRefType.Condition),
							Visibility = (trekCustomisationsTypeTrekCustomisation.VisibilitySpecified ? trekCustomisationsTypeTrekCustomisation.Visibility : packageRefType.Visibility)
						};
					}
					else
					{
						_executable[packageRefType.ID] = new VisibilityCondition
						{
							Condition = packageRefType.Condition,
							Visibility = packageRefType.Visibility
						};
					}
				}
				else
				{
					_executable[packageRefType.ID] = new VisibilityCondition
					{
						Condition = packageRefType.Condition,
						Visibility = packageRefType.Visibility
					};
				}
			}
			else
			{
				_executable[packageRefType.ID] = new VisibilityCondition
				{
					Condition = packageRefType.Condition,
					Visibility = packageRefType.Visibility
				};
			}
		}
	}

	protected virtual void FillCheckBoxList()
	{
		bool flag = true;
		_requiredPackages = new List<int>();
		checkedListBoxComponents.Items.Clear();
		int num = 0;
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType package in installerPackages)
		{
			if (!_executable.ContainsKey(package.ID))
			{
				continue;
			}
			ConditionType condition = _executable[package.ID].Condition;
			VisibilityType visibility = _executable[package.ID].Visibility;
			if (package.ProductManifest.Format != FormatType.Milestone || (!(package.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.X86.ToString()) && !(package.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == _platform.ToString()) && !(package.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.ANY.ToString())) || visibility == VisibilityType.Hidden)
			{
				continue;
			}
			string text = ((package.ProductManifest.PackageConfig != null) ? GetDescription(package.ProductManifest.PackageConfig) : string.Empty);
			CheckState check = CheckState.Unchecked;
			bool flag2 = condition == ConditionType.Required;
			if (condition != ConditionType.Skip)
			{
				if (InstallerManager.Instance.OldInstallInfo != null && InstallerManager.Instance.Parameters["IS_UPGRADE"] == "true")
				{
					if (InstallerManager.Instance.OldInstallInfo.InstalledPackages.Count == 0 && condition != ConditionType.Disabled)
					{
						check = CheckState.Checked;
					}
					else if (InstallerManager.Instance.OldInstallInfo.InstalledPackages.Any((PackageRefType t) => t.ID == package.ID))
					{
						check = CheckState.Checked;
						flag2 = true;
					}
					else if (condition != ConditionType.Disabled && !InstallerManager.Instance.OldInstallInfo.NotInstalledPackages.Any((PackageRefType t) => t.ID == package.ID))
					{
						check = CheckState.Checked;
					}
					else if (package.ID == "78D895AF-AEC4-4500-8509-C16AAB987B66")
					{
						check = CheckState.Checked;
					}
					else if (flag2)
					{
						if (InstallerManager.Instance.OldInstallInfo.InstalledPackages.Any((PackageRefType t) => t.Condition == ConditionType.Finalize))
						{
							check = CheckState.Checked;
						}
						else
						{
							flag2 = false;
						}
					}
				}
				else if (condition != ConditionType.Disabled)
				{
					check = CheckState.Checked;
				}
			}
			checkedListBoxComponents.Items.Add(package.ProductManifest.PackageData.DisplayName, check);
			if (flag2)
			{
				_requiredPackages.Add(num);
			}
			if (flag)
			{
				FillLabel(package.ProductManifest.PackageData.DisplayName, text);
				flag = false;
			}
			_components[package.ProductManifest.PackageData.DisplayName] = text;
			num++;
		}
	}

	protected void LoadDescription()
	{
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (InstallerManager.Instance.InstallInfo.InstallerLanguage == null)
			{
				LogManager.Write("InstallerManager.Instance.InstallInfo.InstallerLanguage equals null");
			}
			try
			{
				if (installerPackageType.ProductManifest.PackageData.Description == null || installerPackageType.ProductManifest.PackageData.Description.Filename == null)
				{
					continue;
				}
				Exception ex = null;
				string filename = Path.Combine(InstallerManager.Instance.InstallInfo.InstallerLanguage, installerPackageType.ProductManifest.PackageData.Description.Filename);
				try
				{
					installerPackageType.ProductManifest.PackageConfig = InstallerConfigManager.LoadDescription(filename);
				}
				catch (Exception ex2)
				{
					ex = ex2;
				}
				if (ex == null)
				{
					continue;
				}
				filename = installerPackageType.ProductManifest.PackageData.Description.Filename;
				try
				{
					installerPackageType.ProductManifest.PackageConfig = InstallerConfigManager.LoadDescription(filename);
				}
				catch (Exception)
				{
					filename = Path.Combine("en-US", installerPackageType.ProductManifest.PackageData.Description.Filename);
					try
					{
						installerPackageType.ProductManifest.PackageConfig = InstallerConfigManager.LoadDescription(filename);
					}
					catch (FileNotFoundException)
					{
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Disregarding file: {0} as it does not exist", Path.GetFileName(filename)), TraceEventType.Information);
					}
					catch (Exception ex5)
					{
						LogManager.Write(ex5.TypeAndMessage());
						break;
					}
				}
			}
			catch (ArgumentNullException ex6)
			{
				LogManager.Write(ex6);
				LogManager.Write(ex6.ParamName);
			}
		}
	}

	protected string GetDescription(PackageConfig packageConfig)
	{
		string result = null;
		PackageDescriptionType[] packageDescriptions = packageConfig.PackageDescriptions;
		foreach (PackageDescriptionType packageDescriptionType in packageDescriptions)
		{
			if (packageDescriptionType.Language == InstallerManager.Instance.InstallInfo.InstallerLanguage)
			{
				result = packageDescriptionType.Description;
				break;
			}
		}
		return result;
	}

	protected virtual void checkedListBoxComponents_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		if (_requiredPackages.Contains(e.Index))
		{
			e.NewValue = e.CurrentValue;
		}
		bool enabled = true;
		if (e.NewValue == CheckState.Unchecked && checkedListBoxComponents.CheckedItems.Count == 1)
		{
			enabled = false;
		}
		Wizard.BackButton.Enabled = enabled;
		Wizard.NextButton.Enabled = enabled;
	}

	protected void checkedListBoxComponents_MouseClick(object sender, MouseEventArgs e)
	{
		if (((e.Button == MouseButtons.Left) & (e.X > 13)) && checkedListBoxComponents.SelectedIndex != -1)
		{
			checkedListBoxComponents.SetItemChecked(checkedListBoxComponents.SelectedIndex, !checkedListBoxComponents.GetItemChecked(checkedListBoxComponents.SelectedIndex));
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
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.WizardPageComponents));
		this.labelHeadline = new System.Windows.Forms.Label();
		this.checkedListBoxComponents = new System.Windows.Forms.CheckedListBox();
		this.groupBox1 = new System.Windows.Forms.GroupBox();
		this.labelSubDescription = new System.Windows.Forms.Label();
		this.labelHeadDescription = new System.Windows.Forms.Label();
		base.WizardPage.SuspendLayout();
		this.groupBox1.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.AutoScaleDimensions = new System.Drawing.SizeF(0f, 0f);
		base.WizardPage.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit;
		base.WizardPage.BackColor = System.Drawing.Color.Transparent;
		base.WizardPage.Controls.Add(this.groupBox1);
		base.WizardPage.Controls.Add(this.checkedListBoxComponents);
		base.WizardPage.Controls.Add(this.labelHeadline);
		componentResourceManager.ApplyResources(base.WizardPage, "WizardPage");
		base.WizardPage.FinishPage = false;
		base.WizardPage.FlowId = new System.Guid("00000000-0000-0000-0000-000000000000");
		base.WizardPage.Name = "WizardPage";
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.WizardPage.Leave += new System.EventHandler(Close);
		componentResourceManager.ApplyResources(this.labelHeadline, "labelHeadline");
		this.labelHeadline.Name = "labelHeadline";
		this.checkedListBoxComponents.CheckOnClick = true;
		componentResourceManager.ApplyResources(this.checkedListBoxComponents, "checkedListBoxComponents");
		this.checkedListBoxComponents.FormattingEnabled = true;
		this.checkedListBoxComponents.Name = "checkedListBoxComponents";
		this.checkedListBoxComponents.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(checkedListBoxComponents_ItemCheck);
		this.checkedListBoxComponents.MouseClick += new System.Windows.Forms.MouseEventHandler(checkedListBoxComponents_MouseClick);
		this.checkedListBoxComponents.SelectedValueChanged += new System.EventHandler(checkedListBoxComponents_SelectedValueChanged);
		this.groupBox1.Controls.Add(this.labelSubDescription);
		this.groupBox1.Controls.Add(this.labelHeadDescription);
		componentResourceManager.ApplyResources(this.groupBox1, "groupBox1");
		this.groupBox1.Name = "groupBox1";
		this.groupBox1.TabStop = false;
		componentResourceManager.ApplyResources(this.labelSubDescription, "labelSubDescription");
		this.labelSubDescription.Name = "labelSubDescription";
		componentResourceManager.ApplyResources(this.labelHeadDescription, "labelHeadDescription");
		this.labelHeadDescription.Name = "labelHeadDescription";
		base.Name = "WizardPageComponents";
		this.PageName = "ComponentPage";
		base.Leave += new System.EventHandler(Close);
		base.WizardPage.ResumeLayout(false);
		this.groupBox1.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
