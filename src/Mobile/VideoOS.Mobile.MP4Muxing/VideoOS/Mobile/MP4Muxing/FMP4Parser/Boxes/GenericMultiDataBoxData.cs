namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class GenericMultiDataBoxData : IGenericMultiDataBoxData
{
	public byte[] Buffer { get; set; }

	public uint StartIndex { get; set; }

	public uint DataCount { get; set; }
}
