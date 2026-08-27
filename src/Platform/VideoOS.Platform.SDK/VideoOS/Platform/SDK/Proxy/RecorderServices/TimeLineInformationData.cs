using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Xml.Serialization;

namespace VideoOS.Platform.SDK.Proxy.RecorderServices;

[Serializable]
[GeneratedCode("wsdl", "4.0.30319.33440")]
[DebuggerStepThrough]
[DesignerCategory("code")]
[XmlType(Namespace = "http://videoos.net/2/XProtectCSRecorderCommand")]
public class TimeLineInformationData
{
	private Guid deviceIdField;

	private Guid typeField;

	private DateTime beginTimeField;

	private TimeDuration intervalField;

	private int countField;

	private string dataField;

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
