using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcAreaViewLocationFormatAttribute : Attribute
{
	public AspMvcAreaViewLocationFormatAttribute(string format)
	{
	}
}
