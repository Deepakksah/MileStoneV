using System;
using VideoOS.Management.Common;

namespace VideoOS.Management.VmoClient;

[EntityName("HardwareDriver")]
public class HardwareDriver
{
	public VmoPropertyCollection ObjectProperties { get; private set; }

	public int DriverNumber => ObjectProperties.GetPropertyValue<int>("Number");

	public string Name => ObjectProperties.GetPropertyValue<string>("Name");

	public string GroupName => ObjectProperties.GetPropertyValue<string>("GroupName");

	public int UseCount => ObjectProperties.GetPropertyValue<int>("UseCount");

	public Guid Id => ObjectProperties.GetPropertyValue<Guid>("Id");

	public string DriverVersion => ObjectProperties.GetPropertyValue<string>("DriverVersion");

	public DriverType DriverType => ObjectProperties.GetPropertyValue<DriverType>("DriverType");

	public string Revision => ObjectProperties.GetPropertyValue<string>("DriverRevision");

	public HardwareDriver(VmoPropertyCollection objectProperties)
	{
		ObjectProperties = objectProperties;
	}
}
