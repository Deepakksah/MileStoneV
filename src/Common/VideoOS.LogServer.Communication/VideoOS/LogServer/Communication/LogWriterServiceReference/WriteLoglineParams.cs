using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WriteLoglineParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class WriteLoglineParams : WriteLoglineParamsBase
{
	[OptionalField]
	private Guid MessageIdField;

	[DataMember]
	public Guid MessageId
	{
		get
		{
			return MessageIdField;
		}
		set
		{
			if (!MessageIdField.Equals(value))
			{
				MessageIdField = value;
				RaisePropertyChanged("MessageId");
			}
		}
	}
}
