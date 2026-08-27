using System;
using System.Linq;

namespace VideoOS.Management.Server.ConfigurationApi.Rules.Backend.Domain;

public static class ActionTypeHelper
{
	public static ActionType GetActionType(string actionTypeString, out Guid mipActionId)
	{
		mipActionId = Guid.Empty;
		string[] array = actionTypeString.Split(':');
		if (array.Any() && Enum.TryParse<ActionType>(array[0], out var result))
		{
			if ((result == ActionType.MIPAction || result == ActionType.MIPAction2) && (array.Length != 2 || !Guid.TryParse(array[1], out mipActionId)))
			{
				return ActionType.None;
			}
			return result;
		}
		return ActionType.None;
	}
}
