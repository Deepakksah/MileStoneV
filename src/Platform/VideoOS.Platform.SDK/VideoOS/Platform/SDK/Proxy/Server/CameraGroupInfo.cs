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
public class CameraGroupInfo : GroupInfo, ICameraGroupInfo, IGroupInfo
{
	private CameraGroupInfo[] cameraGroupsField;

	private Guid[] camerasField;

	ICameraGroupInfo[] ICameraGroupInfo.CameraGroups
	{
		get
		{
			return CameraGroups;
		}
		set
		{
			CameraGroups = (CameraGroupInfo[])value;
		}
	}

	public CameraGroupInfo[] CameraGroups
	{
		get
		{
			return cameraGroupsField;
		}
		set
		{
			cameraGroupsField = value;
		}
	}

	public Guid[] Cameras
	{
		get
		{
			return camerasField;
		}
		set
		{
			camerasField = value;
		}
	}
}
