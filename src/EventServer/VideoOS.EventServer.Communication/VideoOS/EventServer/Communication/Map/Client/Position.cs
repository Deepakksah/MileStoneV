using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.Map.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Position", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Central.Server")]
public class Position : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private double AltitudeField;

	private double AngleField;

	private double LatitudeField;

	private double LongitudeField;

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
	public double Altitude
	{
		get
		{
			return AltitudeField;
		}
		set
		{
			AltitudeField = value;
		}
	}

	[DataMember]
	public double Angle
	{
		get
		{
			return AngleField;
		}
		set
		{
			AngleField = value;
		}
	}

	[DataMember]
	public double Latitude
	{
		get
		{
			return LatitudeField;
		}
		set
		{
			LatitudeField = value;
		}
	}

	[DataMember]
	public double Longitude
	{
		get
		{
			return LongitudeField;
		}
		set
		{
			LongitudeField = value;
		}
	}
}
