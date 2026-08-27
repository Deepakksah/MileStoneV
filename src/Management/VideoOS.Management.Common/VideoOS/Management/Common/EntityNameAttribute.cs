using System;

namespace VideoOS.Management.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class EntityNameAttribute : Attribute
{
	public string Name { get; private set; }

	public EntityNameAttribute(string name)
	{
		Name = name;
	}
}
