using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Serialization;
using Microsoft.Extensions.Logging;
using Microsoft.Win32;
using VideoOS.Common.EnvironmentServices;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Properties;
using VideoOS.Installer.Common.ServiceAccountHelper;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Core.Arguments;
using VideoOS.Installer.Core.Arguments.Interfaces;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Content;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.Manager.CertificateCommon;
using VideoOS.Installer.Manager.Properties;
using VideoOS.Installer.Prerequisites;
using VideoOS.Installer.Sequencer;
using VideoOS.Installer.Sequencer.ErrorHandling;
using VideoOS.Installer.Sequencer.Prerequisite;
using VideoOS.Installer.UI.Framework;
using VideoOS.Installer.UI.Framework.PageActions;
using VideoOS.ServerConfigurator.Interfaces;

namespace VideoOS.Installer.Manager;

public class InstallerManager
{
	private class Arguments : IArgumentCollection, IEnumerable<IArgument>, IEnumerable
	{
		private class ParameterContent : IArgument
		{
			public string Name { get; private set; }

			public string Description { get; private set; }

			public string Value { get; private set; }

			public ParameterContent(string name, string parameterValue, string parameterDescription)
			{
				Name = name;
				Value = parameterValue;
				Description = parameterDescription;
			}
		}

		private Dictionary<string, IArgument> _arguments = new Dictionary<string, IArgument>();

		public VideoOS.Installer.Core.Arguments.Interfaces.LicenseType LicenseType { get; private set; }

		public IArgument this[string argumentName]
		{
			get
			{
				if (!_arguments.TryGetValue(argumentName, out var value))
				{
					return null;
				}
				return value;
			}
		}

		public Arguments()
			: this(VideoOS.Installer.Core.Arguments.Interfaces.LicenseType.Unspecified)
		{
		}

		public Arguments(VideoOS.Installer.Core.Arguments.Interfaces.LicenseType licenseType)
		{
			LicenseType = licenseType;
		}

		public void AddArgument(IArgument parameterContent)
		{
			_arguments[parameterContent.Name] = parameterContent;
		}

		public void AddArgument(string key, string value, string description)
		{
			AddArgument(new ParameterContent(key, value, description));
		}

		public IEnumerator<IArgument> GetEnumerator()
		{
			return _arguments.Values.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}
	}

	public static class ProductProperty
	{
		public const string DISPLAYNAME = "DisplayName";

		public const string DISPLAYVERSION = "DisplayVersion";

		public const string DISPLAYVERSIONMAJOR = "VersionMajor";

		public const string DISPLAYVERSIONMINOR = "VersionMinor";
	}

	public string _softwareKey = VideoOS.Installer.Common.Properties.Settings.Default.SoftwareKey;

	private string _publisherKeyForUnitTestOnly;

	private List<string> _suppressedPages;

	private const string _licenseLocationRegKeyName = "LicenseLocation";

	private const string _licenseProdKeyRegKeyName = "ProductKey";

	private const string VariableLocalMachine = "[LocalMachine]";

	private List<string> _parametersNotSaved = new List<string> { "HARDWARE-MANUFACTURER-MODE", "FINALIZE-MODE", "REBOOTONPACKAGEID", "QUIET-MODE", "SYSTEMPASSWORD", "AWSSECRET" };

	private int _acceptStatistics;

	private bool _debugMode;

	private string _partnerId;

	private string _idmId;

	private bool _isModify;

	private bool _doFormClosingActions;

	private InstallationParams _oldInstallation;

	private ModelRepository _modelRepository;

	public bool FormShowConfirmClosing = true;

	private static InstallerManager _instance = new InstallerManager();

	private string _dashes = "-------------------------------------";

	private string _productSku;

	private string _continueDir;

	private bool _showLangPage;

	private string _exeLocation;

	private string _parentProcessName;

	public IClMsiHandler ClMsiHandler;

	private Lazy<string> _entryAssemblyName = new Lazy<string>(() => Assembly.GetEntryAssembly().GetName().Name + ".exe");

	public string PublisherKey
	{
		get
		{
			return _publisherKeyForUnitTestOnly ?? GlobalData.PublisherKey;
		}
		set
		{
			_publisherKeyForUnitTestOnly = value;
		}
	}

	private static string ManufacturerInfoReg => GlobalData.ManufacturerInfoReg;

	public ReadOnlyCollection<string> AvailablePages10 { get; } = new ReadOnlyCollection<string>(new List<string>
	{
		"LanguagePage", "WelcomePage", "ServiceAccountPage", "PrerequisiteInstall", "InstallationTypePage", "IISSetupPage", "EulaPage", "ComponentPage", "UninstallWelcomePage", "UninstallationPage",
		"ModifyCompPage", "WarningCommonPage", "FileLocatorPage", "InitializePage", "InstallPage", "FinishPage", "CertificateCommonPage"
	});

	public ReadOnlyCollection<string> NecessaryPages { get; } = new ReadOnlyCollection<string>(new List<string> { "FileLocatorPage", "UninstallWelcomePage", "ModifyCompPage", "UninstallationPage", "InitializePage", "InstallPage", "FinishPage", "ErrorMessagePage" });

	public ReadOnlyCollection<string> AvailablePages20 { get; } = new ReadOnlyCollection<string>(new List<string> { "ServerSelectionPage", "TreckIntersectionPage", "LicenseUpdatePage", "ProcessLicenseUpdatePage" });

	public ReadOnlyDictionary<string, Func<IPageSetupActions>> BuiltInPageActions { get; } = new ReadOnlyDictionary<string, Func<IPageSetupActions>>(new Dictionary<string, Func<IPageSetupActions>>
	{
		{
			"ProcessLicenseUpdatePage",
			() => new ProcessLicenseUpdatePageActions()
		},
		{
			"WelcomePage",
			() => new WelcomePageActions()
		},
		{
			"IISSetupPage",
			() => new IISSetupPageActions()
		},
		{
			"ServiceAccountPage",
			() => new ServiceAccountPageActions()
		},
		{
			"TreckIntersectionPage",
			() => new TrecksIntersectionPageActions()
		},
		{
			"ComponentPage",
			() => new ComponentsPageActions()
		},
		{
			"LanguagePage",
			() => new LanguagePageActions()
		},
		{
			"ServerSelectionPage",
			() => new ServerSelectionPageActions()
		},
		{
			"CertificateCommonPage",
			() => new CertificatePageActions()
		}
	});

	public string CurrentDirectory { get; set; }

	public ISequenceManagerWrapper SequenceManagerWrapper { get; set; }

	public ISequenceManager SequenceManager => SequenceManagerWrapper.SequenceManager;

	public InstallerConfig InstallerConfig { get; set; }

	public ManufacturerInfo ManufacturerInfo { get; set; }

	public Dictionary<string, string> Parameters { get; set; }

	public Dictionary<string, string> QuietOverWriteParameters { get; set; }

	public ILicenseProvider LicenseProvider { get; set; }

	public IEnumerable<Plugin> Plugins { get; set; }

	public SubscriptionPlugins SubscriptionPlugins { get; set; }

	public Dictionary<string, IPageSetupActions> PageActionPlugins { get; set; }

	public string[] InstallerLanguages { get; set; }

	public PrerequisiteManager PrerequisiteManager { get; set; }

	public Dictionary<string, List<IPackageParameter>> PackagesParameters { get; private set; }

	public InstallInfo InstallInfo { get; set; }

	public ModelRepository ModelRepository
	{
		get
		{
			if (_modelRepository == null)
			{
				_modelRepository = new ModelRepository(new FileService(), LogManager.InnerLog);
				_modelRepository.InterfaceResolve += _modelRepository_InterfaceResolve;
			}
			return _modelRepository;
		}
		public set
		{
			_modelRepository = value;
		}
	}

	public InstallInfo OldInstallInfo
	{
		get
		{
			return _oldInstallation?.InstallInfo;
		}
		set
		{
			_oldInstallation = ((_oldInstallation != null) ? new InstallationParams(_oldInstallation.InstallerSourceDir, _oldInstallation.InstallerConfiguration, value, _oldInstallation.PackageParameters) : new InstallationParams("", new InstallerConfig(), value, new Dictionary<string, List<IPackageParameter>>()));
		}
	}

	public double SpaceAvailableOnDrive { get; set; }

	public string Language { get; set; }

	public bool IsModify
	{
		get
		{
			return _isModify;
		}
		set
		{
			_isModify = value;
		}
	}

	public bool IsDebugMode
	{
		get
		{
			return _debugMode;
		}
		set
		{
			_debugMode = value;
		}
	}

	public bool IsUninstall { get; set; }

	public bool ShowLangPage => _showLangPage;

	public bool IsUpgrade { get; set; }

	public static InstallerManager Instance => _instance;

	public PlatformType Platform { get; set; }

