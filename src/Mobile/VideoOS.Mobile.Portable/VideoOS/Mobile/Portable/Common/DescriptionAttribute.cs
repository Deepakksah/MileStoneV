using System;

namespace VideoOS.Mobile.Portable.Common;

[AttributeUsage(AttributeTargets.All)]
public class DescriptionAttribute : Attribute
{
	protected string DescriptionValue { get; set; }

	public virtual string Description => DescriptionValue;

	public DescriptionAttribute()
	{
	}

	public DescriptionAttribute(string description)
	{
		DescriptionValue = description;
	}
}
