using System;

namespace VideoOS.Management.VmoClient;

public class VMOWebserviceObject
{
	public VmoPropertyCollection Properties { get; private set; }

	public VmoRelationCollection Relations { get; private set; }

	public string Id { get; private set; }

	public Type Type { get; private set; }

	public VMOWebserviceObject(VmoPropertyCollection properties, VmoRelationCollection relations, string id, Type type)
	{
		Properties = properties;
		Relations = relations;
		Id = id;
		Type = type;
	}

	public VMOWebserviceObject(VmoPropertyCollection properties, VmoRelationCollection relations, string id, string entityName)
	{
		Properties = properties;
		Relations = relations;
		Id = id;
		Type = EntityNameAttributeHandlerWrapper.GetClassType(entityName);
	}
}
