using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Platform.SDK.Proxy.Server;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.17929")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class TrackInfo : ITrackInfo
{
	private Guid trackIdField;

	private bool edgeField;

	public Guid TrackId
	{
		get
		{
			return trackIdField;
		}
		set
		{
			trackIdField = value;
		}
	}

	public bool Edge
	{
		get
		{
			return edgeField;
		}
		set
		{
			edgeField = value;
		}
	}
}
