using System;
using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IViews : IEnumerable<IView>, IEnumerable
{
	IView this[string viewName] { get; }

	IView this[Guid id] { get; }

	bool Contains(string viewName);
}
