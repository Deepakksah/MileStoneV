using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "AccessControlTimeProfileEntry", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class AccessControlTimeProfileEntry : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string ActionIdField;

	private bool IsDefaultTimeProfileField;

	private string SidField;

	private string TimeProfileIdField;

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
	public bool IsDefaultTimeProfile
	{
		get
		{
			return IsDefaultTimeProfileField;
		}
		set
		{
			IsDefaultTimeProfileField = value;
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

	[DataMember]
	public string TimeProfileId
	{
		get
		{
			return TimeProfileIdField;
		}
		set
		{
			TimeProfileIdField = value;
		}
	}
}
