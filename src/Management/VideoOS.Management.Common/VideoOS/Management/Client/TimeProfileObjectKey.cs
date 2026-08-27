namespace VideoOS.Management.Client;

public class TimeProfileObjectKey : NamedObjectKey
{
	public string Type { get; set; }

	public TimeProfileObjectKey(string type, string name)
		: base(name)
	{
		Type = type;
	}

	public override ObjectKeyBase Clone()
	{
		return new TimeProfileObjectKey(Type, base.Name);
	}
}
