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
public class InputGroupInfo : GroupInfo, IInputGroupInfo, IGroupInfo
{
	private InputGroupInfo[] inputGroupsField;

	private Guid[] inputsField;

	IInputGroupInfo[] IInputGroupInfo.InputGroups
	{
		get
		{
			return InputGroups;
		}
		set
		{
			InputGroups = (InputGroupInfo[])value;
		}
	}

	public InputGroupInfo[] InputGroups
	{
		get
		{
			return inputGroupsField;
		}
		set
		{
			inputGroupsField = value;
		}
	}

	public Guid[] Inputs
	{
		get
		{
			return inputsField;
		}
		set
		{
			inputsField = value;
		}
	}
}
