namespace VideoOS.Mobile.MP4Muxing.FMP4Parser.Boxes.Implementations.SampleEntries;

public class Avc1Entry : VisualSampleEntry
{
	public ushort Version
	{
		get
		{
			return _predefined0;
		}
		set
		{
			_predefined0 = value;
		}
	}

	public ushort RevisionLevel
	{
		get
		{
			return _reserved0;
		}
		set
		{
			_reserved0 = value;
		}
	}

	public uint Vendor
	{
		get
		{
			return _predefined1[0];
		}
		set
		{
			_predefined1[0] = value;
		}
	}

	public uint TemporalQuality
	{
		get
		{
			return _predefined1[1];
		}
		set
		{
			_predefined1[1] = value;
		}
	}

	public uint SpacialQuality
	{
		get
		{
			return _predefined1[2];
		}
		set
		{
			_predefined1[2] = value;
		}
	}

	public uint EntryDataSize
	{
		get
		{
			return _reserved1;
		}
		set
		{
			_reserved1 = value;
		}
	}

	public ushort FramesPerSample
	{
		get
		{
			return FrameCount;
		}
		set
		{
			FrameCount = value;
		}
	}

	public short ColorTableIndex
	{
		get
		{
			return (short)_predefined2;
		}
		set
		{
			_predefined2 = (ushort)value;
		}
	}

	public Avc1Entry()
		: base("avc1")
	{
	}
}
