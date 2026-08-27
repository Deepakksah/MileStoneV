using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewItemCollection : IViewItems, IEnumerable<IViewItem>, IEnumerable
{
	private IGeminiRequestContext _requestContext;

	private string _viewId;

	private bool _readAccessVerified;

	private Dictionary<string, IViewItem> _viewItems;

	public IViewItem this[string index]
	{
		get
		{
			SecurePopulate();
			if (_viewItems.TryGetValue(index, out var value))
			{
				return value;
			}
			return null;
		}
	}

	public ServerIViewItemCollection(IGeminiRequestContext requestContext, string viewId)
		: this(requestContext, viewId, readAccessVerified: false)
	{
	}

	public ServerIViewItemCollection(IGeminiRequestContext requestContext, string viewId, bool readAccessVerified)
	{
		_requestContext = requestContext;
		_viewId = viewId;
		_readAccessVerified = readAccessVerified;
	}

	public IEnumerator<IViewItem> GetEnumerator()
	{
		SecurePopulate();
		return _viewItems.Values.GetEnumerator();
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}

	private void SecurePopulate()
	{
		if (_viewItems == null)
		{
			_viewItems = new Dictionary<string, IViewItem>(StringComparer.OrdinalIgnoreCase);
			ViewItem[] array = ((!_readAccessVerified) ? View.QueryViewItems(_requestContext, _viewId) : View.QueryViewItemsNoSecurity(_requestContext, _viewId));
			ViewItem[] array2 = array;
			foreach (ViewItem viewItem in array2)
			{
				_viewItems.Add(viewItem.Id, new ServerIViewItem(_requestContext, viewItem));
			}
		}
	}
}
