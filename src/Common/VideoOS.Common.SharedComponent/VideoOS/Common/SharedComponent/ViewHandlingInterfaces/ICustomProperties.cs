using System.Collections;
using System.Collections.Generic;

namespace VideoOS.Common.SharedComponent.ViewHandlingInterfaces;

public interface ICustomProperties : IEnumerable<ICustomProperty>, IEnumerable
{
	ICustomProperty this[string index] { get; }

	bool Contains(string propertyName);
}
