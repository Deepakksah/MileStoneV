using VideoOS.Management.Client.Proxy;

namespace VideoOS.Management.Client;

public class NamedObjectKey : ObjectKeyBase
{
	public string Name { get; set; }

	public override bool IsNull => string.IsNullOrEmpty(Name);

	public NamedObjectKey(string name)
	{
		Name = name;
	}

	public override ObjectKeyBase Clone()
	{
		return new NamedObjectKey(Name);
	}

	public override string GetExceptionName()
	{
		return Name;
	}

	public override GenericObjectKey ToGenericObjectKey()
	{
		GenericObjectKey genericObjectKey = base.ToGenericObjectKey();
		genericObjectKey.Name = Name;
		return genericObjectKey;
	}
}
