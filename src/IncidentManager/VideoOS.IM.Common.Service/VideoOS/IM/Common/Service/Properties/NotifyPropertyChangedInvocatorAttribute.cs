using System;

namespace VideoOS.IM.Common.Service.Properties;

[AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class NotifyPropertyChangedInvocatorAttribute : Attribute
{
	[UsedImplicitly]
	public string ParameterName { get; private set; }

	public NotifyPropertyChangedInvocatorAttribute()
	{
	}

	public NotifyPropertyChangedInvocatorAttribute(string parameterName)
	{
		ParameterName = parameterName;
	}
}
