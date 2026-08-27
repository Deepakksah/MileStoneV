using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Microsoft.Win32;
using VideoOS.Installer.Configuration;
using VideoOS.Installer.Configuration.Package;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Interfaces.Msi;
using VideoOS.Installer.Logging;
using VideoOS.Installer.Sequencer.Properties;

namespace VideoOS.Installer.Sequencer.Prerequisite;

public class SSRegistrationTask : InstallerTask
{
	private readonly string[] _ssRegiastrationFiles = new string[5] { "SSCMRegister.exe", "SSCMStringLibrary.dll", "SSCM.Interface.dll", "SSCM.Interface.Retriever.dll", "SSCM.Interface1_1.dll" };

	private const string DOWNLOADMANAGER_UPGRADECODE = "{2E92ADF9-E395-45A4-9EC3-3E9832779E48}";

	private List<InstallerPackageType> _installerPackages;

	private string _exeLocation;

	private string _parentProcessName;

	private bool _showErrorMessage;

	private string _ssRegLocation;

	private bool _selfRegistration;

	private IClMsiHandler _handler;

	private string TargetRegistrationToolLocation
	{
		get
		{
			if (_exeLocation != null)
			{
				return _exeLocation;
			}
			return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		}
	}

	private string SSRegLocation
	{
		get
		{
			if (_ssRegLocation == null)
			{
				new StringBuilder();
				if (_handler.EnumRelatedProducts(new StringBuilder("{2E92ADF9-E395-45A4-9EC3-3E9832779E48}"), 0u) != null)
				{
					_ssRegLocation = GetDmLocationFromRegistry();
				}
				else
				{
					LogManager.Write("Download Manager is not installed on the machine");
				}
			}
			return _ssRegLocation;
		}
	}

