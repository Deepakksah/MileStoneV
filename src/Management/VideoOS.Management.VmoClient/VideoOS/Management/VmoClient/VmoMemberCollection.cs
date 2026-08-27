using System;

namespace VideoOS.Management.VmoClient;

public class VmoMemberCollection<T> : VmoCollection<T> where T : VmoObject
{
	private IVmoRepositories repositories;

	public VmoMemberCollection(Func<bool, T[]> loadData, VmoObject parent, bool isInitialized)
		: base(loadData, parent, isInitialized)
	{
		repositories = parent.Repositories;
	}

	public void AddMember(T vmoObject)
	{
		repositories.ServerCommunication.AddGroupMembership(base.Parent.InternalId, base.Parent.GetType(), vmoObject.InternalId);
		Add(vmoObject);
	}

	public void RemoveMember(T vmoObject)
	{
		repositories.ServerCommunication.RemoveGroupMembership(base.Parent.InternalId, base.Parent.GetType(), vmoObject.InternalId);
		Remove(vmoObject.InternalId);
	}
}
