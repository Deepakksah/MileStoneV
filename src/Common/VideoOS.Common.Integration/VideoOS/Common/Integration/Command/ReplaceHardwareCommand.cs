using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public class ReplaceHardwareCommand : CommonCommandBase
{
	[DataMember]
	public Guid RecorderId { get; private set; }

	[DataMember]
	public string RecorderName { get; private set; }

	[DataMember]
	public Uri Uri { get; private set; }

	[DataMember]
	public string UserName { get; private set; }

	[DataMember]
	public string Password { get; set; }

	[DataMember]
	public int DriverNumber { get; private set; }

	[DataMember]
	public string ProductId { get; private set; }

	[DataMember]
	public string NewAuditHardwareName { get; private set; }

	[DataMember]
	public string NewDefinitionXml { get; private set; }

	[DataMember]
	public string NewSettingsXml { get; private set; }

	[DataMember]
	public Guid NewHardwareDriverID { get; private set; }

	[DataMember]
	public Guid OldHardwareID { get; private set; }

	[DataMember]
	public RHInheritedDevice[] InheritedDevices { get; private set; }

	[DataMember]
	public Guid[] RemovedDevices { get; private set; }

	[DataMember]
	public Guid[] RemovedEvents { get; private set; }

	public ReplaceHardwareCommand(Guid recorderId, string recorderName, Uri uri, string userName, string password, int driverNumber, string productId, string newAuditHardwareName, string newDefinitionXml, string newSettingsXml, Guid newHardwareDriverID, Guid oldHardwareID, IEnumerable<RHInheritedDevice> inheritedDevices, IEnumerable<Guid> removedDevices, IEnumerable<Guid> removedEvents)
	{
		RecorderId = recorderId;
		RecorderName = recorderName;
		Uri = uri;
		UserName = userName;
		Password = password;
		DriverNumber = driverNumber;
		ProductId = productId;
		NewAuditHardwareName = newAuditHardwareName;
		NewDefinitionXml = newDefinitionXml;
		NewSettingsXml = newSettingsXml;
		NewHardwareDriverID = newHardwareDriverID;
		OldHardwareID = oldHardwareID;
		InheritedDevices = inheritedDevices.ToArray();
		RemovedDevices = removedDevices.ToArray();
		RemovedEvents = removedEvents.ToArray();
	}

	public override void Accept(ICommandVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonCommandVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}
}
