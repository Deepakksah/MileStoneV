using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class GetLicensePlateCountParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ListId { get; set; }

	[DataMember]
	public string SearchFilter { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
