using System.Runtime.Serialization;

namespace VideoOS.Management.Server;

[DataContract(Name = "GenericObjectInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class GenericObject2
{
	[DataMember]
	public string EntityName { get; set; }

	[DataMember]
	public string Id { get; set; }

	[DataMember]
	public ObjectProperty2[] Properties { get; set; }

	[DataMember]
	public RelationProperty[] Relations { get; set; }

	public GenericObject2(string id, string entityName, ObjectProperty2[] properties, RelationProperty[] relations)
	{
		EntityName = entityName;
		Id = id;
		Properties = properties;
		Relations = relations;
	}
}
