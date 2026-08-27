using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PositionInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class PositionInfo : IExtensibleDataObject, IPositionInfo
{
	private ExtensionDataObject extensionDataField;

	private double PanField;

	private double TiltField;

	private double ZoomField;

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
	public double Pan
	{
		get
		{
			return PanField;
		}
		set
		{
			PanField = value;
		}
	}

	[DataMember]
	public double Tilt
	{
		get
		{
			return TiltField;
		}
		set
		{
			TiltField = value;
		}
	}

	[DataMember]
	public double Zoom
	{
		get
		{
			return ZoomField;
		}
		set
		{
			ZoomField = value;
		}
	}
}
