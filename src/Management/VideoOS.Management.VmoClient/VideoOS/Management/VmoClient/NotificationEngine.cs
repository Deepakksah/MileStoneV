using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Management.VmoClient;

public class NotificationEngine : INotificationEngine
{
	private class NotificationEntry
	{
		public Type Type { get; private set; }

		public string Id { get; private set; }

		public VmoObject VmoObject { get; private set; }

		public bool IsWeakReference { get; private set; }

		public NotificationEntry(Type type, string id, VmoObject vmoObject, bool isWeakReference)
		{
			Type = type;
			Id = id;
			VmoObject = vmoObject;
			IsWeakReference = isWeakReference;
		}

		public override string ToString()
		{
			return Type.Name + ": " + Id + ", " + (IsWeakReference ? "weak" : "strong");
		}
	}

	private List<NotificationEntry> _notificationCache = new List<NotificationEntry>();

	private IVmoRepositories repositories;

	public NotificationEngine(IVmoRepositories repositories)
	{
		this.repositories = repositories;
	}

	void INotificationEngine.Register(VmoObject vmoObject)
	{
		foreach (VmoRelation relation in vmoObject.Relations)
		{
			NotificationEntry item = new NotificationEntry(relation.Type, relation.Id, vmoObject, relation.IsWeakRelation);
			_notificationCache.Add(item);
			repositories.GetGenericRepositoryBase(relation.Type)?.GetVmoObjectFromCache(relation.Id)?.AddNotification(vmoObject);
		}
		repositories.GetGenericRepositoryBase(typeof(ManagementServer))?.GetVmoObjectFromCache("909749AE-88C4-4780-A381-383D691E98FA")?.AddNotification(vmoObject);
	}

	void INotificationEngine.Unregister(VmoObject vmoObject)
	{
		List<VmoObject> list = new List<VmoObject>();
		foreach (NotificationEntry item in _notificationCache)
		{
			if (item.Type == vmoObject.GetType() && string.Equals(item.Id, vmoObject.InternalId, StringComparison.OrdinalIgnoreCase) && !item.IsWeakReference)
			{
				list.Add(item.VmoObject);
			}
		}
		foreach (NotificationEntry item2 in _notificationCache.FindAll((NotificationEntry e) => e.VmoObject == vmoObject))
		{
			_notificationCache.Remove(item2);
		}
		foreach (VmoObject item3 in list)
		{
			item3.OnDelete(new EventArgs());
		}
		vmoObject.OnDelete(new EventArgs());
	}

	void INotificationEngine.UpdateRegistrations(VmoObject vmoObject, bool updateRelatedObjects)
	{
		List<NotificationEntry> previousRelations = _notificationCache.FindAll((NotificationEntry n) => n.VmoObject == vmoObject);
		List<NotificationEntry> newRelations = new List<NotificationEntry>(vmoObject.Relations.Select((VmoRelation r) => new NotificationEntry(r.Type, r.Id, vmoObject, r.IsWeakRelation)));
		List<NotificationEntry> list = new List<NotificationEntry>();
		List<NotificationEntry> list2 = new List<NotificationEntry>();
		list.AddRange(previousRelations.Where((NotificationEntry pr) => !newRelations.Exists((NotificationEntry nr) => pr.Type == nr.Type && string.Equals(pr.Id, nr.Id, StringComparison.OrdinalIgnoreCase))));
		list2.AddRange(newRelations.Where((NotificationEntry nr) => !previousRelations.Exists((NotificationEntry pr) => nr.Type == pr.Type && string.Equals(nr.Id, pr.Id, StringComparison.OrdinalIgnoreCase))));
		if (list.Count() <= 0 && list2.Count() <= 0)
		{
			return;
		}
		foreach (NotificationEntry item in list)
		{
			_notificationCache.Remove(item);
			repositories.GetGenericRepositoryBase(item.Type)?.GetVmoObjectFromCache(item.Id)?.RemoveNotification(vmoObject);
		}
		foreach (NotificationEntry item2 in list2)
		{
			_notificationCache.Add(item2);
			repositories.GetGenericRepositoryBase(item2.Type)?.GetVmoObjectFromCache(item2.Id)?.AddNotification(vmoObject);
		}
		if (!updateRelatedObjects)
		{
			return;
		}
		foreach (VmoObject item3 in (from n in _notificationCache.FindAll((NotificationEntry n) => string.Equals(n.Id, vmoObject.InternalId, StringComparison.OrdinalIgnoreCase))
			select n.VmoObject).ToList())
		{
			item3.Repository.RefreshObject(item3.InternalId, updateRelations: true);
		}
	}

	VmoRelation[] INotificationEngine.GetRelations(Type type, string id)
	{
		NotificationEntry[] array = _notificationCache.Where((NotificationEntry no) => (no.VmoObject.GetType() == type || no.VmoObject.GetType().IsSubclassOf(type)) && no.VmoObject.InternalId == id).ToArray();
		VmoRelationCollection vmoRelationCollection = new VmoRelationCollection();
		NotificationEntry[] array2 = array;
		foreach (NotificationEntry notificationEntry in array2)
		{
			vmoRelationCollection.Add(new VmoRelation(EntityNameAttributeHandlerWrapper.GetEntityName(notificationEntry.Type), notificationEntry.Id));
		}
		return vmoRelationCollection.ToArray();
	}
}
