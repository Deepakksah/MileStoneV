using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class LhevcDecoderConfigurationBox : HevcDecoderConfigurationBox
{
	public ushort OperationPointIdx;

	public override uint DataSize => 2 + GetDataSize();

	public LhevcDecoderConfigurationBox()
	{
		base.BoxType = "lhvC";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = base.ParseData(buffer, startIndex, restInBoxSize);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref ConfigurationVersion);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		base.WriteData(stream);
		stream.WriteUInt16(OperationPointIdx);
	}
}
