using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using VideoOS.Installer.Common;
using VideoOS.Installer.Common.Utilities;
using VideoOS.Installer.Logging;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class AddHardwareViewModel : ProgressViewModel
{
	private List<HardwareConfiguration> _addedHardwareList = new List<HardwareConfiguration>();

	private int _port = 80;

	public static bool OperationCanBeStopped => false;

	public IEnumerable<HardwareConfiguration> AddedHardwareList => _addedHardwareList;

	public AddHardwareViewModel()
	{
		InitServerPort();
	}

	private void InitServerPort()
	{
		if (GlobalData.TryGetValue(ServerParameters.ServerPort.ToString(), out var value) && !int.TryParse(value, out _port))
		{
			_port = 80;
		}
	}

	public void InitStartAddingHardware(object sender, DoWorkEventArgs e)
	{
		string value = "";
		GlobalData.TryGetValue(ServerParameters.SelectedHardware.ToString(), out value);
		StartAddingHardware(value);
	}

	public void StartAddingHardware(string foundCameras)
	{
		using (ConfigurationApiServiceManager serviceManager = new ConfigurationApiServiceManager(_port))
		{
			try
			{
				AddHardwarePlugin addHardwarePlugin = new AddHardwarePlugin(_port);
				addHardwarePlugin.ProgressChanged += AddPlugin_ProgressChanged;
				_addedHardwareList = addHardwarePlugin.DoAddHardware(foundCameras, serviceManager);
				LogManager.Write(FormattableString.Invariant($"Adding Hardware succeeded, Added {AddedHardwareList.Count()} hardware"));
				addHardwarePlugin.ProgressChanged -= AddPlugin_ProgressChanged;
				addHardwarePlugin = null;
			}
			catch (ArgumentNullException)
			{
				throw;
			}
			catch (Exception exception)
			{
				LogManager.Write(exception);
			}
		}
		SetProgress(100);
	}

	private void AddPlugin_ProgressChanged(object sender, ProgressChangedEventArgs e)
	{
		SetProgress(e.Progress);
		base.Header = e.Message;
	}
}
