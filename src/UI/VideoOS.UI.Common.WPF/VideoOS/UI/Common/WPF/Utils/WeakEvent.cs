using System;
using System.Collections.Generic;
using System.Reflection;

namespace VideoOS.UI.Common.WPF.Utils;

public class WeakEvent<TArgs> where TArgs : EventArgs
{
	private class WeakSubscription
	{
		private readonly WeakReference _targetReference;

		private readonly MethodInfo _method;

		private readonly bool _isStatic;

		public bool IsAlive
		{
			get
			{
				if (!_isStatic)
				{
					return _targetReference.IsAlive;
				}
				return true;
			}
		}

		public WeakSubscription(object target, MethodInfo method)
		{
			_targetReference = new WeakReference(target);
			_method = method;
			_isStatic = target == null;
		}

		public bool Equals(object target, MethodInfo method)
		{
			if (method == _method)
			{
				return target == _targetReference.Target;
			}
			return false;
		}

		public bool Invoke(object[] parameters)
		{
			object target = _targetReference.Target;
			if (target != null)
			{
				_method.Invoke(target, parameters);
				return true;
			}
			return false;
		}
	}

	private readonly object _lock = new object();

	private readonly List<WeakSubscription> _subscriptions = new List<WeakSubscription>();

	public WeakEvent()
	{
		_lock = new object();
		_subscriptions = new List<WeakSubscription>();
	}

	public WeakEvent(Action<EventHandler<TArgs>> registerAction)
		: this()
	{
		registerAction(delegate(object s, TArgs e)
		{
			Fire(s, e);
		});
	}

	public void Add(object target, MethodInfo method)
	{
		lock (_lock)
		{
			_subscriptions.Add(new WeakSubscription(target, method));
		}
	}

	public void Remove(object target, MethodInfo method)
	{
		lock (_lock)
		{
			for (int i = 0; i < _subscriptions.Count; i++)
			{
				if (_subscriptions[i].Equals(target, method))
				{
					_subscriptions.RemoveAt(i);
					break;
				}
			}
		}
	}

	public void Fire(object sender, TArgs eventArgs)
	{
		object[] parameters = new object[2] { sender, eventArgs };
		lock (_lock)
		{
			int num = 0;
			while (num < _subscriptions.Count)
			{
				if (!_subscriptions[num].Invoke(parameters))
				{
					_subscriptions.RemoveAt(num);
				}
				else
				{
					num++;
				}
			}
		}
	}
}
