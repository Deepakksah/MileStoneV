using System;
using System.Diagnostics;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Management.Server;

[DebuggerDisplay("{Name}")]
[DataContract(Name = "ObjectPropertyInfo2", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public sealed class ObjectProperty2
{
	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public object Value { get; set; }

	private void Initialize(string propertyName, object propertyValue)
	{
		Name = propertyName;
		Value = propertyValue;
	}

	private ObjectProperty2()
	{
	}

	public ObjectProperty2(string propertyName, string propertyValue)
	{
		Initialize(propertyName, propertyValue);
	}

	public ObjectProperty2(string propertyName, int propertyValue)
	{
		Initialize(propertyName, propertyValue);
	}

	public ObjectProperty2(string propertyName, double propertyValue)
	{
		Initialize(propertyName, propertyValue);
	}

	public ObjectProperty2(string propertyName, DateTime propertyValue)
	{
		Initialize(propertyName, propertyValue);
	}

	public ObjectProperty2(string propertyName, bool propertyValue)
	{
		Initialize(propertyName, propertyValue);
	}

	public ObjectProperty2(string propertyName, ObjectProperty2[] propertyValue)
	{
		string propertyName2 = NestedPropertyHelper.FormatNestedPropertyName(propertyName);
		Initialize(propertyName2, propertyValue);
	}

	public static ObjectProperty2 FromKeyAndValue(KeyAndValue kv)
	{
		ObjectProperty2 objectProperty = new ObjectProperty2();
		objectProperty.Initialize(kv.Key, kv.Value);
		return objectProperty;
	}
}
