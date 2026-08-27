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
public class Change : IChange
{
	private ChangeType changeTypeField;

	private string idField;

	IChangeType IChange.ChangeType
	{
		get
		{
			return ChangeType;
		}
		set
		{
			ChangeType = (ChangeType)value;
		}
	}

	public DateTime Time { get; set; }

	public ChangeType ChangeType
	{
		get
		{
			return changeTypeField;
		}
		set
		{
			changeTypeField = value;
		}
	}

	public string Id
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
}
