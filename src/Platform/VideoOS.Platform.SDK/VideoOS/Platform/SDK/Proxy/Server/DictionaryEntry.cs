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
public class DictionaryEntry : IDictionaryEntry
{
	private object keyField;

	private object valueField;

	public object Key
	{
		get
		{
			return keyField;
		}
		set
		{
			keyField = value;
		}
	}

	public object Value
	{
		get
		{
			return valueField;
		}
		set
		{
			valueField = value;
		}
	}
}
