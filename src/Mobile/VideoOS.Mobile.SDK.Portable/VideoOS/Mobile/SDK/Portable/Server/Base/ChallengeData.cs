using System;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public class ChallengeData
{
	public string Challenge { get; set; }

	public DateTime CreatedUtc { get; set; }

	public ChallengeData()
	{
		CreatedUtc = DateTime.UtcNow;
	}
}
