using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CameraGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
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
