using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class HevcDecoderConfigurationBox : GenericDataBox
{
	public enum ParallelismType
	{
		Unknown,
		SliceBased,
		TileBased,
		EntropyCoding
	}

	public enum ConstantFrameRate
	{
		Unknown,
		Yes,
		YesToTempLayer
	}

	public enum TemporalLayers
	{
		Unknown,
		No,
		Layers2,
		Layers3,
		Layers4,
		Layers5,
		Layers6,
		Layers7
	}

	public enum TemporalIdNested
	{
		No,
		Yes
	}

	public enum NalUnitLenSize
	{
		OneByte,
		TwoBytes,
		ThreeBytes,
		FourBytes
	}

	public byte ConfigurationVersion = 1;

	private byte _profileSpaceIdcTear;

	public uint ProfileCompatibilityFlags;

	public readonly byte[] ConstraintIndicatorFlags = new byte[6];

	public byte LevelIdc;

	private ushort _minSpatialSegmentIdc;

	private byte _parallelismType;

	private byte _chromaFormat;

	private byte _bitDepthLumaMinus8;

	private byte _bitDepthChromaMinus8;

	public ushort AvgFrameRate;

	private byte _constFpsTempLayersAndLen;

	private readonly List<NalUnitArray> _nalUnitArrays = new List<NalUnitArray>();

	public override uint DataSize => GetDataSize();

	public byte ProfileSpace
	{
		get
		{
			return (byte)(_profileSpaceIdcTear >> 6);
		}
		set
		{
			_profileSpaceIdcTear |= (byte)(value << 6);
		}
	}

	public byte TierFlag
	{
		get
		{
			return (byte)((_profileSpaceIdcTear >> 5) & 1);
		}
		set
		{
			_profileSpaceIdcTear |= (byte)((value & 1) << 5);
		}
	}

	public byte ProfileIdc
	{
		get
		{
			return (byte)(_profileSpaceIdcTear & 0x1F);
		}
		set
		{
			_profileSpaceIdcTear |= (byte)(value & 0x1F);
		}
	}

	public ushort MinSpatialSegmentIdc
	{
		get
		{
			return (ushort)(_minSpatialSegmentIdc & 0xFFF);
		}
		set
		{
			_minSpatialSegmentIdc = (ushort)(0xF000 | value);
		}
	}

	public ParallelismType ParallelismTypeValue
	{
		get
		{
			return (ParallelismType)(_parallelismType & 3);
		}
		set
		{
			_parallelismType = (byte)((ParallelismType)252 | value);
		}
	}

	public byte ChromaFormat
	{
		get
		{
			return (byte)(_chromaFormat & 3);
		}
		set
		{
			_chromaFormat = (byte)(0xFC | value);
		}
	}

	public byte BitDepthLumaMinus8
	{
		get
		{
			return (byte)(_bitDepthLumaMinus8 & 7);
		}
		set
		{
			_bitDepthLumaMinus8 = (byte)(0xF8 | value);
		}
	}

	public byte BitDepthChromaMinus8
	{
		get
		{
			return (byte)(_bitDepthChromaMinus8 & 7);
		}
		set
		{
			_bitDepthChromaMinus8 = (byte)(0xF8 | value);
		}
	}

	public ConstantFrameRate ConstantFrameRateValue
	{
		get
		{
			return (ConstantFrameRate)((_constFpsTempLayersAndLen >> 6) & 3);
		}
		set
		{
			_constFpsTempLayersAndLen |= (byte)((int)(value & (ConstantFrameRate)3) << 6);
		}
	}

	public TemporalLayers NumTemporalLayers
	{
		get
		{
			return (TemporalLayers)((_constFpsTempLayersAndLen >> 3) & 7);
		}
		set
		{
			_constFpsTempLayersAndLen |= (byte)((int)(value & TemporalLayers.Layers7) << 3);
		}
	}

	public TemporalIdNested TemporalIdNestedValue
	{
		get
		{
			return (TemporalIdNested)((_constFpsTempLayersAndLen >> 2) & 1);
		}
		set
		{
			_constFpsTempLayersAndLen |= (byte)((int)(value & TemporalIdNested.Yes) << 2);
		}
	}

	public NalUnitLenSize LengthSizeMinusOne
	{
		get
		{
			return (NalUnitLenSize)(_constFpsTempLayersAndLen & 3);
		}
		set
		{
			_constFpsTempLayersAndLen |= (byte)(value & NalUnitLenSize.FourBytes);
		}
	}

	public byte NumOfArrays => (byte)_nalUnitArrays.Count;

	public IList<NalUnitArray> NalUnitArrays => _nalUnitArrays;

	public HevcDecoderConfigurationBox()
	{
		base.BoxType = "hvcC";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref ConfigurationVersion);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _profileSpaceIdcTear);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref ProfileCompatibilityFlags);
		GenericDataBox.ParseByteArray(buffer, startIndex, restInBoxSize, ref readBytes, ConstraintIndicatorFlags);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref LevelIdc);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _minSpatialSegmentIdc);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _parallelismType);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _chromaFormat);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _bitDepthLumaMinus8);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _bitDepthChromaMinus8);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref AvgFrameRate);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _constFpsTempLayersAndLen);
		byte parameter = 0;
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		for (int i = 0; i < parameter; i++)
		{
			NalUnitArray nalUnitArray = new NalUnitArray();
			readBytes += nalUnitArray.ParseData(buffer, startIndex + readBytes, restInBoxSize - readBytes);
			_nalUnitArrays.Add(nalUnitArray);
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteByte(ConfigurationVersion);
		stream.WriteByte(_profileSpaceIdcTear);
		stream.WriteUInt32(ProfileCompatibilityFlags);
		stream.Write(ConstraintIndicatorFlags);
		stream.WriteByte(LevelIdc);
		stream.WriteUInt16(_minSpatialSegmentIdc);
		stream.WriteByte(_parallelismType);
		stream.WriteByte(_chromaFormat);
		stream.WriteByte(_bitDepthLumaMinus8);
		stream.WriteByte(_bitDepthChromaMinus8);
		stream.WriteUInt16(AvgFrameRate);
		stream.WriteByte(_constFpsTempLayersAndLen);
		stream.WriteByte(NumOfArrays);
		foreach (NalUnitArray nalUnitArray in _nalUnitArrays)
		{
			nalUnitArray.WriteData(stream);
		}
	}

	protected uint GetDataSize()
	{
		uint num = 23u;
		foreach (NalUnitArray nalUnitArray in _nalUnitArrays)
		{
			num += nalUnitArray.GetDataSize();
		}
		return num;
	}
}
