using System;
using System.Collections;
using System.Collections.Generic;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerIViewItemLightCollection : List<IViewItem>, IViewItems, IEnumerable<IViewItem>, IEnumerable
{
	public IViewItem this[string index]
	{
		get
		{
			throw new NotSupportedException();
		}
	}
}
