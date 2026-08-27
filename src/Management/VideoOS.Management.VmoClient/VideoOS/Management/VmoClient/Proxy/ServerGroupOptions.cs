using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "ServerGroupOptions", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class ServerGroupOptions : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private CustomPropertyInfo[] CustomPropertiesField;

	private string UserDefinedIdField;

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
	public CustomPropertyInfo[] CustomProperties
	{
		get
		{
			return CustomPropertiesField;
		}
		set
		{
			CustomPropertiesField = value;
		}
	}

	[DataMember]
	public string UserDefinedId
	{
		get
		{
			return UserDefinedIdField;
		}
		set
		{
			UserDefinedIdField = value;
		}
	}
}
