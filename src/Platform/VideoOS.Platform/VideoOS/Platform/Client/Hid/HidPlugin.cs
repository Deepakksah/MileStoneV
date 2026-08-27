using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Client.Hid;

public abstract class HidPlugin
{
	private readonly object _instanceDiscoveredLock = new object();

	private readonly List<EventHandler<InstanceDiscoveryEventArgs>> _instanceDiscoveredSubscribers = new List<EventHandler<InstanceDiscoveryEventArgs>>();

	private readonly List<Guid> _instancesDiscoveredBeforeSubscription = new List<Guid>();

	public abstract Guid Id { get; }

	public abstract string Name { get; }

	public virtual bool IsManualConnectSupported => false;

	public event EventHandler<InstanceDiscoveryEventArgs> InstanceDiscoveredEvent
	{
		add
		{
			lock (_instanceDiscoveredLock)
			{
				_instanceDiscoveredSubscribers.Add(value);
				NotifyEarlyDiscoveredInstances();
			}
		}
		remove
		{
			lock (_instanceDiscoveredLock)
			{
				_instanceDiscoveredSubscribers.Remove(value);
			}
		}
	}

	public event EventHandler<InstanceDiscoveryEventArgs> InstanceUndiscoveredEvent;

	public abstract HidInstance GenerateHidInstance(Guid id);

	public virtual void Init()
	{
	}

	public virtual void Close()
	{
	}

	public virtual IEnumerable<HidConfigurationProperty> GetManualConnectProperties()
	{
		return Enumerable.Empty<HidConfigurationProperty>();
	}

	protected void InstanceDiscovered(Guid instanceId)
	{
		lock (_instanceDiscoveredLock)
		{
			if (_instanceDiscoveredSubscribers.Any())
			{
				foreach (EventHandler<InstanceDiscoveryEventArgs> instanceDiscoveredSubscriber in _instanceDiscoveredSubscribers)
				{
					instanceDiscoveredSubscriber(this, new InstanceDiscoveryEventArgs(instanceId));
				}
				return;
			}
			_instancesDiscoveredBeforeSubscription.Add(instanceId);
		}
	}

	protected void InstanceUndiscovered(Guid instanceId)
	{
		lock (_instanceDiscoveredLock)
		{
			if (!_instanceDiscoveredSubscribers.Any())
			{
				_instancesDiscoveredBeforeSubscription.Remove(instanceId);
			}
		}
		this.InstanceUndiscoveredEvent?.Invoke(this, new InstanceDiscoveryEventArgs(instanceId));
	}

	private void NotifyEarlyDiscoveredInstances()
	{
		lock (_instanceDiscoveredLock)
		{
			foreach (EventHandler<InstanceDiscoveryEventArgs> subscriber in _instanceDiscoveredSubscribers)
			{
				_instancesDiscoveredBeforeSubscription.ForEach(delegate(Guid i)
				{
					subscriber(this, new InstanceDiscoveryEventArgs(i));
				});
			}
			_instancesDiscoveredBeforeSubscription.Clear();
		}
	}
}
