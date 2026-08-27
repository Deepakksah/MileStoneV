using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "AlertTypeInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class AlertTypeInfo : IAlertTypeInfo
{
	private Guid AlertTypeIdField;

	private string DescriptionField;

	private string NameField;

	[DataMember]
	public Guid AlertTypeId
	{
		get
		{
			return AlertTypeIdField;
		}
		set
		{
			AlertTypeIdField = value;
		}
	}

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
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}
}
