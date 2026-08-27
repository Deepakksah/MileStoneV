using System;
using System.Runtime.Serialization;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration;

[Serializable]
[DataContract]
public sealed class PtzSessionInfo
{
	[DataMember]
	public Guid DeviceId { get; set; }

	[DataMember]
	public Guid PatrollingProfileId { get; set; }

	[DataMember]
	public Guid ManualPatrollingProfileId { get; set; }

	[DataMember]
	public Identity OwnerIdentity { get; set; }

	[DataMember]
	public bool IsLocked { get; set; }

	[DataMember]
	public bool IsAnonymous { get; set; }

	[DataMember]
	public bool IsPaused { get; set; }

	[DataMember]
	public bool IsActive { get; set; }

	[DataMember]
	public int Priority { get; set; }

	[DataMember]
	public bool IsLicensed { get; set; }

	public PtzSessionInfo(Guid deviceId, Guid patrollingProfileId, Guid manualPatrollingProfileId, Identity ownerIdentity, bool isLocked, bool isAnonymous, bool isPaused, bool isActive, int priority, bool isLicensed)
	{
		DeviceId = deviceId;
		PatrollingProfileId = patrollingProfileId;
		ManualPatrollingProfileId = manualPatrollingProfileId;
		OwnerIdentity = ownerIdentity;
		IsLocked = isLocked;
		IsAnonymous = isAnonymous;
		IsPaused = isPaused;
		IsActive = isActive;
		Priority = priority;
		IsLicensed = isLicensed;
	}
}
