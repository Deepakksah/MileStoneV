using System;
using System.Linq;
using VideoOS.Installer.SystemConfigurationPlugin.Properties;

namespace VideoOS.Installer.SystemConfigurationPlugin;

public class HardwareSelectionViewModel : SelectionViewModel
{
	public HardwareSelectionViewModel()
	{
		LoadResources();
		base.HardwareSelection = true;
	}

	public void SetHardware(HardwareConfigurations hardwareConfigurations)
	{
		if (hardwareConfigurations == null)
		{
			throw new ArgumentNullException("hardwareConfigurations");
		}
		if (hardwareConfigurations.Count > 0)
		{
			hardwareConfigurations.ForEach(delegate(HardwareConfiguration h)
			{
				h.Name = ConstructHardwareName(h);
			});
			(from h in hardwareConfigurations
				where h.Validated
				orderby h.Name
				select h).ForEach(delegate(HardwareConfiguration c)
			{
				AddHardware(c);
			});
			(from h in hardwareConfigurations
				where !h.Validated
				orderby h.Name
				select h).ForEach(delegate(HardwareConfiguration c)
			{
				AddHardware(c);
			});
		}
		else
		{
			base.NoCameraMessage = Resources.HardwareSelectionNoHardwareMessage;
		}
	}

	public void LoadResources()
	{
		base.Header = Resources.HardwareSelectionHeader;
		base.HeaderInfo = Resources.HardwareSelectionInfo;
	}

	private void AddHardware(HardwareConfiguration hardwareConfiguration)
	{
		HardwareData hardwareData = new HardwareData(hardwareConfiguration);
		hardwareData.EditMode = false;
		SetHardware(hardwareData);
	}

	private static string ConstructHardwareName(HardwareConfiguration configuration)
	{
		string text = "";
		if (Uri.TryCreate(configuration.IPAddress, UriKind.Absolute, out var result))
		{
			return configuration.Name + " - " + result.Host;
		}
		return configuration.Name;
	}
}
