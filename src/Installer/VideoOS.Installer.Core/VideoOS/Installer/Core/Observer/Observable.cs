using System;
using System.Collections.Generic;

namespace VideoOS.Installer.Core.Observer;

public class Observable
{
	private List<Observer> _observers;

	public Observable()
	{
		_observers = new List<Observer>();
	}

	public virtual void AttachObserver(Observer observer)
	{
		lock (_observers)
		{
			_observers.Add(observer);
		}
	}

	public virtual void DetachObserver(Observer observer)
	{
		lock (_observers)
		{
			_observers.Remove(observer);
		}
	}

	public virtual void NotifyObservers()
	{
		lock (_observers)
		{
			foreach (Observer observer in _observers)
			{
				try
				{
					observer.Update(this);
				}
				catch (Exception)
				{
				}
			}
		}
	}
}
