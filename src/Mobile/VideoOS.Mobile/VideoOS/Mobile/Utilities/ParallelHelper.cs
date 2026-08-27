using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VideoOS.Mobile.Portable.Utilities;

namespace VideoOS.Mobile.Utilities;

public static class ParallelHelper
{
	public static void InvokeCatched(params Action[] actions)
	{
		Parallel.Invoke(((IEnumerable<Action>)actions).Select((Func<Action, Action>)((Action action) => delegate
		{
			ExceptionHelper.CallCatched(action);
		})).ToArray());
	}

	public static void BatchInvokeCatched(int batchCount, params Action[] actions)
	{
		foreach (Action[] item in actions.Partition(batchCount))
		{
			InvokeCatched(item);
		}
	}
}
