using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class VideoMediaHeaderBox : FullBox
{
	public enum Colors
	{
		Red,
		Green,
		Blue
	}

	public ushort GraphicsMode;

	public readonly ushort[] OpColor = new ushort[3];

	public override uint DataSize => GetDataSize();

	public VideoMediaHeaderBox()
	{
		base.BoxType = "vmhd";
		SetFlag(1u);
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref GraphicsMode);
		for (int i = 0; i < OpColor.Length; i++)
		{
			GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref OpColor[i]);
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt16(GraphicsMode);
		ushort[] opColor = OpColor;
		foreach (ushort value in opColor)
		{
			stream.WriteUInt16(value);
		}
	}

	private uint GetDataSize()
	{
		return (uint)(2 + 2 * OpColor.Length);
	}
}
