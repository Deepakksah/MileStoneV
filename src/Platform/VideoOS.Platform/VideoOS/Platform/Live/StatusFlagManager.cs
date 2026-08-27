using System;
using System.Collections.Generic;
using System.Linq;

namespace VideoOS.Platform.Live;

public class StatusFlagManager
{
	private static readonly Dictionary<object, Guid> _firstTimeReferences = new Dictionary<object, Guid>();

	private static readonly Dictionary<object, Guid> _referenceToObjects = new Dictionary<object, Guid>();

	private static readonly Dictionary<Guid, StatusFlags> _statusFlags = new Dictionary<Guid, StatusFlags>();

	private static readonly object _syncObject = new object();

	public static void Register(object referenceObject, Guid objectId)
	{
		lock (_syncObject)
		{
			_firstTimeReferences[referenceObject] = objectId;
			_referenceToObjects[referenceObject] = objectId;
		}
	}

	public static void UnRegister(object referenceObject)
	{
		lock (_syncObject)
		{
			if (_firstTimeReferences.ContainsKey(referenceObject))
			{
				_firstTimeReferences.Remove(referenceObject);
			}
			if (_referenceToObjects.ContainsKey(referenceObject))
			{
				Guid objectId = _referenceToObjects[referenceObject];
				_referenceToObjects.Remove(referenceObject);
				if (!_referenceToObjects.Values.Any((Guid objectIdInUse) => objectId == objectIdInUse) && _statusFlags.ContainsKey(objectId))
				{
					_statusFlags.Remove(objectId);
				}
			}
		}
	}

	public static void StoreFlags(object referenceObject, Guid objectId, StatusFlags newStatusFlags)
	{
		lock (_syncObject)
		{
			_statusFlags[objectId] = newStatusFlags;
		}
	}

	public static StatusFlags GetInitialFlags(object referenceObject, Guid objectId)
	{
		lock (_syncObject)
		{
			if (_firstTimeReferences.ContainsKey(referenceObject))
			{
				_firstTimeReferences.Remove(referenceObject);
				if (_statusFlags.ContainsKey(objectId))
				{
					return _statusFlags[objectId];
				}
			}
		}
		return StatusFlags.None;
	}
}
