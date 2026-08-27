namespace VideoOS.Common.Integration.DeviceDiscovery;

public class AddressDriverPair
{
	public const int UnspecifiedDriver = -1;

	public string Address { get; set; }

	public ushort Port { get; set; }

	public bool UseSecurePort { get; set; }

	public ushort SecurePort { get; set; }

	public int DriverNumber { get; set; }
}
