using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "CameraGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CameraGroupInfo : GroupInfo, ICameraGroupInfo, IGroupInfo
{
	private CameraGroupInfo[] CameraGroupsField;

	private Guid[] CamerasField;

	[DataMember]
	public CameraGroupInfo[] CameraGroups
	{
		get
		{
			return CameraGroupsField;
		}
		set
		{
			CameraGroupsField = value;
		}
	}

	[DataMember]
	public Guid[] Cameras
	{
		get
		{
			return CamerasField;
		}
		set
		{
			CamerasField = value;
		}
	}

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
}
