using VideoOS.Management.Common;

namespace VideoOS.Management.Client.Proxy;

public interface IDeviceDataContract : ISimpleDataContract, IDataContract
{
	DeviceTypesOld DeviceType { get; set; }
}
