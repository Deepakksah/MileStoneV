using System;

namespace VideoOS.Common.Drivers;

[AttributeUsage(AttributeTargets.Interface, Inherited = false)]
public sealed class DeviceInterfaceAttribute : Attribute
{
	private Guid _guid;

	public Guid Guid => _guid;

	public DeviceInterfaceAttribute(string guid)
	{
		_guid = new Guid(guid);
	}
}
