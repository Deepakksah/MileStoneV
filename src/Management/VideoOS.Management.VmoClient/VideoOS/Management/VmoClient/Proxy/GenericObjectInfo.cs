using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GenericObjectInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class GenericObjectInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private GenericObjectKey KeyField;

	private string KeyTypeField;

	private ObjectPropertyInfo[] PropertiesField;

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
	public GenericObjectKey Key
	{
		get
		{
			return KeyField;
		}
		set
		{
			KeyField = value;
		}
	}

	[DataMember]
	public string KeyType
	{
		get
		{
			return KeyTypeField;
		}
		set
		{
			KeyTypeField = value;
		}
	}

	[DataMember]
	public ObjectPropertyInfo[] Properties
	{
		get
		{
			return PropertiesField;
		}
		set
		{
			PropertiesField = value;
		}
	}
}
