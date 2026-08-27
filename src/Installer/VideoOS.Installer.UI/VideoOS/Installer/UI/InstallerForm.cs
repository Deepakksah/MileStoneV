using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Manager;
using VideoOS.Installer.UI.ErrorHandling;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Properties;

namespace VideoOS.Installer.UI;

public class InstallerForm : Form
{
	private delegate void ShowErrorDelegate(Exception ex);

	private const string UninstallTrekId = "uninstall";

	private const string ModifyTrekId = "modify";

	private readonly List<string> ParametersCanBeOverwrittenFromCmd = new List<string> { "PREREQUISITESTOSKIP" };

	private List<IWizardPage> _wizardControlPages;

	private InstallerConfig _installerConfig;

	private InstallInfo _installInfo;

	private string _continuePageName;

	private bool _showLangPage;

	private bool _ss_registration;

	private IContainer components;

	private NotifyIcon notifyIcon1;

	private Wizard Wizard;

	private FlowManager FlowManager { get; set; }

	private List<FlowPage> FlowPages { get; set; }

	public Dictionary<string, string> Parameters { get; set; }

	public ILicenseProvider LicenseProvider { get; set; }

	public IEnumerable<Plugin> PluginsWizardPages { get; set; }

	public InstallerForm(int acceptStatistics, bool ss_registration, Dictionary<string, string> paramsFromCmd)
	{
		_installerConfig = InstallerManager.Instance.InstallerConfig;
		_continuePageName = InstallerManager.Instance.GetContinuePageName();
		Parameters = InstallerManager.Instance.Parameters;
		PluginsWizardPages = InstallerManager.Instance.Plugins;
		_installInfo = InstallerManager.Instance.InstallInfo;
		_showLangPage = InstallerManager.Instance.ShowLangPage;
		_ss_registration = ss_registration;
		InstallerManager.Instance.UpdateParameters(paramsFromCmd.Where((KeyValuePair<string, string> p) => ParametersCanBeOverwrittenFromCmd.Contains(p.Key)).ToDictionary((KeyValuePair<string, string> i) => i.Key, (KeyValuePair<string, string> i) => i.Value));
		_wizardControlPages = new List<IWizardPage>();
		if (_showLangPage && InstallerManager.Instance.Language != null)
		{
			InstallerManager.Instance.InstallerConfig.InstallerInfo.PrimaryLanguage = InstallerManager.Instance.Language;
		}
		InitializeComponent();
		if (_installerConfig.InstallerCaption != null)
		{
			Text = _installerConfig.InstallerCaption;
		}
		else
		{
			Text = VersionFormat.GenerateProductNameWithVersion();
		}
		if (GlobalData.OemHelper.InstallerIcon != null)
		{
			base.Icon = GlobalData.OemHelper.InstallerIcon;
		}
		else if (_installerConfig.Graphics.Icon != null)
		{
			base.Icon = Icon.ExtractAssociatedIcon(_installerConfig.Graphics.Icon.Filename);
		}
		BringToFront();
		CenterToParent();
		base.TopMost = true;
		Activate();
		base.TopMost = false;
		Focus();
		FlowPages = new List<FlowPage>();
		if (GlobalData.OemHelper.BackgroundImage != null)
		{
			Wizard.BackgroundImage = GlobalData.OemHelper.BackgroundImage;
		}
		else if (_installerConfig.Graphics.BackgroundImage != null && _installerConfig.Graphics.BackgroundImage.Filename != null)
		{
			Wizard.BackgroundImage = Image.FromFile(_installerConfig.Graphics.BackgroundImage.Filename);
		}
		FlowManager = new FlowManager(Wizard, InstallerManager.Instance.CollectPageSetupActions());
		FlowManager.PageSequencer = CollectPageConditions();
		ErrorHandler.FlowManager = FlowManager;
		Wizard wizard = Wizard;
		wizard.OnButtonBack = (ButtonBackEventHandler)Delegate.Combine(wizard.OnButtonBack, new ButtonBackEventHandler(Wizard_OnButtonBack));
		Wizard wizard2 = Wizard;
		wizard2.OnButtonNext = (ButtonNextEventHandler)Delegate.Combine(wizard2.OnButtonNext, new ButtonNextEventHandler(Wizard_OnButtonNext));
		Wizard.CloseFromCancel += Wizard_CloseFromCancel;
		Wizard.AutoBackPathEnabled = false;
		bool isModify = InstallerManager.Instance.IsModify;
		bool isUninstall = InstallerManager.Instance.IsUninstall;
		if (!isModify && !isUninstall)
		{
			SetupStartupPages(acceptStatistics);
			SetupPostPages();
		}
		else if (isModify || isUninstall)
		{
			StartupModifyPages();
			SetupPostUninstallPages();
			SetCurrentCultureInfo();
		}
		if (!string.IsNullOrEmpty(InstallerManager.Instance.Language))
		{
			SetCurrentCultureInfo();
		}
		List<string> list = InstallerManager.Instance.GetSuppressedPages().ToList();
		List<string> list2 = null;
		if (_installerConfig.InstallerSettings.EventsSettings != null)
		{
			list2 = new List<string>(_installerConfig.InstallerSettings.EventsSettings);
		}
		foreach (IWizardPage wizardControlPage in _wizardControlPages)
		{
			if (list.Count != 0 && list.Contains(wizardControlPage.PageName))
			{
				continue;
			}
			FlowPages.Add(new FlowPage(wizardControlPage, null, "ErrorMessagePage", "ErrorOccured"));
			List<string> eventParameter = wizardControlPage.GetEventParameter();
			if (eventParameter == null)
			{
				continue;
			}
			foreach (string item in eventParameter)
			{
				if (item.IndexOf(",") != -1)
				{
					string nextStep = item.Split(',')[0];
					string text = item.Split(',')[1];
					if (list2 == null || list2.Count == 0 || !list2.Contains(text))
					{
						FlowPages.Add(new FlowPage(wizardControlPage, null, nextStep, text));
					}
				}
			}
		}
		RegisterFlowPages(!isModify && !isUninstall, isUninstall);
	}

