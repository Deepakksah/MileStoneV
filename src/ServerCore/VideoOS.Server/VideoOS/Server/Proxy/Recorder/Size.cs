using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Server.Proxy.Recorder;

[Serializable]
[GeneratedCode("wsdl", "2.0.50727.42")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class Size
{
	private int widthField;

	private int heightField;

	public int Width
	{
		get
		{
			return widthField;
		}
		set
		{
			widthField = value;
		}
	}

	public int Height
	{
		get
		{
			return heightField;
		}
		set
		{
			heightField = value;
		}
	}
}
