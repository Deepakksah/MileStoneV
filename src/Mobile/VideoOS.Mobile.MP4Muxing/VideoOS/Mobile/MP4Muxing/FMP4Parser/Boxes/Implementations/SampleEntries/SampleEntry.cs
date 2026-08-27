using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class SampleEntry : Box
{
	private readonly byte[] _reserved = new byte[6];

	public ushort DataReferenceIndex;

	public override bool CanHaveData => true;

	public override uint DataSize => 8u;

	public SampleEntry(string boxType)
	{
		base.BoxType = boxType;
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseByteArray(buffer, startIndex, restInBoxSize, ref readBytes, _reserved);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref DataReferenceIndex);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.Write(_reserved);
		stream.WriteUInt16(DataReferenceIndex);
	}
}
