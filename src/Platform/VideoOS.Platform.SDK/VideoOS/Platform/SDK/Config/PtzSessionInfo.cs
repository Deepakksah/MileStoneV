using System;
using Newtonsoft.Json;
using VideoOS.Platform.SDK.Proxy.RecorderServices.Net;

namespace VideoOS.Platform.SDK.Config;

public class PtzSessionInfo
{
	[JsonProperty("deviceId")]
	public Guid DeviceId { get; set; }

	[JsonProperty("patrollingProfileId")]
	public Guid PatrollingProfileId { get; set; }

	[JsonProperty("manualPatrollingProfileId")]
	public Guid ManualPatrollingProfileId { get; set; }

	[JsonProperty("userInfo")]
	public UserInfo UserInfo { get; set; }

	[JsonProperty("priority")]
	public int Priority { get; set; }

	[JsonProperty("reserved")]
	public bool Reserved { get; set; }

	[JsonProperty("isPatrolling")]
	public bool IsPatrolling { get; set; }

	[JsonProperty("isAnonymous")]
	public bool IsAnonymous { get; set; }

	[JsonProperty("isPaused")]
	public bool IsPaused { get; set; }

	[JsonProperty("timeoutSec")]
	public int TimeoutSec { get; set; }

	[JsonProperty("timeoutLeftSec")]
	public int TimeoutLeftSec { get; set; }

	[JsonProperty("isLicensed")]
	public bool IsLicensed { get; set; }

	public PtzSessionInfo(VideoOS.Platform.SDK.Proxy.RecorderServices.Net.PtzSessionInfo sessionInfo)
	{
		DeviceId = sessionInfo.DeviceId;
		PatrollingProfileId = sessionInfo.PatrollingProfileId;
		ManualPatrollingProfileId = sessionInfo.ManualPatrollingProfileId;
		UserInfo = new UserInfo(sessionInfo.User);
		Priority = sessionInfo.Priority;
		Reserved = sessionInfo.Reserved;
		IsPatrolling = sessionInfo.IsPatrolling;
		IsAnonymous = sessionInfo.IsAnonymous;
		IsPaused = sessionInfo.IsPaused;
		TimeoutSec = sessionInfo.TimeoutSec;
		TimeoutLeftSec = sessionInfo.TimeoutLeftSec;
		IsLicensed = sessionInfo.IsLicensed;
	}
}
