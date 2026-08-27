using System.Collections.Generic;
using MediatR;

namespace VideoOS.DataCollector.Service;

public interface IEntity
{
	IReadOnlyCollection<INotification>? DomainEvents { get; }

	void ClearDomainEvents();
}
