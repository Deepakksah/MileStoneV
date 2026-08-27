using System;
using System.IO;

namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations;

public class HandlerReferenceBox : FullBox
{
	public enum HandlerTypes
	{
		VideoTrack,
		AudioTrack,
		HintTrack,
		MetadataTrack,
		AuxiliaryVideoTrack,
		ResourcesTrack
	}

	public static readonly uint vide = BoxHelper.ToUInt32("vide");

	public static readonly uint soun = BoxHelper.ToUInt32("soun");

	public static readonly uint hint = BoxHelper.ToUInt32("hint");

	public static readonly uint meta = BoxHelper.ToUInt32("meta");

	public static readonly uint auxv = BoxHelper.ToUInt32("auxv");

	public static readonly uint Null = BoxHelper.ToUInt32("null");

	private static readonly uint[] HandlerTypesUints = new uint[6] { vide, soun, hint, meta, auxv, Null };

	private uint _predefined;

	public uint HandlerTypeValue;

	private readonly uint[] _reserved = new uint[3];

	public string Name;

	public override uint DataSize => GetDataSize();

	public HandlerTypes HandlerType
	{
		get
		{
			int num = Array.IndexOf(HandlerTypesUints, HandlerTypeValue);
			if (num < 0)
			{
				return HandlerTypes.ResourcesTrack;
			}
			return (HandlerTypes)num;
		}
		set
		{
			HandlerTypeValue = HandlerTypesUints[(int)value];
		}
	}

	public HandlerReferenceBox()
	{
		base.BoxType = "hdlr";
	}

	protected override uint ParseData(byte[] buffer, uint startIndex, uint restInBoxSize)
	{
		uint readBytes = 0u;
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _predefined);
		GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref HandlerTypeValue);
		for (int i = 0; i < _reserved.Length; i++)
		{
			GenericDataBox.ParseUInt32(buffer, startIndex, restInBoxSize, ref readBytes, ref _reserved[i]);
		}
		GenericDataBox.ParseUtf8String(buffer, startIndex, restInBoxSize, ref readBytes, ref Name);
		return readBytes;
	}

	protected override void WriteData(Stream stream)
	{
		stream.WriteUInt32(_predefined);
		stream.WriteUInt32(HandlerTypeValue);
		uint[] reserved = _reserved;
		foreach (uint value in reserved)
		{
			stream.WriteUInt32(value);
		}
		stream.WriteStringAsUtf8(Name);
	}

	private uint GetDataSize()
	{
		return (uint)(8 + 4 * _reserved.Length) + GenericDataBox.GetUtf8StringLength(Name);
	}
}
