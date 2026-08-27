using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform;
using VideoOS.Platform.Proxy.Alarm;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchElement", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
[KnownType(typeof(Guid[]))]
[KnownType(typeof(string[]))]
[KnownType(typeof(GetEventLines2Params))]
[KnownType(typeof(SearchGroup))]
[KnownType(typeof(LogicalOperator))]
[KnownType(typeof(SearchElement[]))]
[KnownType(typeof(SearchOperator))]
[KnownType(typeof(SearchGroup[]))]
[KnownType(typeof(SearchOrders))]
[KnownType(typeof(SearchOrder[]))]
[KnownType(typeof(SearchOrder))]
[KnownType(typeof(Direction))]
[KnownType(typeof(GetEventLines2Resp))]
[KnownType(typeof(EventLine[]))]
[KnownType(typeof(EventLine))]
[KnownType(typeof(AlarmServiceFault))]
[KnownType(typeof(AlarmFaultType))]
[KnownType(typeof(Tuple<Guid, Guid>[]))]
[KnownType(typeof(Tuple<Guid, Guid>))]
[KnownType(typeof(KeyValuePair<string, string>[]))]
[KnownType(typeof(KeyValuePair<string, string>))]
[KnownType(typeof(FQID[]))]
[KnownType(typeof(FQID))]
[KnownType(typeof(ServerId))]
[KnownType(typeof(FolderType))]
public class SearchElement : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private SearchOperator SearchOperatorField;

	private string SearchTargetField;

	private object SearchValueField;

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
	public SearchOperator SearchOperator
	{
		get
		{
			return SearchOperatorField;
		}
		set
		{
			SearchOperatorField = value;
		}
	}

	[DataMember]
	public string SearchTarget
	{
		get
		{
			return SearchTargetField;
		}
		set
		{
			SearchTargetField = value;
		}
	}

	[DataMember]
	public object SearchValue
	{
		get
		{
			return SearchValueField;
		}
		set
		{
			SearchValueField = value;
		}
	}
}
