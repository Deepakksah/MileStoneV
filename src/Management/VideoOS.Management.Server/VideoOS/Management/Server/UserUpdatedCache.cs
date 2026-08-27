using System;
using System.Collections.Generic;
using System.Threading;

namespace VideoOS.Management.Server;

public class UserUpdatedCache
{
	public static DateTime LastModifiedMinValue = new DateTime(1970, 1, 1, 10, 0, 0, DateTimeKind.Utc);

	private Dictionary<Guid, UserUpdateInfo> _users = new Dictionary<Guid, UserUpdateInfo>();

	private ReaderWriterLockSlim _usersLock = new ReaderWriterLockSlim();

	public static UserUpdatedCache Instance { get; } = new UserUpdatedCache();

	public DateTime FindUserLastModified(Guid userId)
	{
		UserUpdateInfo value = null;
		_usersLock.EnterReadLock();
		try
		{
			_users.TryGetValue(userId, out value);
		}
		finally
		{
			_usersLock.ExitReadLock();
		}
		if (value != null)
		{
			lock (value)
			{
				return value.LastModified;
			}
		}
		return LastModifiedMinValue;
	}

	public UserUpdateInfo FindOrCreateUserInfo(Guid userId)
	{
		_usersLock.EnterUpgradeableReadLock();
		try
		{
			if (!_users.TryGetValue(userId, out var value))
			{
				_usersLock.EnterWriteLock();
				try
				{
					value = new UserUpdateInfo(userId, LastModifiedMinValue);
					_users.Add(userId, value);
				}
				finally
				{
					_usersLock.ExitWriteLock();
				}
			}
			return value;
		}
		finally
		{
			_usersLock.ExitUpgradeableReadLock();
		}
	}

	public void Initialize(IEnumerable<UserUpdateInfo> userUpdateInfos)
	{
		_usersLock.EnterWriteLock();
		try
		{
			_users.Clear();
			foreach (UserUpdateInfo userUpdateInfo in userUpdateInfos)
			{
				_users.Add(userUpdateInfo.Id, userUpdateInfo);
			}
		}
		finally
		{
			_usersLock.ExitWriteLock();
		}
	}
}
