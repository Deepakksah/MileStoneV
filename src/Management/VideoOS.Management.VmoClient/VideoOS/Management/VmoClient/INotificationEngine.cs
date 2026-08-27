using System;

namespace VideoOS.Management.VmoClient;

public interface INotificationEngine
{
	void Register(VmoObject vmoObject);

	void Unregister(VmoObject vmoObject);

	void UpdateRegistrations(VmoObject vmoObject, bool updateRelatedObjects);

	VmoRelation[] GetRelations(Type type, string id);
}
