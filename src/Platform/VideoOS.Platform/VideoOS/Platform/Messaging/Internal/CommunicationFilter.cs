using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Messaging.Internal;

[Serializable]
[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "3.0.0.0")]
[DataContract(Name = "CommunicationFilter", Namespace = "http://schemas.datacontract.org/2004/07/VideoOS.Platform.Messaging.Internal")]
public class CommunicationFilter : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	[DataMember(Name = "EndPoint", Order = 2, EmitDefaultValue = false)]
	public FQID EndPoint;

	[DataMember(Name = "EndPointType", Order = 3, EmitDefaultValue = false)]
	public EndPointType EndPointType = EndPointType.Any;

	[DataMember(Name = "Id", Order = 4, EmitDefaultValue = false)]
	public Guid Id;

	[DataMember(Name = "RequestedEndPoint", Order = 5, EmitDefaultValue = false)]
	public FQID RequesterEndPoint;

	[IgnoreDataMember]
	public long NumberOfFilterHits;

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

	[DataMember(Name = "CommunicationIdFilter", Order = 1, EmitDefaultValue = false)]
	public CommunicationIdFilter CommunicationIdFilter { get; set; }
}
