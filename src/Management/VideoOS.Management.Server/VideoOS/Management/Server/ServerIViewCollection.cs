using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewCollection : IViews, IEnumerable<IView>, IEnumerable
{
	private IGeminiRequestContext _requestContext;

	private string _parentViewGroup;

	private bool _readAccessVerified;

	private Dictionary<string, IView> _views;

	public IView this[Guid id]
	{
		get
		{
			SecurePopulate();
			if (_views.TryGetValue(id.ToString(), out var value))
			{
				return value;
			}
			return null;
		}
	}

	public IView this[string viewName]
	{
		get
		{
			SecurePopulate();
			foreach (IView value in _views.Values)
			{
				if (value.Name == viewName)
				{
					return value;
				}
			}
			return null;
		}
	}

	public ServerIViewCollection(IGeminiRequestContext requestContext, string parentViewGroup)
		: this(requestContext, parentViewGroup, readAccessVerified: false, null)
	{
	}

	public ServerIViewCollection(IGeminiRequestContext requestContext, string parentViewGroup, bool readAccessVerified)
		: this(requestContext, parentViewGroup, readAccessVerified, null)
	{
	}

	public ServerIViewCollection(IGeminiRequestContext requestContext, string parentViewGroup, bool readAccessVerified, View[] views)
	{
		if (requestContext == null)
		{
			throw new ArgumentException("requestContext cannot be null", "requestContext");
		}
		_requestContext = requestContext;
		_parentViewGroup = parentViewGroup;
		_readAccessVerified = readAccessVerified;
		if (views != null)
		{
			_views = new Dictionary<string, IView>(StringComparer.OrdinalIgnoreCase);
			ViewOld[] array = ViewOld.ConvertToViewOlds(views);
			foreach (ViewOld viewOld in array)
			{
				_views.Add(viewOld.Id, new ServerIView(_requestContext, _parentViewGroup, viewOld, _readAccessVerified));
			}
		}
	}

	public bool Contains(string viewName)
	{
		SecurePopulate();
		return this[viewName] != null;
	}

	public IEnumerator<IView> GetEnumerator()
	{
		SecurePopulate();
		return _views.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void SecurePopulate()
	{
		if (_views == null)
		{
			_views = new Dictionary<string, IView>(StringComparer.OrdinalIgnoreCase);
			View[] views;
			if (_readAccessVerified)
			{
				views = View.QueryViewsNoSecurity(_requestContext, _parentViewGroup);
			}
			else
			{
				string[] viewIds = View.QueryViewIds(_requestContext, _parentViewGroup);
				views = View.QueryViewsByIds(_requestContext, viewIds);
			}
			ViewOld[] array = ViewOld.ConvertToViewOlds(views);
			foreach (ViewOld viewOld in array)
			{
				_views.Add(viewOld.Id, new ServerIView(_requestContext, _parentViewGroup, viewOld, _readAccessVerified));
			}
		}
	}
}