	public string ShortcutPath
	{
		get
		{
			if (InstallerConfig == null || InstallerConfig.InstallerInfo == null)
			{
				return null;
			}
			return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonDesktopDirectory), string.Format(CultureInfo.InvariantCulture, "Setup of {0}.lnk", InstallerConfig.InstallerInfo.DisplayName));
		}
	}

	public string EntryAssemblyName
	{
		get
		{
			return _entryAssemblyName.Value;
		}
		set
		{
			_entryAssemblyName = new Lazy<string>(() => value);
		}
	}

	private string ScriptPath => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonStartup), Path.GetFileNameWithoutExtension(ShortcutPath)) + ".vbs";

	private void _modelRepository_InterfaceResolve(object sender, TypeEventArgs e)
	{
		if (CertificateModelContainer.ImplementedInterfaces.Contains(e.InterfaceType))
		{
			CertificateModelContainer certificateModelContainer = new CertificateModelContainer();
			certificateModelContainer.Load();
			ModelRepository.RegisterModel(certificateModelContainer);
		}
	}

	private InstallerManager()
	{
		Parameters = new Dictionary<string, string>();
		QuietOverWriteParameters = new Dictionary<string, string>();
		PackagesParameters = new Dictionary<string, List<IPackageParameter>>();
		CurrentDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		Platform = ProcessorArchitectureHelper.GetPlatformType();
		ClMsiHandler = new ClMsiHandler();
		CultureManager.Instance.CultureChanged += Instance_CultureChanged;
	}

	public bool PrepareInstallation(bool isModify, bool isUninstall, string language, int acceptStatistics, string partnerId, bool enableMsiLog, bool debugMode, string idmId, bool doContinue, bool showLangPage, string exeLocation, string manufacturerInfoPath, string parentProcessName)
	{
		_isModify = isModify;
		_exeLocation = exeLocation;
		_parentProcessName = parentProcessName;
		IsUninstall = isUninstall;
		IsUpgrade = false;
		IsDebugMode = debugMode;
		_acceptStatistics = acceptStatistics;
		_partnerId = partnerId;
		_idmId = idmId;
		if (doContinue)
		{
			_continueDir = CurrentDirectory;
			if (!GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
			{
				try
				{
					ModelRepository.ResumeFromReboot();
				}
				catch (InvalidOperationException)
				{
					return false;
				}
			}
		}
		_showLangPage = showLangPage;
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Platform: {0}", Platform.ToString()));
		if (!LoadInstallerConfig(enableMsiLog) || !LoadManufacturerInfo(manufacturerInfoPath, doContinue))
		{
			if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
			{
				DeleteStartupScript();
			}
			return false;
		}
		SetGlobalParameters();
		FormatAndVerifyLanguage(ref language);
		Language = language;
		CheckInstallationsStatus();
		LogManager.Write(_dashes);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0}   {1}   {2}", _dashes, "Loading XML file...", _dashes));
		try
		{
			InstallInfo = LoadInstallationInfoXml(CurrentDirectory, IsUninstall || IsModify);
		}
		catch (Exception ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), ex2.StackTrace, TraceEventType.Critical);
			if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
			{
				DeleteStartupScript();
			}
			return false;
		}
		if (InstallInfo == null)
		{
			CreateInstanceInstallInfo();
		}
		if (InstallerConfig != null)
		{
			LoadConfiguration();
		}
		if (!string.IsNullOrEmpty(language))
		{
			Parameters["LANGUAGE"] = language;
			InstallInfo.InstallerLanguage = language;
		}
		else if (!string.IsNullOrEmpty(InstallInfo.InstallerLanguage))
		{
			Parameters["LANGUAGE"] = InstallInfo.InstallerLanguage;
		}
		if (partnerId != null)
		{
			Parameters["PARTNER_ID"] = partnerId;
		}
		if (idmId != null)
		{
			Parameters["IDM_ID"] = idmId;
		}
		IEnumerable<KeyValueParameters<string, string>> enumerable = InstallInfo.InstallEnvironment.Parameters.Where((KeyValueParameters<string, string> i) => i.Key == "PRODUCT-SKU");
		if (enumerable != null && enumerable.Count() == 1)
		{
			KeyValueParameters<string, string> keyValueParameters = enumerable.Single();
			_productSku = keyValueParameters.Value;
			Parameters[keyValueParameters.Key] = keyValueParameters.Value;
		}
		if (acceptStatistics != 0)
		{
			Parameters["ACCEPTSTATISTICS"] = acceptStatistics.ToString(CultureInfo.InvariantCulture);
		}
		if (Parameters.ContainsKey("PRODUCT-SKU"))
		{
			_productSku = Parameters["PRODUCT-SKU"];
		}
		if (Parameters.ContainsKey("PRODUCT-KEY"))
		{
			GlobalData.SetValue("PRODUCT-KEY", Parameters["PRODUCT-KEY"], "PrepareInstallation()");
		}
		LoadParametersFromPreviousInstallation();
		try
		{
			SetShouldLockManagementServer();
		}
		catch (FileNotFoundException)
		{
		}
		if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			DeleteStartupScript();
		}
		return true;
	}

	private void LoadParametersFromPreviousInstallation()
	{
		if (_isModify || IsUninstall)
		{
			UpdateParametersFromInstallInfoAndServerConfigurator();
		}
		else
		{
			FetchParametersIfUpgrade();
		}
	}

	private void SetShouldLockManagementServer()
	{
		ICertificateQueryModel model = ModelRepository.GetModel<ICertificateQueryModel>();
		IEnumerable<ClientEndpointConfiguration> clientEndpointsInstalled = model.ClientEndpointsInstalled;
		IEnumerable<ServerEndpointConfiguration> serverEndpointsInstalled = model.ServerEndpointsInstalled;
		if (!clientEndpointsInstalled.Any() && !serverEndpointsInstalled.Any())
		{
			return;
		}
		var (uri, uri2, uri3, uri4) = GetPreviousUsedServers();
		if (uri != null && uri2 != null && uri3 != null && uri4 != null)
		{
			if (GlobalData.GetValueAsBoolean("QUIET-MODE") && GlobalData.TryGetValue("MANAGEMENTSERVERURI", out var value) && !string.IsNullOrEmpty(value) && new Uri(value).Scheme != uri.Scheme)
			{
				throw new SecuritySchemeChangeDisallowedException("It is not allowed to change security scheme for the Management Server during upgrade. Change it in the Server Configurator before or after upgrading.");
			}
			SetParameter("MANAGEMENTSERVERURI", uri.ToString());
			SetParameter("MANAGEMENTAPPNAMESECURE", uri2.ToString());
			SetParameter("MANAGEMENTAPPNAME", uri3.ToString());
			SetParameter("IDPADDRESS", uri4.ToString());
			SetParameter("LOCKMANAGEMENTSERVER", "true");
		}
	}

	private void CreateStartupScript(string executable, string arguments)
	{
		try
		{
			if (!File.Exists(ScriptPath))
			{
				using (StreamWriter streamWriter = File.CreateText(ScriptPath))
				{
					streamWriter.WriteLine("Dim installerPath");
					streamWriter.WriteLine("Dim parameter");
					streamWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "installerPath = \"{0}\"", executable));
					streamWriter.WriteLine(string.Format(CultureInfo.InvariantCulture, "parameter  = \"{0}\"", arguments));
					streamWriter.WriteLine("Set WshShell = WScript.CreateObject(\"WScript.Shell\")");
					streamWriter.WriteLine("WshShell.Run chr(34) & installerPath & chr(34) & parameter");
					return;
				}
			}
		}
		catch (ArgumentException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
			throw new ShortcutException("There has been a problem creating the startup shortcut", ex);
		}
		catch (DirectoryNotFoundException ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), ex2.StackTrace, TraceEventType.Error);
			throw new ShortcutException("There has been a problem creating the startup shortcut", ex2);
		}
		catch (IOException ex3)
		{
			LogManager.Write(ex3.TypeAndMessage(), ex3.StackTrace, TraceEventType.Error);
			throw new ShortcutException("There has been a problem creating the startup shortcut", ex3);
		}
		catch (NotSupportedException ex4)
		{
			LogManager.Write(ex4.TypeAndMessage(), ex4.StackTrace, TraceEventType.Error);
			throw new ShortcutException("There has been a problem creating the startup shortcut", ex4);
		}
		catch (UnauthorizedAccessException ex5)
		{
			LogManager.Write(ex5.TypeAndMessage(), ex5.StackTrace, TraceEventType.Error);
			throw new ShortcutException("There has been a problem creating the startup shortcut", ex5);
		}
	}

	private void DeleteStartupScript()
	{
		try
		{
			File.Delete(ScriptPath);
		}
		catch (FileNotFoundException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
		catch (ArgumentException ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), ex2.StackTrace, TraceEventType.Error);
		}
		catch (DirectoryNotFoundException ex3)
		{
			LogManager.Write(ex3.TypeAndMessage(), ex3.StackTrace, TraceEventType.Error);
		}
		catch (IOException ex4)
		{
			LogManager.Write(ex4.TypeAndMessage(), ex4.StackTrace, TraceEventType.Error);
		}
		catch (NotSupportedException ex5)
		{
			LogManager.Write(ex5.TypeAndMessage(), ex5.StackTrace, TraceEventType.Error);
		}
		catch (UnauthorizedAccessException ex6)
		{
			LogManager.Write(ex6.TypeAndMessage(), ex6.StackTrace, TraceEventType.Error);
		}
	}

	private void SetGlobalParameters()
	{
		GlobalData.InstallationRegistryPath.SetInstallationRegirstryPath(InstallerConfig.InstallerInfo.ID);
		GlobalData.SetValue("VERSION_MAJOR", string.IsNullOrEmpty(InstallerConfig.InstallerInfo.Version.Major) ? "0" : InstallerConfig.InstallerInfo.Version.Major, "SetGlobalParameters()");
		GlobalData.SetValue("VERSION_MINOR", string.IsNullOrEmpty(InstallerConfig.InstallerInfo.Version.Minor) ? "0" : InstallerConfig.InstallerInfo.Version.Minor, "SetGlobalParameters()");
		GlobalData.SetValue("VERSION_REVISION", string.IsNullOrEmpty(InstallerConfig.InstallerInfo.Version.Revision) ? "0" : InstallerConfig.InstallerInfo.Version.Revision, "SetGlobalParameters()");
	}

	private void FetchParametersIfUpgrade()
	{
		string msiFileName = null;
		string mainPackageName = null;
		FindMainPackageInfo(InstallerConfig.InstallerPackages, out msiFileName, out mainPackageName);
		msiFileName = Path.Combine(CurrentDirectory, msiFileName);
		if (!File.Exists(msiFileName))
		{
			LogManager.Write("FetchParametersIfUpgrade(): Msi of the main package does not exist: " + msiFileName);
			return;
		}
		StringBuilder pProductId = new StringBuilder(1024);
		StringBuilder stringBuilder = new StringBuilder(1024);
		ClMsiHandler.FindProductAndUpgradeCode(msiFileName, pProductId, 1024, stringBuilder, 1024);
		StringBuilder stringBuilder2 = ClMsiHandler.EnumRelatedProducts(stringBuilder, 0u);
		if (stringBuilder2 == null || string.IsNullOrEmpty(stringBuilder2.ToString()))
		{
			return;
		}
		string text = ClMsiHandler.GetProductInfo(stringBuilder2.ToString(), "InstallSource");
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		if (File.Exists(Path.Combine(text, "InstallerConfig.xml")))
		{
			GetUpgradeParameters(text, mainPackageName, "InstallerConfig.xml");
			return;
		}
		LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Attmept to find '{0}' was unsuccessfull", text));
		string name = Path.Combine(Path.Combine(GlobalData.PublisherKey, "Installation"), InstallerConfig.InstallerInfo.ID);
		using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(name);
		if (registryKey != null)
		{
			object value = registryKey.GetValue("InstallLocation");
			if (value == null)
			{
				LogManager.Write($"InstallLocation is not populated for the '{InstallerConfig.InstallerInfo.ID}'. Skip GetUpgradeParameters.", TraceEventType.Warning);
				return;
			}
			text = value.ToString();
			if (!string.IsNullOrEmpty(text))
			{
				text = Path.Combine(text, string.Format(VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, InstallerConfig.InstallerInfo.ID));
			}
		}
		if (File.Exists(Path.Combine(text, "InstallerConfig.xml")))
		{
			GetUpgradeParameters(text, mainPackageName, "InstallerConfig.xml");
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Attmept to find '{0}' was unsuccessfull", text));
		}
	}

	private void GetUpgradeParameters(string installLocation, string mainPackageName, string confFile)
	{
		LogManager.Write(string.Format(CultureInfo.CurrentCulture, "Previous InstallConfig.xml was found '{0}'", installLocation));
		try
		{
			InstallerConfig installerConfig = InstallerConfigManager.LoadInstallerConfigFromFile(confFile, installLocation, is64bitOS: true);
			if (installerConfig == null)
			{
				return;
			}
			string msiFileName = null;
			string mainPackageName2 = null;
			FindMainPackageInfo(installerConfig.InstallerPackages, out msiFileName, out mainPackageName2);
			if (string.Compare(mainPackageName2, mainPackageName, StringComparison.InvariantCultureIgnoreCase) != 0)
			{
				return;
			}
			string text = Path.Combine(installLocation, "InstallInfo.xml");
			if (!File.Exists(text))
			{
				return;
			}
			InstallInfo installInfo = LoadInstallClearInfoXml(text);
			GlobalData.SetValue("IS_UPGRADE", "true", "FetchParametersIfUpgrade()");
			Parameters["IS_UPGRADE"] = "true";
			if (installInfo.InstallEnvironment.Parameters.Any((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == "QUIET-MODE".ToUpperInvariant()))
			{
				int index = installInfo.InstallEnvironment.Parameters.FindIndex((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == "QUIET-MODE".ToUpperInvariant());
				installInfo.InstallEnvironment.Parameters.RemoveAt(index);
			}
			UpdateInstallInfoParameters(installInfo.InstallEnvironment.Parameters);
			Dictionary<string, List<IPackageParameter>> dictionary = new Dictionary<string, List<IPackageParameter>>();
			CreatePackageParameters(dictionary, installerConfig);
			_oldInstallation = new InstallationParams(installLocation, installerConfig, installInfo, dictionary);
		}
		catch (Exception ex)
		{
			LogManager.Write(FormattableString.Invariant($"FetchParametersIfUpgrade(): Prev installed parameters were not loaded because of corrupted .xml or it is pre Installer Framework version.{Environment.NewLine}{ex}"));
		}
	}

	private InstallInfo LoadInstallClearInfoXml(string infoFile)
	{
		InstallInfo installInfo = InstallerConfigManager.LoadInstallInfo(infoFile);
		if (installInfo != null && !(GlobalData.ToDict()["Installation Context"] == "uninstall"))
		{
			AlignPasswords(installInfo);
		}
		return installInfo;
	}

	private void AlignPasswords(InstallInfo info)
	{
		List<string> obj = new List<string> { "ENCRYPTEDPASSWORD", "RECPASSWORD" };
		string parameterValue = string.Empty;
		info.InstallEnvironment.TryGetParameterValue("INTERNALPASSWORD", ref parameterValue);
		string parameterValue2 = string.Empty;
		info.InstallEnvironment.TryGetParameterValue("INTERNALUNICODE", ref parameterValue2);
		if (!string.IsNullOrEmpty(parameterValue2))
		{
			info.InstallEnvironment.Parameters.Remove(info.InstallEnvironment.Parameters.FirstOrDefault((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == "INTERNALUNICODE"));
		}
		if (!string.IsNullOrEmpty(parameterValue))
		{
			string value = TryDecrypt("INTERNALPASSWORD", parameterValue);
			if (!string.IsNullOrEmpty(value))
			{
				UpdateInstallInfoParameter(info, new KeyValueParameters<string, string>
				{
					Key = "INTERNALPASSWORD",
					Value = value
				});
			}
			else
			{
				info.InstallEnvironment.Parameters.Remove(info.InstallEnvironment.Parameters.FirstOrDefault((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == "INTERNALPASSWORD"));
			}
		}
		foreach (string key in obj)
		{
			string parameterValue3 = string.Empty;
			try
			{
				if (info.InstallEnvironment.TryGetParameterValue(key, ref parameterValue3) && string.IsNullOrEmpty(TryDecrypt(key, parameterValue3)))
				{
					LogManager.Write($"{key} Removed. Value could not be decrypted", TraceEventType.Warning);
					info.InstallEnvironment.Parameters.Remove(info.InstallEnvironment.Parameters.FirstOrDefault((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == key.ToUpperInvariant()));
				}
			}
			catch (Exception)
			{
				LogManager.Write($"failed to update {key}", TraceEventType.Error);
			}
		}
	}

	private static string TryDecrypt(string key, string encryptedValue)
	{
		try
		{
			return EncryptHelper.DecryptPassword(encryptedValue, unicodePassword: true);
		}
		catch (Exception)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "failed to update {0}", key), TraceEventType.Error);
		}
		return string.Empty;
	}

	private void FindMainPackageInfo(InstallerPackageType[] packages, out string msiFileName, out string mainPackageName)
	{
		msiFileName = null;
		mainPackageName = null;
		foreach (InstallerPackageType installerPackageType in packages)
		{
			if (installerPackageType.ProductManifest.MainPackage)
			{
				msiFileName = installerPackageType.ProductManifest.PackageData.PrimaryFile[0];
				mainPackageName = installerPackageType.Name;
			}
		}
		if (string.IsNullOrEmpty(msiFileName))
		{
			msiFileName = packages[0].ProductManifest.PackageData.PrimaryFile[0];
			mainPackageName = packages[0].Name;
		}
	}

	public void FormatAndVerifyLanguage(ref string language)
	{
		string text = language;
		if (!string.IsNullOrEmpty(language) && language.IndexOf('-') != -1)
		{
			string[] array = language.Split('-');
			text = $"{array[0].ToLower()}-";
			text = $"{text}{array[1].ToUpper()}";
			if (!InstallerConfig.InstallerLanguages.Contains(text))
			{
				text = InstallerConfig.InstallerInfo.PrimaryLanguage;
			}
		}
		language = text;
	}

	public bool TargetPlatformCheck()
	{
		if (InstallerConfig.InstallerInfo.TargetPlatform != PlatformType.ANY.ToString())
		{
			if (InstallerConfig.InstallerInfo.TargetPlatform.ToUpper() == PlatformType.X64.ToString() && Platform != PlatformType.X64)
			{
				return false;
			}
			if (InstallerConfig.InstallerInfo.TargetPlatform.ToUpper() == PlatformType.X86.ToString() && Platform != PlatformType.X86 && Platform != PlatformType.X64)
			{
				return false;
			}
		}
		return true;
	}

	private string AddPrefix(string id)
	{
		return "{" + id + "}";
	}

	private bool CheckInstallationsStatus()
	{
		bool result = false;
		string name = Path.Combine(PublisherKey, string.Format(VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, InstallerConfig.InstallerInfo.ID));
		RegistryKey registryKey = null;
		try
		{
			registryKey = Registry.LocalMachine.OpenSubKey(name);
		}
		catch (SecurityException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
		if (registryKey != null)
		{
			object value = registryKey.GetValue("InstallLocation");
			if (value != null)
			{
				Parameters["TARGETDIR"] = value.ToString();
				result = true;
			}
		}
		return result;
	}

	private bool CheckPreviousMilestoneProductInstall(out RegistryKey subkey)
	{
		bool result = false;
		string name = Path.Combine(PublisherKey, string.Format(CultureInfo.InvariantCulture, VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, string.Empty));
		RegistryKey registryKey = null;
		RegistryKey registryKey2 = null;
		subkey = null;
		try
		{
			using (registryKey = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry32))
			{
				using (registryKey2 = RegistryKey.OpenBaseKey(RegistryHive.LocalMachine, RegistryView.Registry64))
				{
					subkey = registryKey.OpenSubKey(name);
					if (subkey == null)
					{
						subkey = registryKey2.OpenSubKey(name);
					}
					if (subkey != null && subkey.GetSubKeyNames().Any((string n) => n != "ManufacturerInfo"))
					{
						result = true;
					}
				}
			}
		}
		catch (UnauthorizedAccessException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Error);
		}
		catch (SecurityException ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), ex2.StackTrace, TraceEventType.Error);
		}
		return result;
	}

	private bool CheckPreviousMilestoneProductInstall()
	{
		RegistryKey subkey = null;
		try
		{
			return CheckPreviousMilestoneProductInstall(out subkey);
		}
		finally
		{
			subkey?.Dispose();
		}
	}

	private static bool DoConsiderOtherProductInstallPath(InstallerConfig installerConfig)
	{
		if (installerConfig.InstallerSettings.UseSharedInstallFolder)
		{
			return true;
		}
		string iD = installerConfig.InstallerInfo.ID;
		if (!string.IsNullOrEmpty(iD) && ProductInstallerIds.ProductsWithSharedInstallFolder().Contains(iD.ToUpperInvariant()))
		{
			return true;
		}
		return false;
	}

	public bool TryGetPreviousMilestoneProductInstallPath(out string installationPath)
	{
		bool result = false;
		RegistryKey subkey = null;
		installationPath = null;
		if (!DoConsiderOtherProductInstallPath(InstallerConfig))
		{
			return false;
		}
		if (CheckPreviousMilestoneProductInstall(out subkey))
		{
			List<string> list = new List<string>();
			foreach (string item in from n in subkey.GetSubKeyNames()
				where n != "ManufacturerInfo"
				select n)
			{
				using RegistryKey registryKey = subkey.OpenSubKey(item);
				if (registryKey == null)
				{
					continue;
				}
				object value = registryKey.GetValue("InstallLocation");
				if (value != null)
				{
					string text = value.ToString();
					if (!string.IsNullOrEmpty(text) && Directory.Exists(text))
					{
						list.Add(text);
					}
				}
			}
			foreach (string item2 in list.OrderByDescending((string e) => Directory.GetCreationTime(e)))
			{
				if (File.Exists(Path.Combine(item2, "InstallerConfig.xml")) && !DoConsiderOtherProductInstallPath(InstallerConfigManager.LoadInstallerConfigFromFile("InstallerConfig.xml", item2, Platform == PlatformType.X64)))
				{
					continue;
				}
				string text2 = Path.Combine(item2, "InstallInfo.xml");
				if (!File.Exists(text2))
				{
					continue;
				}
				InstallInfo installInfo = InstallerConfigManager.LoadInstallInfo(text2);
				int num = installInfo.InstallEnvironment.Parameters.FindIndex((KeyValueParameters<string, string> x) => x.Key.ToUpperInvariant() == "TARGETDIR".ToUpperInvariant());
				if (num != -1)
				{
					string value2 = installInfo.InstallEnvironment.Parameters[num].Value;
					if (!string.IsNullOrEmpty(value2))
					{
						installationPath = value2;
						result = true;
						break;
					}
				}
			}
		}
		subkey?.Dispose();
		return result;
	}

	private bool LoadInstallerConfig(bool enableMsiLog)
	{
		try
		{
			InstallerConfig = LoadInstallerConfigurationXml();
			if (InstallerConfig != null && enableMsiLog)
			{
				InstallerConfig.InstallerSettings.EnableExtendedMSILogging = true;
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage(), ex.StackTrace, TraceEventType.Critical);
			return false;
		}
		return true;
	}

	private bool LoadManufacturerInfo(string manufacturerInfoPath, bool doContinue)
	{
		try
		{
			if ((GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") || GlobalData.GetValueAsBoolean("FINALIZE-MODE")) && InstallerConfig.InstallerInfo.ID.ToUpperInvariant() != "B8D7003E-3AD5-433C-8CE2-3077D50D51C9")
			{
				LogManager.Write("This installer cannot run in 'Hardware Manufacturer Mode' or in 'Finalize Mode'", TraceEventType.Critical);
				throw new ManufacturerInfoException("This installer cannot run in 'Hardware Manufacturer Mode' or in 'Finalize Mode'");
			}
			if (!doContinue && GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") && CheckPreviousMilestoneProductInstall())
			{
				LogManager.Write("A previous installation has been detected. Hardware manufacturer mode must be run on a fresh image.", TraceEventType.Critical);
				throw new ManufacturerInfoException("A previous installation has been detected. Hardware manufacturer mode must be run on a fresh image.");
			}
			ManufacturerInfo = LoadManufacturerInfoXml(manufacturerInfoPath, ManufacturerInfoReg);
			if (manufacturerInfoPath != null)
			{
				ManufacturerInfo.SaveToRegistry(ManufacturerInfoReg);
			}
		}
		catch (ManufacturerInfoException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Critical);
			Environment.ExitCode = 1;
			MessageBox.Show(ex.DisplayMessage, "Error", MessageBoxButtons.OK, MessageBoxIcon.Hand, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
			return false;
		}
		return true;
	}

	private void LoadConfiguration()
	{
		if (InstallerConfig.InstallerLanguages != null)
		{
			InstallerLanguages = InstallerConfig.InstallerLanguages;
		}
		LoadParameters();
		LoadPlugins();
		if (!GlobalData.GetValueAsBoolean("QUIET-MODE") || !GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			LoadLicense();
		}
		LoadInstallationTaskPlugins();
	}

	private InstallInfo LoadInstallationInfoXml(string currrentDir, bool isUninstall)
	{
		InstallInfo installInfo = null;
		if (isUninstall)
		{
			using (new ShowWaitCursor())
			{
				string text = Path.Combine(currrentDir, "InstallInfo.xml");
				LogManager.Write("Loading XML file... " + text, TraceEventType.Information);
				if (File.Exists(text))
				{
					installInfo = LoadInstallClearInfoXml(text);
					LogManager.Write(installInfo.InstalledPackages.Count.ToString(CultureInfo.CurrentUICulture));
				}
				else
				{
					LogManager.Write("File not found {0}.");
					LogManager.Write(text);
				}
			}
		}
		return installInfo;
	}

	private InstallerConfig LoadInstallerConfigurationXml()
	{
		using (new ShowWaitCursor())
		{
			try
			{
				LogManager.Write("Loading XML file... InstallerConfig.xml", TraceEventType.Information);
				return InstallerConfigManager.LoadInstallerConfig("InstallerConfig.xml", Platform == PlatformType.X64);
			}
			catch (Exception ex)
			{
				LogManager.Write(ex.TypeAndMessage(), TraceEventType.Critical);
				throw;
			}
		}
	}

	private static ManufacturerInfo LoadManufacturerInfoXml(string manufacturerInfoPath, string key)
	{
		using (new ShowWaitCursor())
		{
			if (!string.IsNullOrEmpty(manufacturerInfoPath))
			{
				LogManager.Write("Loading XML file... " + Path.GetFileName(manufacturerInfoPath), TraceEventType.Information);
				return ManufacturerInfo.LoadFromFile(manufacturerInfoPath);
			}
			LogManager.Write("Loading from registry... " + key, TraceEventType.Information);
			ManufacturerInfo manufacturerInfo = ManufacturerInfo.LoadFromRegistry(key);
			if (manufacturerInfo == null)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Manufacturer info not defined in registry."), TraceEventType.Information);
			}
			return manufacturerInfo;
		}
	}

	private void CreateInstanceInstallInfo()
	{
		InstallInfo = new InstallInfo();
		InstallInfo.InstallEnvironment = new VideoOS.Installer.Configuration.InstallEnvironment();
		InstallInfo.InstalledPackages = new List<PackageRefType>();
		InstallInfo.NotInstalledPackages = new List<PackageRefType>();
		InstallInfo.NotUninstalledPackages = new List<PackageRefType>();
		InstallInfo.InstallEnvironment.Parameters = new List<KeyValueParameters<string, string>>();
	}

	public void CreateSequenceManager()
	{
		int websitePort = 80;
		if (Parameters.ContainsKey("WebSitePort"))
		{
			websitePort = Convert.ToInt32(Parameters["WebSitePort"]);
		}
		SequenceManagerWrapper = new SequenceManagerWrapper(ClMsiHandler, new InstallationParams(CurrentDirectory, InstallerConfig, InstallInfo, PackagesParameters), _oldInstallation, (ILogger)(object)new InstallerLog(), Platform, websitePort, IsDebugMode);
	}

	public void CreatePrerequisiteManager()
	{
		PrerequisiteManager = new PrerequisiteManager(ClMsiHandler, InstallerConfig, PackagesParameters, CurrentDirectory);
	}

	public void UpdatePackageParameters()
	{
		CreatePackageParameters(PackagesParameters, InstallerConfig);
		foreach (KeyValuePair<string, string> parameter in Parameters)
		{
			KeyValueParameters<string, string> keyValueParameters = new KeyValueParameters<string, string>();
			keyValueParameters.Key = parameter.Key;
			keyValueParameters.Value = parameter.Value;
			if (!UpdateInstallInfoParameter(InstallInfo, keyValueParameters))
			{
				InstallInfo.InstallEnvironment.Parameters.Add(keyValueParameters);
			}
		}
		if (PrerequisiteManager == null)
		{
			CreatePrerequisiteManager();
		}
		else
		{
			PrerequisiteManager.PackageParameters = PackagesParameters;
		}
		if (SequenceManagerWrapper != null)
		{
			SequenceManager.PackageParameters = PackagesParameters;
		}
	}

	private void CreatePackageParameters(Dictionary<string, List<IPackageParameter>> packagesParams, InstallerConfig installerConfig)
	{
		Dictionary<string, bool> dictionary = new Dictionary<string, bool>();
		PackageRefType[] installExecuteSequence = installerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			dictionary[packageRefType.ID] = packageRefType.IncludeInARP;
		}
		InstallerPackageType[] installerPackages = installerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (dictionary.ContainsKey(installerPackageType.ID))
			{
				packagesParams[installerPackageType.ID] = CollectParametersAndValues(installerPackageType, dictionary[installerPackageType.ID], installerConfig);
			}
		}
	}

	private bool UpdateInstallInfoParameter(InstallInfo info, KeyValueParameters<string, string> tempKeyValue)
	{
		if (info != null && info.InstallEnvironment != null && info.InstallEnvironment.Parameters != null)
		{
			foreach (KeyValueParameters<string, string> parameter in info.InstallEnvironment.Parameters)
			{
				if (parameter != null && parameter.Key == tempKeyValue.Key)
				{
					parameter.Value = tempKeyValue.Value;
					return true;
				}
			}
		}
		return false;
	}

	public void LoadParameters()
	{
		string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
		Parameters["TARGETDIR"] = Path.Combine(folderPath, InstallerConfig.InstallerInfo.Publisher);
		InstallerPluginType[] installerPlugins = InstallerConfig.InstallerPlugins;
		foreach (InstallerPluginType installerPluginType in installerPlugins)
		{
			if (installerPluginType == null || installerPluginType.Parameters == null)
			{
				continue;
			}
			ParameterType[] parameters = installerPluginType.Parameters;
			foreach (ParameterType parameterType in parameters)
			{
				string text = parameterType.DefaultValue;
				if (!string.IsNullOrEmpty(text))
				{
					if (Platform == PlatformType.X86 && text.Contains(" (x86)"))
					{
						text = (parameterType.DefaultValue = parameterType.DefaultValue.Replace(" (x86)", ""));
					}
					if (Platform == PlatformType.X86 && text.Contains("(x86)"))
					{
						text = (parameterType.DefaultValue = parameterType.DefaultValue.Replace("(x86)", ""));
					}
					text = ExpandEvironmentVariables(text);
				}
				Parameters[parameterType.Name] = text ?? string.Empty;
			}
		}
		Parameters["ACCEPTSTATISTICS"] = ((_acceptStatistics == 0) ? "no" : "yes");
		if (_partnerId != null)
		{
			Parameters["PARTNER_ID"] = _partnerId;
		}
		if (_idmId != null)
		{
			Parameters["IDM_ID"] = _idmId;
		}
		if (Language != null)
		{
			Parameters["LANGUAGE"] = Language;
		}
	}

	private string ExpandEvironmentVariables(string value)
	{
		if (!string.IsNullOrEmpty(value))
		{
			if (value.Contains("[LocalMachine]"))
			{
				value = value.Replace("[LocalMachine]", Environment.MachineName);
			}
			if (value.Contains("[TARGETDIR]"))
			{
				value = value.Replace("[TARGETDIR]", Parameters["TARGETDIR"]);
			}
			return Environment.ExpandEnvironmentVariables(Regex.Replace(value, "[$][(]([a-zA-Z0-9_]*)[)]", "%${1}%"));
		}
		return value;
	}

	private void LoadPlugins()
	{
		PluginLoader pluginLoader = new PluginLoader(InstallerConfig, new PluginTypeLoader());
		try
		{
			SubscriptionPlugins = pluginLoader.LoadSubscriptionPlugins();
			if (!IsModify)
			{
				SubscriptionPlugins.BypassFilter = new SubscriptionBypassFilter(InstallerConfig);
			}
			else
			{
				SubscriptionPlugins.BypassFilter = new SubscriptionBypassFilterInModify(InstallerConfig);
			}
			Plugins = pluginLoader.LoadPlugins();
			PageActionPlugins = pluginLoader.LoadCustomPageSetup();
		}
		catch (TargetInvocationException ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Critical);
		}
	}

	private void LoadLicense()
	{
		if (InstallerConfig.LicenseProvider == null)
		{
			return;
		}
		LicenseProviderType licenseProvider = InstallerConfig.LicenseProvider;
		try
		{
			Assembly assembly = null;
			assembly = Assembly.LoadFrom(Path.Combine(CurrentDirectory, licenseProvider.Filename));
			if (!(assembly != null))
			{
				return;
			}
			try
			{
				assembly.GetTypes();
			}
			catch (ReflectionTypeLoadException)
			{
			}
			Type[] types = assembly.GetTypes();
			foreach (Type type in types)
			{
				if (typeof(ILicenseProvider).IsAssignableFrom(type) && !type.IsAbstract && assembly.CreateInstance(type.ToString()) is ILicenseProvider licenseProvider2)
				{
					LicenseProvider = licenseProvider2;
					LogManager.Write("License loaded.", TraceEventType.Information);
				}
			}
		}
		catch (FileNotFoundException ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), TraceEventType.Critical);
			throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File not found {0}.", licenseProvider.Filename));
		}
		catch (TargetInvocationException ex3)
		{
			LogManager.Write(ex3.TypeAndMessage(), TraceEventType.Critical);
		}
	}

	public double GetFreeSpace(string targetDir)
	{
		if (targetDir == null)
		{
			targetDir = Parameters["TARGETDIR"];
		}
		if (!string.IsNullOrEmpty(targetDir))
		{
			LogManager.Write("Target path: " + targetDir);
			if (NativeMethods.GetDiskFreeSpaceEx(Path.GetPathRoot(targetDir), out var lpFreeBytesAvailable, out var _, out var _) == 0)
			{
				throw new Win32Exception((int)NativeMethods.GetLastError());
			}
			double value = (double)lpFreeBytesAvailable / Math.Pow(1024.0, 2.0);
			SpaceAvailableOnDrive = Math.Round(value, 1);
			return Math.Round(value, 1);
		}
		return 0.0;
	}

	public double GetRequiredSpace()
	{
		double num = 0.0;
		long num2 = 0L;
		long num3 = 0L;
		if (PrerequisiteManager.PackagesToInstall != null)
		{
			foreach (KeyValuePair<string, ProductInstallInfo> item in PrerequisiteManager.PackagesToInstall)
			{
				num2 += item.Value.EstimatedInstalledBytes;
			}
		}
		Dictionary<string, ConditionType> dictionary = new Dictionary<string, ConditionType>();
		PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			dictionary[packageRefType.ID] = packageRefType.Condition;
		}
		InstallerPackageType[] installerPackages = InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (dictionary.ContainsKey(installerPackageType.ID) && dictionary[installerPackageType.ID] != ConditionType.Skip && installerPackageType.ProductManifest.Format == FormatType.Milestone)
			{
				num3 += installerPackageType.ProductManifest.PackageData.ProductInfo.SpaceRequired;
			}
		}
		num = num2 / 1024 + num3;
		FileInfo fileInfo = new FileInfo("media.cab");
		if (fileInfo.Exists)
		{
			num += (double)(fileInfo.Length / 1024 + 10240);
		}
		num /= 1024.0;
		return Math.Round(num, 1);
	}

	public double GetRequiredSpaceByInstaller()
	{
		long num = 0L;
		long num2 = 0L;
		if (PrerequisiteManager.PackagesToInstall != null)
		{
			foreach (KeyValuePair<string, ProductInstallInfo> item in PrerequisiteManager.PackagesToInstall)
			{
				num += item.Value.EstimatedInstalledBytes;
			}
		}
		Dictionary<string, ConditionType> dictionary = new Dictionary<string, ConditionType>();
		PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			dictionary[packageRefType.ID] = packageRefType.Condition;
		}
		InstallerPackageType[] installerPackages = InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (dictionary.ContainsKey(installerPackageType.ID) && dictionary[installerPackageType.ID] != ConditionType.Skip && installerPackageType.ProductManifest.Format == FormatType.Milestone)
			{
				num2 += installerPackageType.ProductManifest.PackageData.ProductInfo.SpaceRequired;
			}
		}
		return Math.Round((double)(num / 1024 + num2) / 1024.0, 1);
	}

	public double IsEnoughOfSpaceOnDrive()
	{
		if (SpaceAvailableOnDrive >= GetRequiredSpace())
		{
			return 0.0;
		}
		return (GetRequiredSpace() - SpaceAvailableOnDrive) * 1024.0;
	}

	public List<IPackageParameter> CollectParametersAndValues(InstallerPackageType installerPackage, bool includeARP, InstallerConfig installerConfig)
	{
		string iD = installerPackage.ID;
		string text = null;
		List<IPackageParameter> list = new List<IPackageParameter>();
		if (Parameters != null)
		{
			bool flag = false;
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = false;
			InstallerPluginType[] installerPlugins = installerConfig.InstallerPlugins;
			foreach (InstallerPluginType installerPluginType in installerPlugins)
			{
				if (installerPluginType == null || installerPluginType.Parameters == null)
				{
					continue;
				}
				ParameterType[] parameters = installerPluginType.Parameters;
				foreach (ParameterType parameterType in parameters)
				{
					text = parameterType.Name;
					string text2 = ExpandEvironmentVariables(parameterType.DefaultValue);
					if (parameterType.TargetPackages == null)
					{
						continue;
					}
					PackageRefType[] targetPackages = parameterType.TargetPackages;
					foreach (PackageRefType packageRefType in targetPackages)
					{
						if (string.Compare(iD, packageRefType.ID) != 0)
						{
							continue;
						}
						text = UpdateParameterName(text, Parameters);
						if (text == null)
						{
							continue;
						}
						string text3 = text;
						string text4 = Parameters[text];
						if (packageRefType.TargetParameter != null)
						{
							text3 = packageRefType.TargetParameter;
						}
						if (packageRefType.DefaultTargetValue != null && (text4 == text2 || packageRefType.DefaultTargetValue.Contains(text)))
						{
							text4 = ExpandEvironmentVariables(packageRefType.DefaultTargetValue);
						}
						if (string.Compare(text, "TARGETDIR") == 0)
						{
							if (installerPackage.ProductManifest.PackageData == null)
							{
								continue;
							}
							if (Parameters.ContainsKey("TARGETDIR") && installerPackage.ProductManifest.PackageData.ProductInfo.InstallationFolder != null && !string.IsNullOrEmpty(text4))
							{
								if (text4.Contains("W6432") && Platform == PlatformType.X86)
								{
									text4 = ExpandEvironmentVariables(text4.Replace("W6432", "FILES"));
								}
								if (text4.Contains(" (x86)") && installerPackage.ProductManifest.PackageData.ProductInfo.TargetPlatform.ToUpper() == PlatformType.X64.ToString())
								{
									text4 = text4.Replace(" (x86)", "");
								}
								list.Add(new PackageParameter(text3, Path.Combine(text4, installerPackage.ProductManifest.PackageData.ProductInfo.InstallationFolder)));
							}
							else
							{
								string text5 = Path.Combine(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), installerConfig.InstallerInfo.Publisher), installerPackage.ProductManifest.PackageData.ProductInfo.DisplayName.Value);
								LogManager.Write($"Default target directory {text5} is used for {installerPackage.ProductManifest.PackageData.ProductInfo.DisplayName} package.");
								list.Add(new PackageParameter(text3, text5));
							}
						}
						else if (string.Compare(text, "LANGUAGE", StringComparison.InvariantCultureIgnoreCase) == 0)
						{
							if (Parameters.ContainsKey(text3) && Language == null && packageRefType.DefaultTargetValue != text4)
							{
								text4 = Parameters[text3];
							}
							list.Add(new PackageParameter(text3, text4));
							flag = true;
						}
						else if (string.Compare(text, "PARTNER_ID") == 0)
						{
							list.Add(new PackageParameter(text3, text4));
							flag2 = true;
						}
						else if (string.Compare(text, "IDM_ID") == 0)
						{
							list.Add(new PackageParameter(text3, text4));
							flag4 = true;
						}
						else if (string.Compare(text, "ACCEPTSTATISTICS") == 0)
						{
							list.Add(new PackageParameter(text3, text4));
							flag3 = true;
						}
						else if (text4 != null)
						{
							list.Add(new PackageParameter(text3, text4));
						}
					}
				}
			}
			if (!includeARP)
			{
				IPackageParameter item = new PackageParameter("ARPSYSTEMCOMPONENT", "1");
				list.Add(item);
			}
			if (!flag)
			{
				if (!Parameters.ContainsKey("LANGUAGE"))
				{
					Parameters.Add("LANGUAGE", installerConfig.InstallerInfo.PrimaryLanguage);
				}
				list.Add(new PackageParameter("LANGUAGE", Parameters["LANGUAGE"]));
			}
			if (!flag2 && Parameters.ContainsKey("PARTNER_ID"))
			{
				list.Add(new PackageParameter("PARTNER_ID", Parameters["PARTNER_ID"]));
			}
			if (!flag4 && Parameters.ContainsKey("IDM_ID"))
			{
				list.Add(new PackageParameter("IDM_ID", Parameters["IDM_ID"]));
			}
			if (!flag3)
			{
				list.Add(new PackageParameter("ACCEPTSTATISTICS", Parameters["ACCEPTSTATISTICS"]));
			}
			if (Parameters.ContainsKey("LICENSE"))
			{
				list.Add(new PackageParameter("LICENSE", Parameters["LICENSE"]));
			}
			return list;
		}
		return null;
	}

	private void LoadInstallationTaskPlugins()
	{
		List<IPackageParameter> list = new List<IPackageParameter>();
		foreach (KeyValuePair<string, string> parameter in Parameters)
		{
			list.Add(new PackageParameter(parameter.Key, parameter.Value));
		}
		InstallerPackageType[] installerPackages = InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			if (installerPackageType.InstallationTaskPlugin == null)
			{
				continue;
			}
			try
			{
				Assembly assembly = null;
				assembly = Assembly.LoadFrom(Path.Combine(CurrentDirectory, installerPackageType.InstallationTaskPlugin.Filename));
				if (!(assembly != null))
				{
					continue;
				}
				try
				{
					assembly.GetTypes();
				}
				catch (ReflectionTypeLoadException)
				{
				}
				Type[] types = assembly.GetTypes();
				foreach (Type type in types)
				{
					if (typeof(IInstallationAdapter).IsAssignableFrom(type) && !type.IsAbstract && assembly.CreateInstance(type.ToString()) is IInstallationAdapter installationAdapter)
					{
						installationAdapter.Initialize(list);
						if (installationAdapter is MSIUninstallerTask)
						{
							installerPackageType.InstallationTaskPlugin.UninstallationTask = installationAdapter;
						}
						else
						{
							installerPackageType.InstallationTaskPlugin.InstallationTask = installationAdapter;
						}
					}
				}
			}
			catch (FileNotFoundException ex2)
			{
				LogManager.Write(ex2.TypeAndMessage(), TraceEventType.Critical);
				throw new ArgumentException(string.Format(CultureInfo.InvariantCulture, "File not found {0}.", installerPackageType.InstallationTaskPlugin.Filename));
			}
			catch (TargetInvocationException ex3)
			{
				LogManager.Write(ex3.TypeAndMessage(), TraceEventType.Critical);
			}
		}
	}

	private void AddEntryToRegistry()
	{
		CreateUpdateSoftwareEntryAndDeleteOldFolders(Path.Combine(CurrentDirectory, EntryAssemblyName));
		CreatePublisherEntry();
	}

	public void CreatePublisherEntry()
	{
		RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(PublisherKey, writable: true);
		if (registryKey == null)
		{
			try
			{
				registryKey = Registry.LocalMachine.CreateSubKey(PublisherKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
		if (registryKey == null)
		{
			return;
		}
		try
		{
			string text = string.Format(VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, InstallerConfig.InstallerInfo.ID);
			registryKey.CreateSubKey(text, RegistryKeyPermissionCheck.ReadWriteSubTree);
			using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(Path.Combine(PublisherKey, text), writable: true);
			registryKey2.SetValue("InstallLocation", CurrentDirectory);
			ProductCompliance.SetProductComplianceVersion(registryKey2, ProductComplianceVersion.V1);
		}
		catch (SecurityException ex)
		{
			LogManager.Write($"Security error detected {ex.TypeAndMessage()}.", TraceEventType.Error);
		}
		catch (Exception exception2)
		{
			LogManager.Write(exception2);
		}
	}

	public void UpdateInstallInfoAfterFinalize()
	{
		foreach (PackageRefType installedPackage in InstallInfo.InstalledPackages)
		{
			PackageRefType packageRefType = _oldInstallation.InstallerConfiguration.InstallExecuteSequence.FirstOrDefault((PackageRefType x) => x.ID == installedPackage.ID);
			if (packageRefType != null)
			{
				installedPackage.Condition = packageRefType.Condition;
			}
		}
		SerializeObject(CurrentDirectory, InstallInfo);
	}

	public void UpdateInstallInfoAfterModify()
	{
		InstallInfo installInfo = LoadInstallationInfoXml(CurrentDirectory, isUninstall: true);
		if (installInfo != null)
		{
			List<PackageRefType> list = new List<PackageRefType>();
			List<PackageRefType> list2 = new List<PackageRefType>();
			List<string> installedPackages = SequenceManager.InstalledPackages;
			List<string> uninstalledPackages = SequenceManager.UninstalledPackages;
			VerifyCurrentMachineState(doContinue: false);
			foreach (PackageRefType installedPackage in installInfo.InstalledPackages)
			{
				if (uninstalledPackages.Contains(installedPackage.ID))
				{
					list2.Add(installedPackage);
				}
				else
				{
					list.Add(installedPackage);
				}
			}
			foreach (PackageRefType notInstalledPackage in installInfo.NotInstalledPackages)
			{
				if (installedPackages.Contains(notInstalledPackage.ID))
				{
					list.Add(notInstalledPackage);
				}
				else
				{
					list2.Add(notInstalledPackage);
				}
			}
			installInfo.NotInstalledPackages = list2;
			installInfo.InstalledPackages = list;
			SerializeObject(CurrentDirectory, installInfo);
		}
		else
		{
			SerializeObject(CurrentDirectory, InstallInfo);
		}
		ModelRepository.PersistModels();
	}

	public void CreateUpdateSoftwareEntryAndDeleteOldFolders(string executable)
	{
		using RegistryKey softwareKey = Registry.LocalMachine.OpenSubKey(_softwareKey, RegistryKeyPermissionCheck.ReadWriteSubTree);
		string text = AddPrefix(InstallerConfig.InstallerInfo.ID);
		try
		{
			CreateNewEntryOrDeleteOldFolders(executable, softwareKey, text);
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine(_softwareKey, text), RegistryKeyPermissionCheck.ReadWriteSubTree);
			ARPEntryType aRPEntry = InstallerConfig.UninstallerInfo.ARPEntry;
			registryKey.SetValue("DisplayName", VersionFormat.GetNeutralProductName());
			string text2 = ((InstallerConfig.UninstallerInfo.ARPEntry.DisplayVersion == null) ? VersionFormat.GetARPDisplayVersion() : VersionFormat.GetValidVersionString(InstallerConfig.UninstallerInfo.ARPEntry.DisplayVersion));
			registryKey.SetValue("DisplayVersion", text2);
			try
			{
				Version version = new Version(text2);
				registryKey.SetValue("Version", version.GetHashCode(), RegistryValueKind.DWord);
				registryKey.SetValue("VersionMajor", version.Major.GetHashCode(), RegistryValueKind.DWord);
				registryKey.SetValue("VersionMinor", version.Minor.GetHashCode(), RegistryValueKind.DWord);
				if (version.Revision != -1)
				{
					registryKey.SetValue("VersionRevision", version.Revision.GetHashCode(), RegistryValueKind.DWord);
				}
			}
			catch (Exception)
			{
				LogManager.Write("Attempt to parse version failed.", TraceEventType.Information);
			}
			registryKey.SetValue("InstallDate", DateTime.Now.GetDateTimeFormats('d')[0]);
			registryKey.SetValue("Size", (GetRequiredSpaceByInstaller() * 1024.0).ToString());
			registryKey.SetValue("EstimatedSize", GetRequiredSpaceByInstaller() * 1024.0, RegistryValueKind.DWord);
			registryKey.SetValue("InstallSource", executable);
			registryKey.SetValue("ModifyPath", executable + " --modify");
			registryKey.SetValue("UninstallString", executable + " --uninstall");
			registryKey.SetValue("NoModify", GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE") ? 1 : 0, RegistryValueKind.DWord);
			if (InstallerConfig.Graphics.Icon != null && InstallerConfig.Graphics.Icon.Filename != null)
			{
				registryKey.SetValue("DisplayIcon", Path.Combine(CurrentDirectory, InstallerConfig.Graphics.Icon.Filename));
			}
			if (aRPEntry.Contact != null)
			{
				registryKey.SetValue("Publisher", aRPEntry.Contact);
				registryKey.SetValue("Contact", aRPEntry.Contact);
			}
			if (!string.IsNullOrEmpty(InstallerConfig.InstallerInfo.HelpLink))
			{
				registryKey.SetValue("HelpLink", InstallerConfig.InstallerInfo.HelpLink, RegistryValueKind.ExpandString);
				registryKey.SetValue("URLUpdateInfo", InstallerConfig.InstallerInfo.HelpLink);
			}
		}
		catch (SecurityException ex2)
		{
			LogManager.Write($"Security error detected {ex2.TypeAndMessage()}.", TraceEventType.Error);
		}
	}

	public static void CreateNewEntryOrDeleteOldFolders(string executable, RegistryKey softwareKey, string guid)
	{
		if (string.IsNullOrEmpty(executable))
		{
			throw new ArgumentNullException("executable");
		}
		if (softwareKey == null)
		{
			throw new ArgumentNullException("softwareKey");
		}
		if (string.IsNullOrEmpty(guid))
		{
			throw new ArgumentNullException("guid");
		}
		using RegistryKey registryKey = softwareKey.OpenSubKey(guid);
		string empty = string.Empty;
		string directoryName = Path.GetDirectoryName(executable);
		if (registryKey != null && registryKey.GetValue("InstallSource") != null && !directoryName.Equals(Path.GetDirectoryName(registryKey.GetValue("InstallSource").ToString()), StringComparison.OrdinalIgnoreCase))
		{
			empty = Path.GetDirectoryName(registryKey.GetValue("InstallSource").ToString());
			DeleteFolderRecursivly(new DirectoryInfo(empty));
			LogManager.Write("Old files and folders were deleted from " + empty);
		}
		else
		{
			softwareKey.CreateSubKey(guid, RegistryKeyPermissionCheck.ReadWriteSubTree);
		}
	}

	private static void DeleteFolderRecursivly(DirectoryInfo pathToDelete)
	{
		try
		{
			RemoveReadOnlyAttributes(pathToDelete);
			pathToDelete.Delete(recursive: true);
		}
		catch (Exception arg)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to delete old folder: {0}, Error {1}", pathToDelete, arg), TraceEventType.Warning);
		}
	}

	private static void RemoveReadOnlyAttributes(DirectoryInfo pathToDelete)
	{
		pathToDelete.Attributes &= ~FileAttributes.ReadOnly;
		FileInfo[] files = pathToDelete.GetFiles();
		for (int i = 0; i < files.Length; i++)
		{
			files[i].Attributes &= ~FileAttributes.ReadOnly;
		}
		DirectoryInfo[] directories = pathToDelete.GetDirectories();
		for (int i = 0; i < directories.Length; i++)
		{
			RemoveReadOnlyAttributes(directories[i]);
		}
	}

	public void SaveInstallationInfo(bool doContinue)
	{
		ModelRepository.PersistModels();
		SavePackages(doContinue);
		FinishSaveInstallInfo(doContinue);
		string fullName = Directory.GetParent(LogManager.GetLogFilename()).Parent.FullName;
		RegistryKey registryKey = Registry.LocalMachine.CreateSubKey("SOFTWARE\\VideoOS");
		registryKey.SetValue("ProgramDataFolder", fullName);
		registryKey.Close();
		LogManager.Write("Registry key ProgramDataFolder is written. Value: " + fullName);
	}

	public void SavePackages(bool doContinue)
	{
		if (IsModify)
		{
			SaveInstalledPackages();
			SerializeObject(CurrentDirectory, InstallInfo);
			return;
		}
		List<string> installedPackages = SequenceManager.InstalledPackages;
		Collection<string> collection = VerifyCurrentMachineState(doContinue);
		CleanUpRegistry();
		if (installedPackages == null)
		{
			return;
		}
		PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType package in installExecuteSequence)
		{
			if (InstallInfo.InstalledPackages.Any((PackageRefType f) => f.ID == package.ID))
			{
				continue;
			}
			if (installedPackages.Contains(package.ID) || collection.Contains(package.ID))
			{
				InstallInfo.InstalledPackages.Add(package);
				InstallInfo.NotInstalledPackages.RemoveAll((PackageRefType f) => f.ID == package.ID);
			}
			else if (!InstallInfo.NotInstalledPackages.Any((PackageRefType f) => f.ID == package.ID))
			{
				InstallInfo.NotInstalledPackages.Add(package);
			}
		}
		DoSkipIsInstalledPackages(InstallInfo.NotInstalledPackages);
		if (InstallerConfig.LicenseProvider != null)
		{
			InstallInfo.InstallEnvironment.License = InstallerConfig.LicenseProvider.Type;
		}
		else
		{
			InstallInfo.InstallEnvironment.License = VideoOS.Installer.Configuration.LicenseType.None;
		}
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			UpdateContinueParameters(BreakAction.InstallContinue.ToString(), "LanguagePage");
		}
		InstallInfo.ProductLanguage = Parameters["LANGUAGE"];
		SerializeObject(CurrentDirectory, InstallInfo);
	}

	private void FinishSaveInstallInfo(bool doContinue)
	{
		AddEntryToRegistry();
		_doFormClosingActions = !doContinue && GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		LogManager.Write(_dashes);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0}   {1}    {2}", _dashes, "Parameters", _dashes));
		LogParameters();
	}

	public void CloseActions()
	{
		if (!_doFormClosingActions)
		{
			return;
		}
		try
		{
			string executable = Path.Combine(CurrentDirectory, EntryAssemblyName);
			string arguments = "--finalizeMode";
			CreateInstallerShortcut(executable, arguments);
			if (!GlobalData.HasParameter("PREPAREFINALIZE") || GlobalData.GetValueAsBoolean("PREPAREFINALIZE"))
			{
				CreateStartupScript(executable, arguments);
			}
			if (!GlobalData.HasParameter("REMOVEINSTALLLOG") || GlobalData.GetValueAsBoolean("REMOVEINSTALLLOG"))
			{
				LogManager.RemoveInstallLogs();
			}
		}
		catch (Exception)
		{
			_doFormClosingActions = false;
			throw;
		}
	}

	private void LogParameters()
	{
		foreach (KeyValuePair<string, string> parameter in Parameters)
		{
			if (!parameter.Key.ToLower(CultureInfo.CurrentCulture).Contains("password") && !parameter.Key.ToLower(CultureInfo.CurrentCulture).Contains("secret"))
			{
				LogManager.Write($"{parameter.Key}   {parameter.Value}", TraceEventType.Information);
			}
		}
	}

	public void SaveInstalledPackages()
	{
		List<string> installedPackages = SequenceManager.InstalledPackages;
		if (GlobalData.GetValueAsBoolean("FINALIZE-MODE"))
		{
			foreach (string package in installedPackages)
			{
				PackageRefType packageRefType = InstallerConfig.InstallExecuteSequence.FirstOrDefault((PackageRefType f) => f.ID.ToUpperInvariant() == package.ToUpperInvariant() && f.Condition == ConditionType.Finalize);
				if (packageRefType != null)
				{
					packageRefType.Condition = ConditionType.Skip;
				}
			}
		}
		List<string> notExecutedUninstallPackages = SequenceManager.NotExecutedUninstallPackages;
		List<string> notExecutedInstallPackages = SequenceManager.NotExecutedInstallPackages;
		List<string> list = new List<string>();
		foreach (PackageRefType installedPackage in InstallInfo.InstalledPackages)
		{
			list.Add(installedPackage.ID);
		}
		InstallInfo.NotInstalledPackages = new List<PackageRefType>();
		if (installedPackages != null)
		{
			PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
			foreach (PackageRefType package2 in installExecuteSequence)
			{
				if (installedPackages.Contains(package2.ID) && !notExecutedInstallPackages.Contains(package2.ID) && InstallInfo.InstalledPackages.Find((PackageRefType x) => x.ID.ToUpperInvariant() == package2.ID.ToUpperInvariant()) == null)
				{
					InstallInfo.InstalledPackages.Add(package2);
				}
				else if (!list.Contains(package2.ID) || notExecutedInstallPackages.Contains(package2.ID))
				{
					InstallInfo.NotInstalledPackages.Add(package2);
				}
				if (notExecutedUninstallPackages.Contains(package2.ID))
				{
					InstallInfo.NotUninstalledPackages.Add(package2);
				}
			}
			if (InstallerConfig.LicenseProvider != null)
			{
				InstallInfo.InstallEnvironment.License = InstallerConfig.LicenseProvider.Type;
			}
			else
			{
				InstallInfo.InstallEnvironment.License = VideoOS.Installer.Configuration.LicenseType.None;
			}
		}
		DoSkipIsInstalledPackages(InstallInfo.NotInstalledPackages);
	}

	private void DoSkipIsInstalledPackages(IEnumerable<PackageRefType> packages)
	{
		foreach (PackageRefType package in packages)
		{
			InstallerPackageTypeInstallationTaskPlugin installerPackageTypeInstallationTaskPlugin = Array.Find(InstallerConfig.InstallerPackages, (InstallerPackageType x) => x.ID.ToUpperInvariant() == package.ID.ToUpperInvariant())?.InstallationTaskPlugin;
			if (installerPackageTypeInstallationTaskPlugin != null && installerPackageTypeInstallationTaskPlugin.IsInstalled && package.Condition == ConditionType.Required)
			{
				package.Condition = ConditionType.Skip;
			}
		}
	}

	private void SerializeObject(string filePath, InstallInfo installInfo)
	{
		string text = Path.Combine(filePath, "InstallInfo.xml");
		string parameterValue = string.Empty;
		if (installInfo.InstallEnvironment.TryGetParameterValue("INTERNALPASSWORD", ref parameterValue))
		{
			UpdateInstallInfoParameter(installInfo, new KeyValueParameters<string, string>
			{
				Key = "INTERNALPASSWORD",
				Value = EncryptHelper.EncryptPassword(parameterValue, unicodePassword: true)
			});
		}
		string text2;
		if ((text2 = installInfo.InstallEnvironment.Parameters.FirstOrDefault((KeyValueParameters<string, string> x) => x.Key == "HARDWARE-MANUFACTURER-MODE")?.Value) != null && text2.ToUpperInvariant() == "TRUE")
		{
			UpdateInstallInfoParameter(installInfo, new KeyValueParameters<string, string>
			{
				Key = "MANAGEMENTAPPNAME",
				Value = "http://localhost"
			});
			installInfo.InstallEnvironment.Parameters.RemoveAll((KeyValueParameters<string, string> f) => f.Key == "MANAGEMENTAPPNAMESECURE");
			installInfo.InstallEnvironment.Parameters.RemoveAll((KeyValueParameters<string, string> f) => f.Key == "IDPADDRESS");
		}
		installInfo.InstallEnvironment.Parameters.RemoveAll((KeyValueParameters<string, string> f) => _parametersNotSaved.Contains(f.Key));
		if (!Directory.Exists(filePath))
		{
			Directory.CreateDirectory(filePath);
		}
		try
		{
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(InstallInfo));
			using (TextWriter textWriter = new StreamWriter(text, append: false))
			{
				xmlSerializer.Serialize(textWriter, installInfo);
			}
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installer info was saved {0}.\t", text), TraceEventType.Information);
		}
		catch (InvalidOperationException ex)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Failed to write to XML file. Details: {0}", ex.TypeAndMessage()), TraceEventType.Critical);
			throw;
		}
	}

	public void UpdateInstallerInfo()
	{
		if (InstallInfo.NotUninstalledPackages.Count == 0)
		{
			DeleteEntryFromRedistry(AddPrefix(InstallerConfig.InstallerInfo.ID));
		}
		CleanUpRegistry();
		ModelRepository.PersistModels();
	}

	public void CleanUpRegistry()
	{
		Dictionary<string, InstallerPackageType> dictionary = new Dictionary<string, InstallerPackageType>();
		InstallerPackageType[] installerPackages = InstallerConfig.InstallerPackages;
		foreach (InstallerPackageType installerPackageType in installerPackages)
		{
			dictionary[installerPackageType.ID] = installerPackageType;
		}
		List<string> list = null;
		if (SequenceManager.PackagesToUninstall == null)
		{
			if (SequenceManager.InstalledPackages != null && SequenceManager.InstalledPackages.Count != 0)
			{
				list = SequenceManager.InstalledPackages;
			}
		}
		else
		{
			list = SequenceManager.PackagesToUninstall;
		}
		PackageRefType[] uninstallerSequencer = InstallerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType in uninstallerSequencer)
		{
			if (list == null || !list.Contains(packageRefType.ID) || !dictionary.ContainsKey(packageRefType.ID) || !packageRefType.AllowUnInstall)
			{
				continue;
			}
			InstallerPackageType installerPackageType2 = dictionary[packageRefType.ID];
			if (installerPackageType2 != null && installerPackageType2.PostUninstallActions != null)
			{
				if (!string.IsNullOrEmpty(installerPackageType2.PostUninstallActions.ProductCode))
				{
					DeleteEntryFromRedistry(installerPackageType2.PostUninstallActions.ProductCode);
				}
				else
				{
					LogManager.Write("Product code is empty");
				}
			}
		}
	}

	public string GetProductProperty(string prodCode, string property)
	{
		string result = null;
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine(VideoOS.Installer.Common.Properties.Settings.Default.SoftwareKey, prodCode.ToString(CultureInfo.CurrentUICulture)));
			if (registryKey != null)
			{
				result = registryKey.GetValue(property).ToString();
			}
			else
			{
				registryKey = Registry.LocalMachine.OpenSubKey(Path.Combine("SOFTWARE\\Wow6432Node\\Microsoft\\Windows\\CurrentVersion\\Uninstall", prodCode));
				if (registryKey != null)
				{
					result = registryKey.GetValue(property).ToString();
				}
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
		return result;
	}

	public Collection<string> VerifyCurrentMachineState(bool doContinue)
	{
		if (doContinue)
		{
			return GetInstalledPackIdsFromOldInstallerInfo();
		}
		List<string> installationsFromRegistry = GetInstallationsFromRegistry();
		List<string> allowedToUninstallPackages = FindAllowedToUninstallPackage();
		Dictionary<string, InstallerPackageType> first = InstallerConfig.InstallerPackages.Where((InstallerPackageType p) => allowedToUninstallPackages.Contains(p.ID)).ToDictionary((InstallerPackageType i) => i.ProductManifest.Filename);
		Dictionary<string, InstallerPackageType> second = InstallerConfig.InstallerPackages.Where((InstallerPackageType i) => i.ProductManifest64 != null && allowedToUninstallPackages.Contains(i.ID)).ToDictionary((InstallerPackageType i) => i.ProductManifest64.Filename);
		first = first.Union(second).ToDictionary((KeyValuePair<string, InstallerPackageType> i) => i.Key, (KeyValuePair<string, InstallerPackageType> i) => i.Value);
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "There are '{0}' packages files in '{1}' ", first.Count, InstallerConfig.InstallerInfo.DisplayName), TraceEventType.Information);
		foreach (KeyValuePair<string, InstallerPackageType> item in first)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Package: {0}", item.Key), TraceEventType.Information);
		}
		List<string> listOfUniqueProductsOnThisMachine = GetListOfUniqueProductsOnThisMachine(installationsFromRegistry, first);
		Collection<string> collection = new Collection<string>();
		foreach (string item2 in listOfUniqueProductsOnThisMachine)
		{
			if (first.ContainsKey(item2))
			{
				collection.Add(first[item2].ID);
			}
		}
		return collection;
	}

	private List<string> GetListOfUniqueProductsOnThisMachine(List<string> installations, Dictionary<string, InstallerPackageType> packsMap)
	{
		List<string> list = new List<string>();
		foreach (string installation in installations)
		{
			PrevInstallationHelper instHelper = new PrevInstallationHelper(installation, Platform);
			if (!instHelper.IsValid)
			{
				continue;
			}
			bool flag = false;
			List<string> stillInstalledPackages = instHelper.StillInstalledPackages;
			foreach (string item in stillInstalledPackages)
			{
				if (!list.Contains(item))
				{
					list.Add(item);
				}
				if (InstallerConfig.InstallerInfo.GroupInstaller && !flag && instHelper.MainPackage == item && instHelper.ID != InstallerConfig.InstallerInfo.ID && instHelper.DeleteBy == ArpEntryDeleteByType.Everyone && packsMap.ContainsKey(item) && packsMap[item].ID == instHelper.MainPackageID)
				{
					flag = RemoveRegEntryFromLocation(instHelper.ID);
				}
			}
			if (instHelper.MainPackage != null && packsMap.ContainsKey(instHelper.MainPackage) && ((stillInstalledPackages.Count == 0 && InstallerConfig.InstallerInfo.GroupInstaller) || !stillInstalledPackages.Contains(instHelper.MainPackage)) && instHelper.ID != InstallerConfig.InstallerInfo.ID && instHelper.DeleteBy == ArpEntryDeleteByType.Everyone && InstallerConfig.InstallExecuteSequence.Any((PackageRefType package) => (package.Condition == ConditionType.Required || package.Condition == ConditionType.Optional) && package.ID == packsMap[instHelper.MainPackage].ID))
			{
				RemoveRegEntryFromLocation(instHelper.ID);
			}
		}
		return list;
	}

	private List<string> GetInstallationsFromRegistry()
	{
		List<string> list = new List<string>();
		string text = Path.Combine(PublisherKey, "Installation");
		try
		{
			using RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text);
			if (registryKey != null)
			{
				string[] subKeyNames = registryKey.GetSubKeyNames();
				foreach (string text2 in subKeyNames)
				{
					using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(Path.Combine(text, text2));
					if (registryKey2 == null)
					{
						continue;
					}
					object value = registryKey2.GetValue("InstallLocation");
					if (value == null)
					{
						continue;
					}
					string text3 = value.ToString();
					if (!text3.Contains(Environment.GetFolderPath(Environment.SpecialFolder.CommonProgramFilesX86)))
					{
						text3 = Path.Combine(text3, string.Format(CultureInfo.InvariantCulture, VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, text2));
					}
					if (Directory.Exists(text3))
					{
						if (text2 == InstallerConfig.InstallerInfo.ID && text3 != CurrentDirectory)
						{
							DeleteFiles(text3);
						}
						list.Add(text3);
					}
				}
			}
		}
		catch (Exception ex)
		{
			LogManager.Write(ex.TypeAndMessage(), TraceEventType.Warning);
		}
		return list;
	}

	private Collection<string> GetInstalledPackIdsFromOldInstallerInfo()
	{
		Collection<string> collection = new Collection<string>();
		if (_oldInstallation?.InstallInfo != null)
		{
			foreach (PackageRefType installedPackage in _oldInstallation.InstallInfo.InstalledPackages)
			{
				collection.Add(installedPackage.ID);
			}
		}
		return collection;
	}

	private List<string> FindAllowedToUninstallPackage()
	{
		return (from s in InstallerConfig.InstallExecuteSequence
			where s.AllowUnInstall
			select s.ID).ToList();
	}

	private bool RemoveRegEntryFromLocation(string id)
	{
		if (!string.IsNullOrEmpty(id))
		{
			DeleteEntryFromRedistry($"{{{id}}}");
			return true;
		}
		return false;
	}

	public static void CopyDirectory(string source, string destination)
	{
		if (destination[destination.Length - 1] != Path.DirectorySeparatorChar)
		{
			destination += Path.DirectorySeparatorChar;
		}
		if (!Directory.Exists(destination))
		{
			Directory.CreateDirectory(destination);
		}
		string[] fileSystemEntries = Directory.GetFileSystemEntries(source);
		foreach (string text in fileSystemEntries)
		{
			if (Directory.Exists(text))
			{
				CopyDirectory(text, destination + Path.GetFileName(text));
				continue;
			}
			try
			{
				File.Copy(text, destination + Path.GetFileName(text), overwrite: true);
			}
			catch (Exception exception)
			{
				LogManager.Write($"Could not copy file {text}.");
				LogManager.Write(exception);
			}
		}
	}

	private void DeleteEntryFromRedistry(string guid)
	{
		DeleteSoftwareEntry(guid);
	}

	private void DeleteSoftwareEntry(string guid)
	{
		if (string.IsNullOrEmpty(guid))
		{
			return;
		}
		try
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Uninstall", RegistryKeyPermissionCheck.ReadWriteSubTree);
			if (registryKey2 != null && registryKey2.OpenSubKey(guid, RegistryKeyPermissionCheck.ReadWriteSubTree) != null)
			{
				registryKey = registryKey2;
			}
			if (registryKey == null)
			{
				RegistryKey registryKey3 = Registry.LocalMachine.OpenSubKey("SOFTWARE\\\\Wow6432Node\\\\Microsoft\\\\Windows\\\\CurrentVersion\\\\Uninstall", RegistryKeyPermissionCheck.ReadWriteSubTree);
				if (registryKey3 != null && registryKey3.OpenSubKey(guid, RegistryKeyPermissionCheck.ReadWriteSubTree) != null)
				{
					registryKey = registryKey3;
				}
			}
			if (registryKey != null)
			{
				LogManager.Write("Software entry was found in the registry. Attemp to delete " + _softwareKey + guid);
				registryKey.DeleteSubKey(guid);
				LogManager.Write($"Entry {_softwareKey + guid} deleted from registry.");
				registryKey.Close();
			}
		}
		catch (Exception ex)
		{
			LogManager.Write($"An attempt to delete an entry from the registry {_softwareKey + guid} failed. Details: {ex.TypeAndMessage()}.", TraceEventType.Warning);
			LogManager.Write(ex);
		}
	}

	public ExitCode RunInstallationInQuietMode(string fileName, string licenseFile, string licenseType, bool doContinue, Dictionary<string, string> cmdParams)
	{
		Dictionary<string, string> arguments = new Dictionary<string, string>();
		if (!doContinue)
		{
			try
			{
				PrepareForInstallationInQuietMode(ref fileName, ref licenseFile, ref licenseType, cmdParams, arguments);
			}
			catch (Exception ex)
			{
				if (ex is DowngradeDisallowedException || ex is NoFreeSpaceOnDiskException || ex is FileNotFoundException || ex is PlatformNotSupportedException || ex is LicenseCheckException || ex is RunActionException || ex is PrerequisitesNotInstalledException || ex is ComplianceException)
				{
					return ExitCode.Error;
				}
				throw;
			}
		}
		else
		{
			_continueDir = CurrentDirectory;
			InstallInfo = LoadInstallationInfoXml(_continueDir, isUninstall: true);
			UpdateParametersFromInstallInfoAndServerConfigurator();
			int websitePort = 80;
			if (Parameters.ContainsKey("WebSitePort"))
			{
				websitePort = Convert.ToInt32(Parameters["WebSitePort"], CultureInfo.InvariantCulture);
			}
			CreateSequenceManager();
			UpdatePackageParameters();
			RemovePrerequisitesByCommandLineParameter();
			SequenceManager.SetPrerequisitesToInstall(PrerequisiteManager.PackagesToInstall, websitePort);
			if (InstallInfo.InstalledPackages.Count != 0 || InstallInfo.NotInstalledPackages.Count != 0)
			{
				SkipInstalledPackages();
				AddNotInstalledPackages();
			}
		}
		try
		{
			SequenceManager.Install();
		}
		catch (DowngradeDisallowedException exception)
		{
			LogManager.Write(exception);
			throw;
		}
		catch (Exception exception2)
		{
			LogManager.Write(exception2);
		}
		if (SequenceManager.FailedInstallPackages.Count != 0)
		{
			string text = null;
			foreach (KeyValuePair<string, string> failedInstallPackage in SequenceManager.FailedInstallPackages)
			{
				text = text + failedInstallPackage.Key + " " + failedInstallPackage.Value + " \n";
			}
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Packages were not installed. Error message: {0} :", text), TraceEventType.Error);
			SaveAndCloseBeforeExit();
			return ExitCode.Error;
		}
		if (SequenceManager.NeedReboot)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Installation was not completed. Reboot required "), TraceEventType.Error);
			HandleReboot(doContinue);
			if (SequenceManager.NotExecutedInstallPackages.Count != 0)
			{
				return ExitCode.SuccessRebootRequired;
			}
			return ExitCode.SuccessRebootRequiredAllPackagesInstalled;
		}
		SaveAndCloseBeforeExit();
		return ExitCode.Success;
	}

	private void SaveAndCloseBeforeExit()
	{
		try
		{
			SaveInstallationInfo(doContinue: false);
			CloseActions();
		}
		catch (ShortcutException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), TraceEventType.Error);
		}
	}

	private void PrepareForInstallationInQuietMode(ref string fileName, ref string licenseFile, ref string licenseType, Dictionary<string, string> cmdParams, Dictionary<string, string> arguments)
	{
		try
		{
			LoadFromArgumentFile(ref fileName, arguments);
		}
		catch (FileNotFoundException)
		{
			throw;
		}
		if (HasFipsEnabledOnUpgradeFromNonFipsCompliantVersion())
		{
			LogManager.Write("You must disable Windows FIPS 140-2 mode in order to proceed with upgrade. To learn more about Windows FIPS mode, visit https://docs.microsoft.com/en-us/windows/security/threat-protection/security-policy-settings/system-cryptography-use-fips-compliant-algorithms-for-encryption-hashing-and-signing.", TraceEventType.Error);
			throw new ComplianceException();
		}
		MergeArguments(cmdParams, arguments);
		ValidateArguments(arguments);
		if (TryGetPreviousMilestoneProductInstallPath(out var installationPath))
		{
			if (arguments.ContainsKey("TARGETDIR"))
			{
				LogManager.Write("PrepareForInstallationInQuietMode(): Ignoring TARGETDIR parameter from command line or argument file as this is an upgrade or other product components already installed", TraceEventType.Information);
			}
			arguments["TARGETDIR"] = installationPath;
			LogManager.Write(FormattableString.Invariant($"PrepareForInstallationInQuietMode(): TARGETDIR updated to {installationPath} as other product components already installed"), TraceEventType.Information);
		}
		if (!TargetPlatformCheck() || !IsWindowsVersionSupported(new OSInformationReader()))
		{
			throw new PlatformNotSupportedException();
		}
		CreatePrerequisiteManager();
		CreateSequenceManager();
		UpdateParameters(QuietOverWriteParameters);
		foreach (KeyValuePair<string, string> quietOverWriteParameter in QuietOverWriteParameters)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Quiet istallation overwrites parameter '{0}' with '{1}'", quietOverWriteParameter.Key, quietOverWriteParameter.Value));
		}
		UpdateParameters(arguments);
		InstallInfo.InstallerLanguage = Parameters["LANGUAGE"];
		InstallInfo.ProductLanguage = Parameters["LANGUAGE"];
		if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			try
			{
				CheckLicense(ref licenseFile, ref licenseType);
			}
			catch (LicenseCheckException)
			{
				throw;
			}
		}
		if (licenseFile != null)
		{
			Parameters["LICENSE"] = licenseFile;
		}
		else if (Parameters.ContainsKey("LICENSE"))
		{
			Parameters.Remove("LICENSE");
		}
		foreach (KeyValuePair<string, string> parameter in Parameters)
		{
			GlobalData.SetValue(parameter.Key, parameter.Value, "RunInstallationInQuietMode()");
		}
		try
		{
			ExecutePreAndPostActions();
		}
		catch (RunActionException)
		{
			throw;
		}
		if (!GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			IEnumerable<CertificatePageContent> enumerable = (from y in SubscriptionPlugins.CertificateSubscriptions
				where y.BeforeActivate(Parameters)
				select y.GetContent()).ToList();
			if (enumerable.Any())
			{
				CertificateModelInitializer.Initialize(enumerable, isQuietMode: true, GlobalData.GetValueAsBoolean("IS_UPGRADE"), null, Parameters);
			}
		}
		UpdatePackageParameters();
		PrerequisiteManager.PrepareInstallationPrerequisites();
		PrerequisiteManager.PrepareInstallation();
		if (PrerequisiteManager.PackagesCannotInstall.Count != 0)
		{
			string text = null;
			foreach (KeyValuePair<string, string> item in PrerequisiteManager.PackagesCannotInstall)
			{
				text = text + item.Key + " ";
			}
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Prerequisites need to be installed by user: {0}.", text), TraceEventType.Error);
			throw new PrerequisitesNotInstalledException();
		}
		if (!CheckFreeSpaceOnDisk())
		{
			throw new NoFreeSpaceOnDiskException();
		}
		if (Parameters.ContainsKey("COMPONENTS"))
		{
			string[] collection = ((Parameters["COMPONENTS"].IndexOf(";", StringComparison.OrdinalIgnoreCase) != -1) ? Parameters["COMPONENTS"].Split(';') : new string[1] { Parameters["COMPONENTS"] });
			List<string> list = new List<string>(collection);
			PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
			foreach (PackageRefType packageRefType in installExecuteSequence)
			{
				if (!list.Contains(packageRefType.Name))
				{
					packageRefType.Condition = ConditionType.Skip;
				}
				else
				{
					packageRefType.Condition = ConditionType.Optional;
				}
			}
			installExecuteSequence = InstallerConfig.UninstallerInfo.UninstallerSequencer;
			foreach (PackageRefType packageRefType2 in installExecuteSequence)
			{
				if (!list.Contains(packageRefType2.Name))
				{
					packageRefType2.Condition = ConditionType.Skip;
				}
				else
				{
					packageRefType2.Condition = ConditionType.Optional;
				}
			}
		}
		int websitePort = 80;
		if (Parameters.ContainsKey("WebSitePort"))
		{
			websitePort = Convert.ToInt32(Parameters["WebSitePort"], CultureInfo.InvariantCulture);
		}
		RemovePrerequisitesByCommandLineParameter();
		SequenceManager.SetPrerequisitesToInstall(PrerequisiteManager.PackagesToInstall, websitePort);
	}

	private void ValidateArguments(Dictionary<string, string> arguments)
	{
		if (Parameters.ContainsKey("LOCKMANAGEMENTSERVER") && bool.Parse(Parameters["LOCKMANAGEMENTSERVER"]) && arguments.ContainsKey("MANAGEMENTSERVERURI") && Parameters["MANAGEMENTSERVERURI"].ToUpperInvariant().TrimEnd('/') != arguments["MANAGEMENTSERVERURI"].ToUpperInvariant().TrimEnd('/'))
		{
			throw new ArgumentException(string.Format(CultureInfo.CurrentCulture, "Management server uri was locked with value \"{0}\", that is different than one specified in arguments \"{1}\"", Parameters["MANAGEMENTSERVERURI"], arguments["MANAGEMENTSERVERURI"]));
		}
	}

	private void ExecutePreAndPostActions()
	{
		List<string> obj = new List<string> { "LicenseUpdatePage", "ProcessLicenseUpdatePage", "WelcomePage", "IISSetupPage", "ServiceAccountPage" };
		List<string> list = new List<string>();
		Dictionary<string, IPageSetupActions> dictionary = CollectPageSetupActions();
		foreach (string item in obj)
		{
			if (dictionary.ContainsKey(item))
			{
				IPageSetupActions action = dictionary[item];
				list.Add(item);
				if (!RunAction(action, item))
				{
					throw new RunActionException();
				}
			}
		}
		foreach (KeyValuePair<string, IPageSetupActions> item2 in dictionary)
		{
			if (!list.Contains(item2.Key) && !RunAction(item2.Value, item2.Key))
			{
				throw new RunActionException();
			}
		}
	}

	private bool CheckFreeSpaceOnDisk()
	{
		GetFreeSpace(Parameters["TARGETDIR"]);
		double num = IsEnoughOfSpaceOnDrive();
		if (num != 0.0)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Not enough free space on the drive. {0} needed.", num), TraceEventType.Error);
			return false;
		}
		return true;
	}

	private void CheckLicense(ref string licenseFile, ref string licenseType)
	{
		if (licenseType == null && licenseFile == null)
		{
			if (Parameters.ContainsKey("LICENSETYPE"))
			{
				licenseType = Parameters["LICENSETYPE"];
			}
			else if (InstallInfo.InstallEnvironment.LicenseSpecified)
			{
				licenseType = InstallInfo.InstallEnvironment.License.ToString();
			}
			if (licenseType == null)
			{
				licenseType = VideoOS.Installer.Configuration.LicenseType.None.ToString();
			}
		}
		else if (licenseFile != null)
		{
			licenseType = VideoOS.Installer.Configuration.LicenseType.LicenseFile.ToString();
		}
		if (licenseFile == null && Parameters.ContainsKey("LICENSE"))
		{
			licenseFile = Parameters["LICENSE"];
			if (!File.Exists(licenseFile))
			{
				licenseFile = null;
			}
			else
			{
				licenseType = VideoOS.Installer.Configuration.LicenseType.LicenseFile.ToString();
			}
		}
		if (licenseFile == null)
		{
			if (licenseType != VideoOS.Installer.Configuration.LicenseType.None.ToString())
			{
				LogManager.Write("The selected license is not found.", TraceEventType.Error);
				throw new LicenseCheckException();
			}
			return;
		}
		try
		{
			RunCheckLicense(licenseFile, licenseType, (Wizard)null);
		}
		catch (LicenseCheckException ex)
		{
			LogManager.Write(ex.LogMessage, TraceEventType.Error);
			throw;
		}
	}

	private void LoadFromArgumentFile(ref string fileName, Dictionary<string, string> arguments)
	{
		if (!string.IsNullOrEmpty(fileName))
		{
			fileName = Path.Combine(CurrentDirectory, fileName);
			if (!File.Exists(fileName))
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "File not found {0}.", fileName), TraceEventType.Critical);
				throw new FileNotFoundException(string.Format(CultureInfo.InvariantCulture, "File not found {0}.", fileName));
			}
			MergeArguments(InstallerConfigManager.LoadCommandLineArguments(fileName).ToDictionary((IArgument a) => a.Name, (IArgument a) => a.Value), arguments);
		}
	}

	private static void MergeArguments(Dictionary<string, string> source, Dictionary<string, string> target)
	{
		foreach (KeyValuePair<string, string> item in source)
		{
			if (!string.IsNullOrEmpty(item.Value) && !string.IsNullOrEmpty(item.Key))
			{
				target[item.Key] = item.Value;
			}
		}
	}

	private void HandleReboot(bool doContinue)
	{
		if (SequenceManager.NotExecutedInstallPackages.Count != 0)
		{
			SaveIntermediateInstallInfo(null, BreakAction.InstallContinue);
		}
		else
		{
			FinalizeInstallation(doContinue);
		}
	}

	private void FinalizeInstallation(bool doContinue)
	{
		try
		{
			SaveInstallationInfo(doContinue);
		}
		catch (ShortcutException)
		{
			throw;
		}
		catch (Exception ex2)
		{
			LogManager.Write(ex2.TypeAndMessage(), TraceEventType.Error);
		}
	}

	public void RemovePrerequisitesByCommandLineParameter()
	{
		if (Parameters.ContainsKey("PREREQUISITESTOSKIP"))
		{
			RemovePrerequisites(PrerequisiteManager.PackagesToInstall, Parameters["PREREQUISITESTOSKIP"]);
		}
	}

	private void RemovePrerequisites(Dictionary<string, ProductInstallInfo> prerequisitesToInstall, string skipCommand)
	{
		foreach (string p in skipCommand.Split(';').ToList())
		{
			if (prerequisitesToInstall.Any((KeyValuePair<string, ProductInstallInfo> i) => i.Key.Equals(p, StringComparison.OrdinalIgnoreCase)))
			{
				prerequisitesToInstall.Remove(p);
				LogManager.Write("RemovePrerequisites(): Prerequisite '" + p + "' was removed by command line. Command line parameter: '" + skipCommand + "'");
			}
		}
	}

	private bool RunAction(IPageSetupActions action, string key)
	{
		if ((InstallerConfig.InstallerSettings.FlowSettings20 != null && InstallerConfig.InstallerSettings.FlowSettings20.PageSequencer != null && InstallerConfig.InstallerSettings.FlowSettings20.PageSequencer.Contains(key)) || (InstallerConfig.InstallerSettings.FlowSettings != null && !InstallerConfig.InstallerSettings.FlowSettings.Contains(key) && AvailablePages10.Contains(key)))
		{
			try
			{
				action.BeforeActivatePage(Parameters);
				MergeGlobalDataToParameters();
				action.BeforeLeavePage(Parameters);
				MergeGlobalDataToParameters();
				MergeCollectedParameter(action.CollectParameters());
			}
			catch (ComplianceException)
			{
				LogManager.Write("To be FIPS compliant, your data must be encrypted by a stronger encryption type. To allow this, the SQL Server must not be running in a FIPS 140-2 compliant mode during the upgrade. To learn more about running a FIPS 140 - 2 compliant SQL Server, visit(https://support.microsoft.com/en-us/help/4014354/how-to-use-sql-server-2016-in-fips-140-2-compliant-mode).", TraceEventType.Error);
				throw;
			}
			catch (LicenseCheckException ex2)
			{
				LogManager.Write(ex2.LogMessage, TraceEventType.Error);
				throw;
			}
			catch (Exception exception)
			{
				LogParameters();
				LogManager.Write($"Setup actions for '{action.PageName} failed.'");
				LogManager.Write(exception);
				return false;
			}
		}
		foreach (Plugin plugin in Plugins)
		{
			InstallerPluginTypeWizardPage[] wizardPages = plugin.WizardPages;
			foreach (InstallerPluginTypeWizardPage installerPluginTypeWizardPage in wizardPages)
			{
				if (plugin.WizardProvider == null || installerPluginTypeWizardPage.UninstallPage)
				{
					continue;
				}
				IWizardPage wizardPage = plugin.WizardProvider.CreateInstance(installerPluginTypeWizardPage.Name);
				if (wizardPage is WizardPageBase)
				{
					WizardPageBase wizardPageBase = wizardPage as WizardPageBase;
					if (wizardPageBase.ExecSetParametersInQuiet)
					{
						wizardPageBase.SetParameters(Parameters);
					}
				}
			}
		}
		return true;
	}

	public void MergeGlobalDataToParameters()
	{
		foreach (IPackageParameter setting in GlobalData.Settings)
		{
			Parameters[setting.Target] = setting.Value;
		}
	}

	private void MergeCollectedParameter(Dictionary<string, string> newParameters)
	{
		foreach (KeyValuePair<string, string> param in newParameters)
		{
			Parameters[param.Key] = param.Value;
			IPackageParameter packageParameter = GlobalData.Settings.FirstOrDefault((IPackageParameter x) => x.Target == param.Key);
			if (packageParameter != null)
			{
				packageParameter.Value = param.Value;
			}
		}
	}

	public ReadOnlyCollection<string> GetSuppressedPages()
	{
		if (_suppressedPages == null)
		{
			List<string> list = Instance.AvailablePages10.ToList();
			list.AddRange(Instance.AvailablePages20);
			if (InstallerConfig.InstallerSettings.FlowSettings != null && InstallerConfig.InstallerSettings.FlowSettings.Length != 0)
			{
				List<string> list2 = new List<string>(InstallerConfig.InstallerSettings.FlowSettings);
				List<string> list3 = new List<string>();
				foreach (string item in Instance.AvailablePages10)
				{
					if (!list2.Contains(item))
					{
						list3.Add(item);
					}
				}
				foreach (string item2 in Instance.AvailablePages20)
				{
					if (list3.Contains(item2))
					{
						list3.Remove(item2);
					}
				}
				if (InstallerConfig.InstallerSettings.FlowSettings20 != null)
				{
					foreach (string item3 in new List<string>(InstallerConfig.InstallerSettings.FlowSettings20.PageSequencer))
					{
						list3.Add(item3);
					}
				}
				list2.Clear();
				foreach (string item4 in list)
				{
					if (!list3.Contains(item4))
					{
						list2.Add(item4);
					}
				}
				_suppressedPages = list2;
			}
			else if (InstallerConfig.InstallerSettings.FlowSettings20 != null)
			{
				foreach (string item5 in new List<string>(InstallerConfig.InstallerSettings.FlowSettings20.PageSequencer))
				{
					if (list.Contains(item5))
					{
						list.Remove(item5);
					}
				}
				_suppressedPages = list;
			}
			foreach (string necessaryPage in Instance.NecessaryPages)
			{
				if (_suppressedPages.Contains(necessaryPage))
				{
					_suppressedPages.Remove(necessaryPage);
				}
			}
		}
		return new ReadOnlyCollection<string>(_suppressedPages);
	}

	public Dictionary<string, IPageSetupActions> CollectPageSetupActions()
	{
		Dictionary<string, IPageSetupActions> dictionary = PageActionPlugins;
		if (dictionary == null)
		{
			dictionary = new Dictionary<string, IPageSetupActions>();
		}
		ReadOnlyCollection<string> suppressedPages = GetSuppressedPages();
		foreach (KeyValuePair<string, Func<IPageSetupActions>> builtInPageAction in BuiltInPageActions)
		{
			if (!dictionary.ContainsKey(builtInPageAction.Key) && !suppressedPages.Contains(builtInPageAction.Key))
			{
				dictionary.Add(builtInPageAction.Key, builtInPageAction.Value());
			}
		}
		return dictionary;
	}

	public void RunCheckLicense(string text, string type, Wizard wizard)
	{
		if (InstallerConfig.LicenseProvider != null && InstallerConfig.LicenseProvider.LicenseCustomisations != null)
		{
			string text2 = null;
			LicenseCheckException foundException = null;
			if (InstallerConfig.LicenseProvider.LicenseCustomisations.ByLicense)
			{
				text2 = CheckLicenseAndGetDisplaNameWithException(text, type, out foundException);
			}
			else
			{
				if (InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom.Length == 0)
				{
					RunCheckLicense(text, type, _productSku);
					return;
				}
				text2 = CheckLicenseByLicenseCustomizationsWithException(text, type, InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom, out foundException);
			}
			if (foundException != null)
			{
				throw foundException;
			}
			if (text2 != null)
			{
				Parameters["PRODUCT-SKU"] = text2;
				GlobalData.SetValue("PRODUCT-SKU", text2, "RunCheckLicense()");
				InstallInfo.IsTrialVersion = false;
				if (wizard != null)
				{
					wizard.ParentForm.Text = VersionFormat.GetFormCaption();
				}
			}
		}
		else
		{
			RunCheckLicense(text, type, _productSku);
		}
	}

	private string CheckLicenseAndGetDisplaNameWithException(string text, string type, out LicenseCheckException foundException)
	{
		string productKey = null;
		foundException = null;
		try
		{
			RunCheckLicenseAndGetDisplayName(text, type, out var displayName, out productKey);
			UpdateLicenseCustomizationNames(displayName, productKey);
		}
		catch (LicenseCheckException ex)
		{
			foundException = ex;
		}
		return productKey;
	}

	private void UpdateLicenseCustomizationNames(string displayName, string productKey)
	{
		if (InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom != null)
		{
			LicenseCustomisationsTypeLicenseCustom[] licenseCustom = InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom;
			foreach (LicenseCustomisationsTypeLicenseCustom licenseCustomisationsTypeLicenseCustom in licenseCustom)
			{
				if (licenseCustomisationsTypeLicenseCustom.ProductKey.Equals(productKey))
				{
					licenseCustomisationsTypeLicenseCustom.DisplayName = displayName;
					licenseCustomisationsTypeLicenseCustom.Caption = displayName;
					return;
				}
			}
		}
		else
		{
			InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom = new LicenseCustomisationsTypeLicenseCustom[0];
		}
		List<LicenseCustomisationsTypeLicenseCustom> list = InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom.ToList();
		list.Add(new LicenseCustomisationsTypeLicenseCustom
		{
			DisplayName = displayName,
			Caption = displayName,
			ProductKey = productKey,
			DisplayVersion = string.Empty
		});
		InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom = list.ToArray();
	}

	private string CheckLicenseByLicenseCustomizationsWithException(string text, string type, LicenseCustomisationsTypeLicenseCustom[] licenseCustomisationsTypeLicenseCustom, out LicenseCheckException foundException)
	{
		string text2 = null;
		foundException = null;
		LicenseCustomisationsTypeLicenseCustom[] licenseCustom = InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom;
		foreach (LicenseCustomisationsTypeLicenseCustom licenseCustomisationsTypeLicenseCustom2 in licenseCustom)
		{
			try
			{
				RunCheckLicense(text, type, licenseCustomisationsTypeLicenseCustom2.ProductKey);
				text2 = licenseCustomisationsTypeLicenseCustom2.ProductKey;
				SaveLicenseLocation(text, text2);
				foundException = null;
			}
			catch (LicenseCheckException ex)
			{
				foundException = ex;
				continue;
			}
			break;
		}
		return text2;
	}

	public string LookupLicenseRegistryEntry()
	{
		return RegistryKeyHelper.GetLocalMachineValue(Path.Combine(PublisherKey, string.Format(VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, InstallerConfig.InstallerInfo.ID)), "LicenseLocation");
	}

	private void SaveLicenseLocation(string text, string updateProductKey)
	{
		string text2 = Path.Combine(PublisherKey, string.Format(VideoOS.Installer.Common.Properties.Settings.Default.InstallationFolder, InstallerConfig.InstallerInfo.ID));
		try
		{
			RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text2, writable: true);
			if (registryKey == null)
			{
				Registry.LocalMachine.CreateSubKey(text2, RegistryKeyPermissionCheck.Default);
				registryKey = Registry.LocalMachine.OpenSubKey(text2, writable: true);
			}
			if (registryKey != null)
			{
				registryKey.SetValue("LicenseLocation", text);
				if (!string.IsNullOrEmpty(updateProductKey))
				{
					registryKey.SetValue("ProductKey", updateProductKey);
				}
			}
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
		}
	}

	private void RunCheckLicenseAndGetDisplayName(string text, string type, out string displayName, out string productKey)
	{
		ILicenseProvider licenseProvider = LicenseProvider;
		LicenseStatus licenseStatus;
		try
		{
			licenseStatus = licenseProvider.CheckLicenseAndGetDisplayName(text, type, out displayName, out productKey);
		}
		catch (Exception ex)
		{
			throw new LicenseCheckException(ex.Message, Resources.LicenseCheck_InvalidCaption);
		}
		ValidateStatus(licenseStatus);
	}

	private void RunCheckLicense(string text, string type, string productKey)
	{
		ILicenseProvider licenseProvider = LicenseProvider;
		LicenseStatus licenseStatus;
		try
		{
			licenseStatus = licenseProvider.CheckLicense(text, type, productKey);
		}
		catch (Exception ex)
		{
			throw new LicenseCheckException(ex.Message, Resources.LicenseCheck_InvalidCaption);
		}
		ValidateStatus(licenseStatus);
	}

	public void ValidateStatus(LicenseStatus licenseStatus)
	{
		switch (licenseStatus)
		{
		case LicenseStatus.Invalid:
			throw new LicenseCheckException(Resources.LicenseCheck_Invalid, Resources.LicenseCheck_InvalidCaption, "The selected license is invalid.");
		case LicenseStatus.Expired:
			throw new LicenseCheckException(Resources.LicenseCheck_Expired, Resources.LicenseCheck_ExpiredCaption, "The selected license is expired.");
		case LicenseStatus.NotFound:
			throw new LicenseCheckException(Resources.LicenseCheck_NotFound, Resources.LicenseCheck_NotFoundCaption, "The selected license is not found.");
		case LicenseStatus.ProductInvalid:
			throw new LicenseCheckException(Resources.LicenseCheck_ProductInvalidLicense, Resources.LicenseCheck_InvalidCaption, "The specified license is not valid for this product.");
		case LicenseStatus.Ok:
		case LicenseStatus.Demo:
			break;
		}
	}

	public void UpdateParameters(Dictionary<string, string> arguments)
	{
		foreach (KeyValuePair<string, string> argument in arguments)
		{
			string text = argument.Value;
			if (Platform == PlatformType.X86 && text.Contains("x86"))
			{
				text = argument.Value.Replace(" (x86)", "");
				if (text.Contains("x86"))
				{
					text = argument.Value.Replace("x86", "");
				}
			}
			if (!(argument.Key == "LANGUAGE") || !Parameters.ContainsKey("LANGUAGE") || Language == null)
			{
				if (argument.Key.ToLower().Contains("password"))
				{
					Parameters[argument.Key] = EncryptHelper.EncryptPassword(argument.Value, unicodePassword: true);
				}
				else
				{
					Parameters[argument.Key] = ExpandEvironmentVariables(text);
				}
			}
		}
		if (!Parameters.ContainsKey("LANGUAGE"))
		{
			Parameters["LANGUAGE"] = "en-US";
		}
	}

	private void Instance_CultureChanged(object sender, CultureChangedEventArgs e)
	{
		Resources.Culture = e.Culture;
	}

	public ExitCode RunUnInstallationInQuietMode()
	{
		List<string> list = new List<string>();
		if (InstallInfo != null)
		{
			foreach (PackageRefType installedPackage in InstallInfo.InstalledPackages)
			{
				list.Add(installedPackage.ID);
			}
		}
		CreateSequenceManager();
		UpdatePackageParameters();
		SequenceManager.PackagesToUninstall = list;
		try
		{
			SequenceManager.Uninstall();
			UpdateInstallerInfo();
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			return ExitCode.Error;
		}
		return ExitCode.Success;
	}

	public void ExtractArgumentsFile(string targetArgDir)
	{
		string fileName = "Arguments.xml";
		ExtractFile(targetArgDir, fileName);
	}

	private void ExtractFile(string targetDir, string fileName)
	{
		if (targetDir[targetDir.Length - 1] != Path.DirectorySeparatorChar)
		{
			targetDir += Path.DirectorySeparatorChar;
		}
		try
		{
			if (!Directory.Exists(Path.GetDirectoryName(targetDir)) && Directory.GetDirectoryRoot(targetDir) != targetDir)
			{
				Directory.CreateDirectory(Path.GetDirectoryName(targetDir));
			}
			if (ExtractParametersFromInstallerConfig(Path.Combine(targetDir, Path.GetFileName(fileName))))
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "File {0} has been created in {1}.", fileName, targetDir));
			}
		}
		catch (Exception exception)
		{
			LogManager.Write($"Could not generate file {fileName}.");
			LogManager.Write(exception);
		}
	}

	public bool ExtractParametersFromInstallerConfig(string argumentFilePath)
	{
		if (LoadInstallerConfig(enableMsiLog: true))
		{
			try
			{
				LoadConfiguration();
			}
			catch (Exception)
			{
				throw;
			}
			Arguments arguments = new Arguments();
			InstallerPluginType[] installerPlugins = InstallerConfig.InstallerPlugins;
			foreach (InstallerPluginType installerPluginType in installerPlugins)
			{
				if (installerPluginType.Parameters == null)
				{
					continue;
				}
				ParameterType[] parameters = installerPluginType.Parameters;
				foreach (ParameterType parameterType in parameters)
				{
					if (!parameterType.SuppressInArgumentsFile)
					{
						arguments.AddArgument(parameterType.Name, parameterType.DefaultValue ?? string.Empty, parameterType.Description);
					}
				}
			}
			using (Stream result = new FileStream(argumentFilePath, FileMode.Create, FileAccess.ReadWrite))
			{
				ArgumentsInterpreter.CreateXmlStreamFromArguments(arguments, result);
			}
			return true;
		}
		LogManager.Write("Could not generate argument xml file, error while loading installer config");
		return false;
	}

	public void SaveIntermediateInstallInfo(string pageName, BreakAction action)
	{
		SaveInstalledPackages();
		if (action == BreakAction.InstallContinue || action == BreakAction.UninstallContinue || action == BreakAction.PrereqInstallContinue)
		{
			SetRunOnceKey(action);
		}
		UpdateContinueParameters(action.ToString(), pageName);
		ModelRepository.PrepareForReboot();
		_parametersNotSaved.Remove("SYSTEMPASSWORD");
		_parametersNotSaved.Remove("AWSSECRET");
		SerializeObject(CurrentDirectory, InstallInfo);
	}

	private void UpdateContinueParameters(string action, string pageName)
	{
		bool flag = false;
		bool flag2 = false;
		foreach (KeyValueParameters<string, string> parameter in InstallInfo.InstallEnvironment.Parameters)
		{
			if (parameter.Key == "Installation Context")
			{
				parameter.Value = action;
				flag = true;
			}
			if (parameter.Key == "PAGENAME" && !string.IsNullOrEmpty(pageName))
			{
				parameter.Value = pageName;
				flag2 = true;
			}
		}
		if (!flag)
		{
			InstallInfo.InstallEnvironment.Parameters.Add(CreateKeyValueParameter("Installation Context", action));
		}
		if (!flag2 && !string.IsNullOrEmpty(pageName))
		{
			InstallInfo.InstallEnvironment.Parameters.Add(CreateKeyValueParameter("PAGENAME", pageName));
		}
	}

	private KeyValueParameters<string, string> CreateKeyValueParameter(string value, string key)
	{
		return new KeyValueParameters<string, string>
		{
			Key = value,
			Value = key
		};
	}

	private void CreateInstallerShortcut(string executable, string arguments)
	{
		try
		{
			ShortcutManager shortcutManager = new ShortcutManager(ShortcutPath);
			string icon = Path.Combine(CurrentDirectory, "icon.ico");
			shortcutManager.Create(executable, arguments, icon);
		}
		catch (ShortcutException ex)
		{
			if (ex.InnerException == null)
			{
				LogManager.Write(ex.TypeAndMessage());
			}
			else
			{
				LogManager.Write(ex.Message + " " + ex.InnerException.TypeAndMessage());
			}
			throw;
		}
	}

	public void RemoveInstallerShortcut()
	{
		try
		{
			new ShortcutManager(ShortcutPath).Remove();
		}
		catch (ShortcutException ex)
		{
			if (ex.InnerException == null)
			{
				LogManager.Write(ex.TypeAndMessage());
			}
			else
			{
				LogManager.Write(ex.Message + " " + ex.InnerException.TypeAndMessage());
			}
		}
	}

	private void SetRunOnceKey(BreakAction action)
	{
		string text = "Software\\Microsoft\\Windows\\CurrentVersion\\RunOnce";
		RegistryKey registryKey = null;
		RegistryKey registryKey2 = null;
		bool valueAsBoolean = GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		bool valueAsBoolean2 = GlobalData.GetValueAsBoolean("FINALIZE-MODE");
		bool valueAsBoolean3 = GlobalData.GetValueAsBoolean("QUIET-MODE");
		try
		{
			registryKey2 = Registry.CurrentUser;
			registryKey = registryKey2.OpenSubKey(text, writable: true);
			if (registryKey == null)
			{
				registryKey = registryKey2.CreateSubKey(text, RegistryKeyPermissionCheck.Default);
				registryKey = registryKey2.OpenSubKey(text, writable: true);
			}
			if (registryKey != null)
			{
				string arg = Path.Combine(CurrentDirectory, EntryAssemblyName);
				List<string> list = new List<string> { "--continue" };
				if (action == BreakAction.UninstallContinue)
				{
					list.Add("--uninstall");
				}
				else if (valueAsBoolean)
				{
					list.Add("--hardwareManufacturerMode");
				}
				else if (valueAsBoolean2)
				{
					list.Add("--finalizeMode");
				}
				if (valueAsBoolean3)
				{
					list.Add("--quiet");
				}
				if (!string.IsNullOrEmpty(Language))
				{
					list.Add(string.Format(CultureInfo.InvariantCulture, "--language={0}", Language));
				}
				registryKey.SetValue("InstallerFramework", string.Format(CultureInfo.InvariantCulture, "{0} {1}", arg, string.Join(" ", list)));
			}
			else
			{
				LogManager.Write("Error: Could not add value to the registry.");
			}
		}
		catch (Exception ex)
		{
			LogManager.Write($"Security error detected {ex.TypeAndMessage()}.", TraceEventType.Error);
		}
	}

	public string GetContinuePageName()
	{
		if (_continueDir != null)
		{
			InstallInfo = LoadInstallationInfoXml(_continueDir, isUninstall: true);
			UpdateParametersFromInstallInfoAndServerConfigurator();
			return GetParameterValue("PAGENAME");
		}
		return null;
	}

	public void UpdateParametersFromInstallInfoAndServerConfigurator()
	{
		UpdateInstallInfoParameters(InstallInfo.InstallEnvironment.Parameters);
	}

	private void UpdateInstallInfoParameters(List<KeyValueParameters<string, string>> installInfoParams)
	{
		List<string> list = new List<string>
		{
			"LICENSE", "LANGUAGE", "LICENSETYPE", "PAGENAME", "USER_ACTION", "Installation Context", "PRODUCT-KEY", "InstallNewServerChecked", "SQL-KEEP-DATA", "SQL-CREATE-DATABASE",
			"Getting Started", "Run Application Title", "Run Application", "COMPONENTS", "Why Uninstalling", "Getting Started Title", "UPDATE_TO_PRODUCTNAME", "ENRYPTED_SLC", "NEW_SLC_PATH", "UPDATE_LECENSE_NOT_REQUIRED",
			"IS_UPGRADE", "VERSION_MAJOR", "VERSION_MINOR", "VERSION_REVISION", "IISManagementConsoleInstalled", "CONNECTEDTOSERVER"
		};
		List<string> list2 = new List<string> { "LANGUAGE", "PRODUCT-KEY", "InstallNewServerChecked", "SQL-KEEP-DATA", "SQL-CREATE-DATABASE", "Getting Started", "Run Application Title", "Run Application" };
		if (!string.IsNullOrEmpty(_continueDir))
		{
			list = new List<string>();
		}
		foreach (KeyValueParameters<string, string> installInfoParam in installInfoParams)
		{
			if (list2.Contains(installInfoParam.Key))
			{
				QuietOverWriteParameters[installInfoParam.Key] = installInfoParam.Value;
			}
			if (!list.Contains(installInfoParam.Key) && !ManagementServerUriUpdated(installInfoParam))
			{
				SetParameter(installInfoParam.Key, installInfoParam.Value);
			}
		}
	}

	private bool ManagementServerUriUpdated(KeyValueParameters<string, string> pair)
	{
		if (ParameterHasThisNameAndAValue(pair, "SERVERHOSTNAME") || ParameterHasThisNameAndAValue(pair, "VMSSERVERHOSTNAME"))
		{
			Uri uri = new UriBuilder(Uri.UriSchemeHttp, pair.Value).Uri;
			SetParameter("MANAGEMENTAPPNAME", uri.AbsoluteUri);
			return true;
		}
		if ((ParameterHasThisNameAndAValue(pair, "SERVERPORT") || ParameterHasThisNameAndAValue(pair, "VMSSERVERPORT")) && !string.IsNullOrWhiteSpace(Parameters.GetValueOrNull("MANAGEMENTAPPNAME")))
		{
			UriBuilder uriBuilder = new UriBuilder(Parameters.GetValueOrNull("MANAGEMENTAPPNAME"));
			uriBuilder.Port = int.Parse(pair.Value, CultureInfo.InvariantCulture);
			SetParameter("MANAGEMENTAPPNAME", uriBuilder.Uri.AbsoluteUri);
			return true;
		}
		if (pair.Key.ToUpperInvariant().Equals("URI"))
		{
			SetParameter("MANAGEMENTAPPNAME", pair.Value);
			return true;
		}
		return false;
	}

	private static bool ParameterHasThisNameAndAValue(KeyValueParameters<string, string> parameter, string parameterName)
	{
		if (parameter.Key.ToUpperInvariant().Equals(parameterName))
		{
			return !string.IsNullOrWhiteSpace(parameter.Value);
		}
		return false;
	}

	private static string UpdateParameterName(string parameterName, Dictionary<string, string> parameters)
	{
		if (parameters.ContainsKey(parameterName))
		{
			return parameterName;
		}
		switch (parameterName.ToUpperInvariant())
		{
		case "SERVERHOSTNAME":
		case "VMSSERVERHOSTNAME":
		case "URI":
			return "MANAGEMENTAPPNAME";
		case "SERVERPORT":
		case "VMSSERVERPORT":
			return null;
		default:
			LogManager.Write(FormattableString.Invariant($"The parameter {parameterName} was used in the previous installation, but is not present in the current installation"));
			return null;
		}
	}

	private (Uri ManagementServerAddress, Uri ManagementServerAddressSecure, Uri ManagementServerAddressUnsecure, Uri IdpServerAddress) GetPreviousUsedServers()
	{
		ICertificateQueryModel model = ModelRepository.GetModel<ICertificateQueryModel>();
		bool? secureState = model.GetSecureState();
		if (secureState.HasValue)
		{
			Uri managementServer = model.GetManagementServer(secureState.Value);
			Uri managementServer2 = model.GetManagementServer(secureState: true);
			Uri managementServer3 = model.GetManagementServer(secureState: false);
			Uri authorizationServer = model.GetAuthorizationServer(secureState.Value);
			return (ManagementServerAddress: managementServer, ManagementServerAddressSecure: managementServer2, ManagementServerAddressUnsecure: managementServer3, IdpServerAddress: authorizationServer);
		}
		return (ManagementServerAddress: null, ManagementServerAddressSecure: null, ManagementServerAddressUnsecure: null, IdpServerAddress: null);
	}

	private void SetParameter(string key, string value)
	{
		Parameters[key] = value;
		GlobalData.SetValue(key, value, "UpdateParametersFromInstallInfo()");
	}

	private string GetParameterValue(string key)
	{
		if (InstallInfo != null)
		{
			foreach (KeyValueParameters<string, string> parameter in InstallInfo.InstallEnvironment.Parameters)
			{
				if (parameter.Key == key)
				{
					return parameter.Value;
				}
			}
		}
		return null;
	}

	public void ExtractMSIFiles(string targetDir)
	{
		PrerequisiteManager = new PrerequisiteManager(ClMsiHandler, InstallerConfig, PackagesParameters, CurrentDirectory);
		if (InstallerConfig.InstallerPrerequisites != null && InstallerConfig.InstallerPrerequisites.InstallerPrerequisite != null)
		{
			InstallerPrerequisiteCollectionType[] installerPrerequisite = InstallerConfig.InstallerPrerequisites.InstallerPrerequisite;
			foreach (InstallerPrerequisiteCollectionType installerPrerequisiteCollectionType in installerPrerequisite)
			{
				if (installerPrerequisiteCollectionType != null && installerPrerequisiteCollectionType.PrerequisiteInfo != null && installerPrerequisiteCollectionType.PrerequisiteInfo.InstallationFolder != null)
				{
					ExtractFolder(targetDir, installerPrerequisiteCollectionType.PrerequisiteInfo.InstallationFolder);
				}
			}
		}
		PrerequisiteManager.Unpack(targetDir, null);
	}

	private void ExtractFolder(string targetDir, string folderName)
	{
		CopyDirectory(Path.Combine(CurrentDirectory, folderName), Path.Combine(targetDir, folderName));
	}

	public double GetFreeSpaceBytes(string targetDir)
	{
		if (targetDir == null)
		{
			targetDir = Parameters["TARGETDIR"];
		}
		if (!string.IsNullOrEmpty(targetDir))
		{
			if (NativeMethods.GetDiskFreeSpaceEx(Path.GetPathRoot(targetDir), out var lpFreeBytesAvailable, out var _, out var _) == 0)
			{
				throw new Win32Exception((int)NativeMethods.GetLastError());
			}
			SpaceAvailableOnDrive = Math.Round((double)lpFreeBytesAvailable / Math.Pow(1024.0, 2.0), 1);
			return lpFreeBytesAvailable;
		}
		return 0.0;
	}

	public void ModeChanged(bool isModifyMode)
	{
		_isModify = isModifyMode;
	}

	public void DeleteFiles(string path)
	{
		DirectoryInfo directoryInfo = new DirectoryInfo(path);
		FileInfo[] files = directoryInfo.GetFiles();
		foreach (FileInfo fileInfo in files)
		{
			try
			{
				fileInfo.Attributes &= ~FileAttributes.ReadOnly;
				fileInfo.Delete();
			}
			catch (Exception)
			{
			}
		}
		DirectoryInfo[] directories = directoryInfo.GetDirectories();
		foreach (DirectoryInfo directoryInfo2 in directories)
		{
			try
			{
				files = directoryInfo2.GetFiles();
				for (int j = 0; j < files.Length; j++)
				{
					files[j].Attributes &= ~FileAttributes.ReadOnly;
				}
				directoryInfo2.Attributes &= ~FileAttributes.ReadOnly;
				directoryInfo2.Delete(recursive: true);
			}
			catch (Exception)
			{
			}
		}
		LogManager.Write("Files and folders were deleted from " + path);
	}

	public string SetupFinishParams()
	{
		string result = null;
		string text = (Parameters.ContainsKey("TRECK_ID") ? Parameters["TRECK_ID"] : null);
		if (text != null && InstallerConfig.InstallerSettings.FlowSettings20 != null && InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings != null && InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks != null)
		{
			TreckType[] flowTrecks = InstallerConfig.InstallerSettings.FlowSettings20.AdvancePageSettings.FlowTrecks;
			foreach (TreckType treckType in flowTrecks)
			{
				if (!(text == treckType.Id))
				{
					continue;
				}
				if (treckType.FinishSetup == null)
				{
					break;
				}
				if (treckType.FinishSetup.RunApp != null)
				{
					bool flag = true;
					if (treckType.FinishSetup.RunApp.DependentPackage != null && !string.IsNullOrEmpty(treckType.FinishSetup.RunApp.DependentPackage.ID))
					{
						if (SequenceManager.InstalledPackages != null)
						{
							flag = SequenceManager.InstalledPackages.Contains(treckType.FinishSetup.RunApp.DependentPackage.ID);
							result = treckType.FinishSetup.RunApp.DependentPackage.ID;
						}
						else
						{
							flag = false;
						}
					}
					if (treckType.FinishSetup.RunApp.AppExec != null && flag)
					{
						Parameters["Run Application"] = treckType.FinishSetup.RunApp.AppExec;
					}
					if (treckType.FinishSetup.RunApp.AppTitle != null && flag)
					{
						Parameters["Run Application Title"] = treckType.FinishSetup.RunApp.AppTitle;
					}
				}
				if (treckType.FinishSetup.OpenLink != null)
				{
					if (treckType.FinishSetup.OpenLink.Link != null)
					{
						Parameters["Getting Started"] = treckType.FinishSetup.OpenLink.Link;
					}
					Parameters["Getting Started Title"] = treckType.FinishSetup.OpenLink.LinkTitle.ToString();
				}
				break;
			}
		}
		if (InstallerConfig.LicenseProvider != null && InstallerConfig.LicenseProvider.LicenseCustomisations != null && InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom != null && Parameters.ContainsKey("PRODUCT-SKU"))
		{
			string strB = Parameters["PRODUCT-SKU"];
			LicenseCustomisationsTypeLicenseCustom[] licenseCustom = InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom;
			foreach (LicenseCustomisationsTypeLicenseCustom licenseCustomisationsTypeLicenseCustom in licenseCustom)
			{
				if (string.Compare(licenseCustomisationsTypeLicenseCustom.ProductKey, strB, StringComparison.OrdinalIgnoreCase) == 0 && licenseCustomisationsTypeLicenseCustom.OpenLinkOnFinish != null && !string.IsNullOrEmpty(licenseCustomisationsTypeLicenseCustom.OpenLinkOnFinish.Link))
				{
					Parameters["Getting Started"] = licenseCustomisationsTypeLicenseCustom.OpenLinkOnFinish.Link;
				}
			}
		}
		return result;
	}

	public ExitCode PerformSSRegistration()
	{
		LogManager.Write($"Location of installer for SS registration: \"{_exeLocation}\"");
		CreateSequenceManager();
		try
		{
			SequenceManager.ServerSideRegistration(_exeLocation, _parentProcessName);
			return ExitCode.Success;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			return ExitCode.Error;
		}
	}

	public bool AnotherInstanceRunning()
	{
		return Process.GetProcessesByName(Process.GetCurrentProcess().ProcessName).Length > 1;
	}

	public static bool HasFipsEnabledOnUpgradeFromNonFipsCompliantVersion()
	{
		if (CryptoConfig.AllowOnlyFipsAlgorithms)
		{
			return ProductCompliance.ProductComplianceVersion < ProductComplianceVersion.V1;
		}
		return false;
	}

	public void VerifyParameters()
	{
		if (Parameters.ContainsKey("SERVICEACCOUNT"))
		{
			bool canBeLoc = true;
			ServiceAccountItem serviceAccountItem = new ServiceAccountItem(Parameters["SERVICEACCOUNT"], out canBeLoc);
			if (canBeLoc)
			{
				Parameters["SERVICEACCOUNT"] = serviceAccountItem.AccountName;
			}
		}
	}

	public InstallerPrerequisitesTypeSystemDetections DetectSystems(ref string detectedProName, ref string detectedVersion)
	{
		if (InstallerConfig.InstallerPrerequisites.SystemDetections != null)
		{
			InstallerPrerequisitesTypeSystemDetections[] systemDetections = InstallerConfig.InstallerPrerequisites.SystemDetections;
			foreach (InstallerPrerequisitesTypeSystemDetections installerPrerequisitesTypeSystemDetections in systemDetections)
			{
				if (!string.IsNullOrEmpty(installerPrerequisitesTypeSystemDetections.UpgradeCode) && DetectSystem(installerPrerequisitesTypeSystemDetections.UpgradeCode, ref detectedProName, ref detectedVersion))
				{
					return installerPrerequisitesTypeSystemDetections;
				}
			}
		}
		return null;
	}

	public bool DetectSystem(string upgradeCode, ref string detectedProName, ref string detectedVersion)
	{
		StringBuilder upgradeCode2 = new StringBuilder(upgradeCode);
		StringBuilder stringBuilder = ClMsiHandler.EnumRelatedProducts(upgradeCode2, 0u);
		if (stringBuilder != null && stringBuilder.Length > 0)
		{
			detectedProName = GetProductProperty(stringBuilder.ToString(), "DisplayName");
			object productProperty = GetProductProperty(stringBuilder.ToString(), "VersionMajor");
			object productProperty2 = GetProductProperty(stringBuilder.ToString(), "VersionMinor");
			if (productProperty != null)
			{
				int num = Convert.ToInt32(productProperty);
				int num2 = ((productProperty2 != null) ? Convert.ToInt32(productProperty2) : 0);
				detectedVersion = $"{num}.{num2}";
			}
			return true;
		}
		return false;
	}

	public bool IsWindowsVersionSupported(IOSInformationReader reader)
	{
		if (InstallerConfig.InstallerPrerequisites != null && InstallerConfig.InstallerPrerequisites.WinVersion != null)
		{
			OSInformation oSInformation = reader?.Read();
			if (oSInformation == null)
			{
				LogManager.Write("Failed while reading the OS version from the environment", TraceEventType.Error);
				return false;
			}
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Version read from the environment: {0}, product type: {1}", oSInformation.OSVersion, oSInformation.ProductType));
			WinVersionType winVersion = InstallerConfig.InstallerPrerequisites.WinVersion;
			if (winVersion.ExcludeBelow10_0 && oSInformation.OSVersion.CompareTo(new Version("10.0")) < 0)
			{
				LogManager.Write("Unable to install, OS version is not supported.");
				return false;
			}
			if (winVersion.VersionsToExclude != null && winVersion.VersionsToExclude.Length != 0)
			{
				WinVersionTypeVersionsToExclude[] versionsToExclude = winVersion.VersionsToExclude;
				foreach (WinVersionTypeVersionsToExclude winVersionTypeVersionsToExclude in versionsToExclude)
				{
					if (Version.TryParse(winVersionTypeVersionsToExclude.Value, out var result))
					{
						if (oSInformation.OSVersion.Major == result.Major && oSInformation.OSVersion.Minor == result.Minor && (oSInformation.OSVersion.Build == result.Build || result.Build == -1) && (oSInformation.OSVersion.Revision == result.Revision || result.Revision == -1) && (winVersionTypeVersionsToExclude.ProductType == WinVersionTypeVersionsToExcludeProductType.All || winVersionTypeVersionsToExclude.ProductType == oSInformation.ProductType))
						{
							LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Excluded OS version detected, exclusion details: {0}, {1}", result, winVersionTypeVersionsToExclude.ProductType));
							return false;
						}
					}
					else
					{
						LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Version in the WinVersion/VersionsToExclude has wrong format ({0}) Should be major.minor.build.revision", winVersionTypeVersionsToExclude));
					}
				}
			}
		}
		return true;
	}

	public void SetupWhyUninstallParams()
	{
		if (InstallerConfig.LicenseProvider == null || InstallerConfig.LicenseProvider.LicenseCustomisations == null || InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom == null || !Parameters.ContainsKey("PRODUCT-SKU"))
		{
			return;
		}
		string strB = Parameters["PRODUCT-SKU"];
		LicenseCustomisationsTypeLicenseCustom[] licenseCustom = InstallerConfig.LicenseProvider.LicenseCustomisations.LicenseCustom;
		foreach (LicenseCustomisationsTypeLicenseCustom licenseCustomisationsTypeLicenseCustom in licenseCustom)
		{
			if (string.Compare(licenseCustomisationsTypeLicenseCustom.ProductKey, strB, StringComparison.OrdinalIgnoreCase) == 0 && licenseCustomisationsTypeLicenseCustom.ShowWhyUninstalling != null && !string.IsNullOrEmpty(licenseCustomisationsTypeLicenseCustom.ShowWhyUninstalling.Link))
			{
				Parameters["Why Uninstalling"] = licenseCustomisationsTypeLicenseCustom.ShowWhyUninstalling.Link;
			}
		}
	}

	public void ResetInstallerManager()
	{
		_instance = new InstallerManager();
	}

	public void AddNotInstalledPackages()
	{
		Dictionary<string, ConditionType> dictionary = new Dictionary<string, ConditionType>();
		foreach (PackageRefType notInstalledPackage in InstallInfo.NotInstalledPackages)
		{
			dictionary[notInstalledPackage.ID] = notInstalledPackage.Condition;
		}
		PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (dictionary.ContainsKey(packageRefType.ID))
			{
				if (dictionary[packageRefType.ID] != ConditionType.Skip && dictionary[packageRefType.ID] != ConditionType.Disabled)
				{
					packageRefType.Condition = ConditionType.Optional;
				}
				else
				{
					packageRefType.Condition = ConditionType.Skip;
				}
			}
		}
		installExecuteSequence = InstallerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType2 in installExecuteSequence)
		{
			if (dictionary.ContainsKey(packageRefType2.ID))
			{
				if (dictionary[packageRefType2.ID] != ConditionType.Skip && dictionary[packageRefType2.ID] != ConditionType.Disabled)
				{
					packageRefType2.Condition = ConditionType.Optional;
				}
				else
				{
					packageRefType2.Condition = ConditionType.Skip;
				}
			}
		}
	}

	public void SkipInstalledPackages()
	{
		Dictionary<string, PackageRefType> dictionary = new Dictionary<string, PackageRefType>();
		foreach (PackageRefType installedPackage in InstallInfo.InstalledPackages)
		{
			dictionary.Add(installedPackage.ID.ToUpperInvariant(), installedPackage);
		}
		string text = string.Empty;
		if (dictionary.Count != 0)
		{
			text = dictionary.Last().Key;
		}
		PackageRefType[] installExecuteSequence = InstallerConfig.InstallExecuteSequence;
		foreach (PackageRefType packageRefType in installExecuteSequence)
		{
			if (dictionary.ContainsKey(packageRefType.ID.ToUpperInvariant()))
			{
				if (text == packageRefType.ID.ToUpperInvariant())
				{
					packageRefType.RunOnlyPlugin = true;
				}
				if (dictionary[packageRefType.ID.ToUpperInvariant()].Condition == ConditionType.Finalize)
				{
					packageRefType.Condition = ConditionType.Finalize;
				}
				else
				{
					packageRefType.Condition = ConditionType.Skip;
				}
			}
			else
			{
				packageRefType.Condition = ConditionType.Optional;
			}
		}
		installExecuteSequence = InstallerConfig.UninstallerInfo.UninstallerSequencer;
		foreach (PackageRefType packageRefType2 in installExecuteSequence)
		{
			if (dictionary.ContainsKey(packageRefType2.ID.ToUpperInvariant()))
			{
				packageRefType2.Condition = ConditionType.Skip;
			}
			else
			{
				packageRefType2.Condition = ConditionType.Optional;
			}
		}
	}

	public void DoNotSaveParameterInInstallerInfo(IEnumerable<string> parameters)
	{
		if (parameters == null)
		{
			throw new ArgumentNullException("parameters");
		}
		foreach (string parameter in parameters)
		{
			_parametersNotSaved.Add(parameter);
		}
	}
}
