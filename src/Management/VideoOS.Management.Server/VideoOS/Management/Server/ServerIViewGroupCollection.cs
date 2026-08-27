using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewGroupCollection : IViewGroups, IEnumerable<IViewGroup>, IEnumerable
{
	private IGeminiRequestContext _requestContext;

	private bool _readAccessVerified;

	private string _parentId;

	private Dictionary<string, IViewGroup> _viewGroups;

	public IViewGroup this[Guid id]
	{
		get
		{
			SecurePopulate();
			if (_viewGroups.TryGetValue(id.ToString(), out var value))
			{
				return value;
			}
			return null;
		}
	}

	public IViewGroup this[string index]
	{
		get
		{
			SecurePopulate();
			foreach (IViewGroup value in _viewGroups.Values)
			{
				if (value.Name == index)
				{
					return value;
				}
			}
			return null;
		}
	}

	public ServerIViewGroupCollection(IGeminiRequestContext requestContext, string parentId)
		: this(requestContext, parentId, readAccessVerified: false)
	{
	}

	public ServerIViewGroupCollection(IGeminiRequestContext requestContext, string parentId, bool readAccessVerified)
	{
		if (requestContext == null)
		{
			throw new ArgumentException("requestContext cannot be null", "requestContext");
		}
		_requestContext = requestContext;
		_parentId = parentId;
		_readAccessVerified = readAccessVerified;
	}

	public bool Contains(string key)
	{
		SecurePopulate();
		return this[key] != null;
	}

	public IEnumerator<IViewGroup> GetEnumerator()
	{
		SecurePopulate();
		return _viewGroups.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void SecurePopulate()
	{
		if (_viewGroups == null)
		{
			_viewGroups = new Dictionary<string, IViewGroup>(StringComparer.OrdinalIgnoreCase);
			ServerGroup[] array = ((!_readAccessVerified) ? ServerGroup.QueryServerGroupsByParentGroupId(_requestContext, ServerGroup.EnumStructureType.ViewGroup, _parentId) : ServerGroup.QueryServerGroupsByParentGroupIdNoSecurity(_requestContext, ServerGroup.EnumStructureType.ViewGroup, _parentId));
			ServerGroup[] array2 = array;
			foreach (ServerGroup serverGroup in array2)
			{
				_viewGroups.Add(serverGroup.Id, new ServerIViewGroup(_requestContext, serverGroup, _readAccessVerified));
			}
		}
	}
}
