using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.ConfigurationApi.ClientService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "PropertyFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.ConfigurationAPI")]
public class PropertyFilter : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string KeyField;

	private Operator OperatorField;

	private string ValueField;

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
	public string Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
		}
	}

	[DataMember]
	public Operator Operator
	{
		get
		{
			return OperatorField;
		}
		set
		{
			OperatorField = value;
		}
	}

	[DataMember]
	public string Value
	{
		get
		{
			return ValueField;
		}
		set
		{
			ValueField = value;
		}
	}

	public PropertyFilter()
	{
	}

	public PropertyFilter(string key, Operator opr, string value)
	{
		Key = key;
		Operator = opr;
		Value = value;
	}
}
