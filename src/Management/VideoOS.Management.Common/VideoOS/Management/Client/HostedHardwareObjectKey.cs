namespace VideoOS.Management.Client;

public class HostedHardwareObjectKey : NamedObjectKey
{
	public string Type { get; set; }

	public HostedHardwareObjectKey(string type, string name)
		: base(name)
	{
		Type = type;
	}

	public override ObjectKeyBase Clone()
	{
		return new HostedHardwareObjectKey(Type, base.Name);
	}
}
