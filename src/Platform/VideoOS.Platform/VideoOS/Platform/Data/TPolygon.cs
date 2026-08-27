using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TPolygon", Namespace = "urn:milestone-systems")]
public class TPolygon : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool ClosedField;

	private TColor ColorField;

	private TColor FillColorField;

	private PointList PointListField;

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

	[DataMember(EmitDefaultValue = false)]
	public bool Closed
	{
		get
		{
			return ClosedField;
		}
		set
		{
			ClosedField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
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

	[DataMember(EmitDefaultValue = false)]
	public TColor FillColor
	{
		get
		{
			return FillColorField;
		}
		set
		{
			FillColorField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public PointList PointList
	{
		get
		{
			return PointListField;
		}
		set
		{
			PointListField = value;
		}
	}
}
