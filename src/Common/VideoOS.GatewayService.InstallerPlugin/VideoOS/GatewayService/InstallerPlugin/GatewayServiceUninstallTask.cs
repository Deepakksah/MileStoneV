using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Extensions.Logging;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;

namespace VideoOS.GatewayService.InstallerPlugin;

public class GatewayServiceUninstallTask : MSIUninstallerTask, IInstallationAdapter
{
	public delegate void DoRunBase();

	private IServerConfigurator _configurator;

	private IServerConfiguratorFactory _configuratorFactory;

	private ILogger _log;

	public DoRunBase BaseDoRun { get; set; }

	public GatewayServiceUninstallTask()
		: base(null)
	{
		BaseDoRun = base.DoRun;
		_configuratorFactory = new ServerConfiguratorFactory();
		_log = LogManager.InnerLog;
	}

	public GatewayServiceUninstallTask(IServerConfiguratorFactory serverConfiguratorFactory, ILogger log)
	{
		_configuratorFactory = serverConfiguratorFactory;
		_log = log;
	}

	public bool CheckPrequisite(string fileName, List<IPackageParameter> installParameters)
	{
		return false;
	}

	public void Initialize(List<IPackageParameter> installParameters)
	{
	}

	public void Finalize(List<IPackageParameter> installParameters)
	{
	}

	public void Install(string fileName, List<IPackageParameter> installParameters)
	{
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
		if (installParameters != null)
		{
			_parameters = new List<IPackageParameter>(installParameters);
		}
		_configurator = _configuratorFactory.CreateServerConfigurator(installParameters, installation: false);
	}

	protected override void DoRun()
	{
		LoggerExtensions.LogDebug(_log, string.Format(CultureInfo.InvariantCulture, "DoRun(): Configuration task uninstallation started"), Array.Empty<object>());
		_configurator.RemoveCertificateRegistration();
		BaseDoRun();
		try
		{
			base.MaximumProgress = _configurator.MaxProgress;
			base.Message = DefaultMessage;
			NotifyObservers();
			_configurator.OnProgressChanged += _configurator_OnProgressChanged;
			_configurator.RunUninstall();
		}
		catch (Exception ex)
		{
			LoggerExtensions.LogError(_log, ex.Message, Array.Empty<object>());
			throw new InstallationAdapterException(ex.Message, ex);
		}
		finally
		{
			_configurator.OnProgressChanged -= _configurator_OnProgressChanged;
		}
	}

	private void _configurator_OnProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		base.CurrentProgress = e.Progress;
		NotifyObservers();
	}
}
