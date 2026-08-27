using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.RecorderWebservice.Shared;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class MotionAreaInfo
{
	private int xField;

	private int yField;

	private int widthField;

	private int heightField;

	public int X
	{
		get
		{
			return xField;
		}
		set
		{
			xField = value;
		}
	}

	public int Y
	{
		get
		{
			return yField;
		}
		set
		{
			yField = value;
		}
	}

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
