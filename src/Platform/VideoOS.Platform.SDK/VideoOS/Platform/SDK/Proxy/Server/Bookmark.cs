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
public class Bookmark : IBookmark
{
	private Guid idField;

	private Guid deviceIdField;

	private string referenceField;

	private string headerField;

	private string descriptionField;

	private string userField;

	private DateTime timeBeginField;

	private DateTime timeTriggedField;

	private DateTime timeEndField;

	public Guid Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

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

	public string Reference
	{
		get
		{
			return referenceField;
		}
		set
		{
			referenceField = value;
		}
	}

	public string Header
	{
		get
		{
			return headerField;
		}
		set
		{
			headerField = value;
		}
	}

	public string Description
	{
		get
		{
			return descriptionField;
		}
		set
		{
			descriptionField = value;
		}
	}

	public string User
	{
		get
		{
			return userField;
		}
		set
		{
			userField = value;
		}
	}

	public DateTime TimeBegin
	{
		get
		{
			return timeBeginField;
		}
		set
		{
			timeBeginField = value;
		}
	}

	public DateTime TimeTrigged
	{
		get
		{
			return timeTriggedField;
		}
		set
		{
			timeTriggedField = value;
		}
	}

	public DateTime TimeEnd
	{
		get
		{
			return timeEndField;
		}
		set
		{
			timeEndField = value;
		}
	}
}
