using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.EventServer.Communication.AlarmClient;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GetEventCountParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
public class GetEventCountParams : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private SearchGroup SearchGroupField;

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
}
