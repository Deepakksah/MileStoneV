using System;

namespace VideoOS.Management.Common;

[AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
public sealed class FunctionHandlerAttribute : Attribute
{
	public string Name { get; private set; }

	public FunctionHandlerAttribute(string name)
	{
		Name = name;
	}
}
