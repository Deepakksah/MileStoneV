using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "SearchGroup", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class SearchGroup : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private LogicalOperator GroupOperatorField;

	private SearchElement[] SearchElementsField;

	private SearchGroup[] SearchGroupsField;

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
	public LogicalOperator GroupOperator
	{
		get
		{
			return GroupOperatorField;
		}
		set
		{
			GroupOperatorField = value;
		}
	}

	[DataMember]
	public SearchElement[] SearchElements
	{
		get
		{
			return SearchElementsField;
		}
		set
		{
			SearchElementsField = value;
		}
	}

	[DataMember]
	public SearchGroup[] SearchGroups
	{
		get
		{
			return SearchGroupsField;
		}
		set
		{
			SearchGroupsField = value;
		}
	}
}
