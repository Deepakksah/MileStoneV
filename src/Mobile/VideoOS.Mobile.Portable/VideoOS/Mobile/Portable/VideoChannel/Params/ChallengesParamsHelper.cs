namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public class ChallengesParamsHelper
{
	public enum ParamMask : uint
	{
		Nothing = 0u,
		NumberChallenges = 1u,
		Challenge = 2u,
		ChalAnswer = 4u,
		ChalAndHash = 6u,
		All = 7u
	}
}
