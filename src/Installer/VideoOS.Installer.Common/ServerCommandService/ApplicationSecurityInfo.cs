using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ApplicationSecurityInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class ApplicationSecurityInfo : IExtensibleDataObject
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
}
