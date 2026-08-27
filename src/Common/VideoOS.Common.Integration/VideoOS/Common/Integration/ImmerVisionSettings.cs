using System;
using System.Globalization;

namespace VideoOS.Common.Integration;

public class ImmerVisionSettings
{
	public string RegisteredPanomorphLensNumber { get; set; }

	public DevicePosition Position { get; set; }

	public bool Enabled { get; set; }

	public double FieldOfView { get; set; }

	public static ImmerVisionSettings FromXml(string settingsXml)
	{
		ImmerVisionSettings immerVisionSettings = new ImmerVisionSettings();
		Settings settings = new Settings(settingsXml);
		immerVisionSettings.RegisteredPanomorphLensNumber = settings.Get("rpl");
		if (Enum.TryParse<DevicePosition>(settings.Get("position"), out var result))
		{
			immerVisionSettings.Position = result;
		}
		if (bool.TryParse(settings.Get("enabled"), out var result2))
		{
			immerVisionSettings.Enabled = result2;
		}
		if (double.TryParse(settings.Get("fovangle"), out var result3))
		{
			immerVisionSettings.FieldOfView = result3;
		}
		else
		{
			immerVisionSettings.FieldOfView = -1.0;
		}
		return immerVisionSettings;
	}

	public string ToXml()
	{
		Settings settings = new Settings();
		settings.Set("rpl", RegisteredPanomorphLensNumber);
		settings.Set("position", Position.ToString());
		settings.Set("enabled", Enabled.ToString());
		settings.Set("fovangle", FieldOfView.ToString(CultureInfo.InvariantCulture));
		return settings.SaveAsXml();
	}
}
