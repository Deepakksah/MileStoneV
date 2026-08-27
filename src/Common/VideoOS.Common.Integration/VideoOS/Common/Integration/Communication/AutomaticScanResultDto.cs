using System;

namespace VideoOS.Common.Integration.Communication;

[Serializable]
[Obsolete("We need to keep class only because it is used in legacy interfaces.")]
public class AutomaticScanResultDto
{
	public string Manufacturer { get; private set; }

	public string IPAddress { get; private set; }

	public bool Validated { get; private set; }

	public int Port { get; private set; }

	public int DeviceId { get; private set; }

	public string DllName { get; private set; }

	public int DriverNumber { get; private set; }

	public string UserName { get; private set; }

	public string Password { get; private set; }

	public string Product { get; private set; }

	public string MacAddress { get; private set; }

	public Guid RecordingServerId { get; private set; }

	public AutomaticScanResultDto()
	{
		Manufacturer = null;
		IPAddress = null;
		Validated = false;
		Port = 0;
		DeviceId = 0;
		DllName = null;
		DriverNumber = 0;
		UserName = null;
		Password = null;
		Product = null;
		MacAddress = null;
		RecordingServerId = Guid.Empty;
	}
}
