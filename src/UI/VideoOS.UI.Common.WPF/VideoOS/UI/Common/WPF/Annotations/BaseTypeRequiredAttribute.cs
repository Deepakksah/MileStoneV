using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
[BaseTypeRequired(typeof(Attribute))]
public sealed class BaseTypeRequiredAttribute : Attribute
{
	[NotNull]
	public Type BaseType { get; private set; }

	public BaseTypeRequiredAttribute([NotNull] Type baseType)
	{
		BaseType = baseType;
	}
}
