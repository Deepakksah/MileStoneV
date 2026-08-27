using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "RelationPropertyInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class RelationProperty
{
	[DataMember]
	public string EntityName { get; set; }

	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public bool IsWeakRelation { get; set; }

	public RelationProperty(string entityName, string id)
		: this(entityName, id, isWeakRelation: false)
	{
	}

	public RelationProperty(string entityName, string id, bool isWeakRelation)
	{
		EntityName = entityName;
		Id = id;
		IsWeakRelation = isWeakRelation;
	}
}
