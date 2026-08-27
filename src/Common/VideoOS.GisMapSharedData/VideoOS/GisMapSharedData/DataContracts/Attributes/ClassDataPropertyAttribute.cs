using System;

namespace VideoOS.GisMapSharedData.DataContracts.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public sealed class ClassDataPropertyAttribute : Attribute
{
	private string _name;

	public string Name => _name;

	public ClassDataPropertyAttribute(string name)
	{
		_name = name;
	}
}
