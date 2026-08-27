using System.Runtime.InteropServices;

namespace VideoOS.Mobile.Portable.VideoChannel;

[StructLayout(LayoutKind.Explicit)]
public struct GbdHeader
{
	[FieldOffset(0)]
	public ushort DataType;

	[FieldOffset(2)]
	public uint TotalLength;

	[FieldOffset(6)]
	public ushort CodecType;

	[FieldOffset(8)]
	public ushort SequenceNumber;

	[FieldOffset(10)]
	public ushort Flags;

	[FieldOffset(12)]
	public ulong TimestampSync;

	[FieldOffset(20)]
	public ulong TimestampPic;

	[FieldOffset(28)]
	public uint Reserved;
}
