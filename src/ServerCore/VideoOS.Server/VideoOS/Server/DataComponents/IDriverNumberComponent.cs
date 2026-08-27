using System;
using System.Collections.Generic;

namespace VideoOS.Server.DataComponents;

public interface IDriverNumberComponent : IDisposable
{
	IDictionary<Guid, int> UpdateDriverNumbers(IDictionary<Guid, int> parameter);
}
