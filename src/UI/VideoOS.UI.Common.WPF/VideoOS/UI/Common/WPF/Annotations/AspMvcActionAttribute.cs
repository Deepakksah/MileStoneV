using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Parameter)]
public sealed class AspMvcActionAttribute : Attribute
{
	[NotNull]
	public string AnonymousProperty { get; private set; }

	public AspMvcActionAttribute()
	{
	}

	public AspMvcActionAttribute([NotNull] string anonymousProperty)
	{
		AnonymousProperty = anonymousProperty;
	}
}
