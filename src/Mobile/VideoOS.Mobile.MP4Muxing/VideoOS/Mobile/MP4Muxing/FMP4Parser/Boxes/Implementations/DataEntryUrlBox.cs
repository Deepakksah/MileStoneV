using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class DataEntryUrlBox : FullBox
{
	public const uint SelfContainedFlag = 1u;

	public string Location;

	public override uint DataSize => GetDataSize();

	public bool SelfContained
	{
		get
		{
			return IsSetFlag(1u);
		}
		set
		{
			if (value)
			{
				SetFlag(1u);
			}
			else
			{
				ResetFlag(1u);
			}
		}
	}

	public DataEntryUrlBox()
	{
		base.BoxType = "url ";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		if (!IsSetFlag(1u))
		{
			GenericDataBox.ParseUtf8String(buffer, startIndex, restInBoxSize, ref readBytes, ref Location);
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		if (!IsSetFlag(1u))
		{
			stream.WriteStringAsUtf8(Location);
		}
	}

	private uint GetDataSize()
	{
		if (IsSetFlag(1u))
		{
			return 0u;
		}
		return GenericDataBox.GetUtf8StringLength(Location);
	}
}
