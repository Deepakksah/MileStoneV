namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public interface IGenericMultiDataBoxData
{
	byte[] Buffer { get; }

	uint StartIndex { get; }

	uint DataCount { get; }
}
