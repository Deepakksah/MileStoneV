using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Status2;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderStatus2")]
public class VideoStreamStatistics : MediaStreamStatisticsBase
{
	private Size imageResolutionField;

	private string videoFormatField;

	private ulong imageSizeInBytesField;

	private double fPSRequestedField;

	public Size ImageResolution
	{
		get
		{
			return imageResolutionField;
		}
		set
		{
			imageResolutionField = value;
		}
	}

	public string VideoFormat
	{
		get
		{
			return videoFormatField;
		}
		set
		{
			videoFormatField = value;
		}
	}

	public ulong ImageSizeInBytes
	{
		get
		{
			return imageSizeInBytesField;
		}
		set
		{
			imageSizeInBytesField = value;
		}
	}

	public double FPSRequested
	{
		get
		{
			return fPSRequestedField;
		}
		set
		{
			fPSRequestedField = value;
		}
	}
}
