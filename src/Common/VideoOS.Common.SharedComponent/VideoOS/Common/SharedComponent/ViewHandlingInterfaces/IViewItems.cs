using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface IViewItems : IEnumerable<IViewItem>, IEnumerable
{
	IViewItem this[string index] { get; }
}
