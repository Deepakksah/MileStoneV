using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.SecurityService;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AccessControlEntry", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class AccessControlEntry : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private bool AccessDenyField;

	private string ActionIdField;

	private string SidField;

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
	public bool AccessDeny
	{
		get
		{
			return AccessDenyField;
		}
		set
		{
			AccessDenyField = value;
		}
	}

	[DataMember]
	public string ActionId
	{
		get
		{
			return ActionIdField;
		}
		set
		{
			ActionIdField = value;
		}
	}

	[DataMember]
	public string Sid
	{
		get
		{
			return SidField;
		}
		set
		{
			SidField = value;
		}
	}
}
