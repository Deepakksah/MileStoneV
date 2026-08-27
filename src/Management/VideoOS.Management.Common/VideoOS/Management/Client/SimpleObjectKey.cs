using System.Runtime.Serialization;
using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.Client;

public class SimpleObjectKey : ObjectKeyBase
{
	[DataMember]
	public string Id { get; set; }

	public override bool IsNull => string.IsNullOrEmpty(Id);

	public SimpleObjectKey(string id)
	{
		Id = id;
	}

	public override ObjectKeyBase Clone()
	{
		return new SimpleObjectKey(Id);
	}

	public override string GetExceptionName()
	{
		return Id.ToString();
	}

	public override GenericObjectKey ToGenericObjectKey()
	{
		GenericObjectKey genericObjectKey = base.ToGenericObjectKey();
		genericObjectKey.Id = Id;
		return genericObjectKey;
	}
}
