using System;
using System.Collections.Generic;
using System.Diagnostics;
using VideoOS.Common.Interfaces.Licensing;

namespace VideoOS.Server.Licensing;

[DebuggerDisplay("Id: {Id}")]
public class RecorderRequestInfo
{
	public Guid Id { get; private set; }

	public bool Enabled { get; set; }

	public List<IHardwareInfo> Hardware { get; private set; }

	public List<IDeviceInfo> Devices { get; private set; }

	public RecorderRequestInfo(Guid recorderId)
	{
		Id = recorderId;
		Hardware = new List<IHardwareInfo>();
		Devices = new List<IDeviceInfo>();
	}
}
