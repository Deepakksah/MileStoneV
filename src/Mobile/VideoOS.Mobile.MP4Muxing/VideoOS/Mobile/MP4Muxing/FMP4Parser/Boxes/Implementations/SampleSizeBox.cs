using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class SampleSizeBox : FullBox
{
	public uint SampleSize;

	public readonly List<uint> SampleSizes = new List<uint>();

	public override uint DataSize
	{
		get
		{
			uint num = 8u;
			if (SampleSize == 0)
			{
				num += (uint)(SampleSizes.Count * 4);
			}
			return num;
		}
	}

	public uint SampleCount => (uint)SampleSizes.Count;

	public SampleSizeBox()
	{
		base.BoxType = "stsz";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref SampleSize);
		uint parameter = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		if (SampleSize == 0)
		{
			for (int i = 0; i < parameter; i++)
			{
				uint parameter2 = 0u;
				GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter2);
				SampleSizes.Add(parameter2);
			}
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(SampleSize);
		stream.WriteUInt32(SampleCount);
		if (SampleSize == 0)
		{
			for (int i = 0; i < SampleCount; i++)
			{
				stream.WriteUInt32(SampleSizes[i]);
			}
		}
	}
}
