using System.Runtime.Serialization;

namespace VideoOS.Management.Client.Proxy;

[DataContract(Name = "CustomPropertyInfo", Namespace = "http://schemas.datacontract.org/2008/06/VideoOS.Management.Objects")]
public class CustomPropertyInfo : INamedDataContract, IDataContract
{
	[DataMember]
	public string ObjectId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Value { get; set; }

	public CustomPropertyInfo()
		: this(string.Empty, string.Empty, string.Empty)
	{
	}

	public CustomPropertyInfo(string name, string value)
		: this(string.Empty, name, value)
	{
	}

	public CustomPropertyInfo(string objectId, string name, string value)
	{
		ObjectId = objectId;
		Name = name;
		Value = value;
	}

	public ObjectKeyBase GetObjectKey()
	{
		return new NamedObjectKey(Name);
	}
}
