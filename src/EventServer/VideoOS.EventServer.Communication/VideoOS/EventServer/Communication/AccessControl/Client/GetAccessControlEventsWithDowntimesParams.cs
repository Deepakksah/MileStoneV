using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AccessControl.Client;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetAccessControlEventsWithDowntimesParams", Namespace = "http://videoos.net/2/EventServerAccessControlCommand")]
public class GetAccessControlEventsWithDowntimesParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private DateTime EndField;

	private Guid[] IdsField;

	private int PageField;

	private int PageSizeField;

	private SearchGroup SearchGroupField;

	private Guid SearchIdField;

	private SearchOrders SearchOrdersField;

	private DateTime StartField;

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
	public DateTime End
	{
		get
		{
			return EndField;
		}
		set
		{
			EndField = value;
		}
	}

	[DataMember]
	public Guid[] Ids
	{
		get
		{
			return IdsField;
		}
		set
		{
			IdsField = value;
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
	public DateTime Start
	{
		get
		{
			return StartField;
		}
		set
		{
			StartField = value;
		}
	}
}
