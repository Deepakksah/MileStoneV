using System;
using System.Collections.Generic;
using VideoOS.Mobile.Portable.Log;
using VideoOS.Mobile.Portable.MetaChannel;

namespace VideoOS.Mobile.Portable.VideoChannel.Params;

public static class ChallengesHelper
{
	public static ErrorCodes PutChallengesInCommand(Command command, IEnumerable<Guid> challenges)
	{
		ErrorCodes result = ErrorCodes.Ok;
		try
		{
			if (command != null && challenges != null)
			{
				foreach (Guid challenge in challenges)
				{
					command.OutputParams.Add(new Parameter("Challenge", challenge.ToString().ToUpper()));
				}
			}
		}
		catch (Exception ex)
		{
			Logger.Instance.Log(LogContent.Error, "ChallengesHelper.ReturnChallenges", ex.Message);
			result = ErrorCodes.Unknown;
		}
		return result;
	}
}
