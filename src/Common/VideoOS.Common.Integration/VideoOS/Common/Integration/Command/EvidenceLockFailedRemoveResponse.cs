using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace VideoOS.Common.Integration.Command;

[Serializable]
[DataContract]
public sealed class EvidenceLockFailedRemoveResponse : CommonResponseBase
{
	[DataMember]
	public EvidenceLockFailedRemoveEntityType EntityType { get; private set; }

	[DataMember]
	public string EntityName { get; private set; }

	[DataMember]
	public Collection<LockedDevices> FaultDevices { get; private set; } = new Collection<LockedDevices>();

	private EvidenceLockFailedRemoveResponse(EvidenceLockFailedRemoveEntityType entityType, string entityName, IEnumerable<LockedDevices> devices)
	{
		EntityType = entityType;
		EntityName = entityName;
		foreach (LockedDevices device in devices)
		{
			FaultDevices.Add(device);
		}
	}

	public override string ToString()
	{
		return GetType().Name;
	}

	public override void Accept(ICommonResponseVisitor visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		visitor.Visit(this);
	}

	public override async Task AcceptAsync(ICommonResponseVisitorAsync visitor)
	{
		if (visitor == null)
		{
			throw new ArgumentNullException("visitor");
		}
		await visitor.Visit(this);
	}

	public static EvidenceLockFailedRemoveResponse CreateRemoveDevicesFailedResponse(string hardwareName, IEnumerable<LockedDevices> devices)
	{
		if (hardwareName == null)
		{
			throw new ArgumentNullException("hardwareName");
		}
		if (devices == null)
		{
			throw new ArgumentNullException("devices");
		}
		return new EvidenceLockFailedRemoveResponse(EvidenceLockFailedRemoveEntityType.Hardware, hardwareName, devices);
	}

	public static EvidenceLockFailedRemoveResponse CreateRemoveRecordingServerFailedResponse(string recorderName)
	{
		if (recorderName == null)
		{
			throw new ArgumentNullException("recorderName");
		}
		return new EvidenceLockFailedRemoveResponse(EvidenceLockFailedRemoveEntityType.RecordingServer, recorderName, Enumerable.Empty<LockedDevices>());
	}
}
