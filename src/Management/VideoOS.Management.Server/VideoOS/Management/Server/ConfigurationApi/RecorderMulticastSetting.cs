using VideoOS.Common.Integration;

namespace VideoOS.Management.Server.ConfigurationApi;

public class RecorderMulticastSetting
{
	private Settings multicastSettings;

	public bool Enabled { get; set; }

	public string IPAddressStart { get; set; }

	public string IPAddressEnd { get; set; }

	public string PortRangeStart { get; set; }

	public string PortRangeEnd { get; set; }

	public string MTU { get; set; }

	public string TTL { get; set; }

	public RecorderMulticastSetting(string settingsXml)
	{
		multicastSettings = new Settings(settingsXml);
		Enabled = bool.Parse(multicastSettings.Get("MulticastEnabled"));
		IPAddressStart = multicastSettings.Get("ClientIPRangeStart");
		IPAddressEnd = multicastSettings.Get("ClientIPRangeEnd");
		PortRangeStart = multicastSettings.Get("ClientPortRangeStart");
		PortRangeEnd = multicastSettings.Get("ClientPortRangeEnd");
		MTU = multicastSettings.Get("MTU");
		TTL = multicastSettings.Get("TTL");
	}

	public string SaveAsXml()
	{
		multicastSettings.Set("MulticastEnabled", Enabled.ToString());
		multicastSettings.Set("ClientIPRangeStart", IPAddressStart);
		multicastSettings.Set("ClientIPRangeEnd", IPAddressEnd);
		multicastSettings.Set("ClientPortRangeStart", PortRangeStart);
		multicastSettings.Set("ClientPortRangeEnd", PortRangeEnd);
		multicastSettings.Set("MTU", MTU);
		multicastSettings.Set("TTL", TTL);
		return multicastSettings.SaveAsXml();
	}
}
