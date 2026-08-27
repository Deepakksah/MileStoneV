using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TColor", Namespace = "urn:milestone-systems")]
public class TColor : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private byte AField;

	private byte RField;

	private byte GField;

	private byte BField;

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
	public byte A
	{
		get
		{
			return AField;
		}
		set
		{
			AField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public byte R
	{
		get
		{
			return RField;
		}
		set
		{
			RField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public byte G
	{
		get
		{
			return GField;
		}
		set
		{
			GField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public byte B
	{
		get
		{
			return BField;
		}
		set
		{
			BField = value;
		}
	}
}
