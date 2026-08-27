using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "Bookmark", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class Bookmark : IBookmark
{
	private string DescriptionField;

	private Guid DeviceIdField;

	private string HeaderField;

	private Guid IdField;

	private string ReferenceField;

	private DateTime TimeBeginField;

	private DateTime TimeEndField;

	private DateTime TimeTriggedField;

	private string UserField;

	[DataMember]
	public string Description
	{
		get
		{
			return DescriptionField;
		}
		set
		{
			DescriptionField = value;
		}
	}

	[DataMember]
	public Guid DeviceId
	{
		get
		{
			return DeviceIdField;
		}
		set
		{
			DeviceIdField = value;
		}
	}

	[DataMember]
	public string Header
	{
		get
		{
			return HeaderField;
		}
		set
		{
			HeaderField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public string Reference
	{
		get
		{
			return ReferenceField;
		}
		set
		{
			ReferenceField = value;
		}
	}

	[DataMember]
	public DateTime TimeBegin
	{
		get
		{
			return TimeBeginField;
		}
		set
		{
			TimeBeginField = value;
		}
	}

	[DataMember]
	public DateTime TimeEnd
	{
		get
		{
			return TimeEndField;
		}
		set
		{
			TimeEndField = value;
		}
	}

	[DataMember]
	public DateTime TimeTrigged
	{
		get
		{
			return TimeTriggedField;
		}
		set
		{
			TimeTriggedField = value;
		}
	}

	[DataMember]
	public string User
	{
		get
		{
			return UserField;
		}
		set
		{
			UserField = value;
		}
	}
}
