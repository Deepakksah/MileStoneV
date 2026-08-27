using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewGroup : IViewGroup
{
	private IGeminiRequestContext _requestContext;

	private ServerGroup _serverGroup;

	private bool _readAccessVerified;

	private ServerICustomPropertyCollection _customProperies;

	private ServerIViewGroupCollection _childServerGroups;

	private ServerIViewCollection _childViews;

	public ICustomProperties CustomProperties
	{
		get
		{
			if (_customProperies == null)
			{
				_customProperies = new ServerICustomPropertyCollection(_requestContext, Id.ToString(), _readAccessVerified);
			}
			return _customProperies;
		}
	}

	public Guid Id => new Guid(_serverGroup.Id);

	public string Name
	{
		get
		{
			return _serverGroup.Name;
		}
		set
		{
			_serverGroup.Name = value;
		}
	}

	public IViewGroups ViewGroups
	{
		get
		{
			if (_childServerGroups == null)
			{
				_childServerGroups = new ServerIViewGroupCollection(_requestContext, Id.ToString(), _readAccessVerified);
			}
			return _childServerGroups;
		}
	}

	public IViews Views
	{
		get
		{
			if (_childViews == null)
			{
				_childViews = new ServerIViewCollection(_requestContext, Id.ToString(), _readAccessVerified);
			}
			return _childViews;
		}
	}

	public ServerIViewGroup(IGeminiRequestContext requestContext, ServerGroup serverGroup)
		: this(requestContext, serverGroup, readAccessVerified: false)
	{
	}

	public ServerIViewGroup(IGeminiRequestContext requestContext, ServerGroup serverGroup, bool readAccessVerified)
	{
		if (requestContext == null)
		{
			throw new ArgumentException("requestContext cannot be null", "requestContext");
		}
		_requestContext = requestContext;
		_serverGroup = serverGroup;
		_readAccessVerified = readAccessVerified;
	}

	public void Create()
	{
		string text = null;
		try
		{
			if (_serverGroup != null)
			{
				text = _serverGroup.Id;
			}
			ServerGroup.CreateServerGroup(_requestContext, _serverGroup);
		}
		catch
		{
			if (text != null)
			{
				_serverGroup.Id = text;
			}
		}
	}

	public void Delete()
	{
		ServerGroup.DeleteServerGroup(_requestContext, Id.ToString());
	}

	public ICustomProperty NewCustomProperty(string key, string value)
	{
		CustomProperty customProperty = new CustomProperty();
		customProperty.ObjectId = _serverGroup.Id;
		ICustomProperty customProperty2;
		try
		{
			customProperty2 = new ServerICustomProperty(_requestContext, customProperty);
			customProperty2.Key = key;
			customProperty2.Value = value;
		}
		catch
		{
			customProperty2 = null;
		}
		return customProperty2;
	}

	public IView NewView(string name, Guid id)
	{
		ViewOld viewOld = new ViewOld();
		viewOld.Id = id.ToString();
		viewOld.Name = name;
		viewOld.ViewOptions = new ViewOptionsOld
		{
			UserDefinedId = id.ToString(),
			ViewItems = new ViewItemOld[0]
		};
		ViewOld view = viewOld;
		try
		{
			return new ServerIView(_requestContext, _serverGroup.Id, view);
		}
		catch
		{
			return null;
		}
	}

	public IViewGroup NewViewGroup(string name)
	{
		ServerGroup serverGroup = new ServerGroup
		{
			Name = name,
			ParentId = _serverGroup.Id,
			StructureType = ServerCommandServiceViewHandling._viewGroupStructureType
		};
		try
		{
			return new ServerIViewGroup(_requestContext, serverGroup);
		}
		catch
		{
			return null;
		}
	}

	public IViewGroup NewViewGroup(string name, Guid id)
	{
		ServerGroup serverGroup = new ServerGroup
		{
			Name = name,
			ParentId = _serverGroup.Id,
			Id = id.ToString(),
			StructureType = ServerCommandServiceViewHandling._viewGroupStructureType
		};
		try
		{
			return new ServerIViewGroup(_requestContext, serverGroup);
		}
		catch
		{
			return null;
		}
	}

	public void Update()
	{
		try
		{
			ServerGroup.UpdateServerGroup(_requestContext, _serverGroup);
		}
		catch
		{
		}
	}
}
