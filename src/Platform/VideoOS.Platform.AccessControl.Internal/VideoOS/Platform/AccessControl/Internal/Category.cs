using System;

namespace VideoOS.Platform.AccessControl.Internal;

public abstract class Category
{
	public Guid Id { get; private set; }

	public string Name { get; protected set; }

	public Type AccessControlType { get; private set; }

	public Category(Guid id, string name, Type accessControlType)
	{
		Id = id;
		Name = name;
		AccessControlType = accessControlType;
	}

	public override string ToString()
	{
		return Name;
	}

	public override bool Equals(object obj)
	{
		if (obj is Category { Id: var id })
		{
			return id.Equals(Id);
		}
		return false;
	}

	public override int GetHashCode()
	{
		return Id.GetHashCode();
	}
}
