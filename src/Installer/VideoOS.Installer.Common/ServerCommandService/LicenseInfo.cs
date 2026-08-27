using System;
using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ServerCommandService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "LicenseInfo", Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public class LicenseInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string DataField;

	private Guid LicenseIdField;

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