	private PageSequencerManager CollectPageConditions()
	{
		PageSequencerManager pageSequencerManager = new PageSequencerManager();
		if (_installerConfig.InstallerSettings.FlowSettings20 != null && _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings != null)
		{
			TreckType[] flowTrecks = _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks;
			foreach (TreckType treckType in flowTrecks)
			{
				ShowPageType[] showPages = treckType.ShowPages;
				foreach (ShowPageType showPageType in showPages)
				{
					if (showPageType.ByPassIf != null)
					{
						ShowPageTypeByPassIf[] byPassIf = showPageType.ByPassIf;
						foreach (ShowPageTypeByPassIf byPass in byPassIf)
						{
							pageSequencerManager.AddPageCondition(treckType.Id, showPageType.Name, byPass, isUninstall: false);
						}
					}
				}
			}
		}
		InstallerPluginType[] installerPlugins = _installerConfig.InstallerPlugins;
		foreach (InstallerPluginType installerPluginType in installerPlugins)
		{
			if (installerPluginType.WizardPages == null || installerPluginType.WizardPages.Length == 0)
			{
				continue;
			}
			InstallerPluginTypeWizardPage[] wizardPages = installerPluginType.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (installerPluginTypeWizardPage.ByPassIf != null)
				{
					ShowPageTypeByPassIf showPageTypeByPassIf = new ShowPageTypeByPassIf();
					showPageTypeByPassIf.ID = installerPluginTypeWizardPage.ByPassIf.ID;
					showPageTypeByPassIf.Name = installerPluginTypeWizardPage.ByPassIf.Name;
					showPageTypeByPassIf.Condition = installerPluginTypeWizardPage.ByPassIf.Condition;
					pageSequencerManager.AddPageCondition("modify", installerPluginTypeWizardPage.Name, showPageTypeByPassIf, installerPluginTypeWizardPage.UninstallPage);
					if (installerPluginTypeWizardPage.UninstallPage)
					{
						pageSequencerManager.AddPageCondition("uninstall", installerPluginTypeWizardPage.Name, showPageTypeByPassIf, installerPluginTypeWizardPage.UninstallPage);
					}
				}
			}
		}
		return pageSequencerManager;
	}

	private void SetCurrentCultureInfo()
	{
		string installerLanguage = _installInfo.InstallerLanguage;
		try
		{
			CultureInfo cultureInfo = ((installerLanguage == null) ? new CultureInfo(InstallerManager.Instance.InstallerConfig.InstallerInfo.PrimaryLanguage) : new CultureInfo(installerLanguage));
			Thread.CurrentThread.CurrentUICulture = cultureInfo;
			RefreshCulture(cultureInfo);
			CultureManager.Instance.Language_CultureChanged(this, new CultureChangedEventArgs(cultureInfo));
		}
		catch (ArgumentException exception)
		{
			LogManager.Write(exception);
		}
	}

	private void SetupPostUninstallPages()
	{
		List<IWizardPage> list = new List<IWizardPage>();
		List<IWizardPage> uninstallPluginWizardPages = GetUninstallPluginWizardPages();
		list.AddRange(uninstallPluginWizardPages);
		list.AddRange(new List<IWizardPage>
		{
			new WizardPageUninstallation(_installInfo),
			new WizardPageErrorMessage()
		});
		foreach (IWizardPage item in list)
		{
			_wizardControlPages.Add(item);
			FlowPages.Add(new FlowPage(item, null, null, null));
			WizardPageControl wizardPageControl = item as WizardPageControl;
			Wizard.Pages.Add(wizardPageControl.Page);
		}
	}

	private List<IWizardPage> GetUninstallPluginWizardPages()
	{
		Dictionary<string, IWizardPage> dictionary = new Dictionary<string, IWizardPage>();
		foreach (Plugin pluginsWizardPage in PluginsWizardPages)
		{
			InstallerPluginTypeWizardPage[] wizardPages = pluginsWizardPage.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (pluginsWizardPage.WizardProvider != null && installerPluginTypeWizardPage.UninstallPage)
				{
					dictionary[installerPluginTypeWizardPage.Name] = pluginsWizardPage.WizardProvider.CreateInstance(installerPluginTypeWizardPage.Name);
				}
			}
		}
		List<IWizardPage> list = new List<IWizardPage>();
		foreach (Plugin pluginsWizardPage2 in PluginsWizardPages)
		{
			InstallerPluginTypeFlowStep[] pageFlow = pluginsWizardPage2.PageFlow;
			foreach (InstallerPluginTypeFlowStep installerPluginTypeFlowStep in pageFlow)
			{
				if (dictionary.ContainsKey(installerPluginTypeFlowStep.Page) && !list.Contains(dictionary[installerPluginTypeFlowStep.Page]))
				{
					list.Add(dictionary[installerPluginTypeFlowStep.Page]);
				}
			}
		}
		return list;
	}

	private void StartupModifyPages()
	{
		List<string> list = InstallerManager.Instance.GetSuppressedPages().ToList();
		List<IWizardPage> list2 = new List<IWizardPage>();
		list2.Add(new WizardPageUninstallWelcome());
		list2.Add(new WizardPageModifyComponent());
		list2.AddRange(GetPluginWizardPages(skipInTypical: false).ToArray());
		list2.AddRange(GetUninstallPluginWizardPages().ToArray());
		list2.AddRange(new List<IWizardPage>
		{
			new WizardPageWarning(),
			new WizardPageCertificate(),
			new WizardPageInstall(),
			new WizardPageFinish()
		});
		foreach (IWizardPage item in list2)
		{
			_wizardControlPages.Add(item);
			if (list.Count == 0 || !list.Contains(item.PageName))
			{
				if (item.PageName == "UninstallWelcomePage")
				{
					FlowPages.Add(new FlowPage(item, null, "UninstallationPage", "Uninstall"));
				}
				FlowPages.Add(new FlowPage(item, null, null, null));
				WizardPageControl wizardPageControl = item as WizardPageControl;
				Wizard.Pages.Add(wizardPageControl.Page);
			}
		}
	}

	private void SetupStartupPages(int acceptStatistics)
	{
		List<string> list = InstallerManager.Instance.GetSuppressedPages().ToList();
		if (list.Contains("LanguagePage") && string.IsNullOrEmpty(InstallerManager.Instance.Language))
		{
			InstallerManager.Instance.Language = InstallerManager.Instance.InstallerConfig.InstallerInfo.PrimaryLanguage;
			InstallerManager.Instance.InstallInfo.InstallerLanguage = InstallerManager.Instance.InstallerConfig.InstallerInfo.PrimaryLanguage;
		}
		WizardPageLanguage wizardPageLanguage = new WizardPageLanguage(InstallerManager.Instance.InstallerLanguages, InstallerManager.Instance.InstallerConfig.InstallerInfo.PrimaryLanguage, InstallerManager.Instance.Language, Wizard);
		wizardPageLanguage.CultureChanged += langPage_CultureChanged;
		wizardPageLanguage.CultureChanged += CultureManager.Instance.Language_CultureChanged;
		wizardPageLanguage.ErrorOccurred += langPage_ErrorOccurred;
		WizardPageInstallationType item = ((_installerConfig.InstallerSettings.FlowSettings20 == null || _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings == null || !_installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.IntersectionPageName.Equals("InstallationTypePage", StringComparison.OrdinalIgnoreCase) || _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks.Length != 2) ? new WizardPageInstallationType() : new WizardPageInstallationType(_installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks[0].Id, _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks[1].Id));
		List<IWizardPage> list2 = new List<IWizardPage>();
		list2.Add(wizardPageLanguage);
		list2.Add(new WizardPageEula(acceptStatistics));
		list2.Add(new WizardPageWarning());
		list2.Add(new WizardPageCertificate());
		list2.Add(new WizardPageTrecksIntersection());
		list2.Add(item);
		list2.Add(new WizardPageComponents());
		list2.Add(new WizardPageIISSetup());
		List<IWizardPage> pluginWizardPages = GetPluginWizardPages(skipInTypical: true);
		list2.AddRange(pluginWizardPages);
		foreach (IWizardPage item2 in list2)
		{
			_wizardControlPages.Add(item2);
			WizardPageControl wizardPageControl = item2 as WizardPageControl;
			if (list.Count != 0 && list.Contains(wizardPageControl.PageName))
			{
				continue;
			}
			string pageName = wizardPageControl.PageName;
			if (!(pageName == "InstallationTypePage"))
			{
				if (pageName == "LanguagePage")
				{
					if (InstallerManager.Instance.Language == null || _showLangPage)
					{
						FlowPages.Add(new FlowPage(item2, null, null, null));
					}
					else
					{
						SetCurrentCultureInfo();
					}
				}
				else
				{
					FlowPages.Add(new FlowPage(item2, null, null, null));
				}
			}
			else
			{
				FlowPages.Add(new FlowPage(item2, null, GetPageForTypical(), "TypicalInstall"));
				FlowPages.Add(new FlowPage(item2, null, null, null));
			}
			if (!(wizardPageControl.PageName == "LanguagePage") || InstallerManager.Instance.Language == null || _showLangPage)
			{
				Wizard.Pages.Add(wizardPageControl.Page);
			}
		}
	}

	private string GetPageForTypical()
	{
		List<string> list = new List<string>();
		foreach (Plugin pluginsWizardPage in PluginsWizardPages)
		{
			InstallerPluginTypeWizardPage[] wizardPages = pluginsWizardPage.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (pluginsWizardPage.WizardProvider != null && !installerPluginTypeWizardPage.SkipInTypical && !installerPluginTypeWizardPage.UninstallPage)
				{
					list.Add(installerPluginTypeWizardPage.Name);
				}
			}
		}
		foreach (Plugin pluginsWizardPage2 in PluginsWizardPages)
		{
			InstallerPluginTypeFlowStep[] pageFlow = pluginsWizardPage2.PageFlow;
			foreach (InstallerPluginTypeFlowStep installerPluginTypeFlowStep in pageFlow)
			{
				if (list.Contains(installerPluginTypeFlowStep.Page))
				{
					return installerPluginTypeFlowStep.Page;
				}
			}
		}
		if (InstallerManager.Instance.GetSuppressedPages().ToList().Contains("ServiceAccountPage"))
		{
			return "InitializePage";
		}
		return "ServiceAccountPage";
	}

	private List<IWizardPage> GetPluginWizardPages(bool skipInTypical)
	{
		Dictionary<string, IWizardPage> dictionary = new Dictionary<string, IWizardPage>();
		foreach (Plugin pluginsWizardPage in PluginsWizardPages)
		{
			InstallerPluginTypeWizardPage[] wizardPages = pluginsWizardPage.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (pluginsWizardPage.WizardProvider != null && installerPluginTypeWizardPage.SkipInTypical == skipInTypical && !installerPluginTypeWizardPage.UninstallPage)
				{
					dictionary[installerPluginTypeWizardPage.Name] = pluginsWizardPage.WizardProvider.CreateInstance(installerPluginTypeWizardPage.Name);
				}
			}
		}
		List<IWizardPage> list = new List<IWizardPage>();
		foreach (Plugin pluginsWizardPage2 in PluginsWizardPages)
		{
			InstallerPluginTypeFlowStep[] pageFlow = pluginsWizardPage2.PageFlow;
			foreach (InstallerPluginTypeFlowStep installerPluginTypeFlowStep in pageFlow)
			{
				if (dictionary.ContainsKey(installerPluginTypeFlowStep.Page) && !list.Contains(dictionary[installerPluginTypeFlowStep.Page]))
				{
					list.Add(dictionary[installerPluginTypeFlowStep.Page]);
				}
			}
		}
		return list;
	}

	private void langPage_ErrorOccurred(object sender, ErrorOccuredEventArgs e)
	{
		SetErroPage(e.Exception);
	}

	private void SetErroPage(Exception ex)
	{
		if (base.InvokeRequired)
		{
			ShowErrorDelegate method = SetErroPage;
			Invoke(method, ex);
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

	private void RefreshCulture(CultureInfo culture)
	{
		try
		{
			Resources.Culture = culture;
			foreach (IWizardPage wizardControlPage in _wizardControlPages)
			{
				wizardControlPage.ReloadResources(culture);
			}
			Wizard.ReloadResources(culture);
			if (GlobalData.OemHelper.BackgroundImage != null)
			{
				Wizard.BackgroundImage = GlobalData.OemHelper.BackgroundImage;
			}
			else if (_installerConfig.Graphics.BackgroundImage != null && _installerConfig.Graphics.BackgroundImage.Filename != null)
			{
				Wizard.BackgroundImage = Image.FromFile(_installerConfig.Graphics.BackgroundImage.Filename);
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			ErrorHandler.ShowError(exception);
		}
	}

	private void langPage_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		RefreshCulture(e.Culture);
	}

	private void SetupPostPages()
	{
		List<IWizardPage> pluginWizardPages = GetPluginWizardPages(skipInTypical: false);
		InstallerSettingsTypeServiceAccountSetup serviceAccountSetup = InstallerManager.Instance.InstallerConfig.InstallerSettings.ServiceAccountSetup;
		pluginWizardPages.AddRange(new List<IWizardPage>
		{
			new WizardPageServiceAccount(serviceAccountSetup?.DisableAllAccount ?? false, serviceAccountSetup?.DisablePreferedAccounts, serviceAccountSetup?.DefaultPredefAccount, serviceAccountSetup?.OnParameterName, serviceAccountSetup?.OnParameterValue, serviceAccountSetup?.DisableAllPreferedAccount ?? false),
			new WizardPageServerSelection(),
			new WizardPageFileLocation(),
			new WizardPageInitialize(),
			new WizardPageInstall(),
			new WizardPageFinish(),
			new WizardPageErrorMessage(),
			new WizardPagePrerequisiteInstall()
		});
		List<string> list = InstallerManager.Instance.GetSuppressedPages().ToList();
		foreach (IWizardPage item in pluginWizardPages)
		{
			if (!list.Contains(item.PageName))
			{
				_wizardControlPages.Add(item);
				_ = item.PageName == "InstallPage";
				FlowPages.Add(new FlowPage(item, null, null, null));
				WizardPageControl wizardPageControl = item as WizardPageControl;
				Wizard.Pages.Add(wizardPageControl.Page);
			}
		}
	}

	private void RegisterFlowPages(bool isInstallation, bool isUninstall)
	{
		Dictionary<string, List<FlowPage>> dictionary = new Dictionary<string, List<FlowPage>>();
		bool onlyOneTreck = false;
		dictionary = ((!isInstallation) ? CreateUninstallationTreks() : CreateInstallationTrecks(out onlyOneTreck));
		if (onlyOneTreck)
		{
			List<FlowPage> list = new List<FlowPage>();
			string nextStep = null;
			if (_installerConfig.InstallerSettings.FlowSettings20 != null && _installerConfig.InstallerSettings.FlowSettings20.PageSequencer != null)
			{
				List<string> list2 = new List<string>(_installerConfig.InstallerSettings.FlowSettings20.PageSequencer);
				list2.AddRange(InstallerManager.Instance.NecessaryPages);
				for (int num = list2.Count - 1; num >= 0; num--)
				{
					string text = list2[num];
					List<FlowPage> flowPage = GetFlowPage(text);
					if (flowPage != null)
					{
						foreach (FlowPage item in flowPage)
						{
							if (string.IsNullOrEmpty(item.NextStep))
							{
								item.NextStep = nextStep;
							}
						}
						list.AddRange(flowPage);
						nextStep = text;
					}
				}
				dictionary.Add("default", list);
				FlowManager.ActiveTreckKey = "default";
			}
			else
			{
				for (int num2 = FlowPages.Count - 1; num2 > 0; num2--)
				{
					if (string.IsNullOrEmpty(FlowPages[num2 - 1].NextStep))
					{
						FlowPages[num2 - 1].NextStep = FlowPages[num2].Name;
					}
				}
				dictionary.Add("default", FlowPages);
				FlowManager.ActiveTreckKey = "default";
			}
		}
		foreach (KeyValuePair<string, List<FlowPage>> item2 in dictionary)
		{
			FlowManager.AddFlowTreckPages(item2);
		}
		if (_continuePageName == null)
		{
			FlowManager.ActivePage = FlowPages[0].Page;
		}
		else
		{
			FlowManager.ActivePage = FlowPages[GetContinuePageIndex()].Page;
			FlowManager.SetContinueFromPage(FlowPages[GetContinuePageIndex()]);
			if (!isInstallation)
			{
				FlowManager.ActiveTreckKey = (isUninstall ? "uninstall" : "modify");
			}
		}
		if (FlowManager.ActiveTreckKey == null && !isInstallation)
		{
			FlowManager.ActiveTreckKey = (isUninstall ? "uninstall" : "modify");
		}
		Wizard.CancelButton.Text = Resources.btn_Cancel;
	}

	private Dictionary<string, List<FlowPage>> CreateUninstallationTreks()
	{
		Dictionary<string, List<FlowPage>> dictionary = new Dictionary<string, List<FlowPage>>();
		List<string> uninstallPluginWizardPagesNames = GetUninstallPluginWizardPagesNames();
		List<string> list = new List<string>();
		if (_installerConfig.InstallerSettings.FlowSettings20 != null && _installerConfig.InstallerSettings.FlowSettings20.PageSequencer != null)
		{
			list.AddRange(_installerConfig.InstallerSettings.FlowSettings20.PageSequencer);
		}
		else
		{
			list.AddRange(InstallerManager.Instance.AvailablePages10);
		}
		List<string> list2 = new List<string>(new string[4] { "UninstallWelcomePage", "ModifyCompPage", "ServiceAccountPage", "ServerSelectionPage" });
		List<string> list3 = new List<string>();
		foreach (IWizardPage pluginWizardPage in GetPluginWizardPages(skipInTypical: false))
		{
			list3.Add(pluginWizardPage.PageName);
		}
		list3.AddRange(uninstallPluginWizardPagesNames);
		list2.AddRange(list3);
		list2.AddRange(new string[2] { "CertificateCommonPage", "WarningCommonPage" });
		list2.AddRange(new string[3] { "InstallPage", "FinishPage", "ErrorMessagePage" });
		List<FlowPage> list4 = new List<FlowPage>();
		string nextStep = null;
		for (int num = list2.Count - 1; num >= 0; num--)
		{
			string text = list2[num];
			if (list.Contains(text) || InstallerManager.Instance.NecessaryPages.Contains(text) || list3.Contains(text))
			{
				List<FlowPage> flowPage = GetFlowPage(text);
				if (flowPage != null)
				{
					foreach (FlowPage item in flowPage)
					{
						if (string.IsNullOrEmpty(item.NextStep))
						{
							item.NextStep = nextStep;
						}
					}
					list4.AddRange(flowPage);
					nextStep = text;
				}
			}
		}
		dictionary.Add("modify", list4);
		List<string> list5 = new List<string>();
		list5.Add("UninstallWelcomePage");
		list5.AddRange(uninstallPluginWizardPagesNames);
		list5.Add("UninstallationPage");
		list5.Add("ErrorMessagePage");
		List<FlowPage> list6 = new List<FlowPage>();
		for (int num2 = list5.Count - 1; num2 >= 0; num2--)
		{
			string text2 = list5[num2];
			if (list.Contains(text2) || InstallerManager.Instance.NecessaryPages.Contains(text2) || uninstallPluginWizardPagesNames.Contains(text2))
			{
				List<FlowPage> flowPage2 = GetFlowPage(text2);
				if (flowPage2 != null)
				{
					foreach (FlowPage item2 in flowPage2)
					{
						if (string.IsNullOrEmpty(item2.NextStep))
						{
							item2.NextStep = nextStep;
						}
					}
					list6.AddRange(flowPage2);
					nextStep = text2;
				}
			}
		}
		dictionary.Add("uninstall", list6);
		return dictionary;
	}

	private List<string> GetUninstallPluginWizardPagesNames()
	{
		List<string> list = new List<string>();
		foreach (Plugin pluginsWizardPage in PluginsWizardPages)
		{
			InstallerPluginTypeWizardPage[] wizardPages = pluginsWizardPage.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (pluginsWizardPage.WizardProvider != null && installerPluginTypeWizardPage.UninstallPage)
				{
					list.Add(installerPluginTypeWizardPage.Name);
				}
			}
		}
		return list;
	}

	private Dictionary<string, List<FlowPage>> CreateInstallationTrecks(out bool onlyOneTreck)
	{
		Dictionary<string, List<FlowPage>> dictionary = new Dictionary<string, List<FlowPage>>();
		onlyOneTreck = false;
		if (_installerConfig.InstallerSettings.FlowSettings20 != null && _installerConfig.InstallerSettings.FlowSettings20.PageSequencer != null)
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>(_installerConfig.InstallerSettings.FlowSettings20.PageSequencer);
			bool flag = false;
			if (_installerConfig.InstallerSettings.FlowSettings20 != null && _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings != null && _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks != null)
			{
				TreckType[] flowTrecks = _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks;
				for (int i = 0; i < flowTrecks.Length; i++)
				{
					if (flowTrecks[i].PagesToShowPostInstall != null)
					{
						flag = true;
						break;
					}
				}
			}
			if (!flag)
			{
				list2.AddRange(InstallerManager.Instance.NecessaryPages);
			}
			if (_installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings != null)
			{
				string intersectionPageName = _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.IntersectionPageName;
				foreach (string item in list2)
				{
					list.Add(item);
					if (item == intersectionPageName)
					{
						break;
					}
				}
				TreckType[] flowTrecks2 = _installerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks;
				TreckType[] flowTrecks = flowTrecks2;
				foreach (TreckType treckType in flowTrecks)
				{
					List<string> list3 = new List<string>();
					ShowPageType[] showPages = treckType.ShowPages;
					foreach (ShowPageType showPageType in showPages)
					{
						list3.Add(showPageType.Name);
					}
					HashSet<string> hashSet = new HashSet<string>();
					if (treckType.PagesToShowPostInstall != null)
					{
						showPages = treckType.PagesToShowPostInstall;
						foreach (ShowPageType showPageType2 in showPages)
						{
							hashSet.Add(showPageType2.Name);
						}
					}
					if (hashSet.Count > 0)
					{
						list2.RemoveAll((string s) => s.Equals("FinishPage"));
					}
					List<FlowPage> list4 = new List<FlowPage>();
					string nextStep = null;
					for (int num = list2.Count - 1; num >= 0; num--)
					{
						string text = list2[num];
						if (list.Contains(text) || list3.Contains(text) || InstallerManager.Instance.NecessaryPages.Contains(text) || hashSet.Contains(text))
						{
							List<FlowPage> flowPage = GetFlowPage(text);
							if (flowPage != null)
							{
								foreach (FlowPage item2 in flowPage)
								{
									if (string.IsNullOrEmpty(item2.NextStep))
									{
										item2.NextStep = nextStep;
									}
									if (hashSet.Contains(text))
									{
										item2.PostInstall = true;
									}
								}
								list4.AddRange(flowPage);
								nextStep = text;
							}
						}
					}
					dictionary.Add(treckType.Id, list4);
				}
				FlowManager.ActiveTreckKey = flowTrecks2[0].Id;
			}
			else
			{
				onlyOneTreck = true;
			}
		}
		else
		{
			onlyOneTreck = true;
		}
		return dictionary;
	}

	private List<FlowPage> GetFlowPage(string pageName)
	{
		List<FlowPage> list = new List<FlowPage>();
		foreach (FlowPage flowPage in FlowPages)
		{
			if (flowPage.Name == pageName)
			{
				FlowPage item = new FlowPage(flowPage.Page, flowPage.Control, flowPage.NextStep, flowPage.Event);
				list.Add(item);
			}
		}
		if (list.Count == 0)
		{
			return null;
		}
		return list;
	}

	private int GetContinuePageIndex()
	{
		int num = 0;
		foreach (FlowPage flowPage in FlowPages)
		{
			if (flowPage.Name == _continuePageName)
			{
				LogManager.Write($"Installation continues from {flowPage.Name}.");
				return num;
			}
			num++;
		}
		return num;
	}

	private bool Wizard_OnButtonBack(Wizard wizard)
	{
		FlowManager.Back();
		return false;
	}

	private bool Wizard_OnButtonNext(Wizard wizard)
	{
		bool errorOccured = false;
		using (new ShowWaitCursor())
		{
			if (FlowManager.RunBeforeLeave(out errorOccured))
			{
				if (errorOccured)
				{
					return false;
				}
				Dictionary<string, string> obj = FlowManager.ActivePage.CollectParameters() ?? new Dictionary<string, string>();
				obj.Merge(FlowManager.ActivePageSetupActions?.CollectParameters());
				InstallerManager.Instance.MergeGlobalDataToParameters();
				foreach (KeyValuePair<string, string> item in obj)
				{
					Parameters[item.Key] = item.Value;
					GlobalData.SetValue(item.Key, item.Value, "Wizard_OnButtonNext.MergeParametersFromPage()");
					if (item.Key == "LANGUAGE")
					{
						if (FlowManager.ActivePage.PageName == "FileLocatorPage")
						{
							InstallerManager.Instance.InstallInfo.ProductLanguage = item.Value;
						}
						else if (FlowManager.ActivePage.PageName == "LanguagePage")
						{
							InstallerManager.Instance.InstallInfo.InstallerLanguage = item.Value;
						}
						InstallerManager.Instance.Language = item.Value;
					}
					else if (item.Key == "TRECK_ID")
					{
						FlowManager.ActiveTreckKey = Parameters["TRECK_ID"];
					}
				}
				try
				{
					InstallerManager.Instance.UpdatePackageParameters();
				}
				catch (Exception exception)
				{
					LogManager.Write(exception);
					ErrorHandler.ShowError(exception);
				}
				string flowEvent = FlowManager.ActivePage.OnTriggerEvent();
				if (FlowManager.ActivePage.PageName == "TreckIntersectionPage")
				{
					Wizard.NavigationPanel.Show();
				}
				FlowManager.SetParametersProp(Parameters);
				FlowManager.TriggerEvent(flowEvent);
				InstallerManager.Instance.MergeGlobalDataToParameters();
				FlowManager.ActivePage.SetParameters(Parameters);
			}
		}
		return false;
	}

	private void InstallerForm_Load(object sender, EventArgs e)
	{
		if (FlowManager.ActivePage.PageName == "WelcomePage")
		{
			Wizard.NavigationPanel.Hide();
		}
		Focus();
		Activate();
		if (!string.IsNullOrEmpty(_continuePageName))
		{
			return;
		}
		if (_ss_registration)
		{
			Environment.ExitCode = (int)InstallerManager.Instance.PerformSSRegistration();
			string text;
			if (Environment.ExitCode == 0)
			{
				text = string.Format(Resources.Message_SSRegistrationPerformed, VersionFormat.GenerateProductNameWithVersion());
				ErrorInfo.SetErrorInfo(VersionFormat.GenerateProductNameWithVersion(), text, null, null, enablePreviousButton: false);
			}
			else
			{
				text = string.Format(Resources.Message_SSRegistrationPerfomFailed, InstallerManager.Instance.InstallerConfig.InstallerInfo.DisplayName, Environment.ExitCode);
				ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, text, null, null, enablePreviousButton: false);
			}
			SetErroPage(null);
			LogManager.Write(text, TraceEventType.Error);
			return;
		}
		if (_installerConfig.InstallerPrerequisites != null && !InstallerManager.Instance.IsModify && !InstallerManager.Instance.IsUninstall && FlowManager.ActivePage.PageName != "PrerequisiteInstall" && _installerConfig.InstallerPrerequisites.InstallerPrerequisite != null && _installerConfig.InstallerPrerequisites.InstallerPrerequisite.Length != 0)
		{
			try
			{
				InstallerManager.Instance.PrerequisiteManager.CheckInstallerPrerequisites();
			}
			catch (Exception exception)
			{
				ErrorHandler.ShowError(exception);
				return;
			}
			Dictionary<string, string> packagesCannotInstall = InstallerManager.Instance.PrerequisiteManager.PackagesCannotInstall;
			if (packagesCannotInstall.Count != 0)
			{
				ErrorInfo.SetMissingPrereqErrorInfo(Resources.ErrorMessage_PackagesRequiredLabelHeadLine, "", null, packagesCannotInstall, enablePreviousButton: false);
				SetErroPage(null);
			}
		}
		if (!InstallerManager.Instance.TargetPlatformCheck())
		{
			string text2 = string.Format(Resources.ErrorMessage_ArchitectureMissmatchText, (InstallerManager.Instance.InstallerConfig.InstallerInfo.TargetPlatform.ToUpper() == "X86") ? "32" : "64");
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, text2, null, null, enablePreviousButton: false);
			SetErroPage(null);
			LogManager.Write(text2, TraceEventType.Error);
		}
		if (!InstallerManager.Instance.IsWindowsVersionSupported(new OSInformationReader()))
		{
			string text3 = string.Format(Resources.ErrorMessage_WinVersionMissmatchText, VersionFormat.GetNeutralProductName());
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, text3, null, null, enablePreviousButton: false);
			SetErroPage(null);
			LogManager.Write(text3, TraceEventType.Error);
		}
		if (InstallerManager.Instance.AnotherInstanceRunning())
		{
			string errorMessage_AnotherInstanceRunning = Resources.ErrorMessage_AnotherInstanceRunning;
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, errorMessage_AnotherInstanceRunning, null, null, enablePreviousButton: false);
			SetErroPage(null);
			LogManager.Write(errorMessage_AnotherInstanceRunning, TraceEventType.Error);
		}
		if (!FlowManager.ActivePage.OnBeforeActivate(null))
		{
			FlowManager.TriggerEvent(null);
		}
	}

	private bool GetCloseConfirmationFromUser()
	{
		if (FlowManager.ActivePage.PageName == "InstallPage" || FlowManager.ActivePage.PageName == "UninstallationPage" || FlowManager.ActivePage.PageName == "PrerequisiteInstall" || FlowManager.ActivePage.PageName == "InitializePage")
		{
			return false;
		}
		if (FlowManager.ActivePage.PageName == "ProcessLicenseUpdatePage" && GlobalData.TryGetValue("LICENSE", out var value) && value.Contains("license.lic.new"))
		{
			if (MessageHelper.ShowInstallerMessageBox(Resources.MessageBox_ConfirmCancelCaption, string.Format(CultureInfo.CurrentCulture, Resources.MessageBox_ConfirmCancelLicenseText, GlobalData.OemHelper.CompanyName), InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return false;
			}
			Environment.ExitCode = 100;
			return true;
		}
		if (!(FlowManager.ActivePage as WizardPageControl).Page.FinishPage && FlowManager.ActivePage.PageName != "ErrorMessagePage")
		{
			string empty = string.Empty;
			string message;
			if (FlowManager.IsPagePostInstallation(FlowManager.ActivePage.PageName))
			{
				message = Resources.MessageBox_ConfirmPostInstallationClose;
				empty = Resources.MessageBox_ConfirmPostInstallationCloseCaption;
			}
			else
			{
				message = Resources.MessageBox_ConfirmCancelText;
				empty = Resources.MessageBox_ConfirmCancelCaption;
			}
			if (MessageHelper.ShowInstallerMessageBox(empty, message, InstallerMessageBoxIcon.Question, InstallerMessageBoxButtons.YesNo) != DialogResult.Yes)
			{
				return false;
			}
			if (GlobalData.TryGetValue("INSTALLFINISHED", out var value2) && bool.Parse(value2))
			{
				Environment.ExitCode = 80;
			}
			else
			{
				Environment.ExitCode = 100;
			}
		}
		else if (FlowManager.ActivePage.PageName == "ErrorMessagePage")
		{
			Environment.ExitCode = 60;
		}
		return true;
	}

	private void InstallerForm_FormClosing(object sender, FormClosingEventArgs e)
	{
		if (e.CloseReason != CloseReason.UserClosing)
		{
			return;
		}
		if (InstallerManager.Instance.FormShowConfirmClosing)
		{
			FlowManager.ActivePage.OnBeforeCancel();
			if (!GetCloseConfirmationFromUser())
			{
				e.Cancel = true;
				return;
			}
		}
		try
		{
			InstallerManager.Instance.CloseActions();
		}
		catch (LogManagerFailedException ex)
		{
			e.Cancel = true;
			ErrorInfo.SetErrorInfo(Resources.ErrorMessage_GeneralErrorLabelHeadLine, ex.Message, null, null, enablePreviousButton: false);
			SetErroPage(null);
		}
		catch (Exception ex2)
		{
			e.Cancel = true;
			LogManager.Write("Error when closing the installer form: ", ex2.TypeAndMessage(), TraceEventType.Error);
			SetErroPage(ex2);
		}
	}

	private void Wizard_CloseFromCancel(object sender, CancelEventArgs e)
	{
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
		this.components = new System.ComponentModel.Container();
		System.ComponentModel.ComponentResourceManager componentResourceManager = new System.ComponentModel.ComponentResourceManager(typeof(VideoOS.Installer.UI.InstallerForm));
		this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
		this.Wizard = new VideoOS.Installer.UI.Framework.Wizard();
		base.SuspendLayout();
		componentResourceManager.ApplyResources(this.notifyIcon1, "notifyIcon1");
		this.Wizard.AutoBackPathEnabled = false;
		componentResourceManager.ApplyResources(this.Wizard, "Wizard");
		this.Wizard.HelpButtonEventHandler = null;
		this.Wizard.Name = "Wizard";
		this.Wizard.Page = null;
		componentResourceManager.ApplyResources(this, "$this");
		base.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
		base.Controls.Add(this.Wizard);
		base.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
		base.KeyPreview = true;
		base.MaximizeBox = false;
		base.MinimizeBox = false;
		base.Name = "InstallerForm";
		base.FormClosing += new System.Windows.Forms.FormClosingEventHandler(InstallerForm_FormClosing);
		base.Load += new System.EventHandler(InstallerForm_Load);
		base.ResumeLayout(false);
	}
}
