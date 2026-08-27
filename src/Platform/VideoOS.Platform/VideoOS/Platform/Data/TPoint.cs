using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "TPoint", Namespace = "urn:milestone-systems")]
public class TPoint : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double XField;

	private double YField;

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
	public double X
	{
		get
		{
			return XField;
		}
		set
		{
			XField = value;
		}
	}

	[DataMember(IsRequired = true)]
	public double Y
	{
		get
		{
			return YField;
		}
		set
		{
			YField = value;
		}
	}
}
