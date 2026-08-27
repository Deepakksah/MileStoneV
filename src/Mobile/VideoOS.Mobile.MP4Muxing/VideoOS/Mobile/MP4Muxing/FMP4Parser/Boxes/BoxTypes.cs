namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public static class BoxTypes
{
	public const string Unknown = "    ";

	public const string FileTypeBox = "ftyp";

	public const string MediaDataBox = "mdat";

	public const string MovieBox = "moov";

	public const string MovieHeaderBox = "mvhd";

	public const string TrackBox = "trak";

	public const string TrackHeaderBox = "tkhd";

	public const string TrackReferenceBox = "tref";

	public const string MediaBox = "mdia";

	public const string MediaHeaderBox = "mdhd";

	public const string HandlerReferenceBox = "hdlr";

	public const string MediaInformationBox = "minf";

	public const string VideoMediaHeaderBox = "vmhd";

	public const string DataInformationBox = "dinf";

	public const string DataReferenceBox = "dref";

	public const string DataEntryUrlBox = "url ";

	public const string SampleTableBox = "stbl";

	public const string SampleDescriptionBox = "stsd";

	public const string Avc1SampleEntry = "avc1";

	public const string AvcDecoderConfigurationBox = "avcC";

	public const string Hvc1SampleEntry = "hvc1";

	public const string Hev1SampleEntry = "hev1";

	public const string HevcDecoderConfigurationBox = "hvcC";

	public const string LhevcDecoderConfigurationBox = "lhvC";

	public const string DecodingTimeToSampleBox = "stts";

	public const string CompositionTimeToSampleBox = "ctts";

	public const string SyncSampleBox = "stss";

	public const string SampleSizeBox = "stsz";

	public const string SampleToChunkBox = "stsc";

	public const string ChunkOffsetBox = "stco";

	public const string MovieExtendsBox = "mvex";

	public const string TrackExtendsBox = "trex";

	public const string MovieFragmentBox = "moof";

	public const string MovieFragmentHeaderBox = "mfhd";

	public const string TrackFragmentBox = "traf";

	public const string TrackFragmentHeaderBox = "tfhd";

	public const string TrackFragmentRunBox = "trun";

	public const string MovieFragmentRandomAccessBox = "mfra";

	public const string TrackFragmentRandomAccessBox = "tfra";

	public const string MovieFragmentRandomAccessOffsetBox = "mfro";

	public const string TrackFragmentDecodeTimeBox = "tfdt";

	public const string UserDataBox = "udta";

	public const string SegmentTypeBox = "styp";

	public const string SegmentIndexBox = "sidx";
}
