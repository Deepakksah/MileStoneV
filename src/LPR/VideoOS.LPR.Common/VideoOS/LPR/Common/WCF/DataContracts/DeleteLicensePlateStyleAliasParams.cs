using System;
using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class DeleteLicensePlateStyleAliasParams : IExtensibleDataObject
{
	[DataMember]
	public LicensePlateStyleAlias[] LicensePlateStyleAliases { get; set; } = Array.Empty<LicensePlateStyleAlias>();

	public ExtensionDataObject ExtensionData { get; set; }
}
