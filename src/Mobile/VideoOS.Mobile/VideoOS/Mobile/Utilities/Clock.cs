using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoOS.Mobile.Utilities;

public class Clock : IClock
{
	private DateTime _startTime;

	private DateTime _currentTime;

	private ClockDirection _clockDirection;

	private int _ticksPerSecond;

	private bool _isRunning;

	private bool _jumpNeeded;

	private Task _tickTask;

	private CancellationTokenSource tokenSource;

	public bool ClockIsTicking => _isRunning;

	public DateTime StartTime
	{
		get
		{
			return _startTime;
		}
		set
		{
			_startTime = value;
		}
	}

	public ClockDirection ClockDirection
	{
		get
		{
			return _clockDirection;
		}
		set
		{
			_clockDirection = value;
		}
	}

	public int TicksPerSecond
	{
		get
		{
			return _ticksPerSecond;
		}
		set
		{
			_ticksPerSecond = value;
		}
	}

	public event Tick Tick;

	public Clock()
	{
	}

	public Clock(DateTime startTime, ClockDirection direction, int ticksPerSecond)
	{
		_startTime = startTime.ToUniversalTime();
		_clockDirection = direction;
		_ticksPerSecond = ticksPerSecond;
		tokenSource = new CancellationTokenSource();
		_tickTask = new Task(Run, tokenSource.Token, TaskCreationOptions.LongRunning);
	}

	public void RestartFromTime(DateTime time, ClockDirection direction, int ticksPerSecond)
	{
		_startTime = time.ToUniversalTime();
		_clockDirection = direction;
		Stop();
		Start();
	}

	public void JumpToTime(DateTime time)
	{
		_startTime = time.ToUniversalTime();
		_jumpNeeded = true;
	}

	public void Start()
	{
		if (!_isRunning)
		{
			_isRunning = true;
			_tickTask?.Start();
		}
	}

	public void Stop()
	{
		_isRunning = false;
	}

	private void Run()
	{
		_currentTime = _startTime;
		DateTime utcNow = DateTime.UtcNow;
		TimeSpan timeSpan = default(TimeSpan);
		while (_isRunning)
		{
			int num = 1000 / _ticksPerSecond;
			if (_jumpNeeded)
			{
				_currentTime = _startTime;
				_jumpNeeded = false;
			}
			timeSpan = timeSpan.Add(TimeSpan.FromMilliseconds(num));
			while (DateTime.UtcNow - utcNow <= timeSpan)
			{
				Thread.Sleep(10);
			}
			_currentTime = ((_clockDirection == ClockDirection.Forward) ? _currentTime.Add(TimeSpan.FromMilliseconds(num)) : _currentTime.Add(-TimeSpan.FromMilliseconds(num)));
			if (_isRunning)
			{
				this.Tick?.Invoke(_currentTime);
			}
		}
	}
}
