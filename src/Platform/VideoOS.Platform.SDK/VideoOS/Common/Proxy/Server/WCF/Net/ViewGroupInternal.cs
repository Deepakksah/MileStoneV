using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "ViewGroupInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
public class ViewGroupInternal : IViewGroup
{
	private ViewGroupInternalData ViewGroupDataField;

	private ViewGroupInfo ViewGroupInfoField;

	[DataMember]
	public ViewGroupInternalData ViewGroupData
	{
		get
		{
			return ViewGroupDataField;
		}
		set
		{
			ViewGroupDataField = value;
		}
	}

	[DataMember]
	public ViewGroupInfo ViewGroupInfo
	{
		get
		{
			return ViewGroupInfoField;
		}
		set
		{
			ViewGroupInfoField = value;
		}
	}

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
			ViewGroupData = ViewGroupData;
		}
	}
}
