using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
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
