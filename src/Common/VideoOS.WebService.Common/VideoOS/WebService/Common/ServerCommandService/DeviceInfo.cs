using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace VideoOS.WebService.Common.ServerCommandService;

[DebuggerDisplay("DeviceInfo: {Name} ({DeviceId})")]
[DataContract(Namespace = "http://videoos.net/2/XProtectCSServerCommand")]
public abstract class DeviceInfo
{
	[DataMember]
	public Guid RecorderId { get; set; }

	[DataMember]
	public Guid HardwareId { get; set; }

	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public string Name { get; set; }

	[DataMember]
	public string Description { get; set; }

	[DataMember]
	public string Shortcut { get; set; }

	[DataMember]
	public int DeviceIndex { get; set; }

	[DataMember]
	public string GisPoint { get; set; }

	[DataMember]
	public string ShortName { get; set; }

	[DataMember]
	public int Icon { get; set; }

	[DataMember]
	public double CoverageDirection { get; set; }

	[DataMember]
	public double CoverageDepth { get; set; }

	[DataMember]
	public double CoverageFieldOfView { get; set; }
}
