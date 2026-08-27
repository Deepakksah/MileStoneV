using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewLightCollection : List<IView>, IViews, IEnumerable<IView>, IEnumerable
{
	public IView this[string viewName]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public IView this[Guid id]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public bool Contains(string viewName)
	{
		throw new NotSupportedException();
	}
}
