using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CameraGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CameraGroupInfo : GroupInfo
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
}
