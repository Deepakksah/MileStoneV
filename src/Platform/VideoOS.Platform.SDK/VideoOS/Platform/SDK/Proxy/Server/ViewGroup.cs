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
public class ViewGroup : IViewGroup
{
	private ViewGroupInfo viewGroupInfoField;

	private ViewGroupData viewGroupDataField;

	IViewGroupInfo IViewGroup.ViewGroupInfo
	{
		get
		{
			return ViewGroupInfo;
		}
		set
		{
			ViewGroupInfo = (ViewGroupInfo)value;
		}
	}

	IViewGroupData IViewGroup.ViewGroupData
	{
		get
		{
			return ViewGroupData;
		}
		set
		{
			ViewGroupData = (ViewGroupData)value;
		}
	}

	public ViewGroupInfo ViewGroupInfo
	{
		get
		{
			return viewGroupInfoField;
		}
		set
		{
			viewGroupInfoField = value;
		}
	}

	public ViewGroupData ViewGroupData
	{
		get
		{
			return viewGroupDataField;
		}
		set
		{
			viewGroupDataField = value;
		}
	}
}
