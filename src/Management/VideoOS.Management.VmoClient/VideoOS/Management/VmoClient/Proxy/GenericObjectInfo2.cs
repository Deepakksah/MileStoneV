using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "GenericObjectInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class GenericObjectInfo2 : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string EntityNameField;

	private string IdField;

	private ObjectPropertyInfo2[] PropertiesField;

	private RelationPropertyInfo[] RelationsField;

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
	public string EntityName
	{
		get
		{
			return EntityNameField;
		}
		set
		{
			EntityNameField = value;
		}
	}

	[DataMember]
	public string Id
	{
		get
		{
			return IdField;
		}
		set
		{
			IdField = value;
		}
	}

	[DataMember]
	public ObjectPropertyInfo2[] Properties
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

	[DataMember]
	public RelationPropertyInfo[] Relations
	{
		get
		{
			return RelationsField;
		}
		set
		{
			RelationsField = value;
		}
	}
}
