using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Linq;
using Newtonsoft.Json.Linq;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.DataCollector.Plugin;

public class DataCollectorTaskInstall : MSIInstallerTask, IInstallationAdapter
{
	private Dictionary<string, string> _installParameters;

	private JObject _oldAppsettingsPathToFile;

	private DataCollectorConfigurator _configurator;

	public override bool RunInFinalizeMode => true;

	public override bool RunInHardwareManufacturerMode => true;

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		if (installParameters != null)
		{
			_parameters = new List<IPackageParameter>(installParameters);
		}
		_installParameters = _parameters?.ToDictionary((IPackageParameter item) => item.Target, (IPackageParameter item) => item.Value);
		_configurator = new DataCollectorConfigurator(_installParameters);
	}

	[SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists", Justification = "Implementing existing interface")]
	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void PreRun()
	{
		LogManager.Write("DataCollectorTaskInstall::PreRun(): Configuration pre-run task  installation started");
		base.PreRun();
		string appsettingsPathToFile = DataCollectorUpdater.GetAppsettingsPathToFile(_installParameters["INSTALLDIR"]);
		if (appsettingsPathToFile != null)
		{
			try
			{
				string text = File.ReadAllText(appsettingsPathToFile);
				_oldAppsettingsPathToFile = JObject.Parse(text);
			}
			catch (Exception arg)
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Exception occurred.\r\nException: {0}", arg));
			}
		}
	}

	protected override void DoRun()
	{
		LogManager.Write("DataCollectorTaskInstall::DoRun(): Configuration task installation started");
		bool valueAsBoolean = GlobalData.GetValueAsBoolean("FINALIZE-MODE");
		bool valueAsBoolean2 = GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE");
		if (!base.RunOnlyPlugin && !valueAsBoolean)
		{
			base.DoRun();
		}
		else
		{
			_result = TaskResult.Success;
		}
		if (Result == TaskResult.Success)
		{
			try
			{
				base.Message = DefaultMessage;
				NotifyObservers();
				_configurator.RunInstall(valueAsBoolean2, _oldAppsettingsPathToFile);
				LogManager.Write("DataCollectorTaskInstall::DoRun(): Configuration task installation finished");
				return;
			}
			catch (Exception ex)
			{
				throw new InstallationAdapterException("An error has occurred while running the DataCollector plugin during installation", ex);
			}
		}
		throw LastError;
	}
}
