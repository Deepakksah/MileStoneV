using System;
using System.Windows;
using System.Windows.Controls;
using VideoOS.Platform.Telemetry;

namespace VideoOS.Platform.Client;

public abstract class LoginUserControl : UserControl
{
	public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register("Progress", typeof(double), typeof(LoginUserControl), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((LoginUserControl)o).ProgressChanged(o, new EventArgs());
	}));

	public static readonly DependencyProperty ProgressMessageProperty = DependencyProperty.Register("ProgressMessage", typeof(string), typeof(LoginUserControl), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((LoginUserControl)o).ProgressMessageChanged(o, new EventArgs());
	}));

	public static readonly DependencyProperty ErrorMessageProperty = DependencyProperty.Register("ErrorMessage", typeof(string), typeof(LoginUserControl), new PropertyMetadata(delegate(DependencyObject o, DependencyPropertyChangedEventArgs args)
	{
		((LoginUserControl)o).ErrorMessageChanged(o, new EventArgs());
	}));

	public double Progress
	{
		get
		{
			return (double)GetValue(ProgressProperty);
		}
		set
		{
			SetValue(ProgressProperty, value);
		}
	}

	public string ProgressMessage
	{
		get
		{
			return (string)GetValue(ProgressMessageProperty);
		}
		set
		{
			SetValue(ProgressMessageProperty, value);
		}
	}

	public string ErrorMessage
	{
		get
		{
			return (string)GetValue(ErrorMessageProperty);
		}
		set
		{
			SetValue(ErrorMessageProperty, value);
		}
	}

	public event EventHandler Completed = delegate
	{
	};

	public event EventHandler ProgressChanged = delegate
	{
	};

	public event EventHandler ProgressMessageChanged = delegate
	{
	};

	public event EventHandler ErrorMessageChanged = delegate
	{
	};

	public void SignalCompleted()
	{
		this.Completed(this, new EventArgs());
		new TrackPageViewData(typeof(LoginUserControl)).Send();
	}

	public abstract void Init();
}
