using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Snapshot", Namespace = "urn:milestone-systems")]
public class Snapshot : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int TimeOffsetField;

	private uint WidthField;

	private uint HeightField;

	private bool HasOverlayField;

	private uint SizeInBytesField;

	private byte[] ImageField;

	private string PathField;

	private FQID FQIDField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public int TimeOffset
	{
		get
		{
			return TimeOffsetField;
		}
		set
		{
			TimeOffsetField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public uint Width
	{
		get
		{
			return WidthField;
		}
		set
		{
			WidthField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public uint Height
	{
		get
		{
			return HeightField;
		}
		set
		{
			HeightField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 3)]
	public bool HasOverlay
	{
		get
		{
			return HasOverlayField;
		}
		set
		{
			HasOverlayField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public uint SizeInBytes
	{
		get
		{
			return SizeInBytesField;
		}
		set
		{
			SizeInBytesField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 5)]
	public byte[] Image
	{
		get
		{
			return ImageField;
		}
		set
		{
			ImageField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 6)]
	public string Path
	{
		get
		{
			return PathField;
		}
		set
		{
			PathField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 7)]
	public FQID FQID
	{
		get
		{
			return FQIDField;
		}
		set
		{
			FQIDField = value;
		}
	}
}
