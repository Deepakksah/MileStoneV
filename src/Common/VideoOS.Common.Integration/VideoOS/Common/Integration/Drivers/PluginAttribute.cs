using System;

namespace VideoOS.Common.Integration.Drivers;

[AttributeUsage(AttributeTargets.Class, Inherited = false)]
public sealed class PluginAttribute : Attribute
{
	public Guid Id { get; private set; }

	public PluginAttribute(string id)
	{
		Id = new Guid(id);
	}
}
