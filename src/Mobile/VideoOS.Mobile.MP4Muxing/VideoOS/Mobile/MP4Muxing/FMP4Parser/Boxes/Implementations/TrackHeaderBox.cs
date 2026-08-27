using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackHeaderBox : FullBox
{
	public const uint TrackEnabledFlag = 1u;

	public const uint TrackInMovieFlag = 2u;

	public const uint TrackInPreviewFlag = 4u;

	public DateTime CreationTime;

	public DateTime ModificationTime;

	public uint TrackId;

	private uint _reserved_0;

	public ulong Duration;

	private readonly uint[] _reserved_1 = new uint[2];

	public ushort Layer;

	public ushort AlternateGroup;

	public ushort Volume;

	private ushort _reserved_2;

	public readonly uint[] Matrix = new uint[9] { 65536u, 0u, 0u, 0u, 65536u, 0u, 0u, 0u, 1073741824u };

	public ushort Width;

	public ushort WidthFraction;

	public ushort Height;

	public ushort HeightFraction;

	public override uint DataSize => GetDataSize();

	public bool TrackEnabled
	{
		get
		{
			return IsSetFlag(1u);
		}
		set
		{
			SetResetFlag(value, 1u);
		}
	}

	public bool TrackInMovie
	{
		get
		{
			return IsSetFlag(2u);
		}
		set
		{
			SetResetFlag(value, 2u);
		}
	}

	public bool TrackInPreview
	{
		get
		{
			return IsSetFlag(4u);
		}
		set
		{
			SetResetFlag(value, 4u);
		}
	}

	public TrackHeaderBox()
	{
		base.BoxType = "tkhd";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		if (base.Version == 1)
		{
			GenericDataBox.ParseDateAsUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref CreationTime);
			GenericDataBox.ParseDateAsUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref ModificationTime);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref TrackId);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved_0);
			GenericDataBox.ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref Duration);
		}
		else
		{
			GenericDataBox.ParseDateAsUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref CreationTime);
			GenericDataBox.ParseDateAsUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref ModificationTime);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref TrackId);
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved_0);
			uint parameter = 0u;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			Duration = parameter;
		}
		for (int i = 0; i < _reserved_1.Length; i++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved_1[i]);
		}
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Layer);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref AlternateGroup);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Volume);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved_2);
		for (int j = 0; j < Matrix.Length; j++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref Matrix[j]);
		}
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Width);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref WidthFraction);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Height);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref HeightFraction);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		if (base.Version == 1)
		{
			stream.WriteDateAsUInt64(CreationTime);
			stream.WriteDateAsUInt64(ModificationTime);
			stream.WriteUInt32(TrackId);
			stream.WriteUInt32(_reserved_0);
			stream.WriteUInt64(Duration);
		}
		else
		{
			stream.WriteDateAsUInt32(CreationTime);
			stream.WriteDateAsUInt32(ModificationTime);
			stream.WriteUInt32(TrackId);
			stream.WriteUInt32(_reserved_0);
			stream.WriteUInt32((uint)Duration);
		}
		for (int i = 0; i < _reserved_1.Length; i++)
		{
			stream.WriteUInt32(_reserved_1[i]);
		}
		stream.WriteUInt16(Layer);
		stream.WriteUInt16(AlternateGroup);
		stream.WriteUInt16(Volume);
		stream.WriteUInt16(_reserved_2);
		for (int j = 0; j < Matrix.Length; j++)
		{
			stream.WriteUInt32(Matrix[j]);
		}
		stream.WriteUInt16(Width);
		stream.WriteUInt16(WidthFraction);
		stream.WriteUInt16(Height);
		stream.WriteUInt16(HeightFraction);
	}

	private uint GetDataSize()
	{
		uint num = 0u;
		if (base.Version == 1)
		{
			num += 8;
			num += 8;
			num += 4;
			num += 4;
			num += 8;
		}
		else
		{
			num += 4;
			num += 4;
			num += 4;
			num += 4;
			num += 4;
		}
		num += (uint)(4 * _reserved_1.Length);
		num += 2;
		num += 2;
		num += 2;
		num += 2;
		num += (uint)(4 * Matrix.Length);
		num += 4;
		return num + 4;
	}
}
