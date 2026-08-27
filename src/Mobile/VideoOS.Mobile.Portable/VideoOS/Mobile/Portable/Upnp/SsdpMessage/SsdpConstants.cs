namespace VideoOS.Mobile.Portable.Upnp.SsdpMessage;

public class SsdpConstants
{
	public const string UpnpMulticastAddress = "239.255.255.250";

	public const string UpnpMulticastAddressIPv6 = "[FF02::C]";

	public const ushort UpnpMulticastPort = 1900;

	public const string SsdpAll = "ssdp:all";

	public const string SsdpDiscover = "\"ssdp:discover\"";

	public const string UpnpRootDevice = "upnp:rootdevice";

	public const string DeviceRootDevice = "device:rootdevice";

	public const string CacheControlFormat = "max-age={0}";

	public const string ServerFormat = "{0} UpnP/1.0 {1}/{2}";

	public const string Host = "HOST";

	public const string Nt = "NT";

	public const string Nts = "NTS";

	public const string Usn = "USN";

	public const string CacheControl = "CACHE-CONTROL";

	public const string Server = "SERVER";

	public const string Man = "MAN";

	public const string Mx = "MX";

	public const string St = "ST";

	public const string Location = "LOCATION";

	public const string Ext = "EXT";

	public const string Date = "DATE";

	public static readonly string UpnpMulticastEndPoint = string.Format("{0}:{1}", "239.255.255.250", (ushort)1900);

	public static readonly string UpnpMulticastEndPointIPv6 = string.Format("{0}:{1}", "[FF02::C]", (ushort)1900);

	public const string NtsAlive = "ssdp:alive";

	public const string NtsByeBye = "ssdp:byebye";

	public const string InternetGatewayDeviceUrn = "urn:schemas-upnp-org:device:InternetGatewayDevice:1";

	public const string BasicDevice = "urn:schemas-upnp-org:device:Basic:1";

	public const string InternetGatewayDevice = "InternetGatewayDevice";

	public const string UpnpDeviceSchema = "urn:schemas-upnp-org:device-1-0";
}
