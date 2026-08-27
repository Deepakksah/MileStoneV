using System.Collections.Generic;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class FileTypeBox : Box
{
	public static readonly uint mp41 = BoxHelper.ToUInt32("mp41");

	public static readonly uint mp42 = BoxHelper.ToUInt32("mp42");

	public static readonly uint isom = BoxHelper.ToUInt32("isom");

	public static readonly uint iso6 = BoxHelper.ToUInt32("iso6");

	public uint MajorBrand;

	public uint MinorVersion;

	private readonly List<uint> _compatibleBrands = new List<uint>();

	public override bool CanHaveData => true;

	public override uint DataSize => GetDataSize();

	public IList<uint> CompatibleBrands => _compatibleBrands;

	public FileTypeBox()
	{
		base.BoxType = "ftyp";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref MajorBrand);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref MinorVersion);
		while (readBytes < restInBoxSize)
		{
			uint parameter = 0u;
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref parameter);
			_compatibleBrands.Add(parameter);
		}
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(MajorBrand);
		stream.WriteUInt32(MinorVersion);
		foreach (uint compatibleBrand in _compatibleBrands)
		{
			stream.WriteUInt32(compatibleBrand);
		}
	}

	private uint GetDataSize()
	{
		return (uint)(8 + 4 * _compatibleBrands.Count);
	}
}
