using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using VideoOS.ConfigurationAPI;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class ConfigureHardwareViewModel : ProgressViewModel
{
	private int _port = 80;

	private HardwareConfigurations _configuredHardwareConfigurations = new HardwareConfigurations();

	public static bool OperationCanBeStopped => false;

	private HardwareConfigurations ConfiguredHardwareConfigurations
	{
		get
		{
			return _configuredHardwareConfigurations;
		}
		set
		{
			_configuredHardwareConfigurations = value;
		}
	}

	public ConfigureHardwareViewModel()
	{
		InitServerPort();
	}

	public void SetConfiguredHardware(HardwareConfigurations hardwareCollection)
	{
		ConfiguredHardwareConfigurations = hardwareCollection;
	}

	public void InitStartConfiguringHardware(object sender, DoWorkEventArgs e)
	{
		EnableAndRenameDevices();
		CreateViewGroups();
		CreateAndGetRoles();
		OnlineActivate();
	}

	private void OnlineActivate()
	{
		if (ConfiguredHardwareConfigurations.ToArray().Length != 0)
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Starting online activation..."), TraceEventType.Information);
			ActivateLicensePlugin activateLicensePlugin = new ActivateLicensePlugin(_port);
			activateLicensePlugin.ProgressChanged += ActivateLicensePlugin_ProgressChanged;
			if (activateLicensePlugin.DoOnlineActivation())
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Online activation succeeded"), TraceEventType.Information);
			}
			else
			{
				LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Online activation failed"), TraceEventType.Information);
			}
			activateLicensePlugin.ProgressChanged -= ActivateLicensePlugin_ProgressChanged;
		}
	}

	private void CreateAndGetRoles()
	{
		RolePlugin rolePlugin = new RolePlugin(_port);
		using ConfigurationApiServiceManager serviceManager = new ConfigurationApiServiceManager(_port);
		rolePlugin.ProgressChanged += OperatorRolePlugin_ProgressChanged;
		if (rolePlugin.DoCreateOperatorRole(serviceManager))
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Operator role created."), TraceEventType.Information);
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Create Operator role failed."), TraceEventType.Information);
		}
		ConfigurationItem[] roles = rolePlugin.GetRoles(serviceManager);
		if (roles != null)
		{
			string value = SystemConfigurationHelper.ConvertCollectionToCsvString(roles.Select((ConfigurationItem f) => f.Properties.Single((Property p) => p.Key.Equals("Name")).Value));
			GlobalData.SetValue(ServerParameters.Roles.ToString(), value, "SystemConfigurationPlugin.InitStartConfiguringHardware");
		}
		else
		{
			LogManager.Write(string.Format(CultureInfo.InvariantCulture, "No roles where found."), TraceEventType.Information);
		}
		rolePlugin.ProgressChanged -= OperatorRolePlugin_ProgressChanged;
		rolePlugin = null;
	}

	private void CreateViewGroups()
	{
		CreateViewPlugin createViewPlugin = new CreateViewPlugin(_port);
		createViewPlugin.ProgressChanged += CameraViewPlugin_ProgressChanged;
		createViewPlugin.Start(ConfiguredHardwareConfigurations.SelectMany((HardwareConfiguration hardware) => hardware.Devices.Where((DeviceConfiguration device) => device.GetType() == typeof(CameraConfiguration) && device.Enabled)));
		LogManager.Write(string.Format(CultureInfo.InvariantCulture, "Adding view finished."), TraceEventType.Information);
		createViewPlugin.ProgressChanged -= CameraViewPlugin_ProgressChanged;
	}

	private void EnableAndRenameDevices()
	{
		AddHardwarePlugin addHardwarePlugin = new AddHardwarePlugin(_port);
		addHardwarePlugin.ProgressChanged += HardwarePlugin_ProgressChanged;
		using (ConfigurationApiServiceManager serviceManager = new ConfigurationApiServiceManager(_port))
		{
			addHardwarePlugin.EnableAndRenameDevices(serviceManager, ConfiguredHardwareConfigurations);
		}
		addHardwarePlugin.ProgressChanged -= HardwarePlugin_ProgressChanged;
		addHardwarePlugin = null;
	}

	private void InitServerPort()
	{
		if (GlobalData.TryGetValue(ServerParameters.ServerPort.ToString(), out var value) && !int.TryParse(value, out _port))
		{
			_port = 80;
		}
	}

	private void HardwarePlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}

	private void ActivateLicensePlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}

	private void OperatorRolePlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}

	private void CameraViewPlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}
}
