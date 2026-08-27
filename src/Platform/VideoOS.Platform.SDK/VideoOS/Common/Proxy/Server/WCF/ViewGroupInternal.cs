using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ViewGroupInternal", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Common.Proxy.Server")]
[ExcludeFromCodeCoverage]
public class ViewGroupInternal : IExtensibleDataObject, IViewGroup
{
	private ExtensionDataObject extensionDataField;

	private ViewGroupInternalData ViewGroupDataField;

	private ViewGroupInfo ViewGroupInfoField;

	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

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
