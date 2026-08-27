namespace VideoOS.Management.Client;

public class HostingServerObjectKey : NamedObjectKey
{
	public string Type { get; set; }

	public HostingServerObjectKey(string type, string name)
		: base(name)
	{
		Type = type;
	}

	public override ObjectKeyBase Clone()
	{
		return new HostingServerObjectKey(Type, base.Name);
	}
}
