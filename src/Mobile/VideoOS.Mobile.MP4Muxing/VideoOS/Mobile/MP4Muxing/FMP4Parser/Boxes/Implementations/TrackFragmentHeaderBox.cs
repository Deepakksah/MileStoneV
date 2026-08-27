using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class TrackFragmentHeaderBox : FullBox
{
	public const uint BaseDataOffsetPresentFlag = 1u;

	public const uint SampleDescriptionIndexPresent = 2u;

	public const uint DefaultSampleDurationPresent = 8u;

	public const uint DefaultSampleSizePresent = 16u;

	public const uint DefaultSampleFlagsPresent = 32u;

	public const uint DurationIsEmptyFlag = 65536u;

	public const uint DefaultBaseIsMoofFlag = 131072u;

	private uint _trackId;

	private ulong? _baseDataOffset;

	private uint? _sampleDescriptionIndex;

	private uint? _defaultSampleDuration;

	private uint? _defaultSampleSize;

	private uint? _defaultSampleFlags;

	private bool _durationIsEmpty;

	private bool _defaultBaseIsMoof;

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public uint TrackId
	{
		get
		{
			return _trackId;
		}
		set
		{
			_trackId = value;
		}
	}

	public ulong? BaseDataOffset
	{
		get
		{
			return _baseDataOffset;
		}
		set
		{
			_baseDataOffset = value;
			SetResetFlag(value, 1u);
		}
	}

	public uint? SampleDescriptionIndex
	{
		get
		{
			return _sampleDescriptionIndex;
		}
		set
		{
			_sampleDescriptionIndex = value;
			SetResetFlag(value, 2u);
		}
	}

	public uint? DefaultSampleDuration
	{
		get
		{
			return _defaultSampleDuration;
		}
		set
		{
			_defaultSampleDuration = value;
			SetResetFlag(value, 8u);
		}
	}

	public uint? DefaultSampleSize
	{
		get
		{
			return _defaultSampleSize;
		}
		set
		{
			_defaultSampleSize = value;
			SetResetFlag(value, 16u);
		}
	}

	public uint? DefaultSampleFlags
	{
		get
		{
			return _defaultSampleFlags;
		}
		set
		{
			_defaultSampleFlags = value;
			SetResetFlag(value, 32u);
		}
	}

	public bool DurationIsEmpty
	{
		get
		{
			return _durationIsEmpty;
		}
		set
		{
			_durationIsEmpty = value;
			SetResetFlag(value, 65536u);
		}
	}

	public bool DefaultBaseIsMoof
	{
		get
		{
			return _defaultBaseIsMoof;
		}
		set
		{
			_defaultBaseIsMoof = value;
			SetResetFlag(value, 131072u);
		}
	}

	public TrackFragmentHeaderBox()
	{
		base.BoxType = "tfhd";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _trackId);
		ParseUInt64(buffer, startIndex, restInBoxSize, ref readBytes, ref _baseDataOffset, 1u);
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _sampleDescriptionIndex, 2u);
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _defaultSampleDuration, 8u);
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _defaultSampleSize, 16u);
		ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _defaultSampleFlags, 32u);
		_durationIsEmpty = IsSetFlag(65536u);
		_defaultBaseIsMoof = IsSetFlag(131072u);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(TrackId);
		WriteInt64(stream, _baseDataOffset, 1u);
		WriteInt32(stream, _sampleDescriptionIndex, 2u);
		WriteInt32(stream, _defaultSampleDuration, 8u);
		WriteInt32(stream, _defaultSampleSize, 16u);
		WriteInt32(stream, _defaultSampleFlags, 32u);
	}

	private uint GetDataSize()
	{
		uint num = 4u;
		if (_baseDataOffset.HasValue)
		{
			num += 8;
		}
		if (_sampleDescriptionIndex.HasValue)
		{
			num += 4;
		}
		if (_defaultSampleDuration.HasValue)
		{
			num += 4;
		}
		if (_defaultSampleSize.HasValue)
		{
			num += 4;
		}
		if (_defaultSampleFlags.HasValue)
		{
			num += 4;
		}
		return num;
	}
}
