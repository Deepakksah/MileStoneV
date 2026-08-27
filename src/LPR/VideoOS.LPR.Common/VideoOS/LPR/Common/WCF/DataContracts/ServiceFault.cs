using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class ServiceFault : IExtensibleDataObject
{
	[DataMember]
	public ErrorCode ErrorCode { get; set; }

	[DataMember]
	public string ErrorMessage { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }

	public ServiceFault(ErrorCode errorCode, string errorMessage)
	{
		ErrorCode = errorCode;
		ErrorMessage = errorMessage;
	}
}
