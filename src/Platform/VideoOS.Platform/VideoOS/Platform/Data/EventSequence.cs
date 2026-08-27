using System;
using System.Drawing;
using System.Runtime.Serialization;

namespace VideoOS.Platform.Data;

[DataContract(Name = "EventSequence", Namespace = "urn:milestone-systems")]
public class EventSequence : IExtensibleDataObject
{
	[DataMember]
	public DateTime StartDateTime { get; set; }

	[DataMember]
	public DateTime EndDateTime { get; set; }

	public Image[] SnapshotImages { get; set; }

	[DataMember]
	public byte[][] SnapshotJPEGs { get; set; }

	public ExtensionDataObject ExtensionData { get; set; }
}
