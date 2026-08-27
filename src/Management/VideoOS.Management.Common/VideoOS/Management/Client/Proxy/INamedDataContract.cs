namespace VideoOS.Management.Client.Proxy;

public interface INamedDataContract : IDataContract
{
	string Name { get; set; }
}
