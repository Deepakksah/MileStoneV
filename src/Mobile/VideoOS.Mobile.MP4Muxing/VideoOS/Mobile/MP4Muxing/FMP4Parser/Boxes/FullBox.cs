using System;
using System.IO;
using System.Runtime.CompilerServices;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class FullBox : GenericDataBox
{
	private uint _flags;

	public uint Version { get; set; }

	public uint Flags => _flags;

	public FullBox()
	{
		base.HeaderSize = 12u;
	}

	protected override uint ParseHeader(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint size, ref string type)
	{
		uint num = base.ParseHeader(buffer, startIndex, restInBoxSize, ref size, ref type);
		uint num2 = ParseVersionAndFlags(buffer, startIndex + num, restInBoxSize - num);
		return num + num2;
	}

	protected uint ParseVersionAndFlags(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		if (restInBoxSize < 4 || buffer.Length - startIndex < 4)
		{
			throw new Exception("Not enough data");
		}
		Version = buffer[startIndex];
		_flags = BoxHelper.ToUInt24(buffer, startIndex + 1);
		return 4u;
	}

	protected override void WriteHeader(Stream stream)
	{
		base.WriteHeader(stream);
		stream.WriteByte((byte)Version);
		stream.Write(BoxHelper.Int24ToBuffer(_flags));
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void SetFlag(uint mask)
	{
		_flags |= mask;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ResetFlag(uint mask)
	{
		_flags &= ~mask;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void SetResetFlag<T>(T? optionalParameter, uint mask) where T : struct
	{
		SetResetFlag(optionalParameter.HasValue, mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void SetResetFlag(bool condition, uint mask)
	{
		if (condition)
		{
			SetFlag(mask);
		}
		else
		{
			ResetFlag(mask);
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected bool IsSetFlag(uint mask)
	{
		return (_flags & mask) == mask;
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Parse<T>(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, Func<byte[], uint, T> convertor, uint bytesInType, ref T? parameter, uint mask) where T : struct
	{
		if (IsSetFlag(mask))
		{
			if (restInBoxSize - readBytes < bytesInType)
			{
				throw new Exception("Not enough data in box");
			}
			parameter = convertor(buffer, startIndex + readBytes);
			readBytes += bytesInType;
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ParseUInt32(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref uint? parameter, uint mask)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt32, 4u, ref parameter, mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void ParseUInt64(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ref ulong? parameter, uint mask)
	{
		Parse(buffer, startIndex, restInBoxSize, ref readBytes, BoxHelper.ToUInt64, 8u, ref parameter, mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void Write<T>(Stream stream, Func<T, byte[]> convertor, T? parameter, uint mask) where T : struct
	{
		if (IsSetFlag(mask))
		{
			T valueOrDefault = parameter.GetValueOrDefault();
			stream.Write(convertor(valueOrDefault));
		}
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void WriteInt64(Stream stream, ulong? value, uint mask)
	{
		Write(stream, BoxHelper.Int64ToBuffer, value, mask);
	}

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	protected void WriteInt32(Stream stream, uint? value, uint mask)
	{
		Write(stream, BoxHelper.Int32ToBuffer, value, mask);
	}
}
