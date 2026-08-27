using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IViewGroups : IEnumerable<IViewGroup>, IEnumerable
{
	IViewGroup this[string index] { get; }

	IViewGroup this[Guid id] { get; }

	bool Contains(string key);
}
