using System.IO;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.DataTypes;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackExtendsBox : FullBox
{
	public uint TrackId;

	public uint DefaultSampleDescriptionIndex;

	public uint DefaultSampleDuration;

	public uint DefaultSampleSize;

	public readonly SampleFlags DefaultSampleFlags = new SampleFlags();

	public override uint DataSize => 20u;

	public TrackExtendsBox()
	{
		base.BoxType = "trex";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref TrackId);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref DefaultSampleDescriptionIndex);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref DefaultSampleDuration);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref DefaultSampleSize);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref DefaultSampleFlags.Value);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(TrackId);
		stream.WriteUInt32(DefaultSampleDescriptionIndex);
		stream.WriteUInt32(DefaultSampleDuration);
		stream.WriteUInt32(DefaultSampleSize);
		stream.WriteUInt32(DefaultSampleFlags.Value);
	}
}
