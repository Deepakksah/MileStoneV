using System;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.Tween;

public class Tween
{
	public delegate void TweenUpdateEventHandler(object sender, TweenEventArgs e);

	private delegate void TweenThreadTickDelegate();

	private TweenThread _tweenThread;

	private DateTime _startTime;

	private Dispatcher _dispatcher;

	private double[] _currentValues;

	public double[] StartValues { get; set; }

	public double[] EndValues { get; set; }

	public TimeSpan Duration { get; set; }

	public int FPS { get; set; }

	public TweenType TweenType { get; set; }

	public bool IsTweening
	{
		get
		{
			if (_tweenThread != null)
			{
				return true;
			}
			return false;
		}
	}

	public double[] CurrentValues => _currentValues;

	public event TweenUpdateEventHandler TweenUpdateEvent;

	public event EventHandler TweenStartEvent;

	public event EventHandler TweenCompleteEvent;

	private void FireTweenUpdateEvent(double[] newValues, double[] oldValues)
	{
		if (this.TweenUpdateEvent != null)
		{
			this.TweenUpdateEvent(this, new TweenEventArgs(newValues, oldValues));
		}
	}

	private void FireTweenStartEvent()
	{
		if (this.TweenStartEvent != null)
		{
			this.TweenStartEvent(this, new EventArgs());
		}
	}

	private void FireTweenCompleteEvent()
	{
		if (this.TweenCompleteEvent != null)
		{
			this.TweenCompleteEvent(this, new EventArgs());
		}
	}

	public Tween(Dispatcher dispatcher)
	{
		_dispatcher = dispatcher;
	}

	public Tween(Dispatcher dispatcher, double[] startValues, double[] endValues, TimeSpan duration, int fps, TweenType tweenType)
	{
		_dispatcher = dispatcher;
		StartValues = startValues;
		EndValues = endValues;
		Duration = duration;
		FPS = fps;
		TweenType = tweenType;
	}

	public Tween(Dispatcher dispatcher, double startValue, double endValue, double currentValue, TimeSpan duration, int fps, TweenType tweenType)
	{
		double val = (endValue - currentValue) / (endValue - startValue);
		val = Math.Min(val, 1.0);
		val = Math.Max(val, 0.0);
		_dispatcher = dispatcher;
		StartValues = new double[1] { currentValue };
		EndValues = new double[1] { endValue };
		Duration = (double.IsNaN(val) ? TimeSpan.Zero : TimeSpan.FromMilliseconds(duration.TotalMilliseconds * val));
		FPS = fps;
		TweenType = tweenType;
	}

	public void Start()
	{
		if (_tweenThread == null)
		{
			if (Duration == TimeSpan.Zero)
			{
				FireTweenStartEvent();
				FireTweenUpdateEvent(StartValues, EndValues);
				FireTweenCompleteEvent();
			}
			else
			{
				_currentValues = StartValues;
				_startTime = DateTime.Now;
				_tweenThread = new TweenThread(1000 / FPS, new TweenThreadTickDelegate(TweenThreadTick), _dispatcher);
				FireTweenStartEvent();
			}
		}
	}

	public void Stop()
	{
		if (_tweenThread != null)
		{
			_tweenThread.Stop();
			_tweenThread = null;
		}
	}

	private void TweenThreadTick()
	{
		TimeSpan timeSpan = DateTime.Now.Subtract(_startTime);
		double[] array;
		if (timeSpan > Duration)
		{
			array = EndValues;
			if (_tweenThread != null)
			{
				_tweenThread.Stop();
				_tweenThread = null;
			}
		}
		else
		{
			array = new double[StartValues.Length];
			for (int i = 0; i < array.Length; i++)
			{
				array[i] = CalculateValue(TweenType, timeSpan.TotalMilliseconds, Duration.TotalMilliseconds, StartValues[i], EndValues[i] - StartValues[i]);
			}
		}
		double[] currentValues = _currentValues;
		_currentValues = array;
		FireTweenUpdateEvent(_currentValues, currentValues);
		if (_tweenThread == null)
		{
			FireTweenCompleteEvent();
		}
	}

	private static double CalculateValue(TweenType tweenType, double elapsedTime, double totalTime, double startValue, double valueSpan)
	{
		switch (tweenType)
		{
		default:
			return valueSpan * elapsedTime / totalTime + startValue;
		case TweenType.EaseInQuad:
			return valueSpan * (elapsedTime /= totalTime) * elapsedTime + startValue;
		case TweenType.EaseOutQuad:
			return (0.0 - valueSpan) * (elapsedTime /= totalTime) * (elapsedTime - 2.0) + startValue;
		case TweenType.EaseInOutQuad:
			if ((elapsedTime /= totalTime / 2.0) < 1.0)
			{
				return valueSpan / 2.0 * elapsedTime * elapsedTime + startValue;
			}
			return (0.0 - valueSpan) / 2.0 * ((elapsedTime -= 1.0) * (elapsedTime - 2.0) - 1.0) + startValue;
		case TweenType.EaseInCubic:
			return valueSpan * (elapsedTime /= totalTime) * elapsedTime * elapsedTime + startValue;
		case TweenType.EaseOutCubic:
			return valueSpan * ((elapsedTime = elapsedTime / totalTime - 1.0) * elapsedTime * elapsedTime + 1.0) + startValue;
		case TweenType.EaseInOutCubic:
			if ((elapsedTime /= totalTime / 2.0) < 1.0)
			{
				return valueSpan / 2.0 * elapsedTime * elapsedTime * elapsedTime + startValue;
			}
			return valueSpan / 2.0 * ((elapsedTime -= 2.0) * elapsedTime * elapsedTime + 2.0) + startValue;
		case TweenType.EaseInQuart:
			return valueSpan * (elapsedTime /= totalTime) * elapsedTime * elapsedTime * elapsedTime + startValue;
		case TweenType.EaseInExpo:
			if (elapsedTime == 0.0)
			{
				return startValue;
			}
			return valueSpan * Math.Pow(2.0, 10.0 * (elapsedTime / totalTime - 1.0)) + startValue;
		case TweenType.EaseOutExpo:
			if (elapsedTime == totalTime)
			{
				return startValue + valueSpan;
			}
			return valueSpan * (0.0 - Math.Pow(2.0, -10.0 * elapsedTime / totalTime) + 1.0) + startValue;
		}
	}
}
