using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class AvcDecoderConfigurationBox : Box
{
	public byte ConfigurationVersion = 1;

	public byte AvcProfileIndication;

	public byte ProfileCompatibility;

	public byte AvcLevelIndication;

	private byte _nalUnitSize = byte.MaxValue;

	private byte _spsEntryCount = 224;

	private ushort _spsSize;

	private byte[] _spsData;

	public byte PpsEntryCount;

	private ushort _ppsSize;

	private byte[] _ppsData;

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public byte NalUnitSize
	{
		get
		{
			return (byte)((_nalUnitSize & 3) + 1);
		}
		set
		{
			_nalUnitSize = (byte)(((value - 1) & 3) | 0xFC);
		}
	}

	public byte SpsEntryCount
	{
		get
		{
			return (byte)(_spsEntryCount & 0x1F);
		}
		set
		{
			_spsEntryCount = (byte)((value & 0x1F) | 0xE0);
		}
	}

	public byte[] SpsData
	{
		get
		{
			return _spsData;
		}
		set
		{
			_spsData = value;
			_spsSize = (ushort)((value != null) ? ((ushort)value.Length) : 0);
		}
	}

	public byte[] PpsData
	{
		get
		{
			return _ppsData;
		}
		set
		{
			_ppsData = value;
			_ppsSize = (ushort)((value != null) ? ((ushort)value.Length) : 0);
		}
	}

	public AvcDecoderConfigurationBox()
	{
		base.BoxType = "avcC";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref ConfigurationVersion);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref AvcProfileIndication);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref ProfileCompatibility);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref AvcLevelIndication);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _nalUnitSize);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _spsEntryCount);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _spsSize);
		ParseData(buffer, startIndex, restInBoxSize, ref readBytes, _spsSize, ref _spsData);
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref PpsEntryCount);
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref _ppsSize);
		ParseData(buffer, startIndex, restInBoxSize, ref readBytes, _ppsSize, ref _ppsData);
		return readBytes;
	}

	private void ParseData(byte[] buffer, uint startIndex, uint restInBoxSize, ref uint readBytes, ushort size, ref byte[] data)
	{
		if (size > 0)
		{
			data = new byte[size];
			GenericDataBox.ParseByteArray(buffer, startIndex, restInBoxSize, ref readBytes, data);
		}
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteByte(ConfigurationVersion);
		stream.WriteByte(AvcProfileIndication);
		stream.WriteByte(ProfileCompatibility);
		stream.WriteByte(AvcLevelIndication);
		stream.WriteByte(_nalUnitSize);
		stream.WriteByte(_spsEntryCount);
		stream.WriteUInt16(_spsSize);
		if (_spsData != null)
		{
			stream.Write(_spsData);
		}
		stream.WriteByte(PpsEntryCount);
		stream.WriteUInt16(_ppsSize);
		if (_ppsData != null)
		{
			stream.Write(_ppsData);
		}
	}

	private uint GetDataSize()
	{
		uint num = 0u;
		num += 6;
		num += 2;
		if (_spsData != null)
		{
			num += (uint)_spsData.Length;
		}
		num++;
		num += 2;
		if (_ppsData != null)
		{
			num += (uint)_ppsData.Length;
		}
		return num;
	}
}
