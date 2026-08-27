using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericMultiDataBox : GenericDataBox
{
	private readonly List<GenericMultiDataBoxData> _datas = new List<GenericMultiDataBoxData>();

	public override uint DataSize => (uint)_datas.Sum((GenericMultiDataBoxData data) => data.DataCount);

	public IList<GenericMultiDataBoxData> Datas => _datas;

	public GenericMultiDataBox(string boxType)
	{
		base.BoxType = boxType;
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		_datas.Add(new GenericMultiDataBoxData
		{
			Buffer = buffer,
			StartIndex = startIndex,
			DataCount = restInBoxSize
		});
		return restInBoxSize;
	}

	protected override void WriteData(Stream stream)
	{
		foreach (GenericMultiDataBoxData data in _datas)
		{
			stream.Write(data.Buffer, (int)data.StartIndex, (int)data.DataCount);
		}
	}
}
