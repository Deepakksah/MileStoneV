using System.Runtime.Serialization;
using VideoOS.Management.Client.Proxy;
using VideoOS.Management.Common;

namespace VideoOS.Management.Client;

public class DeviceObjectKey : SimpleObjectKey
{
	[DataMember]
	public DeviceTypesOld DeviceType{ get; set; }

	public DeviceObjectKey(string id, DeviceTypesOld deviceType)
		: base(id)
	{
		DeviceType = deviceType;
	}

	public override ObjectKeyBase Clone()
	{
		return new DeviceObjectKey(base.Id, DeviceType);
	}

	public override GenericObjectKey ToGenericObjectKey()
	{
		GenericObjectKey genericObjectKey = base.ToGenericObjectKey();
		genericObjectKey.Type = DeviceType.ToString();
		return genericObjectKey;
	}
}
