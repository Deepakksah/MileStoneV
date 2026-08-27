using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcViewLocationFormatAttribute : Attribute
{
	public AspMvcViewLocationFormatAttribute(string format)
	{
	}
}
