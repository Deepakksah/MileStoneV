using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class MediaHeaderBox : FullBox
{
	public ulong CreationTime;

	public ulong ModificationTime;

	public uint TimeScale;

	public ulong Duration;

	public ushort LanguageCode;

	private ushort _predefined;

	public override uint DataSize => GetDataSize();

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

	public MediaHeaderBox()
	{
		base.BoxType = "mdhd";
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
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref LanguageCode);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined);
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
		stream.WriteUInt16(LanguageCode);
		stream.WriteUInt16(_predefined);
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
		num += 2;
		return num + 2;
	}
}
