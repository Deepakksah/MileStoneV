using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Installer.Common;
using VideoOS.Installer.Core.Task;
using VideoOS.Installer.Interfaces;
using VideoOS.Installer.Logging;
using VideoOS.Installer.MSI;
using VideoOS.Installer.ServerConfiguratorPlugin.Properties;

namespace VideoOS.Installer.ServerConfiguratorPlugin;

public class ConfigurationTaskInstall : MSIInstallerTask, IInstallationAdapter
{
	private ServerConfigurator _configurator;

	public override bool RunInFinalizeMode => true;

	public override bool RunInHardwareManufacturerMode => true;

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
		string value = string.Empty;
		if (!GlobalData.TryGetValue("PRODUCT-SKU", out value))
		{
			LogManager.Write("Getting ProductSku failed.", TraceEventType.Error);
		}
		_configurator = new ServerConfigurator(installParameters, installation: true, value);
		_parameters = installParameters;
		_parameters?.Add(new PackageParameter("WEBAPIPORT", ServerConfigFileUpdater.GetPortFromConfigFile()));
	}

	public void Uninstall(string fileName, List<IPackageParameter> installParameters)
	{
	}

	protected override void DoRun()
	{
		LogManager.Write("DoRun(): Configuration task installation started");
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
				base.MaximumProgress = _configurator.MaxProgress;
				base.Message = DefaultMessage;
				NotifyObservers();
				_configurator.OnProgressChanged += _configurator_OnProgressChanged;
				_configurator.RunInstall(valueAsBoolean2, valueAsBoolean);
				return;
			}
			catch (Exception ex)
			{
				throw new InstallationAdapterException(Resources.ConfigurationPlugin_ErrorOccured, ex);
			}
			finally
			{
				_configurator.OnProgressChanged -= _configurator_OnProgressChanged;
			}
		}
		throw LastError;
	}

	private void _configurator_OnProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		base.CurrentProgress = e.Progress;
		NotifyObservers();
	}

	protected override void PostRun()
	{
		base.PostRun();
		if (GlobalData.GetValueAsBoolean("HARDWARE-MANUFACTURER-MODE"))
		{
			return;
		}
		LogManager.Write("PostRun(): Configuration task waiting for service initialization to complete");
		if (!GlobalData.TryGetValue("MANAGEMENTSERVERURI", out var value))
		{
			LogManager.Write("PostRun(): Missing parameter: MANAGEMENTSERVERURI", TraceEventType.Error);
			throw new InstallationAdapterException(Resources.ConfigurationPlugin_ErrorOccured);
		}
		using ManagementServerEndpointVerifier managementServerEndpointVerifier = new ManagementServerEndpointVerifier(new Uri(value));
		bool result = managementServerEndpointVerifier.WaitForServiceAvailabilityAsync(TimeSpan.FromMinutes(2.0)).Result;
		LogManager.Write("PostRun(): Wait for service availablity responded: " + result);
		if (!result)
		{
			LogManager.Write("PostRun(): Management Server did not respond in a timely fashion", TraceEventType.Error);
			throw new InstallationAdapterException(Resources.ConfigurationPlugin_ErrorOccured);
		}
	}
}
