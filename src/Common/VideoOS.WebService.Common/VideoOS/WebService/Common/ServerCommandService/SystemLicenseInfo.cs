using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("SystemLicenseInfo: {SLC}")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class SystemLicenseInfo
{
	[DataMember]
	public bool HasLicense { get; set; }

	[DataMember]
	public bool IsTrialLicense { get; set; }

	[DataMember]
	public string SLC { get; set; }

	[DataMember]
	public string SLCEnvironment { get; set; }

	[DataMember]
	public string ProductSku { get; set; }

	[DataMember]
	public bool Expires { get; set; }

	[DataMember]
	public DateTime ExpiryDate { get; set; }

	[DataMember]
	public string[] Features { get; set; }
}
