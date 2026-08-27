using System;
using System.CodeDom.Compiler;

namespace VideoOS.EventServer.Events.RestApi.Client;

[GeneratedCode("NJsonSchema", "14.0.7.0 (NJsonSchema v11.0.0.0 (Newtonsoft.Json v13.0.0.0))")]
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface, AllowMultiple = true)]
public class JsonInheritanceAttribute : Attribute
{
	public string Key { get; }

	public Type Type { get; }

	public JsonInheritanceAttribute(string key, Type type)
	{
		Key = key;
		Type = type;
	}
}
