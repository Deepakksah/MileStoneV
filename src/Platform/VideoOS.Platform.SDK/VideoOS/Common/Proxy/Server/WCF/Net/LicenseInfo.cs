using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF.Net;

[DebuggerStepThrough]
[GeneratedCode("Microsoft.Tools.ServiceModel.Svcutil", "2.1.0")]
[DataContract(Name = "LicenseInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LicenseInfo : ILicenseInfo
{
	private string DataField;

	private Guid LicenseIdField;

	[DataMember]
	public string Data
	{
		get
		{
			return DataField;
		}
		set
		{
			DataField = value;
		}
	}

	[DataMember]
	public Guid LicenseId
	{
		get
		{
			return LicenseIdField;
		}
		set
		{
			LicenseIdField = value;
		}
	}
}
