using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
[DebuggerDisplay("{DebuggerDisplay,nq}")]
public class RHInheritedDevice
{
	public string DebuggerDisplay => FormattableString.Invariant(FormattableStringFactory.Create("{0}. New hwid: {1}", "RHInheritedDevice", NewDeviceHwidText));

	[DataMember]
	public string NewDeviceHwidText { get; private set; }

	[DataMember]
	public string NewDeviceName { get; private set; }

	[DataMember]
	public string NewAuditDeviceName { get; private set; }

	[DataMember]
	public string NewSettingsXml { get; private set; }

	[DataMember]
	public string NewPresetsXml { get; private set; }

	[DataMember]
	public Guid OldDeviceID { get; private set; }

	[DataMember]
	public bool TransferDeviceInternalPresets { get; private set; }

	public RHInheritedDevice(string newDeviceHwidText, string newDeviceName, string newAuditDeviceName, string newSettingsXml, string newPresetsXml, Guid oldDeviceID, bool transferDevicePresets)
	{
		NewDeviceHwidText = newDeviceHwidText;
		NewDeviceName = newDeviceName;
		NewAuditDeviceName = newAuditDeviceName;
		NewSettingsXml = newSettingsXml;
		NewPresetsXml = newPresetsXml;
		OldDeviceID = oldDeviceID;
		TransferDeviceInternalPresets = transferDevicePresets;
	}
}