	private string GetDmLocationFromRegistry()
	{
		string text = "SOFTWARE\\VideoOS\\SSCM";
		string name = "InstalledPath";
		string text2 = null;
		using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(text))
		{
			if (registryKey != null)
			{
				text2 = (string)registryKey.GetValue(name);
				registryKey.Close();
			}
		}
		if (string.IsNullOrEmpty(text2))
		{
			string name2 = "";
			string text3 = text.ToLower();
			if (text3.StartsWith("software") && !text3.StartsWith("software\\wow6432node"))
			{
				name2 = Regex.Replace(text3, "^software(.*)", "software\\wow6432node$1");
			}
			using RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(name2);
			if (registryKey2 != null)
			{
				text2 = (string)registryKey2.GetValue(name);
				registryKey2.Close();
			}
		}
		return text2;
	}

	public SSRegistrationTask(IClMsiHandler handler, List<InstallerPackageType> installerPackages)
	{
		_installerPackages = CreateFullSSRegistrationList(installerPackages);
		base.Id = "SSREGISTRATION";
		base.DisplayName = Resources.SSRegistrationTask_DisplayName;
		_handler = handler;
	}

	public SSRegistrationTask(IClMsiHandler handler, List<InstallerPackageType> installerPackages, string exeLocation, string parentProcessName, bool selfRegistration)
		: this(handler, installerPackages)
	{
		_exeLocation = exeLocation;
		_parentProcessName = parentProcessName;
		_showErrorMessage = _exeLocation != null;
		_selfRegistration = selfRegistration;
	}

	private List<InstallerPackageType> CreateFullSSRegistrationList(List<InstallerPackageType> installerPackages)
	{
		List<InstallerPackageType> list = new List<InstallerPackageType>();
		list.AddRange(installerPackages);
		string directoryName = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
		foreach (InstallerPackageType installerPackage in installerPackages)
		{
			if (installerPackage.ProductManifest64 != null)
			{
				string filename = ((!(installerPackage.ProductManifest.Platform == "x64")) ? installerPackage.ProductManifest64.Filename : installerPackage.ProductManifest.Filename);
				InstallerPackageTypeProductManifest installerPackageTypeProductManifest = new InstallerPackageTypeProductManifest();
				installerPackageTypeProductManifest.ManifestData = InstallerConfigManager.LoadPackageFromFile(filename, directoryName);
				installerPackageTypeProductManifest.Filename = filename;
				InstallerPackageType installerPackageType = new InstallerPackageType();
				installerPackageType.ProductManifest = installerPackageTypeProductManifest;
				installerPackageType.Name = ((installerPackage.ProductManifest.Platform == "x64") ? installerPackage.Name : (installerPackage.Name + "x64"));
				list.Add(installerPackageType);
			}
		}
		return list;
	}

	private void GenerateArgsFile(InstallerPackageType package)
	{
		if (package.ProductManifest.PackageData.SSRegistrationArgs != null && package.ProductManifest.PackageData.SSRegistrationArgs.args != null)
		{
			SerializeObject(ConvertToArgs(package.ProductManifest.PackageData.SSRegistrationArgs.args), package.Name);
		}
	}

	private args ConvertToArgs(argsType args)
	{
		args args2 = new args();
		args2.displayname = args.displayname;
		args2.downloadpage = args.downloadpage;
		args2.group = args.group;
		args2.language = args.language;
		args2.platform = args.platform;
		args2.product = args.product;
		if (!string.IsNullOrEmpty(_parentProcessName))
		{
			args2.referencedfile = _parentProcessName + ".exe";
			args2.sourcefiles = _parentProcessName + ".exe";
		}
		else
		{
			args2.referencedfile = args.referencedfile;
			args2.sourcefiles = args.sourcefiles;
		}
		args2.showwithalllanguages = args.showwithalllanguages;
		args2.version = args.version;
		return args2;
	}

	private void SerializeObject(args args, string name)
	{
		string text = Path.Combine(TargetRegistrationToolLocation, "args.xml");
		LogManager.Write("SSRegistration: Serialize args.xml. Path='" + text + "'");
		try
		{
			XmlSerializerNamespaces xmlSerializerNamespaces = new XmlSerializerNamespaces();
			xmlSerializerNamespaces.Add("", "");
			XmlSerializer xmlSerializer = new XmlSerializer(typeof(args));
			TextWriter textWriter = new StreamWriter(text, append: false);
			xmlSerializer.Serialize(textWriter, args, xmlSerializerNamespaces);
			textWriter.Close();
			LogManager.Write($"Args.xml were generated for {name}", TraceEventType.Information);
		}
		catch (InvalidOperationException ex)
		{
			LogManager.Write($"Args.xml for {name} failed to generate. Error details: {ex.TypeAndMessage()}", TraceEventType.Critical);
			throw;
		}
	}

	protected override void DoRun()
	{
		Status = TaskStatus.Running;
		base.Message = string.Format(Resources.SSRegistrationTask_DisplayName);
		LogManager.Write(base.Message);
		base.MaximumProgress = 100;
		base.CurrentProgress = 0;
		NotifyObservers();
		int num = base.MaximumProgress / _installerPackages.Count;
		base.CurrentProgress++;
		CopyToExeFolder();
		foreach (InstallerPackageType installerPackage in _installerPackages)
		{
			GenerateArgsFile(installerPackage);
			int num2 = RunSSCMRegistrationTool();
			string text = $"{installerPackage.Name} was registered. Exit code: {num2.ToString()}";
			if (num2 != 0 && _showErrorMessage)
			{
				throw new Exception(text);
			}
			LogManager.Write(text);
			base.CurrentProgress += num;
			NotifyObservers();
		}
		CleanupFiles();
		base.MaximumProgress = 0;
		Status = TaskStatus.Completed;
		NotifyObservers();
		_result = TaskResult.Success;
	}

	private int RunSSCMRegistrationTool()
	{
		try
		{
			if (_exeLocation != null && File.Exists("args.xml"))
			{
				File.Copy("args.xml", Path.Combine(TargetRegistrationToolLocation, "args.xml"), overwrite: true);
			}
			Process process = new Process();
			process.StartInfo.FileName = Path.Combine(TargetRegistrationToolLocation, Settings.Default.SSCMRegisterFileName);
			process.StartInfo.Arguments = "startsuppress SSCMRegister.temp";
			process.Start();
			process.WaitForExit();
			_ = process.ExitCode;
			process.StartInfo.Arguments = "";
			process.Start();
			process.WaitForExit();
			_ = process.ExitCode;
			process.StartInfo.Arguments = "stopsuppresssilent SSCMRegister.temp";
			process.Start();
			process.WaitForExit();
			int exitCode = process.ExitCode;
			if (File.Exists("args.xml"))
			{
				File.Delete("args.xml");
			}
			File.Delete(Path.Combine(TargetRegistrationToolLocation, "args.xml"));
			return exitCode;
		}
		catch (Exception exception)
		{
			LogManager.Write(exception);
			return -1;
		}
	}

	private void CleanupFiles()
	{
		string[] ssRegiastrationFiles = _ssRegiastrationFiles;
		foreach (string path in ssRegiastrationFiles)
		{
			try
			{
				File.Delete(Path.Combine(TargetRegistrationToolLocation, path));
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
		string path2 = Path.Combine(TargetRegistrationToolLocation, "args.xml");
		if (File.Exists(path2))
		{
			File.Delete(path2);
		}
	}

	private void CopyToExeFolder()
	{
		LogManager.Write("SSRegistration: SSRegLocation=" + SSRegLocation + ".");
		LogManager.Write("SSRegistration: TargetRegistrationToolLocation=" + TargetRegistrationToolLocation + ".");
		string[] ssRegiastrationFiles = _ssRegiastrationFiles;
		foreach (string path in ssRegiastrationFiles)
		{
			try
			{
				string sourceFileName = Path.Combine(SSRegLocation, path);
				string destFileName = Path.Combine(TargetRegistrationToolLocation, path);
				File.Copy(sourceFileName, destFileName, overwrite: true);
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
	}

	protected override void PreRun()
	{
	}

	protected override void PostRun()
	{
		if (_selfRegistration)
		{
			return;
		}
		foreach (InstallerPackageType installerPackage in _installerPackages)
		{
			if (installerPackage.ProductManifest.PackageData.SSRegistrationArgs != null && installerPackage.ProductManifest.PackageData.SSRegistrationArgs.args != null)
			{
				try
				{
					File.Delete(Path.Combine(TargetRegistrationToolLocation, installerPackage.ProductManifest.PackageData.SSRegistrationArgs.args.sourcefiles));
				}
				catch (Exception exception)
				{
					LogManager.Write(exception);
				}
			}
		}
	}
}
