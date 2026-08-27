using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public interface IGenericBoxEntryProcessor<T>
{
	uint DataSize { get; }

	T ParseData(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes);

	void WriteToStream(Stream stream, T entry);
}
