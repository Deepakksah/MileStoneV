namespace VideoOS.Management.Client.Proxy;

public interface INumberedDataContract : IDataContract
{
	int Position { get; set; }
}
