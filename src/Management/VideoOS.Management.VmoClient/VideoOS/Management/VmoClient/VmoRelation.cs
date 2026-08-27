using System;
using System.Diagnostics;

namespace VideoOS.Management.VmoClient;

[DebuggerDisplay("Type: {Type.Name}; Id: {Id}; IsWeak: {IsWeakRelation}")]
public class VmoRelation
{
	public string Id { get; private set; }

	public Type Type { get; private set; }

	public bool IsWeakRelation { get; private set; }

	public VmoRelation(string entityName, string id)
		: this(entityName, id, isWeakRelation: false)
	{
	}

	public VmoRelation(string entityName, string id, bool isWeakRelation)
	{
		Id = id;
		Type = EntityNameAttributeHandlerWrapper.GetClassType(entityName);
		IsWeakRelation = isWeakRelation;
	}

	public override string ToString()
	{
		return Type.Name + ": " + Id + ", " + (IsWeakRelation ? "weak" : "strong");
	}
}
