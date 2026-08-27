using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class WizardPageModifyComponent : WizardPageComponents
{
	private class ComponentsHelper
	{
		private string _dispName;

		private CheckState _state;

		public string DisplayName => _dispName;

		public CheckState State => _state;

		public ComponentsHelper(CheckState state, string displayName)
		{
			_state = state;
			_dispName = displayName;
		}
	}

	private delegate void ShowErrorDelegate(Exception ex);

	private const string ManServerPackageID = "58451C4D-72B8-4D83-885D-A74B82F6C997";

	private List<string> _notInstalledPackages;

	private List<string> _installedPackages;

	private bool _errorOccured;

	private IContainer components;

	public WizardPageModifyComponent()
	{
		InitializeComponent();
		labelHeadline.UseMnemonic = false;
		_notInstalledPackages = new List<string>();
		_installedPackages = new List<string>();
		_platform = InstallerManager.Instance.Platform;
	}

	protected override void LoadResources()
	{
		labelHeadDescription.ForeColor = Color.FromArgb(6, 85, 142);
		labelHeadline.Text = string.Format(Resources.ModifyComponents_LabelHeadLine, VersionFormat.GenerateProductNameWithVersionUninstallMode());
	}

	public override List<string> GetEventParameter()
	{
		return base.GetEventParameter();
	}

	public override string OnTriggerEvent()
	{
		return base.OnTriggerEvent();
	}

	protected override void CollectComponents()
	{
	}

	protected override void FillCheckBoxList()
	{
		if (checkedListBoxComponents.Items.Count != 0)
		{
			return;
		}
		_components = new Dictionary<string, string>();
		Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			dictionary[installerPackageType.ID] = installerPackageType;
		}
		PackageRefType[] installExecuteSequence = InstallerManager.Instance.InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			_executable[packageRefType.ID] = new VisibilityCondition
			{
				Condition = packageRefType.Condition,
				Visibility = packageRefType.Visibility
			};
		}
		int num = 0;
		bool flag = true;
		Dictionary<string, ComponentsHelper> dictionary2 = new Dictionary<string, ComponentsHelper>();
		List<int> list = new List<int>();
		foreach (PackageRefType installedPackage in InstallerManager.Instance.InstallInfo.InstalledPackages)
		{
			InstallerPackageType installerPackageType2 = dictionary[installedPackage.ID];
			if ((!(installerPackageType2.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.X86.ToString()) && !(installerPackageType2.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == _platform.ToString()) && !(installerPackageType2.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.ANY.ToString())) || _executable[installedPackage.ID].Visibility == VisibilityType.Hidden)
			{
				continue;
			}
			string productName = null;
			switch (ComponentInstalledHelper.ComponentIsInstalled(installerPackageType2.ProductManifest.PackageData.ProductInfo.ProductCode, Path.Combine(InstallerManager.Instance.CurrentDirectory, installerPackageType2.ProductManifest.PackageData.PrimaryFile[0]), ref productName))
			{
			case ComponentInstalledHelper.InstallationState.InstalledByOtherSource:
			case ComponentInstalledHelper.InstallationState.InstalledByThisSource:
				if (productName != null)
				{
					if (productName.Contains(InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher))
					{
						productName = productName.Replace($"{InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher} ", "");
					}
					installerPackageType2.ProductManifest.PackageData.DisplayName = productName;
				}
				dictionary2.Add(installerPackageType2.ID, new ComponentsHelper(CheckState.Checked, productName));
				_installedPackages.Add(installedPackage.ID);
				break;
			case ComponentInstalledHelper.InstallationState.NotInstalled:
				dictionary2.Add(installerPackageType2.ID, new ComponentsHelper(CheckState.Unchecked, productName));
				_notInstalledPackages.Add(installedPackage.ID);
				break;
			}
			if (_executable[installedPackage.ID].Condition == ConditionType.Required)
			{
				list.Add(num);
			}
			_components[installerPackageType2.ProductManifest.PackageData.DisplayName] = ((installerPackageType2.ProductManifest.PackageConfig != null) ? GetDescription(installerPackageType2.ProductManifest.PackageConfig) : "");
			if (flag)
			{
				FillLabel(installerPackageType2.ProductManifest.PackageData.DisplayName, _components[installerPackageType2.ProductManifest.PackageData.DisplayName]);
				flag = false;
			}
			num++;
		}
		foreach (PackageRefType notInstalledPackage in InstallerManager.Instance.InstallInfo.NotInstalledPackages)
		{
			InstallerPackageType installerPackageType3 = dictionary[notInstalledPackage.ID];
			if ((!(installerPackageType3.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.X86.ToString()) && !(installerPackageType3.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == _platform.ToString()) && !(installerPackageType3.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.ANY.ToString())) || notInstalledPackage.Visibility == VisibilityType.Hidden)
			{
				continue;
			}
			string productName2 = null;
			switch (ComponentInstalledHelper.ComponentIsInstalled(installerPackageType3.ProductManifest.PackageData.ProductInfo.ProductCode, Path.Combine(InstallerManager.Instance.CurrentDirectory, installerPackageType3.ProductManifest.PackageData.PrimaryFile[0]), ref productName2))
			{
			case ComponentInstalledHelper.InstallationState.NotInstalled:
				dictionary2.Add(installerPackageType3.ID, new ComponentsHelper(CheckState.Unchecked, productName2));
				_notInstalledPackages.Add(notInstalledPackage.ID);
				break;
			case ComponentInstalledHelper.InstallationState.InstalledByOtherSource:
			case ComponentInstalledHelper.InstallationState.InstalledByThisSource:
				if (productName2 != null && productName2.Contains(InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher))
				{
					productName2 = productName2.Replace($"{InstallerManager.Instance.InstallerConfig.InstallerInfo.Publisher} ", "");
					installerPackageType3.ProductManifest.PackageData.DisplayName = productName2;
				}
				dictionary2.Add(installerPackageType3.ID, new ComponentsHelper(CheckState.Checked, productName2));
				_installedPackages.Add(notInstalledPackage.ID);
				break;
			}
			_components[installerPackageType3.ProductManifest.PackageData.DisplayName] = ((installerPackageType3.ProductManifest.PackageConfig != null) ? GetDescription(installerPackageType3.ProductManifest.PackageConfig) : "");
		}
		AddComponentsToList(dictionary2, dictionary);
		_requiredPackages = list;
	}

	private void AddComponentsToList(Dictionary<string, ComponentsHelper> components, Dictionary<string, InstallerPackageType> packagesMap)
	{
		foreach (KeyValuePair<string, ComponentsHelper> item in from p in packagesMap.Where((KeyValuePair<string, InstallerPackageType> p) => p.Key.ToUpperInvariant() == "58451C4D-72B8-4D83-885D-A74B82F6C997".ToUpperInvariant()).Concat(from p in packagesMap
				where p.Key.ToUpperInvariant() != "58451C4D-72B8-4D83-885D-A74B82F6C997".ToUpperInvariant()
				orderby p.Value.ProductManifest.PackageData.DisplayName
				select p)
			select p.Key into key
			where components.ContainsKey(key)
			select new KeyValuePair<string, ComponentsHelper>(key, components[key]))
		{
			if (packagesMap.ContainsKey(item.Key))
			{
				checkedListBoxComponents.Items.Add(item.Value.DisplayName ?? packagesMap[item.Key].ProductManifest.PackageData.DisplayName, item.Value.State);
			}
		}
	}

	protected override void checkedListBoxComponents_ItemCheck(object sender, ItemCheckEventArgs e)
	{
		base.checkedListBoxComponents_ItemCheck(sender, e);
		if (e.CurrentValue == CheckState.Indeterminate)
		{
			e.NewValue = e.CurrentValue;
		}
	}

	protected override void Close(object sender, EventArgs e)
	{
		InstallerManager.Instance.SequenceManager.PackagesToInstall = new List<string>();
		InstallerManager.Instance.SequenceManager.PackagesToUninstall = new List<string>();
		Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
		_uncheckedPackages = new List<string>();
		InstallerPackageType[] installerPackages = InstallerManager.Instance.InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			dictionary[installerPackageType.ID] = installerPackageType;
		}
		foreach (string installedPackage in _installedPackages)
		{
			InstallerPackageType installerPackageType2 = dictionary[installedPackage];
			bool flag = false;
			foreach (object checkedItem in checkedListBoxComponents.CheckedItems)
			{
				if (installerPackageType2.ProductManifest.PackageData.DisplayName == checkedItem.ToString())
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				continue;
			}
			List<string> list = FindDependingPackages(dictionary, installerPackageType2.ID);
			InstallerManager.Instance.SequenceManager.PackagesToUninstall.Add(installedPackage);
			if (list != null && list.Count != 0)
			{
				foreach (string item in list)
				{
					InstallerManager.Instance.SequenceManager.PackagesToUninstall.Add(item);
				}
			}
			_uncheckedPackages.Add(installedPackage);
		}
		foreach (string notInstalledPackage in _notInstalledPackages)
		{
			InstallerPackageType installerPackageType3 = dictionary[notInstalledPackage];
			bool flag2 = false;
			foreach (object checkedItem2 in checkedListBoxComponents.CheckedItems)
			{
				if (installerPackageType3.ProductManifest.PackageData.DisplayName == checkedItem2.ToString())
				{
					flag2 = true;
					break;
				}
			}
			if (flag2)
			{
				List<string> list2 = FindDependingPackages(dictionary, installerPackageType3.ID);
				if (list2 != null && list2.Count != 0)
				{
					foreach (string item2 in list2)
					{
						InstallerManager.Instance.SequenceManager.PackagesToInstall.Add(item2);
					}
				}
				InstallerManager.Instance.SequenceManager.PackagesToInstall.Add(notInstalledPackage);
			}
			else
			{
				_uncheckedPackages.Add(notInstalledPackage);
			}
		}
		UpdatePackageConfiguration("CUSTOM");
		if (InstallerManager.Instance.SequenceManager.PackagesToInstall.Count != 0)
		{
			Cursor = Cursors.WaitCursor;
			InstallerManager.Instance.PrerequisiteManager.PrepareModifyInstallation(InstallerManager.Instance.SequenceManager.PackagesToInstall);
			ErroCheck();
			Cursor = Cursors.Default;
		}
	}

	private void ErroCheck()
	{
		Cursor = Cursors.Default;
		Dictionary<string, string> packagesCannotInstall = InstallerManager.Instance.PrerequisiteManager.PackagesCannotInstall;
		if (packagesCannotInstall.Count == 0)
		{
			return;
		}
		string arg = null;
		bool flag = true;
		foreach (KeyValuePair<string, string> item in packagesCannotInstall)
		{
			if (flag)
			{
				arg = "\n\n" + item.Key;
			}
			if (item.Value != null && !flag)
			{
				arg = $"{arg} \n{item.Key} ";
			}
			flag = false;
		}
		ErrorInfo.SetErrorInfo(Resources.ErrorMessage_PackagesRequiredLabelHeadLine, "", null, Resources.ErrorMessage_PrerequisitesRequiredLabelSubLine, enablePreviousButton: false);
		_errorOccured = true;
		ShowErrorPage(null);
	}

	private void ShowErrorPage(Exception ex)
	{
		if (checkedListBoxComponents.InvokeRequired)
		{
			ShowErrorDelegate method = ShowErrorPage;
			checkedListBoxComponents.Invoke(method, ex);
			return;
		}
		Wizard.NavigationPanel.Hide();
		if (ex != null)
		{
			ErrorHandler.ShowError(ex);
		}
		else
		{
			ErrorHandler.ShowError();
		}
	}

	private List<string> FindDependingPackages(Dictionary<string, InstallerPackageType> packagesMap, string packageId)
	{
		List<string> list = new List<string>();
		foreach (KeyValuePair<string, InstallerPackageType> item in packagesMap)
		{
			InstallerPackageType value = item.Value;
			if (value.InstallConditions != null && IsDependentOnMyPackage(value, packageId) && !HasOtherInstalledDependencies(value, packageId))
			{
				list.Add(item.Key);
			}
		}
		return list;
	}

	private bool HasOtherInstalledDependencies(InstallerPackageType package, string packageId)
	{
		int num = 0;
		InstallConditionTypeByPassIf[] installConditions = package.InstallConditions;
		foreach (InstallConditionTypeByPassIf byPass in installConditions)
		{
			if (byPass.Condition == InstallStateType.NotInstalled && (byPass.ID == packageId || InstallerManager.Instance.InstallInfo.InstalledPackages.Any((PackageRefType f) => f.ID.Equals(byPass.ID, StringComparison.OrdinalIgnoreCase))))
			{
				num++;
			}
		}
		if (num == 1)
		{
			return false;
		}
		return true;
	}

	private bool IsDependentOnMyPackage(InstallerPackageType package, string packageId)
	{
		return package.InstallConditions.Any((InstallConditionTypeByPassIf f) => f.ID.Equals(packageId, StringComparison.OrdinalIgnoreCase) && f.Condition == InstallStateType.NotInstalled);
	}

	protected override void WizardPage_PageActivated(object sender, PageEventArgs e)
	{
		base.WizardPage_PageActivated(sender, e);
		Wizard.NavigationPanel.Show();
	}

	public override bool OnBeforeContinue()
	{
		return !_errorOccured;
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
		base.WizardPage.SuspendLayout();
		base.SuspendLayout();
		base.WizardPage.PageActivated += new VideoOS.Installer.UI.Framework.PageEventHandler(WizardPage_PageActivated);
		base.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
		base.Name = "WizardPageModifyComponent";
		this.PageName = "ModifyCompPage";
		base.WizardPage.ResumeLayout(false);
		base.ResumeLayout(false);
	}
}
