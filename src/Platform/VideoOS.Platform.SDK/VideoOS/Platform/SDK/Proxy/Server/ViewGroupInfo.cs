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
public class ViewGroupInfo : IViewGroupInfo
{
	private Guid idField;

	private string nameField;

	private ViewGroupType viewGroupTypeField;

	private bool readOnlyField;

	private DateTime lastModifiedTimeField;

	private string lastModifiedUserField;

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

	public Guid Id
	{
		get
		{
			return idField;
		}
		set
		{
			idField = value;
		}
	}

	public string Name
	{
		get
		{
			return nameField;
		}
		set
		{
			nameField = value;
		}
	}

	public ViewGroupType ViewGroupType
	{
		get
		{
			return viewGroupTypeField;
		}
		set
		{
			viewGroupTypeField = value;
		}
	}

	public bool ReadOnly
	{
		get
		{
			return readOnlyField;
		}
		set
		{
			readOnlyField = value;
		}
	}

	public DateTime LastModifiedTime
	{
		get
		{
			return lastModifiedTimeField;
		}
		set
		{
			lastModifiedTimeField = value;
		}
	}

	public string LastModifiedUser
	{
		get
		{
			return lastModifiedUserField;
		}
		set
		{
			lastModifiedUserField = value;
		}
	}
}
