namespace VideoOS.Management.Client.Proxy;

public interface ITaskDataContract : ISimpleDataContract, IDataContract
{
	string AsyncOperation { get; set; }
}
