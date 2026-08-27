using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ObjectMotion", Namespace = "urn:milestone-systems")]
public class ObjectMotion : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double SpeedField;

	private string SpeedUnitField;

	private TPolygon PathField;

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
	public double Speed
	{
		get
		{
			return SpeedField;
		}
		set
		{
			SpeedField = value;
		}
	}

	[DataMember(EmitDefaultValue = false)]
	public string SpeedUnit
	{
		get
		{
			return SpeedUnitField;
		}
		set
		{
			SpeedUnitField = value;
		}
	}

	[DataMember(EmitDefaultValue = false, Order = 2)]
	public TPolygon Path
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
}
