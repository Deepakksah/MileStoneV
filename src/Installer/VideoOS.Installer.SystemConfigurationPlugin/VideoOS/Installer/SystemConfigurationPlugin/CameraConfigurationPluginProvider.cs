using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public sealed class CameraConfigurationPluginProvider : IWizardPageProvider, IDisposable
{
	private CameraUserNamePasswordWizard _cameraUserNamePasswordWizard = new CameraUserNamePasswordWizard();

	private LicenseWizard _licenseWizard = new LicenseWizard();

	private InstallationFinishWizardPage _finishInstallationWizard = new InstallationFinishWizardPage();

	private ConfigurationFinishWizardPage _finishConfigurationWizard = new ConfigurationFinishWizardPage();

	private ScanHardwareWizard _scanHardwareWizard = new ScanHardwareWizard();

	private AddHardwareWizard _addHardwareWizard = new AddHardwareWizard();

	private AddUsersWizard _addUsersWizard = new AddUsersWizard();

	private HardwareSelectionWizard _hardwareSelectionWizard = new HardwareSelectionWizard();

	private HardwareConfigurationWizard _hardwareConfigurationPage = new HardwareConfigurationWizard();

	private ConfigureHardwareProgressWizard _configureHardwareProgressWizard = new ConfigureHardwareProgressWizard();

	public IWizardPage CreateInstance(string name)
	{
		return name switch
		{
			"CameraUserNamePasswordPage" => _cameraUserNamePasswordWizard, 
			"LicensePage" => _licenseWizard, 
			"FinishInstallationPage" => _finishInstallationWizard, 
			"FinishConfigurationPage" => _finishConfigurationWizard, 
			"ScanHardwarePage" => _scanHardwareWizard, 
			"AddHardwarePage" => _addHardwareWizard, 
			"HardwareSelectionPage" => _hardwareSelectionWizard, 
			"HardwareConfigurationPage" => _hardwareConfigurationPage, 
			"ConfigureHardwareProgressPage" => _configureHardwareProgressWizard, 
			"AddUsersPage" => _addUsersWizard, 
			_ => null, 
		};
	}

	public List<IWizardPage> GetWizardPages()
	{
		return new List<IWizardPage> { _cameraUserNamePasswordWizard, _licenseWizard, _scanHardwareWizard, _addHardwareWizard, _finishInstallationWizard, _hardwareSelectionWizard, _finishConfigurationWizard, _configureHardwareProgressWizard, _addUsersWizard };
	}

	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return new List<IWizardPage>();
	}

	public void Dispose()
	{
		if (_cameraUserNamePasswordWizard != null)
		{
			_cameraUserNamePasswordWizard.Dispose();
		}
		if (_licenseWizard != null)
		{
			_licenseWizard.Dispose();
		}
		if (_finishInstallationWizard != null)
		{
			_finishInstallationWizard.Dispose();
		}
		if (_finishConfigurationWizard != null)
		{
			_finishConfigurationWizard.Dispose();
		}
		if (_scanHardwareWizard != null)
		{
			_scanHardwareWizard.Dispose();
		}
		if (_addHardwareWizard != null)
		{
			_addHardwareWizard.Dispose();
		}
		if (_configureHardwareProgressWizard != null)
		{
			_configureHardwareProgressWizard.Dispose();
		}
		if (_addUsersWizard != null)
		{
			_addUsersWizard.Dispose();
		}
		if (_hardwareConfigurationPage != null)
		{
			_hardwareConfigurationPage.Dispose();
		}
		if (_hardwareSelectionWizard != null)
		{
			_hardwareSelectionWizard.Dispose();
		}
	}
}
