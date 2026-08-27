using System;

namespace VideoOS.Mobile.Portable.Utilities.Json;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter | AttributeTargets.ReturnValue)]
public class JsonElementAttribute : JsonSerializerAttribute
{
	public string Name { get; set; }

	public JsonElementAttribute()
	{
	}

	public JsonElementAttribute(string name)
	{
		Name = name;
	}
}
