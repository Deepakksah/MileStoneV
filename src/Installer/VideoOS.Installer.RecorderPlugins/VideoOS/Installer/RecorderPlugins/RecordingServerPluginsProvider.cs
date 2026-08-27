using System;
using System.Collections.Generic;
using VideoOS.Installer.Interfaces;

namespace VideoOS.Installer.RecorderPlugins;

public sealed class RecordingServerPluginsProvider : IWizardPageProvider, IDisposable
{
	private ServerConfigurationControl _configPage = new ServerConfigurationControl();

	private ServerUninstallControl _uninstallPage = new ServerUninstallControl();

	private ServerServiceAccountControl _serviceAccountPage = new ServerServiceAccountControl();

	private RecorderMultiInstanceValidationControl _recorderMultiInstanceValidationControl = new RecorderMultiInstanceValidationControl();

	public List<IWizardPage> GetWizardPages()
	{
		return new List<IWizardPage> { _configPage, _serviceAccountPage };
	}

	public IWizardPage CreateInstance(string name)
	{
		return name switch
		{
			"ServerConfigurationPage" => _configPage, 
			"ServerUninstallPage" => _uninstallPage, 
			"ServerServiceAccountPage" => _serviceAccountPage, 
			"RecorderMultiInstanceValidationPage" => _recorderMultiInstanceValidationControl, 
			_ => null, 
		};
	}

	public List<IWizardPage> GetWizardPagesUninstallation()
	{
		return new List<IWizardPage> { _uninstallPage };
	}

	public void Dispose()
	{
		_serviceAccountPage.Dispose();
		_uninstallPage.Dispose();
		_configPage.Dispose();
		_recorderMultiInstanceValidationControl.Dispose();
	}
}
