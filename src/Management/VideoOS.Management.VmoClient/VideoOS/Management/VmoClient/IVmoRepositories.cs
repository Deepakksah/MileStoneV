using System;

namespace VideoOS.Management.VmoClient;

public interface IVmoRepositories
{
	INotificationEngine Notification { get; }

	IServer ServerCommunication { get; }

	object RepositoryLocker { get; }

	ServerVersion ServerVersion { get; }

	string ConnectedUser { get; }

	GenericRepository<TObject> GetGenericRepository<TObject>() where TObject : VmoObject;

	GenericRepositoryBase GetGenericRepositoryBase(Type type);

	bool CheckVersion(ServerVersion requiredVersion);

	TObject GetObjectById<TObject>(int id, bool refreshCachedObjects) where TObject : VmoObjectIntBased;

	TObject GetObjectById<TObject>(Guid id, bool refreshCachedObjects) where TObject : VmoObjectGuidBased;

	TObject[] GetObjectsByIds<TObject>(int[] id, bool refreshCachedObjects) where TObject : VmoObjectIntBased;

	TObject[] GetObjectsByIds<TObject>(Guid[] id, bool refreshCachedObjects) where TObject : VmoObjectGuidBased;

	TObject[] GetObjectsByParentId<TParent, TObject>(int parentId, bool refreshCachedObjects) where TParent : VmoObjectIntBased where TObject : VmoObject;

	TObject[] GetObjectsByParentId<TParent, TObject>(Guid parentId, bool refreshCachedObjects) where TParent : VmoObjectGuidBased where TObject : VmoObject;

	TObject GetNonVmoObjectById<TObject>(Guid Id) where TObject : NonCachedVmoObject;

	TObject GetNonVmoObjectById<TObject>(int Id) where TObject : NonCachedVmoObject;

	TObject[] GetNonVmoObjectsByParentId<TParent, TObject>(Guid parentId) where TParent : VmoObject where TObject : NonCachedVmoObject;

	TObject[] GetNonVmoObjectsByParentId<TParent, TObject>(int parentId) where TParent : VmoObject where TObject : NonCachedVmoObject;
}
