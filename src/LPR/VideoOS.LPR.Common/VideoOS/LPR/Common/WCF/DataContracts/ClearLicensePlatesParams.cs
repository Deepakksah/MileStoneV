using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class ClearLicensePlatesParams : IExtensibleDataObject
{
	[DataMember]
	public Guid ListId { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
