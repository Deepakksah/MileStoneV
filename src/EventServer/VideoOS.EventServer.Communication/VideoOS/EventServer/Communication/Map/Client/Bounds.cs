using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Bounds", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class Bounds : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool IsRealGPSField;

	private Position LowerRightPositionField;

	private Position UpperLeftOrCenterPositionField;

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

	[DataMember]
	public bool IsRealGPS
	{
		get
		{
			return IsRealGPSField;
		}
		set
		{
			IsRealGPSField = value;
		}
	}

	[DataMember]
	public Position LowerRightPosition
	{
		get
		{
			return LowerRightPositionField;
		}
		set
		{
			LowerRightPositionField = value;
		}
	}

	[DataMember]
	public Position UpperLeftOrCenterPosition
	{
		get
		{
			return UpperLeftOrCenterPositionField;
		}
		set
		{
			UpperLeftOrCenterPositionField = value;
		}
	}
}
