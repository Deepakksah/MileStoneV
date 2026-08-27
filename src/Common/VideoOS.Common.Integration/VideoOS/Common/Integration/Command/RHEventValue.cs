using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RHEventValue
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. Device hwid: {1}; Event hwid: {2}", "RHEventValue", DeviceHWId, EventHWId));

	[DataMember]
	public string DeviceHWId { get; private set; }

	[DataMember]
	public string DeviceName { get; private set; }

	[DataMember]
	public string EventHWId { get; private set; }

	[DataMember]
	public Guid EventNameId { get; private set; }

	public RHEventValue(string deviceHWId, string deviceName, string eventHWId, Guid eventNameId)
	{
		DeviceHWId = deviceHWId;
		DeviceName = deviceName;
		EventHWId = eventHWId;
		EventNameId = eventNameId;
	}
}
