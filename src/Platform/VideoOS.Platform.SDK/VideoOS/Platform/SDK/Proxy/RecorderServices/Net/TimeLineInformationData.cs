using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DebuggerStepThrough]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class TimeLineInformationData
{
	private Guid deviceIdField;

	private Guid typeField;

	private DateTime beginTimeField;

	private TimeDuration intervalField;

	private int countField;

	private string dataField;

	[XmlElement(Order = 0)]
	public Guid DeviceId
	{
		get
		{
			return deviceIdField;
		}
		set
		{
			deviceIdField = value;
		}
	}

	[XmlElement(Order = 1)]
	public Guid Type
	{
		get
		{
			return typeField;
		}
		set
		{
			typeField = value;
		}
	}

	[XmlElement(Order = 2)]
	public DateTime BeginTime
	{
		get
		{
			return beginTimeField;
		}
		set
		{
			beginTimeField = value;
		}
	}

	[XmlElement(Order = 3)]
	public TimeDuration Interval
	{
		get
		{
			return intervalField;
		}
		set
		{
			intervalField = value;
		}
	}

	[XmlElement(Order = 4)]
	public int Count
	{
		get
		{
			return countField;
		}
		set
		{
			countField = value;
		}
	}

	[XmlElement(Order = 5)]
	public string Data
	{
		get
		{
			return dataField;
		}
		set
		{
			dataField = value;
		}
	}
}
