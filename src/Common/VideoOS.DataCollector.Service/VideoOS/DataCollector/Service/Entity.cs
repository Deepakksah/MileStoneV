using System.Collections.Generic;
using MediatR;

namespace VideoOS.DataCollector.Service;

public abstract class Entity : IEntity
{
	private List<INotification>? _domainEvents;

	public IReadOnlyCollection<INotification>? DomainEvents => _domainEvents?.AsReadOnly();

	public void AddDomainEvent(INotification eventItem)
	{
		_domainEvents = _domainEvents ?? new List<INotification>();
		_domainEvents.Add(eventItem);
	}

	public void RemoveDomainEvent(INotification eventItem)
	{
		_domainEvents?.Remove(eventItem);
	}

	public void ClearDomainEvents()
	{
		_domainEvents?.Clear();
	}
}
