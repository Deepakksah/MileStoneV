using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewGroupLightCollection : List<IViewGroup>, IViewGroups, IEnumerable<IViewGroup>, IEnumerable
{
	public IViewGroup this[string index]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public IViewGroup this[Guid id]
	{
		get
		{
			throw new NotSupportedException();
		}
	}

	public bool Contains(string key)
	{
		throw new NotSupportedException();
	}
}
