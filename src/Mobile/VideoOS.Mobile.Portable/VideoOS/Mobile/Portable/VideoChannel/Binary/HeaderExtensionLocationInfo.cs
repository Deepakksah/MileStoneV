using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 48)]
public struct HeaderExtensionLocationInfo
{
	public uint Length;

	public uint Version;

	public uint Source;

	public uint Reserved;

	public uint Fields;

	public uint Longitude;

	public uint Latitude;

	public uint Altitude;

	public uint HorizontalAccuracy;

	public uint VerticalAccuracy;

	public uint Speed;

	public uint Azimuth;
}
