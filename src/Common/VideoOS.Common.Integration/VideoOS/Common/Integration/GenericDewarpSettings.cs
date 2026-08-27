using System;
using System.Globalization;

namespace VideoOS.Common.Integration;

public class GenericDewarpSettings
{
	private static class PropertyName
	{
		public const string Position = "position";

		public const string Enabled = "enabled";

		public const string FieldOfViewAngle = "fovangle";
	}

	public DevicePosition Position { get; set; }

	public bool Enabled { get; set; }

	public double FieldOfViewAngle { get; set; }

	public static GenericDewarpSettings FromXml(string settingsXml)
	{
		GenericDewarpSettings genericDewarpSettings = new GenericDewarpSettings();
		Settings settings = new Settings(settingsXml);
		if (Enum.TryParse<DevicePosition>(settings.Get("position"), out var result))
		{
			genericDewarpSettings.Position = result;
		}
		if (bool.TryParse(settings.Get("enabled"), out var result2))
		{
			genericDewarpSettings.Enabled = result2;
		}
		if (double.TryParse(settings.Get("fovangle"), out var result3))
		{
			genericDewarpSettings.FieldOfViewAngle = result3;
		}
		else
		{
			genericDewarpSettings.FieldOfViewAngle = -1.0;
		}
		return genericDewarpSettings;
	}

	public string ToXml()
	{
		Settings settings = new Settings();
		settings.Set("position", Position.ToString());
		settings.Set("enabled", Enabled.ToString());
		settings.Set("fovangle", FieldOfViewAngle.ToString(CultureInfo.InvariantCulture));
		return settings.SaveAsXml();
	}
}
