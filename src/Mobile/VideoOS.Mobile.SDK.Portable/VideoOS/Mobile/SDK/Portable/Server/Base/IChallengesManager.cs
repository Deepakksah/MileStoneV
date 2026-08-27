using System.Collections.Generic;

namespace VideoOS.Mobile.SDK.Portable.Server.Base;

public interface IChallengesManager
{
	GetChallenges GetChallenges { get; set; }

	ChallengePair GetNextChallenge();

	void AddChallenges(IEnumerable<string> challenges);
}
