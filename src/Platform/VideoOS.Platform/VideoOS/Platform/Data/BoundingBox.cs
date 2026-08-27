using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "BoundingBox", Namespace = "urn:milestone-systems")]
public class BoundingBox : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double TopField;

	private double LeftField;

	private double BottomField;

	private double RightField;

	private TColor ColorField;

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
	public double Top
	{
		get
		{
			return TopField;
		}
		set
		{
			TopField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 1)]
	public double Left
	{
		get
		{
			return LeftField;
		}
		set
		{
			LeftField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 2)]
	public double Bottom
	{
		get
		{
			return BottomField;
		}
		set
		{
			BottomField = value;
		}
	}

	[DataMember(IsRequired = true, Order = 3)]
	public double Right
	{
		get
		{
			return RightField;
		}
		set
		{
			RightField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 4)]
	public TColor Color
	{
		get
		{
			return ColorField;
		}
		set
		{
			ColorField = value;
		}
	}
}
