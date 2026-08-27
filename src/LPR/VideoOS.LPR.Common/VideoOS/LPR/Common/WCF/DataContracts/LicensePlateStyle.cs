using System.Runtime.Serialization;

namespace VideoOS.LPR.Common.WCF.DataContracts;

[DataContract(Namespace = "http://videoos.net/2/LPRCommand")]
public class LicensePlateStyle : IExtensibleDataObject
{
	[DataMember]
	public string Code { get; set; }

	public string Style
	{
		get
		{
			if (LicensePlateStyleAlias == null)
			{
				return Code;
			}
			return LicensePlateStyleAlias.Alias;
		}
	}

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public LicensePlateStyleAlias LicensePlateStyleAlias { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
