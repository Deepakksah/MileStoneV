using System;

namespace VideoOS.Central.Util;

public class NthFailDetector
{
	private readonly uint _cycleCnt;

	private uint _failsCount;

	public NthFailDetector(uint cycleCnt = uint.MaxValue)
	{
		_cycleCnt = cycleCnt;
	}

	public void OnSuccess(Action<uint> onPrevFails)
	{
		if (_failsCount != 0)
		{
			onPrevFails?.Invoke(_failsCount);
		}
		_failsCount = 0u;
	}

	public void OnFail(Action<uint> onNthFail)
	{
		if (_failsCount % _cycleCnt == 0)
		{
			onNthFail(_failsCount + 1);
		}
		_failsCount++;
	}
}
