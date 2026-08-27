using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, Inherited = true)]
public sealed class HtmlAttributeValueAttribute : Attribute
{
	[NotNull]
	public string Name { get; private set; }

	public HtmlAttributeValueAttribute([NotNull] string name)
	{
		Name = name;
	}
}
