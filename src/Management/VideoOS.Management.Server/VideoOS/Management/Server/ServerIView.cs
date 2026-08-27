using System;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIView : IView
{
	private IGeminiRequestContext _requestContext;

	private ViewOld _view;

	private string _viewGroupId;

	private bool _readAccessVerified;

	private ServerIViewItemCollection _viewItems;

	public string DefinitionXml
	{
		get
		{
			return _view.DefinitionXml;
		}
		set
		{
			_view.DefinitionXml = value;
		}
	}

	public Guid Id => new Guid(_view.Id);

	public string Name
	{
		get
		{
			return _view.Name;
		}
		set
		{
			_view.Name = value;
		}
	}

	public IViewItems ViewItems
	{
		get
		{
			if (_viewItems == null)
			{
				_viewItems = new ServerIViewItemCollection(_requestContext, _view.Id, _readAccessVerified);
			}
			return _viewItems;
		}
	}

	public ServerIView(IGeminiRequestContext requestContext, string viewGroupId, ViewOld view)
		: this(requestContext, viewGroupId, view, readAccessVerified: false)
	{
	}

	public ServerIView(IGeminiRequestContext requestContext, string viewGroupId, ViewOld view, bool readAccessVerified)
	{
		_requestContext = requestContext;
		_view = view;
		_viewGroupId = viewGroupId;
		_readAccessVerified = readAccessVerified;
	}

	public void AddViewItem(int position, Guid id, string definitionXml)
	{
		if (_view.ViewOptions == null)
		{
			_view.ViewOptions = new ViewOptionsOld();
			_view.ViewOptions.ViewItems = new ViewItemOld[0];
		}
		ViewItemOld item = new ViewItemOld
		{
			Id = id.ToString(),
			Position = position,
			DefinitionXml = definitionXml,
			ViewItemOptions = new ViewItemOldOptions
			{
				UserDefinedId = id.ToString()
			}
		};
		List<ViewItemOld> list = new List<ViewItemOld>(_view.ViewOptions.ViewItems);
		list.Add(item);
		_view.ViewOptions.ViewItems = list.ToArray();
	}

	public void Create()
	{
		ViewOld.CreateView(_requestContext, _viewGroupId, _view);
	}

	public void Delete()
	{
		ViewOld.DeleteView(_requestContext, _view.Id);
	}

	public void Update()
	{
		ViewOld.UpdateView(_requestContext, _view);
	}
}
