using System;
using System.Collections.Generic;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;
using VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes;

public class BoxFactory : IBoxFactory
{
	public static readonly Dictionary<string, Func<IBoxParse>> Fmp4RemuxerKnownTypes = new Dictionary<string, Func<IBoxParse>>
	{
		{
			"moof",
			() => new MovieFragmentBox()
		},
		{
			"mfhd",
			() => new MovieFragmentHeaderBox()
		},
		{
			"traf",
			() => new TrackFragmentBox()
		},
		{
			"tfhd",
			() => new TrackFragmentHeaderBox()
		},
		{
			"tfdt",
			() => new TrackFragmentDecodeTimeBox()
		},
		{
			"trun",
			() => new TrackFragmentRunBox()
		},
		{
			"mdat",
			() => new MediaDataBox()
		}
	};

	public static readonly Dictionary<string, Func<IBoxParse>> Fmp4MuxerKnownTypes = new Dictionary<string, Func<IBoxParse>>
	{
		{
			"ftyp",
			() => new FileTypeBox()
		},
		{
			"moov",
			() => new MovieBox()
		},
		{
			"mvhd",
			() => new MovieHeaderBox()
		},
		{
			"trak",
			() => new TrackBox()
		},
		{
			"tkhd",
			() => new TrackHeaderBox()
		},
		{
			"mdia",
			() => new MediaBox()
		},
		{
			"mdhd",
			() => new MediaHeaderBox()
		},
		{
			"hdlr",
			() => new HandlerReferenceBox()
		},
		{
			"minf",
			() => new MediaInformationBox()
		},
		{
			"vmhd",
			() => new VideoMediaHeaderBox()
		},
		{
			"dinf",
			() => new DataInformationBox()
		},
		{
			"dref",
			() => new DataReferenceBox()
		},
		{
			"url ",
			() => new DataEntryUrlBox()
		},
		{
			"stbl",
			() => new SampleTableBox()
		},
		{
			"avc1",
			() => new Avc1Entry()
		},
		{
			"avcC",
			() => new AvcDecoderConfigurationBox()
		},
		{
			"hvc1",
			() => new Hvc1Entry()
		},
		{
			"hev1",
			() => new Hev1Entry()
		},
		{
			"hvcC",
			() => new HevcDecoderConfigurationBox()
		},
		{
			"stsd",
			() => new SampleDescriptionBox()
		},
		{
			"stts",
			() => new DecodingTimeToSampleBox()
		},
		{
			"stsc",
			() => new SampleToChunkBox()
		},
		{
			"stsz",
			() => new SampleSizeBox()
		},
		{
			"stco",
			() => new ChunkOffsetBox()
		},
		{
			"mvex",
			() => new MovieExtendsBox()
		},
		{
			"trex",
			() => new TrackExtendsBox()
		}
	};

	public static readonly Dictionary<string, Func<IBoxParse>> MfraKnownTypes = new Dictionary<string, Func<IBoxParse>> { 
	{
		"mfra",
		() => new GenericParentBox("mfra")
	} };

	public static readonly Dictionary<string, Func<IBoxParse>> TimeScaleKnownTypes = new Dictionary<string, Func<IBoxParse>>
	{
		{
			"moov",
			() => new MovieBox()
		},
		{
			"mvhd",
			() => new MovieHeaderBox()
		},
		{
			"moof",
			() => new MovieFragmentBox()
		},
		{
			"mfhd",
			() => new MovieFragmentHeaderBox()
		}
	};

	public static readonly Dictionary<string, Func<IBoxParse>> Mp4TrunSamplesParserKnownTypes = new Dictionary<string, Func<IBoxParse>>
	{
		{
			"moof",
			() => new MovieFragmentBox()
		},
		{
			"traf",
			() => new TrackFragmentBox()
		},
		{
			"trun",
			() => new TrackFragmentRunBox()
		}
	};

	private readonly IDictionary<string, Func<IBoxParse>> _knownBoxCreators;

	public static BoxFactory Fmp4Remuxer => new BoxFactory(Fmp4RemuxerKnownTypes);

	public static BoxFactory Fmp4Muxer => new BoxFactory(Fmp4MuxerKnownTypes);

	public static BoxFactory Mfra => new BoxFactory(MfraKnownTypes);

	public static BoxFactory TimeScale => new BoxFactory(TimeScaleKnownTypes);

	public static BoxFactory Mp4TrunSamplesParser => new BoxFactory(Mp4TrunSamplesParserKnownTypes);

	public BoxFactory(IDictionary<string, Func<IBoxParse>> knownBoxCreators)
	{
		_knownBoxCreators = knownBoxCreators;
	}

	public IBoxParse CreateBox(string boxType)
	{
		if (_knownBoxCreators.ContainsKey(boxType))
		{
			return _knownBoxCreators[boxType]();
		}
		return new GenericDataBox();
	}

	public BoxFactory(IEnumerable<Dictionary<string, Func<IBoxParse>>> allBoxesCreators)
		: this(allBoxesCreators.Combine())
	{
	}
}
