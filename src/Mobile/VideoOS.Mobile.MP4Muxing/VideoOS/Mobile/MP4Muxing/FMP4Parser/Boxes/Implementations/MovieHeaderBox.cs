using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class MovieHeaderBox : FullBox
{
	public ulong CreationTime;

	public ulong ModificationTime;

	public uint TimeScale;

	public ulong Duration;

	public ushort Rate = 1;

	public ushort RateFraction;

	public byte Volume = 1;

	public byte VolumeFraction;

	private ushort _reserved16;

	private readonly uint[] _reserved2x32 = new uint[2];

	private readonly uint[] _matrix = new uint[9] { 65536u, 0u, 0u, 0u, 65536u, 0u, 0u, 0u, 1073741824u };

	private readonly uint[] _predefined = new uint[6];

	public uint NextTrackId;

	public override uint DataSize => GetDataSize();

	public uint[] Matrix => _matrix;

	public DateTime CreationDateTime
	{
		get
		{
			return BoxTimeConvertor.FromBoxTime(CreationTime);
		}
		set
		{
			CreationTime = value.ToBoxTime();
		}
	}

	public DateTime ModificationDateTime
	{
		get
		{
			return BoxTimeConvertor.FromBoxTime(ModificationTime);
		}
		set
		{
			ModificationTime = value.ToBoxTime();
		}
	}

	public TimeSpan DurationTimeSpan
	{
		get
		{
			return BoxTimeConvertor.FromBoxDuration(Duration, TimeScale);
		}
		set
		{
			Duration = value.ToBoxDuration(TimeScale);
		}
	}

	public MovieHeaderBox()
	{
		base.BoxType = "mvhd";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		if (base.Version == 1)
		{
			GenericDataBox.ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref CreationTime);
			GenericDataBox.ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref ModificationTime);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref TimeScale);
			GenericDataBox.ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref Duration);
		}
		else
		{
			uint parameter = 0u;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			CreationTime = parameter;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			ModificationTime = parameter;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref TimeScale);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			Duration = parameter;
		}
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Rate);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref RateFraction);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref Volume);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref VolumeFraction);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved16);
		for (int i = 0; i < _reserved2x32.Length; i++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved2x32[i]);
		}
		for (int j = 0; j < _matrix.Length; j++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _matrix[j]);
		}
		for (int k = 0; k < _predefined.Length; k++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined[k]);
		}
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref NextTrackId);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		if (base.Version == 1)
		{
			stream.WriteUInt64(CreationTime);
			stream.WriteUInt64(ModificationTime);
			stream.WriteUInt32(TimeScale);
			stream.WriteUInt64(Duration);
		}
		else
		{
			stream.WriteUInt32((uint)CreationTime);
			stream.WriteUInt32((uint)ModificationTime);
			stream.WriteUInt32(TimeScale);
			stream.WriteUInt32((uint)Duration);
		}
		stream.WriteUInt16(Rate);
		stream.WriteUInt16(RateFraction);
		stream.WriteByte(Volume);
		stream.WriteByte(VolumeFraction);
		stream.WriteUInt16(_reserved16);
		for (int i = 0; i < _reserved2x32.Length; i++)
		{
			stream.WriteUInt32(_reserved2x32[i]);
		}
		for (int j = 0; j < _matrix.Length; j++)
		{
			stream.WriteUInt32(_matrix[j]);
		}
		for (int k = 0; k < _predefined.Length; k++)
		{
			stream.WriteUInt32(_predefined[k]);
		}
		stream.WriteUInt32(NextTrackId);
	}

	private uint GetDataSize()
	{
		uint num = 0u;
		if (base.Version == 1)
		{
			num += 8;
			num += 8;
			num += 4;
			num += 8;
		}
		else
		{
			num += 4;
			num += 4;
			num += 4;
			num += 4;
		}
		num += 4;
		num += 2;
		num += 2;
		num += 8;
		num += (uint)(4 * _matrix.Length);
		num += (uint)(4 * _predefined.Length);
		return num + 4;
	}
}
