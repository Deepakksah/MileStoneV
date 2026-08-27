using System;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public static class StreamExtensions
{
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void Write(this Stream stream, byte[] buffer)
	{
		stream.Write(buffer, 0, buffer.Length);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUInt16(this Stream stream, ushort value)
	{
		stream.Write(BoxHelper.Int16ToBuffer(value));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUInt32(this Stream stream, uint value)
	{
		stream.Write(BoxHelper.Int32ToBuffer(value));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteUInt64(this Stream stream, ulong value)
	{
		stream.Write(BoxHelper.Int64ToBuffer(value));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteDateAsUInt32(this Stream stream, DateTime value)
	{
		stream.Write(BoxHelper.Int32ToBuffer((uint)value.ToBoxTime()));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteDateAsUInt64(this Stream stream, DateTime value)
	{
		stream.Write(BoxHelper.Int64ToBuffer(value.ToBoxTime()));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static void WriteStringAsUtf8(this Stream stream, string text)
	{
		stream.Write(Encoding.UTF8.GetBytes(text));
		stream.WriteByte(0);
	}
}
