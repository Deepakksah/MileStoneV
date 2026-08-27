using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public interface IBoxParse : IBox
{
	IBoxParse Parent { get; set; }

	IList<IBoxParse> Children { get; }

	uint InitFromBuffer(byte[] buffer, uint startIndex, uint maxSize, IBoxFactory boxFactory);

	void WriteToStream(Stream steam);
}
