using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.SDK.Proxy.Transact;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "WSTransactLine", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Transact.Webservice")]
public class WSTransactLine : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private int LineIdField;

	private int LineNumberField;

	private string TextField;

	private DateTime TimeStampField;

	private int TransactionIdField;

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
	public int LineId
	{
		get
		{
			return LineIdField;
		}
		set
		{
			LineIdField = value;
		}
	}

	[DataMember]
	public int LineNumber
	{
		get
		{
			return LineNumberField;
		}
		set
		{
			LineNumberField = value;
		}
	}

	[DataMember]
	public string Text
	{
		get
		{
			return TextField;
		}
		set
		{
			TextField = value;
		}
	}

	[DataMember]
	public DateTime TimeStamp
	{
		get
		{
			return TimeStampField;
		}
		set
		{
			TimeStampField = value;
		}
	}

	[DataMember]
	public int TransactionId
	{
		get
		{
			return TransactionIdField;
		}
		set
		{
			TransactionIdField = value;
		}
	}
}
