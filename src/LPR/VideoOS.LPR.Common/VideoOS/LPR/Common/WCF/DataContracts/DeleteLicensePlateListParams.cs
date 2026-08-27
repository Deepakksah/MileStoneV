using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class DeleteLicensePlateListParams : IExtensibleDataObject
{
	[DataMember]
	public Guid Id { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
