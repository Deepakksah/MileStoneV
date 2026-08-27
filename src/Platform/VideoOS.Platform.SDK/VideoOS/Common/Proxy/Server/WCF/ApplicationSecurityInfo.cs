using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.Serialization;
using VideoOS.Platform.SDK.Interface;

namespace VideoOS.Common.Proxy.Server.WCF;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ApplicationSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
[ExcludeFromCodeCoverage]
public class ApplicationSecurityInfo : IExtensibleDataObject, IApplicationSecurityInfo
{
	private ExtensionDataObject extensionDataField;

	private SmartClientSecurityInfo SmartClientSecurityField;

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

	[DataMember]
	public SmartClientSecurityInfo SmartClientSecurity
	{
		get
		{
			return SmartClientSecurityField;
		}
		set
		{
			SmartClientSecurityField = value;
		}
	}

	ISmartClientSecurityInfo IApplicationSecurityInfo.SmartClientSecurity
	{
		get
		{
			return SmartClientSecurity;
		}
		set
		{
			SmartClientSecurity = (SmartClientSecurityInfo)value;
		}
	}
}
