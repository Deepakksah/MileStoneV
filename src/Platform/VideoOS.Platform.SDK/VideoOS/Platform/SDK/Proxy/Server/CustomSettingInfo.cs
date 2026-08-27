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
public class CustomSettingInfo : ICustomSettingInfo
{
	private Guid idField;

	private DateTime lastModifiedTimeField;

	private string lastModifiedUserField;

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

	public DateTime LastModifiedTime
	{
		get
		{
			return lastModifiedTimeField;
		}
		set
		{
			lastModifiedTimeField = value;
		}
	}

	public string LastModifiedUser
	{
		get
		{
			return lastModifiedUserField;
		}
		set
		{
			lastModifiedUserField = value;
		}
	}
}
