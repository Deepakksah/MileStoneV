using System;
using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel.Binary;

[StructLayout(LayoutKind.Sequential, Size = 24)]
public struct HeaderTypeDeviceStateInfo
{
	public Guid ItemID;

	public uint ErrorCode;

	public uint Reseved;
}
