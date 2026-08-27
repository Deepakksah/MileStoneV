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
public class OutputGroupInfo : GroupInfo, IOutputGroupInfo, IGroupInfo
{
	private OutputGroupInfo[] outputGroupsField;

	private Guid[] outputsField;

	IOutputGroupInfo[] IOutputGroupInfo.OutputGroups
	{
		get
		{
			return OutputGroups;
		}
		set
		{
			OutputGroups = (OutputGroupInfo[])value;
		}
	}

	public OutputGroupInfo[] OutputGroups
	{
		get
		{
			return outputGroupsField;
		}
		set
		{
			outputGroupsField = value;
		}
	}

	public Guid[] Outputs
	{
		get
		{
			return outputsField;
		}
		set
		{
			outputsField = value;
		}
	}
}
