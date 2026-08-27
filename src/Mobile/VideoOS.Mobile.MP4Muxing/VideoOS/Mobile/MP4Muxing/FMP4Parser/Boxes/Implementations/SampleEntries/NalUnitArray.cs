using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class NalUnitArray
{
	public enum ArrayCompleteness
	{
		SomeCouldBeInStream,
		AllOfTypeAreHere
	}

	private byte _completenessAndUnitType;

	private readonly List<byte[]> _nalUnitEntries = new List<byte[]>();

	public ArrayCompleteness ArrayCompletenessValue
	{
		get
		{
			return (ArrayCompleteness)((_completenessAndUnitType >> 7) & 1);
		}
		set
		{
			_completenessAndUnitType |= (byte)((int)value << 7);
		}
	}

	public byte NalUnitType
	{
		get
		{
			return (byte)(_completenessAndUnitType & 0x3F);
		}
		set
		{
			_completenessAndUnitType |= (byte)(value & 0x3F);
		}
	}

	public ushort NumberOfNalus => (ushort)_nalUnitEntries.Count;

	public IList<byte[]> NalUnitEntries => _nalUnitEntries;

	public uint GetDataSize()
	{
		uint num = 3u;
		foreach (byte[] nalUnitEntry in NalUnitEntries)
		{
			num += 2;
			num += (uint)nalUnitEntry.Length;
		}
		return num;
	}

	public uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt8(buffer, startIndex, restInBoxSize, ref readBytes, ref _completenessAndUnitType);
		ushort parameter = 0;
		GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
		ushort parameter2 = 0;
		for (int i = 0; i < parameter; i++)
		{
			GenericDataBox.ParseUInt16(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter2);
			byte[] array = new byte[parameter2];
			GenericDataBox.ParseByteArray(buffer, startIndex, restInBoxSize, ref readBytes, array);
			_nalUnitEntries.Add(array);
		}
		return readBytes;
	}

	public void WriteData(Stream stream)
	{
		stream.WriteByte(_completenessAndUnitType);
		stream.WriteUInt16(NumberOfNalus);
		foreach (byte[] nalUnitEntry in _nalUnitEntries)
		{
			stream.WriteUInt16((ushort)nalUnitEntry.Length);
			stream.Write(nalUnitEntry);
		}
	}

	public static NalUnitArray FromData(byte naluType, byte[] naluData)
	{
		return new NalUnitArray
		{
			NalUnitType = naluType,
			ArrayCompletenessValue = ArrayCompleteness.AllOfTypeAreHere,
			NalUnitEntries = { naluData }
		};
	}
}
