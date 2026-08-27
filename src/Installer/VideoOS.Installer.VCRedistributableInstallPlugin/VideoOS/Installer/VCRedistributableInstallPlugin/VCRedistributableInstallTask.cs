using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Timers;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.VCRedistributableInstallPlugin.Properties;

namespace VideoOS.Installer.VCRedistributableInstallPlugin;

public class VCRedistributableInstallTask : InstallerTask, IInstallationAdapter
{
	private readonly VCRedistributableInstaller[] _exeToRunList = new VCRedistributableInstaller[2]
	{
		new VCRedistributableInstaller("VCREDISTTORUNX64", "x64"),
		new VCRedistributableInstaller("VCREDISTTORUNX86", "x86")
	};

	private readonly int _interval = 4;

	public override bool RunInHardwareManufacturerMode => true;

	public override bool RunInFinalizeMode => false;

	protected override void DoRun()
	{
		if (base.RunOnlyPlugin)
		{
			_result = TaskResult.Success;
			return;
		}
		List<int> list = new List<int>();
		string empty = string.Empty;
		VCRedistributableInstaller[] array = Array.FindAll(_exeToRunList, (VCRedistributableInstaller x) => x.IsDefined() && !x.IsInstalled());
		foreach (VCRedistributableInstaller vCRedistributableInstaller in array)
		{
			empty = vCRedistributableInstaller.DisplayName;
			base.Message = string.Format(CultureInfo.CurrentCulture, Resources.Installing, empty);
			base.CurrentProgress = 0;
			base.MaximumProgress = _interval * 15;
			NotifyObservers();
			using Timer timer = new Timer();
			timer.Elapsed += OnTimedProgressUpdate;
			timer.Interval = 1000.0;
			timer.Enabled = true;
			try
			{
				LogManager.Write("DoRun(): " + empty + " installation started");
				int num2 = vCRedistributableInstaller.RunProcess();
				list.Add(num2);
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "{0} installation exited with code {1}", empty, num2));
			}
			catch (Exception ex)
			{
				throw new InstallationAdapterException(string.Format(CultureInfo.InvariantCulture, Resources.InstallError, empty, ex.Message), ex);
			}
			finally
			{
				timer.Enabled = false;
				base.CurrentProgress = base.MaximumProgress;
				NotifyObservers();
				base.MaximumProgress = 0;
			}
		}
		if (!list.Exists((int exitCode) => exitCode == 1641 || exitCode == 3010))
		{
			return;
		}
		base.NeedsReboot = true;
		throw new InstallationAdapterException("Reboot needed to complete the installation");
	}

	private void OnTimedProgressUpdate(object source, ElapsedEventArgs e)
	{
		if (base.CurrentProgress + _interval < base.MaximumProgress)
		{
			base.CurrentProgress += _interval;
			NotifyObservers();
		}
	}

	private void InitializeExeToRun(IEnumerable<IPackageParameter> installParameters)
	{
		VCRedistributableInstaller[] exeToRunList = _exeToRunList;
		foreach (VCRedistributableInstaller vcRedist in exeToRunList)
		{
			IPackageParameter packageParameter = installParameters.FirstOrDefault((IPackageParameter x) => string.Compare(x.Target, vcRedist.ParameterName, StringComparison.OrdinalIgnoreCase) == 0);
			string installerFile = null;
			if (packageParameter != null)
			{
				installerFile = ((!Path.IsPathRooted(packageParameter.Value)) ? Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), packageParameter.Value) : packageParameter.Value);
			}
			vcRedist.SetInstallerFile(installerFile);
		}
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		InitializeExeToRun(installParameters);
		return Array.TrueForAll(_exeToRunList, (VCRedistributableInstaller x) => !x.IsDefined() || x.IsInstalled());
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
		InitializeExeToRun(installParameters);
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}
}
