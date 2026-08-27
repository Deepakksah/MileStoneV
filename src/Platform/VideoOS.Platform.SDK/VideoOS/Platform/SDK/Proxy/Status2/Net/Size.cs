using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2.Net;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Size", Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class Size : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int WidthField;

	private int HeightField;

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
	public int Width
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

	[DataMember(IsRequired = true, Order = 1)]
	public int Height
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
}
