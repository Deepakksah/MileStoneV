using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Transact;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "WSTransactSource", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
public class WSTransactSource : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int DaysToKeepTransactionsField;

	private Guid EventGroupIdField;

	private Guid IdField;

	private string NameField;

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
	public int DaysToKeepTransactions
	{
		get
		{
			return DaysToKeepTransactionsField;
		}
		set
		{
			DaysToKeepTransactionsField = value;
		}
	}

	[DataMember]
	public Guid EventGroupId
	{
		get
		{
			return EventGroupIdField;
		}
		set
		{
			EventGroupIdField = value;
		}
	}

	[DataMember]
	public Guid Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public string Name
	{
		get
		{
			return NameField;
		}
		set
		{
			NameField = value;
		}
	}
}
