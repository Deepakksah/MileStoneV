using System;
using VideoOS.Management.Common;

namespace VideoOS.Common.Integration.WebApi.Models.Server;

public class PtzSessionResponseModel : ResponseModel
{
	[RequiredNonemptyGuid]
	public Guid DeviceId { get; set; }

	public Guid PatrollingProfileId { get; set; }

	public Guid ManualPatrollingProfileId { get; set; }

	public Identity OwnerIdentity { get; set; }

	public bool IsLocked { get; set; }

	public bool IsAnonymous { get; set; }

	public bool IsPaused { get; set; }

	public bool IsActive { get; set; }

	public int Priority { get; set; }

	public bool IsLicensed { get; set; }
}
