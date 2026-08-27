using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.LogServer.Communication.LogWriterServiceReference;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "WriteLegacyLoglineParams", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.LogServer.WebService.DataContracts")]
public class WriteLegacyLoglineParams : WriteLoglineParamsBase
{
	[OptionalField]
	private string MessageIdField;

	[DataMember]
	public string MessageId
	{
		get
		{
			return MessageIdField;
		}
		set
		{
			if ((object)MessageIdField != value)
			{
				MessageIdField = value;
				RaisePropertyChanged("MessageId");
			}
		}
	}
}
