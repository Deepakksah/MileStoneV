using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

namespace VideoOS.Management.Server;

public class ServerICustomPropertyLightCollection : List<ICustomProperty>, ICustomProperties, IEnumerable<ICustomProperty>, IEnumerable
{
	public ICustomProperty this[string index] => this.First((ICustomProperty f) => f.Key.Equals(index, StringComparison.Ordinal));

	public bool Contains(string propertyName)
	{
		return this.Any((ICustomProperty f) => f.Key.Equals(propertyName, StringComparison.Ordinal));
	}
}
