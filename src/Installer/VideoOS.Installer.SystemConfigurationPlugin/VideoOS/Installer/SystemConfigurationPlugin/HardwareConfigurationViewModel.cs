using System;
using System.Linq;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareConfigurationViewModel : SelectionViewModel
{
	public HardwareConfigurations HardwareConfiguration
	{
		get
		{
			HardwareConfigurations hardwareConfigurations = new HardwareConfigurations();
			foreach (HardwareData item in base.Hardware)
			{
				hardwareConfigurations.Add(item.HardwareConfiguration);
			}
			return hardwareConfigurations;
		}
	}

	public HardwareConfigurationViewModel()
	{
		LoadResources();
		base.HardwareSelection = false;
	}

	public void SetHardware(HardwareConfigurations hardwareCollection)
	{
		if (hardwareCollection == null)
		{
			throw new ArgumentNullException("hardwareCollection");
		}
		SetAddedHardware(hardwareCollection);
		SetFailedHardware(hardwareCollection);
	}

	public void LoadResources()
	{
		base.Header = Resources.HardwareConfigurationHeader;
		base.HeaderInfo = Resources.HardwareConfigurationHeaderInfo;
	}

	private void SetAddedHardware(HardwareConfigurations hardwareCollection)
	{
		foreach (HardwareConfiguration item in from h in hardwareCollection
			where h.Added
			orderby h.Name
			select h)
		{
			HardwareData hardwareData = new HardwareData(item);
			hardwareData.Error = false;
			hardwareData.EditMode = true;
			SetHardware(hardwareData);
		}
	}

	private void SetFailedHardware(HardwareConfigurations hardwareCollection)
	{
		foreach (HardwareConfiguration item in from h in hardwareCollection
			where !h.Added
			orderby h.Name
			select h)
		{
			HardwareData hardwareData = new HardwareData(item);
			hardwareData.Error = true;
			hardwareData.ErrorInfo = Resources.HardwareConfigurationFailedHardwareInfo;
			SetHardware(hardwareData);
		}
	}
}
