using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Proxy.Alarm;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AlarmSearchCriteria", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Proxy.Alarm")]
[KnownType(typeof(NameNumberPairValue))]
public class AlarmSearchCriteria : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string CriteriaField;

	private object[] ValuesField;

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
	public string Criteria
	{
		get
		{
			return CriteriaField;
		}
		set
		{
			CriteriaField = value;
		}
	}

	[DataMember]
	public object[] Values
	{
		get
		{
			return ValuesField;
		}
		set
		{
			ValuesField = value;
		}
	}
}
