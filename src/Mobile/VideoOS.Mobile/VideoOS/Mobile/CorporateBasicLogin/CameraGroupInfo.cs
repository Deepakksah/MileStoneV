using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Mobile.CorporateBasicLogin;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "CameraGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class CameraGroupInfo : GroupInfo
{
	[OptionalField]
	private CameraGroupInfo[] CameraGroupsField;

	[OptionalField]
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
			if (CameraGroupsField != value)
			{
				CameraGroupsField = value;
				RaisePropertyChanged("CameraGroups");
			}
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
			if (CamerasField != value)
			{
				CamerasField = value;
				RaisePropertyChanged("Cameras");
			}
		}
	}
}
