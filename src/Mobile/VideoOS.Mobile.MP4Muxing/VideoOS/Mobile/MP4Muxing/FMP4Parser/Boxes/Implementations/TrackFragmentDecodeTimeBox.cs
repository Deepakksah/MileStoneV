using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackFragmentDecodeTimeBox : FullBox
{
	public ulong BaseMediaDecodeTime;

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public TrackFragmentDecodeTimeBox()
	{
		base.BoxType = "tfdt";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		if (base.Version == 1)
		{
			GenericDataBox.ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref BaseMediaDecodeTime);
		}
		else
		{
			uint parameter = 0u;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			BaseMediaDecodeTime = parameter;
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		if (base.Version == 1)
		{
			stream.WriteUInt64(BaseMediaDecodeTime);
		}
		else
		{
			stream.WriteUInt32((uint)BaseMediaDecodeTime);
		}
	}

	private uint GetDataSize()
	{
		if (base.Version != 1)
		{
			return 4u;
		}
		return 8u;
	}
}
