using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetEventLines2Params", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class GetEventLines2Params : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int PageField;

	private int PageSizeField;

	private SearchGroup SearchGroupField;

	private Guid SearchIdField;

	private SearchOrders SearchOrdersField;

	private bool TruePagingField;

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
	public int Page
	{
		get
		{
			return PageField;
		}
		set
		{
			PageField = value;
		}
	}

	[DataMember]
	public int PageSize
	{
		get
		{
			return PageSizeField;
		}
		set
		{
			PageSizeField = value;
		}
	}

	[DataMember]
	public SearchGroup SearchGroup
	{
		get
		{
			return SearchGroupField;
		}
		set
		{
			SearchGroupField = value;
		}
	}

	[DataMember]
	public Guid SearchId
	{
		get
		{
			return SearchIdField;
		}
		set
		{
			SearchIdField = value;
		}
	}

	[DataMember]
	public SearchOrders SearchOrders
	{
		get
		{
			return SearchOrdersField;
		}
		set
		{
			SearchOrdersField = value;
		}
	}

	[DataMember]
	public bool TruePaging
	{
		get
		{
			return TruePagingField;
		}
		set
		{
			TruePagingField = value;
		}
	}
}
