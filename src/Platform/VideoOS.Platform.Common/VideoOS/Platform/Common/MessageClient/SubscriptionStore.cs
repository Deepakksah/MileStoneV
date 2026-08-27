using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace VideoOS.Platform.Common.MessageClient;

public class SubscriptionStore : IDisposable
{
	private readonly ReaderWriterLockSlim _subscriptionsLock = new ReaderWriterLockSlim();

	private readonly Dictionary<Guid, Subscription> _subscriptions = new Dictionary<Guid, Subscription>();

	private readonly Dictionary<string, HashSet<Guid>> _subscriptionsPerTopic = new Dictionary<string, HashSet<Guid>>();

	public void StoreSubscription(Subscription subscription)
	{
		_subscriptionsLock.EnterWriteLock();
		try
		{
			_subscriptions[subscription.Id] = subscription;
			if (!_subscriptionsPerTopic.ContainsKey(subscription.Topic))
			{
				_subscriptionsPerTopic[subscription.Topic] = new HashSet<Guid>();
			}
			_subscriptionsPerTopic[subscription.Topic].Add(subscription.Id);
		}
		finally
		{
			_subscriptionsLock.ExitWriteLock();
		}
	}

	public bool RemoveSubscription(Guid id)
	{
		_subscriptionsLock.EnterWriteLock();
		try
		{
			Subscription subscription = _subscriptions[id];
			_subscriptions.Remove(id);
			_subscriptionsPerTopic[subscription.Topic].Remove(subscription.Id);
			if (_subscriptionsPerTopic[subscription.Topic].Count == 0)
			{
				_subscriptionsPerTopic.Remove(subscription.Topic);
				return false;
			}
			return true;
		}
		finally
		{
			_subscriptionsLock.ExitWriteLock();
		}
	}

	public Subscription GetSubscription(Guid id)
	{
		_subscriptionsLock.EnterReadLock();
		try
		{
			_subscriptions.TryGetValue(id, out var value);
			return value;
		}
		finally
		{
			_subscriptionsLock.ExitReadLock();
		}
	}

	public List<Subscription> GetSubscriptions(string topic)
	{
		_subscriptionsLock.EnterReadLock();
		try
		{
			HashSet<Guid> value;
			return (!_subscriptionsPerTopic.TryGetValue(topic, out value)) ? new List<Subscription>() : value.Select((Guid id) => _subscriptions[id]).ToList();
		}
		finally
		{
			_subscriptionsLock.ExitReadLock();
		}
	}

	public List<Subscription> GetAllSubscriptions()
	{
		_subscriptionsLock.EnterReadLock();
		try
		{
			return _subscriptions.Values.ToList();
		}
		finally
		{
			_subscriptionsLock.ExitReadLock();
		}
	}

	public void Dispose()
	{
		_subscriptionsLock?.Dispose();
	}
}
