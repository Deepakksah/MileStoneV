using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ViewGroupInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ViewGroupInfo : IViewGroupInfo
{
	private Guid IdField;

	private DateTime LastModifiedTimeField;

	private string LastModifiedUserField;

	private string NameField;

	private bool ReadOnlyField;

	private ViewGroupType ViewGroupTypeField;

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public DateTime LastModifiedTime
	{
		get
		{
			return LastModifiedTimeField;
		}
		set
		{
			LastModifiedTimeField = value;
		}
	}

	[DataMember]
	public string LastModifiedUser
	{
		get
		{
			return LastModifiedUserField;
		}
		set
		{
			LastModifiedUserField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}

	[DataMember]
	public bool ReadOnly
	{
		get
		{
			return ReadOnlyField;
		}
		set
		{
			ReadOnlyField = value;
		}
	}

	[DataMember]
	public ViewGroupType ViewGroupType
	{
		get
		{
			return ViewGroupTypeField;
		}
		set
		{
			ViewGroupTypeField = value;
		}
	}

	VideoOS.Platform.SDK.Interface.ViewGroupType IViewGroupInfo.ViewGroupType
	{
		get
		{
			_ = ViewGroupType;
			return VideoOS.Platform.SDK.Interface.ViewGroupType.Normal;
		}
		set
		{
			if (value == VideoOS.Platform.SDK.Interface.ViewGroupType.Normal)
			{
				ViewGroupType = ViewGroupType.Normal;
			}
			else
			{
				ViewGroupType = ViewGroupType.Normal;
			}
		}
	}
}
