using System.CodeDom.Compiler;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.Management.VmoClient.Proxy;

[DebuggerStepThrough]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "RelationPropertyInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
[DebuggerDisplay("RelationPropertyInfo; EntityName: {EntityNameField}, Id: {IdField} ({IsWeakRelationField})")]
public class RelationPropertyInfo : IExtensibleDataObject
{
	private ExtensionDataObject extensionDataField;

	private string EntityNameField;

	private string IdField;

	private bool IsWeakRelationField;

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
	public bool IsWeakRelation
	{
		get
		{
			return IsWeakRelationField;
		}
		set
		{
			IsWeakRelationField = value;
		}
	}
}
