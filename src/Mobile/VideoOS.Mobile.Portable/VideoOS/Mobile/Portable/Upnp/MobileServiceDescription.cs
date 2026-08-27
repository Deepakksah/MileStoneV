using System;

namespace VideoOS.Mobile.Portable.Upnp;

public class MobileServiceDescription
{
	public static string MobileServerNameUpnpServiceDescription = "Milestone XProtect Mobile Server";

	public static string CompanyShortNameUpnpServiceDescription = "Milestone";

	public const string DeviceDescriptionPath = "/deviceDesc.xml";

	public static readonly Guid MobileServiceToken = new Guid("9C61B882-C240-424B-AD49-96B46E3B54F9");

	public static readonly string MobileServerDeviceType = $"urn:{CompanyShortNameUpnpServiceDescription}:device:{MobileServerNameUpnpServiceDescription}:1";

	public static readonly string MobileServerUidFormt = MobileServiceToken.ToString() + "-{0}";
}
