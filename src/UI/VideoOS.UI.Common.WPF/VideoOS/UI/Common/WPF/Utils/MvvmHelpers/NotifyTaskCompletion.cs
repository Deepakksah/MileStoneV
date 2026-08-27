using System;
using System.ComponentModel;
using System.Threading.Tasks;

namespace VideoOS.UI.Common.WPF.Utils.MvvmHelpers;

public sealed class NotifyTaskCompletion<TResult> : INotifyPropertyChanged
{
	public TaskFactory<TResult> Factorty { get; private set; }

	public Task<TResult> Task { get; private set; }

	public TResult Result
	{
		get
		{
			if (Task == null)
			{
				return default(TResult);
			}
			if (Task.Status != TaskStatus.RanToCompletion)
			{
				return default(TResult);
			}
			return Task.Result;
		}
	}

	public TaskStatus Status => Task.Status;

	public bool IsCompleted => Task.IsCompleted;

	public bool IsNotCompleted => !Task.IsCompleted;

	public bool IsSuccessfullyCompleted
	{
		get
		{
			if (Task == null)
			{
				return false;
			}
			return Task.Status == TaskStatus.RanToCompletion;
		}
	}

	public bool IsCanceled => Task.IsCanceled;

	public bool IsFaulted => Task.IsFaulted;

	public AggregateException Exception => Task.Exception;

	public Exception InnerException
	{
		get
		{
			if (Exception != null)
			{
				return Exception.InnerException;
			}
			return null;
		}
	}

	public string ErrorMessage
	{
		get
		{
			if (InnerException != null)
			{
				return InnerException.Message;
			}
			return null;
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	public NotifyTaskCompletion()
	{
	}

	public NotifyTaskCompletion(Task<TResult> task)
	{
		Restart(task);
	}

	public void Restart(Task<TResult> task)
	{
		Task = task;
		if (!task.IsCompleted)
		{
			WatchTaskAsync(task);
		}
	}

	private async Task WatchTaskAsync(Task task)
	{
		try
		{
			await task;
		}
		catch
		{
		}
		PropertyChangedEventHandler propertyChangedEventHandler = this.PropertyChanged;
		if (propertyChangedEventHandler != null)
		{
			propertyChangedEventHandler(this, new PropertyChangedEventArgs("Status"));
			propertyChangedEventHandler(this, new PropertyChangedEventArgs("IsCompleted"));
			propertyChangedEventHandler(this, new PropertyChangedEventArgs("IsNotCompleted"));
			if (task.IsCanceled)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("IsCanceled"));
			}
			else if (task.IsFaulted)
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("IsFaulted"));
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("Exception"));
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("InnerException"));
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("ErrorMessage"));
			}
			else
			{
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("IsSuccessfullyCompleted"));
				propertyChangedEventHandler(this, new PropertyChangedEventArgs("Result"));
			}
		}
	}
}
