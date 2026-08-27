using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class VisualSampleEntry : SampleEntry
{
	protected ushort _predefined0;

	protected ushort _reserved0;

	protected readonly uint[] _predefined1 = new uint[3];

	public ushort Width;

	public ushort Height;

	public ushort HorizontalResolution = 72;

	public ushort HorizontalResolutionFraction;

	public ushort VerticalResolution = 72;

	public ushort VerticalResolutionFraction;

	protected uint _reserved1;

	public ushort FrameCount = 1;

	public readonly byte[] CompressorName = new byte[32];

	public ushort Depth = 24;

	protected ushort _predefined2 = ushort.MaxValue;

	public override bool CanHaveChildren => true;

	public override uint DataSize => base.DataSize + GetDataSize();

	public string CompressorNameString
	{
		get
		{
			return CompressorName.FixedArrayToString();
		}
		set
		{
			value.WriteToFixedArray(CompressorName);
		}
	}

	public VisualSampleEntry(string boxType)
		: base(boxType)
	{
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = base.ParseData(buffer, startIndex, restInBoxSize);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined0);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved0);
		for (int i = 0; i < _predefined1.Length; i++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined1[i]);
		}
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Width);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Height);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref HorizontalResolution);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref HorizontalResolutionFraction);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref VerticalResolution);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref VerticalResolutionFraction);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved1);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref FrameCount);
		GenericDataBox.ParseByteArray(buffer, startIndex, restInBoxSize, ref readBytes, CompressorName);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref Depth);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined2);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		base.WriteData(stream);
		stream.WriteUInt16(_predefined0);
		stream.WriteUInt16(_reserved0);
		uint[] predefined = _predefined1;
		foreach (uint value in predefined)
		{
			stream.WriteUInt32(value);
		}
		stream.WriteUInt16(Width);
		stream.WriteUInt16(Height);
		stream.WriteUInt16(HorizontalResolution);
		stream.WriteUInt16(HorizontalResolutionFraction);
		stream.WriteUInt16(VerticalResolution);
		stream.WriteUInt16(VerticalResolutionFraction);
		stream.WriteUInt32(_reserved1);
		stream.WriteUInt16(FrameCount);
		stream.Write(CompressorName);
		stream.WriteUInt16(Depth);
		stream.WriteUInt16(_predefined2);
	}

	private uint GetDataSize()
	{
		return 70u;
	}
}
