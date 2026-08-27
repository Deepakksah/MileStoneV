using System;

namespace VideoOS.UI.Common.WPF.Annotations;

[AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
public sealed class AspMvcMasterLocationFormatAttribute : Attribute
{
	public AspMvcMasterLocationFormatAttribute(string format)
	{
	}
}
