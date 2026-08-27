namespace VideoOS.Mobile.Portable.Upnp;

public interface IUpnpDeviceDescriptionSerializer
{
	string SerializeToString(UpnpDeviceDescription deviceDescription);

	UpnpDeviceDescription Deserialize(string description);
}
