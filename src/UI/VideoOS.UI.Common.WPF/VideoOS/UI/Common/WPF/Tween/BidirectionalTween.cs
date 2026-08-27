using System;
using System.Windows.Threading;

namespace VideoOS.UI.Common.WPF.Tween;

public class BidirectionalTween
{
	private Tween _forwardTween;

	private Tween _backwardTween;

	public const short DefaultFps = 60;

	public Dispatcher Dispatcher { get; set; }

	public Func<TimeSpan> Duration { get; set; }

	public int Fps { get; set; }

	public TweenType TweenType { get; set; }

	public double StartValue { get; set; }

	public double EndValue { get; set; }

	public Func<double> StartValueFunc { get; set; }

	public Func<double> EndValueFunc { get; set; }

	public Func<double> CurrentValueFunc { get; set; }

	public Action<double> ApplyValueAction { get; set; }

	public bool IsActive
	{
		get
		{
			if (_backwardTween == null)
			{
				return _forwardTween != null;
			}
			return true;
		}
	}

	public event EventHandler<SingleTweenEventArgs> TweenUpdated;

	public event EventHandler TweenCompleted;

	public BidirectionalTween(Dispatcher dispatcher, double startValue, double endValue, TimeSpan duration, int fps, TweenType tweenType)
	{
		Dispatcher = dispatcher;
		StartValue = startValue;
		EndValue = endValue;
		Duration = () => duration;
		Fps = fps;
		TweenType = tweenType;
	}

	public BidirectionalTween()
		: this(Dispatcher.CurrentDispatcher, 0.0, 0.0, TimeSpan.Zero, 60, TweenType.Linear)
	{
	}

	public void Stop()
	{
		StopForwardTween();
		StopBackwardTween();
	}

	public void StartForward(double currentValue)
	{
		if (_forwardTween == null)
		{
			StopBackwardTween();
			CalculateStartAndEndValue(out var startValue, out var endValue);
			_forwardTween = new Tween(Dispatcher, startValue, endValue, currentValue, Duration(), Fps, TweenType);
			_forwardTween.TweenUpdateEvent += tween_TweenUpdateEvent;
			_forwardTween.TweenCompleteEvent += _forwardTween_TweenCompleteEvent;
			_forwardTween.Start();
		}
	}

	public void StartForward()
	{
		StartForward(CurrentValueFunc());
	}

	public void StartBackward(double currentValue)
	{
		if (_backwardTween == null)
		{
			StopForwardTween();
			CalculateStartAndEndValue(out var startValue, out var endValue);
			_backwardTween = new Tween(Dispatcher, endValue, startValue, currentValue, Duration(), Fps, TweenType);
			_backwardTween.TweenUpdateEvent += tween_TweenUpdateEvent;
			_backwardTween.TweenCompleteEvent += _backwardTween_TweenCompleteEvent;
			_backwardTween.Start();
		}
	}

	public void StartBackward()
	{
		StartBackward(CurrentValueFunc());
	}

	private void CalculateStartAndEndValue(out double startValue, out double endValue)
	{
		if (StartValueFunc != null)
		{
			startValue = StartValueFunc();
		}
		else
		{
			startValue = StartValue;
		}
		if (EndValueFunc != null)
		{
			endValue = EndValueFunc();
		}
		else
		{
			endValue = EndValue;
		}
	}

	private void _forwardTween_TweenCompleteEvent(object sender, EventArgs e)
	{
		StopForwardTween();
		OnTweenCompleted();
	}

	private void _backwardTween_TweenCompleteEvent(object sender, EventArgs e)
	{
		StopBackwardTween();
		OnTweenCompleted();
	}

	private void StopBackwardTween()
	{
		if (_backwardTween != null)
		{
			_backwardTween.TweenUpdateEvent -= tween_TweenUpdateEvent;
			_backwardTween.TweenCompleteEvent -= _backwardTween_TweenCompleteEvent;
			_backwardTween = null;
		}
	}

	private void StopForwardTween()
	{
		if (_forwardTween != null)
		{
			_forwardTween.TweenUpdateEvent -= tween_TweenUpdateEvent;
			_forwardTween.TweenCompleteEvent -= _forwardTween_TweenCompleteEvent;
			_forwardTween = null;
		}
	}

	private void tween_TweenUpdateEvent(object sender, TweenEventArgs e)
	{
		if (ApplyValueAction != null)
		{
			ApplyValueAction(e.NewValues[0]);
		}
		OnTweenUpdated(e.NewValues[0], e.OldValues[0]);
	}

	protected void OnTweenUpdated(double newValue, double oldValue)
	{
		this.TweenUpdated?.Invoke(this, new SingleTweenEventArgs(newValue, oldValue));
	}

	protected void OnTweenCompleted()
	{
		this.TweenCompleted?.Invoke(this, EventArgs.Empty);
	}
}
