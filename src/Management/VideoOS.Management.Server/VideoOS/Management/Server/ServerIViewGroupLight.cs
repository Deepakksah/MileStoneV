using System;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewGroupLight : IViewGroup
{
	public Guid Id { get; set; }

	public string Name { get; set; }

	public Guid ParentGroupId { get; set; }

	public IViewGroups ViewGroups { get; set; }

	public IViews Views { get; set; }

	public ICustomProperties CustomProperties { get; set; }

	public void Create()
	{
		throw new NotSupportedException();
	}

	public void Delete()
	{
		throw new NotSupportedException();
	}

	public IViewGroup NewViewGroup(string name, Guid id)
	{
		throw new NotSupportedException();
	}

	public void Update()
	{
		throw new NotSupportedException();
	}

	public ICustomProperty NewCustomProperty(string key, string value)
	{
		throw new NotSupportedException();
	}

	public IView NewView(string name, Guid id)
	{
		throw new NotSupportedException();
	}

	public IViewGroup NewViewGroup(string name)
	{
		throw new NotSupportedException();
	}
}
