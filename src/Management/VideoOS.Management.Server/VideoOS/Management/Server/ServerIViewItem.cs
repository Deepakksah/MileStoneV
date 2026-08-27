using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewItem : IViewItem
{
	private IGeminiRequestContext _requestContext;

	private ViewItem _viewItem;

	public Guid Id => new Guid(_viewItem.Id);

	public string DefinitionXml
	{
		get
		{
			return _viewItem.DefinitionXml;
		}
		set
		{
			_viewItem.DefinitionXml = value;
		}
	}

	public int Position
	{
		get
		{
			return _viewItem.Position;
		}
		set
		{
			_viewItem.Position = value;
		}
	}

	public ServerIViewItem(IGeminiRequestContext requestContext, ViewItem viewItem)
	{
		_viewItem = viewItem;
		_requestContext = requestContext;
	}

	public void ChangeId(Guid id)
	{
		ViewItem.ChangeViewItemId(_requestContext, _viewItem.Id, id.ToString());
		_viewItem.Id = id.ToString();
	}

	public void Update()
	{
		ViewItem.UpdateViewItem(_requestContext, _viewItem);
	}
}
